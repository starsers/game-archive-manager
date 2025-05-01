using SQLite;
using System;

namespace game_archive_manager.DataItems
{
    public class Rule
    {
        private string _ruleName = string.Empty;
        private string _ruleContent = string.Empty;

        [PrimaryKey, AutoIncrement]
        public int RuleId { get; set; }  // RID，主键，这个要不要自增？

        public string RuleName
        {
            get => _ruleName;
            set => _ruleName = value ?? string.Empty;
        }

        public string RuleContent
        {
            get => _ruleContent;
            set => _ruleContent = value ?? string.Empty;
        }

        public Rule()
        {
            _ruleName = string.Empty;
            _ruleContent = string.Empty;
        }
    }
}
