using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PdfFileRenameFromContent.Application;
using PdfFileRenameFromContent.DependencyInjection;

namespace PdfFileRenameFromContent.Executable
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var configuration = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: false)
                .Build();

            var settings = configuration.Get<SettingsOptions>()!;

            var rulesFilePath = settings.RulesFilePath;
            var sourceFilePath = settings.SourceFilePath;
            var destinationFilePath = settings.DestinationFilePath;

            var serviceProvider = RuleServiceCollectionRegistrations.BuildServiceProvider();

            var scope = serviceProvider.CreateScope();

            var pdfFileSearcher = scope.ServiceProvider.GetService<PdfFileSearcher>()!;
            var renameProcessor = scope.ServiceProvider.GetService<RenameProcessor>()!;

            var pdfFilePaths = pdfFileSearcher.GetFilesToProcess(sourceFilePath);

            foreach (var filePath in pdfFilePaths)
            {
                await renameProcessor.Run(rulesFilePath, filePath, destinationFilePath);
            }

            Console.WriteLine("Exit");
            Console.ReadKey();
        }
    }
}