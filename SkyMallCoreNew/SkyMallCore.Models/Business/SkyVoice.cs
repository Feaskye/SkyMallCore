using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace SkyMallCore.Models
{
    [DisplayName("客户分组")]
    [Table("SkyVoice")]
    public class SkyVoice : ModelEntity
    {
        /// <summary>
        /// -T拨入拨出号码（电话号码，一般是座机号码或者手机号等等，也可能是空）
        /// </summary>
        public string PhoneNumber { get; set; }

        /// <summary>
        /// 方向：拨入I拨出O手动M方向
        /// </summary>
        public string Direction { get; set; }
        /// <summary>
        /// 挂断
        /// </summary>
        public string CutLine { get; set; }

        /// <summary>
        /// 通话时长（秒） 录音总共有多少秒
        /// </summary>
        public int LineTime { get; set; }
        
        /// <summary>
        /// 文件
        /// </summary>
        public string VoiceFile { get; set; }
        
        public string Type { get; set; }

        /// <summary>
        /// 线路号
        /// </summary>
        public string LineNumber { get; set; }

        //笔记和备注 默认是N，可数字英文和汉字，位数不定


        /// <summary>
        /// -P本地话务人员姓名（默认是P，也可能变成客户设置的张三、李四等）
        /// </summary>
        public string LocalOperator { get; set; }

        /// <summary>
        ///文件日期
        /// </summary>
        public DateTime FileDate { get; set; }

        /// <summary>
        /// S/R保留字段
        /// </summary>
        public string SRField { get; set; }

    }

    /// <summary>
    /// 方向
    /// </summary>
    public enum DirectionType
    {
        /// <summary>
        /// 拨入
        /// </summary>
        [Display(Name = "拨入")]
        I,
        /// <summary>
        /// 拨出
        /// </summary>
        [Display(Name = "拨出")]
        O,
        /// <summary>
        /// 手动
        /// </summary>
        [Display(Name = "手动")]
        M
    }


}
