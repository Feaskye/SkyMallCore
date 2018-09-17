using Newtonsoft.Json;
using System.Net.Http;

namespace SkyMallCore.WebApi.Controllers
{
    /// <summary>
    /// ApiResult
    /// </summary>
    /// <typeparam name="T">类型</typeparam>
    public class ApiResult<T> :HttpResponseMessage
    {
        /// <summary>
        /// 初始化
        /// </summary>
        /// <param name="data"></param>
        /// <param name="success"></param>
        /// <param name="message"></param>
        public ApiResult(T data, bool success = true, string message = null)
        {
            this.Success = success;
            this.Data = data;
            this.Message = message;
        }

        /// <summary>
        /// 是否成功
        /// </summary>
        public bool Success { get; set; }

        /// <summary>
        /// 消息
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// 数据
        /// </summary>
        public T Data { get; set; }


    }
}