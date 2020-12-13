using EPS.Administration.APIAccess.Models.Exceptions;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace EPS.Administration.APIAccess.Services
{
    internal class RequestHandler : BaseService
    {
        public static async Task<RESPONSE> ProcessPostRequest<RESPONSE, PACKAGE>(string request, PACKAGE package, string token = null) where RESPONSE : class where PACKAGE : class
        {
            RESPONSE result = default(RESPONSE);
            try
            {
                HttpClient httpClient = string.IsNullOrEmpty(token) ? GetClient() : GetClient(token);
                HttpContent content = new StringContent(ObjectToJson(package), Encoding.UTF8, "application/json");

                var response = await httpClient.PostAsync(request, content);
                if (response.StatusCode != HttpStatusCode.OK)
                {
                    throw new ServiceException($"Status code '{response.StatusCode}':'{(int)response.StatusCode}' | {request} Failed to service address:'{httpClient.BaseAddress}'");
                }

                string apiResponse = await response.Content.ReadAsStringAsync();
                result = JsonMessageHandler<RESPONSE>(apiResponse);

                response.Dispose();
            }
            catch (Exception ex)
            {
                throw new ServiceException(ex.Message);
            }

            return result;
        }

        public static async Task<RESPONSE> ProcessPostFileRequest<RESPONSE, FILE>(string request, FILE package, string fileName, string token = null) where RESPONSE : class where FILE : FileStream
        {
            RESPONSE result = default(RESPONSE);
            try
            {
                HttpClient httpClient = string.IsNullOrEmpty(token) ? GetClient() : GetClient(token);

                var content = new MultipartFormDataContent();
                content.Add(new StreamContent(package), "file", fileName);

                var response = await httpClient.PostAsync(request, content);
                if (response.StatusCode != HttpStatusCode.OK)
                {
                    throw new ServiceException($"Status code '{response.StatusCode}':'{(int)response.StatusCode}' | {request} Failed to service address:'{httpClient.BaseAddress}'");
                }

                string apiResponse = await response.Content.ReadAsStringAsync();
                result = JsonMessageHandler<RESPONSE>(apiResponse);

                response.Dispose();
            }
            catch (Exception ex)
            {
                throw new ServiceException(ex.Message);
            }

            return result;
        }

        public static async Task<RESPONSE> ProcessGetRequest<RESPONSE>(string request, string token = null) where RESPONSE : class
        {
            RESPONSE result = default(RESPONSE);
            try
            {
                HttpClient httpClient = string.IsNullOrEmpty(token) ? GetClient() : GetClient(token);

                var response = await httpClient.GetAsync(request);
                if (response.StatusCode != HttpStatusCode.OK)
                {
                    throw new ServiceException($"Status code '{response.StatusCode}':'{(int)response.StatusCode}' | {request} Failed to service address:'{httpClient.BaseAddress}'");
                }

                string apiResponse = await response.Content.ReadAsStringAsync();
                result = JsonMessageHandler<RESPONSE>(apiResponse);

                response.Dispose();
            }
            catch (Exception ex)
            {
                throw new ServiceException(ex.Message);
            }

            return result;
        }

        private static string ObjectToJson<G>(G obj)
        {
            if (obj == null)
            {
                return string.Empty;
            }

            return JsonConvert.SerializeObject(obj);
        }

        private static T JsonMessageHandler<T>(string response)
        {
            return JsonConvert.DeserializeObject<T>(response);
        }
    }
}