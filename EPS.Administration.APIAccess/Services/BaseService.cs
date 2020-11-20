using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;

namespace EPS.Administration.APIAccess.Services
{
    public class BaseService
    {
        private static HttpClient _httpClient;

        public static HttpClient Client
        {
            get 
            { 
                if(_httpClient == null)
                {
                    _httpClient = new HttpClient();
                    _httpClient.BaseAddress = new Uri("http://localhost:13377/");
                }

                return _httpClient;
            }
        }


        public static HttpClient GetClient(HttpContentHeaders headers = null)
        {
            if (headers == null || !headers.Contains("ContentType"))
            {
                Client.DefaultRequestHeaders.Add("ContentType", "application/json");
            }
            
            if (headers != null)
            {
                foreach(var header in headers)
                {
                    Client.DefaultRequestHeaders.Add(header.Key, header.Value);
                }
            }

            if(Client.DefaultRequestHeaders.Contains("BAuth"))
            {
                Client.DefaultRequestHeaders.Authorization = null;
            }

            return Client;
        }

        public static HttpClient GetClient(string authenticationToken, HttpContentHeaders headers = null)
        {
            if (headers == null || !headers.Contains("ContentType"))
            {
                Client.DefaultRequestHeaders.Add("ContentType", "application/json");
            }

            if (headers != null)
            {
                foreach (var header in headers)
                {
                    Client.DefaultRequestHeaders.Add(header.Key, header.Value);
                }
            }

            if (!Client.DefaultRequestHeaders.Contains("BAuth"))
            {
                Client.DefaultRequestHeaders.Add("BAuth", "Basic " + authenticationToken);
            }

            return Client;
        }
    }
}