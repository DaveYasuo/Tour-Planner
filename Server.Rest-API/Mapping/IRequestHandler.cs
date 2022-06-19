using Server.Rest_API.Controller;
using System;
using System.Collections.Generic;

namespace Server.Rest_API.Mapping
{
    public interface IRequestHandler
    {
        public IController GetController(string controller);
        public Tuple<List<string>, Dictionary<string, string>> ParseUrl(string rawUrl);
    }
}
