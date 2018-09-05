
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Linq;
using System.Net.WebSockets;
using System.Threading;
using System.Threading.Tasks;

namespace SkyNetCore.Web.Services
{
    //https://www.cnblogs.com/maxzhang1985/p/6208165.html
    //http://www.cnblogs.com/boxrice/p/8571853.html
    public class SocketHandler
    {
        public const int BufferSize = 4096;
        public static ConcurrentQueue<string> MessageQue=new ConcurrentQueue<string>();

        WebSocket socket;

        SocketHandler(WebSocket socket)
        {
            this.socket = socket;
        }

        async Task EchoLoop()
        {
            var buffer = new byte[BufferSize];
            var seg = new ArraySegment<byte>(buffer);
            
            while (this.socket.State == WebSocketState.Open)
            {
                MessageQue.Enqueue(DateTime.Now.ToString());

                Thread.Sleep(800);

                var msg = string.Empty;

                MessageQue.TryDequeue(out msg);

                Console.WriteLine("msg:" + msg);

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
    }
}
