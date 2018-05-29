
using Microsoft.AspNetCore.Http;
using System;
using System.IO;
using System.Threading;
using System.Web;

namespace SkyMallCore.Core
{
    public class FileDownHelper
    {
        public FileDownHelper()
        { }
        
        public static string FileNameExtension(string FileName)
        {
            return Path.GetExtension(MapPathFile(FileName));
        }
        public static string MapPathFile(string FileName)
        {
            return FileHelper.MapPath(FileName);
        }
        public static bool FileExists(string FileName)
        {
            string destFileName = FileName;
            if (File.Exists(destFileName))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public static void DownLoadold(string FileName, string name)
        {
            string destFileName = FileName;
            if (File.Exists(destFileName))
            {
                FileInfo fi = new FileInfo(destFileName);
                CoreProviderContext.HttpContext.Response.Clear();
                //CoreProviderContext.HttpContext.Response.ClearHeaders();
                //CoreProviderContext.HttpContext.Response.Buffer = false;
                //CoreProviderContext.HttpContext.Response.AppendHeader("Content-Disposition", "attachment;filename=" + HttpUtility.UrlEncode(name, System.Text.Encoding.UTF8));
                //CoreProviderContext.HttpContext.Response.AppendHeader("Content-Length", fi.Length.ToString());
                CoreProviderContext.HttpContext.Response.ContentType = "application/octet-stream";
                //CoreProviderContext.HttpContext.Response.WriteFile(destFileName);
                //CoreProviderContext.HttpContext.Response.Flush();
                //CoreProviderContext.HttpContext.Response.End();
            }
        }
        public static void DownLoad(string FileName)
        {
            string filePath = MapPathFile(FileName);
            long chunkSize = 204800;             //指定块大小 
            byte[] buffer = new byte[chunkSize]; //建立一个200K的缓冲区 
            long dataToRead = 0;                 //已读的字节数   
            FileStream stream = null;
            try
            {
                //打开文件   
                stream = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.Read);
                dataToRead = stream.Length;

                //添加Http头   
                CoreProviderContext.HttpContext.Response.ContentType = "application/octet-stream";
                CoreProviderContext.HttpContext.Response.Headers.Add("Content-Disposition", "attachement;filename=" + HttpUtility.UrlEncode(Path.GetFileName(filePath)));
                CoreProviderContext.HttpContext.Response.Headers.Add("Content-Length", dataToRead.ToString());

                while (dataToRead > 0)
                {
                    //if (CoreProviderContext.HttpContext.Response.IsClientConnected)
                    //{
                    //    int length = stream.Read(buffer, 0, Convert.ToInt32(chunkSize));
                    //    //CoreProviderContext.HttpContext.Response.OutputStream.Write(buffer, 0, length);
                    //    //CoreProviderContext.HttpContext.Response.Response.Flush();
                    //    CoreProviderContext.HttpContext.Response.Clear();
                    //    dataToRead -= length;
                    //}
                    //else
                    //{
                    //    dataToRead = -1; //防止client失去连接 
                    //}
                }
            }
            catch (Exception ex)
            {
                CoreProviderContext.HttpContext.Response.WriteAsync("Error:" + ex.Message);
            }
            finally
            {
                if (stream != null) stream.Close();
                CoreProviderContext.HttpContext.Response.Clear();
            }
        }
        public static bool ResponseFile(HttpRequest _Request, HttpResponse _Response, string _fileName, string _fullPath, long _speed)
        {
            try
            {
                FileStream myFile = new FileStream(_fullPath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
                BinaryReader br = new BinaryReader(myFile);
                try
                {
                    _Response.Headers.Add("Accept-Ranges", "bytes");
                    //_Response.Buffer = false;

                    long fileLength = myFile.Length;
                    long startBytes = 0;
                    int pack = 10240;  //10K bytes
                    int sleep = (int)Math.Floor((double)(1000 * pack / _speed)) + 1;

                    if (_Request.Headers["Range"].Count > 0)
                    {
                        _Response.StatusCode = 206;
                        string[] range = _Request.Headers["Range"].ToString().Split(new char[] { '=', '-' });
                        startBytes = Convert.ToInt64(range[1]);
                    }
                    _Response.Headers.Add("Content-Length", (fileLength - startBytes).ToString());
                    if (startBytes != 0)
                    {
                        _Response.Headers.Add("Content-Range", string.Format(" bytes {0}-{1}/{2}", startBytes, fileLength - 1, fileLength));
                    }

                    _Response.Headers.Add("Connection", "Keep-Alive");
                    _Response.ContentType = "application/octet-stream";
                    _Response.Headers.Add("Content-Disposition", "attachment;filename=" + HttpUtility.UrlEncode(_fileName, System.Text.Encoding.UTF8));

                    br.BaseStream.Seek(startBytes, SeekOrigin.Begin);
                    int maxCount = (int)Math.Floor((double)((fileLength - startBytes) / pack)) + 1;

                    for (int i = 0; i < maxCount; i++)
                    {
                        //if (_Response.IsClientConnected)
                        //{
                        //    _Response.BinaryWrite(br.ReadBytes(pack));
                        //    Thread.Sleep(sleep);
                        //}
                        //else
                        //{
                        //    i = maxCount;
                        //}
                    }
                }
                catch
                {
                    return false;
                }
                finally
                {
                    br.Close();
                    myFile.Close();
                }
            }
            catch
            {
                return false;
            }
            return true;
        }
    }
}
