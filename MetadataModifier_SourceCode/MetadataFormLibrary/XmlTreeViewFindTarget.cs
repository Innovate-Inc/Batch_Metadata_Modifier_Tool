using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xml;
using System.Windows.Forms;
using System.Text.RegularExpressions;
using System.Xml;
using System.Drawing;

namespace MetadataFormLibrary
{
    internal class XmlTreeViewFindTarget : IFindTarget
    {
        TreeView treeView;
        string expression;
        FindFlags flags;
        SearchFilter filter;
        Regex regex;
        StringComparison comp;
        List<NodeDataMatch> nodeList;
        NodeDataMatch match;
        XmlNamespaceManager nsmgr; //Really trying to ignore this.

        bool Backwards { get { return (flags & FindFlags.Backwards) != 0; } }
        bool IsXPath { get { return (flags & FindFlags.XPath) != 0; } }
        bool IsRegex { get { return (flags & FindFlags.Regex) != 0; } }
        bool MatchCase { get { return ((this.flags & FindFlags.MatchCase) != 0); } }
        bool WholeWord { get { return (this.flags & FindFlags.WholeWord) != 0; } }

        private class NodeDataMatch
        {
            public NodeDataMatch(TreeNodeData nodeData, int index, int length)
            {
                NodeData = nodeData;
                Indexes = new List<int>();
                Lengths = new List<int>();

                Indexes.Add(index);
                Lengths.Add(length);
            }

            public TreeNodeData NodeData { get; set; }
            public List<int> Indexes { get; set; }
            public List<int> Lengths { get; set; }
        }

        public XmlTreeViewFindTarget(TreeView treeView)
        {
            this.treeView = treeView;
        }

        #region IFindTarget Members

        public Rectangle MatchRect
        {
            get
            {
                if(match != null) {
                    Rectangle bounds;
                    //if(match.IsName) {
                    bounds = treeView.Bounds;
                    //}
                    return bounds;
                }
                return Rectangle.Empty;
            }
        }

        public FindResult FindNext(string expression, FindFlags flags, SearchFilter filter)
        {
            CheckCurrentState(expression, flags, filter);

            if(string.IsNullOrEmpty(expression))
                return FindResult.None;

            this.match = null;

            if(this.nodeList == null) {
                FindNodes();
            }

            // In case user changed the selection since the last find.
            int pos = FindSelectedNodeIndex();
            int wrap = -1;
            bool first = true;
            bool hasSomething = this.nodeList.Count > 0;
            while(this.nodeList != null && hasSomething && (first || pos != wrap))
            {
                first = false;
                if(wrap == -1) wrap = pos;
                if(this.Backwards)
                {
                    pos--;
                    if(pos < 0) pos = nodeList.Count - 1;
                }
                else
                {
                    pos++;
                    if(pos >= nodeList.Count) pos = 0;
                }

                NodeDataMatch m = nodeList[pos] as NodeDataMatch;
                this.match = m;


                if(m.NodeData.TreeNode == treeView.SelectedNode) {
                    continue;
                }

                treeView.SelectedNode = m.NodeData.TreeNode;

                return FindResult.Found;
            }

            return hasSomething ? FindResult.NoMore : FindResult.None;
        }

