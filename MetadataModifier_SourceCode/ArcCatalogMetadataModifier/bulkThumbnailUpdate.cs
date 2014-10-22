using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using ESRI.ArcGIS.Catalog;
using ESRI.ArcGIS.CatalogUI;
using ESRI.ArcGIS.esriSystem;
using ESRI.ArcGIS.Framework;
using ESRI.ArcGIS.Geodatabase;
using MetadataFormLibrary;
using System.Windows.Forms;

namespace ArcCatalogMetadataModifier
{
    public class bulkThumbnailUpdate : ESRI.ArcGIS.Desktop.AddIns.Button
    {
        private string processLog;
        
        public bulkThumbnailUpdate()
        {
        }

        protected override void OnClick()
        {
            //Users must select items within the Contents view to generate thumbnails
            //The selection set is first put into a list of IgxObjects
            //For each item in the list, the view is changed to the Preview tab
            //If the GeographicView is available, the thumbnail tool is run

            processLog = "Operation Complete" + "\r\n";
            
            IGxSelection selection = ArcCatalog.ThisApplication.Selection;            
            IEnumGxObject selectedObjects = selection.SelectedObjects as IEnumGxObject;            
            List<IGxObject> selectedGxObjectList = new List<IGxObject>();

            if (selection.Count < 1)
            {
                MessageBox.Show("Select objects from Contents Tab before creating thumbnails");
            }
            else
            {
                IGxObject selectedObject;
                for (int i = 0; i < selection.Count; ++i)
                {
                    //Add Each selected object to a list
                    selectedObject = selectedObjects.Next();
                    selectedGxObjectList.Add(selectedObject);
                }


                //Clear the selection and loop through each item in the list individually.
                selection.Clear(null);
                for (int i = 0; i < selectedGxObjectList.Count; ++i)
                {
                    IGxApplication myGxApplication = ArcCatalog.ThisApplication;
                    myGxApplication.Selection.Select(selectedGxObjectList[i], false, null);
                    myGxApplication.Location = selectedGxObjectList[i].FullName;
                    GenerateThumbNail(myGxApplication);
                }
                MessageBox.Show(processLog);
            }

        }     
               
        private void GenerateThumbNail(IGxApplication application)
        {
            IGxApplication pGxApp = application;
            UID pUID = new UIDClass();
            
            try
            {                
                //MessageBox.Show(pGxApp.View.Name);  //Values: Contents, Preview, Description
                //Set application to the Preview Tab first
                if (pGxApp.View.GetType() != typeof(IGxPreview))
                {
                    pUID.Value = "{B1DE27AF-D892-11D1-AA81-064342000000}"; //GUID for GxPreview TAB
                    pGxApp.ViewClassID = pUID;
                    //pGxApp.View.Activate(pGxApp, pGxApp.Catalog);
                }

                //Once on the preview tab, set to preview mode (GeographicView vs. TableView) if supported
                //Refer to the CatalogUI OMD, the IGXPreview object will not be created if the selected item cannot be previewed
                //To work around the issue, a try/catch statment was added.  An exception will happen if it cannot be previewed
                IGxPreview pGxPreview = (IGxPreview)pGxApp.View;
                pUID = new UIDClass();
                pUID.Value = "{B1DE27B0-D892-11D1-AA81-064342000000}";//GUID for Preview Mode aka Geographic View
                pGxPreview.ViewClassID = pUID;

                //Once in Geographic Run the Generate Thumbnail Command
                FindCommandAndExecute((IApplication)ArcCatalog.ThisApplication, "esriArcCatalogUI.CreateThumbnailCommand");

            }
            catch (Exception e)
            {
                //Couldn't Generate Thumbnail
                //MessageBox.Show("Could not generate thumbnail for: " + pGxApp.SelectedObject.Name);
                processLog += "\r\n" + "Could not generate thumbnail for: " + pGxApp.SelectedObject.Name;

            }
            
            #region Test Area
            //Test Code
            //IGxViewContainer vcont = (IGxViewContainer)pGxPreview;
            //IEnumGxView gview = vcont.Views;
            //gview.Next();
            //IGxView j;
            //while((j=gview.Next()) != null)
            //{                
            //    MessageBox.Show("GxViewName: "+j.Name + "  Supports Tools: " + j.SupportsTools);
            //    //MessageBox.Show("View Applies: "+ j.Applies(application.SelectedObject));                
            //}
            #endregion

        }
        
        public void FindCommandAndExecute(ESRI.ArcGIS.Framework.IApplication application, System.String commandName)
        {
            ESRI.ArcGIS.Framework.ICommandBars commandBars = application.Document.CommandBars;
            ESRI.ArcGIS.esriSystem.UID uid = new ESRI.ArcGIS.esriSystem.UIDClass();
            uid.Value = commandName; // Example: "esriFramework.HelpContentsCommand" or "{D74B2F25-AC90-11D2-87F8-0000F8751720}"
            ESRI.ArcGIS.Framework.ICommandItem commandItem = commandBars.Find(uid, false, false);
            
            if (commandItem != null)
            {
                commandItem.Execute();
            }
            
            #region ESRI Documentation for running tools
            //http://resources.arcgis.com/en/help/arcobjects-net/conceptualhelp/index.html#/ArcCatalog_commands/000100000020000000/
            //Runs the create Thumbnail Tool:  esriArcCatalogUI.CreateThumbnailCommand   //{7410F770-B225-11D2-AB24-000000000000}
            //Activates Preview: esriArcCatalogUI.ThumbnailsCommand {7410F771-B225-11D2-AB24-000000000000}

            //http://edndoc.esri.com/arcobjects/8.3/TechnicalDocuments/ArcCatalogIds.htm
            //this at version 8.3..  esriCore.CreateThumbnailCommand
            #endregion

        }
       

        protected override void OnUpdate()
        {
        }
    }
}
