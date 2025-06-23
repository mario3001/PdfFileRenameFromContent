using PdfFileRenameFromContent.Application;

namespace PdfFileRenameFromContent;

public class RenameProcessor(RuleReader ruleReader, PdfFileProcessor pdfFileProcessor, FilenameForTags filenameForTags, FileRename fileRename)
{
    private ICollection<TagRule> _rules;

    public async Task Run(string[] rulesFilePaths, string filePath, string targetFolderPath)
    {
        var rules = await GetRules(rulesFilePaths);

        var fileContentInformation = await pdfFileProcessor.SearchMatchingFileTagsAsync(filePath, rules);
        if (!fileContentInformation.MatchingTagRules.Any())
        {
            Console.WriteLine("Nothing found in file");
        }
        else
        {
            var newFileName = filenameForTags.GetFilenameByTags(filePath, fileContentInformation);

            var targetFolderPathByTags = filenameForTags.GetTargetFolderByTags(fileContentInformation);
            if (!string.IsNullOrEmpty(targetFolderPathByTags))
            {
                targetFolderPath = Path.Combine(targetFolderPath, targetFolderPathByTags);
            }

            fileRename.RenameAndMove(filePath, newFileName, targetFolderPath);
        }
    }

    private async Task<ICollection<TagRule>> GetRules(string[] rulesFilePaths)
    {
        return _rules = await ruleReader.ReadRulesAsync(rulesFilePaths);
    }
}