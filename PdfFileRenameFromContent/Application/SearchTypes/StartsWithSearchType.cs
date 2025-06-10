using PdfFileRenameFromContent.Model;

namespace PdfFileRenameFromContent.Application.SearchTypes;

public class StartsWithSearchType : ISearchType
{
    public string Searchtype { get; } = Constants.SearchTypes.StartsWith;
    public bool IsMatch(string searchSource, string searchstring, bool casesensitive)
    {
        return searchSource.StartsWith(searchstring, casesensitive ? StringComparison.Ordinal : StringComparison.OrdinalIgnoreCase);
    }
}