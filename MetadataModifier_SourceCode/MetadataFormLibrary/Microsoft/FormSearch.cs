using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Xml;
using System.Xml.XPath;
using System.Reflection;
using System.Diagnostics;
using MetadataFormLibrary;



namespace Microsoft.Xml
{
    public partial class FormSearch : Form
    {
        IFindTarget target;
        bool findOnly;
        FindFlags lastFlags = FindFlags.Normal;
        string lastExpression;
        const int MaxRecentlyUsed = 15;
        
        SearchFilter filter;

        public FormSearch()
        {
            this.SetStyle(ControlStyles.Selectable, true);
            this.KeyPreview = true;
            InitializeComponent();
            //DS OnClick events set here... do not link up elsewhere otherwise the event will run twice
            this.buttonFindNext.Click += new EventHandler(buttonFindNext_Click);
            this.buttonReplace.Click += new EventHandler(buttonReplace_Click);
            this.buttonReplaceAll.Click += new EventHandler(buttonReplaceAll_Click);
            this.comboBoxFind.KeyDown += new KeyEventHandler(comboBoxFind_KeyDown);
            this.comboBoxFind.LostFocus += new EventHandler(comboBoxFind_LostFocus);

            this.comboBoxFilter.Items.AddRange(new object[] { SearchFilter.Everything, SearchFilter.Names, SearchFilter.Text });
            this.comboBoxFilter.SelectedItem = this.filter;
            this.comboBoxFilter.SelectedValueChanged += new EventHandler(comboBoxFilter_SelectedValueChanged);
            this.toolTip1.SetToolTip(this.buttonReplace, "Replaces all occurrences of a search phrase within an individually selected data element.\r\nThe user can select compound elements and step through each child element.\r\nAll open files are processed on a given data element.");
            this.toolTip1.SetToolTip(this.buttonReplaceAll, "Replaces all occurrences of a search phrase within an individually selected data element.\r\nAll child data elements are automatically processed if a compound element is selected.\r\nAll open files are processed on a given data element.");

        }

        void comboBoxFind_LostFocus(object sender, EventArgs e)
        {
            return;
        }

        void comboBoxFilter_SelectedValueChanged(object sender, EventArgs e)
        {
            this.filter = (SearchFilter)this.comboBoxFilter.SelectedItem;
        }

        public FormSearch(FormSearch old)
            : this()
        {
            //DS Overloaded constructor to add previous searches into combo box
            if(old != null) {
                foreach(string s in old.comboBoxFind.Items) {
                    this.comboBoxFind.Items.Add(s);
                }
                this.comboBoxFilter.SelectedItem = old.comboBoxFilter.SelectedItem;
            }
            
            //this.Site = site;
        }

        public override ISite Site
        {
            get
            {
                return base.Site;
            }
            set
            {
                base.Site = value;
                OnSiteChanged();
            }
        }

        public SearchFilter Filter
        {
            get { return filter; }
            set { filter = value; }
        }

        void buttonFindNext_Click(object sender, EventArgs e)
        {
            DoFind();
        }

        void buttonReplace_Click(object sender, EventArgs e)
        {
            DoReplace();
        }

        Control Window
        {
            get { return this.IsDisposed ? null : this; }
        }

        void OnNotFound()
        {
            MessageBox.Show(this.Window, "The specified text was not found", "Find Error", MessageBoxButtons.OK, MessageBoxIcon.Information);
            this.comboBoxFind.Focus();
        }

        void OnFindDone()
        {
            MessageBox.Show(this.Window, "No more matching nodes", "Replace Complete", MessageBoxButtons.OK, MessageBoxIcon.Information);
            this.comboBoxFind.Focus();
        }

        void OnError(Exception e, string caption)
        {
            MessageBox.Show(this.Window, e.Message, caption, MessageBoxButtons.OK, MessageBoxIcon.Error);
            this.comboBoxFind.Focus();
        }

        void DoFind()
        {
            try {
                FindNext(false);  //DS False= Warning received when at end of search;  true then no warning box at end of search
            } catch(Exception ex) {
                OnError(ex, "Find Error");
            }
        }

        void DoReplace()
        {
            try {
                string replacement = this.comboBoxReplace.Text;
                Utils.changeLogDT.Rows.Add("...Searching to replace: " + comboBoxFind.Text + "  with: " + replacement);
                FindNext(false); //FindNext goes first before target.replace
                //target.ReplaceCurrent(replacement);
                if (checkBoxDataSetName.Checked == true)
                {
                    //DS  Created a new method to handle this for each file and node
                    //I didn't want to change the orignal method                        
                    //target.ReplaceCurrentwFileName(replacement);
                    target.ReplaceCurrentwFileName(comboBoxFind.Text);
                }
                else
                {
                    target.ReplaceCurrent(comboBoxFind.Text, replacement);                    
                }
                //FindNext(false);   //This needs to go before target.replace            
                
            } catch(Exception ex) {
                OnError(ex, "Replace Error");
            }
        }

