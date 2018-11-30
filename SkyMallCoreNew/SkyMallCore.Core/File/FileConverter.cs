using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace SkyMallCore.Core
{
    public class FileProcess
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="appPath"></param>
        /// <param name="arguments"></param>
        public static void RunProcess(string appPath,string arguments)
        {
            Process p = new Process();
            p.StartInfo.FileName = appPath;
            p.StartInfo.UseShellExecute = false;
            p.StartInfo.Arguments = arguments;
            //p.StartInfo.CreateNoWindow = true;
            p.Start();
        }



        public static Task<int> RunProcessAsync(string appPath,string arguments)
        {
            var tcs = new TaskCompletionSource<int>();

            var process = new Process
            {
                StartInfo = {
                    FileName = appPath,
                    UseShellExecute = false,
                    Arguments = arguments,
                    CreateNoWindow = true
                },
                EnableRaisingEvents = true
            };

            process.Exited += (sender, args) =>
            {
                tcs.SetResult(process.ExitCode);
                process.Dispose();
            };

            process.Start();

            return tcs.Task;
        }










    }
}
