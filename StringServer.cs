using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SocketServer
{
    class StringServer
    {
        private static int myProt = 8885;   //端口  
        private static byte[] result = new byte[1024];
        public static void InitSocketServer()
        {
            IPAddress ip = IPAddress.Parse("127.0.0.1");
            Socket serverSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            serverSocket.Bind(new IPEndPoint(ip, myProt));  //绑定IP地址：端口  
            serverSocket.Listen(100);

            //开启线程 接收数据
            Thread thread = new Thread(ListenConnect);
            thread.Start(serverSocket);
        }

        public static void ListenConnect(object serverSocket)
        {
            Socket myServerSocket = (Socket)serverSocket;
            while (true)
            {
                Socket mySocket = myServerSocket.Accept();
                Thread thread = new Thread(ReceiveFile);
                thread.Start(mySocket);

            }
        }


        public static void ReceiveFile(object mySocket)
        {
            Socket myClientSocket = (Socket)mySocket;
            //读取文件后缀名
            myClientSocket.Receive(result, 3, 0);
            string ext = Encoding.UTF8.GetString(result, 0, 3);

            //获取文件大小
            myClientSocket.Receive(result, 20, SocketFlags.None);
            int filelength = int.Parse(Encoding.UTF8.GetString(result, 0,20));
            //获取文件内容

            string path = Environment.CurrentDirectory + "/temp/a." + ext;
            FileStream fs = File.Create(path);
            int receivedLength = 0;
            while (receivedLength < filelength)
            {
                int bytes = myClientSocket.Receive(result, result.Length, 0);
                receivedLength += bytes;
                fs.Write(result, 0, result.Length);
            }
            fs.Flush();
            fs.Close();
            Console.WriteLine("接收文件完成");
            Console.WriteLine("回复客户端接收完成");
            //回复客户端接收成功
            result = Encoding.UTF8.GetBytes("yes");
            myClientSocket.Send(result);
            Thread.Sleep(10000);
            myClientSocket.Shutdown(SocketShutdown.Both);
            myClientSocket.Close();






        }

    }
}
