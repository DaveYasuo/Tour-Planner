using System.Threading.Tasks;

namespace Server.Rest_API
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
         
            //var conString = builder.GetConnectionString("DefaultDB");
            //Postgres connection = new Postgres(conString);
            Server server = new Server();
            await server.StartAsync();
        }
    }
}
