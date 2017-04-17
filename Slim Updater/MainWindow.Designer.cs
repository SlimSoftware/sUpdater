namespace Slim_Updater
{
    partial class MainWindow
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainWindow));
            this.startPage = new System.Windows.Forms.Panel();
            this.topBar = new System.Windows.Forms.Panel();
            this.aboutLabel = new System.Windows.Forms.Label();
            this.updatePage = new System.Windows.Forms.Panel();
            this.updatesListView = new BrightIdeasSoftware.ObjectListView();
            this.appNameColumn = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.latestVersionColumn = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.LocalVersion = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.updatesContextMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.changelogContextMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.ignoreContextMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.titleButton = new Slim_Updater.titleButton();
            this.updateTile = new Slim_Updater.Custom_Controls.flatTile();
            this.portableAppsTile = new Slim_Updater.Custom_Controls.flatTile();
            this.getNewAppsTile = new Slim_Updater.Custom_Controls.flatTile();
            this.settingsTile = new Slim_Updater.Custom_Controls.flatTile();
            this.startPage.SuspendLayout();
            this.topBar.SuspendLayout();
            this.updatePage.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.updatesListView)).BeginInit();
            this.updatesContextMenu.SuspendLayout();
            this.SuspendLayout();
            // 
            // startPage
            // 
            this.startPage.Controls.Add(this.settingsTile);
            this.startPage.Controls.Add(this.getNewAppsTile);
            this.startPage.Controls.Add(this.portableAppsTile);
            this.startPage.Controls.Add(this.updateTile);
            this.startPage.Dock = System.Windows.Forms.DockStyle.Fill;
            this.startPage.Location = new System.Drawing.Point(0, 0);
            this.startPage.Name = "startPage";
            this.startPage.Size = new System.Drawing.Size(751, 449);
            this.startPage.TabIndex = 1;
            // 
            // topBar
            // 
            this.topBar.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.topBar.BackColor = System.Drawing.SystemColors.ButtonHighlight;
            this.topBar.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.topBar.Controls.Add(this.titleButton);
            this.topBar.Controls.Add(this.aboutLabel);
            this.topBar.Location = new System.Drawing.Point(-8, -1);
            this.topBar.Name = "topBar";
            this.topBar.Size = new System.Drawing.Size(767, 38);
            this.topBar.TabIndex = 5;
            // 
            // aboutLabel
            // 
            this.aboutLabel.AutoSize = true;
            this.aboutLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.aboutLabel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(186)))), ((int)(((byte)(0)))));
            this.aboutLabel.Location = new System.Drawing.Point(707, 10);
            this.aboutLabel.Name = "aboutLabel";
            this.aboutLabel.Size = new System.Drawing.Size(50, 17);
            this.aboutLabel.TabIndex = 1;
            this.aboutLabel.Text = "About";
            // 
            // updatePage
            // 
            this.updatePage.Controls.Add(this.updatesListView);
            this.updatePage.Dock = System.Windows.Forms.DockStyle.Fill;
            this.updatePage.Location = new System.Drawing.Point(0, 0);
            this.updatePage.Name = "updatePage";
            this.updatePage.Size = new System.Drawing.Size(751, 449);
            this.updatePage.TabIndex = 6;
            // 
            // updatesListView
            // 
            this.updatesListView.AllColumns.Add(this.appNameColumn);
            this.updatesListView.AllColumns.Add(this.latestVersionColumn);
            this.updatesListView.AllColumns.Add(this.LocalVersion);
            this.updatesListView.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.updatesListView.CellEditUseWholeCell = false;
            this.updatesListView.CheckBoxes = true;
            this.updatesListView.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.appNameColumn,
            this.latestVersionColumn,
            this.LocalVersion});
            this.updatesListView.Cursor = System.Windows.Forms.Cursors.Default;
            this.updatesListView.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
            this.updatesListView.Location = new System.Drawing.Point(0, 36);
            this.updatesListView.Name = "updatesListView";
            this.updatesListView.SelectedBackColor = System.Drawing.Color.White;
            this.updatesListView.SelectedForeColor = System.Drawing.Color.Black;
            this.updatesListView.ShowGroups = false;
            this.updatesListView.Size = new System.Drawing.Size(751, 413);
            this.updatesListView.Sorting = System.Windows.Forms.SortOrder.Descending;
            this.updatesListView.TabIndex = 0;
            this.updatesListView.UseCompatibleStateImageBehavior = false;
            this.updatesListView.UseHyperlinks = true;
            this.updatesListView.View = System.Windows.Forms.View.Details;
            // 
            // appNameColumn
            // 
            this.appNameColumn.AspectName = "Name";
            this.appNameColumn.HeaderCheckBox = true;
            this.appNameColumn.Text = "Application";
            this.appNameColumn.Width = 537;
            // 
            // latestVersionColumn
            // 
            this.latestVersionColumn.AspectName = "LatestVersion";
            this.latestVersionColumn.Text = "Latest Version";
            this.latestVersionColumn.Width = 109;
            // 
            // LocalVersion
            // 
            this.LocalVersion.AspectName = "LocalVersion";
            this.LocalVersion.Text = "Installed Version";
            this.LocalVersion.Width = 100;
            // 
            // updatesContextMenu
            // 
            this.updatesContextMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.changelogContextMenuItem,
            this.ignoreContextMenuItem});
            this.updatesContextMenu.Name = "updatesContextMenu";
            this.updatesContextMenu.Size = new System.Drawing.Size(150, 48);
            // 
            // changelogContextMenuItem
            // 
            this.changelogContextMenuItem.Name = "changelogContextMenuItem";
            this.changelogContextMenuItem.Size = new System.Drawing.Size(149, 22);
            this.changelogContextMenuItem.Text = "Changelog";
            // 
            // ignoreContextMenuItem
            // 
            this.ignoreContextMenuItem.Name = "ignoreContextMenuItem";
            this.ignoreContextMenuItem.Size = new System.Drawing.Size(149, 22);
            this.ignoreContextMenuItem.Text = "Ignore Update";
            // 
            // titleButton
            // 
            this.titleButton.Arrow = false;
            this.titleButton.AutoSize = true;
            this.titleButton.BackColor = System.Drawing.Color.Transparent;
            this.titleButton.Location = new System.Drawing.Point(10, 1);
            this.titleButton.Name = "titleButton";
            this.titleButton.Size = new System.Drawing.Size(121, 32);
            this.titleButton.TabIndex = 2;
            this.titleButton.Click += new System.EventHandler(this.titleButton_Click);
            // 
            // updateTile
            // 
            this.updateTile.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(186)))), ((int)(((byte)(0)))));
            this.updateTile.Image = global::Slim_Updater.Properties.Resources.Updates_Icon;
            this.updateTile.Location = new System.Drawing.Point(48, 79);
            this.updateTile.Name = "updateTile";
            this.updateTile.Size = new System.Drawing.Size(300, 150);
            this.updateTile.TabIndex = 5;
            // 
            // portableAppsTile
            // 
            this.portableAppsTile.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(186)))), ((int)(((byte)(0)))));
            this.portableAppsTile.Image = global::Slim_Updater.Properties.Resources.PortableApps_Icon;
            this.portableAppsTile.Location = new System.Drawing.Point(48, 261);
            this.portableAppsTile.Name = "portableAppsTile";
            this.portableAppsTile.Size = new System.Drawing.Size(300, 150);
            this.portableAppsTile.TabIndex = 6;
            // 
            // getNewAppsTile
            // 
            this.getNewAppsTile.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(186)))), ((int)(((byte)(0)))));
            this.getNewAppsTile.Image = global::Slim_Updater.Properties.Resources.GetNewApps_Icon;
            this.getNewAppsTile.Location = new System.Drawing.Point(402, 79);
            this.getNewAppsTile.Name = "getNewAppsTile";
            this.getNewAppsTile.Size = new System.Drawing.Size(300, 150);
            this.getNewAppsTile.TabIndex = 7;
            // 
            // settingsTile
            // 
            this.settingsTile.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(186)))), ((int)(((byte)(0)))));
            this.settingsTile.Image = global::Slim_Updater.Properties.Resources.Settings_Icon;
            this.settingsTile.Location = new System.Drawing.Point(402, 261);
            this.settingsTile.Name = "settingsTile";
            this.settingsTile.Size = new System.Drawing.Size(300, 150);
            this.settingsTile.TabIndex = 8;
            // 
            // MainWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.BackColor = System.Drawing.SystemColors.ButtonHighlight;
            this.ClientSize = new System.Drawing.Size(751, 449);
            this.Controls.Add(this.topBar);
            this.Controls.Add(this.startPage);
            this.Controls.Add(this.updatePage);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "MainWindow";
            this.Text = "Slim Updater";
            this.startPage.ResumeLayout(false);
            this.topBar.ResumeLayout(false);
            this.topBar.PerformLayout();
            this.updatePage.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.updatesListView)).EndInit();
            this.updatesContextMenu.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Panel startPage;
        private System.Windows.Forms.Panel topBar;
        private System.Windows.Forms.Label aboutLabel;
        private System.Windows.Forms.Panel updatePage;
        private BrightIdeasSoftware.ObjectListView updatesListView;
        private BrightIdeasSoftware.OLVColumn appNameColumn;
        private BrightIdeasSoftware.OLVColumn latestVersionColumn;
        private BrightIdeasSoftware.OLVColumn LocalVersion;
        private System.Windows.Forms.ContextMenuStrip updatesContextMenu;
        private System.Windows.Forms.ToolStripMenuItem changelogContextMenuItem;
        private System.Windows.Forms.ToolStripMenuItem ignoreContextMenuItem;
        private titleButton titleButton;
        private Custom_Controls.flatTile updateTile;
        private Custom_Controls.flatTile settingsTile;
        private Custom_Controls.flatTile getNewAppsTile;
        private Custom_Controls.flatTile portableAppsTile;
    }
}

