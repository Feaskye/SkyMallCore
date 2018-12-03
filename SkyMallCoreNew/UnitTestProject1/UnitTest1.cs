using Microsoft.VisualStudio.TestTools.UnitTesting;
using SkyCoreLib.Utils;

namespace UnitTestProject1
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
        {
            var file = @"D:\temp\∞À°¢∆∑÷ øÿ÷∆.zip";//@"D:\SkyeSpace\wwwnet\ClientPorjects\SkyMall\branches\SkyOilWeb\web\SkyMallCoreWeb\wwwroot\UploadFiles\20181127\e1194fc79236417c9c90b3b3688a00bc.zip";
            var unzipDir = FileDownHelper.UnZip(file, file.Replace(".zip", "topic"));


        }
    }
}
