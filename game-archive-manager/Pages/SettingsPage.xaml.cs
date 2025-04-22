using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Media.Animation;
using Microsoft.UI.Xaml.Navigation;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using System.Collections.ObjectModel;


// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

using game_archive_manager.DataItems;
using System.Diagnostics;

namespace game_archive_manager
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class SettingsPage : Page
    {
        public ObservableCollection<MatchRule> Rules { get; set; }

        public MatchRule? SelectedRule { get; set; }
        public SettingsPage()
        {
            this.InitializeComponent();
            //Rules = new ObservableCollection<MatchRule>();
            Rules = new ObservableCollection<MatchRule>
                {
                    new MatchRule { RuleName = "规则一" },
                    new MatchRule { RuleName = "规则二" }
                };
            // 通过数据库引入数据
            SelectedRule = Rules.First();
            this.DataContext = this;
        }

        private void Rules_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // 获取 ComboBox 的选中项
            var comboBox = sender as ComboBox;
            if (comboBox != null && comboBox.SelectedItem != null)
            {
                // 获取 ComboBox 所在的 MatchRule 实例
                var matchRule = comboBox.DataContext as MatchRule;
                if (matchRule != null)
                {
                    Debug.WriteLine($"当前规则: {matchRule}");

                    if (comboBox.SelectedItem != null)
                    {
                        RuleNameAndRule selectedItem = (RuleNameAndRule)comboBox.SelectedItem;
                        Debug.WriteLine($"选中的项: {selectedItem}");
                    }
                }
            }
        }
        private void AddRuleButton_Click(object sender, RoutedEventArgs e)
        {
            // 添加新规则
            Rules.Add(new MatchRule { RuleName = "新规则" });
        }
        private void NavigateToHomePage(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(HomePage), null, new DrillInNavigationTransitionInfo());
        }

        private void SaveRuleButton_Click(object sender, RoutedEventArgs e)
        {
            // 保存规则 未绑定数据库，且规则内容还未在类内定义
            if (SelectedRule != null)
            {
                //SelectedRule.RuleContent = RuleContent.Text;
                Debug.WriteLine($"保存规则: {SelectedRule.RuleName}");
                SelectedRule.RuleName = RuleName.Text;

                // 为了实现规则显示在下拉列表中，需在前端页面删除rule后重新添加，可修改
                MatchRule Ruletemp= new MatchRule(SelectedRule.RuleName);
                Rules.Remove(SelectedRule);
                Rules.Add(Ruletemp);
                SelectedRule = Ruletemp; // 选择新添加的规则
            }
        }

        private void DeleteRuleButton_Click(object sender, RoutedEventArgs e)
        {
            // 删除规则
            if (SelectedRule != null)
            {
                Debug.WriteLine($"删除规则: {SelectedRule.RuleName}");
                Rules.Remove(SelectedRule);
                if(Rules.Count > 0)
                {
                    SelectedRule = Rules[0]; // 选择第一个规则
                }
                else
                {
                    SelectedRule = null; // 没有规则可选
                }
            }
        }
    }
}
