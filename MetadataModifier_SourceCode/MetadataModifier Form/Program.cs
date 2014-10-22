using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using MetadataFormLibrary;

namespace MetadataModifier_Test_Form
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new MetadataForm());
        }
    }
}
