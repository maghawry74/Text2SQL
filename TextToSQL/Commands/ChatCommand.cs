using MediatR;
using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.ChatCompletion;
using Microsoft.SemanticKernel.Connectors.OpenAI;

namespace TextToSQL.Commands;

public class ChatCommand : IRequest<object>
{
    public string Prompt { get; set; } = null!;
}

public class ChatCommandHandler : IRequestHandler<ChatCommand, object>
{
    private readonly IChatCompletionService _chatCompletionService;
    private readonly Kernel _kernel;

    public ChatCommandHandler(IChatCompletionService chatCompletionService,Kernel kernel)
    {
        _chatCompletionService = chatCompletionService;
        _kernel = kernel;
    }
    public async Task<object> Handle(ChatCommand request, CancellationToken cancellationToken)
    {
        var chatHistory = new ChatHistory();
        chatHistory.AddSystemMessage("You are helpful assistant that take user input and make a sql query.");
        chatHistory.AddUserMessage(request.Prompt);

        var options = new OpenAIPromptExecutionSettings
        {
            Seed = 42,
            Temperature = .1,
            FunctionChoiceBehavior = FunctionChoiceBehavior.Auto()
        };

        var response = await _chatCompletionService.GetChatMessageContentAsync(chatHistory, options, _kernel, cancellationToken);
        
        return new {Result = response.Content};
    }
}