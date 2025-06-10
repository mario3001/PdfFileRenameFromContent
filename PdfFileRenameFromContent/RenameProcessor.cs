using PdfFileRenameFromContent.Application;

namespace PdfFileRenameFromContent;

public class RenameProcessor(RuleReader ruleReader, PdfFileProcessor pdfFileProcessor, FilenameForTags filenameForTags, FileRename fileRename)
{
    private ICollection<TagRule> _rules;

    public async Task Run(string[] rulesFilePath, string filePath, string destinationFilePath)
    {
        var rules = await GetRules(rulesFilePath);

        var fileContentInformation = await pdfFileProcessor.SearchMatchingFileTagsAsync(filePath, rules);
        if (!fileContentInformation.MatchingTagRules.Any())
        {
            Console.WriteLine("Nothing found in file");
        }
        else
        {
            var newFileName = filenameForTags.GetFilenameByTags(filePath, fileContentInformation);
            fileRename.RenameAndMove(filePath, newFileName, destinationFilePath);
        }
    }

    private async Task<ICollection<TagRule>> GetRules(string[] rulesFilePath)
    {
        return _rules = await ruleReader.ReadRulesAsync(rulesFilePath);
    }
}