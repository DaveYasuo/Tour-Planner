using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Server.Rest_API.Controller
{
    public interface IController
    {
        string Get();
        string Get(object id);
        Task<string> Post(object body);
        void Patch(object body);
        void Delete(object id);

        // http://9090/api/Tour/1
        // http://9090/api/Tour/?id=1
        async Task<string> Handle(string httpMethod, Tuple<List<string>, Dictionary<string, string>> urlParams, string documentContents)
        {
            httpMethod = httpMethod.ToLower();

            string result = "";
            switch (httpMethod)
            {
                case "get":
                    {
                        // api/tour
                        result = urlParams.Item1.Count is 1 ? Get() : Get(urlParams.Item1[1]);;
                        break;
                    }
                case "post":
                    {
                        result = await Post(documentContents);
                        break;
                    }
                case "delete":
                    {
                        Delete(int.Parse(urlParams.Item1[1]));
                        break;
                    }
                case "patch":
                    {
                        Patch(documentContents);
                        break;
                    }
                default:
                    return result;
            };
            return result;
        }
    }
}
