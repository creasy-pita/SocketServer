using System;
using System.Net.Sockets;
using System.Net;
using System.Threading;
using System.Text;  

namespace SocketServer
{
    class Program
    {
        private static byte[] result = new byte[1024];
        private static int myProt = 8885;   //端口  
        static Socket serverSocket;
        static void Main(string[] args)
        {
            StringServer.InitSocketServer();

            ////服务器IP地址  
            //IPAddress ip = IPAddress.Parse("127.0.0.1");
            //serverSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            //serverSocket.Bind(new IPEndPoint(ip, myProt));  //绑定IP地址：端口  
            //serverSocket.Listen(10);    //设定最多10个排队连接请求  
            //Console.WriteLine("启动监听{0}成功", serverSocket.LocalEndPoint.ToString());
            ////通过Clientsoket发送数据  
            //Thread myThread = new Thread(ListenClientConnect);
            //myThread.Start();
            //Console.ReadLine();
        }

        /// <summary>  
        /// 监听客户端连接  
        /// </summary>  
        private static void ListenClientConnect()
        {
            while (true)
            {

                Socket clientSocket = serverSocket.Accept();//**接收一个新客户端的连接请求，clientsocket.connect,没有时阻塞等待，例如 客户端 192.168.5.21 192.168.5.22
                clientSocket.Send(Encoding.ASCII.GetBytes("Server Say Hello"));
                Thread receiveThread = new Thread(ReceiveMessage);//每个客户端创建线程进行请求接收并处理
                
                receiveThread.Start(clientSocket);
            }
        }

        private static void ReceiveMessage2(object clientSocket)
        {
            Socket myClientSocket = (Socket)clientSocket;
            while (true)
            {
                byte[] buffer = new byte[1024];
                
            }
        }

        /// <summary>  
        /// 接收消息  
        /// </summary>  
        /// <param name="clientSocket"></param>  
        private static void ReceiveMessage(object clientSocket)
        {
            Socket myClientSocket = (Socket)clientSocket;
            while (true)
            {
                try
                {
                    //通过clientSocket接收数据  
                    int receiveNumber = myClientSocket.Receive(result);//
                    Console.WriteLine("接收客户端{0}消息{1}", myClientSocket.RemoteEndPoint.ToString(), Encoding.ASCII.GetString(result, 0, receiveNumber));
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    myClientSocket.Shutdown(SocketShutdown.Both);
                    myClientSocket.Close();
                    break;
                }
            }
        }
    }
}  
