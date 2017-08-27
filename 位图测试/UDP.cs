using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;
using System.Net;
using System.Threading;

namespace 位图测试
{
    class UDP
    {
        public static Socket server;
        public static Byte[] data = new Byte[1500];
        static bool ReciveMsg_prepare()
        {
            EndPoint point = new IPEndPoint(IPAddress.Any, 0);//用来保存发送方的ip和端口号
                                                              //byte[] buffer = new byte[1024];
            int length = server.ReceiveFrom(data, ref point);//接收数据报
            string message = Encoding.UTF8.GetString(data, 0, length);
            if(message=="get ready")
                return true;
            else
                return false;
        }
    }
}
