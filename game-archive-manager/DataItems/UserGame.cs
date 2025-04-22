using SQLite;
using System;

namespace game_archive_manager.DataItems
{
    public class UserGame
    {
        [PrimaryKey]
        public int UserGameId { get; set; }  // UGID，主键
        public Guid UUID { get; set; }  // 用户ID，外键
        public int GameId { get; set; }  // 游戏ID，外键

        // 导航属性
        public User User { get; set; } = null!;
        public Game Game { get; set; } = null!;

        public UserGame() { }
    }
}
