using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Rest_API.Controller
{
    public interface IController
    {
        string Get();
        string Get(object id);
        string Post(string body);
        void Put(object id);
        void Delete(object id);

        // http://9090/api/Tour/1
        // http://9090/api/Tour/?id=1
        string Handle(string httpMethod, Tuple<List<string>, Dictionary<string, string>> urlParams, string documentContents)
        {
            httpMethod = httpMethod.ToLower();

            string result = "";
            switch (httpMethod)
            {
                case "get":
                    {
                        if (urlParams.Item1.Count is 1)
                        {
                            result = Get();
                        }
                        else
                        {
                            result = Get(urlParams);
                        };
                        break;
                    }
                case "post":
                    {
                        result = Post(documentContents);
                        break;
                    }
                case "delete":
                    {
                        Delete(urlParams);
                        break;
                    }
                case "put":
                    {
                        Put(urlParams);
                        break;
                    }
                default:
                    return result;
            };
            return result;
        }
    }
}
