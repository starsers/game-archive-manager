using System;
using System.Web;

using System.Collections.ObjectModel;

namespace game_archive_manager.Models
{
    public class GameItem
    {
        public Game LeftGame { get; set; }
        public Game MiddleGame { get; set; }
        public Game RightGame { get; set; }
    }
}