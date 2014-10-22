using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Windows.Forms;

namespace MetadataFormLibrary
{
    class TreeNodeData
    {
        public enum Icons
        {
            Folder,
            FolderWarning,
            FolderError,
            Text,
            TextWarning,
            TextError
        }

        //public string Metadata { get; set; }
        public HashSet<string> ValueSet { get; set; }
        public HashSet<XmlNode> NodeRefs { get; set; }
        public TreeNode TreeNode { get; set; }
        public XmlNodeType Type { get; set; }
        public string Name { get; set; }
        public Icons ImageIndex { get; set; }
        
        public TreeNodeData()
        {
            ValueSet = new HashSet<string>();
            NodeRefs = new HashSet<XmlNode>();
        }

        public static void AddIcons(ImageList imageList)
        {
            imageList.Images.Add("Folder", MetadataFormLibrary.Properties.Resources.FolderIcon16);
            imageList.Images.Add("FolderWarning", MetadataFormLibrary.Properties.Resources.FolderWarningIcon16);
            imageList.Images.Add("FolderError", MetadataFormLibrary.Properties.Resources.FolderErrorIcon16);
            imageList.Images.Add("Text", MetadataFormLibrary.Properties.Resources.TextIcon16);
            imageList.Images.Add("TextWarning", MetadataFormLibrary.Properties.Resources.TextErrorIcon16);
            imageList.Images.Add("TextError", MetadataFormLibrary.Properties.Resources.TextErrorIcon16);
        }
    }
}
