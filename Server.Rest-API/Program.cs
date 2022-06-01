using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Runtime.CompilerServices;
using System.Text;
using Microsoft.Extensions.Configuration;
using Server.Rest_API.Common;
using Server.Rest_API.SqlServer;

namespace Server.Rest_API
{
    internal class Program
    {
        static void Main(string[] args)
        {
         
            //var conString = builder.GetConnectionString("DefaultDB");
            //Postgres connection = new Postgres(conString);
            Server server = new Server();
            server.Start();
        }
    }
}
