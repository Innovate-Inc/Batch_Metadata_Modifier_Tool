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
    public partial class MetaDataImporter : Form
    {
        /// <summary>
        /// The xml template file
        /// </summary>
        private List<KeyValuePair<string, XmlDocument>> mdDocuments = new List<KeyValuePair<string, XmlDocument>>();
        /// <summary>
        /// The xml files in the main form
        /// </summary>
        private List<KeyValuePair<string, XmlDocument>> mdDocumentsImporter = new List<KeyValuePair<string, XmlDocument>>();        
        private List<string> checkedNodeList;        
        private XMLValidation validator;
        private XmlSchemaSet schemaSet;
        private XmlNamespaceManager isoNsManager;

        // 8/30/2013 Need to update import to preserve the structure of the metadata records.  If ISO elements are not
        // placed in the correct order the document will not schema validate.

        public MetaDataImporter(ref List<KeyValuePair<string, XmlDocument>> mdDocuments2)
        {
            if (mdDocuments2.Count > 0)
            {
                InitializeComponent();                
                
                toolTip1.SetToolTip(cmdAppendContent, "Selected elements for import will be appended to the target metadata records." + Environment.NewLine +
                    "This will preserve existing occurrences of matching elements within the target" + Environment.NewLine +
                    "metadata record(s) and potentially create repeated elements.");
                toolTip1.SetToolTip(cmdReplaceContent, "Selected elements for import will replace all occurrences found within the" + Environment.NewLine + 
                    "target metadata records.  Existing matching elments in target records will be removed.");

                //Refernce of the mdDocs open in the main form is passed into this form.
                mdDocumentsImporter = mdDocuments2;

                TreeNodeData.AddIcons(ilTvMetaImporter);
                checkedNodeList = new List<string>();
                
                #region Old Schema Import Method
                               
                //string validationXSD = @"http://www.fgdc.gov/schemas/metadata/fgdc-std-001-1998.xsd";
                //string validationXSD = @"http://dev.insideidaho.org/HelpDocs/metadata/tool/schemas/fgdc-std-001-1998/fgdc-std-001-1998.xsd";
                //string validationXSD = Directory.GetCurrentDirectory() + @"\Resources\fgdc-std-001-1998.xsd";
                //string validationXSD = Directory.GetCurrentDirectory() + @"\fgdc-std-001-1998-ann\fgdc-std-001-1998-ann.xsd";
                //string validationXSD = @"C:\Documents and Settings\jgentry\My Documents\Schemas\iso\gmd.xsd";
                //string validationXSD = @"http://www.isotc211.org/2005/gmi/gmi.xsd";

                //validator = new XMLValidation(validationXSD);
                //XmlSchema xs = XmlSchema.Read(XmlReader.Create(validationXSD), null);
                //schemaSet = new XmlSchemaSet();
                //schemaSet.Add(xs);
                //schemaSet.Compile();
                #endregion
                //New Method.  Shouldn't need to load any xsd files unless it has namespaces
                schemaSet = new XmlSchemaSet();                            
                schemaSet.Add(new XmlSchema());
                schemaSet.Compile();

                //Load required namespaces
                NameTable nt = new NameTable();
                isoNsManager = new XmlNamespaceManager(nt);
                #region Removed the URL since not validating
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

            }                      
        }
       
        public void LoadDocument(string filename)
        {
            tvMetadataImportElements.Nodes.Clear();
            XmlDocument xmlDoc = new XmlDocument();
            try
            {
                string sourceDocRootNode = mdDocumentsImporter[0].Value.DocumentElement.Name;
                xmlDoc.Load(filename);
                string importDocRootNode = xmlDoc.DocumentElement.Name;
                if (sourceDocRootNode != importDocRootNode)
                {
                    MessageBox.Show(this, "Root Node Mismatch", "Error Loading file!", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return;
                }
                
                mdDocuments.Add(new KeyValuePair<string, XmlDocument>(filename, xmlDoc));
                                
                tvMetadataImportElements.BeginUpdate();

                if (tvMetadataImportElements.Nodes.Count == 0)//TSGetCount() == 0)
                {
                    //tvMetadataImportElements.TSAdd(tvMetadataImportElements.Nodes, new TreeNode(xmlDoc.DocumentElement.Name));
                    tvMetadataImportElements.Nodes.Add(new TreeNode(xmlDoc.DocumentElement.Name));
                }
                AddNode(xmlDoc.DocumentElement, tvMetadataImportElements.Nodes[0]);

                UpdateTreeView(tvMetadataImportElements.Nodes[0], ref tvMetadataImportElements);
                tvMetadataImportElements.EndUpdate();
                cmdReplaceContent.Enabled = true;
                cmdAppendContent.Enabled = true;
            }
            catch (Exception e)
            {
                MessageBox.Show("Error Loading XML Document");
            }
        }

        private void AddNode(XmlNode xmlNode, TreeNode treeNode)
        {
            if (xmlNode.Name != treeNode.Text)
                return;

            //Add NodeData to tag.
            if (treeNode.Tag == null) treeNode.Tag = new TreeNodeData();

            TreeNodeData nodeData = (TreeNodeData)treeNode.Tag;
            nodeData.Name = xmlNode.Name;
            nodeData.NodeRefs.Add(xmlNode);
            nodeData.TreeNode = treeNode;

            #region xsdValidation
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
            #endregion

            if (xmlNode.HasChildNodes && xmlNode.ChildNodes[0].NodeType != XmlNodeType.Text)
            {
                nodeData.Type = xmlNode.NodeType;

                for (int i = 0; i < xmlNode.ChildNodes.Count; ++i)
                {
                    //Create new nodes for each new node in a given doc otherwise use the current nodes. 
                    //We can only have a max number that equals the max number of nodes in any one doc. This allows
                    //us to support multiple nodes for multiple documents while being able to sync documents. (1st xml node always matches 1st tree node...)

                    //Get the current index of the xml (child) node from all children with the same name
                    int nodeIndex = 0;

                    if (xmlNode.ChildNodes[i].NodeType != XmlNodeType.Comment)
                    {
                        ////Modified By Bruce...                    
                        XmlNodeList childXMLNodeList = xmlNode.SelectNodes(xmlNode.ChildNodes[i].Name, isoNsManager);
                        ////End of Bruce's Modification

                        ////XmlNodeList childXMLNodeList = xmlNode.SelectNodes(xmlNode.ChildNodes[i].Name);
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
                            tvMetadataImportElements.TSAdd(treeNode.Nodes, newNode);
                        }

                        nodeData.ImageIndex = TreeNodeData.Icons.Folder;

                        AddNode(xmlNode.ChildNodes[i], newNode);
                    }
                }
            }
            else
            {
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
              
        private void UpdateTreeView(TreeNode treeNode, ref TreeView targetTreeview)
        {
            TreeNodeData nodeData = (TreeNodeData)treeNode.Tag;
            targetTreeview.TSSetImageKey(treeNode, (int)nodeData.ImageIndex);
            targetTreeview.TSSetSelectedImageKey(treeNode, (int)nodeData.ImageIndex);

            string parentName = nodeData.TreeNode.Parent != null ? nodeData.TreeNode.Parent.Text : string.Empty;
            XmlSchemaElement schemaElement = (XmlSchemaElement)schemaSet.GlobalElements[new XmlQualifiedName(parentName, null)];

            if (schemaElement != null && schemaElement.ElementSchemaType is XmlSchemaComplexType)
            {
                XmlSchemaComplexType complexType = (XmlSchemaComplexType)schemaElement.ElementSchemaType;
                XmlSchemaParticle particle = complexType.Particle;

                switch (particle.GetType().Name)
                {
                    case "XmlSchemaSequence":
                        XmlSchemaSequence sequence = (XmlSchemaSequence)particle;

                        for (int i = 0; i < sequence.Items.Count; ++i)
                        {
                            if (sequence.Items[i] is XmlSchemaElement)
                            {
                                XmlSchemaElement element = (XmlSchemaElement)sequence.Items[i];
                                if (element.QualifiedName.Name == nodeData.Name)
                                {
                                    if (element.MinOccurs > 0)
                                        nodeData.TreeNode.ForeColor = Color.Red;
                                }
                            }
                        }

                        break;
                }
            }

            for (int i = 0; i < treeNode.Nodes.Count; ++i)
            {
                UpdateTreeView(treeNode.Nodes[i], ref targetTreeview);
            }
        }
               
        
        /// <summary>
        /// Main method to import elements into source xml documents.  Updated 8/29/2013 to import repeated elemements
        /// and provide functionality to Replace or Append imported elements.  Append = true
        /// Method need to be updated to insert elements in ISO schema required order to prevent validation problems.
        /// </summary>
        /// <param name="appendElemnts"></param>
        private void importElements(bool appendElements)
        {
            try
            {
                //Create a list of each checked element.  It will contain the complete path to the root element "metadata"
                checkedNodeList.Clear();
                createCheckedNodeList(tvMetadataImportElements.Nodes[0]);

                if (checkedNodeList.Count > 0)
                {
                    if (appendElements == false) { removeNodesBeforeImport(); }

                    for (int i = 0; i < checkedNodeList.Count; ++i)
                    {
                        string[] xpathString = checkedNodeList[i].Split(';');
                        int importNodeIndex = Convert.ToInt16(xpathString[1]);//+1; //xpath expression is not zero-based!
                        string[] strNodeParts = xpathString[0].Split('/');

                        Debug.WriteLine("Importing: " + xpathString[0]);
                        Utils.changeLogDT.Rows.Add("Imported: " + xpathString[0]);

                        //** Loop this for each of the loaded metadata files
                        for (int docCount = 0; docCount < mdDocumentsImporter.Count; ++docCount)
                        {
                            ////Need to ensure the node-path exists all the way to destination node in the target.
                            ////Empty Parent nodes may have been removed in previous method.
                            ////If the node-path does not exist, import each node as needed.                                
                            ////Break down the nodepath into separate parts                                           

                            string strNodepathParent = "";
                            string strNodepathNextChild = "";

                            //From the root (metadata), find the last node and import each subsequent node all the way until
                            //the second to last node.  From there, the last element will be imported from the source xml doc

                            for (int ii = 0; ii < strNodeParts.Length - 1; ++ii)
                            {
                                if (ii == 0)
                                {
                                    //On first loop set the parent w/o a '/' in case user selects a node/element direclty under metadata
                                    strNodepathParent = strNodeParts[ii];
                                    strNodepathNextChild = strNodeParts[ii] + "/" + strNodeParts[ii + 1];
                                }
                                else
                                {
                                    strNodepathParent = strNodepathParent + "/" + strNodeParts[ii];
                                    strNodepathNextChild = strNodepathNextChild + "/" + strNodeParts[ii + 1];
                                }

                                Debug.WriteLine("Parent Node: " + strNodepathParent + " Child: " + strNodepathNextChild);
                                Debug.WriteLine(strNodepathNextChild);
                                //If = 0 then no node exists and needs to be inserted. Otherwise continue thru nodes                            
                                if (strNodepathNextChild != xpathString[0])
                                {
                                    if (mdDocumentsImporter[docCount].Value.SelectNodes(strNodepathNextChild, isoNsManager).Count == 0)
                                    {
                                        //If the node path is missing, create a shallow clone of the specified node.
                                        //A shallow clone is needed to prevent inclusion of extra child nodes not selected for import
                                        XmlNode sourceMissingNode = mdDocuments[0].Value.SelectSingleNode(strNodepathNextChild, isoNsManager);
                                        XmlNode clonedMissingNode = sourceMissingNode.CloneNode(false);

                                        XmlNode targetMissingNode = mdDocumentsImporter[docCount].Value.ImportNode(clonedMissingNode, true);
                                        mdDocumentsImporter[docCount].Value.SelectSingleNode(strNodepathParent, isoNsManager).AppendChild(targetMissingNode);

                                        //Difficult to use with ISO; Shallow clone is easier.
                                        //XmlElement missingElement = mdDocumentsImporter[docCount].Value.CreateElement(strNodeParts[ii + 1]);
                                        //XmlNode destNode = mdDocumentsImporter[docCount].Value.SelectSingleNode(strNodepathParent);
                                        //destNode.AppendChild(missingElement);                                    
                                    }
                                }
                                else
                                {
                                    //After node path is established insert the selected nodes for import.
                                    XmlNodeList sourceNodeList = mdDocuments[0].Value.SelectNodes(strNodepathParent + "/*", isoNsManager);
                                    XmlNode sourceNode = sourceNodeList[importNodeIndex];
                                    //XmlNode sourceNode = mdDocuments[0].Value.SelectSingleNode(strCurrentNode, isoNsManager);

                                    //Import creates a copy of the source node w/o reference to parent node
                                    XmlNode targetNode = mdDocumentsImporter[docCount].Value.ImportNode(sourceNode, true);
                                    //Add refernce to insertion point w/in node-tree
                                    mdDocumentsImporter[docCount].Value.SelectSingleNode(strNodepathParent, isoNsManager).AppendChild(targetNode);

                                }
                            }
                        }
                    }
                    MessageBox.Show(this, "Nodes imported. Save your work in the main form", "Import Complete", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    Utils.changeLogDT.Rows.Add("****Nodes imported.  Save your work.****");
                    this.Hide();
                    this.Owner.Focus();
                }
                else { MessageBox.Show(this, "Select a Node to Import", "No Nodes Selected", MessageBoxButtons.OK, MessageBoxIcon.Exclamation); }
            }
            catch (Exception e)
            {
                MessageBox.Show(this,"Error: " + e.Message,"Error Importing Nodes", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        private string createParentNodeXpath(string childNodeXpath)
        {
            string[] strNodeParts = childNodeXpath.Split('/');           

            string strNodepathParent = "";
            //string strNodepathNextChild = "";

            for (int ii = 0; ii < strNodeParts.Length - 1; ++ii)
            {
                if (ii == 0)
                {
                    //On first loop set the parent w/o a '/' in case user selects a node/element direclty under metadata
                    strNodepathParent = strNodeParts[ii];
                    //strNodepathNextChild = strNodeParts[ii] + "/" + strNodeParts[ii + 1];
                }
                else
                {
                    strNodepathParent = strNodepathParent + "/" + strNodeParts[ii];
                    //strNodepathNextChild = strNodepathNextChild + "/" + strNodeParts[ii + 1];
                }
            }
            return strNodepathParent;
        }

        /// <summary>
        /// Method to remove nodes before importing content.  All occurances of the specified node and children
        /// are removed.  Otherwise imported nodes will be appended.
        /// </summary>
        /// <param name="nodeRemovalList"></param>
        private void removeNodesBeforeImport()
        {
            for (int i = 0; i < checkedNodeList.Count; ++i)
            {                
                string[] xpathString = checkedNodeList[i].Split(';');//Removes the sibling index                
                string strCurrentNodeGeneral = xpathString[0];//Any orrcurance of this xpath expression
                Debug.WriteLine("Check for Existing Node: " + strCurrentNodeGeneral);
                
                //**** Loop this for each of the loaded metadata files
                for (int docCount = 0; docCount < mdDocumentsImporter.Count; ++docCount)
                {
                    //Remove all existing nodes if they exist
                    //**This will remove all occurances of this xpath expression since each document may be structured differently                                  

                    foreach (XmlNode xnode in mdDocumentsImporter[docCount].Value.SelectNodes(strCurrentNodeGeneral, isoNsManager))
                    {
                        XmlNode pNode = xnode.ParentNode;
                        Debug.WriteLine("Removing: " + xnode.OuterXml);
                        xnode.ParentNode.RemoveChild(xnode);  //use this to remove the node, vs. just it's child nodes

                        if (!pNode.HasChildNodes)
                        {
                            //Recursively remove all empty parent nodes now that the child element was deleted
                            //removeEmptyParentNodes(pNode);
                        }
                    }                   
                }
            }
        }
        
        private void removeEmptyParentNodes(XmlNode node)
        {
            //When deleting Elements, this removes empty parent nodes above the element
            if (node.Name == "metadata") { return; }
            else if (!node.HasChildNodes)
            {
                XmlNode pnode = node.ParentNode;
                Debug.WriteLine("Removing Parent: " + node.OuterXml);
                node.ParentNode.RemoveChild(node);
                if (pnode.ParentNode != null)
                {
                    removeEmptyParentNodes(pnode);
                }
            }
        }

        private void checkChildNodes(TreeNode node)
        {
            //This will check or uncheck child nodes when a parent node is checked
            //If some sibling nodes are unchecked while others are checked, the parent must be unchecked!
            bool ischecked = node.Checked;
            if (node.Nodes.Count != 0)
            {
                foreach (TreeNode childNode in node.Nodes)
                {
                    childNode.Checked = ischecked;
                    //Recursively check or uncheck all sub children
                    if (childNode.Nodes.Count != 0)
                    {
                        checkChildNodes(childNode);
                    }
                }
            }
        }

        private void uncheckNodes(TreeNode node)
        {
            bool ischecked = node.Checked;
            foreach (TreeNode childNode in node.Nodes)
            {
                childNode.Checked = ischecked;
                //Recursively check or uncheck all sub children
                if (childNode.Nodes.Count != 0)
                {
                    checkChildNodes(childNode);
                }

                //childNode.Parent.Checked = false;
            }
        }

        private bool HasCheckedChildNodes(TreeNode node)
        {
            // Returns a value indicating whether the specified  
            // TreeNode has checked child nodes.
            if (node.Nodes.Count == 0) return false;
            foreach (TreeNode childNode in node.Nodes)
            {
                if (childNode.Checked) return true;
                // Recursively check the children of the current child node. 
                if (HasCheckedChildNodes(childNode)) return true;
            }
            return false;
        }

        private void createCheckedNodeList(TreeNode node)
        {
            
            if (node.Nodes.Count != 0)
            {
                int siblingIndex = 0;
                foreach (TreeNode tNode in node.Nodes){
                    
                    int nodeSiblingCount = tNode.Parent.Nodes.Count;
                    if (tNode.Checked)
                    {
                        //If A parent is checked then all siblings will be added.  For repeated elements it is important to note the sibling
                        //index so that the correct sibling is imported since all similar siblings will be selected with the same xpath expression and
                        //placed in a zero-based nodelist array.
                                                                               
                        Debug.WriteLine(tNode.FullPath.ToString() + " Tag: " + tNode.Tag + " Sibling Count: " + nodeSiblingCount + " siblingIndex: " + siblingIndex);
                        string str = tNode.FullPath;
                        str = str.Replace("\\", "/");//this creates the propper xpath expression
                        checkedNodeList.Add(str +";"+siblingIndex);
                        
                    }
                    else
                    {
                        // Recursively check the children of the current child node since some of its children may be checked 
                        if (tNode.Nodes.Count != 0)
                        {
                            createCheckedNodeList(tNode);
                        }
                    }
                    siblingIndex++;
                }
            }
            #region Original
            //if (node.Nodes.Count != 0)
            //{
            //    foreach (TreeNode tNode in node.Nodes)
            //    {
            //        int nodeSiblingCount = tNode.Parent.Nodes.Count;
            //        int nodeSiblindIndex = 0;
            //        if (tNode.Checked)
            //        {

            //            //Check that it doesn't have any children. If none add to list.  This will give you
            //            //only the actual checked elements
            //            //if (childNode.GetNodeCount(true) == 0){                                                        
            //            Debug.WriteLine(tNode.FullPath.ToString() + " Tag: " + tNode.Tag);
            //            string str = tNode.FullPath;
            //            str = str.Replace("\\", "/");
            //            checkedNodeList.Add(str);
            //            //}
            //        }
            //        // Recursively check the children of the current child node. 
            //        if (tNode.Nodes.Count != 0)
            //        {
            //            createCheckedNodeList(tNode);
            //        }

            //    }
            //}
            #endregion

        }

        private void uncheckSingleNodes(TreeNode node)
        {            
            TreeNode currentNode = node.Parent;            
            currentNode.Checked = false;
            if (currentNode.Level > 0)
            {
                //Level = 0 is root node
                uncheckSingleNodes(currentNode);
            }            
        }
                
        private void tvMetadataImportElements_AfterCheck(object sender, TreeViewEventArgs e)
        {
            //tvMetadataImportElements.BeginUpdate();
            if (e.Action != TreeViewAction.Unknown)
            {
                if (e.Node.Checked == true)
                {
                    checkChildNodes(e.Node);
                }
                else if (e.Node.Checked == false)
                {
                    //Uncheck single nodes if some children are unchecked or else import will not work correctly
                    TreeNode currentnode = e.Node;                    
                    int siblingUnCheckCount = 0;
                    TreeNodeCollection pNodeCollection = currentnode.Parent.Nodes;
                    int siblingCount = pNodeCollection.Count;
                    foreach (TreeNode pNode in pNodeCollection)
                    {
                        if (pNode.Checked == false) { siblingUnCheckCount++; }
                    }
                    if (siblingUnCheckCount > 0)
                    {
                        //when this orrcurs check for single nodes up the chain and uncheck.
                        currentnode.Parent.Checked = false;
                        uncheckSingleNodes(currentnode.Parent);
                    }

                    checkChildNodes(e.Node);

                }
            }
            //tvMetadataImportElements.EndUpdate();
            //Debug.WriteLine("AfterCheckEvent Checked: "+e.Node.Checked + "  Node: " + e.Node.FullPath);
        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofDialog = new OpenFileDialog();
            ofDialog.Filter = "XML Metadata (*.XML|*.XML";
            ofDialog.Multiselect = false;
            ofDialog.Title = "Select the template metadata file you would like to open.";
            if (ofDialog.ShowDialog() == DialogResult.OK)
            {
                //string dom1Filename = @"C:\InnovateWork\Testing\TestData\test1.xml";
                //string dom1Filename = @"C:\zInnovate\Projects\UIMetaData\Testing\TestData\test1.xml";
                //LoadDocument(dom1Filename);
                LoadDocument(ofDialog.FileName);
            }

        }

        private void cmdAppendContent_Click(object sender, EventArgs e)
        {
            importElements(true);

        }
        private void cmdReplaceContent_Click(object sender, EventArgs e)
        {
            importElements(false);
        }

        private void tvMetadataImportElements_Click(object sender, EventArgs e)
        {

        }       
        
    }
}
