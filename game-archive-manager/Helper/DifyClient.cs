using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;

// using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace game_archive_manager.Helper
{
    internal class DifyClient
    {
        private readonly HttpClient _httpClient;
        private readonly string _apiKey;
        private readonly string _baseUrl;

        // 检查 _baseUrl 是否使用了正确的 HTTPS 协议  
        // 如果 _baseUrl 是 HTTP 而非 HTTPS，可能会导致请求失败。  
        // 确保在构造函数中传入的 baseUrl 是以 "https://" 开头的。  

        public DifyClient(string apiKey, string baseUrl = "https://dify.yujieweb.top")
        {
            if (!baseUrl.StartsWith("https://", StringComparison.OrdinalIgnoreCase))
            {
                throw new ArgumentException("Base URL 必须使用 HTTPS 协议。", nameof(baseUrl));
            }

            _apiKey = apiKey;
            _baseUrl = baseUrl;
            _httpClient = new HttpClient();
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _apiKey);
        }

        /// <summary>
        /// 发送阻塞式聊天消息请求
        /// </summary>
        public async Task<JObject> SendChatMessageAsync(
    string query,
    Dictionary<string, object> inputs = null,
    string conversationId = "",
    string user = "abc",  // 设置默认用户，与Python脚本一致
    List<FileAttachment> files = null)
        {
            var url = $"{_baseUrl}/v1/chat-messages";

            var payload = new
            {
                inputs = inputs ?? new Dictionary<string, object>(),
                query,
                response_mode = "blocking",
                conversation_id = conversationId,
                user,  // 确保不为null
                files = files?.Count > 0 ? files : null  // 如果为空列表，则传null
            };

            // 输出请求详情以进行调试
            var jsonPayload = JsonConvert.SerializeObject(payload);
            Debug.WriteLine($"请求URL: {url}");
            Debug.WriteLine($"请求头: Bearer {_apiKey}");
            Debug.WriteLine($"请求体: {jsonPayload}");

            var content = new StringContent(
                jsonPayload,
                Encoding.UTF8,
                "application/json");

            try
            {
                var response = await _httpClient.PostAsync(url, content);

                // 如果请求不成功，获取错误详情
                if (!response.IsSuccessStatusCode)
                {
                    var errorContent = await response.Content.ReadAsStringAsync();
                    Debug.WriteLine($"请求失败: {response.StatusCode}, 错误信息: {errorContent}");
                    throw new HttpRequestException($"请求失败，状态码: {response.StatusCode}, 错误信息: {errorContent}");
                }

                var jsonResponse = await response.Content.ReadAsStringAsync();
                return JObject.Parse(jsonResponse);
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"异常: {ex.Message}");
                throw;
            }
        }

         /// <summary>
        /// 发送流式聊天消息请求并使用回调函数处理流式响应
        /// </summary>
        public async Task SendStreamingChatMessageAsync(
            string query,
            Action<string, JObject> onMessageChunk,
            Action<JObject> onComplete = null,
            Dictionary<string, object> inputs = null,
            string conversationId = "",
            string user = "a",
            List<FileAttachment> files = null)
        {
            var url = $"{_baseUrl}/v1/chat-messages";
            
            var payload = new
            {
                inputs = inputs ?? new Dictionary<string, object>(),
                query,
                response_mode = "streaming",
                conversation_id = conversationId,
                user,
                files = files ?? new List<FileAttachment>()
            };

            var content = new StringContent(
                JsonConvert.SerializeObject(payload),
                Encoding.UTF8,
                "application/json");

            var request = new HttpRequestMessage(HttpMethod.Post, url)
            {
                Content = content
            };

            var response = await _httpClient.SendAsync(
                request, 
                HttpCompletionOption.ResponseHeadersRead);
            
            response.EnsureSuccessStatusCode();
            
            using (var stream = await response.Content.ReadAsStreamAsync())
            using (var reader = new StreamReader(stream))
            {
                string line;
                JObject metadata = null;
                
                while ((line = await reader.ReadLineAsync()) != null)
                {
                    if (string.IsNullOrEmpty(line) || !line.StartsWith("data: "))
                        continue;

                    var jsonData = line.Substring(6); // 移除 "data: "
                    var eventData = JObject.Parse(jsonData);
                    var eventType = eventData["event"].ToString();

                    switch (eventType)
                    {
                        case "message":
                            var text = eventData["answer"].ToString();
                            onMessageChunk?.Invoke(text, eventData);
                            break;
                            
                        case "message_end":
                            metadata = (JObject)eventData["metadata"];
                            onComplete?.Invoke(eventData);
                            break;
                            
                        case "agent_thought":
                        case "agent_message":
                        case "message_file":
                        case "tts_message":
                        case "tts_message_end":
                            // 处理其他事件类型
                            onMessageChunk?.Invoke(null, eventData);
                            break;
                    }
                }
            }
        }
        public async Task<JObject> SafeSendChatMessageAsync(
            string query,
            Dictionary<string, object> inputs = null,
            string conversationId = "",
            string user = null,
            List<FileAttachment> files = null)
        {
            var url = $"{_baseUrl}/v1/chat-messages";

            var payload = new
            {
                inputs = inputs ?? new Dictionary<string, object>(),
                query,
                response_mode = "blocking",
                conversation_id = conversationId,
                user,
                files = files ?? new List<FileAttachment>()
            };

            var content = new StringContent(
                JsonConvert.SerializeObject(payload),
                Encoding.UTF8,
                "application/json");

            try
            {
                var response = await _httpClient.PostAsync(url, content);

                if (!response.IsSuccessStatusCode)
                {
                    var errorContent = await response.Content.ReadAsStringAsync();
                    throw new HttpRequestException($"请求失败，状态码: {response.StatusCode}, 错误信息: {errorContent}");
                }

                var jsonResponse = await response.Content.ReadAsStringAsync();
                return JObject.Parse(jsonResponse);
            }
            catch (HttpRequestException ex)
            {
                // 根据状态码或错误信息处理特定错误
                if (ex.Message.Contains("404"))
                {
                    throw new Exception("对话不存在");
                }
                else if (ex.Message.Contains("invalid_param"))
                {
                    throw new Exception("传入参数异常");
                }
                else if (ex.Message.Contains("app_unavailable"))
                {
                    throw new Exception("App 配置不可用");
                }
                else if (ex.Message.Contains("provider_not_initialize"))
                {
                    throw new Exception("无可用模型凭据配置");
                }
                else if (ex.Message.Contains("provider_quota_exceeded"))
                {
                    throw new Exception("模型调用额度不足");
                }
                else if (ex.Message.Contains("model_currently_not_support"))
                {
                    throw new Exception("当前模型不可用");
                }
                else if (ex.Message.Contains("completion_request_error"))
                {
                    throw new Exception("文本生成失败");
                }
                else if (ex.Message.Contains("500"))
                {
                    throw new Exception("服务内部异常");
                }
                else
                {
                    throw new Exception($"未知错误: {ex.Message}");
                }
            }
        }
    }
    public class FileAttachment
    {
        [JsonProperty("type")]
        public string Type { get; set; } // "image" 等

        [JsonProperty("transfer_method")]
        public string TransferMethod { get; set; } // "remote_url" 或 "local_file"

        [JsonProperty("url")]
        public string Url { get; set; }
        
        // 如果使用本地文件，可以添加其他属性
    }
}
