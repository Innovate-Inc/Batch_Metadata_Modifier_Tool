using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Schema;

namespace MetadataFormLibrary
{
    public enum XSDValidationResult { Valid, General, InvalidChild, NotDeclared };

    [Serializable]
    public class XSDValidationResultArgs
    {
        public XSDValidationResult Type { get; set; }
        public string Message { get; set; }
        public XmlSeverityType Severity { get; set; }
        public XmlNode Node {get; set; }
        public string InvalidNodeName { get; set; }
    }
}
