using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using MetadataFormLibrary;
using System.Xml;
using System.Xml.Schema;

namespace MetadataEditor_Test_Form
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {

            //XmlReader reader = XmlReader.Create(@"C:\Documents and Settings\jgentry\Desktop\fgdc-std-001-1998-ann\fgdc-std-001-1998-ann.xsd");
            //XmlDocument doc = new XmlDocument();
            //doc.Load(@"C:\Documents and Settings\jgentry\Desktop\fgdc-std-001-1998-ann\fgdc-std-001-1998-ann.xsd");

            /*XmlSchemaSet set = new XmlSchemaSet();
            set.Add(XmlSchema.Read(XmlReader.Create(@"C:\Documents and Settings\jgentry\Desktop\fgdc-std-001-1998-ann\fgdc-std-001-1998-ann.xsd"), null));
            set.Compile();*/

            //GetParentNode(set);
            /*Console.WriteLine("Elements");
            foreach(XmlSchemaElement obj in set.GlobalElements.Values) {
                Console.WriteLine("{0}: {1}", obj.Name, obj.SourceUri);
            }*/

            //List all types
           /* Console.WriteLine("Types");
            foreach(XmlSchemaType obj in set.GlobalTypes.Values) {
                if(obj is XmlSchemaComplexType && ((XmlSchemaComplexType)obj).Particle != null && !(((XmlSchemaComplexType)obj).Particle is XmlSchemaSequence)) {
                    Console.WriteLine("{0}: {1}", obj.Name, ((XmlSchemaComplexType)obj).Particle.GetType());
                }
            }*/

        }

        private string GetParentNode(XmlSchemaSet set)
        {
            foreach(XmlSchemaElement element in set.GlobalElements.Values) {
                bool containsRef = false;
                foreach(XmlSchemaType type in set.GlobalTypes.Values) {
                    if(type is XmlSchemaComplexType) {
                        XmlSchemaComplexType complexType = type as XmlSchemaComplexType;
                        containsRef = TypeContainsRef(complexType.Particle, element.Name);
                        if(containsRef) break;
                    }
                }
                if(!containsRef) Console.WriteLine("{0}: {1}", element.Name, element.SourceUri);
            }
        
            return "";
        }

        private bool TypeContainsRef(XmlSchemaParticle particle, string elementName) 
        {
            if(particle is XmlSchemaGroupBase) {
                XmlSchemaGroupBase groupBase = particle as XmlSchemaGroupBase;
                foreach(XmlSchemaObject schemaObject in groupBase.Items) {
                    if(schemaObject is XmlSchemaElement) {
                        if(((XmlSchemaElement)schemaObject).RefName.Name == elementName)
                            return true;
                    } else {
                        if(TypeContainsRef(((XmlSchemaParticle)schemaObject), elementName))
                            return true;
                    }
                }
            }

            return false;
        }
    }
}
