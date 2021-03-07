using System;
using System.Collections.Immutable;
using System.IO;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;

namespace Exceptional.Test.Verifiers
{
    internal static class CSharpVerifierHelper
    {
        /// <summary>
        ///     By default, the compiler reports diagnostics for nullable reference types at
        ///     <see cref="DiagnosticSeverity.Warning" />, and the analyzer test framework defaults to only validating
        ///     diagnostics at <see cref="DiagnosticSeverity.Error" />. This map contains all compiler diagnostic IDs
        ///     related to nullability mapped to <see cref="ReportDiagnostic.Error" />, which is then used to enable all
        ///     of these warnings for default validation during analyzer and code fix tests.
        /// </summary>
        internal static ImmutableDictionary<string, ReportDiagnostic> NullableWarnings { get; } =
            GetNullableWarningsFromCompiler();

        /// <summary>
        ///     Load source files from assets directory.
        /// </summary>
        /// <param name="ruleId">Id of the exceptional id.</param>
        /// <param name="testName">Name of the test case.</param>
        /// <returns>Code of the test case.</returns>
        internal static string LoadSource(string ruleId, string testName)
        {
            string fileName = $"./Assets/{ruleId}/{testName}.cs";

            return File.ReadAllText(fileName);
        }

        private static ImmutableDictionary<string, ReportDiagnostic> GetNullableWarningsFromCompiler()
        {
            string[] args = {"/warnaserror:nullable"};
            CSharpCommandLineArguments commandLineArguments =
                CSharpCommandLineParser.Default.Parse(args, Environment.CurrentDirectory, Environment.CurrentDirectory);
            ImmutableDictionary<string, ReportDiagnostic> nullableWarnings =
                commandLineArguments.CompilationOptions.SpecificDiagnosticOptions;

            // Workaround for https://github.com/dotnet/roslyn/issues/41610
            nullableWarnings = nullableWarnings
               .SetItem("CS8632", ReportDiagnostic.Error)
               .SetItem("CS8669", ReportDiagnostic.Error);

            return nullableWarnings;
        }
    }
}
