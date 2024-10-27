using Microsoft.Extensions.AI;
using System.Security.Claims;
using System.Text;

namespace AspireApp.WebApp.Chatbot;

public class ChatState
{
    private readonly ILogger _logger;
    private readonly IChatClient _chatClient;
    private List<ChatMessage> _chatMessages;

    public List<ChatMessage> ChatMessages { get => _chatMessages; set => _chatMessages = value; }

    public ChatState(ClaimsPrincipal user, IChatClient chatClient, List<ChatMessage> chatMessages, ILogger logger)
    {
        _logger = logger;
        _chatClient = chatClient;
        ChatMessages = chatMessages;
    }

    public async Task AddUserMessageAsync(string userText, Action onMessageAdded)
    {
        ChatMessages.Add(new ChatMessage(ChatRole.User, userText));
        onMessageAdded();

        try
        {
            _logger.LogInformation("Sending message to chat client.");
            _logger.LogInformation($"user Text: {userText}");

            var result = await _chatClient.CompleteAsync(ChatMessages);
            ChatMessages.Add(new ChatMessage(ChatRole.Assistant, result.Message.Text));
            
            _logger.LogInformation($"Assistant Response: {result.Message.Text}");
        }
        catch (Exception e)
        {
            if (_logger.IsEnabled(LogLevel.Error))
            {
                _logger.LogError(e, "Error getting chat completions.");
            }

            // format the exception using HTML to show the exception details in a chat panel as response
            ChatMessages.Add(new ChatMessage(ChatRole.Assistant, $"My apologies, but I encountered an unexpected error.\n\n<p style=\"color: red\">{e}</p>"));
        }
        onMessageAdded();
    }
}