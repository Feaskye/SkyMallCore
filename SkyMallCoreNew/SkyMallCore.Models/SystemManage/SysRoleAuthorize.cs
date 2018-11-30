
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace SkyMallCore.Models
{
    [Table("Sys_RoleAuthorize")]
    public class SysRoleAuthorize : CreatorEntity
    {
        public int? ItemType { get; set; }
        public string ItemId { get; set; }
        public int? ObjectType { get; set; }
        public string ObjectId { get; set; }

    }
}