        void buttonReplaceAll_Click(object sender, EventArgs e)
        {
            //UndoManager mgr = (UndoManager)this.Site.GetService(typeof(UndoManager));
            //mgr.OpenCompoundAction("Replace All");
            
            #region Original Code commented out by DS

            //try
            //{
            //    string replacement = this.comboBoxReplace.Text;
            //    target.ReplaceCurrent(replacement);
            //    bool rc = FindNext(false);
            //    while (rc)
            //    {
            //        Application.DoEvents();
            //        target.ReplaceCurrent(replacement);
            //        rc = FindNext(true);
            //    }
            //}
            //catch (Exception ex)
            //{
            //    OnError(ex, "Replace Error");
            //}
            //finally
            //{
            //    //mgr.CloseCompoundAction();
            //}

            #endregion
            
            //DS  Loop through all records until true received indicating the replace has finished searching
            //Appears to be a bug if you run replaceAll and not all nodes match
            //Appears to be a bug if node only contains one word.  The last letter of the word is not included in the replacement
            try {
                string replacement = this.comboBoxReplace.Text;
                Utils.changeLogDT.Rows.Add("...Searching to replace: " + comboBoxFind.Text + "  with: " + replacement);
                FindNext(false);
                bool rc = true;                 
                while(rc==true)
                {
                    Application.DoEvents();
                    //DS  Check if Dataset Name needs to be replaced                    
                    if (checkBoxDataSetName.Checked == true)
                    {
                        //DS  Created aa new method to handle this for each file and node
                        //I didn't want to change the orignal method                        
                        //target.ReplaceCurrentwFileName(replacement);
                        target.ReplaceCurrentwFileName(comboBoxFind.Text);
                        
                    }
                    else
                    {
                        target.ReplaceCurrent(comboBoxFind.Text, replacement);                        
                    }
                    //target.ReplaceCurrent(replacement);
                    rc = FindNext(true);
                }
                MessageBox.Show("ReplaceAll Complete");

            } catch(Exception ex) {
                OnError(ex, "Replace Error: " + ex.Message);
            } finally {
                //mgr.CloseCompoundAction();
            }
        }

        bool FindNext(bool quiet)
        {

            FindFlags flags = FindFlags.Normal;
            if(this.checkBoxRegex.Checked) flags |= FindFlags.Regex;
            else if(this.checkBoxXPath.Checked) flags |= FindFlags.XPath;
            if(this.checkBoxMatchCase.Checked) flags |= FindFlags.MatchCase;
            if(this.checkBoxWholeWord.Checked) flags |= FindFlags.WholeWord;
            if(this.radioButtonUp.Checked) flags |= FindFlags.Backwards;

            string expr = this.Expression;
            if(!this.comboBoxFind.Items.Contains(expr))
            {
                this.comboBoxFind.Items.Add(expr);
                if(this.comboBoxFind.Items.Count > MaxRecentlyUsed)
                {
                    this.comboBoxFind.Items.RemoveAt(0);
                }
            }

            lastFlags = flags;
            lastExpression = expr;

            FindResult rc = target.FindNext(expr, flags, filter);
            if(rc == FindResult.Found && !this.IsDisposed)
            {
                this.MoveFindDialog(target.MatchRect);
            }
            if(!quiet)
            //DS true = will receive messsage box when search done
            {
                if(rc == FindResult.None)
                {
                    OnNotFound();
                }
                else if(rc == FindResult.NoMore)
                {
                    OnFindDone();
                }
            }
            return rc == FindResult.Found;
        }

        public void FindAgain(bool reverse)
        {
            // The find dialog might have been disposed, so we can only find using previous 
            // find state information.
            if(string.IsNullOrEmpty(lastExpression)) {
                return;
            }
            try {
                if(reverse) {
                    lastFlags |= FindFlags.Backwards;
                } else {
                    lastFlags &= ~FindFlags.Backwards;
                }
                FindResult rc = target.FindNext(lastExpression, lastFlags, filter);
                if(rc == FindResult.Found && !this.IsDisposed) {
                    this.MoveFindDialog(target.MatchRect);
                }
                if(rc == FindResult.None) {
                    OnNotFound();
                } else if(rc == FindResult.NoMore) {
                    OnFindDone();
                }
            } catch(Exception ex) {
                OnError(ex, "Find Error");
            }
        }

