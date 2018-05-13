using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace SkyMallCore.Models
{
    [Table("Sys_Area")]
    public class SysArea : ModelEntity
    {
        public string ParentId { get; set; }
        public int? Layers { get; set; }
        public string EnCode { get; set; }
        public string FullName { get; set; }
        public string SimpleSpelling { get; set; }
        public int? SortCode { get; set; }

    }
}
