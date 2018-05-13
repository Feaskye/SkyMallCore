using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace SkyMallCore.Models
{
    [Table("Sys_ModuleButton")]
    public class SysModuleButton : ModelEntity
    {
        public string ModuleId { get; set; }
        public string ParentId { get; set; }
        public int? Layers { get; set; }
        public string EnCode { get; set; }
        public string FullName { get; set; }
        public string Icon { get; set; }
        public int? Location { get; set; }
        public string JsEvent { get; set; }
        public string UrlAddress { get; set; }
        public bool? Split { get; set; }
        public bool? IsPublic { get; set; }
        public bool? AllowEdit { get; set; }
        public bool? AllowDelete { get; set; }
        public int? SortCode { get; set; }
    }
}
