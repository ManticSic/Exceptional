using System.Threading;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Diagnostics;
using Microsoft.CodeAnalysis.Testing;
using Microsoft.CodeAnalysis.Testing.Verifiers;
using Microsoft.CodeAnalysis.VisualBasic.Testing;

namespace Exceptional.Test.Verifiers
{
    public static class VisualBasicAnalyzerVerifier<TAnalyzer> where TAnalyzer : DiagnosticAnalyzer, new()
    {
        /// <inheritdoc cref="AnalyzerVerifier{TAnalyzer, TTest, TVerifier}.Diagnostic()" />
        public static DiagnosticResult Diagnostic()
        {
            return VisualBasicAnalyzerVerifier<TAnalyzer, MSTestVerifier>.Diagnostic();
        }

        /// <inheritdoc cref="AnalyzerVerifier{TAnalyzer, TTest, TVerifier}.Diagnostic(string)" />
        public static DiagnosticResult Diagnostic(string diagnosticId)
        {
            return VisualBasicAnalyzerVerifier<TAnalyzer, MSTestVerifier>.Diagnostic(diagnosticId);
        }

        /// <inheritdoc cref="AnalyzerVerifier{TAnalyzer, TTest, TVerifier}.Diagnostic(DiagnosticDescriptor)" />
        public static DiagnosticResult Diagnostic(DiagnosticDescriptor descriptor)
        {
            return VisualBasicAnalyzerVerifier<TAnalyzer, MSTestVerifier>.Diagnostic(descriptor);
        }

        /// <inheritdoc cref="AnalyzerVerifier{TAnalyzer, TTest, TVerifier}.VerifyAnalyzerAsync(string, DiagnosticResult[])" />
        public static async Task VerifyAnalyzerAsync(string source, params DiagnosticResult[] expected)
        {
            Test test = new()
            {
                TestCode = source,
            };

            test.ExpectedDiagnostics.AddRange(expected);
            await test.RunAsync(CancellationToken.None);
        }

        public class Test : VisualBasicAnalyzerTest<TAnalyzer, MSTestVerifier>
        {
        }
    }
}
