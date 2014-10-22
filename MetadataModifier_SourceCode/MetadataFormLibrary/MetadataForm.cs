using System;
using System.Collections.Generic;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Schema;
using System.Text.RegularExpressions;
using System.IO;
using Microsoft.Xml;
using System.Diagnostics;

namespace MetadataFormLibrary
{
    public partial class MetadataForm : Form
    {
        delegate void LoadDocumentsDelegate(string[] filenames);
        //DS   Show progress asynchronously - Used delegate to force invoke because InvokeRequired == false.
        private FormSearch frmSearch;
        private bool ESRIMode = false;
        private XMLValidation validator;        
        private XmlSchemaSet schemaSet;
        private XmlNamespaceManager isoNsManager;
        private string rootNodeName;

        private List<KeyValuePair<string, XmlDocument>> mdDocuments = new List<KeyValuePair<string, XmlDocument>>();

        public event SaveEventHandler SaveEvent;
                                
        public MetadataForm()
        {
            InitializeComponent();
           

            //DS changeLogDT is a Global variable to track changes
            //This is the first place it would get called in the form
            if (Utils.changeLogDT.Columns.Contains("Procedure") == false)
            {
                Utils.changeLogDT.Columns.Add("Procedure");
            }            
            this.lboChangeLog.DisplayMember = "Procedure";            
            this.lboChangeLog.ValueMember = "Procedure";
            this.lboChangeLog.DataSource = Utils.changeLogDT;
            
            //schema loader
            TreeNodeData.AddIcons(ilTvMetadata);

            #region Old schema loading method

            //The program will not run without a an internet connection to the fgdc site.  Below are several schema to choose from:
            //string validationXSD = @"http://www.fgdc.gov/schemas/metadata/fgdc-std-001-1998.xsd";
            //string validationXSD = @"http://dev.insideidaho.org/HelpDocs/metadata/tool/schemas/fgdc-std-001-1998/fgdc-std-001-1998.xsd";
            //string validationXSD2 = @"http://dev.insideidaho.org/HelpDocs/metadata/tool/schemas/ags/ags10.xsd";
            //MessageBox.Show(Directory.GetCurrentDirectory() + @"\Resources\fgdc-std-001-1998.xsd");
            //string validationXSD = Directory.GetCurrentDirectory() + @"\Resources\fgdc-std-001-1998.xsd";
            //string validationXSD = Directory.GetCurrentDirectory() + @"\fgdc-std-001-1998-ann\fgdc-std-001-1998-ann.xsd";
            //string validationXSD = @"C:\Documents and Settings\jgentry\My Documents\Schemas\iso\gmd.xsd";
            //string validationXSD = @"http://www.isotc211.org/2005/gmi/gmi.xsd";
            //string validationXSD = @"ftp://ftp.ncddc.noaa.gov/pub/Metadata/Online_ISO_Training/Intro_to_ISO/schemas/ISO/schema.xsd";

            //XmlReaderSettings readerSettings = new XmlReaderSettings();
            //readerSettings.IgnoreComments = true;

            //validator = new XMLValidation(validationXSD);
            //XmlSchema xs = XmlSchema.Read(XmlReader.Create(validationXSD, readerSettings), null);
            //XmlSchema xs = XmlSchema.Read(XmlReader.Create(validationXSD), null);
            //schemaSet = new XmlSchemaSet();
            //schemaSet.Add(xs);            
            //schemaSet.Compile();
            #endregion
            
            schemaSet = new XmlSchemaSet();            
            schemaSet.Add(new XmlSchema());
            schemaSet.Compile();

            NameTable nt = new NameTable();
            isoNsManager = new XmlNamespaceManager(nt);
            #region Requires internet connection.  Use this if validating schema 
            isoNsManager.AddNamespace("gco", "http://www.isotc211.org/2005/gco");
            isoNsManager.AddNamespace("gfc", "http://www.isotc211.org/2005/gfc");
            isoNsManager.AddNamespace("gmd", "http://www.isotc211.org/2005/gmd");
            isoNsManager.AddNamespace("gmi", "http://www.isotc211.org/2005/gmi");
            isoNsManager.AddNamespace("gml", "http://www.isotc211.org/2005/gml");
            isoNsManager.AddNamespace("gmx", "http://www.isotc211.org/2005/gmx");
            //nsmanager.AddNamespace("gplr", "http://www.isotc211.org/2005/gplr");
            isoNsManager.AddNamespace("grg", "http://www.isotc211.org/2005/grg");
            isoNsManager.AddNamespace("gsr", "http://www.isotc211.org/2005/gsr");
            isoNsManager.AddNamespace("gss", "http://www.isotc211.org/2005/gss");
            isoNsManager.AddNamespace("gts", "http://www.isotc211.org/2005/gts");
            #endregion
            //isoNsManager.AddNamespace("gco", "");
            //isoNsManager.AddNamespace("gfc", "");
            //isoNsManager.AddNamespace("gmd", "");
            //isoNsManager.AddNamespace("gmi", ""); 
            //isoNsManager.AddNamespace("gml", "");
            //isoNsManager.AddNamespace("gmx", "");
            ////nsmanager.AddNamespace("gplr", "");
            //isoNsManager.AddNamespace("grg", "");
            //isoNsManager.AddNamespace("gsr", "");
            //isoNsManager.AddNamespace("gss", "");            
            //isoNsManager.AddNamespace("gts", "");
                                                
            //TraverseSchema(); //THis does not work.

#if TEST_MODE
            //Load xml for testing
            //string dom1Filename = @"C:\Users\jgentry\Documents\Inside Idaho Projects\Metadata\Editor Test\test1.xml";
            //string dom1Filename = @"C:\Users\jgentry\Documents\Inside Idaho Projects\Metadata\Editor Test\lilacBloom_id_igdc.shp.xml";
            //LoadDocument(dom1Filename);

            //string dom2Filename = @"C:\Users\jgentry\Documents\Inside Idaho Projects\Metadata\Editor Test\test2.xml";
            //LoadDocument(dom2Filename);
#endif
        }

