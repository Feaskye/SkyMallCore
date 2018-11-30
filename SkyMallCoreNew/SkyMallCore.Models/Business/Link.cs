using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace SkyMallCore.Models
{
    [Table("Link")]
    public class Link : ModelEntity
    {
        public string LinkName { get; set; }

        public string LinkUrl { get; set; }
    }
}
