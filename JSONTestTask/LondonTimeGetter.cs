using System.IO;
using Newtonsoft.Json;
using System.Net;

namespace JSONTestTask
{
    public class LondonTimeGetter
    {
        public TimeInfo GetTime()
        {
            var req = WebRequest.Create("http://worldtimeapi.org/api/timezone/Europe/London");
            req.Method = "GET";
            req.Timeout = 5000;

            var res = req.GetResponse() as HttpWebResponse;
            var resStream = res.GetResponseStream();
            var streamReader = new StreamReader(resStream);

            return new JsonSerializer().Deserialize(streamReader, typeof(TimeInfo)) as TimeInfo;
        }
    }
}
