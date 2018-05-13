
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace SkyMallCore.Models
{
    [Table("Sys_UserLogOn")]
    public class SysUserLogOn : KeyEntity
    {
        public string UserId { get; set; }
        public string UserPassword { get; set; }
        public string UserSecretkey { get; set; }
        public DateTime? AllowStartTime { get; set; }
        public DateTime? AllowEndTime { get; set; }
        public DateTime? LockStartDate { get; set; }
        public DateTime? LockEndDate { get; set; }
        public DateTime? FirstVisitTime { get; set; }
        public DateTime? PreviousVisitTime { get; set; }
        public DateTime? LastVisitTime { get; set; }
        public DateTime? ChangePasswordDate { get; set; }
        public bool? MultiUserLogin { get; set; }
        public int? LogOnCount { get; set; }
        public bool? UserOnLine { get; set; }
        public string Question { get; set; }
        public string AnswerQuestion { get; set; }
        public bool? CheckIPAddress { get; set; }
        public string Language { get; set; }
        public string Theme { get; set; }
    }
}
