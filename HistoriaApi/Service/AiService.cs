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
                new("assistant", $"Can {message} be considered a significant historical subject for articles that focus on well-documented events or figures? Answer only Yes or No."),
                new("user", message),
            },
            Model = OpenAI.ObjectModels.Models.Gpt_3_5_Turbo,
            Temperature = 0.0F,
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
}
