using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace SkyMallCore.Models
{
    /// <summary>
    /// 登录日志表
    /// </summary>
    [Table("Sys_Log")]
    public class SysLog:CreatorEntity
    {
        public DateTime? Date { get; set; }
        public string Account { get; set; }
        public string NickName { get; set; }
        public string Type { get; set; }
        public string IPAddress { get; set; }
        public string IPAddressName { get; set; }
        public string ModuleId { get; set; }
        public string ModuleName { get; set; }
        public bool? Result { get; set; }
        public string Description { get; set; }
    }


    public enum DbLogType
    {
        [Description("其他")]
        Other = 0,
        [Description("登录")]
        Login = 1,
        [Description("退出")]
        Exit = 2,
        [Description("访问")]
        Visit = 3,
        [Description("新增")]
        Create = 4,
        [Description("删除")]
        Delete = 5,
        [Description("修改")]
        Update = 6,
        [Description("提交")]
        Submit = 7,
        [Description("异常")]
        Exception = 8,
    }

}
