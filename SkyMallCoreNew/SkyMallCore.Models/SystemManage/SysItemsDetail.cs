
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace SkyMallCore.Models
{
    [Table("Sys_ItemsDetail")]
    public class SysItemsDetail : ModelEntity
    {
        public string ItemId { get; set; }
        public string ParentId { get; set; }

        [ForeignKey("ItemId")]
        public virtual SysItems SysItems { get; set; }


        public string ItemCode { get; set; }
        public string ItemName { get; set; }
        public string SimpleSpelling { get; set; }
        public bool? IsDefault { get; set; }
        public int? Layers { get; set; }

    }
}
