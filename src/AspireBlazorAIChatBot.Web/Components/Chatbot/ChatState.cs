using Microsoft.Extensions.AI;
using System.Security.Claims;

namespace AspireApp.WebApp.Chatbot;

public class ChatState
{
    private readonly ILogger _logger;
    private readonly IChatClient _chatClient;
    private List<ChatMessage> _chatMessages;

    public List<ChatMessage> ChatMessages { get => _chatMessages; set => _chatMessages = value; }

    public ChatState(ClaimsPrincipal user, IChatClient chatClient, List<ChatMessage> chatMessages, ILogger logger)
    {
        _logger = logger; // loggerFactory.CreateLogger(typeof(ChatState));
        _chatClient = chatClient;
        ChatMessages = chatMessages;
    }

    public async Task AddUserMessageAsync(string userText, Action onMessageAdded)
    {
        ChatMessages.Add(new ChatMessage(ChatRole.User, userText));
        onMessageAdded();

        try
        {
            var result = await _chatClient.CompleteAsync(ChatMessages);
            ChatMessages.Add(new ChatMessage(ChatRole.Assistant, result.Message.Text));
        }
        catch (Exception e)
        {
            if (_logger.IsEnabled(LogLevel.Error))
            {
                _logger.LogError(e, "Error getting chat completions.");
            }
            ChatMessages.Add(new ChatMessage(ChatRole.Assistant, $"My apologies, but I encountered an unexpected error.\n{e}"));
        }
        onMessageAdded();
    }
}