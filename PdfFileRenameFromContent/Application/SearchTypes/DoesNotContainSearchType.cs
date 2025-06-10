using PdfFileRenameFromContent.Model;

namespace PdfFileRenameFromContent.Application.SearchTypes;

public class DoesNotContainSearchType : ISearchType
{
    public string Searchtype { get; } = Constants.SearchTypes.DoesNotContain;
    public bool IsMatch(string searchSource, string searchstring, bool casesensitive)
    {
        return !searchSource.Contains(searchstring, casesensitive ? StringComparison.Ordinal : StringComparison.OrdinalIgnoreCase);
    }
}