        void TraverseSchema()
        {
            // Retrieve the compiled XmlSchema object from the XmlSchemaSet
            // by iterating over the Schemas property.
            XmlSchema customerSchema = null;
            foreach(XmlSchema schema in schemaSet.Schemas()) {
                customerSchema = schema;
            }

            // Iterate over each XmlSchemaElement in the Values collection
            // of the Elements property.
            foreach(XmlSchemaElement element in customerSchema.Elements.Values) {

                Console.WriteLine("Element: {0}", element.Name);

                // Get the complex type of the Customer element.
                XmlSchemaComplexType complexType = element.ElementSchemaType as XmlSchemaComplexType;

                if(complexType != null) {
                    // If the complex type has any attributes, get an enumerator 
                    // and write each attribute name to the console.
                    if(complexType.AttributeUses.Count > 0) {
                        System.Collections.IDictionaryEnumerator enumerator =
                            complexType.AttributeUses.GetEnumerator();

                        while(enumerator.MoveNext()) {
                            XmlSchemaAttribute attribute =
                                (XmlSchemaAttribute)enumerator.Value;

                            Console.WriteLine("Attribute: {0}", attribute.Name);
                        }
                    }

                    // Get the sequence particle of the complex type.
                    XmlSchemaSequence sequence = complexType.ContentTypeParticle as XmlSchemaSequence;

                    if(sequence != null) {
                        // Iterate over each XmlSchemaElement in the Items collection.
                        foreach(XmlSchemaElement childElement in sequence.Items) {
                            Console.WriteLine("Element: {0}", childElement.Name);
                        }
                    }
                }
            }
        }

        void Start(XmlSchema xs)
        {
            XmlSchemaComplexType complexType;
            foreach(XmlSchemaType type in xs.SchemaTypes.Values) {
                complexType = type as XmlSchemaComplexType;
                if(complexType != null)
                    TraverseParticle(complexType.ContentTypeParticle);
            }

            foreach(XmlSchemaElement el in xs.Elements.Values)
                TraverseParticle(el);
        }

        void TraverseParticle(XmlSchemaParticle particle)
        {
            if(particle is XmlSchemaElement) {
                XmlSchemaElement elem = particle as XmlSchemaElement;

                if(elem.RefName.IsEmpty) {
                    XmlSchemaType type = (XmlSchemaType)elem.ElementSchemaType;
                    XmlSchemaComplexType complexType = type as XmlSchemaComplexType;
                    if(complexType != null && complexType.Name == null) {
                        Console.WriteLine(particle.Id);
                        TraverseParticle(complexType.ContentTypeParticle);
                    }
                }
            } else if(particle is XmlSchemaGroupBase) { //xs:all, xs:choice, xs:sequence
                XmlSchemaGroupBase baseParticle = particle as XmlSchemaGroupBase;
                foreach(XmlSchemaParticle subParticle in baseParticle.Items) {
                    Console.WriteLine(particle.Id);
                    TraverseParticle(subParticle);
                }
            }
        }

        #region Events
        protected virtual void OnSaveEvent(SaveEventArgs e)
        {
            SaveEvent(this, e);
        }
        #endregion

        public void AddDocument(ref string xml)
        {
            //TODO: Think of a way to deal with having esri stay in the esri project and allow for many files.
            //Need to map something and use a event to trigger a save.
            ESRIMode = true;

            //Hide the open option while in ESRI. This may or may not be enabled later but for now with the
            //two methods of saving we should not combine them. ESRI currently wont know how to save these newly opened files.
            openToolStripButton.Visible = false;
            directorytoolStripButton.Visible = false;

            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(xml);
            mdDocuments.Add(new KeyValuePair<string, XmlDocument>(string.Empty, xmlDoc));

            if(tvMetadata.Nodes.Count == 0)
            {
                tvMetadata.Nodes.Add(new TreeNode(xmlDoc.DocumentElement.Name));
            }

            tvMetadata.BeginUpdate();
            AddNode(xmlDoc.DocumentElement, tvMetadata.Nodes[0]);
            //UpdateTreeView will make the elements colored per the standard
            UpdateTreeView(tvMetadata.Nodes[0]);
            tvMetadata.EndUpdate();
        }

