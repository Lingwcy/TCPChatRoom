using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Channels;
using System.Threading.Tasks;

namespace Server.Hub
{
    public class User
    {
        public TcpClient Client { get; private set; }
        public NetworkStream Stream { get; private set; }
        public BinaryWriter Writer { get; private set; }
        public BinaryReader Reader { get; private set; }
        public string UserName { get; set; } =string.Empty;
        public bool IsLogined { get; set; } =false;
        public User(TcpClient client)
        {
            Client = client;
            Stream = client.GetStream();
            Writer = new BinaryWriter(Stream);
            Reader = new BinaryReader(Stream);
        }

        public void Close()
        {
            Writer.Close();
            Reader.Close();
            Stream.Close();
            Client.Close();
        }
    }
}
