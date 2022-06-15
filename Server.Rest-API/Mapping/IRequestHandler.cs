using Server.Rest_API.Controller;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Rest_API.Mapping
{
    public interface IRequestHandler
    {
        public IController GetController(string controller);
        public Tuple<List<string>, Dictionary<string, string>> ParseUrl(string rawUrl);
    }
}
