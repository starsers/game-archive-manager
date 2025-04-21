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
    }
}