        public bool ReplaceCurrent(string searchValue, string replaceWith)
        {
            if(this.match != null)
            {
                //////////////////////////////////////////////////////////
                //DS Comments 3/27/2013
                //this.match will return a set of nodes matching the search expression found in the treeView
                //If a value is found multiple times on the same node, the node reference is repeated
                //This can cause problems as values are processed in the XML node but not updated in the treeView
                //Therefore each node should be processed as a whole at once.
                //Need to process values/characters on the same node together. E.g., a repeated word within the text of an abstract
                //Need to process values/characters on the same node but different file
                //Need to process values on a repeated element
                /////////////////////////////////////////////////////////
                
                #region Test Area
                //List<NodeDataMatch> sublist = new List<NodeDataMatch>();
                
                ////Find the index of the first occurance of the this.match from the node list.
                ////Then remove it from the list so that it is not processed twice                
                //int removeindex = this.nodeList.FindIndex(delegate(NodeDataMatch i) { return i == this.match; });
                //this.nodeList.RemoveAt(removeindex);

                ////Populate the sublist with the remaining unique nodes
                ////Since some terminal elements are repeated and can have the same TreeNode.FullPath I added it's Treenode Index                
                //foreach (NodeDataMatch selectNode in this.nodeList)
                //{
                //    string a = selectNode.NodeData.TreeNode.FullPath + selectNode.NodeData.TreeNode.Index;
                //    string b = match.NodeData.TreeNode.FullPath + match.NodeData.TreeNode.Index;
                //    if (a != b)
                //    {                       
                //        sublist.Add(selectNode);
                //    }
                //}
                ////Reset the application node list with the unique node list
                //this.nodeList = sublist;
                #endregion

                this.nodeList.Remove(this.match);
                TreeNodeData nodeData = this.match.NodeData;
                if(nodeData.Type == XmlNodeType.Text)
                {
                    XmlNode[] xmlNodes = nodeData.NodeRefs.ToArray();
                    for(int i = 0; i < nodeData.NodeRefs.Count; ++i)
                    {
                        XmlNode xmlNode = xmlNodes[i];
                        //See Line 80 Utils.public static string replace  //This method no longer used.
                        //xmlNode.InnerText = xmlNode.InnerText.Replace(this.match.Indexes[i], this.match.Lengths[i], searchValue, replaceWith);                      
                        
                        //Test the value exist to prevent out-of-index error
                        bool hasSearchValue = xmlNode.InnerText.Contains(searchValue);
                        if (hasSearchValue == true)
                        {
                            xmlNode.InnerText = xmlNode.InnerText.Replace(searchValue, replaceWith);//.Replace is case sensitive
                            Utils.changeLogDT.Rows.Add("New Value: " + xmlNode.OuterXml);
                        }
                    }                    
                }
                return true;
            }
            return false;
        }
        public bool ReplaceCurrentwFileName(string searchValue)
        {
            
            #region Old Method
            if (this.match != null)
            {
                this.nodeList.Remove(this.match);

                TreeNodeData nodeData = this.match.NodeData;
                if (nodeData.Type == XmlNodeType.Text)
                {
                    //DS
                    //Utils.changeLogDT.Rows.Add(nodeData.NodeRefs.Count + " records updated with filenames:");
                    XmlNode[] xmlNodes = nodeData.NodeRefs.ToArray();
                    for (int i = 0; i < nodeData.NodeRefs.Count; ++i)
                    {
                        XmlNode xmlNode = xmlNodes[i];
                        string fileNameString = FindXMLDocumentName.FindTheNameOfTheXMLFile(xmlNode, out fileNameString);
                        //xmlNode.InnerText = xmlNode.InnerText.Replace(this.match.Indexes[i], this.match.Lengths[i], fileNameString);                        
                        //System.Diagnostics.Debug.WriteLine("Replacement Name: " + xmlNode.InnerText);                        
                        //Utils.changeLogDT.Rows.Add(xmlNodes[i].OuterXml);

                        //Test the value exist to prevent out-of-index error
                        bool hasSearchValue = xmlNode.InnerText.Contains(searchValue);
                        if (hasSearchValue == true)
                        {
                            xmlNode.InnerText = xmlNode.InnerText.Replace(searchValue, fileNameString);//.Replace is case sensitive
                            Utils.changeLogDT.Rows.Add("New Value: " + xmlNode.OuterXml);
                        }
                    }
                }
                return true;
            }
            return false;
            #endregion

        }      


        public string Location
        {
            get
            {
                return this.GetLocation();
            }
        }

        public XmlNamespaceManager Namespaces
        {
            get
            {
                if(nsmgr == null) {
                    this.GetLocation();
                }
                return nsmgr;
            }
            set
            {
                this.nsmgr = value;
            }
        }

        #endregion

        private void CheckCurrentState(string expression, FindFlags flags, SearchFilter filter)
        {
            if(this.expression != expression || this.flags != flags || this.filter != filter) {
                this.expression = expression;
                this.flags = flags;
                this.filter = filter;
                this.nodeList = null;
                this.match = null;
            }
        }

        private void FindNodes()
        {
            this.regex = null;
            if(IsRegex) {
                this.regex = new Regex(this.expression);
            }

            this.comp = MatchCase ? StringComparison.CurrentCulture : StringComparison.CurrentCultureIgnoreCase;

            this.nodeList = new List<NodeDataMatch>();

            //TODO: Maybe we can make subnode feature an option in the find/replace dialog or even a hotkey/meny item that 
            //starts the find/replace dialog in subtree mode. For now this is an easy fix.
            //foreach(TreeNode treeNode in treeView.Nodes) {
            if(treeView.SelectedNode != null)
                MatchNodes(treeView.SelectedNode);
            else
                MatchNodes(treeView.TopNode);
            //}
        }

        private void MatchNodes(TreeNode treeNode)
        {
            TreeNodeData nodeData = treeNode.Tag as TreeNodeData;

            XmlNode[] xmlNodes = nodeData.NodeRefs.ToArray();

            if(nodeData.Type == XmlNodeType.Element) { //Named Node
                if(xmlNodes.Length > 0)
                    MatchNode(nodeData, xmlNodes[0]);
            } else if(nodeData.Type == XmlNodeType.Text) {
                for(int i = 0; i < nodeData.NodeRefs.Count; ++i) {
                    MatchNode(nodeData, xmlNodes[i]);
                }
            }

            // Iterate through the child nodes of the parent node passed into
            // this method and display their values.
            for(int i = 0; i < treeNode.Nodes.Count; ++i) {
                // Recursively call the DisplayChildNodeText method to
                // traverse the tree and display all the child nodes.
                MatchNodes(treeNode.Nodes[i]);

            }
        }