        public void LoadDocument(string filename)
        {
            //Modified to ignore xml comments... this will delete them out of the original xml file!!!!
            //XmlReaderSettings readerSettings = new XmlReaderSettings();
            //readerSettings.IgnoreComments = true;
            //XmlReader reader = XmlReader.Create(filename, readerSettings);
                    
            XmlDocument xmlDoc = new XmlDocument();
            //xmlDoc.Load(reader);//this will remove comments
            try
            {
                xmlDoc.Load(filename);

                //Don't load document if the root node does not match the first record added!!!
                if (rootNodeName != "" && rootNodeName != xmlDoc.DocumentElement.Name)
                {
                    Utils.changeLogDT.Rows.Add("*Root Node Mismatch!, Skipped File: " + filename);
                    return;
                }
                //Marked out the line below since it was slowing down load performance 2/4/2014 DS
                //Utils.changeLogDT.Rows.Add("Loaded: " + filename);
                mdDocuments.Add(new KeyValuePair<string, XmlDocument>(filename, xmlDoc));

                if (tvMetadata.TSGetCount() == 0)
                {
                    //Get the root node from the first document loaded
                    tvMetadata.TSAdd(tvMetadata.Nodes, new TreeNode(xmlDoc.DocumentElement.Name));
                    rootNodeName = xmlDoc.DocumentElement.Name;
                }
                AddNode(xmlDoc.DocumentElement, tvMetadata.Nodes[0]);
            }
            catch (Exception e)
            {
                Utils.changeLogDT.Rows.Add("*Skipped File Due to Error: " + filename);
            }
        }

        public void LoadDocuments(string[] filenames)
        {
            Utils.changeLogDT.Clear(); //Clear the log before each load
            Utils.changeLogDT.Rows.Add("****Metadata Session Started: " + System.DateTime.Now + " Click Save to preserve changes****");
            rootNodeName = ""; //Clear this before each load
            
            //DS  Getting Thread Error on this method in debug mode.
            tvMetadata.Nodes.Clear();
            dgvValidationErrors.Rows.Clear();
            mdDocuments.Clear();
            cbValues.Items.Clear();
            tbCurrentValue.Clear();
            
            //Thread begins here
            tvMetadata.TSBeginUpdate();
            
            int i = 0;
            foreach(string fileName in filenames) {
                LoadDocument(fileName);
                UpdateProgressBar(++i, filenames.Length);
                //DS  ToDO  Add files loaded to dialog
                //Utils.changeLogDT.Rows.Add("Loaded: " + fileName);
            }
            UpdateTreeView(tvMetadata.Nodes[0]);
            
            //Multi-thread end
            tvMetadata.TSEndUpdate();
            Utils.changeLogDT.Rows.Add("");          

        }

        private Icon GetErrorIcon(XmlSeverityType severity)
        {
            Icon errorIcon = new Icon(MetadataFormLibrary.Properties.Resources.ErrorIcon, 16, 16);
            Icon warningIcon = new Icon(MetadataFormLibrary.Properties.Resources.WarningIcon, 16, 16);
            return severity == XmlSeverityType.Warning ? warningIcon : errorIcon;
        }

