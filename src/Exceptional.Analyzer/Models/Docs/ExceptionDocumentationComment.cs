using System.Xml.Serialization;

namespace Exceptional.Analyzer.Models.Docs
{
    /// <summary>
    ///     Simpel XMLDoc exception model
    /// </summary>
    public class ExceptionDocumentationComment
    {
        private string? type;
        private string? description;

        /// <summary>
        ///     XMLDoc exception type
        /// </summary>
        [XmlAttribute("cref")]
        public string? Type
        {
            get => type;
            set => type = value?.Trim();
        }

        /// <summary>
        ///     XMLDoc exception description
        /// </summary>
        [XmlText]
        public string? Description
        {
            get => description;
            set => description = value?.Trim();
        }
    }
}
