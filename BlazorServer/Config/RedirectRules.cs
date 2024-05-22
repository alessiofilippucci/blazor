namespace BlazorServer.Config
{
    public class RedirectRules
    {
        public Rule? Rule1 { get; set; }
        public Rule? Rule2 { get; set; }
        public Rule? Rule3 { get; set; }
    }

    public class Rule
    {
        public string? Regex { get; set; }
        public string? Replacement { get; set; }
        public bool SkipRemainingRules { get; set; }
    }
}