        private void AddNode(XmlNode xmlNode, TreeNode treeNode)
        {
            if(xmlNode.Name != treeNode.Text)
                return;

            //Add NodeData to tag.
            if(treeNode.Tag == null) treeNode.Tag = new TreeNodeData();

            TreeNodeData nodeData = (TreeNodeData)treeNode.Tag;
            nodeData.Name = xmlNode.Name;
            nodeData.NodeRefs.Add(xmlNode);
            nodeData.TreeNode = treeNode;

            //XSDValidationResultArgs results = validator.ValidateSubnode(xmlNode);

            //if(results.Type == XSDValidationResult.InvalidChild) {

            //    //If this is the root node.
            //    if(xmlNode.ParentNode == xmlNode.OwnerDocument) {
            //        //TODO: Right now this is done very poorly. I basically remove an invalid child node from the xml and 
            //        //then re-add the parent to the tree. I do this for every bad node until there are none. This is bad
            //        //because if a node is valid but out of order it is removed. It also is not very efficient. I think I 
            //        //should not be playing with the original xml anyways except for editing.

            //        //Remove the invalid child(ren) by name.
            //        for(int i = xmlNode.ChildNodes.Count - 1; i >= 0; --i) {
            //            /*if(xmlNode.ChildNodes[i].Name == results.InvalidNodeName)
            //                xmlNode.RemoveChild(xmlNode.ChildNodes[i]);*/
            //        }

            //        //AddNode(xmlNode, treeNode);
            //        //return;
            //    } else {
            //        //TODO: Not working right now because to find invalid children we look ahead. But we cant 
            //        //set the color until we actually add it to the tree. So we need a way to save this or
            //        //build the tree in a different way...
            //        /*for(int i = xmlNode.ChildNodes.Count - 1; i >= 0; --i) {
            //            if(xmlNode.ChildNodes[i].Name == results.InvalidNodeName)
            //                treeNode.ForeColor = Color.Red;
            //        }       */
            //    }
            //}

            //TODO: Find missing nodes that are required. Right now we can find the first one for a given node.
            //To find them all I would have to temporarily add them and validate again. This is maybe fine (performance?)
            //but then I need to track the nodes I add and remove for display purposes. Then if someone adds or removes a node 
            //I need to make sure that we can save the xml properly.
            //Add errors to error list
            //if(results.Type != XSDValidationResult.Valid) {
            //    dataGridView1.Rows.Add(GetErrorIcon(results.Severity), results.Message);
            //}

            // Loop through the XML nodes until the leaf is reached.
            // Add the nodes to the TreeView during the looping process.
            if(xmlNode.HasChildNodes && xmlNode.ChildNodes[0].NodeType != XmlNodeType.Text) {
                nodeData.Type = xmlNode.NodeType;

                for(int i = 0; i < xmlNode.ChildNodes.Count; ++i) {
                    //Create new nodes for each new node in a given doc otherwise use the current nodes. 
                    //We can only have a max number that equals the max number of nodes in any one doc. This allows
                    //us to support multiple nodes for multiple documents while being able to sync documents. (1st xml node always matches 1st tree node...)
                    
                    //Get the current index of the xml (child) node from all children with the same name
                    int nodeIndex = 0;                            
                    
                    ////Modified To Handle ISO, and to exclude XML Comments

                    if (xmlNode.ChildNodes[i].NodeType != XmlNodeType.Comment)
                    {
                        XmlNodeList childXMLNodeList = xmlNode.SelectNodes(xmlNode.ChildNodes[i].Name, isoNsManager);
                        ////Old method w/o an isoNsManger object                    
                        //XmlNodeList childXMLNodeList = xmlNode.SelectNodes(xmlNode.ChildNodes[i].Name);

                        for (int childIndex = 0; childIndex < childXMLNodeList.Count; ++childIndex)
                        {
                            if (xmlNode.ChildNodes[i] == childXMLNodeList[childIndex])
                            {
                                nodeIndex = childIndex;
                            }
                        }

                        //If the tree node does not exist with the same index we need to create and add an additional tree node with this element name.
                        TreeNode newNode = Utils.TreeNodeByElemementNameAtIndex(treeNode, xmlNode.ChildNodes[i].Name, nodeIndex);
                        if (newNode == null)
                        {
                            newNode = new TreeNode(xmlNode.ChildNodes[i].Name);
                            newNode.Name = xmlNode.ChildNodes[i].Name;
                            //treeNode.Nodes.Add(newNode);
                            tvMetadata.TSAdd(treeNode.Nodes, newNode);
                        }

                        nodeData.ImageIndex = TreeNodeData.Icons.Folder;

                        AddNode(xmlNode.ChildNodes[i], newNode);
                    }
                }
             } else {
                //Here is where we setup the NodeData for text nodes.
                nodeData.Type = XmlNodeType.Text;
                nodeData.ValueSet.Add(xmlNode.InnerText);
                nodeData.ImageIndex = TreeNodeData.Icons.Text;
            }

            ////Set icons
            //if(results.Type != XSDValidationResult.Valid) {
            //    if(nodeData.Type == XmlNodeType.Text)
            //        treeNode.ImageKey = treeNode.SelectedImageKey = results.Severity == XmlSeverityType.Warning ? "TextWarning" : "TextError";
            //    else
            //        treeNode.ImageKey = treeNode.SelectedImageKey = results.Severity == XmlSeverityType.Warning ? "FolderWarning" : "FolderError";
            //}
        }

        #region TreeView
        
        //This method is used to update any visual stuff in the tree post document processing. This method should also be/stay threadsafe.
        private void UpdateTreeView(TreeNode treeNode)
        {
            TreeNodeData nodeData = (TreeNodeData)treeNode.Tag;
            tvMetadata.TSSetImageKey(treeNode, (int)nodeData.ImageIndex);
            tvMetadata.TSSetSelectedImageKey(treeNode, (int)nodeData.ImageIndex);

            string parentName = nodeData.TreeNode.Parent != null ? nodeData.TreeNode.Parent.Text : string.Empty;
            //XmlSchemaElement schemaElement = (XmlSchemaElement)schemaSet.GlobalElements[new XmlQualifiedName(parentName, null)];

            //if(schemaElement != null && schemaElement.ElementSchemaType is XmlSchemaComplexType)
            //{
            //    XmlSchemaComplexType complexType = (XmlSchemaComplexType)schemaElement.ElementSchemaType;
            //    XmlSchemaParticle particle = complexType.Particle;

            //    switch (particle.GetType().Name)
            //    {
            //        case "XmlSchemaSequence":
            //            XmlSchemaSequence sequence = (XmlSchemaSequence)particle;

            //            for (int i = 0; i < sequence.Items.Count; ++i)
            //            {
            //                if (sequence.Items[i] is XmlSchemaElement)
            //                {
            //                    XmlSchemaElement element = (XmlSchemaElement)sequence.Items[i];
            //                    if (element.QualifiedName.Name == nodeData.Name)
            //                    {
            //                        if (element.MinOccurs > 0)
            //                            nodeData.TreeNode.ForeColor = Color.Red;
            //                    }
            //                }
            //            }

            //            break;
            //    }
            //}

            for(int i = 0; i < treeNode.Nodes.Count; ++i) {
                UpdateTreeView(treeNode.Nodes[i]);
            }
        }

