using System.Collections.Generic;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Exceptional.Analyzer.Helpers
{
    internal static class FindStatementSyntax
    {
        internal static IEnumerable<ThrowStatementSyntax> FindThrowStatements(IEnumerable<SyntaxNode> statements)
        {
            SyntaxNode[]               statementArray = statements.ToArray();
            List<ThrowStatementSyntax> result         = new();

            foreach (SyntaxNode statement in statementArray)
            {
                if (statement is ThrowStatementSyntax throwStatement)
                {
                    result.Add(throwStatement);
                }
                else
                {
                    IEnumerable<SyntaxNode> childStatements = statement.ChildNodes();

                    IEnumerable<ThrowStatementSyntax> childResults = FindThrowStatements(childStatements);

                    result.AddRange(childResults);
                }
            }

            return result;
        }
    }
}
