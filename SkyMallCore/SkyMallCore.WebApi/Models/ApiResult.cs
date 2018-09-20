using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Newtonsoft.Json;
using System.Net.Http;

namespace SkyMallCore.WebApi.Controllers
{
    /// <summary>
    /// ApiResult 继承自IConvertToActionResult
    /// 详解参考ActionResult<T>
    /// </summary>
    /// <typeparam name="T">类型</typeparam>
    public class ApiResult<T> : IConvertToActionResult
    {
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
        /// result
        /// </summary>
        /// <param name="result"></param>
        public ApiResult(ActionResult result)
        {
            Result = result;
        }


        private ActionResult Result { get; }

        IActionResult IConvertToActionResult.Convert()
        {
            return Result ?? new ObjectResult(new { Success, Data, Message })
            {
                DeclaredType = typeof(T),
            };
        }

        /// <summary>
        /// 实现了两个完成大部分工作的隐式运算符
        /// </summary>
        /// <param name="value"></param>
        public static implicit operator ApiResult<T>(T value)
        {
            return new ApiResult<T>(value);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="result"></param>
        public static implicit operator ApiResult<T>(ActionResult result)
        {
            return new ApiResult<T>(result);
        }


    }
    


}