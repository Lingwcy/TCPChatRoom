using Client.Hub;
using Client.Model;
using ChatProtocol;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client.Handler
{
    public delegate void PacketReceivedEventHandler(object sender, Packet packet);
    public class PacketHandler
    {
        public void Handle(Packet packet)
        {
            switch (packet.Command)
            {
                case Command.Shake:
                    HandleShake(packet);
                    break;
                case Command.Login:
                    HandleLogin(packet);
                    break;
                case Command.Logout:
                    HandleLogout(packet);
                    break;
                case Command.Chat:
                    HandleChat(packet);
                    break;
                default:
                    Console.WriteLine($"无效命令: {packet.Command}");
                    break;
            }
        }

        private void HandleShake(Packet packet)
        {
            Console.WriteLine("收到Shake指令");
            // 处理 Shake 指令 ...
        }
        private void HandleLogin(Packet packet)
        {
            Console.WriteLine("收到Login指令");
            // 处理 Login 指令 ...
        }

        private void HandleLogout(Packet packet)
        {
            Console.WriteLine("收到Logout指令");
            // 处理 Logout 指令 ...
        }

        private void HandleChat(Packet packet)
        {
            Console.WriteLine("收到Chat指令");
            // 处理 Chat 指令 ...
        }

    }
}
