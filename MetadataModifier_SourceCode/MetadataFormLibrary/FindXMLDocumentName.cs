using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace MetadataFormLibrary
{
    /*
     * This class is called whenever the option to use the filename as the replacement string 
     * is chosen on the frmFindReplace form.
     */

    public static class FindXMLDocumentName
    {
       
        public static string FindTheNameOfTheXMLFile(XmlNode xmlNodeParser, out string strFilePath)
        {
            // We're going to break the filename down into its components to find the dataset name 
            strFilePath = xmlNodeParser.OwnerDocument.BaseURI.ToString();

            #region Split the BaseURI into components

            string[] strArrFileNameParts = strFilePath.Split('/');
            foreach (string word in strArrFileNameParts)
            {

                if (word.Contains(".xml") == true) // detect the metadata file
                {
                    strFilePath = word.Replace(".xml", "");
                    strFilePath = strFilePath.Replace(".shp", "");
                    strFilePath = strFilePath.Replace(".shpx", "");
                    strFilePath = strFilePath.Replace(".gdb", "");
                    strFilePath = strFilePath.Replace(".nc", "");
                    strFilePath = strFilePath.Replace(".html", "");
                } // end if

            }// end foreach

            #endregion

            return strFilePath;
        } // end FindTheNameOfTheXMLFile()

    }
}

