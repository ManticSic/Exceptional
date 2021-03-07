using System;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Exceptional.Analyzer.Models
{
    internal class ThrowStatementAnalysisData
    {
        private ThrowStatementAnalysisData(ThrowStatementSyntax throwStatement,
                                           ITypeSymbol thrownType,
                                           CatchClauseSyntax? rethrowOf)
        {
            ThrowStatement = throwStatement;
            ThrownType     = thrownType;
            RethrowOf      = rethrowOf;
        }

        public ThrowStatementSyntax ThrowStatement { get; }

        public ITypeSymbol ThrownType { get; }

        public CatchClauseSyntax? RethrowOf { get; }

        public static ThrowStatementAnalysisData Create(ThrowStatementSyntax throwStatement,
                                                        SemanticModel semanticModel)
        {
            (ITypeSymbol, CatchClauseSyntax?) information = GetThrowInformation(throwStatement, semanticModel);
            ITypeSymbol                       thrownType  = information.Item1;
            CatchClauseSyntax?                rethrowOf   = information.Item2;

            return new ThrowStatementAnalysisData(throwStatement, thrownType, rethrowOf);
        }

        private static (ITypeSymbol, CatchClauseSyntax?) GetThrowInformation(ThrowStatementSyntax throwStatement,
                                                                             SemanticModel semanticModel)
        {
            ITypeSymbol?       thrownType = null;
            CatchClauseSyntax? rethrowOf  = null;

            if (throwStatement.Expression != null)
            {
                thrownType = semanticModel.GetTypeInfo(throwStatement.Expression)
                   .Type;
            }
            else
            {
                SyntaxNode parent = throwStatement.Parent;

                while (parent != null)
                {
                    if (parent is CatchClauseSyntax catchClause)
                    {
                        rethrowOf  = catchClause;
                        thrownType = semanticModel.GetTypeInfo(catchClause.Declaration.Type).Type;
                    }

                    parent = parent.Parent;
                }
            }

            if (thrownType == null)
            {
                throw new InvalidOperationException();
            }

            return (thrownType, rethrowOf);
        }
    }
}
