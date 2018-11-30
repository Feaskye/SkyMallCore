using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace SkyMallCore.Models
{
    [Table("Sys_DbBackup")]
    public class DbBackup:ModelEntity
    {
        public string BackupType { get; set; }
        public string DbName { get; set; }
        public string FileName { get; set; }
        public string FileSize { get; set; }
        public string FilePath { get; set; }
        public DateTime? BackupTime { get; set; }
    }
}
