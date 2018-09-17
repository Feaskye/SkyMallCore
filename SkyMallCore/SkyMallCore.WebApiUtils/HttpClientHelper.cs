using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace SkyMallCore.WebApiUtils
{
    /// <summary>
    /// HttpClient客户端请求帮助类
    /// </summary>
    public class HttpClientHelper
    {
        private IHttpClientFactory _httpClientFactory;
        private string _baseUri;

        public HttpClientHelper(IHttpClientFactory httpClientFactory, string baseUri)
        {
            //IHttpClientFactory解决HttpClient缺陷详解：https://www.cnblogs.com/willick/p/9640589.html
            _httpClientFactory = httpClientFactory;
            _baseUri = baseUri;
        }

        private HttpClient GetHttpClient(DataType dataType)
        {
            var httpClient = _httpClientFactory.CreateClient();
            httpClient.BaseAddress = new Uri(_baseUri);

            //data type
            var header = new MediaTypeWithQualityHeaderValue("application/json");
            if (dataType == DataType.Xml)
            {
                header = new MediaTypeWithQualityHeaderValue("application/xml");
            }
            httpClient.DefaultRequestHeaders.Accept.Add(header);

            return httpClient;
        }



        public async Task<TResult> Get<TResult>(string requestUri, DataType dataType = DataType.Json) where TResult : class
        {
            TResult result = default(TResult);
            HttpResponseMessage response = GetHttpClient(dataType).GetAsync(requestUri).Result;

            if (response.IsSuccessStatusCode)
            {
                var s = response.Content.ReadAsStringAsync().Result;
                result = s.ToObject<TResult>();
            }
            return result;
        }


        /// <summary>
        /// 发起post请求
        /// </summary>
        /// <typeparam name="TRequest"></typeparam>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="url"></param>
        /// <param name="requestModel"></param>
        /// <param name="dataType"></param>
        /// <returns></returns>
        public TResult PostResponse<TRequest, TResult>(string url, TRequest requestModel,DataType dataType = DataType.Json)
           where TResult : class, new()
        {
            HttpContent httpContent = new StringContent(requestModel.ToJson());
            httpContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            var httpClient = GetHttpClient(dataType);

            TResult result = default(TResult);

            HttpResponseMessage response = httpClient.PostAsync(url, httpContent).Result;

            if (response.IsSuccessStatusCode)
            {
                var s = response.Content.ReadAsStringAsync().Result;
                result = s.ToObject<TResult>();
            }
            return result;
        }


    }




    /// <summary>
    /// 数据类型
    /// </summary>
    public enum DataType
    {
        /// <summary>
        /// json
        /// </summary>
        Json,
        /// <summary>
        /// xml
        /// </summary>
        Xml
    }

}
