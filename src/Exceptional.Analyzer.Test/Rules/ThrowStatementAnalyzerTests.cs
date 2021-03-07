using System.Threading.Tasks;
using Exceptional.Analyzer.Rules;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Testing;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using VerifyCS = Exceptional.Test.Verifiers.CSharpAnalyzerVerifier<Exceptional.Analyzer.Rules.ThrowStatementAnalyzer>;
using static Exceptional.Test.Verifiers.CSharpVerifierHelper;

namespace Exceptional.Test
{
    [TestClass]
    public class ExceptionalUnitTest
    {
        [TestMethod]
        public async Task OneExceptionIsThrownButCaught()
        {
            string source = LoadSource(ThrowStatementAnalyzer.DiagnosticId,
                nameof(OneExceptionIsThrownButCaught));

            await VerifyCS.VerifyAnalyzerAsync(source);
        }

        [TestMethod]
        public async Task OneExceptionIsThrownButDocumented()
        {
            string source = LoadSource(ThrowStatementAnalyzer.DiagnosticId,
                nameof(OneExceptionIsThrownButDocumented));

            await VerifyCS.VerifyAnalyzerAsync(source);
        }

        [TestMethod]
        public async Task OneExceptionIsThrownButSuperClassIsDocumented()
        {
            string source = LoadSource(ThrowStatementAnalyzer.DiagnosticId,
                nameof(OneExceptionIsThrownButSuperClassIsDocumented));

            await VerifyCS.VerifyAnalyzerAsync(source);
        }

        [TestMethod]
        public async Task OneExceptionIsThrownButNotHandled()
        {
            string source = LoadSource(ThrowStatementAnalyzer.DiagnosticId,
                nameof(OneExceptionIsThrownButNotHandled));

            DiagnosticResult expected = new(ThrowStatementAnalyzer.DiagnosticId, DiagnosticSeverity.Warning);
            expected = expected.WithLocation("/0/Test0.cs", 15, 13);
            expected = expected.WithMessage("System.InvalidOperationException is neither documented nor caught.");

            await VerifyCS.VerifyAnalyzerAsync(source, expected);
        }

        [TestMethod]
        public async Task OneExceptionIsThrownAndRethrownButDocumented()
        {
            string source = LoadSource(ThrowStatementAnalyzer.DiagnosticId,
                nameof(OneExceptionIsThrownAndRethrownButDocumented));

            await VerifyCS.VerifyAnalyzerAsync(source);
        }

        [TestMethod]
        public async Task OneExceptionIsThrownAndRethrownButNotDocumented()
        {
            string source = LoadSource(ThrowStatementAnalyzer.DiagnosticId,
                nameof(OneExceptionIsThrownAndRethrownButNotDocumented));

            DiagnosticResult expected = new(ThrowStatementAnalyzer.DiagnosticId, DiagnosticSeverity.Warning);
            expected = expected.WithLocation("/0/Test0.cs", 23, 17);
            expected = expected.WithMessage("System.InvalidOperationException is neither documented nor caught.");

            await VerifyCS.VerifyAnalyzerAsync(source, expected);
        }

        [TestMethod]
        public async Task OneExceptionIsThrownAndNestedRethrownButDocumented()
        {
            string source = LoadSource(ThrowStatementAnalyzer.DiagnosticId,
                nameof(OneExceptionIsThrownAndNestedRethrownButDocumented));

            await VerifyCS.VerifyAnalyzerAsync(source);
        }

        [TestMethod]
        public async Task OneExceptionIsThrownAndNestedRethrownButCaughtOnHigherLevel()
        {
            string source = LoadSource(ThrowStatementAnalyzer.DiagnosticId,
                nameof(OneExceptionIsThrownAndNestedRethrownButCaughtOnHigherLevel));

            await VerifyCS.VerifyAnalyzerAsync(source);
        }

        [TestMethod]
        public async Task OneExceptionIsThrownAndCaughtAndNewExceptionIsThrownButDocumented()
        {
            string source = LoadSource(ThrowStatementAnalyzer.DiagnosticId,
                nameof(OneExceptionIsThrownAndCaughtAndNewExceptionIsThrownButDocumented));

            await VerifyCS.VerifyAnalyzerAsync(source);
        }

        [TestMethod]
        public async Task OneExceptionIsThrownAndCaughtAndNewExceptionIsThrownButNotDocumented()
        {
            string source = LoadSource(ThrowStatementAnalyzer.DiagnosticId,
                nameof(OneExceptionIsThrownAndCaughtAndNewExceptionIsThrownButNotDocumented));

            DiagnosticResult expected = new(ThrowStatementAnalyzer.DiagnosticId, DiagnosticSeverity.Warning);
            expected = expected.WithLocation("/0/Test0.cs", 23, 17);
            expected = expected.WithMessage("System.ArgumentException is neither documented nor caught.");

            await VerifyCS.VerifyAnalyzerAsync(source, expected);
        }

        [TestMethod]
        public async Task MultipleExceptionsAreThrownButHandledDifferent()
        {
            string source = LoadSource(ThrowStatementAnalyzer.DiagnosticId,
                nameof(MultipleExceptionsAreThrownButHandledDifferent));

            await VerifyCS.VerifyAnalyzerAsync(source);
        }

        [TestMethod]
        public async Task MultipleExceptionsAreThrownButOneIsUnhandled()
        {
            string source = LoadSource(ThrowStatementAnalyzer.DiagnosticId,
                nameof(MultipleExceptionsAreThrownButOneIsUnhandled));

            DiagnosticResult expectedBase = new(ThrowStatementAnalyzer.DiagnosticId, DiagnosticSeverity.Warning);

            DiagnosticResult expected1 = expectedBase.WithLocation("/0/Test0.cs", 19, 17);
            expected1 = expected1.WithMessage("System.InvalidOperationException is neither documented nor caught.");

            DiagnosticResult expected2 = expectedBase.WithLocation("/0/Test0.cs", 36, 13);
            expected2 = expected2.WithMessage("System.ArgumentException is neither documented nor caught.");

            DiagnosticResult[] expected = {expected1, expected2};

            await VerifyCS.VerifyAnalyzerAsync(source, expected);
        }
    }
}
