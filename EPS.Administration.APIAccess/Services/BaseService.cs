using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;

namespace EPS.Administration.APIAccess.Services
{
    public class BaseService
    {
        public static HttpClient GetClient()
        {
            return GetClient(string.Empty);
        }

        public static HttpClient GetClient(string authenticationToken)
        {
            var client = new HttpClient();
            client.BaseAddress = new Uri("http://localhost:13377/");
            client.DefaultRequestHeaders.Add("ContentType", "application/json");
            
            if (!string.IsNullOrEmpty(authenticationToken))
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", authenticationToken);
            }

            return client;
        }
    }
}