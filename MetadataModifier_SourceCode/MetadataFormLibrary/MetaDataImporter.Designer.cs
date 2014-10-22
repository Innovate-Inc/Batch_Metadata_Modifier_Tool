namespace MetadataFormLibrary
{
    partial class MetaDataImporter
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
            if (disposing && (components != null))
            {
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
            this.tvMetadataImportElements = new System.Windows.Forms.TreeView();
            this.ilTvMetaImporter = new System.Windows.Forms.ImageList(this.components);
            this.cmdReplaceContent = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.toolStripButton1 = new System.Windows.Forms.ToolStripButton();
            this.cmdAppendContent = new System.Windows.Forms.Button();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.toolStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // tvMetadataImportElements
            // 
            this.tvMetadataImportElements.CheckBoxes = true;
            this.tvMetadataImportElements.ImageIndex = 0;
            this.tvMetadataImportElements.ImageList = this.ilTvMetaImporter;
            this.tvMetadataImportElements.Location = new System.Drawing.Point(22, 46);
            this.tvMetadataImportElements.Name = "tvMetadataImportElements";
            this.tvMetadataImportElements.SelectedImageIndex = 0;
            this.tvMetadataImportElements.Size = new System.Drawing.Size(319, 362);
            this.tvMetadataImportElements.TabIndex = 0;
            this.tvMetadataImportElements.AfterCheck += new System.Windows.Forms.TreeViewEventHandler(this.tvMetadataImportElements_AfterCheck);
            // 
            // ilTvMetaImporter
            // 
            this.ilTvMetaImporter.ColorDepth = System.Windows.Forms.ColorDepth.Depth8Bit;
            this.ilTvMetaImporter.ImageSize = new System.Drawing.Size(16, 16);
            this.ilTvMetaImporter.TransparentColor = System.Drawing.Color.Transparent;
            // 
            // cmdReplaceContent
            // 
            this.cmdReplaceContent.Enabled = false;
            this.cmdReplaceContent.Location = new System.Drawing.Point(22, 414);
            this.cmdReplaceContent.Name = "cmdReplaceContent";
            this.cmdReplaceContent.Size = new System.Drawing.Size(75, 23);
            this.cmdReplaceContent.TabIndex = 2;
            this.cmdReplaceContent.Text = "Replace";
            this.cmdReplaceContent.UseVisualStyleBackColor = true;
            this.cmdReplaceContent.Click += new System.EventHandler(this.cmdReplaceContent_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(19, 30);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(122, 13);
            this.label1.TabIndex = 3;
            this.label1.Text = "Select Nodes To Import:";
            // 
            // toolStrip1
            // 
            this.toolStrip1.BackColor = System.Drawing.SystemColors.Control;
            this.toolStrip1.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripButton1});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(364, 25);
            this.toolStrip1.TabIndex = 4;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // toolStripButton1
            // 
            this.toolStripButton1.Image = global::MetadataFormLibrary.Properties.Resources.FolderIcon16;
            this.toolStripButton1.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton1.Name = "toolStripButton1";
            this.toolStripButton1.Size = new System.Drawing.Size(180, 22);
            this.toolStripButton1.Text = "Load Template Metadata File";
            this.toolStripButton1.Click += new System.EventHandler(this.toolStripButton1_Click);
            // 
            // cmdAppendContent
            // 
            this.cmdAppendContent.Enabled = false;
            this.cmdAppendContent.Location = new System.Drawing.Point(103, 414);
            this.cmdAppendContent.Name = "cmdAppendContent";
            this.cmdAppendContent.Size = new System.Drawing.Size(75, 23);
            this.cmdAppendContent.TabIndex = 5;
            this.cmdAppendContent.Text = "Append";
            this.cmdAppendContent.UseVisualStyleBackColor = true;
            this.cmdAppendContent.Click += new System.EventHandler(this.cmdAppendContent_Click);
            // 
            // toolTip1
            // 
            this.toolTip1.AutoPopDelay = 9000;
            this.toolTip1.InitialDelay = 500;
            this.toolTip1.ReshowDelay = 100;
            // 
            // MetaDataImporter
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(364, 455);
            this.Controls.Add(this.cmdAppendContent);
            this.Controls.Add(this.toolStrip1);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.cmdReplaceContent);
            this.Controls.Add(this.tvMetadataImportElements);
            this.Name = "MetaDataImporter";
            this.Text = "Metadata Importer";
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TreeView tvMetadataImportElements;
        private System.Windows.Forms.Button cmdReplaceContent;
        private System.Windows.Forms.ImageList ilTvMetaImporter;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripButton toolStripButton1;
        private System.Windows.Forms.Button cmdAppendContent;
        private System.Windows.Forms.ToolTip toolTip1;
    }
}