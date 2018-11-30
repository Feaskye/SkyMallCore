using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace SkyMallCore.Models
{
    [DisplayName("系统配置")]
    [Table("Sys_Configuration")]
    public class SysConfiguration : ModelEntity
    {
        public string ConfigName { get; set; }

        public string ConfigValue { get; set; }
        public string ConfigCode { get; set; }
    }
}
