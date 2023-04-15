using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client.Model
{
    public class User
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public User(int id,string name)
        {
            Id= id;
            Name= name;
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as User);
        }

        public bool Equals(User other)
        {
            return other != null && other.Id == Id && other.Name == Name;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Id, Name);
        }

    }
}
