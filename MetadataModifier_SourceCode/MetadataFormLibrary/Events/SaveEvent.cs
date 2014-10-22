using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MetadataFormLibrary
{
    public delegate void SaveEventHandler(object sender, SaveEventArgs e);

    public class SaveEventArgs : EventArgs
    {
        public int Index { get; set; }
        public string XML { get; set; }

        public SaveEventArgs(int index, string xml) {
            Index = index;
            XML = xml;
        }
    }
}
