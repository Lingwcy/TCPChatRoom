using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace ChatProtocol
{
    public class Packet
    {
        public int Length { get; set; }
        public string Data { get; set; }
        public Command Command { get; set; }
        public const byte Delimiter = (byte)'|';
        public const char EscapeChar = '\\';
        public const int HeaderSize = 4;
        public byte[] TotalBytes {get;  set;} = Array.Empty<byte>();
        public Packet(Command cmd,string data)
        {
            Command= cmd;
            Data= data;
            TotalBytes = GetBytes(this);
        }
        protected Packet() { }
        public Packet(int length, Command cmd ,string data) {
            Length= length;
            Command= cmd;
            Data = data;
        }

        public Packet(byte[] bytes)
        {
            Packet p = FromBytes(bytes);
            Length=p.Length;
            Command= p.Command;
            Data= p.Data;
            TotalBytes = GetBytes(p);
        }

        public static byte[] GetBytes(Packet p)
        {
            /**
             * 包格式: 包头(固定4字节-》包总长度) 包体(命令+分隔符+内容)
             * */
            // 计算数据部分长度及总长度
            byte[] commandBytes = Encoding.UTF8.GetBytes(p.Command.ToString());
            byte[] dataBytes = Encoding.UTF8.GetBytes(p.Data);
            int contentLength = commandBytes.Length + 1 + dataBytes.Length;
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
            Buffer.BlockCopy(Encoding.UTF8.GetBytes(p.Data), 0, content, idx, dataBytes.Length);

            // 返回完整数据包
            byte[] packet = new byte[totalLength];
            Buffer.BlockCopy(header, 0, packet, 0, HeaderSize);
            Buffer.BlockCopy(content, 0, packet, HeaderSize, contentLength);
            return packet;

        }

        public static Packet FromBytes(byte[] bytes)
        {
            // 解析包头，获取包体总长度
            int totalLength = bytes[0] | bytes[1] << 8 | bytes[2] << 16 | bytes[3] << 24;
            int contentLength = totalLength - HeaderSize;

            // 解析包体，获取命令和内容
            byte[] content = new byte[contentLength];
            Buffer.BlockCopy(bytes, HeaderSize, content, 0, contentLength);
            string contentString = Encoding.UTF8.GetString(content);
            int delimiterIndex = contentString.IndexOf('|');
            if(delimiterIndex < 0)
            {
                throw new Exception("数据包格式错误,未找到包分隔符!");
            }
            string commandString = contentString.Substring(0, delimiterIndex);
            string dataString = contentString.Substring(delimiterIndex + 1);

            // 返回还原的 Packet 对象
            return new Packet(totalLength, (Command)Enum.Parse(typeof(Command), commandString), dataString);
        }

    }
    public enum Command
    {
        Shake =0,
        Login = 1,
        Logout = 2,
        Chat = 3,
    }
}
