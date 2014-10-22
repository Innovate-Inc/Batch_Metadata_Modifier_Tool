using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using ESRI.ArcGIS.Catalog;
using ESRI.ArcGIS.esriSystem;
using ESRI.ArcGIS.Geodatabase;
using MetadataFormLibrary;
using System.Windows.Forms;
using System.Xml;


namespace ArcCatalogMetadataModifier
{
    public class Button1 : ESRI.ArcGIS.Desktop.AddIns.Button
    {
        //10.1 Addin created by DS.  
        private List<KeyValuePair<string, IMetadata>> mdDocuments = new List<KeyValuePair<string, IMetadata>>();

        public Button1()
        {
        }

        protected override void OnClick()
        {
            #region Original 10.0 Code modified by DS
            
            MetadataForm metadataForm = new MetadataForm();
            metadataForm.SaveEvent += new SaveEventHandler(metadataForm_SaveEvent);
            mdDocuments.Clear();  //DS Make sure to clear this before each OnClick

            //IGxObject junk;
            IGxSelection selection = ArcCatalog.ThisApplication.Selection;
            IEnumGxObject selectedObjects = selection.SelectedObjects as IEnumGxObject;

            Utils.changeLogDT.Clear(); //DS Clear the log before each load
            Utils.changeLogDT.Rows.Add("****Metadata Session Started: " + System.DateTime.Now + " Click Save to preserve changes****");
            //The following to variables are used to test the root node
            XmlDocument xmlDoc = new XmlDocument();
            string rootNodeName = "";

            
            // Console.WriteLine(ArcCatalog.ThisApplication.Selection.Count);
            for (int i = 0; i < selection.Count; ++i)
            {
                IGxObject selectedObject = selectedObjects.Next();
                
                //DS  ArcObjects Example to get the imetadata object...
                //Get the metadata
                //IMetadata metaData = (IMetadata)dataLayer.DataSourceName;
                //Get the xml property set from the metadata
                //IXmlPropertySet2 xml = (IXmlPropertySet2)metaData.Metadata;

                //******Will crash if a non-featureclass type object is open.  Need to test for IGxObject type and maybe create a IGxObjecfactory or somethign.
                //DS  Needed to use InternalObjectName to get the propper object name for each open featureclass
                try
                {
                    IMetadata metadata = (IMetadata)selectedObject.InternalObjectName;
                    if (metadata != null)
                    {
                        //System.Windows.Forms.MessageBox.Show(selectedObject.FullName);
                        IXmlPropertySet2 xmlPropSet2 = metadata.Metadata as IXmlPropertySet2;
                        string xmlMetadata = xmlPropSet2.GetXml("/");

                        //load here first to test that root nodes are the same.  There might be a better place to do this.
                        //See also the LoadDocument Method in MetadataForm.cs
                        xmlDoc.LoadXml(xmlMetadata);
                        //Don't load document if the root node does not match the first record added!!!
                        if (rootNodeName != "" && rootNodeName != xmlDoc.DocumentElement.Name)
                        {
                            Utils.changeLogDT.Rows.Add("*Root Node Mismatch!, Skipped File: " + selectedObject.FullName);
                        }
                        else
                        {
                            rootNodeName = xmlDoc.DocumentElement.Name;
                            mdDocuments.Add(new KeyValuePair<string, IMetadata>(xmlMetadata, metadata));
                            metadataForm.AddDocument(ref xmlMetadata);

                            Utils.changeLogDT.Rows.Add("Loaded:" + selectedObject.FullName);
                        }
                    }
                }
                catch (Exception e)
                {
                    Utils.changeLogDT.Rows.Add("*File Type Not Supported in Add-In* Skipped File: " + selectedObject.Name);
                }
            }

            metadataForm.Show();
            #endregion

        }

        protected override void OnUpdate()
        {
            Enabled = ArcCatalog.Application != null;
        }

        //DS  Joey's Code Block for custom save event.  See delegate expression
        private void metadataForm_SaveEvent(object sender, SaveEventArgs e)
        {
            IMetadata metadata = mdDocuments[e.Index].Value as IMetadata;
            if (metadata != null)
            {
                IXmlPropertySet2 xmlPropSet2 = metadata.Metadata as IXmlPropertySet2;
                xmlPropSet2.SetXml(e.XML);                
                metadata.Metadata = xmlPropSet2 as IPropertySet;
            }
        }
    }
}
