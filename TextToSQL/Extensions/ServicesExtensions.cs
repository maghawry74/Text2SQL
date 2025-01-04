using System.Diagnostics.CodeAnalysis;
using Microsoft.SemanticKernel;
using TextToSQL.Tools;

namespace TextToSQL.Extensions;

public static class ServicesExtensions
{
    [Experimental("SKEXP0010")]
    public static IServiceCollection AddSemanticServices(this IServiceCollection services)
    {
        services.AddOpenAIChatCompletion(
            modelId: "modelId",
            endpoint: new Uri("URL")
        );

        services.AddKernel().Plugins.AddFromType<Text2SqlTool>();
        return services;
    }
}