using SQLite;
using System;

namespace game_archive_manager.DataItems
{
    public class UserRule
    {
        [PrimaryKey]
        public int UserRuleId { get; set; }  // URID，主键
        public Guid UUID { get; set; }  // 用户ID，外键
        public int RuleId { get; set; }  // 规则ID，外键

        // 导航属性
        public User User { get; set; } = null!;
        public Rule Rule { get; set; } = null!;

        public UserRule() { }
    }
}
