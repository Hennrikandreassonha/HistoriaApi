using OpenAI.ObjectModels.RequestModels;

namespace HistoriaApi.Service;

    public class AiService
    {
        public string ApiKey { get; set; }
        public OpenAI.Managers.OpenAIService ChatService { get; set; }
        public AiService()
        {
        }
        public AiService(string apiKey)
        {
            ApiKey = apiKey;
            ChatService = new OpenAI.Managers.OpenAIService(new OpenAI.OpenAiOptions()
            {
                ApiKey = ApiKey
            });
        }
        public async Task<string> SendQuestion(string message)
        {
            var completionResult = await ChatService.ChatCompletion.CreateCompletion(
            new ChatCompletionCreateRequest
            {
                Messages = new List<ChatMessage>
                {
                new("assistant", "Tell me if this is a subject that i can write historic articles about. If it is for example something silly then answer no. Only answer Yes or No"),
                new("user", message),
                },
                Model = OpenAI.ObjectModels.Models.Gpt_3_5_Turbo,
                Temperature = 0.3F,
                MaxTokens = 1000,
                N = 1
            });

            if (completionResult.Successful)
            {
                return completionResult.Choices[0].Message.Content;
            }
            else
            {
                if (completionResult.Error == null)
                {
                    throw new Exception("Unknown Error");
                }
                Console.WriteLine($"{completionResult.Error.Code}: {completionResult.Error.Message}");
            }

            return "";
        }
        public void ClearList(string filePath)
        {
            var emptyList = new List<string>();
            File.WriteAllLines(filePath, emptyList);
        }
        public void AddSubjectsToList(string filePath, List<string> newSubjects)
        {
            var subjectSkips = File.ReadAllLines(filePath).ToList();
            subjectSkips.AddRange(newSubjects);
            File.WriteAllLines(filePath, subjectSkips);
        }

        public string GetSubject(string filePath)
        {
            var subjectSkips = File.ReadAllLines(filePath).ToList();
            string subject = subjectSkips.First();
            subjectSkips.RemoveAt(0);
            File.WriteAllLines(filePath, subjectSkips);
            return subject;
        }
    }
