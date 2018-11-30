using System;
using System.Collections.Generic;
using System.Text;

namespace SkyMallCore.ViewModel
{
    public class FileUploadModel
    {
        /// <summary>
        /// 控件Id，必填
        /// </summary>
        public string Id { get; set; }

        public string RequestUrl { get; set; }

        public bool ShowMultiple { get; set; } = false;

        public string ControlId { get; set; }

        /// <summary>
        /// 选完文件后，是否自动上传。
        /// </summary>
        public bool AutoUpload { get; set; }

        /// <summary>
        /// 获取详细信息
        /// </summary>
        public bool HasUploadDetail { get; set; }


        public UpLoadAction? ActionType { get; set; }


        /// <summary>
        /// 特殊处理文件夹名
        /// </summary>
        public string SpecilName { get; set; }

    }


    /// <summary>
    /// 已上传文件信息
    /// </summary>
    public class UploadedFileModel
    {
        public string FileName { get; set; }

        public string Name { get; set; }

        public string FileType { get; set; }

        public long FileSize { get; set; }

        /// <summary>
        /// 格式化生成之后的img文件
        /// </summary>
        public List<UploadedFileModel> ImgFiles { get; set; }
    }




    /// <summary>
    /// 上传事件类型
    /// </summary>
    public enum UpLoadAction
    {
        /// <summary>
        /// 图
        /// </summary>
        cover = 0,
        /// <summary>
        /// 资源包
        /// </summary>
        package =1,
        /// <summary>
        /// 文库资源
        /// </summary>
        attichfile = 2
    }










}
