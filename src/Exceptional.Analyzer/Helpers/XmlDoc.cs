using System;
using System.IO;
using System.Xml.Serialization;
using Exceptional.Analyzer.Models.Docs;
using Microsoft.CodeAnalysis.Diagnostics;

namespace Exceptional.Analyzer.Helpers
{
    internal static class XmlDoc
    {
        internal static DocumentationComment GetDocumentationComment(SymbolAnalysisContext context)
        {
            string documentationCommentXml = context.Symbol.GetDocumentationCommentXml();

            return String.IsNullOrWhiteSpace(documentationCommentXml)
                ? new DocumentationComment()
                : Deserialize<DocumentationComment>(context.Symbol.GetDocumentationCommentXml());
        }

        private static T Deserialize<T>(string xml) where T : new()
        {
            XmlSerializer serializer = new(typeof(T));

            using TextReader reader = new StringReader(xml);
            return (T) serializer.Deserialize(reader);
        }
    }
}
