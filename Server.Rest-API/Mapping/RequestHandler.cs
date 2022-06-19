using Server.Rest_API.Controller;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Server.Rest_API.Mapping
{
    public class RequestHandler : IRequestHandler
    {
        private readonly IController _tourController;
        private readonly IController _tourLogController;
        public RequestHandler()
        {
            _tourController = new TourController();
            _tourLogController = new TourLogController();
        }

        public IController GetController(string controller)
        {
            if (!Enum.TryParse(controller, out EndPoint endPoint)) return null;

            return endPoint switch
            {
                EndPoint.Tour => _tourController,
                EndPoint.TourLog => _tourLogController,
                _ => null
            };
        }

        public Tuple<List<string>, Dictionary<string, string>> ParseUrl(string rawUrl)
        {
            string check = "/api/";
            if (!rawUrl.StartsWith(check)) return null;

            rawUrl = rawUrl[check.Length..];
            List<string> urlParam = rawUrl.Split('/', StringSplitOptions.RemoveEmptyEntries).ToList();
            Dictionary<string, string> param = null;
            if (urlParam.Count != 0 && urlParam.Last().Contains('?'))
            { //http://qer.qwer/api/Tour?key=value&key1=value1
                param = new Dictionary<string, string>();
                var entries = urlParam.Last().Split('&');
                foreach (var entry in entries)
                {
                    var tmp = entry.Split('=');
                    if (tmp.Length != 2) continue;
                    var key = tmp[0];
                    var val = tmp[1];
                    param.Add(key, val);
                }
            }
            var result = new Tuple<List<string>, Dictionary<string, string>>(urlParam, param);
            return result;
        }
    }
}
