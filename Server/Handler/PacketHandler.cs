using Server.Hub;
using ChatProtocol;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Handler
{
    public delegate void PacketReceivedEventHandler(object sender, Packet packet);
    public class PacketHandler
    {
        private SystemMessages SysMessages { get; set; }

        public PacketHandler(SystemMessages sysMessages)
        {
            SysMessages = sysMessages;
        }

        public void Handle(Packet packet, User user)
        {
            if (user.UserName == string.Empty && packet.Command != Command.Login)
            {
                SysMessages.AddInOtherThread($"用户 {user.Client.Client.LocalEndPoint} 未登录发出 {packet.Command} | {packet.Data} 数据将不会被处理");
                return;
            }
            switch (packet.Command)
            {
                case Command.Shake:
                    HandleShake(packet);
                    break;
                case Command.Login:
                    HandleLogin(packet, user);
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
        private void HandleLogin(Packet packet, User user)
        {
            var exist = ChatHub.UserList.Find(a => a.UserName == packet.Data);
            if (exist != null)
            {
                SysMessages.AddInOtherThread($"用户 {user.UserName} 登陆失败,已经有一个相同的用户名在线了! ");
                user.Client.Close();
                return;
            }
            user.UserName = packet.Data;
            user.IsLogined = true;
            SysMessages.AddInOtherThread($"用户 {user.UserName} 成功登陆 ");
        }

        private void HandleLogout(Packet packet)
        {
            Console.WriteLine("收到Logout指令");
            // 处理 Logout 指令 ...
        }

        private void HandleChat(Packet packet)
        {
            var res = ChatPacket.FromBytes(Packet.GetBytes(packet));
            SysMessages.AddInOtherThread($"收到Chat命令 发送人:{res.Sender} 接收人:{res.Receiver} 消息:{res.Data}");

        }

    }
}