        void MoveFindDialog(Rectangle selection)
        {
            /*Rectangle r = this.Bounds;
            if(r.IntersectsWith(selection)) {
                // find smallest adjustment (left,right,up,down) that still fits on screen.
                List<Adjustment> list = new List<Adjustment>();
                list.Add(new Adjustment(Direction.Up, this, selection));
                list.Add(new Adjustment(Direction.Down, this, selection));
                list.Add(new Adjustment(Direction.Left, this, selection));
                list.Add(new Adjustment(Direction.Right, this, selection));
                list.Sort();

                Adjustment smallest = list[0];
                smallest.AdjustDialog();
                return;
            }*/
        }

        enum Direction { Up, Down, Left, Right };
        class Adjustment : IComparable
        {
            Direction dir;
            Form dialog;
            Rectangle selection;
            Rectangle formBounds;

            public Adjustment(Direction dir, Form dialog, Rectangle selection)
            {
                this.dir = dir;
                this.dialog = dialog;
                this.selection = selection;
                this.formBounds = this.dialog.Bounds;
            }

            public int Delta
            {
                get
                {
                    int delta = 0;
                    Rectangle screen = Screen.FromControl(dialog).Bounds;
                    switch(this.dir) {
                        case Direction.Up:
                            delta = formBounds.Bottom - selection.Top;
                            if(formBounds.Top - delta < screen.Top) {
                                delta = Int32.MaxValue; // don't choose this one then.
                            }
                            break;
                        case Direction.Down:
                            delta = selection.Bottom - formBounds.Top;
                            if(formBounds.Bottom + delta > screen.Bottom) {
                                delta = Int32.MaxValue; // don't choose this one then.
                            }
                            break;
                        case Direction.Left:
                            delta = formBounds.Right - selection.Left;
                            if(formBounds.Right - delta < screen.Left) {
                                delta = Int32.MaxValue; // don't choose this one then.
                            }
                            break;
                        case Direction.Right:
                            delta = selection.Right - formBounds.Left;
                            if(formBounds.Right + delta > screen.Right) {
                                delta = Int32.MaxValue; // don't choose this one then.
                            }
                            break;
                    }
                    return delta;
                }
            }

            public void AdjustDialog()
            {
                if(this.Delta == Int32.MaxValue)
                    return; // none of the choices were ideal

                switch(this.dir) {
                    case Direction.Up:
                        this.dialog.Top -= this.Delta;
                        break;
                    case Direction.Down:
                        this.dialog.Top += this.Delta;
                        break;
                    case Direction.Left:
                        this.dialog.Left -= this.Delta;
                        break;
                    case Direction.Right:
                        this.dialog.Left += this.Delta;
                        break;
                }
            }

            public int CompareTo(object obj)
            {
                Adjustment a = obj as Adjustment;
                if(a != null) {
                    return this.Delta - a.Delta;
                }
                return 0;
            }
        }

        public virtual void OnSiteChanged()
        {
            HelpProvider hp = this.Site.GetService(typeof(HelpProvider)) as HelpProvider;
            if(hp != null) {
                hp.SetHelpKeyword(this, "Find");
                hp.SetHelpNavigator(this, HelpNavigator.KeywordIndex);
            }

            this.SuspendLayout();


            SetFindModeControls(!this.findOnly);


            //if ((Point)location != Point.Empty) {
            //    //Ctrl is  the main window
            //    Control ctrl = this.Site as Control;
            //    if(ctrl != null) {
            //        Rectangle ownerBounds = ctrl.TopLevelControl.Bounds;
            //        if (IsSameScreen((Point)location, ownerBounds)) {
            //            this.Location = (Point)location;
            //        } else {
            //            this.Location = CenterPosition(ownerBounds);
            //        }
            //        this.StartPosition = FormStartPosition.Manual;
            //    }
            //}

            this.ResumeLayout();
        }

        Point CenterPosition(Rectangle bounds)
        {
            Size s = this.ClientSize;
            Point center = new Point(bounds.Left + (bounds.Width / 2) - (s.Width / 2),
                bounds.Top + (bounds.Height / 2) - (s.Height / 2));

            if(center.X < 0) center.X = 0;
            if(center.Y < 0) center.Y = 0;

            return center;
        }

        bool IsSameScreen(Point location, Rectangle ownerBounds)
        {
            Point center = new Point(ownerBounds.Left + ownerBounds.Width / 2,
                ownerBounds.Top + ownerBounds.Height / 2);

            foreach(Screen s in Screen.AllScreens) {
                Rectangle sb = s.WorkingArea;
                if(sb.Contains(location)) {
                    return sb.Contains(center);
                }
            }

            // Who knows where that location is (perhaps secondary monitor was removed!)
            return false;
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            base.OnClosing(e);
        }

        public IFindTarget Target
        {
            get { return this.target; }
            set { this.target = value ; OnTargetChanged(); }
        }

        public bool ReplaceMode
        {
            get { return !findOnly; }
            set
            {
                findOnly = !value;
                SetFindModeControls(!findOnly);
            }
            //DS propety to set form in either search mode or search & replace mode
            //false is find only

        }

