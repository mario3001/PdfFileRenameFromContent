using System.Text.RegularExpressions;
using PdfFileRenameFromContent.Application.SearchTypes;
using PdfFileRenameFromContent.Model;

namespace PdfFileRenameFromContent.Application;

public class FileContentRuleSearch(IEnumerable<ISearchType> searchTypes)
{
    public bool SearchFileAsync(string filePath, string content, TagRule rule)
    {
        return TagRuleIsMatch(filePath, content, rule);
    }

    private bool TagRuleIsMatch(string filePath, string content, TagRule rule)
    {
        var filename = Path.GetFileName(filePath);

        var matchesAll = rule.Condition.ToLower() == Constants.Conditions.All; // Default is "any"

        var hasMatch = false;

        foreach (var subRule in rule.Subrules)
        {
            var subRuleIsMatch = SubRuleIsMatch(filename, content, subRule);

            if (matchesAll && !subRuleIsMatch)
            {
                return false;
            }
            else if (!matchesAll && subRuleIsMatch)
            {
                return true;
            }

            hasMatch |= subRuleIsMatch;
        }

        return matchesAll && hasMatch;
    }

    private bool SubRuleIsMatch(string filePath, string content, SubRule subRule)
    {
        bool isMatch;

        var searchSource = subRule.Source.ToLower() == Constants.Source.Filename ? Path.GetFileName(filePath) : content;

        if (subRule.IsRegEx)
        {
            isMatch = Regex.IsMatch(content, subRule.Searchstring);
        }
        else
        {

            var searchType = searchTypes.SingleOrDefault(x => string.Equals(x.Searchtype, subRule.Searchtyp, StringComparison.CurrentCultureIgnoreCase));

            if (searchType == null)
            {
                Console.WriteLine($"SearchType not found: '{subRule.Searchtyp}'");
                return false;
            }

            isMatch = searchType.IsMatch(searchSource, subRule.Searchstring, subRule.Casesensitive);
        }

        return isMatch;
    }
}