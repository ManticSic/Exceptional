using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using Exceptional.Analyzer.Helpers;
using Exceptional.Analyzer.Models;
using Exceptional.Analyzer.Models.Docs;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;

namespace Exceptional.Analyzer.Rules
{
    /// <summary>
    ///     Searching for uncaught and undocumented throw statements
    /// </summary>
    [DiagnosticAnalyzer(LanguageNames.CSharp)]
    public class ThrowStatementAnalyzer : DiagnosticAnalyzer
    {
        /// <summary>
        ///     Diagnostic id
        /// </summary>
        public const string DiagnosticId = "EX1001";

        private static readonly string Title         = Resources.EX1001_Title;
        private static readonly string MessageFormat = Resources.EX1001_Message;
        private static readonly string Description   = Resources.EX1001_Description;

        private static readonly Category Category = Category.Documentation;

        private static readonly DiagnosticDescriptor Rule = new(
            DiagnosticId,
            Title,
            MessageFormat,
            Category.DisplayName,
            DiagnosticSeverity.Warning,
            true,
            Description,
            Category.GetHelpLinkUri(DiagnosticId)
        );

        /// <inheritdoc />
        public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics => ImmutableArray.Create(Rule);

        /// <inheritdoc />
        public override void Initialize(AnalysisContext context)
        {
            context.EnableConcurrentExecution();
            context.ConfigureGeneratedCodeAnalysis(GeneratedCodeAnalysisFlags.None);

            context.RegisterSymbolAction(AnalyzeSymbol, SymbolKind.Method);
        }

        private static void AnalyzeSymbol(SymbolAnalysisContext context)
        {
            IEnumerable<SyntaxNode> syntaxNodes = context.Symbol
               .DeclaringSyntaxReferences
               .Select(syntaxReference => syntaxReference.GetSyntax(context.CancellationToken));

            DocumentationComment symbolDocumentation = XmlDoc.GetDocumentationComment(context);

            foreach (SyntaxNode syntaxNode in syntaxNodes)
            {
                if (!(syntaxNode is MethodDeclarationSyntax methodDeclarationSyntax))
                {
                    continue;
                }

                SemanticModel semanticModel = context.Compilation.GetSemanticModel(syntaxNode.SyntaxTree);
                ThrowStatementSyntax[] throwStatements = FindStatementSyntax
                   .FindThrowStatements(methodDeclarationSyntax.Body.Statements)
                   .ToArray();

                foreach (ThrowStatementSyntax throwStatement in throwStatements)
                {
                    AnalyzeThrowStatement(context, throwStatement, semanticModel, symbolDocumentation);
                }
            }
        }

        private static void AnalyzeThrowStatement(SymbolAnalysisContext context,
                                                  ThrowStatementSyntax throwStatement,
                                                  SemanticModel semanticModel,
                                                  DocumentationComment symbolDocumentation)
        {
            ThrowStatementAnalysisData analysisData = ThrowStatementAnalysisData.Create(throwStatement, semanticModel);

            bool isCaught     = IsExceptionCaught(analysisData, semanticModel);
            bool isDocumented = IsExceptionDocumented(context, analysisData, symbolDocumentation);

            if (isCaught || isDocumented)
            {
                return;
            }

            Diagnostic diagnostic = Diagnostic.Create(Rule, throwStatement.GetLocation(), analysisData.ThrownType);
            context.ReportDiagnostic(diagnostic);
        }

        private static bool IsExceptionCaught(ThrowStatementAnalysisData analysisData,
                                              SemanticModel semanticModel)
        {
            SyntaxNode currentParent = analysisData.ThrowStatement.Parent;

            while (currentParent != null)
            {
                if (currentParent is TryStatementSyntax tryStatement)
                {
                    bool thrownTypeIsCaught = tryStatement.Catches
                       .Where(catchClause => catchClause != analysisData.RethrowOf)
                       .Select(catchClause => semanticModel.GetTypeInfo(catchClause.Declaration.Type).Type)
                       .Any(caughtType =>
                            SymbolEqualityComparer.IncludeNullability.Equals(caughtType, analysisData.ThrownType));

                    if (thrownTypeIsCaught)
                    {
                        return true;
                    }
                }

                currentParent = currentParent.Parent;
            }

            return false;
        }

        private static bool IsExceptionDocumented(SymbolAnalysisContext context,
                                                  ThrowStatementAnalysisData analysisData,
                                                  DocumentationComment documentationComment)
        {
            if (documentationComment.Exceptions == null)
            {
                return false;
            }

            IEnumerable<ITypeSymbol> thrownBaseTypes = GetAllTypesOf(analysisData.ThrownType);

            return documentationComment.Exceptions
               .Where(exceptionDocumentationComment => exceptionDocumentationComment?.Type != null)
               .Select(exceptionDocumentationComment => exceptionDocumentationComment.Type?.Substring(2))
               .Select(documentedExceptionTypeName =>
                    context.Compilation.GetTypeByMetadataName(documentedExceptionTypeName))
               .Any(documentedExceptionType => thrownBaseTypes.Contains(documentedExceptionType));
        }

        private static ImmutableArray<ITypeSymbol> GetAllTypesOf(ITypeSymbol? superset)
        {
            IList<ITypeSymbol> result = new List<ITypeSymbol>();

            ITypeSymbol? type = superset;

            while (type != null)
            {
                result.Add(type);
                type = type.BaseType;
            }

            return result.ToImmutableArray();
        }
    }
}
