using Server.Handler;
using ChatProtocol;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Server.Hub
{
    public class ChatHub
    {
        private SystemMessages SysMessages { get; set; }
        public static List<User> UserList = new List<User>();
        private TcpListener ServerListener { get; set; }
        public IPAddress ServerAddress { get; private set; } = IPAddress.Parse("127.0.0.1");
        public int ServerPort { get; private set; } = 8889;
        public IPEndPoint ServerIP { get; private set; }
        public bool IsNormalExit { get; set; } = false;
        public PacketReceivedEventHandler PacketReceivedHandler { get; set; }
        public PacketHandler PacketHandler { get; set; }

        public ChatHub(SystemMessages msg)
        {
            ServerIP = new IPEndPoint(ServerAddress, ServerPort);
            ServerListener = new TcpListener(ServerIP);
            SysMessages = msg;
            PacketHandler = new PacketHandler(SysMessages);
        }

        public void StartListener()
        {
            ServerListener.Start();
            SysMessages.Messages.Add($"ChatHub 服务器开始监听于: {ServerListener.LocalEndpoint}");
            Task.Factory.StartNew(() =>
            {
                TcpClient newClient;
                while (true)
                {
                    try
                    {
                        newClient = ServerListener.AcceptTcpClient();
                    }
                    catch (Exception e)
                    {
                        SysMessages.AddInOtherThread($"连接发送错误,监听取消: {e.Message}");
                        break;
                    }
                    User user = new User(newClient);
                    UserList.Add(user);
                    SysMessages.AddInOtherThread($"用户 {newClient.Client.LocalEndPoint} 开始连接");
                    //启动接受和发送数据监听
                    PacketReceivedHandler += OnPacketReceived;
                    Task.Factory.StartNew(() => { ReceiveData(user); });
                    
                }
            });
        }

        public void ReceiveData(User user)
        {
            while (!IsNormalExit)
            {
                byte[] data = new byte[1024];
                try
                {
                   if(user.Stream.CanRead)
                    {
                       user.Stream.Read(data,0,data.Length);
                       Packet p = Packet.FromBytes(data);
                       SysMessages.AddInOtherThread($"{user.Client.Client.LocalEndPoint}({user.UserName}) 发送-> 指令:{p.Command} 数据:{p.Data}");
                       PacketReceivedHandler.Invoke(user, p);
                    }

                }
                catch(Exception e)
                {
                    SysMessages.AddInOtherThread($"接受用户 {user.Client.Client.LocalEndPoint}({user.UserName}) 数据发生异常。 原因: {e.Message}");
                    RemoveUser(user);
                    return;
                }
            }
        }
        private void OnPacketReceived(object sender, Packet packet)
        {
            PacketHandler.Handle(packet,(User)sender);
        }


        public void SendData()
        {

        }

        public void RemoveUser(User user)
        {

        }

        public void StopListener()
        {
            IsNormalExit = true;
            foreach (User user in UserList)
            {
                RemoveUser(user);
            }
            ServerListener.Stop();
        }
    }
}
