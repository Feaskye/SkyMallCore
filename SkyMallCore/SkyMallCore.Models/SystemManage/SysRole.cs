
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace SkyMallCore.Models
{
    [Table("Sys_Role")]
    public class SysRole : ModelEntity
    {
        public string OrganizeId { get; set; }
        public int? Category { get; set; }
        public string EnCode { get; set; }
        public string FullName { get; set; }
        public string Type { get; set; }
        public bool? AllowEdit { get; set; }
        public bool? AllowDelete { get; set; }
        public int? SortCode { get; set; }
    }
}
