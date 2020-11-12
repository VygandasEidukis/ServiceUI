using System;
using System.Net.Http;

namespace EPS.Administration.APIAccess.Services
{
    public class BaseService
    {
        private static HttpClient _client { get; set; }
        public static HttpClient Client
        {
            get
            {
                if (_client == null)
                {
                    _client = new HttpClient();
                    _client.BaseAddress = new Uri("");
                }
                return _client;
            }
        }

        public BaseService()
        {

        }
    }
}
