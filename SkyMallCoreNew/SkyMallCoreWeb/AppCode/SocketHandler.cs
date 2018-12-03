using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Net.WebSockets;
using System.Threading;
using System.Threading.Tasks;
using SkyCoreLib.Utils;
using Microsoft.Extensions.Logging;
using SkyCore.GlobalProvider;

namespace SkyMallCoreWeb.AppCode
{
    //https://www.cnblogs.com/maxzhang1985/p/6208165.html
    //http://www.cnblogs.com/boxrice/p/8571853.html
    public class SocketHandler
    {
        public const int BufferSize = 4096;
        WebSocket socket;
        ILogger logger;
        SocketHandler(WebSocket socket)
        {
            logger = CoreContextProvider.GetLogger("SocketHandler");
            this.socket = socket;
        }

        async Task EchoLoop()
        {
            var buffer = new byte[BufferSize];
            var seg = new ArraySegment<byte>(buffer);
            
            while (this.socket.State == WebSocketState.Open)
            {
                var msg = GetLastestMessage();

                Thread.Sleep(500);
                buffer = System.Text.Encoding.Default.GetBytes(msg);
                seg = new ArraySegment<byte>(buffer);

                //var incoming = await this.socket.ReceiveAsync(seg, CancellationToken.None);
                //var outgoing = new ArraySegment<byte>(buffer, 0, );
                await this.socket.SendAsync(seg, WebSocketMessageType.Text, true, CancellationToken.None);
            }
        }

        static async Task Acceptor(HttpContext hc, Func<Task> n)
        {
            if (!hc.WebSockets.IsWebSocketRequest)
                return;

            var socket = await hc.WebSockets.AcceptWebSocketAsync();
            var h = new SocketHandler(socket);
            await h.EchoLoop();
        }

        /// <summary>
        /// branches the request pipeline for this SocketHandler usage
        /// </summary>
        /// <param name="app"></param>
        public static void Map(IApplicationBuilder app)
        {
            app.UseWebSockets();
            app.Use(Acceptor);
        }

        
        private string GetLastestMessage()
        {
            var result = string.Empty;
            var code = 1;
            try
            {
                //if (BooksCrawler.IsRuning)
                //{
                //    if (BooksCrawler.RequesWaiting)
                //    {
                //        result = "请求等待中....";
                //    }
                //    else
                //    {
                //        BooksCrawler.MessageQueue.TryDequeue(out result);
                //        if (result.IsEmpty())
                //        {
                //            result = "请求处理中....";
                //        }
                //    }
                //}
                //else
                //{
                //    result = "操作完毕，请求停止.......";
                //    code = 0;
                //}
            }
            catch (Exception ex)
            {
                logger.LogError("[GetLastestMessage]" + ex.ToString());
                code = 0;
                result = ex.Message;
            }
            return new AjaxResult { state = code == 1 ? ResultType.success.ToString() : ResultType.error.ToString(), message = result }.ToJson();
        }


    }
}
