using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using ChatProtocol;

namespace ChatProtocol
{
    public class ChatPacket : Packet
    {
        public string Sender { get; set; }
        public string Receiver { get; set; }
        public ChatPacket(string sender, string receiver, Command cmd ,string chatMessage) : base()
        {
            if (cmd != Command.Chat)
            {
                Sender = "";
                Receiver = "";
                Data = "";
                TotalBytes = GetBytes(this);
                return;
            }
            Sender = sender;
            Receiver = receiver;
            Data = chatMessage;
            TotalBytes = GetBytes(this);
        }
        //bug 不能检测 bytes中 Command是否为chat
        public ChatPacket(byte[] bytes):base(bytes)
        {

        }
        /// <summary>
        /// 数据包格式: {包头(固定四字节)} + (包体(命令|内容 )) =>基础Packet   => ChactPacket =>(发送人|接收人|聊天信息)
        /// 除了约定的分隔符 | 在包内容中出现的一律解析为 "//|"
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        public static byte[] GetBytes(ChatPacket p)
        {
            /*
                 
                string Sender = ReplacePipe(p.Sender);
                string Receiver = ReplacePipe(p.Receiver);
                string Data = ReplacePipe(p.Data);
            */

            byte[] commandBytes = Encoding.UTF8.GetBytes(p.Command.ToString());
            byte[] chatMessageBytes = Encoding.UTF8.GetBytes(p.Data);
            byte[] senderBytes = Encoding.UTF8.GetBytes(p.Sender);
            byte[] receiverBytes = Encoding.UTF8.GetBytes(p.Receiver);
            int contentLength = commandBytes.Length + 1 + chatMessageBytes.Length + 1 + senderBytes.Length +1+ receiverBytes.Length;
            int totalLength = HeaderSize + contentLength;


            // 构建包头
            byte[] header = new byte[HeaderSize];
            header[0] = (byte)totalLength;
            header[1] = (byte)(totalLength >> 8);
            header[2] = (byte)(totalLength >> 16);
            header[3] = (byte)(totalLength >> 24);

            // 构建数据部分
            byte[] content = new byte[contentLength];
            int idx = 0;
            Buffer.BlockCopy(Encoding.UTF8.GetBytes(p.Command.ToString()), 0, content, idx, commandBytes.Length);
            idx += p.Command.ToString().Length;
            content[idx++] = Delimiter;
            Buffer.BlockCopy(Encoding.UTF8.GetBytes(p.Sender), 0, content, idx, senderBytes.Length);
            idx += p.Sender.ToString().Length;
            content[idx++] = Delimiter;
            Buffer.BlockCopy(Encoding.UTF8.GetBytes(p.Receiver), 0, content, idx, receiverBytes.Length);
            idx += p.Receiver.ToString().Length;
            content[idx++] = Delimiter;
            Buffer.BlockCopy(Encoding.UTF8.GetBytes(p.Data), 0, content, idx, chatMessageBytes.Length);


            // 返回完整数据包
            byte[] packet = new byte[totalLength];
            Buffer.BlockCopy(header, 0, packet, 0, HeaderSize);
            Buffer.BlockCopy(content, 0, packet, HeaderSize, contentLength);
            return packet;
        }


        public static new ChatPacket FromBytes(byte[] bytes)
        {
            Packet p = Packet.FromBytes(bytes);
            ChatPacket chat;
            string data = p.Data;
            int prev = -1; // 记录上一个字符的位置
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < data.Length; i++)
            {
                if (i > 0 && data[i] == '|' && data[i - 1] != EscapeChar) // 如果当前字符是分隔符，且前一个字符不是反斜杠
                {
                    sb.Append(data.Substring(prev + 1, i - prev)); // 提取分隔符前的子串
                    prev = i;
                }
            }
            if (prev == -1) throw new Exception("Chat数据包格式错误 未找到分隔符");
            sb.Append(data.Substring(prev + 1)); // 提取最后一个子串
            string message = sb.ToString().Replace(EscapeChar+"|", "|");
            string[] arr = message.Split('|');
            if(arr.Length != 3) throw new Exception("Chat数据包格式错误 不足3个分隔符");
            chat = new ChatPacket(arr[0], arr[1], Command.Chat, arr[2]);
            return chat;
        }

        public static string ReplacePipe(string input)
        {
            return input.Replace("|", "\\|");
        }

    }
}
