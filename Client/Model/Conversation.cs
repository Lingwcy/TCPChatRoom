using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client.Model
{
    public class Conversation
    {
        public Conversation(User user, ObservableCollection<Message> messages)
        {
            User = user;
            Messages = messages;
        }

        public User User { get; set; }
        public ObservableCollection<Message> Messages { get; set; }
    }
}
