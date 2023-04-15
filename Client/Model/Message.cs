using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client.Model
{
    public class Message
    {
        public Message(string data, MessageType type)
        {
            Data = data; 
            Type = type;
        }
        public string Data { get; set; }
        public MessageType Type { get; set; }
    }

    public enum MessageType
    {
        发出=1,
        接受=2
    }
}
