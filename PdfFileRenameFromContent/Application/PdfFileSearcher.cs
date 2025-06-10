namespace PdfFileRenameFromContent.Application;

public class PdfFileSearcher
{
    public string[] GetFilesToProcess(string sourceFilePath)
    {
        var files = Directory.GetFiles(sourceFilePath, "*.pdf", SearchOption.TopDirectoryOnly);
        return files;
    }
}