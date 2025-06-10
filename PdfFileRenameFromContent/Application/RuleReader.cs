using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

namespace PdfFileRenameFromContent.Application;

public class RuleReader
{
    public async Task<ICollection<TagRule>> ReadRulesAsync(string[] filePaths)
    {
        var deserializer = new DeserializerBuilder()
            .WithNamingConvention(CamelCaseNamingConvention.Instance)
            .IgnoreUnmatchedProperties()
            .Build();

        var rules = new List<TagRule>();

        foreach (var filePath in filePaths)
        {
            var yamlContent = await File.ReadAllTextAsync(filePath);
            var fileRules = deserializer.Deserialize<Dictionary<string, TagRule>>(yamlContent);

            foreach (var fileRule in fileRules)
            {
                fileRule.Value.TagCategory = fileRule.Key.Contains("Sender") ? "Sender" : "Other";
                fileRule.Value.TagId = fileRule.Key;
                rules.Add(fileRule.Value);
            }
        }

        return rules;
    }
}