using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Windows.Devices.Power;

namespace game_archive_manager.Helper
{
    public static class RuleHelper
    {
        public static string IsHaveMatchContent(string RuleContent,string FilePath)
        {
            if (string.IsNullOrEmpty(RuleContent) || string.IsNullOrEmpty(FilePath))
            {
                throw new ArgumentException("Rule content and file path cannot be null or empty.");
            }
            // 这里可以根据需要实现具体的匹配逻辑
            // 例如，检查文件路径是否包含规则内容
            string FileContent = File.ReadAllText(FilePath);
            return Regex.IsMatch(FileContent, RuleContent) ? "Match" : "No Match";
        }
        public static string GetMatchContent(string RuleContent, string FilePath)
        {
            if (string.IsNullOrEmpty(RuleContent) || string.IsNullOrEmpty(FilePath))
            {
                throw new ArgumentException("Rule content and file path cannot be null or empty.");
            }
            // 这里可以根据需要实现具体的匹配逻辑
            // 例如，检查文件路径是否包含规则内容
            string FileContent = File.ReadAllText(FilePath);
            string MatchContent = string.Empty;
            Match match = Regex.Match(FileContent, RuleContent);
            if (match.Success)
            {
                MatchContent = match.Value; 
            }
            else
            {
                MatchContent = "No Match";
            }
            return MatchContent;
        }
    }
}
