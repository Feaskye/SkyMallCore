using System;
using System.Collections.Generic;
using System.Text;

namespace SkyMallCore.Models
{
    /// <summary>
    /// 系统管理Model基类
    /// </summary>
    public class ModelEntity: CreatorEntity
    {
        public bool? DeleteMark { get; set; }
        public bool? EnabledMark { get; set; }
        public string Description { get; set; }

        public DateTime? LastModifyTime { get; set; }
        public string LastModifyUserId { get; set; }
        public DateTime? DeleteTime { get; set; }
        public string DeleteUserId { get; set; }
    }


    public class CreatorEntity: KeyEntity
    {
        public DateTime? CreatorTime { get; set; }
        public string CreatorUserId { get; set; }
    }

    public class KeyEntity
    {
        public virtual string Id { get; set; }
    }


    }
