using PdfFileRenameFromContent.Model;

namespace PdfFileRenameFromContent.Application.SearchTypes;

public class ContainsSearchType : ISearchType
{
    public string Searchtype { get; } = Constants.SearchTypes.Contains;

    public bool IsMatch(string searchSource, string searchstring, bool casesensitive)
    {
        return searchSource.Contains(searchstring, casesensitive ? StringComparison.Ordinal : StringComparison.OrdinalIgnoreCase);
    }
}