using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace SkyMallCore.Models
{
    [Table("Member")]
    public class Member : ModelEntity
    {

        public string UserName { get; set; }
        public string Password { get; set; }
        
        public string RealName { get; set; }
        public string NickName { get; set; }
        public string HeadIcon { get; set; }
        public bool? Gender { get; set; }
        public DateTime? Birthday { get; set; }
        public string MobilePhone { get; set; }
        public string Email { get; set; }
        public string WeChat { get; set; }
        public string ManagerId { get; set; }
        public int? SecurityLevel { get; set; }
        
    }
}
