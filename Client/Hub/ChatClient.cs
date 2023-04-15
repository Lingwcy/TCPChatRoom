using Client.Handler;
using ChatProtocol;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Client.Hub
{
    public class ChatClient
    {
        public TcpClient Client { get; set; }
        private static IPAddress ServerAddress = IPAddress.Parse("127.0.0.1");
        private static IPEndPoint ServerIP = new IPEndPoint(ServerAddress, 8889);
        private NetworkStream Stream;
        public PacketReceivedEventHandler PacketReceivedHandler { get; set; }
        public PacketHandler PacketHandler { get; set; } = new PacketHandler();

        public ChatClient()
        {
            Client = new TcpClient();
            Client.Connect(ServerIP);
            Stream = Client.GetStream();
            PacketReceivedHandler += OnPacketReceived;
            _ = ReceiveDataAsync();
        }


        public async Task ReceiveDataAsync()
        {
            while (true)
            {
                byte[] data = new byte[1024];
                try
                {
                    if (Stream.CanRead)
                    {
                        await Stream.ReadAsync(data, 0, data.Length);
                        Packet p = Packet.FromBytes(data);
                        PacketReceivedHandler?.Invoke(this, p);
                    }
                }
                catch (Exception e)
                {
                    MessageBox.Show($"接受服务器 {Client.Client.LocalEndPoint} 数据发生异常。 原因: {e.Message}");
                    Client.Close();
                    return;
                }
            }
        }
        private void OnPacketReceived(object sender, Packet packet)
        {
            PacketHandler.Handle(packet);
        }

        public async Task SendChatPacket (ChatPacket p)
        {
           await Stream.WriteAsync(p.TotalBytes);
        }

        public async Task LoginAsync(string username)
        {
            Packet p = new Packet(Command.Login, username);
            await Stream.WriteAsync(p.TotalBytes);
        }
    }
}
