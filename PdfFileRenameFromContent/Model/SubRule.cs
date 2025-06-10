using PdfFileRenameFromContent.Model;

public class SubRule
{
    public string Searchstring { get; set; }

    public string Searchtyp { get; set; } = Constants.SearchTypes.Contains; // default "contains"

    public bool IsRegEx { get; set; }

    public string Source { get; set; } = Constants.Source.Content; // "content" or "filename"

    public bool Casesensitive { get; set; }
}