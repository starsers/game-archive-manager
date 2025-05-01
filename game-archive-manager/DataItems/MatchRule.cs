using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace game_archive_manager.DataItems
{
    public class RuleNameAndRule
    {
        public MatchRule Rule { get; set; }
        public int RuleId { get; set; }
        public RuleNameAndRule(MatchRule rule)
        {
            Rule = rule;
        }
        public string RuleName()
        {
            if (Rule == null) return string.Empty;
            else
                return Rule.RuleName;
        }
        public override string ToString()
        {
            return RuleName();
        }
        // 重写 Equals 方法
        public override bool Equals(object? obj)
        {
            if (obj is RuleNameAndRule other)
            {
                return Equals(Rule, other.Rule);
            }
            return false;
        }

        // 重写 GetHashCode 方法
        public override int GetHashCode()
        {
            return Rule?.GetHashCode() ?? 0;
        }
    }

    // 封装每个规则的类
    public class MatchRule
    {
        public string RuleName { get; set; }
        public int RuleId { get; set; }
        public RuleNameAndRule SelectedRule { get; set; }
        public ObservableCollection<RuleNameAndRule> OtherMatchRuleNames
        { get; set; }
        public MatchRule()
        {
            RuleName = string.Empty;
            SelectedRule = new RuleNameAndRule(this);
            OtherMatchRuleNames = new ObservableCollection<RuleNameAndRule>();
        }
        public MatchRule(string ruleName)
        {
            SelectedRule = new RuleNameAndRule(this);
            RuleName = ruleName;
            OtherMatchRuleNames = new ObservableCollection<RuleNameAndRule>();
            RuleId = 0;
        }

        // Replace the problematic line in the MatchRule constructor:
        public MatchRule(RuleNameAndRule ruleNameAndRule)
        {
            // Instead of assigning to 'this', copy the properties from the provided rule.
            if (ruleNameAndRule.Rule == null)
            {
                RuleName = string.Empty;
                SelectedRule = new RuleNameAndRule(this);
                OtherMatchRuleNames = new ObservableCollection<RuleNameAndRule>();
                RuleId = 0;
            }
            else
            {
                RuleName = ruleNameAndRule.Rule.RuleName;
                SelectedRule = ruleNameAndRule.Rule.SelectedRule;
                OtherMatchRuleNames = new ObservableCollection<RuleNameAndRule>(ruleNameAndRule.Rule.OtherMatchRuleNames);
                RuleId = ruleNameAndRule.Rule.RuleId;
            }
        }

        public override string ToString()
        {
            return RuleName;
        }
    }

}
