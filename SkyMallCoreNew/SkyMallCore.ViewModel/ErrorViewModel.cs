using System;
using System.Collections.Generic;
using System.Text;

namespace SkyMallCore.ViewModel
{
    public class ErrorViewModel
    {
        public string RequestId { get; set; }

        public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);


        public string Message { get; set; }
    }


    public enum AuthCodeEnum
    {
        SysLogin = 0,
        MemLogin = 1,
        ChangeEmail = 2
    }




    public abstract class DetailView
    {
        public string Id { get; set; }

        public DateTime? CreatorTime { get; set; }

        public int? SortCode { get; set; }

        public bool? EnabledMark { get; set; }

    }



}
