using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using System.Threading;

namespace Server.Rest_API
{
    public class Server
    {
        private bool _listening;
        private readonly HttpListener _listener;


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
        }

        public void Start()
        {
            //netsh http add urlacl url = http://+:80/MyUri user=DOMAIN\user
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
            ServerHandler();
        }

        private void ServerHandler()
        {
            while (_listening)
            {
                // Note: The GetContext method blocks while waiting for a request.
                HttpListenerContext context = _listener.GetContext();

                HttpListenerRequest request = context.Request;

                string documentContents;
                using (Stream receiveStream = request.InputStream)
                {
                    using (StreamReader readStream = new StreamReader(receiveStream, Encoding.UTF8))
                    {
                        documentContents = readStream.ReadToEnd();
                    }
                }
                Console.WriteLine($"Received request for {request.Url}");

                Console.WriteLine($"Received request for {request}");

                // Handle this data
                Console.WriteLine(documentContents);

                // Obtain a response object.
                HttpListenerResponse response = context.Response;
                // Construct a response.
                string responseString = "<HTML><BODY> Hello world!</BODY></HTML>";
                byte[] buffer = System.Text.Encoding.UTF8.GetBytes(responseString);
                // Get a response stream and write the response to it.
                response.ContentLength64 = buffer.Length;
                System.IO.Stream output = response.OutputStream;
                output.Write(buffer, 0, buffer.Length);
                // You must close the output stream.
                output.Close();
            }
        }

        public void Stop()
        {
            _listening = false;
            _listener.Stop();
        }
    }

}