        void SetFindModeControls(bool visible)
        {
            this.comboBoxReplace.Visible = visible;
            this.label2.Visible = visible;
            this.buttonReplace.Visible = visible;
            this.buttonReplaceAll.Visible = visible;
            //DS Added checkbox for file name replace functionality
            this.checkBoxDataSetName.Visible = visible;
            this.checkBoxDataSetName.Checked = false;
            this.Text = visible ? "Replace" : "Find";

            //Only allow text filter for replace.
            if(visible) {
                this.comboBoxFilter.Items.Clear();
                this.comboBoxFilter.Items.Add(SearchFilter.Text);
                this.comboBoxFilter.SelectedIndex = 0;
                this.checkBoxMatchCase.Enabled = false;
                this.checkBoxMatchCase.Checked = true;
            } else {
                this.comboBoxFilter.Items.Clear();
                this.comboBoxFilter.Items.AddRange(new object[] { SearchFilter.Everything, SearchFilter.Names, SearchFilter.Text });
                this.comboBoxFilter.SelectedIndex = 0;
                this.checkBoxMatchCase.Enabled = true;
                this.checkBoxMatchCase.Checked = false;
            }
        }

        public string Expression
        {
            get { return this.comboBoxFind.Text; }
            set { this.comboBoxFind.Text = value; }
        }

        protected override void OnKeyDown(KeyEventArgs e)
        {
            HandleKeyDown(e);
            if(!e.Handled) {
                base.OnKeyDown(e);
            }
        }

        void comboBoxFind_KeyDown(object sender, KeyEventArgs e)
        {
            HandleKeyDown(e);
        }

        //protected override bool ProcessDialogKey(Keys keyData) {
        //    if (keyData == Keys.Tab || (keyData == (Keys.Tab | Keys.Shift))) {
        //        tnav.HandleTab(new KeyEventArgs(keyData));
        //        return true;
        //    } else {
        //        return base.ProcessDialogKey(keyData);
        //    }
        //}

        void HandleKeyDown(KeyEventArgs e)
        {
            if(e.KeyCode == Keys.Escape) {
                this.Close();
                e.Handled = true;
            } else if(e.KeyCode == Keys.Enter) {
                if(this.buttonReplace.Focused) {
                    DoReplace();
                } else {
                    DoFind();
                }
                e.Handled = true;
            } else if((e.Modifiers & Keys.Control) != 0) {
                if(e.KeyCode == Keys.H) {
                    ReplaceMode = true;
                    e.Handled = true;
                } else if(e.KeyCode == Keys.F) {
                    ReplaceMode = false;
                    e.Handled = true;
                }
            }
        }

        void OnTargetChanged()
        {
            this.dataTableNamespaces.Clear();
            if(target != null && ShowNamespaces)
            {
                this.Expression = target.Location;
                XmlNamespaceManager nsmgr = target.Namespaces;
                if(nsmgr != null)
                {
                    foreach(string prefix in nsmgr)
                    {
                        if(!string.IsNullOrEmpty(prefix) && prefix != "xmlns")
                        {
                            string uri = nsmgr.LookupNamespace(prefix);
                            this.dataTableNamespaces.Rows.Add(new object[] { prefix, uri });
                        }
                    }
                }
            } 
            else
            {
                this.Expression = null;
            }
        }

        private void checkBoxXPath_CheckedChanged(object sender, EventArgs e)
        {
            bool namespaces = this.ShowNamespaces;

            if(namespaces && string.IsNullOrEmpty(this.comboBoxFind.Text)) {
                OnTargetChanged();
            }
            dataGridViewNamespaces.Visible = false;// namespaces;
            if(checkBoxRegex.Checked) {
                ManualToggle(checkBoxRegex, false);
            }
        }

        bool ShowNamespaces
        {
            get { return checkBoxXPath.Checked; }
        }

        bool checkBoxLatch;
        void ManualToggle(CheckBox box, bool value)
        {
            if(!checkBoxLatch) {
                checkBoxLatch = true;
                box.Checked = value;
                checkBoxLatch = false;
            }
        }

        private void checkBoxRegex_CheckedChanged(object sender, EventArgs e)
        {
            if(checkBoxRegex.Checked) {
                ManualToggle(checkBoxXPath, false);
            }
        }

        private void checkBoxDataSetName_CheckedChanged(object sender, EventArgs e)
        {
            //DS  Added this for Checkbox dataset name functionality
            if (checkBoxDataSetName.Checked == true)
            {
                comboBoxReplace.Text = "< Replacement string = XML metadata file name >";
                comboBoxFind.Text = "UNIQUEFILENAME";
                checkBoxMatchCase.Checked = true;
            }
            else
            {
                comboBoxReplace.Items.Clear();
                comboBoxReplace.Text = "";
                comboBoxFind.Text = "";
            }
        }


       

    }

}