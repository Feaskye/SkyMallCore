using System.IO;

namespace SkyMallCore.WebApi.Controllers
{

    public class FileUploadParam
    {
        public FileStream fileStreams { get; set; }

        public string Token { get; set; }
    }
}