        private void MatchNode(TreeNodeData nodeData, XmlNode xmlNode)
        {
            if(!IsXPath) {
                if(nodeData.Type == XmlNodeType.Element && (filter == SearchFilter.Names || filter == SearchFilter.Everything)) {
                    MatchStrings(nodeData, nodeData.Name);
                }
                if(nodeData.Type == XmlNodeType.Text && (filter == SearchFilter.Text || filter == SearchFilter.Everything)) {
                    MatchStrings(nodeData, xmlNode.InnerText);
                }
            } else {
                if(xmlNode == (xmlNode.SelectSingleNode(this.expression))) {
                    NodeDataMatch lastMatch = nodeList.Count > 0 ? nodeList[nodeList.Count - 1] : null;
                    if(lastMatch != null && lastMatch.NodeData == nodeData) {
                        lastMatch.Indexes.Add(0);
                        lastMatch.Lengths.Add(int.MaxValue);
                    } else {
                        nodeList.Add(new NodeDataMatch(nodeData, 0, int.MaxValue));
                    }
                }
            }
        }

        private void MatchStrings(TreeNodeData nodeData, string value)
        {
            if(string.IsNullOrEmpty(value))
                return;
            //DS  If Element only contains 1-word the last letter is not being parsed
            // Normalize the newlines the same way the text editor does so that we
            // don't get off-by-one errors after newlines.
            if(value.IndexOf('\n') >= 0 && value.IndexOf("\r\n") < 0) {
                value = value.Replace("\n", "\r\n");
            }

            string[] strings = new string[] { value };

            char[] ws = new char[] { ' ', '\t', '\n', '\r', '.', ',', ';', '!', '\'', '"', '+', '=', '-', '<', '>', '(', ')' };
            if(value.IndexOfAny(ws) >= 0 && WholeWord) {
                strings = value.Split(ws, StringSplitOptions.RemoveEmptyEntries);
            }
            int len = this.expression.Length;
            int index = 0;

            foreach(string word in strings) {
                index = value.IndexOf(word, index);

                NodeDataMatch lastMatch = nodeList.Count > 0 ? nodeList[nodeList.Count - 1] : null;
                    
                if(this.regex != null) {
                    foreach(Match m in this.regex.Matches(word)) {
                        if(lastMatch != null && lastMatch.NodeData == nodeData) {
                            lastMatch.Indexes.Add(m.Index + index);
                            lastMatch.Lengths.Add(m.Length);
                        } else {
                            nodeList.Add(new NodeDataMatch(nodeData, m.Index + index, m.Length));
                        }
                    }
                } else if(this.WholeWord) {
                    if(string.Compare(this.expression, word, comp) == 0) {
                        if(lastMatch != null && lastMatch.NodeData == nodeData) {
                            lastMatch.Indexes.Add(index);
                            lastMatch.Lengths.Add(len);
                        } else {
                            nodeList.Add(new NodeDataMatch(nodeData, index, len));
                        }
                    }
                } else {
                    int j = word.IndexOf(this.expression, 0, comp);
                    while(j >= 0) {
                        if(lastMatch != null && lastMatch.NodeData == nodeData) {
                            lastMatch.Indexes.Add(j + index);
                            lastMatch.Lengths.Add(len);
                        } else {
                            nodeList.Add(new NodeDataMatch(nodeData, j + index, len));
                        }
                        j = word.IndexOf(this.expression, j + len, comp);
                    }
                }
            }
        }

        private int FindSelectedNodeIndex()
        {
            // Now find where the selectedNode is in the matching list so we can start there.
            int index = -1;
            TreeNodeData selectedNodeData = null;
            TreeNode selected = treeView.SelectedNode;

            if(selected != null && nodeList != null) {
                selectedNodeData = selected.Tag as TreeNodeData;
                if(selectedNodeData != null) {
                    // I'm not using XPathNavigator.ComparePosition because it is returning XmlNodeOrder.Unknown
                    // sometimes which is not very useful!
                    for(int i = 0; i < nodeList.Count; ++i) {
                        if(nodeList[i].NodeData == selectedNodeData) {
                            return i; // selected node is one of the matching nodes.
                        }
                    }
                }
            }

            //if(Backwards) index++;

            return index;
        }

        private string GetLocation()
        {
            TreeNodeData nodeData = treeView.SelectedNode.Tag as TreeNodeData;

            return GetXPath(nodeData);
        }

        private string GetXPath(TreeNodeData nodeData)
        {
            string path = null;

            XmlNode[] xmlNodes = nodeData.NodeRefs.ToArray();
            if(xmlNodes.Length > 0) {
                XmlNode xnode = xmlNodes[0];
                if(xnode != null) {
                    XPathGenerator gen = new XPathGenerator();
                    path = gen.GetXPath(xnode, nsmgr);
                }
            }

            return path;
        }
    }
}
