using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace SkyMallCore.Models
{
    [Table("Sys_Items")]
    public class SysItems : ModelEntity
    {
        public string ParentId { get; set; }
        public string EnCode { get; set; }
        public string FullName { get; set; }
        public bool? IsTree { get; set; }
        public int? Layers { get; set; }
        public int? SortCode { get; set; }
    }
}
