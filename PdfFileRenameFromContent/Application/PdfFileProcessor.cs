using iText.Kernel.Pdf;
using iText.Kernel.Pdf.Canvas.Parser;
using iText.Kernel.Pdf.Canvas.Parser.Listener;
using PdfFileRenameFromContent.Model;

namespace PdfFileRenameFromContent.Application;

public class PdfFileProcessor(DateExtractor dateExtractor, FileContentRuleSearch fileContentRuleSearch)
{
    public Task<ContentInformation> SearchMatchingFileTagsAsync(string filePath, ICollection<TagRule> rules)
    {
        var contentOfPage1 = ExtractTextOfPage(filePath, 1);

        if (string.IsNullOrWhiteSpace(contentOfPage1))
        {
            return Task.FromResult<ContentInformation>(null);
        }

        var date = dateExtractor.GetDate(contentOfPage1, (pageNum) => ExtractTextOfPage(filePath, pageNum));

        var matchingContentTagRules = new List<TagRule>();
        var ruleCategories = rules.Select(x => x.TagCategory).Distinct();
        foreach (var ruleCategory in ruleCategories)
        {
            var matchingContentCategoryTagRules = rules.Where(x => x.TagCategory == ruleCategory && fileContentRuleSearch.SearchFileAsync(filePath, contentOfPage1, x)).ToList();

            var nextPage = 2;
            while (matchingContentCategoryTagRules.Count == 0 && nextPage <= 3)
            {
                var contentOfNextPage = ExtractTextOfPage(filePath, nextPage);
                matchingContentCategoryTagRules.AddRange(rules.Where(x => x.TagCategory == ruleCategory && fileContentRuleSearch.SearchFileAsync(filePath, contentOfNextPage, x)).ToList());

                nextPage += 1;
            }

            matchingContentTagRules.AddRange(matchingContentCategoryTagRules);
        }

        var contentInformation = new ContentInformation
        {
            Date = $"{date:yyyy-MM-dd}",
            MatchingTagRules = matchingContentTagRules
        };

        return Task.FromResult(contentInformation);
    }

    private static string ExtractTextOfPage(string filename, int pageNum = 1)
    {
        using var reader = new PdfReader(filename);

        var document = new PdfDocument(reader);

        if (pageNum > document.GetNumberOfPages())
        {
            return string.Empty;
        }

        var page = document.GetPage(pageNum);
        var txt = PdfTextExtractor.GetTextFromPage(page, new LocationTextExtractionStrategy());
        return txt;
    }
}