using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using log4net;
using Server.Rest_API.Controller;
using Server.Rest_API.Mapping;
using Server.Rest_API.SqlServer;

namespace Server.Rest_API
{
    public class Server
    {
        private bool _listening;
        private readonly HttpListener _listener;
        private readonly IRequestHandler handler;
        private CancellationTokenSource _tokenSource;
        private readonly ConcurrentDictionary<string, Task> _tasks = new();
        private static readonly ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod()!.DeclaringType);

        public Server()
        {
            if (!HttpListener.IsSupported)
            {
                Console.WriteLine("Windows XP SP2 or Server 2003 is required to use the HttpListener class.");
                return;
            }
            // URI prefixes are required,
            var prefixes = new List<string>() { "http://localhost:8888/" };

            // Create a listener.
            _listener = new HttpListener();
            // Add the prefixes.
            foreach (string s in prefixes)
            {
                _listener.Prefixes.Add(s);
            }
            handler = new RequestHandler();
        }

        public async Task StartAsync()
        {
            _tokenSource = new CancellationTokenSource();
            if (_listening) return;
            _listening = true;
            _listener.Start();
            Console.WriteLine("Listening...");
            Console.CancelKeyPress += (_, e) =>
            {
                Console.WriteLine($"Main Server closed by: {e.SpecialKey}.");
                Stop();
                Environment.Exit(0);
            };
            await ServerHandler();
        }

        private async Task ServerHandler()
        {
            while (_listening)
            {
                try
                {
                    var token = _tokenSource.Token;
                    var id = Guid.NewGuid().ToString();
                    var context = await _listener.GetContextAsync();
                    var task = Task.Run(() => ProcessRequestAsync(context), token);
                    _ = task.ContinueWith(t =>
                    {
                        if (t == null) return;
                        _tasks.TryRemove(id, out t);
                    }, token);
                }
                catch (Exception)
                {

                }
            }
        }

        private async Task ProcessRequestAsync(HttpListenerContext context)
        {

            HttpListenerRequest request = context.Request;

            string documentContents;
            using (Stream receiveStream = request.InputStream)
            {
                using StreamReader readStream = new(receiveStream, Encoding.UTF8);
                documentContents = readStream.ReadToEnd();
            }
            Console.WriteLine($"Received request for {request.Url}");
            Console.WriteLine($"Received request for {request.HttpMethod}"); // post
            Console.WriteLine($"Received request for {request.RawUrl}"); //api/Tour

            Tuple<List<string>, Dictionary<string, string>> urlParams = handler.ParseUrl(request.RawUrl);
            // Obtain a response object.
            HttpListenerResponse response = context.Response;
            string responseString = null;
            if (urlParams is not null && urlParams.Item1.Count != 0)
            {
                IController controller = handler.GetController(urlParams.Item1[0]);
                if (controller is not null) responseString = await controller.Handle(request.HttpMethod, urlParams, documentContents);
            }
            // Get a response stream and write the response to it.
            Stream output = response.OutputStream;
            byte[] buffer;
            if (responseString is null)
            {
                response.StatusCode = (int)HttpStatusCode.BadRequest;
                buffer = Encoding.UTF8.GetBytes("");
            }
            else
            {
                buffer = Encoding.UTF8.GetBytes(responseString);
            }
            response.ContentLength64 = buffer.Length;
            output.Write(buffer, 0, buffer.Length);
            // You must close the output stream.
            output.Close();
        }

        public void Stop()
        {
            if (_listening == false) return;
            // Stop listening
            _listening = false;
            // Cleanup tasks
            _tokenSource.Cancel();
            foreach (var task in _tasks.Values)
            {
                if (task.IsCompleted) continue;
                try
                {
                    Log.Info(task.Wait(500) ? "Task completed" : "Task failed.");
                }
                catch (Exception e)
                {
                    // Prevent TaskCanceledException
                    Log.Warn("Waiting for Tasks to finish" + e.Message);
                }
            }

            _tokenSource.Dispose();
            _tasks.Clear();
            _listener.Stop();
        }
    }

}
