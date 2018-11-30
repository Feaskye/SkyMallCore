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


    public class RequestResult
    {

        public static InvokeResult<bool> Result(bool result,string message = null)
        {
            if (result)
            {
                return Success(result);
            }
            return Failed<bool>(message);
        }


        public static InvokeResult<TResult> Success<TResult>(TResult result)
        {
            return new InvokeResult<TResult>() { Success = true, Data = result };
        }

        public static InvokeResult<TResult> Failed<TResult>(string msg)
        {
            return new InvokeResult<TResult>() { Success = false, Message = msg};
        }

    }




    public class ListItem
    {
        public string Code { get; set; }
        
        public string Text { get; set; }
        public bool Selected { get; set; }

        public string ParentId { get; set; }

        public string PicUrl { get; set; }

        public string Type { get; set; }

        public List<ListItem> Childs { get; set; }
    }





















}
