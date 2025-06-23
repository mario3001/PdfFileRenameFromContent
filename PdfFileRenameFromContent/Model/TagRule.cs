public class TagRule
{
    public string TagId { get; set; }
    public string TagCategory { get; set; }
    public string Tagname { get; set; }

    public string TargetFolder { get; set; }
    
    //public string Postscript { get; set; }
    //public bool MultilineRegex { get; set; }
    public string Condition { get; set; } = "any"; // any, all
    public List<SubRule> Subrules { get; set; }
}