        private void tvMetadata_AfterSelect(object sender, TreeViewEventArgs e)
        {
            //TODO: XmlSchemaComplexContentRestriction 
            //Parse the schema and build the GUI.
            cbValues.Items.Clear();
            tbCurrentValue.Clear();

            TreeNodeData nodeData = e.Node.Tag as TreeNodeData;
            
            
            if(nodeData != null) 
            {
                //schemaElement.
                if(nodeData.Type == XmlNodeType.Text)
                {
                    //Add enum/list/union to dropdown
                    
                    
                    
                    XmlSchemaElement schemaElement = (XmlSchemaElement)schemaSet.GlobalElements[new XmlQualifiedName(nodeData.Name, null)];
                    if(schemaElement != null)
                    {
                        if(schemaElement.ElementSchemaType is XmlSchemaSimpleType)
                        {

                            XmlSchemaSimpleType simpleType = (XmlSchemaSimpleType)schemaElement.ElementSchemaType;

                            //Easy way to do enumeration. Does not currently support more than one level. This may be ok though.
                            if(simpleType.Content is XmlSchemaSimpleTypeRestriction)
                            {
                                XmlSchemaSimpleTypeRestriction simpleTypeRestriction = simpleType.Content as XmlSchemaSimpleTypeRestriction;
                                for(int i = 0; i < simpleTypeRestriction.Facets.Count; ++i)
                                {
                                    if(simpleTypeRestriction.Facets[i] is XmlSchemaEnumerationFacet)
                                    {
                                        XmlSchemaEnumerationFacet enumerationFacet = simpleTypeRestriction.Facets[i] as XmlSchemaEnumerationFacet;
                                        cbValues.Items.Add(enumerationFacet.Value);
                                    }
                                }

                                //Unneeded? 
                                //TODO: Delete later if we verify this is not required. We should not need to test the type to add a value
                                //We can just add any values in the ValueSet. (Below)
                                //There are no restrictions other than the base type.
                                //TODO: Can we assume that if its a simple type with no restrictions/facets then it is a string.
                                /*if(simpleTypeRestriction.Facets.Count == 0) {
                                    cbValues.Items.AddRange(nodeData.ValueSet.ToArray());
                                }*/
                            }

                            //schemaSimpleType.ba
                            //TODO: Find the very basic type and pattern. Detect if enum/list/union. 
                            //schemaSimpleType.Content
                        } else if(schemaElement.SchemaType is XmlSchemaComplexType) {
                            //Im thinking all text nodes are simple types.
                        }
                    }

                    //TODO: Instead of using the initial values why dont we grab them from the xml data. It would be more reliable.
                    cbValues.Items.AddRange(nodeData.ValueSet.ToArray());

                    if(cbValues.Items.Count > 0) {
                        cbValues.SelectedIndex = 0;
                    }

                    //TODO: Access the panel for the controls. Here in case we need it later.
                    //this.splitContainer2.Panel2.Controls.Add(this.tvMetadata);
                }
            }
        }

        private void tvMetadata_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            if(e.Button == MouseButtons.Right) {
                tvMetadata.SelectedNode = e.Node;
                TreeNodeData nodeData = e.Node.Tag as TreeNodeData;
                cmsEditTree.Show(tvMetadata, e.Location);
            }
        }

        #endregion

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            TreeNodeData nodeData = tvMetadata.SelectedNode.Tag as TreeNodeData;

            if(nodeData.Type == XmlNodeType.Text)
            {
                XmlNode[] xmlNodes = nodeData.NodeRefs.ToArray();
                for(int i = 0; i < nodeData.NodeRefs.Count; ++i)
                {
                    XmlNode xmlNode = xmlNodes[i];
                    xmlNode.InnerText = tbCurrentValue.Text;
                    //Utils.changeLogDT.Rows.Add("Update File: " + xmlNode.BaseURI);
                    //Utils.changeLogDT.Rows.Add("Update Value: " + xmlNode.InnerText);
                }
                //string str = xmlNodes[0].OuterXml + " " + xmlNodes[0].ParentNode;
                Utils.changeLogDT.Rows.Add(nodeData.NodeRefs.Count + " records updated with: " + xmlNodes[0].OuterXml);
                lboChangeLog.SelectedIndex = lboChangeLog.Items.Count-1;                
            }

