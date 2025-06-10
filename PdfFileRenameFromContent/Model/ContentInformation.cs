namespace PdfFileRenameFromContent.Model;

public class ContentInformation
{
    public string Date { get; set; }
    public ICollection<TagRule> MatchingTagRules { get; set; }
}