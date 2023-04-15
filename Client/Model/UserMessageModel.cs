
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client.Model
{
    public class UserMessageModel
    {
        public UserMessageModel()
        {
            Conversations = new ObservableCollection<Conversation> { 
                new Conversation(new User(1,"jack"),new ObservableCollection<Message>{new Message("你好",MessageType.发出),new Message("What Time Now?",MessageType.接受) }),
                new Conversation(new User(2,"Alice"),new ObservableCollection<Message>{new Message("dddd",MessageType.发出),new Message("What Time Now?",MessageType.接受) })
            };
        }

        public ObservableCollection<Conversation> Conversations { get; set; }

        public int FindConversation(User user)
        {
            int index = 0;
            Conversations.FirstOrDefault(c => { index++; return c.User.Equals(user); });
            return index - 1;
        }
        
        public User FindUser(string name)
        {
           var res = Conversations.SingleOrDefault(a => a.User.Name == name);
           return res.User;
        }
    }
}
