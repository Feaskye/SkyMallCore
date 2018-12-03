
using ICSharpCode.SharpZipLib.Zip;
using Microsoft.AspNetCore.Http;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Web;

namespace SkyCoreLib.Utils
{
    public static class FileDownHelper
    {

        /// <summary>
        /// 下载并保存
        /// </summary>
        /// <param name="url">网络路径</param>
        /// <param name="savePath">保存本地的文件夹</param>
        public static void DownRemoteFile(string url, string savePath)
        {
            var httpClient = new HttpClient();
            var t = httpClient.GetByteArrayAsync(url);
            t.Wait();
            Stream responseStream = new MemoryStream(t.Result);
            Stream stream = new FileStream(savePath, FileMode.Create,FileAccess.ReadWrite);
            byte[] bArr = new byte[1024];
            int size = responseStream.Read(bArr, 0, bArr.Length);
            while (size > 0)
            {
                stream.Write(bArr, 0, size);
                size = responseStream.Read(bArr, 0, bArr.Length);
            }
            stream.Close();
            responseStream.Close();
        }

        #region 单文件
        public static void DownLoadold(this HttpContext httpContext, string FileName, string name)
        {
            string destFileName = FileName;
            if (File.Exists(destFileName))
            {
                FileInfo fi = new FileInfo(destFileName);
                httpContext.Response.Clear();
                //CoreContextProvider.HttpContext.Response.ClearHeaders();
                //CoreContextProvider.HttpContext.Response.Buffer = false;
                //CoreContextProvider.HttpContext.Response.AppendHeader("Content-Disposition", "attachment;filename=" + HttpUtility.UrlEncode(name, System.Text.Encoding.UTF8));
                //CoreContextProvider.HttpContext.Response.AppendHeader("Content-Length", fi.Length.ToString());
                httpContext.Response.ContentType = "application/octet-stream";
                //CoreContextProvider.HttpContext.Response.WriteFile(destFileName);
                //CoreContextProvider.HttpContext.Response.Flush();
                //CoreContextProvider.HttpContext.Response.End();
            }
        }
        public static byte[] DownLoad(string fileName)
        {
            string filePath =FileHelper.MapFilePath(fileName);
            long chunkSize = 204800;             //指定块大小 
            byte[] buffer = new byte[chunkSize]; //建立一个200K的缓冲区 
            long dataToRead = 0;                 //已读的字节数   
            using (FileStream stream = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.Read))
            {
                dataToRead = stream.Length;
                buffer = new byte[dataToRead];
                stream.Position = 0;
                stream.Read(buffer, 0, buffer.Length);
                return buffer;
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
        #endregion


        #region 解压缩多文件
        /// <summary>
        /// 压缩多个文件到zip
        /// </summary>
        /// <param name="files"></param>
        /// <returns></returns>
        public static byte[] DownloadZip(Dictionary<string, string> files)
        {
            byte[] buffer = null;
            using (MemoryStream ms = new MemoryStream())
            {
                using (ZipFile file = ZipFile.Create(ms))
                {
                    file.BeginUpdate();
                    file.NameTransform = new MyNameTransfom();
                    //通过这个名称格式化器，可以将里面的文件名进行一些处理。默认情况下，会自动根据文件的路径在zip中创建有关的文件夹。
                    foreach (var f in files)
                    {
                        var path = FileHelper.MapFilePath(f.Key);
                        if (File.Exists(path))
                        {
                            if (f.Value == f.Key)
                            {
                                file.Add(path);
                            }
                            else
                            {
                                file.Add(path,f.Value+FileHelper.GetExtension(path));
                            }
                            //file.Add(path);
                        }
                    }

                    file.CommitUpdate();

                    buffer = new byte[ms.Length];
                    ms.Position = 0;
                    ms.Read(buffer, 0, buffer.Length);
                    return buffer;
                }
            }
        }

        /// <summary>
        /// 解压zip文件，返回解压目录
        /// </summary>
        /// <param name="srcFile"></param>
        /// <param name="dstFile"></param>
        /// <param name="BufferSize"></param>
        public static string UnZip(string srcFile, string dstFile)
        {
            //var ischangeFIle = false;
            var extension = Path.GetExtension(srcFile).ToLower();
            if (!Directory.Exists(dstFile))
            {
                Directory.CreateDirectory(dstFile);
            }
            if (extension != ".zip")
            {
                return DeCompressRar(srcFile, dstFile);
                //FileHelper.CopyFile(srcFile, srcFile.Replace(extension, ".zip"));
                //var newSrcFile = srcFile.Replace(extension, ".zip");
                //if (File.Exists(srcFile))
                //{
                //    File.Copy(srcFile, newSrcFile, true);
                //}
                //ischangeFIle = true;
            }

            var files = Directory.GetFiles(dstFile);
            if (files != null && files.Length > 0)
            {
                Directory.Delete(dstFile,true);
                Directory.CreateDirectory(dstFile);
            }
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
            System.IO.Compression.ZipFile.ExtractToDirectory(srcFile, dstFile, Encoding.GetEncoding("gb2312"));
            return dstFile;

            //using (FileStream fileStreamIn = new FileStream
            //    (srcFile, FileMode.Open, FileAccess.Read))
            //{
            //    using (ZipInputStream zipInStream = new ZipInputStream(fileStreamIn))
            //    {
            //        ZipEntry entry;
            //        while ((entry = zipInStream.GetNextEntry()) != null)
            //        {
            //            using (FileStream fileStreamOut = new FileStream
            //                 (dstFile + @"\" + entry.Name, FileMode.Create, FileAccess.Write))
            //            {
            //                int size;
            //                byte[] buffer = new byte[fileStreamIn.Length];
            //                do
            //                {
            //                    size = zipInStream.Read(buffer, 0, buffer.Length);
            //                    fileStreamOut.Write(buffer, 0, size);
            //                } while (size > 0);

            //                //return dstFile + @"\" + entry.Name;
            //            }
            //        }
            //        return dstFile;
            //    }
            //}
        }

        /// <summary>
        /// 将格式为rar的压缩文件解压到指定的目录
        /// </summary>
        /// <param name="rarFileName"></param>
        /// <param name="saveDir"></param>
        public static string DeCompressRar(string rarFileName, string saveDir)
        {
            if (!Directory.Exists(saveDir))
            {
                Directory.CreateDirectory(saveDir);
            }
            string regKey = @"SOFTWARE\Microsoft\Windows\CurrentVersion\App Paths\WinRAR.exe";
            RegistryKey registryKey = Registry.LocalMachine.OpenSubKey(regKey);
            string winrarPath = registryKey.GetValue("").ToString();
            registryKey.Close();
            string winrarDir = System.IO.Path.GetDirectoryName(winrarPath);
            String commandOptions = string.Format("x {0} {1} -y", rarFileName, saveDir);

            ProcessStartInfo processStartInfo = new ProcessStartInfo();
            processStartInfo.WorkingDirectory = winrarDir;
            processStartInfo.FileName = System.IO.Path.Combine(winrarDir, "winrar.exe");
            processStartInfo.Arguments = commandOptions;
            processStartInfo.WindowStyle = ProcessWindowStyle.Hidden;

            Process process = new Process();
            process.StartInfo = processStartInfo;
            process.Start();
            process.WaitForExit();
            process.Close();
            return saveDir;
        }

        #endregion


    }



    /// <summary>
    /// 名称格式化
    /// </summary>
    public class MyNameTransfom : ICSharpCode.SharpZipLib.Core.INameTransform
    {

        #region INameTransform 成员

        public string TransformDirectory(string name)
        {
            return null;
        }

        public string TransformFile(string name)
        {
            return Path.GetFileName(name);
        }

        #endregion
    }




}
