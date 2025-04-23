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

using game_archive_manager.Helper;
using Windows.ApplicationModel.DataTransfer;
using Microsoft.UI.Xaml.Documents;
using System.ComponentModel;
namespace game_archive_manager
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class SettingsPage : Page
    {
        public event PropertyChangedEventHandler? PropertyChanged;

        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        public ObservableCollection<MatchRule> Rules { get; set; }

        private MatchRule? _selectedRule { get; set; }
        public MatchRule? SelectedRule {
            get => _selectedRule;
            set
            {
                _selectedRule = value ?? new MatchRule();
                // 触发属性更改通知
                OnPropertyChanged(nameof(SelectedRule));
            }
        }

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
                var matchRule = comboBox.SelectedItem as MatchRule;
                if (matchRule != null)
                {
                    Debug.WriteLine($"之前规则: {SelectedRule}");
                    Debug.WriteLine($"当前规则: {matchRule}");

                    SelectedRule = matchRule;
                    UpdateSelectedRuleShow();
                    Debug.WriteLine($"选中的项: {SelectedRule}");

                }
            }
        }
        private void UpdateSelectedRuleShow()
        {
            // 更新选中规则的显示
            if (SelectedRule != null)
            {
                RuleName.Text = SelectedRule.RuleName;
                //RuleContent.Text = SelectedRule.RuleContent;
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

        // 当 AI Chat Expander 展开时触发
        private void AiChatExpander_Expanded(Expander sender, ExpanderExpandingEventArgs args)
        {
            // Add your logic here
        }

        // 当 AI Chat Expander 收起时触发
        private void AiChatExpander_Collapsed(Expander sender, ExpanderCollapsedEventArgs args)
        {
            // Add your logic here for when the Expander is collapsed
        }

        // 处理 Send 按钮点击事件
        private async void SendButton_Click(object sender, RoutedEventArgs e)
        {
            // 获取 TextBox 中的消息内容
            var button = sender as Button;
            if (button != null)
            {
                var parentPanel = button.Parent as StackPanel;
                if (parentPanel != null)
                {
                    // 查找 StackPanel 中的 RichEditBox
                    var messageBox = parentPanel.Children.OfType<RichEditBox>().FirstOrDefault();
                    if (messageBox != null)
                    {
                        string message;
                        messageBox.Document.GetText((Microsoft.UI.Text.TextGetOptions)Windows.UI.Text.TextGetOptions.None, out message);
                        Debug.WriteLine($"发送的消息: {message}");

                        // 在这里处理发送的消息
                        var difyClient = new DifyClient("app-xFnWSffQtkmxTUHHOKRiqwXq");
                        try
                        {
                            var response = await difyClient.SendChatMessageAsync(message);

                            Debug.WriteLine("回答: " + response["answer"]);
                            Debug.WriteLine("使用的Token数: " + response["metadata"]["usage"]["total_tokens"]);

                            // 更新 AI 回复内容
                            if (AiResponseBox != null && response["answer"] != null)
                            {
                                AiResponseBox.Blocks.Clear();
                                var paragraph = new Paragraph();
                                paragraph.Inlines.Add(new Run { Text = response["answer"].ToString() });
                                AiResponseBox.Blocks.Add(paragraph);
                            }
                        }
                        catch (Exception ex)
                        {
                            Debug.WriteLine($"请求失败: {ex.Message}");
                        }

                        // 清空输入框
                        messageBox.Document.SetText((Microsoft.UI.Text.TextSetOptions)Windows.UI.Text.TextSetOptions.None, string.Empty);
                    }
                }
            }
        }

        private void CopyButton_Click(object sender, RoutedEventArgs e)
        {
            // 获取 RichTextBlock 中的选中文本
            var selectedText = AiResponseBox.SelectedText;

            if (!string.IsNullOrEmpty(selectedText))
            {
                // 创建 DataPackage 并将选中文本复制到剪贴板
                var dataPackage = new DataPackage();
                dataPackage.SetText(selectedText);
                Clipboard.SetContent(dataPackage);

                // 可选：显示复制成功的提示
                var dialog = new ContentDialog
                {
                    Title = "复制成功",
                    Content = "选中文本已复制到剪贴板。",
                    CloseButtonText = "确定",
                    XamlRoot = this.Content.XamlRoot

                };
                _ = dialog.ShowAsync();
            }
            else
            {
                // 可选：显示未选择文本的提示
                var dialog = new ContentDialog
                {
                    Title = "复制失败",
                    Content = "请先选择要复制的文本。",
                    CloseButtonText = "确定",
                    XamlRoot = this.Content.XamlRoot
                };
                _ = dialog.ShowAsync();
            }
        }
    }
}
