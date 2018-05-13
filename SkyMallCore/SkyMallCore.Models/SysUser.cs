using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace SkyMallCore.Models
{
    [Table("Sys_User")]
    public class SysUser: ModelEntity
    {
        public string Id { get; set; }

        public string Account { get; set; }

        public string RealName { get; set; }

        public string NickName { get; set; }
    }


    public class ModelEntity { }


}
