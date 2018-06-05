using System;
using System.Collections.Generic;
using System.Text;

namespace SkyMallCore.ViewModel
{
    public class InvokeResult<TResult>
    {
        public bool Success { get; set; }

        public string Message { get; set; }

        public TResult Data { get; set; }


    }


    public class RquestResult
    {
        public static InvokeResult<TResult> Success<TResult>(TResult result)
        {
            return new InvokeResult<TResult>() { Success = true, Data = result };
        }

        public static InvokeResult<TResult> Failed<TResult>(string msg)
        {
            return new InvokeResult<TResult>() { Success = false, Message = msg};
        }

    }


}
