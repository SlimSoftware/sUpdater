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
            this.aboutPage = new System.Windows.Forms.Panel();
            this.siteLink = new System.Windows.Forms.LinkLabel();
            this.slimsoftwareLabel = new System.Windows.Forms.Label();
            this.versionLabel = new System.Windows.Forms.Label();
            this.aboutTitle = new System.Windows.Forms.Label();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.changelogLabel = new System.Windows.Forms.Label();
            this.changelogBox = new System.Windows.Forms.RichTextBox();
            this.titleButton = new Slim_Updater.titleButton();
            this.settingsTile = new Slim_Updater.Custom_Controls.flatTile();
            this.getNewAppsTile = new Slim_Updater.Custom_Controls.flatTile();
            this.portableAppsTile = new Slim_Updater.Custom_Controls.flatTile();
            this.updaterTile = new Slim_Updater.Custom_Controls.flatTile();
            this.startPage.SuspendLayout();
            this.topBar.SuspendLayout();
            this.updatePage.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.updatesListView)).BeginInit();
            this.updatesContextMenu.SuspendLayout();
            this.aboutPage.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // startPage
            // 
            this.startPage.Controls.Add(this.settingsTile);
            this.startPage.Controls.Add(this.getNewAppsTile);
            this.startPage.Controls.Add(this.portableAppsTile);
            this.startPage.Controls.Add(this.updaterTile);
            this.startPage.Location = new System.Drawing.Point(0, 36);
            this.startPage.Name = "startPage";
            this.startPage.Size = new System.Drawing.Size(751, 413);
            this.startPage.TabIndex = 1;
            // 
            // topBar
            // 
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
            this.aboutLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.aboutLabel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(186)))), ((int)(((byte)(0)))));
            this.aboutLabel.Location = new System.Drawing.Point(704, 9);
            this.aboutLabel.Name = "aboutLabel";
            this.aboutLabel.Size = new System.Drawing.Size(50, 17);
            this.aboutLabel.TabIndex = 1;
            this.aboutLabel.Text = "About";
            this.aboutLabel.Click += new System.EventHandler(this.aboutLabel_Click);
            this.aboutLabel.MouseEnter += new System.EventHandler(this.aboutLabel_MouseEnter);
            this.aboutLabel.MouseLeave += new System.EventHandler(this.aboutLabel_MouseLeave);
            // 
            // updatePage
            // 
            this.updatePage.BackColor = System.Drawing.SystemColors.ButtonHighlight;
            this.updatePage.Controls.Add(this.changelogBox);
            this.updatePage.Controls.Add(this.changelogLabel);
            this.updatePage.Controls.Add(this.updatesListView);
            this.updatePage.Location = new System.Drawing.Point(0, 36);
            this.updatePage.Name = "updatePage";
            this.updatePage.Size = new System.Drawing.Size(751, 413);
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
            this.updatesListView.Location = new System.Drawing.Point(0, 0);
            this.updatesListView.Name = "updatesListView";
            this.updatesListView.SelectedBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(186)))), ((int)(((byte)(0)))));
            this.updatesListView.SelectedForeColor = System.Drawing.Color.White;
            this.updatesListView.ShowGroups = false;
            this.updatesListView.Size = new System.Drawing.Size(751, 219);
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
            // aboutPage
            // 
            this.aboutPage.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(186)))), ((int)(((byte)(0)))));
            this.aboutPage.Controls.Add(this.siteLink);
            this.aboutPage.Controls.Add(this.slimsoftwareLabel);
            this.aboutPage.Controls.Add(this.versionLabel);
            this.aboutPage.Controls.Add(this.aboutTitle);
            this.aboutPage.Controls.Add(this.pictureBox1);
            this.aboutPage.Location = new System.Drawing.Point(0, 37);
            this.aboutPage.Name = "aboutPage";
            this.aboutPage.Size = new System.Drawing.Size(751, 415);
            this.aboutPage.TabIndex = 9;
            // 
            // siteLink
            // 
            this.siteLink.ActiveLinkColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(106)))), ((int)(((byte)(0)))));
            this.siteLink.AutoSize = true;
            this.siteLink.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.siteLink.LinkColor = System.Drawing.SystemColors.ButtonHighlight;
            this.siteLink.Location = new System.Drawing.Point(325, 387);
            this.siteLink.Name = "siteLink";
            this.siteLink.Size = new System.Drawing.Size(101, 17);
            this.siteLink.TabIndex = 4;
            this.siteLink.TabStop = true;
            this.siteLink.Text = "www.slimsoft.tk";
            this.siteLink.VisitedLinkColor = System.Drawing.SystemColors.ButtonHighlight;
            this.siteLink.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.siteLink_LinkClicked);
            // 
            // slimsoftwareLabel
            // 
            this.slimsoftwareLabel.AutoSize = true;
            this.slimsoftwareLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.slimsoftwareLabel.ForeColor = System.Drawing.SystemColors.ButtonHighlight;
            this.slimsoftwareLabel.Location = new System.Drawing.Point(177, 365);
            this.slimsoftwareLabel.Name = "slimsoftwareLabel";
            this.slimsoftwareLabel.Size = new System.Drawing.Size(396, 18);
            this.slimsoftwareLabel.TabIndex = 3;
            this.slimsoftwareLabel.Text = "Open Source Software developped by SlimSoftware\r\n";
            // 
            // versionLabel
            // 
            this.versionLabel.AutoSize = true;
            this.versionLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.versionLabel.ForeColor = System.Drawing.SystemColors.ButtonHighlight;
            this.versionLabel.Location = new System.Drawing.Point(325, 205);
            this.versionLabel.Name = "versionLabel";
            this.versionLabel.Size = new System.Drawing.Size(100, 20);
            this.versionLabel.TabIndex = 2;
            this.versionLabel.Text = "Version 3.0";
            // 
            // aboutTitle
            // 
            this.aboutTitle.AutoSize = true;
            this.aboutTitle.Font = new System.Drawing.Font("Microsoft Sans Serif", 16F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.aboutTitle.ForeColor = System.Drawing.SystemColors.ButtonHighlight;
            this.aboutTitle.Location = new System.Drawing.Point(300, 172);
            this.aboutTitle.Name = "aboutTitle";
            this.aboutTitle.Size = new System.Drawing.Size(151, 26);
            this.aboutTitle.TabIndex = 1;
            this.aboutTitle.Text = "Slim Updater";
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = global::Slim_Updater.Properties.Resources.SlimUpdater_new;
            this.pictureBox1.Location = new System.Drawing.Point(311, 19);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(128, 128);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox1.TabIndex = 0;
            this.pictureBox1.TabStop = false;
            // 
            // changelogLabel
            // 
            this.changelogLabel.AutoSize = true;
            this.changelogLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.changelogLabel.Location = new System.Drawing.Point(0, 222);
            this.changelogLabel.Name = "changelogLabel";
            this.changelogLabel.Size = new System.Drawing.Size(80, 17);
            this.changelogLabel.TabIndex = 1;
            this.changelogLabel.Text = "Changelog:";
            // 
            // changelogBox
            // 
            this.changelogBox.BackColor = System.Drawing.SystemColors.ButtonHighlight;
            this.changelogBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.changelogBox.Location = new System.Drawing.Point(0, 243);
            this.changelogBox.Name = "changelogBox";
            this.changelogBox.ReadOnly = true;
            this.changelogBox.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.Vertical;
            this.changelogBox.Size = new System.Drawing.Size(751, 129);
            this.changelogBox.TabIndex = 2;
            this.changelogBox.Text = "";
            // 
            // titleButton
            // 
            this.titleButton.Arrow = false;
            this.titleButton.AutoSize = true;
            this.titleButton.BackColor = System.Drawing.Color.Transparent;
            this.titleButton.Location = new System.Drawing.Point(10, 1);
            this.titleButton.Name = "titleButton";
            this.titleButton.Size = new System.Drawing.Size(69, 32);
            this.titleButton.TabIndex = 2;
            this.titleButton.Click += new System.EventHandler(this.titleButton_Click);
            // 
            // settingsTile
            // 
            this.settingsTile.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(186)))), ((int)(((byte)(0)))));
            this.settingsTile.Image = global::Slim_Updater.Properties.Resources.Settings_Icon;
            this.settingsTile.Location = new System.Drawing.Point(402, 222);
            this.settingsTile.Name = "settingsTile";
            this.settingsTile.Size = new System.Drawing.Size(300, 150);
            this.settingsTile.TabIndex = 8;
            this.settingsTile.Text = "Settings";
            // 
            // getNewAppsTile
            // 
            this.getNewAppsTile.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(186)))), ((int)(((byte)(0)))));
            this.getNewAppsTile.Image = global::Slim_Updater.Properties.Resources.GetNewApps_Icon;
            this.getNewAppsTile.Location = new System.Drawing.Point(402, 40);
            this.getNewAppsTile.Name = "getNewAppsTile";
            this.getNewAppsTile.Size = new System.Drawing.Size(300, 150);
            this.getNewAppsTile.TabIndex = 7;
            this.getNewAppsTile.Text = "Get New Applications";
            // 
            // portableAppsTile
            // 
            this.portableAppsTile.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(186)))), ((int)(((byte)(0)))));
            this.portableAppsTile.Image = global::Slim_Updater.Properties.Resources.PortableApps_Icon;
            this.portableAppsTile.Location = new System.Drawing.Point(48, 222);
            this.portableAppsTile.Name = "portableAppsTile";
            this.portableAppsTile.Size = new System.Drawing.Size(300, 150);
            this.portableAppsTile.TabIndex = 6;
            this.portableAppsTile.Text = "Portable Apps";
            // 
            // updaterTile
            // 
            this.updaterTile.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(186)))), ((int)(((byte)(0)))));
            this.updaterTile.Image = global::Slim_Updater.Properties.Resources.Updates_Icon;
            this.updaterTile.Location = new System.Drawing.Point(48, 40);
            this.updaterTile.Name = "updaterTile";
            this.updaterTile.Size = new System.Drawing.Size(300, 150);
            this.updaterTile.TabIndex = 5;
            this.updaterTile.Text = "No updates available";
            this.updaterTile.Click += new System.EventHandler(this.updaterTile_Click);
            // 
            // MainWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.BackColor = System.Drawing.SystemColors.ButtonHighlight;
            this.ClientSize = new System.Drawing.Size(751, 449);
            this.Controls.Add(this.topBar);
            this.Controls.Add(this.updatePage);
            this.Controls.Add(this.startPage);
            this.Controls.Add(this.aboutPage);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "MainWindow";
            this.Text = "Slim Updater";
            this.startPage.ResumeLayout(false);
            this.topBar.ResumeLayout(false);
            this.topBar.PerformLayout();
            this.updatePage.ResumeLayout(false);
            this.updatePage.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.updatesListView)).EndInit();
            this.updatesContextMenu.ResumeLayout(false);
            this.aboutPage.ResumeLayout(false);
            this.aboutPage.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
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
        private Custom_Controls.flatTile updaterTile;
        private Custom_Controls.flatTile settingsTile;
        private Custom_Controls.flatTile getNewAppsTile;
        private Custom_Controls.flatTile portableAppsTile;
        private System.Windows.Forms.Panel aboutPage;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Label aboutTitle;
        private System.Windows.Forms.LinkLabel siteLink;
        private System.Windows.Forms.Label slimsoftwareLabel;
        private System.Windows.Forms.Label versionLabel;
        private System.Windows.Forms.RichTextBox changelogBox;
        private System.Windows.Forms.Label changelogLabel;
    }
}

