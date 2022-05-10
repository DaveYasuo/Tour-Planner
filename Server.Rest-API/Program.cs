using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Runtime.CompilerServices;
using System.Text;

namespace Server.Rest_API
{
    internal class Program
    {
        static void Main(string[] args)
        {
            DatabaseConnection connection = new DatabaseConnection();
            Server server = new Server();
            server.Start();
        }
    }
}
