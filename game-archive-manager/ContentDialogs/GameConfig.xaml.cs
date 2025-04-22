using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Windows.Input;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Microsoft.UI.Dispatching;

using System.Diagnostics;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

using game_archive_manager.DataItems;

namespace game_archive_manager.ContentDialogs
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class GameConfig : Page
    {
        public ObservableCollection<MatchRule> MatchRules { get; set; }
        public ObservableCollection<MatchRule> ActiveMatchRules { get; set; }
        public ICommand AddRuleCommand { get; }
        public ICommand RemoveRuleCommand { get; }
        public ObservableCollection<RuleNameAndRule> GetMatchRuleNames { get; }
        public GameConfig()
        {
            this.InitializeComponent();
            // 初始化规则集合
            MatchRules = new ObservableCollection<MatchRule>
                {
                    new MatchRule { RuleName = "规则一" },
                    new MatchRule { RuleName = "规则二" }
                };
            ActiveMatchRules = new ObservableCollection<MatchRule>
            {
                new MatchRule()
            };
            GetMatchRuleNames = MatchRuleNames();
            RefreshOtherMatchRuleNames();
            // 初始化命令
            AddRuleCommand = new RelayCommand(AddRule);
            //RemoveRuleCommand = new RelayCommand(param => RemoveRule(param as MatchRule));
            RemoveRuleCommand = new RelayCommand(param =>
            {
                if (param is MatchRule rule)
                {
                    RemoveRule(rule);
                }
            });

            // 设置数据上下文
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
                        matchRule.SelectedRule = selectedItem;
                        int index = ActiveMatchRules.IndexOf(matchRule);
                        if (index >= 0)
                        {
                            DispatcherQueue.GetForCurrentThread().TryEnqueue(() =>
                            {
                                ActiveMatchRules[index] = selectedItem.Rule;
                                RefreshOtherMatchRuleNames();
                            });
                        }
                    }
                }
            }
        }
        private void AddRule()
        {
            MatchRule newrule = new MatchRule();
            ActiveMatchRules.Add(newrule);
            GetMatchRuleNames.Add(new RuleNameAndRule(newrule));
            RefreshMatchRuleNames();
            RefreshOtherMatchRuleNames();

        }

        private void RemoveRule(MatchRule rule)
        {
            if (ActiveMatchRules.Contains(rule))
            {
                ActiveMatchRules.Remove(rule);
                //GetMatchRuleNames.Remove(rule.RuleName);
                RefreshMatchRuleNames();
                RefreshOtherMatchRuleNames();

            }
        }

        private ObservableCollection<RuleNameAndRule> MatchRuleNames()
        {
            ObservableCollection<RuleNameAndRule> a = new ObservableCollection<RuleNameAndRule>();
            foreach(var rule in MatchRules)
            {
                a.Add(new RuleNameAndRule(rule));
            }
            return a;
        }
        private void RefreshMatchRuleNames()
        {
            GetMatchRuleNames.Clear();
            foreach (var rule in MatchRules)
            {
                GetMatchRuleNames.Add(new RuleNameAndRule(rule));
            }
        }
        private void RefreshOtherMatchRuleNames()
        {
            foreach (var rule in ActiveMatchRules)
            {        
                // 创建 GetMatchRuleNames 的副本
                ObservableCollection<RuleNameAndRule> otherMatchRuleNames = new ObservableCollection<RuleNameAndRule>(GetMatchRuleNames);

                //otherMatchRuleNames.Remove(new RuleNameAndRule(rule));
                rule.OtherMatchRuleNames = otherMatchRuleNames;
            }
        }

    }
    // Updated RelayCommand class to fix CS8618 and IDE0290 issues
    public class RelayCommand : ICommand
    {
        private readonly Action<object> _execute;
        private readonly Predicate<object>? _canExecute;

        // Primary constructor to initialize fields
        public RelayCommand(Action<object> execute, Predicate<object>? canExecute = null)
        {
            _execute = execute ?? throw new ArgumentNullException(nameof(execute));
            _canExecute = canExecute;
        }

        // Secondary constructor for simpler usage
        public RelayCommand(Action execute) : this(o => execute(), null) { }

        public bool CanExecute(object? parameter)
        {
            return _canExecute?.Invoke(parameter!) ?? true;
        }

        public void Execute(object? parameter)
        {
            _execute(parameter);
        }

        public event EventHandler? CanExecuteChanged;

        public void RaiseCanExecuteChanged() => CanExecuteChanged?.Invoke(this, EventArgs.Empty);
    }
}
