using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace game_archive_manager.Models
{
    public class User
    {
        public string Username { get; set; }
        public string PasswordHash { get; set; }
        public User()
        {
            Username= string.Empty;
            PasswordHash= string.Empty;
        }
        public User(string username, string passwordHash)
        {
            Username = username;
            PasswordHash = passwordHash;
        }
    }
}
