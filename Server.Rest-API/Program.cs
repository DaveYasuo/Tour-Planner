using System.Reflection;
using System.Threading.Tasks;
using log4net;

namespace Server.Rest_API
{
    internal class Program
    {
        private static readonly ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod()!.DeclaringType);
        private static async Task Main(string[] args)
        {
            Log.Info("Start Server application");
            Server server = new();
            await server.StartAsync();
        }
    }
}
