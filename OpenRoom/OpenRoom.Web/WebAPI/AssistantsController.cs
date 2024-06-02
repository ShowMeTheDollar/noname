using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OpenRoom.Web.Services;
using System.IO.IsolatedStorage;
using System.Net.Http.Headers;
using System.Text;

namespace OpenRoom.Web.WebAPI
{
    [Route("api/[controller]")]
    [ApiController]
    public class AssistantsController : ControllerBase
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly string _userService;
        public AssistantsController(IHttpClientFactory httpClientFactory, UserService userService)
        {
            _httpClientFactory = httpClientFactory;
            _userService = userService.ToString();
        }

        [HttpPost]
        public async Task<string> GetResponseFromGPT([FromBody] dynamic message)
        {
            if (message.GetProperty("message").GetString().Contains("/reset"))
            {
                ChatHistoryManager.DeleteIsolatedStorageFile();
                return "我已經把之前的對談都給忘了!";
            }
            else
            {
                List<Message> chatHistory = ChatHistoryManager.GetMessagesFromIsolatedStorage(_userService);

                var client = _httpClientFactory.CreateClient();

                client.DefaultRequestHeaders.Authorization =
                    new AuthenticationHeaderValue("Bearer", "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");

                var messages = new List<ChatMessage>
                    {
                        new ChatMessage {
                            role = Role.system ,
                            content = @"
                                假設你是一個專業的房東，對於客戶非常有禮貌、也能夠安撫客戶的抱怨情緒。
                                請檢視底下的客戶訊息，以最親切有禮的方式回應。

                                但回應時，請注意以下幾點:
                                * 不要說 '感謝你的來信' 之類的話，因為客戶是從對談視窗輸入訊息的，不是寫信來的
                                * 不能過度承諾
                                * 要同理客戶的情緒
                                * 要能夠盡量解決客戶的問題
                                * 不要以回覆信件的格式書寫，請直接提供對談機器人可以直接給客戶的回覆
                                ----------------------"
                        }
                    };

                //添加歷史對話紀錄
                foreach (var historyMessageItem in chatHistory)
                {
                    //添加一組對話紀錄
                    messages.Add(new ChatMessage()
                    {
                        role = Role.user,
                        content = historyMessageItem.UserMessage
                    });
                    messages.Add(new ChatMessage()
                    {
                        role = Role.assistant,
                        content = historyMessageItem.ResponseMessage
                    });
                }
                messages.Add(new ChatMessage()
                {
                    role = Role.user,
                    content = message.GetProperty("message").GetString()
                });

                var payload = new
                {
                    model = "gpt-4-turbo",
                    messages = messages
                };

                string jsonPayload = JsonConvert.SerializeObject(payload);
                var content = new StringContent(jsonPayload, Encoding.UTF8, "application/json");
                var response = await client.PostAsync("https://api.openai.com/v1/chat/completions", content);

                if (response.IsSuccessStatusCode)
                {
                    var responseContent = await response.Content.ReadAsStringAsync();
                    var chatGPTResponse = JsonConvert.DeserializeObject<dynamic>(responseContent);
                    ChatHistoryManager.SaveMessageToIsolatedStorage(
                                DateTime.Now, _userService, message.GetProperty("message").GetString(), chatGPTResponse.choices[0].message.content.Value);
                    return chatGPTResponse.choices[0].message.content.Value;
                }
                else
                {
                    return "Error: Failed to get response";
                }
            }         
        }
    }

    [JsonConverter(typeof(Newtonsoft.Json.Converters.StringEnumConverter))]
    public enum Role
    {
        assistant, user, system
    }
    public class ChatMessage
    {
        public Role role { get; set; }
        public string content { get; set; }
    }

    //一組對談訊息
    public class Message
    {
        public DateTime Time { get; set; }
        public string UserID { get; set; }
        public string UserMessage { get; set; }
        public string ResponseMessage { get; set; }
    }

    //對話紀錄處理
    public class ChatHistoryManager
    {
        const string fileName = "messages.json"; //儲存到 IsolatedStorage 的檔案名稱

        /// <summary>
        /// 將所有對話紀錄刪除
        /// </summary>
        public static void DeleteIsolatedStorageFile()
        {
            using (var isolatedStorage = IsolatedStorageFile.GetUserStoreForAssembly())
            {
                if (isolatedStorage.FileExists(fileName))
                {
                    isolatedStorage.DeleteFile(fileName);
                }
            }
        }

        /// <summary>
        /// 取得對談紀錄(依照 UserID)
        /// </summary>
        /// <param name="UserID"></param>
        /// <returns></returns>
        public static List<Message> GetMessagesFromIsolatedStorage(string UserID)
        {
            var messages = new List<Message>();
            using (var isolatedStorage = IsolatedStorageFile.GetUserStoreForAssembly())
            {
                if (isolatedStorage.FileExists(fileName))
                {
                    using (var fileStream = new IsolatedStorageFileStream(fileName, FileMode.Open, isolatedStorage))
                    {
                        using (var reader = new StreamReader(fileStream))
                        {
                            string line;
                            while ((line = reader.ReadLine()) != null)
                            {
                                var message = JsonConvert.DeserializeObject<Message>(line);
                                messages.Add(message);
                            }
                        }
                    }
                }
            }

            return messages.Where(c => c.UserID == UserID).OrderBy(c => c.Time).ToList();
        }

        /// <summary>
        /// 儲存對談紀錄到IsolatedStorage
        /// </summary>
        /// <param name="time"></param>
        /// <param name="userID"></param>
        /// <param name="userMessage"></param>
        /// <param name="responseMessage"></param>
        public static void SaveMessageToIsolatedStorage(
            DateTime time, string userID, string userMessage, string responseMessage)
        {
            // 建立 JSON 物件
            var messageObject = new
            {
                Time = time,
                UserID = userID,
                UserMessage = userMessage,
                ResponseMessage = responseMessage
            };
            var json = JsonConvert.SerializeObject(messageObject);

            // 讀取 Isolated Storage 中的資料
            List<string> messages = new List<string>();
            var fileName = "messages.json";

            using (var isolatedStorage = IsolatedStorageFile.GetUserStoreForAssembly())
            {
                if (isolatedStorage.FileExists(fileName))
                {
                    using (var fileStream = new IsolatedStorageFileStream(fileName, FileMode.Open, isolatedStorage))
                    {
                        using (var reader = new StreamReader(fileStream))
                        {
                            string line;
                            while ((line = reader.ReadLine()) != null)
                            {
                                messages.Add(line);
                            }
                        }
                    }
                }
            }

            // 加上新的 JSON 物件
            messages.Add(json);

            // 寫回 Isolated Storage 中
            using (var isolatedStorage = IsolatedStorageFile.GetUserStoreForAssembly())
            {
                using (var fileStream = new IsolatedStorageFileStream(fileName, FileMode.Create, isolatedStorage))
                {
                    using (var writer = new StreamWriter(fileStream))
                    {
                        foreach (var message in messages)
                        {
                            writer.WriteLine(message);
                        }
                    }
                }
            }
        }
    }
}
