using System.IO;

namespace SkyMallCore.WebApi.Models
{
    /// <summary>
    /// 文件上传参数
    /// </summary>
    public class FileUploadParam
    {
        public FileStream fileStreams { get; set; }

        public string Token { get; set; }
    }
}