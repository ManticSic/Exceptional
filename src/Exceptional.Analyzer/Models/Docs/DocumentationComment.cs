using System.Xml.Serialization;

namespace Exceptional.Analyzer.Models.Docs
{
    /// <summary>
    ///     Simpel XMLDoc model
    /// </summary>
    [XmlRoot("member")]
    public class DocumentationComment
    {
        private string? summary;

        /// <summary>
        ///     XMLDoc summary
        /// </summary>
        [XmlElement("summary")]
        public string? Summary
        {
            get => summary;
            set => summary = value?.Trim();
        }

        /// <summary>
        ///     XMLDoc exceptions
        /// </summary>
        [XmlElement("exception")]
        public ExceptionDocumentationComment[]? Exceptions { get; set; } = new ExceptionDocumentationComment[0];
    }
}
