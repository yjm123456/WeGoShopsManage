using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Jd.Api.Request;
using Jd.Api;

namespace jos_sdk_net
{
    class Program
    {
        static void Main(string[] args)
        {
            int num = 9980980;

            string url = "https://api.jd.com/routerjson";
            string appkey = "";
            string appsecret = "";
            string token ="";
            IJdClient client = new DefaultJdClient(url, appkey, appsecret);

            
        }
    }
}
