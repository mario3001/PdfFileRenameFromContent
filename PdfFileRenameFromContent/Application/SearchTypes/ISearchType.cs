namespace PdfFileRenameFromContent.Application.SearchTypes;

public interface ISearchType
{
    string Searchtype { get; }
    bool IsMatch(string searchSource, string searchstring, bool casesensitive);
}