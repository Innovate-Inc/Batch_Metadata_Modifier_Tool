namespace MetadataFormLibrary
{
    partial class MetadataForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if(disposing && (components != null)) {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MetadataForm));
            this.tvMetadata = new System.Windows.Forms.TreeView();
            this.ilTvMetadata = new System.Windows.Forms.ImageList(this.components);
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.splitContainer2 = new System.Windows.Forms.SplitContainer();
            this.flpMain = new System.Windows.Forms.FlowLayoutPanel();
            this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
            this.label1 = new System.Windows.Forms.Label();
            this.cbValues = new System.Windows.Forms.ComboBox();
            this.lblElementValue = new System.Windows.Forms.Label();
            this.flowLayoutPanel2 = new System.Windows.Forms.FlowLayoutPanel();
            this.tbCurrentValue = new System.Windows.Forms.TextBox();
            this.btnUpdate = new System.Windows.Forms.Button();
            this.flowLayoutPanel3 = new System.Windows.Forms.FlowLayoutPanel();
            this.lboChangeLog = new System.Windows.Forms.ListBox();
            this.dgvValidationErrors = new System.Windows.Forms.DataGridView();
            this.colSeverity = new System.Windows.Forms.DataGridViewImageColumn();
            this.colMessage = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colMetadata = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.newToolStripButton = new System.Windows.Forms.ToolStripButton();
            this.openToolStripButton = new System.Windows.Forms.ToolStripButton();
            this.directorytoolStripButton = new System.Windows.Forms.ToolStripButton();
            this.saveToolStripButton = new System.Windows.Forms.ToolStripButton();
            this.printToolStripButton = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator = new System.Windows.Forms.ToolStripSeparator();
            this.cutToolStripButton = new System.Windows.Forms.ToolStripButton();
            this.copyToolStripButton = new System.Windows.Forms.ToolStripButton();
            this.pasteToolStripButton = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.helpToolStripButton = new System.Windows.Forms.ToolStripButton();
            this.cmsEditTree = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.tsmiAddElement = new System.Windows.Forms.ToolStripMenuItem();
            this.placeholderToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiDeleteElement = new System.Windows.Forms.ToolStripMenuItem();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.editToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.findToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.replaceToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.importMetaDataToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.helpToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.viewHelpToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.tslblStatus = new System.Windows.Forms.ToolStripStatusLabel();
            this.tspProgressBar = new System.Windows.Forms.ToolStripProgressBar();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.splitContainer2.Panel1.SuspendLayout();
            this.splitContainer2.Panel2.SuspendLayout();
            this.splitContainer2.SuspendLayout();
            this.flpMain.SuspendLayout();
            this.flowLayoutPanel1.SuspendLayout();
            this.flowLayoutPanel2.SuspendLayout();
            this.flowLayoutPanel3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvValidationErrors)).BeginInit();
            this.toolStrip1.SuspendLayout();
            this.cmsEditTree.SuspendLayout();
            this.menuStrip1.SuspendLayout();
            this.statusStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // tvMetadata
            // 
            this.tvMetadata.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.tvMetadata.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tvMetadata.HideSelection = false;
            this.tvMetadata.ImageIndex = 0;
            this.tvMetadata.ImageList = this.ilTvMetadata;
            this.tvMetadata.Location = new System.Drawing.Point(0, 0);
            this.tvMetadata.Name = "tvMetadata";
            this.tvMetadata.SelectedImageIndex = 0;
            this.tvMetadata.Size = new System.Drawing.Size(290, 376);
            this.tvMetadata.TabIndex = 0;
            this.tvMetadata.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.tvMetadata_AfterSelect);
            this.tvMetadata.NodeMouseClick += new System.Windows.Forms.TreeNodeMouseClickEventHandler(this.tvMetadata_NodeMouseClick);
            // 
            // ilTvMetadata
            // 
            this.ilTvMetadata.ColorDepth = System.Windows.Forms.ColorDepth.Depth8Bit;
            this.ilTvMetadata.ImageSize = new System.Drawing.Size(16, 16);
            this.ilTvMetadata.TransparentColor = System.Drawing.Color.Transparent;
            // 
            // splitContainer1
            // 
            this.splitContainer1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.splitContainer1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.splitContainer1.Location = new System.Drawing.Point(0, 49);
            this.splitContainer1.Name = "splitContainer1";
            this.splitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.splitContainer2);
            this.splitContainer1.Panel2Collapsed = true;
            this.splitContainer1.Panel2MinSize = 0;
            this.splitContainer1.Size = new System.Drawing.Size(818, 378);
            this.splitContainer1.SplitterDistance = 378;
            this.splitContainer1.SplitterWidth = 2;
            this.splitContainer1.TabIndex = 2;
            // 
            // splitContainer2
            // 
            this.splitContainer2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.splitContainer2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer2.Location = new System.Drawing.Point(0, 0);
            this.splitContainer2.Name = "splitContainer2";
            // 
            // splitContainer2.Panel1
            // 
            this.splitContainer2.Panel1.Controls.Add(this.tvMetadata);
            // 
            // splitContainer2.Panel2
            // 
            this.splitContainer2.Panel2.Controls.Add(this.flpMain);
            this.splitContainer2.Size = new System.Drawing.Size(818, 378);
            this.splitContainer2.SplitterDistance = 292;
            this.splitContainer2.SplitterWidth = 2;
            this.splitContainer2.TabIndex = 2;
            this.splitContainer2.SplitterMoved += new System.Windows.Forms.SplitterEventHandler(this.splitContainer2_SplitterMoved);
            // 
            // flpMain
            // 
            this.flpMain.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.flpMain.AutoSize = true;
            this.flpMain.Controls.Add(this.flowLayoutPanel1);
            this.flpMain.Controls.Add(this.lblElementValue);
            this.flpMain.Controls.Add(this.flowLayoutPanel2);
            this.flpMain.Controls.Add(this.flowLayoutPanel3);
            this.flpMain.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
            this.flpMain.Location = new System.Drawing.Point(0, 3);
            this.flpMain.Name = "flpMain";
            this.flpMain.Size = new System.Drawing.Size(518, 372);
            this.flpMain.TabIndex = 0;
            // 
            // flowLayoutPanel1
            // 
            this.flowLayoutPanel1.AutoSize = true;
            this.flowLayoutPanel1.Controls.Add(this.label1);
            this.flowLayoutPanel1.Controls.Add(this.cbValues);
            this.flowLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.flowLayoutPanel1.Location = new System.Drawing.Point(3, 3);
            this.flowLayoutPanel1.MinimumSize = new System.Drawing.Size(2, 56);
            this.flowLayoutPanel1.Name = "flowLayoutPanel1";
            this.flowLayoutPanel1.Size = new System.Drawing.Size(510, 56);
            this.flowLayoutPanel1.TabIndex = 2;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(3, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(281, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "Unique Element Value Across Open Files (Select To View)";
            // 
            // cbValues
            // 
            this.cbValues.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cbValues.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbValues.FormattingEnabled = true;
            this.cbValues.Location = new System.Drawing.Point(3, 16);
            this.cbValues.Name = "cbValues";
            this.cbValues.Size = new System.Drawing.Size(502, 21);
            this.cbValues.TabIndex = 0;
            this.toolTip1.SetToolTip(this.cbValues, "Select to view unique values across open files.");
            this.cbValues.SelectedIndexChanged += new System.EventHandler(this.cbValues_SelectedIndexChanged);
            // 
            // lblElementValue
            // 
            this.lblElementValue.AutoSize = true;
            this.lblElementValue.Location = new System.Drawing.Point(3, 62);
            this.lblElementValue.Name = "lblElementValue";
            this.lblElementValue.Size = new System.Drawing.Size(266, 13);
            this.lblElementValue.TabIndex = 4;
            this.lblElementValue.Text = "Selected Element Editor (Changes below affect all files)";
            // 
            // flowLayoutPanel2
            // 
            this.flowLayoutPanel2.AutoSize = true;
            this.flowLayoutPanel2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.flowLayoutPanel2.Controls.Add(this.tbCurrentValue);
            this.flowLayoutPanel2.Controls.Add(this.btnUpdate);
            this.flowLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.flowLayoutPanel2.Location = new System.Drawing.Point(3, 78);
            this.flowLayoutPanel2.MinimumSize = new System.Drawing.Size(2, 115);
            this.flowLayoutPanel2.Name = "flowLayoutPanel2";
            this.flowLayoutPanel2.Size = new System.Drawing.Size(510, 126);
            this.flowLayoutPanel2.TabIndex = 5;
            // 
            // tbCurrentValue
            // 
            this.tbCurrentValue.Location = new System.Drawing.Point(3, 3);
            this.tbCurrentValue.Multiline = true;
            this.tbCurrentValue.Name = "tbCurrentValue";
            this.tbCurrentValue.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.tbCurrentValue.Size = new System.Drawing.Size(502, 86);
            this.tbCurrentValue.TabIndex = 2;
            // 
            // btnUpdate
            // 
            this.btnUpdate.Location = new System.Drawing.Point(3, 95);
            this.btnUpdate.Name = "btnUpdate";
            this.btnUpdate.Size = new System.Drawing.Size(177, 26);
            this.btnUpdate.TabIndex = 1;
            this.btnUpdate.Tag = "";
            this.btnUpdate.Text = "Apply Value to All Open Files";
            this.toolTip1.SetToolTip(this.btnUpdate, "Caution!  When clicked, the value in the Editor window will be applied to all ope" +
        "n files!");
            this.btnUpdate.UseVisualStyleBackColor = true;
            this.btnUpdate.Click += new System.EventHandler(this.btnUpdate_Click);
            // 
            // flowLayoutPanel3
            // 
            this.flowLayoutPanel3.AutoSize = true;
            this.flowLayoutPanel3.Controls.Add(this.lboChangeLog);
            this.flowLayoutPanel3.Location = new System.Drawing.Point(3, 210);
            this.flowLayoutPanel3.Name = "flowLayoutPanel3";
            this.flowLayoutPanel3.Size = new System.Drawing.Size(509, 153);
            this.flowLayoutPanel3.TabIndex = 6;
            // 
            // lboChangeLog
            // 
            this.lboChangeLog.BackColor = System.Drawing.Color.Black;
            this.lboChangeLog.ForeColor = System.Drawing.SystemColors.Window;
            this.lboChangeLog.FormattingEnabled = true;
            this.lboChangeLog.HorizontalScrollbar = true;
            this.lboChangeLog.Location = new System.Drawing.Point(3, 3);
            this.lboChangeLog.Name = "lboChangeLog";
            this.lboChangeLog.Size = new System.Drawing.Size(503, 147);
            this.lboChangeLog.TabIndex = 3;
            // 
            // dgvValidationErrors
            // 
            this.dgvValidationErrors.AllowUserToAddRows = false;
            this.dgvValidationErrors.AllowUserToDeleteRows = false;
            this.dgvValidationErrors.AllowUserToOrderColumns = true;
            this.dgvValidationErrors.AllowUserToResizeRows = false;
            this.dgvValidationErrors.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.dgvValidationErrors.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
            this.dgvValidationErrors.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvValidationErrors.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colSeverity,
            this.colMessage,
            this.colMetadata});
            this.dgvValidationErrors.Location = new System.Drawing.Point(163, 411);
            this.dgvValidationErrors.Name = "dgvValidationErrors";
            this.dgvValidationErrors.ReadOnly = true;
            this.dgvValidationErrors.RowHeadersVisible = false;
            this.dgvValidationErrors.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvValidationErrors.Size = new System.Drawing.Size(227, 21);
            this.dgvValidationErrors.TabIndex = 0;
            this.dgvValidationErrors.Visible = false;
            // 
            // colSeverity
            // 
            this.colSeverity.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.colSeverity.HeaderText = "";
            this.colSeverity.MinimumWidth = 16;
            this.colSeverity.Name = "colSeverity";
            this.colSeverity.ReadOnly = true;
            this.colSeverity.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.colSeverity.Width = 16;
            // 
            // colMessage
            // 
            this.colMessage.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.colMessage.HeaderText = "Error Description";
            this.colMessage.Name = "colMessage";
            this.colMessage.ReadOnly = true;
            // 
            // colMetadata
            // 
            this.colMetadata.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.DisplayedCells;
            this.colMetadata.HeaderText = "Metadata";
            this.colMetadata.Name = "colMetadata";
            this.colMetadata.ReadOnly = true;
            this.colMetadata.Width = 77;
            // 
            // toolStrip1
            // 
            this.toolStrip1.CanOverflow = false;
            this.toolStrip1.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.newToolStripButton,
            this.openToolStripButton,
            this.directorytoolStripButton,
            this.saveToolStripButton,
            this.printToolStripButton,
            this.toolStripSeparator,
            this.cutToolStripButton,
            this.copyToolStripButton,
            this.pasteToolStripButton,
            this.toolStripSeparator1,
            this.helpToolStripButton});
            this.toolStrip1.LayoutStyle = System.Windows.Forms.ToolStripLayoutStyle.HorizontalStackWithOverflow;
            this.toolStrip1.Location = new System.Drawing.Point(0, 24);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(818, 25);
            this.toolStrip1.TabIndex = 3;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // newToolStripButton
            // 
            this.newToolStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.newToolStripButton.Image = ((System.Drawing.Image)(resources.GetObject("newToolStripButton.Image")));
            this.newToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.newToolStripButton.Name = "newToolStripButton";
            this.newToolStripButton.Size = new System.Drawing.Size(23, 22);
            this.newToolStripButton.Text = "&New";
            this.newToolStripButton.Visible = false;
            // 
            // openToolStripButton
            // 
            this.openToolStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.openToolStripButton.Image = ((System.Drawing.Image)(resources.GetObject("openToolStripButton.Image")));
            this.openToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.openToolStripButton.Name = "openToolStripButton";
            this.openToolStripButton.Size = new System.Drawing.Size(23, 22);
            this.openToolStripButton.Text = "&Open Individual File";
            this.openToolStripButton.Click += new System.EventHandler(this.openToolStripButton_Click);
            // 
            // directorytoolStripButton
            // 
            this.directorytoolStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.directorytoolStripButton.Image = global::MetadataFormLibrary.Properties.Resources.folderIn;
            this.directorytoolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.directorytoolStripButton.Name = "directorytoolStripButton";
            this.directorytoolStripButton.Size = new System.Drawing.Size(23, 22);
            this.directorytoolStripButton.Text = "Open Directory";
            this.directorytoolStripButton.Click += new System.EventHandler(this.direcotytoolStripButton_Click);
            // 
            // saveToolStripButton
            // 
            this.saveToolStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.saveToolStripButton.Image = ((System.Drawing.Image)(resources.GetObject("saveToolStripButton.Image")));
            this.saveToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.saveToolStripButton.Name = "saveToolStripButton";
            this.saveToolStripButton.Size = new System.Drawing.Size(23, 22);
            this.saveToolStripButton.Text = "&Save";
            this.saveToolStripButton.Click += new System.EventHandler(this.saveToolStrip_Click);
            // 
            // printToolStripButton
            // 
            this.printToolStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.printToolStripButton.Image = ((System.Drawing.Image)(resources.GetObject("printToolStripButton.Image")));
            this.printToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.printToolStripButton.Name = "printToolStripButton";
            this.printToolStripButton.Size = new System.Drawing.Size(23, 22);
            this.printToolStripButton.Text = "&Print";
            this.printToolStripButton.Visible = false;
            // 
            // toolStripSeparator
            // 
            this.toolStripSeparator.Name = "toolStripSeparator";
            this.toolStripSeparator.Size = new System.Drawing.Size(6, 25);
            // 
            // cutToolStripButton
            // 
            this.cutToolStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.cutToolStripButton.Image = ((System.Drawing.Image)(resources.GetObject("cutToolStripButton.Image")));
            this.cutToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.cutToolStripButton.Name = "cutToolStripButton";
            this.cutToolStripButton.Size = new System.Drawing.Size(23, 22);
            this.cutToolStripButton.Text = "C&ut";
            this.cutToolStripButton.Visible = false;
            // 
            // copyToolStripButton
            // 
            this.copyToolStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.copyToolStripButton.Image = ((System.Drawing.Image)(resources.GetObject("copyToolStripButton.Image")));
            this.copyToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.copyToolStripButton.Name = "copyToolStripButton";
            this.copyToolStripButton.Size = new System.Drawing.Size(23, 22);
            this.copyToolStripButton.Text = "&Copy";
            this.copyToolStripButton.Visible = false;
            // 
            // pasteToolStripButton
            // 
            this.pasteToolStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.pasteToolStripButton.Image = ((System.Drawing.Image)(resources.GetObject("pasteToolStripButton.Image")));
            this.pasteToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.pasteToolStripButton.Name = "pasteToolStripButton";
            this.pasteToolStripButton.Size = new System.Drawing.Size(23, 22);
            this.pasteToolStripButton.Text = "&Paste";
            this.pasteToolStripButton.Visible = false;
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 25);
            // 
            // helpToolStripButton
            // 
            this.helpToolStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.helpToolStripButton.Image = ((System.Drawing.Image)(resources.GetObject("helpToolStripButton.Image")));
            this.helpToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.helpToolStripButton.Name = "helpToolStripButton";
            this.helpToolStripButton.Size = new System.Drawing.Size(23, 22);
            this.helpToolStripButton.Text = "He&lp";
            this.helpToolStripButton.Visible = false;
            // 
            // cmsEditTree
            // 
            this.cmsEditTree.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsmiAddElement,
            this.tsmiDeleteElement});
            this.cmsEditTree.Name = "cmsEditTree";
            this.cmsEditTree.Size = new System.Drawing.Size(154, 48);
            // 
            // tsmiAddElement
            // 
            this.tsmiAddElement.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.placeholderToolStripMenuItem});
            this.tsmiAddElement.Name = "tsmiAddElement";
            this.tsmiAddElement.Size = new System.Drawing.Size(153, 22);
            this.tsmiAddElement.Text = "Add Element";
            this.tsmiAddElement.Visible = false;
            this.tsmiAddElement.DropDownOpening += new System.EventHandler(this.tsmiAddElement_DropDownOpening);
            // 
            // placeholderToolStripMenuItem
            // 
            this.placeholderToolStripMenuItem.Name = "placeholderToolStripMenuItem";
            this.placeholderToolStripMenuItem.Size = new System.Drawing.Size(144, 22);
            this.placeholderToolStripMenuItem.Text = "[Placeholder]";
            // 
            // tsmiDeleteElement
            // 
            this.tsmiDeleteElement.Name = "tsmiDeleteElement";
            this.tsmiDeleteElement.Size = new System.Drawing.Size(153, 22);
            this.tsmiDeleteElement.Text = "Delete Element";
            this.tsmiDeleteElement.Click += new System.EventHandler(this.tsmiDeleteElement_Click);
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.editToolStripMenuItem,
            this.helpToolStripMenuItem});
            this.menuStrip1.LayoutStyle = System.Windows.Forms.ToolStripLayoutStyle.HorizontalStackWithOverflow;
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(818, 24);
            this.menuStrip1.TabIndex = 4;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.saveToolStripMenuItem});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.F)));
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(37, 20);
            this.fileToolStripMenuItem.Text = "&File";
            // 
            // saveToolStripMenuItem
            // 
            this.saveToolStripMenuItem.Name = "saveToolStripMenuItem";
            this.saveToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.S)));
            this.saveToolStripMenuItem.Size = new System.Drawing.Size(138, 22);
            this.saveToolStripMenuItem.Text = "&Save";
            this.saveToolStripMenuItem.Click += new System.EventHandler(this.saveToolStrip_Click);
            // 
            // editToolStripMenuItem
            // 
            this.editToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.findToolStripMenuItem,
            this.replaceToolStripMenuItem,
            this.importMetaDataToolStripMenuItem});
            this.editToolStripMenuItem.Name = "editToolStripMenuItem";
            this.editToolStripMenuItem.Size = new System.Drawing.Size(39, 20);
            this.editToolStripMenuItem.Text = "Edit";
            // 
            // findToolStripMenuItem
            // 
            this.findToolStripMenuItem.Name = "findToolStripMenuItem";
            this.findToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.F)));
            this.findToolStripMenuItem.Size = new System.Drawing.Size(167, 22);
            this.findToolStripMenuItem.Text = "&Find...";
            this.findToolStripMenuItem.Click += new System.EventHandler(this.findToolStripMenuItem_Click);
            // 
            // replaceToolStripMenuItem
            // 
            this.replaceToolStripMenuItem.Name = "replaceToolStripMenuItem";
            this.replaceToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.H)));
            this.replaceToolStripMenuItem.Size = new System.Drawing.Size(167, 22);
            this.replaceToolStripMenuItem.Text = "Replace...";
            this.replaceToolStripMenuItem.Click += new System.EventHandler(this.replaceToolStripMenuItem_Click);
            // 
            // importMetaDataToolStripMenuItem
            // 
            this.importMetaDataToolStripMenuItem.Name = "importMetaDataToolStripMenuItem";
            this.importMetaDataToolStripMenuItem.Size = new System.Drawing.Size(167, 22);
            this.importMetaDataToolStripMenuItem.Text = "Import Metadata";
            this.importMetaDataToolStripMenuItem.Click += new System.EventHandler(this.importMetaDataToolStripMenuItem_Click);
            // 
            // helpToolStripMenuItem
            // 
            this.helpToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.viewHelpToolStripMenuItem});
            this.helpToolStripMenuItem.Name = "helpToolStripMenuItem";
            this.helpToolStripMenuItem.Size = new System.Drawing.Size(44, 20);
            this.helpToolStripMenuItem.Text = "Help";
            // 
            // viewHelpToolStripMenuItem
            // 
            this.viewHelpToolStripMenuItem.Name = "viewHelpToolStripMenuItem";
            this.viewHelpToolStripMenuItem.Size = new System.Drawing.Size(127, 22);
            this.viewHelpToolStripMenuItem.Text = "View Help";
            this.viewHelpToolStripMenuItem.Click += new System.EventHandler(this.viewHelpToolStripMenuItem_Click);
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tslblStatus,
            this.tspProgressBar});
            this.statusStrip1.Location = new System.Drawing.Point(0, 430);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(818, 22);
            this.statusStrip1.TabIndex = 5;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // tslblStatus
            // 
            this.tslblStatus.Name = "tslblStatus";
            this.tslblStatus.Size = new System.Drawing.Size(0, 17);
            // 
            // tspProgressBar
            // 
            this.tspProgressBar.Name = "tspProgressBar";
            this.tspProgressBar.Size = new System.Drawing.Size(100, 16);
            this.tspProgressBar.Visible = false;
            // 
            // toolTip1
            // 
            this.toolTip1.AutoPopDelay = 9900;
            this.toolTip1.InitialDelay = 500;
            this.toolTip1.ReshowDelay = 100;
            // 
            // MetadataForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(818, 452);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.toolStrip1);
            this.Controls.Add(this.menuStrip1);
            this.Controls.Add(this.splitContainer1);
            this.Controls.Add(this.dgvValidationErrors);
            this.MainMenuStrip = this.menuStrip1;
            this.MinimumSize = new System.Drawing.Size(834, 490);
            this.Name = "MetadataForm";
            this.Text = "Batch Metadata Modifier Tool";
            this.Resize += new System.EventHandler(this.MetadataForm_Resize);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.ResumeLayout(false);
            this.splitContainer2.Panel1.ResumeLayout(false);
            this.splitContainer2.Panel2.ResumeLayout(false);
            this.splitContainer2.Panel2.PerformLayout();
            this.splitContainer2.ResumeLayout(false);
            this.flpMain.ResumeLayout(false);
            this.flpMain.PerformLayout();
            this.flowLayoutPanel1.ResumeLayout(false);
            this.flowLayoutPanel1.PerformLayout();
            this.flowLayoutPanel2.ResumeLayout(false);
            this.flowLayoutPanel2.PerformLayout();
            this.flowLayoutPanel3.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvValidationErrors)).EndInit();
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.cmsEditTree.ResumeLayout(false);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TreeView tvMetadata;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.SplitContainer splitContainer2;
        private System.Windows.Forms.DataGridView dgvValidationErrors;
        private System.Windows.Forms.DataGridViewImageColumn colSeverity;
        private System.Windows.Forms.DataGridViewTextBoxColumn colMessage;
        private System.Windows.Forms.DataGridViewTextBoxColumn colMetadata;
        private System.Windows.Forms.ImageList ilTvMetadata;
        private System.Windows.Forms.FlowLayoutPanel flpMain;
        private System.Windows.Forms.ComboBox cbValues;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;
        private System.Windows.Forms.Button btnUpdate;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripButton newToolStripButton;
        private System.Windows.Forms.ToolStripButton openToolStripButton;
        private System.Windows.Forms.ToolStripButton saveToolStripButton;
        private System.Windows.Forms.ToolStripButton printToolStripButton;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator;
        private System.Windows.Forms.ToolStripButton cutToolStripButton;
        private System.Windows.Forms.ToolStripButton copyToolStripButton;
        private System.Windows.Forms.ToolStripButton pasteToolStripButton;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripButton helpToolStripButton;
        private System.Windows.Forms.ContextMenuStrip cmsEditTree;
        private System.Windows.Forms.ToolStripMenuItem tsmiDeleteElement;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem saveToolStripMenuItem;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripMenuItem editToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem findToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem replaceToolStripMenuItem;
        private System.Windows.Forms.TextBox tbCurrentValue;
        private System.Windows.Forms.ToolStripMenuItem tsmiAddElement;
        private System.Windows.Forms.ToolStripMenuItem placeholderToolStripMenuItem;
        private System.Windows.Forms.ToolStripStatusLabel tslblStatus;
        private System.Windows.Forms.ToolStripProgressBar tspProgressBar;
        private System.Windows.Forms.ListBox lboChangeLog;
        private System.Windows.Forms.Label lblElementValue;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel2;
        private System.Windows.Forms.ToolStripMenuItem importMetaDataToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem helpToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem viewHelpToolStripMenuItem;
        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel3;
        private System.Windows.Forms.ToolStripButton directorytoolStripButton;

    }
}