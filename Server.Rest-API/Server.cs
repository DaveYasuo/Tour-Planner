using log4net;
using Server.Rest_API.Controller;
using Server.Rest_API.Mapping;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Server.Rest_API
{
    public class Server
    {
        private bool _listening;
        private readonly HttpListener _listener;
        private readonly IRequestHandler _handler;
        private CancellationTokenSource _tokenSource;
        private readonly ConcurrentDictionary<string, Task> _tasks = new();
        private static readonly ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod()!.DeclaringType);

        public Server()
        {
            if (!HttpListener.IsSupported)
            {
                Log.Error("Windows XP SP2 or Server 2003 is required to use the HttpListener class.");
                return;
            }
            // URI prefixes are required,
            var prefixes = new List<string>() { "http://localhost:8888/" };

            // Create a listener.
            _listener = new HttpListener();
            // Add the prefixes.
            foreach (var s in prefixes)
            {
                _listener.Prefixes.Add(s);
            }
            _handler = new RequestHandler();
        }

        public async Task StartAsync()
        {
            _tokenSource = new CancellationTokenSource();
            if (_listening) return;
            _listening = true;
            _listener.Start();
            Log.Info("Server started: Listening...");
            Console.CancelKeyPress += (_, e) =>
            {
                Log.Info($"Main Server closed by: {e.SpecialKey}.");
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
                    // ignored
                }
            }
        }

        private async Task ProcessRequestAsync(HttpListenerContext context)
        {

            HttpListenerRequest request = context.Request;

            string documentContents;
            await using (Stream receiveStream = request.InputStream)
            {
                using StreamReader readStream = new(receiveStream, Encoding.UTF8);
                documentContents = await readStream.ReadToEndAsync();
            }
            Log.Info($"Received request for {request.Url}");

            Tuple<List<string>, Dictionary<string, string>> urlParams = _handler.ParseUrl(request.RawUrl);
            // Obtain a response object.
            HttpListenerResponse response = context.Response;
            string responseString = null;
            if (urlParams is not null && urlParams.Item1.Count != 0)
            {
                IController controller = _handler.GetController(urlParams.Item1[0]);
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
            await output.WriteAsync(buffer, 0, buffer.Length);
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
