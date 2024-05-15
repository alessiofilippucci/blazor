namespace BlazorServer
{
    public static class Roles
    {
        public const string Admin = "Admin";
        public const string User = "User";

        public const string Abbonato = "Abbonato";
        public const string Private = "Private";
        public const string MarketReport = "MarketReport";

        private static readonly Dictionary<string, List<string>> codeRoles = new Dictionary<string, List<string>>()
        {
            {"MF", new List<string>() { Abbonato, MarketReport } },
            {"IO", new List<string>() { Abbonato} }
        };

        public static List<string> GetRoleByCode(string code) => codeRoles.ContainsKey(code) ? codeRoles[code] : [];

        public static string BuildAutorization(params string[] roles) => string.Join(",", roles);
    }
}
