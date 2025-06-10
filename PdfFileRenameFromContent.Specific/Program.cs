using Microsoft.Extensions.Configuration;
using PdfFileRenameFromContent.DependencyInjection;

using Microsoft.Extensions.DependencyInjection;

namespace PdfFileRenameFromContent.Specific
{
    public class Program
    {
        static async Task Main(string[] args)
        {
            var configuration = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: false)
                .Build();

            var settings = configuration.Get<SettingsOptions>()!;

            var rulesFilePath = settings.RulesFilePath;

            var serviceProvider = RuleServiceCollectionRegistrations.BuildServiceProvider();

            var scope = serviceProvider.CreateScope();

            var renameProcessor = scope.ServiceProvider.GetService<RenameProcessor>()!;

            Console.WriteLine("FilePath:");

            var hasArguments = args.Length > 0;
            string file = hasArguments ? args[0] : string.Empty;

            if (hasArguments)
            {
                Console.WriteLine(file);
                await RenameFile(file, renameProcessor, rulesFilePath);
            }
            else
            {


                while ((file = Console.ReadLine()?.Trim()) != string.Empty)
                {
                    if (file != null)
                    {
                        try
                        {
                            await RenameFile(file, renameProcessor, rulesFilePath);
                        }
                        catch (Exception e)
                        {
                            Console.WriteLine($"Error with: {file}\r\n\t" + e.ToString().Replace("\r\n", "\r\n\t"));
                        }

                    }

                    Console.WriteLine();
                    Console.WriteLine("FilePath:");
                }
            }

            Console.WriteLine("Exit");
        }

        private static async Task RenameFile(string file, RenameProcessor renameProcessor, string[] rulesFilePath)
        {
            file = file.Trim('"');
            var cleanedFilePath = file.Replace("\"", string.Empty);
            var folderPath = Path.GetDirectoryName(cleanedFilePath);

            await renameProcessor.Run(rulesFilePath, cleanedFilePath, folderPath);
        }
    }
}