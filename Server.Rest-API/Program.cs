using System.Threading.Tasks;

namespace Server.Rest_API
{
    internal class Program
    {
        private static async Task Main(string[] args)
        {

            Server server = new();
            await server.StartAsync();
        }
    }
}
