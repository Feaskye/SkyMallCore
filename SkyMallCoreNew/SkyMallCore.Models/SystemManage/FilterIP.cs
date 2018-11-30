
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;

namespace SkyMallCore.Models
{
    [Table("Sys_FilterIP")]
    public class FilterIP : ModelEntity
    {
        public bool? Type { get; set; }
        public string StartIP { get; set; }
        public string EndIP { get; set; }
    }
}
