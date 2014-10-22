using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Xml;
using System.Windows.Forms;

namespace MetadataFormLibrary
{
    public static class Utils
    {
        public static string GetXPathToNode(XmlNode node)
        {
            if(node.NodeType == XmlNodeType.Attribute) {
                // attributes have an OwnerElement, not a ParentNode; also they have
                // to be matched by name, not found by position
                return String.Format(
                    "{0}/@{1}",
                    GetXPathToNode(((XmlAttribute)node).OwnerElement),
                    node.Name
                    );
            }
            if(node.ParentNode == null) {
                // the only node with no parent is the root node, which has no path
                return "";
            }
            //get the index
            int iIndex = 1;
            XmlNode xnIndex = node;
            while(xnIndex.PreviousSibling != null) { iIndex++; xnIndex = xnIndex.PreviousSibling; }
            // the path to a node is the path to its parent, plus "/node()[n]", where 
            // n is its position among its siblings.
            return String.Format(
                "{0}/node()[{1}]",
                GetXPathToNode(node.ParentNode),
                iIndex
                );
        }

        /// <summary>
        /// Checks to see if the tree node contains a node with a given name.
        /// </summary>
        /// <param name="node">TreeNode to search</param>
        /// <param name="elementName">Name of Node to search for</param>
        /// <returns>If the node is found return the node else return null.</returns>
        public static TreeNode TreeNodeContainsElemement(TreeNode node, string elementName)
        {
            for(int i = 0; i < node.Nodes.Count; ++i) {
                if(node.Nodes[i].Text == elementName) {
                    return node.Nodes[i];
                }
            }

            return null;
        }

        /// <summary>
        /// Gets the node by the elementname at the current index relative to the name.
        /// </summary>
        /// <param name="node">TreeNode to search</param>
        /// <param name="elementName">Name of Node to search for</param>
        /// <param name="index">Index of the node relative to name</param>
        /// <returns>If the node is found return the node else return null.</returns>
        public static TreeNode TreeNodeByElemementNameAtIndex(TreeNode node, string elementName, int index)
        {
            int count = 0;
            for(int i = 0; i < node.Nodes.Count; ++i) {
                if(node.Nodes[i].Text == elementName) {
                    if(count++ == index) {
                        return node.Nodes[i];
                    }
                }
            }

            return null;
        }

        //Extension method for string class
        public static string Replace(this string orig, int index, int length, string replacement)
        {
            string returnVal = string.Empty;
            //DS Appear to be getting a parsing error if the element only contains one word
            //Changed logic to fix parsing error
            //index = the zero-based number where replacement starts i.e. aaaabbb, if replacing bbb then index = 4 (the 5th position)
            //lenght = the length of the text being replaced  i.e. aaaabbb, if bbb being replace len = 3
            //remainder = value should never be less than zero... if so its an error            
            int remainder = orig.Length - (index + length);
            ////int end = Math.Min(index + length, orig.Length - 1);
            string head = (index > 0) ? orig.Substring(0, index) : "";
            ////string tail = (end < orig.Length) ? orig.Substring(end) : "";
            string tail = (remainder > 0) ? orig.Substring(index+length) : "";
            
            return head + replacement + tail;
            
        }
        public static string Replace(this string orig, int index, int length, string searchVal, string replacement)
        {
            string returnVal = string.Empty;
            //DS Appear to be getting a parsing error if the element only contains one word
            //Changed logic to fix parsing error
            //index = the zero-based number where replacement starts i.e. aaaabbb, if replacing bbb then index = 4 (the 5th position)
            //lenght = the length of the text being replaced  i.e. aaaabbb, if bbb being replace len = 3
            //remainder = value should never be less than zero... if so its an error            
            int remainder = orig.Length - (index + length);
            ////int end = Math.Min(index + length, orig.Length - 1);
            string head = (index > 0) ? orig.Substring(0, index) : "";
            ////string tail = (end < orig.Length) ? orig.Substring(end) : "";
            string tail = (remainder > 0) ? orig.Substring(index+length) : "";

            string targetReplaceValue = searchVal; //orig.Substring(index, length);
            string newNodeInnerText = orig.Replace(targetReplaceValue, replacement);
            //MessageBox.Show("Target Value to replace: "+ targetReplaceValue +" Start index:"+ index +" Len: "+ length+ "  New: " + newNodeInnerText);

            //return head + replacement + tail;
            return newNodeInnerText;
        }

        public static DataTable changeLogDT = new DataTable();
        public static List<string> esriFilePathList = new List<string>();
    }
    
}
