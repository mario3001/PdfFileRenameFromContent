namespace PdfFileRenameFromContent.Model;

public static class Constants
{
    public static class Conditions
    {
        public const string All = "all";
        public const string Any = "any";
    }

    public static class SearchTypes
    {
        public const string Contains = "contains";
        public const string DoesNotContain = "does not contain";
        public const string StartsWith = "starts with";
    }

    public static class Source
    {
        public const string Content = "content";
        public const string Filename = "filename";
    }
}