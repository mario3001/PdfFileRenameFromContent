using PdfFileRenameFromContent.Model;

namespace PdfFileRenameFromContent.Application;

public class FilenameForTags
{
    public string GetFilenameByTags(string file, ContentInformation contentInformation)
    {
        var fileExtension = Path.GetExtension(file);
        var newFileNameChars = contentInformation.MatchingTagRules
            .OrderBy(x => x.TagId)
            .SelectMany(x => x.Tagname)
            .ToArray();

        var newFileName = new string(newFileNameChars.ToArray());

        var filenameByTags = $"{contentInformation.Date} {newFileName}{fileExtension}";

        return CleanFileName(filenameByTags);
    }

    public string GetTargetFolderByTags(ContentInformation contentInformation)
    {
        var targetFolder = string.Join("", contentInformation.MatchingTagRules
        .OrderBy(x => x.TagId)
        .Select(x => x.TargetFolder));

        if (string.IsNullOrEmpty(targetFolder))
        {
            return string.Empty;
        }

        return targetFolder;
    }

    private static string CleanFileName(string filename)
    {
        return filename
            .Replace(':', ' ')
            .Replace('/', '-')
            .Replace('\'', ' ')
            .Replace('\"', '-');
    }
}