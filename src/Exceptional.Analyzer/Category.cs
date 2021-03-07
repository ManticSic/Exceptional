using System.Collections.Generic;
using Exceptional.Analyzer.Rules;

namespace Exceptional.Analyzer
{
    internal class Category
    {
        private const string RulesDocumentUri = "https://github.com/ManticSic";

        public static readonly Category Documentation = new("Documentation");

        private static readonly Dictionary<string, string> IdToAnchorMap = new()
        {
            {
                ThrowStatementAnalyzer.DiagnosticId, "ex1001--thrown-exception-is-neither-documented-nor-caught"
            },
        };

        private Category(string displayName)
        {
            DisplayName = displayName;
        }

        public string DisplayName { get; }

        public string GetHelpLinkUri(string ruleId)
        {
            string anchor = IdToAnchorMap[ruleId];

            return $"{RulesDocumentUri}#{anchor}";
        }
    }
}
