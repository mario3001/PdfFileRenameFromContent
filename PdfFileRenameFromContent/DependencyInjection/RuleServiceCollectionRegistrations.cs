using Microsoft.Extensions.DependencyInjection;
using PdfFileRenameFromContent.Application;
using PdfFileRenameFromContent.Application.SearchTypes;

namespace PdfFileRenameFromContent.DependencyInjection;

public static class RuleServiceCollectionRegistrations
{
    public static IServiceCollection RegisterRuleServices(this IServiceCollection services)
    {
        services.AddTransient<DateExtractor>();

        services.AddTransient<FileContentRuleSearch>();
        services.AddTransient<FilenameForTags>();
        services.AddTransient<FileRename>();
        services.AddTransient<PdfFileProcessor>();
        services.AddTransient<PdfFileSearcher>();
        services.AddTransient<RuleReader>();
        services.AddTransient<RenameProcessor>();

        services.AddTransient<ISearchType, ContainsSearchType>();
        services.AddTransient<ISearchType, DoesNotContainSearchType>();
        services.AddTransient<ISearchType, StartsWithSearchType>();

        return services;
    }

    public static ServiceProvider BuildServiceProvider()
    {
        var serviceProvider = new ServiceCollection()
            .RegisterRuleServices()
            .BuildServiceProvider();

        return serviceProvider;
    }
}