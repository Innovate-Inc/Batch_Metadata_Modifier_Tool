using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Windows.Forms;
using System.Xml.Schema;
using System.Diagnostics;

namespace MetadataFormLibrary
{
    public static class SchemaUtils
    {
        public static List<XmlSchemaElement> GetChildrenElements(this XmlSchemaElement element)
        {
            List<XmlSchemaElement> childrenElements = new List<XmlSchemaElement>();

            if(element != null) {
                if(element.ElementSchemaType is XmlSchemaSimpleType) {
                    //Ignore Simple Types. I am assuming for now that all simple types are text and have no children.
                } else if(element.ElementSchemaType is XmlSchemaComplexType) {
                    XmlSchemaComplexType complexType = (XmlSchemaComplexType)element.ElementSchemaType;

                    if(complexType.ContentTypeParticle is XmlSchemaElement) { //Element
                        childrenElements.Add((XmlSchemaElement)complexType.ContentTypeParticle);
                    } else if(complexType.ContentTypeParticle is XmlSchemaGroupBase) { //GroupBase (All, Choice, Sequences)
                        XmlSchemaGroupBase gbChild = (XmlSchemaGroupBase)complexType.ContentTypeParticle;
                        childrenElements.AddRange(gbChild.GetChildrenElements());
                    } else {
                        Debug.Assert(false, "XmlSchemaElement.GetChildrenElements: ContentTypeParticle is not of type XmlSchemaSequence.");
                    }
                }
            }

            return childrenElements;
        }

        private static List<XmlSchemaElement> GetChildrenElements(this XmlSchemaGroupBase groupBase)
        {
            List<XmlSchemaElement> childrenElements = new List<XmlSchemaElement>();

            foreach(XmlSchemaParticle child in groupBase.Items) {
                if(child is XmlSchemaElement) { //Element
                    childrenElements.Add((XmlSchemaElement)child);
                } else if(child is XmlSchemaGroupBase) { //GroupBase (All, Choice, Sequences)
                    XmlSchemaGroupBase gbChild = (XmlSchemaGroupBase)child;
                    childrenElements.AddRange(gbChild.GetChildrenElements());
                } else if(child is XmlSchemaGroupRef) { //Reference to a Group (not to be confused with GroupBase)
                    Debug.Assert(false, "XmlSchemaGroupBase.GetChildrenElements: GroupRef is not supported yet.");
                }
            }

            return childrenElements;
        }
    }

}