            //TODO: Refresh the current node here. Probably only need to refresh the dropdown list though. This may not 
            //be required however. We may want to keep all the values we already loaded incase user wants to change their mind after changing a node.
        }

        #region Menu Bar

        private void openToolStripButton_Click(object sender, EventArgs e)
        {

            OpenFileDialog ofDialog = new OpenFileDialog();
            ofDialog.Filter = "XML Metadata (*.XML)|*.XML|All Files (*)|(*)";
            ofDialog.Multiselect = true;
            ofDialog.Title = "Select all the xml metadata files you would like to open.";

            if (ofDialog.ShowDialog() == DialogResult.OK)
            {
                foreach (string s in ofDialog.FileNames)
                {
                    Console.WriteLine(s);
                }
                //Show progress asynchronously - Used delegate to force invoke because InvokeRequired == false.
                LoadDocumentsDelegate loadDocuments = new LoadDocumentsDelegate(LoadDocuments);
                loadDocuments.BeginInvoke(ofDialog.FileNames, null, null);
            }

            //lboChangeLog.DisplayMember = "Procedure";
            //lboChangeLog.ValueMember = "Procedure";
            //lboChangeLog.DataSource = changeLogDT;
            //lboChangeLog.Refresh();
            

        }

        private void saveToolStrip_Click(object sender, EventArgs e)
        {
            List<string> reOpenList = new List<string>();
            Utils.changeLogDT.Rows.Add("Saving... ");
            lboChangeLog.SelectedIndex = lboChangeLog.Items.Count - 1;
            for(int i = 0; i < mdDocuments.Count; ++i)
            {
                //DS TreeView not refreshing after save.  Need to either clear the treeview completely, or refresh it with the committed changes
                //Should the treeview reflect change of the In-memmory metadata records?  Does not currently update to reflect the file or in-memmory records
                if(!ESRIMode)
                {
                    mdDocuments[i].Value.Save(mdDocuments[i].Key);
                    //Utils.changeLogDT.Rows.Add("Saving: " + mdDocuments[i].Key);
                    //lboChangeLog.SelectedIndex = lboChangeLog.Items.Count - 1;
                    reOpenList.Add(mdDocuments[i].Key);
                }
                else
                {
                    StringWriter sw = new StringWriter();
                    XmlTextWriter xw = new XmlTextWriter(sw);
                    mdDocuments[i].Value.WriteTo(xw);
                    OnSaveEvent(new SaveEventArgs(i, sw.ToString()));                    
                }                
            }
           
            //Update the treeview after processing the mdDocuments
            //Need to check if in ESRI Mode
            if (!ESRIMode)
            {
                //Not in ESRI Mode.  Thread safe...
                LoadDocumentsDelegate loadDocuments = new LoadDocumentsDelegate(LoadDocuments);
                loadDocuments.BeginInvoke(reOpenList.ToArray<string>(), null, null);
                //LoadDocuments(reOpenList.ToArray<string>());
            }
            else
            {
                //ESRI Mode.  No file list is passed in.  Just clear or hide the form for now
                //To reload the form with the updated files you wild need ArcObject refernces pass in,
                //or add a custom event in the Addin to handle.
                MessageBox.Show("Files Saved");
                this.Hide();               
                //Could also try updating the 
                //UpdateTreeView(tvMetadata.Nodes[0]);
            }
                                  

        }

        private void findToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Search(false);
        }

        private void replaceToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Search(true);
        }

        #endregion

        #region TreeView Edit Context Menu

        //InsertElement turned off to avoid errors with inserting incorrect nodes against allowed schema
        private void tsmiAddElement_DropDownOpening(object sender, EventArgs e)
        {
            //TreeNodeData nodeData = tvMetadata.SelectedNode.Tag as TreeNodeData;

            ////TODO: Do validation on returned nodes. Eg. Check for choices and don't allow adding nodes to invalid node trees. ect.
            //XmlSchemaElement element = schemaSet.GlobalElements[new XmlQualifiedName(nodeData.Name, null)] as XmlSchemaElement;
            //List<XmlSchemaElement> childrenElements = element.GetChildrenElements();

            //tsmiAddElement.DropDownItems.Clear();

            //foreach (XmlSchemaElement childElement in childrenElements)
            //{
            //    Image image = childElement.ElementSchemaType is XmlSchemaSimpleType ?
            //        MetadataFormLibrary.Properties.Resources.TextIcon16 :
            //        image = MetadataFormLibrary.Properties.Resources.FolderIcon16;

            //    ToolStripMenuItem item = new ToolStripMenuItem(childElement.QualifiedName.Name, image, new EventHandler(tsmiAddElement_Click));
            //    item.Tag = childElement.ElementSchemaType is XmlSchemaSimpleType ? XmlNodeType.Text : XmlNodeType.Element;
            //    tsmiAddElement.DropDownItems.Add(item);
            //}

            ////Add a disabled placeholder letting the user know there are no possible nodes to add.
            //if (childrenElements.Count == 0)
            //{
            //    tsmiAddElement.DropDownItems.Add("[No Elements Found]");
            //    tsmiAddElement.DropDownItems[0].Enabled = false;
            //}
        }

        //InsertElement
        //private void tsmiAddElement_Click(object sender, EventArgs e)
        //{
        //    ToolStripMenuItem item = (ToolStripMenuItem)sender;
        //    XmlNodeType nodeType = (XmlNodeType)item.Tag;

        //    TreeNodeData nodeData = tvMetadata.SelectedNode.Tag as TreeNodeData;

        //    XmlNode[] xmlNodes = nodeData.NodeRefs.ToArray();
        //    for(int i = 0; i < nodeData.NodeRefs.Count; ++i) {
        //        XmlNode xmlNode = xmlNodes[i];

        //        /*
        //        //Add the new node to the previous element in the path.
        //        XmlElement newNode = xmlNode.OwnerDocument.CreateElement(item.Text);

        //        //Add text element if needed.
        //        if(nodeType == XmlNodeType.Text) {
        //            XmlText textNode = xmlNode.OwnerDocument.CreateTextNode(string.Empty);
        //            newNode.AppendChild(textNode);
        //        }

        //        xmlNode.AppendChild(newNode);
        //        */

        //        InsertElement(xmlNode, item.Text, true);

        //        //Update Treeview
        //        AddNode(xmlNode, tvMetadata.SelectedNode);
        //    }
        //}

        
        private void tsmiAddElement_Click_Old(object sender, EventArgs e)
        {
            ToolStripMenuItem item = (ToolStripMenuItem)sender;
            XmlNodeType nodeType = (XmlNodeType)item.Tag;

            //Create list to determine the selected node path.
            List<string> pathNames = new List<string>();

            TreeNode treeNode = tvMetadata.SelectedNode;
            while(treeNode != null) {
                TreeNodeData nodeData = treeNode.Tag as TreeNodeData;

                //Don't add the root node.
                if(treeNode.Parent != null) {
                    pathNames.Insert(0, nodeData.Name);
                }

                treeNode = treeNode.Parent;
            }

            //TODO: Support multiple nodes with the same name. Need to support this when loading as well. 
            //Currently only adds to the first node of a given name.
            //Add node to all metadata.
            foreach(KeyValuePair<string, XmlDocument> documentPair in mdDocuments) {
                XmlDocument document = documentPair.Value;

                //Save the first node we edit:
                TreeNode baseTreeNode = tvMetadata.Nodes[0];
                XmlNode baseXmlNode = document.DocumentElement;

                //Build node path for each document. (Adds nodes that dont exist that should.)
                XmlNode xmlNode = document.DocumentElement;
                foreach(string nodeName in pathNames) {
                    if(xmlNode[nodeName] == null) { //Node does not exist.
                        XmlElement pathNode = document.CreateElement(nodeName);

                        //TODO: Add child to the correct location based on the schema.
                        xmlNode.AppendChild(pathNode);
                    } else {
                        //TODO: This would be more efficient if we go down to the last node added. Currently it only 
                        //goes to the last node - 1 deep. (If we add to idinfo it sets idinfo as the base when we can 
                        //get away with setting the new node as the base).
                        baseXmlNode = xmlNode[nodeName];
                        baseTreeNode = baseTreeNode.Nodes[nodeName];
                    }

                    xmlNode = xmlNode[nodeName];
                }

                //Add the new node to the previous element in the path.
                XmlElement newNode = document.CreateElement(item.Text);

                if(nodeType == XmlNodeType.Text) {
                    XmlText textNode = document.CreateTextNode(string.Empty);
                    newNode.AppendChild(textNode);
                }

                xmlNode.AppendChild(newNode);

                //Update Treeview
                AddNode(baseXmlNode, baseTreeNode);
            }

            return;
        }

        private void tsmiDeleteElement_Click(object sender, EventArgs e)
        {
            TreeNodeData nodeData = tvMetadata.SelectedNode.Tag as TreeNodeData;

            XmlNode[] xmlNodes = nodeData.NodeRefs.ToArray();
            for(int i = 0; i < xmlNodes.Length; ++i) {
                xmlNodes[i].ParentNode.RemoveChild(xmlNodes[i]);
            }

            tvMetadata.SelectedNode.Parent.Nodes.Remove(tvMetadata.SelectedNode);
        }

        #endregion

        void Search(bool replace)
        {
            //replace=true then is in replace mode
            if(frmSearch == null || !frmSearch.Visible) {
                frmSearch = new FormSearch(frmSearch);
            } else {
                frmSearch.Activate();
            }

            frmSearch.Target = new XmlTreeViewFindTarget(this.tvMetadata);
            frmSearch.ReplaceMode = replace;

            if(!frmSearch.Visible) {
                frmSearch.Show(this); // modeless
            }
        }

        private void cbValues_SelectedIndexChanged(object sender, EventArgs e)
        {
            tbCurrentValue.Text = (string)cbValues.SelectedItem;
        }

                    
        //InsertElement
        //private void InsertElement(XmlNode xmlNode, string newElementName, bool recursive, List<string> nodeTrail = null)
        //{
        //    //TODO: Do validation on returned nodes. Eg. Check for choices and don't allow adding nodes to invalid node trees. ect.
        //    XmlSchemaElement schemaElement = schemaSet.GlobalElements[new XmlQualifiedName(newElementName, null)] as XmlSchemaElement;
            
        //    //Add the new node to the previous element in the path.
        //    XmlElement newNode = xmlNode.OwnerDocument.CreateElement(newElementName);

        //    //Add text element if needed.
        //    if(schemaElement.ElementSchemaType is XmlSchemaSimpleType) {
        //        XmlText textNode = xmlNode.OwnerDocument.CreateTextNode(string.Empty);
        //        newNode.AppendChild(textNode);
        //        recursive = false;
        //    }

        //    if(recursive) {
        //        if(nodeTrail == null) {
        //            nodeTrail = new List<string>();
        //        }

        //        nodeTrail.Add(newElementName);

        //        foreach(XmlSchemaElement childElement in schemaElement.GetChildrenElements()) {
        //            bool nodeRepeat = false;
        //            foreach(string nodeName in nodeTrail) {
        //                if(childElement.QualifiedName.Name == nodeName) {
        //                    nodeRepeat = true;
        //                    break;
        //                }
        //            }

        //            if(!nodeRepeat)
        //                InsertElement(newNode, childElement.QualifiedName.Name, recursive, nodeTrail);
        //        }
        //    }

        //    xmlNode.AppendChild(newNode);
        //}

        void UpdateProgressBar(int value, int maximum)
        {
            // Make sure we're on the right thread
            if(this.InvokeRequired == false) {
                tslblStatus.Text = "Processed: " + string.Format("{0} / {1}", value, maximum);
                tspProgressBar.Value = value;
                tspProgressBar.Maximum = maximum;
                tspProgressBar.Visible = true;
            } else {
                // Show progress asynchronously
                //DS  See ThreadSafeUIExt helper class
                this.InvokeSync<int, int>((t1, t2) => UpdateProgressBar(t1, t2), value, maximum);
            }
        }

        private void importMetaDataToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (mdDocuments.Count > 0)
            {
                MetaDataImporter importMetadataFrm = new MetaDataImporter(ref mdDocuments);
                importMetadataFrm.Activate();
                importMetadataFrm.Show(this);
            }
            else { MessageBox.Show(this,"Load files before importing metadata","No Files Loaded",MessageBoxButtons.OK,MessageBoxIcon.Exclamation);}

        }
           

        private void viewHelpToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //WebBrowser webnav = new WebBrowser();
            //webnav.Navigate("http://dev.insideidaho.org");
            //Process.Start("http://dev.insideidaho.org");            
            Process.Start("http://cloud.insideidaho.org/HelpDocs/batch_metadata_modifier_tool.html");
        }

        private void MetadataForm_Resize(object sender, EventArgs e)
        {
            //lboChangeLog.Width = flpMain.ClientSize.Width - 20;
            ////lboChangeLog.Height = flpMain.ClientSize.Height-200;
            cbValues.Width = splitContainer2.Panel2.Width - 20;
            tbCurrentValue.Width = splitContainer2.Panel2.Width - 20;
            lboChangeLog.Width = splitContainer2.Panel2.Width - 20;
            lboChangeLog.Height = splitContainer2.Panel2.Height - 230;
            //lboChangeLog.Height = Convert.ToInt32( splitContainer2.Panel2.Height * .395);
            splitContainer2.SplitterDistance = 292;
        }

        private void splitContainer2_SplitterMoved(object sender, SplitterEventArgs e)
        {
            cbValues.Width = splitContainer2.Panel2.Width - 20;
            tbCurrentValue.Width = splitContainer2.Panel2.Width - 20;
            lboChangeLog.Width = splitContainer2.Panel2.Width - 20;
            lboChangeLog.Height = splitContainer2.Panel2.Height - 230;
        }

        private void direcotytoolStripButton_Click(object sender, EventArgs e)
        {
            //use FolderBrowserDialog
            FolderBrowserDialog foDialog = new FolderBrowserDialog();
            foDialog.ShowNewFolderButton = false;
            foDialog.RootFolder = Environment.SpecialFolder.MyComputer;
            foDialog.Description = "Please select a directory of Metadata files. This will include all sub-directories";
            

            if (foDialog.ShowDialog() == DialogResult.OK)
            {
                string[] metadatafiles = Directory.GetFiles(foDialog.SelectedPath, "*.xml", SearchOption.AllDirectories);
                List<string> filesToEdit = new List<string>();
                filesToEdit =  metadatafiles.ToList();

                foreach (string s in metadatafiles)
                {
                    if (s.Contains(".aux.xml"))
                    {
                        //Console.WriteLine(s);
                        filesToEdit.Remove(s);
                    }  
                } 
                LoadDocumentsDelegate loadDocuments = new LoadDocumentsDelegate(LoadDocuments);
                loadDocuments.BeginInvoke(filesToEdit.ToArray(), null, null);
            }
        }            
                       
    }
}
