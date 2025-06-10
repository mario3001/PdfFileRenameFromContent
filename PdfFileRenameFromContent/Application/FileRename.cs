using System.Diagnostics;

namespace PdfFileRenameFromContent.Application;

public class FileRename
{
    private const string LfCr = "\r\n";

    public void RenameAndMove(string oldFilePath, string newFileName, string destinationFolderPath)
    {
        var minLengthDate = 10;
        var minLengthWithDate = minLengthDate + 2;
        if (!string.IsNullOrEmpty(newFileName) && newFileName.Length > minLengthWithDate)
        {
            Console.WriteLine($"neu: {newFileName,-40}\r\nalt: {Path.GetFileName(oldFilePath)}");
            Console.Write("Rename?");
            var key = Console.ReadKey();
            if (key.KeyChar == 'y' || key.KeyChar == 'j')
                RenameAndMoveFile(oldFilePath, newFileName, destinationFolderPath);

            Console.WriteLine(LfCr);
        }
        else if (!string.IsNullOrEmpty(newFileName) && !newFileName.StartsWith("0001-01-01"))
        {
            var oldFileName = Path.GetFileName(oldFilePath);
            var newFileName2 = oldFileName + " " + newFileName.Substring(0, 10) + " " + Path.GetExtension(oldFileName);
            Console.WriteLine($"neu: {newFileName2,-40}\r\nalt: {oldFileName}");
            Console.Write("Rename?");
            var key = Console.ReadKey();
            if (key.KeyChar == 'y' || key.KeyChar == 'j')
            {
                RenameAndMoveFile(oldFilePath, newFileName2, destinationFolderPath);
                Process.Start($"\"{destinationFolderPath}\\{newFileName2}\"");
            }

            Console.WriteLine(LfCr);
        }
        else
        {
            var oldFileName = Path.GetFileName(oldFilePath);
            Console.WriteLine("Nothing done: " + oldFileName);
            Console.WriteLine(LfCr);
        }
    }

    private static void RenameAndMoveFile(string oldFilePath, string newFileName, string destinationFilePath)
    {
        var newFilePath = $"{destinationFilePath}\\{newFileName}";
        var i = 2;
        while (File.Exists(newFilePath)) newFilePath = newFilePath.Replace(".pdf", $"({i++}).pdf");
        File.Move(oldFilePath, newFilePath);

        Console.WriteLine($"Renamed to: {newFilePath}");
    }
}