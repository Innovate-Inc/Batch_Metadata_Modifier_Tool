using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Schema;
using System.Text.RegularExpressions;

namespace MetadataFormLibrary
{
    public class XMLValidation
    {
        private XmlSchemaSet schemaSet;
        private XSDValidationResultArgs results;

        public XMLValidation(string schemaFileName)
        {
            XmlSchema schema = XmlSchema.Read(XmlReader.Create(schemaFileName), null);

            schemaSet = new XmlSchemaSet();
            schemaSet.Add(schema);
        }

        public XSDValidationResultArgs ValidateSubnode(XmlNode node)
        {
            results = new XSDValidationResultArgs();
            results.Type = XSDValidationResult.Valid;
            results.Node = node;

            XmlSchemaInfo schemaInfo = new XmlSchemaInfo();

            NameTable nt = new NameTable();
            XmlNamespaceManager nsmgr = new XmlNamespaceManager(nt);

            XmlSchemaValidator xsv = new XmlSchemaValidator(nt, schemaSet, nsmgr, XmlSchemaValidationFlags.ProcessInlineSchema);
            xsv.ValidationEventHandler += new ValidationEventHandler(ValidationEventHandler);
            xsv.Initialize();

            //Validate root node
            if(node.NodeType == XmlNodeType.Text) {
                xsv.ValidateText(node.Value);
            } else {
                xsv.ValidateElement(node.LocalName, string.Empty, schemaInfo);
                xsv.ValidateEndOfAttributes(schemaInfo);

                //Validate its children
                for(int i = 0; i < node.ChildNodes.Count; ++i) {
                    //Validate child and skip its content.
                    //TODO: Make sure this works with text nodes.
                    if(node.ChildNodes[i].NodeType == XmlNodeType.Text) {
                        xsv.ValidateText(node.ChildNodes[i].Value);
                    } else {
                        xsv.ValidateElement(node.ChildNodes[i].LocalName, string.Empty, schemaInfo);
                        xsv.SkipToEndElement(schemaInfo);
                    }
                }

                //End validation for root node.
                xsv.ValidateEndElement(schemaInfo);
            }

            xsv.EndValidation();

            return results;
        }

        private void ValidationEventHandler(object sender, ValidationEventArgs e)
        {
            Console.WriteLine(string.Format("XSD Error - Message: {0}", e.Message));

            results.Type = XSDValidationResult.General;
            results.InvalidNodeName = null;

            //TODO: If this gets too big we should put the values in an array and just loop them.
            Regex regex = new Regex(@"has invalid child element '(?<element>\w*)'");
            if(regex.IsMatch(e.Message)) {
                Match match = regex.Match(e.Message);

                results.Type = XSDValidationResult.InvalidChild;
                results.InvalidNodeName = match.Groups["element"].Value;
            }
            //TODO: Validate text node eventually.

            regex = new Regex(@"'(?<element>\w*)' element is not declared");
            if(regex.IsMatch(e.Message)) {
                Match match = regex.Match(e.Message);

                results.Type = XSDValidationResult.NotDeclared;
                results.InvalidNodeName = match.Groups["element"].Value;
            }
                

            results.Message = e.Message;
            results.Severity = e.Severity;
        }
    }
}
