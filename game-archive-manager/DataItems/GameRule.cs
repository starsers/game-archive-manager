using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SQLite;

namespace game_archive_manager.DataItems
{
    public class GameRule
    {
        [PrimaryKey]
        public int UserRuleId { get; set; }  // URID，主键,外键
        public Guid UGID { get; set; }  // 游戏ID，外键
        public int RuleId { get; set; }  // 规则ID，外键

        // 导航属性
        public Game Game { get; set; } = null!;
        public Rule Rule { get; set; } = null!;

        public GameRule() { }
        
    }
}