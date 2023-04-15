using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client.Model
{
    public class UserOnlineModel
    {
        public ObservableCollection<User> Users { get; set; }

        public UserOnlineModel()
        {
            Users = new ObservableCollection<User>
            {
                new User(1, "jack"), new User(2,"Alice"),new User(3,"Bob")
            };

        }
    }
}
