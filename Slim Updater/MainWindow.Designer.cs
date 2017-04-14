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
            this.settingsButton = new System.Windows.Forms.Panel();
            this.settingsLabel = new System.Windows.Forms.Label();
            this.settingsIcon = new System.Windows.Forms.PictureBox();
            this.portableAppsButton = new System.Windows.Forms.Panel();
            this.portableAppsIcon = new System.Windows.Forms.PictureBox();
            this.portableAppsLabel = new System.Windows.Forms.Label();
            this.getNewAppsButton = new System.Windows.Forms.Panel();
            this.getNewAppsIcon = new System.Windows.Forms.PictureBox();
            this.getNewAppsLabel = new System.Windows.Forms.Label();
            this.updaterButton = new System.Windows.Forms.Panel();
            this.updaterLabel = new System.Windows.Forms.Label();
            this.updaterIcon = new System.Windows.Forms.PictureBox();
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
            this.startPage.SuspendLayout();
            this.settingsButton.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.settingsIcon)).BeginInit();
            this.portableAppsButton.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.portableAppsIcon)).BeginInit();
            this.getNewAppsButton.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.getNewAppsIcon)).BeginInit();
            this.updaterButton.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.updaterIcon)).BeginInit();
            this.topBar.SuspendLayout();
            this.updatePage.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.updatesListView)).BeginInit();
            this.updatesContextMenu.SuspendLayout();
            this.SuspendLayout();
            // 
            // startPage
            // 
            this.startPage.Controls.Add(this.settingsButton);
            this.startPage.Controls.Add(this.portableAppsButton);
            this.startPage.Controls.Add(this.getNewAppsButton);
            this.startPage.Controls.Add(this.updaterButton);
            this.startPage.Dock = System.Windows.Forms.DockStyle.Fill;
            this.startPage.Location = new System.Drawing.Point(0, 0);
            this.startPage.Name = "startPage";
            this.startPage.Size = new System.Drawing.Size(751, 449);
            this.startPage.TabIndex = 1;
            // 
            // settingsButton
            // 
            this.settingsButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(186)))), ((int)(((byte)(0)))));
            this.settingsButton.Controls.Add(this.settingsLabel);
            this.settingsButton.Controls.Add(this.settingsIcon);
            this.settingsButton.Location = new System.Drawing.Point(416, 263);
            this.settingsButton.Name = "settingsButton";
            this.settingsButton.Size = new System.Drawing.Size(286, 148);
            this.settingsButton.TabIndex = 4;
            // 
            // settingsLabel
            // 
            this.settingsLabel.AutoSize = true;
            this.settingsLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.settingsLabel.ForeColor = System.Drawing.SystemColors.ButtonHighlight;
            this.settingsLabel.Location = new System.Drawing.Point(110, 121);
            this.settingsLabel.Name = "settingsLabel";
            this.settingsLabel.Size = new System.Drawing.Size(67, 17);
            this.settingsLabel.TabIndex = 2;
            this.settingsLabel.Text = "Settings";
            // 
            // settingsIcon
            // 
            this.settingsIcon.BackColor = System.Drawing.Color.Transparent;
            this.settingsIcon.Image = global::Slim_Updater.Properties.Resources.Settings_Icon;
            this.settingsIcon.Location = new System.Drawing.Point(113, 26);
            this.settingsIcon.Name = "settingsIcon";
            this.settingsIcon.Size = new System.Drawing.Size(64, 64);
            this.settingsIcon.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.settingsIcon.TabIndex = 1;
            this.settingsIcon.TabStop = false;
            // 
            // portableAppsButton
            // 
            this.portableAppsButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(186)))), ((int)(((byte)(0)))));
            this.portableAppsButton.Controls.Add(this.portableAppsIcon);
            this.portableAppsButton.Controls.Add(this.portableAppsLabel);
            this.portableAppsButton.Location = new System.Drawing.Point(48, 263);
            this.portableAppsButton.Name = "portableAppsButton";
            this.portableAppsButton.Size = new System.Drawing.Size(286, 148);
            this.portableAppsButton.TabIndex = 3;
            // 
            // portableAppsIcon
            // 
            this.portableAppsIcon.BackColor = System.Drawing.Color.Transparent;
            this.portableAppsIcon.Image = global::Slim_Updater.Properties.Resources.PortableApps_Icon;
            this.portableAppsIcon.Location = new System.Drawing.Point(111, 26);
            this.portableAppsIcon.Name = "portableAppsIcon";
            this.portableAppsIcon.Size = new System.Drawing.Size(64, 64);
            this.portableAppsIcon.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.portableAppsIcon.TabIndex = 4;
            this.portableAppsIcon.TabStop = false;
            // 
            // portableAppsLabel
            // 
            this.portableAppsLabel.AutoSize = true;
            this.portableAppsLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.portableAppsLabel.ForeColor = System.Drawing.SystemColors.ButtonHighlight;
            this.portableAppsLabel.Location = new System.Drawing.Point(88, 121);
            this.portableAppsLabel.Name = "portableAppsLabel";
            this.portableAppsLabel.Size = new System.Drawing.Size(110, 17);
            this.portableAppsLabel.TabIndex = 5;
            this.portableAppsLabel.Text = "Portable Apps";
            // 
            // getNewAppsButton
            // 
            this.getNewAppsButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(186)))), ((int)(((byte)(0)))));
            this.getNewAppsButton.Controls.Add(this.getNewAppsIcon);
            this.getNewAppsButton.Controls.Add(this.getNewAppsLabel);
            this.getNewAppsButton.Location = new System.Drawing.Point(416, 79);
            this.getNewAppsButton.Name = "getNewAppsButton";
            this.getNewAppsButton.Size = new System.Drawing.Size(286, 148);
            this.getNewAppsButton.TabIndex = 3;
            // 
            // getNewAppsIcon
            // 
            this.getNewAppsIcon.Image = global::Slim_Updater.Properties.Resources.PortableApps_Icon;
            this.getNewAppsIcon.Location = new System.Drawing.Point(113, 27);
            this.getNewAppsIcon.Name = "getNewAppsIcon";
            this.getNewAppsIcon.Size = new System.Drawing.Size(64, 64);
            this.getNewAppsIcon.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.getNewAppsIcon.TabIndex = 4;
            this.getNewAppsIcon.TabStop = false;
            // 
            // getNewAppsLabel
            // 
            this.getNewAppsLabel.AutoSize = true;
            this.getNewAppsLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.getNewAppsLabel.ForeColor = System.Drawing.SystemColors.ButtonHighlight;
            this.getNewAppsLabel.Location = new System.Drawing.Point(62, 122);
            this.getNewAppsLabel.Name = "getNewAppsLabel";
            this.getNewAppsLabel.Size = new System.Drawing.Size(162, 17);
            this.getNewAppsLabel.TabIndex = 4;
            this.getNewAppsLabel.Text = "Get New Applications";
            // 
            // updaterButton
            // 
            this.updaterButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(186)))), ((int)(((byte)(0)))));
            this.updaterButton.Controls.Add(this.updaterLabel);
            this.updaterButton.Controls.Add(this.updaterIcon);
            this.updaterButton.Location = new System.Drawing.Point(48, 79);
            this.updaterButton.Name = "updaterButton";
            this.updaterButton.Size = new System.Drawing.Size(286, 148);
            this.updaterButton.TabIndex = 2;
            this.updaterButton.Click += new System.EventHandler(this.updaterButton_Click);
            this.updaterButton.MouseEnter += new System.EventHandler(this.updaterButton_MouseEnter);
            this.updaterButton.MouseLeave += new System.EventHandler(this.updaterButton_MouseLeave);
            // 
            // updaterLabel
            // 
            this.updaterLabel.AutoSize = true;
            this.updaterLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.updaterLabel.ForeColor = System.Drawing.SystemColors.ButtonHighlight;
            this.updaterLabel.Location = new System.Drawing.Point(63, 122);
            this.updaterLabel.Name = "updaterLabel";
            this.updaterLabel.Size = new System.Drawing.Size(161, 17);
            this.updaterLabel.TabIndex = 3;
            this.updaterLabel.Text = "No updates available";
            // 
            // updaterIcon
            // 
            this.updaterIcon.Image = global::Slim_Updater.Properties.Resources.Updates_Icon;
            this.updaterIcon.Location = new System.Drawing.Point(113, 27);
            this.updaterIcon.Name = "updaterIcon";
            this.updaterIcon.Size = new System.Drawing.Size(64, 64);
            this.updaterIcon.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.updaterIcon.TabIndex = 3;
            this.updaterIcon.TabStop = false;
            this.updaterIcon.Click += new System.EventHandler(this.updaterIcon_Click);
            this.updaterIcon.MouseEnter += new System.EventHandler(this.updaterIcon_MouseEnter);
            this.updaterIcon.MouseLeave += new System.EventHandler(this.updaterIcon_MouseLeave);
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
            this.settingsButton.ResumeLayout(false);
            this.settingsButton.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.settingsIcon)).EndInit();
            this.portableAppsButton.ResumeLayout(false);
            this.portableAppsButton.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.portableAppsIcon)).EndInit();
            this.getNewAppsButton.ResumeLayout(false);
            this.getNewAppsButton.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.getNewAppsIcon)).EndInit();
            this.updaterButton.ResumeLayout(false);
            this.updaterButton.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.updaterIcon)).EndInit();
            this.topBar.ResumeLayout(false);
            this.topBar.PerformLayout();
            this.updatePage.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.updatesListView)).EndInit();
            this.updatesContextMenu.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Panel startPage;
        private System.Windows.Forms.PictureBox settingsIcon;
        private System.Windows.Forms.Panel topBar;
        private System.Windows.Forms.Label aboutLabel;
        private System.Windows.Forms.Panel settingsButton;
        private System.Windows.Forms.Panel portableAppsButton;
        private System.Windows.Forms.Panel getNewAppsButton;
        private System.Windows.Forms.Panel updaterButton;
        private System.Windows.Forms.Label settingsLabel;
        private System.Windows.Forms.Label portableAppsLabel;
        private System.Windows.Forms.Label getNewAppsLabel;
        private System.Windows.Forms.Label updaterLabel;
        private System.Windows.Forms.PictureBox updaterIcon;
        private System.Windows.Forms.PictureBox getNewAppsIcon;
        private System.Windows.Forms.PictureBox portableAppsIcon;
        private System.Windows.Forms.Panel updatePage;
        private BrightIdeasSoftware.ObjectListView updatesListView;
        private BrightIdeasSoftware.OLVColumn appNameColumn;
        private BrightIdeasSoftware.OLVColumn latestVersionColumn;
        private BrightIdeasSoftware.OLVColumn LocalVersion;
        private System.Windows.Forms.ContextMenuStrip updatesContextMenu;
        private System.Windows.Forms.ToolStripMenuItem changelogContextMenuItem;
        private System.Windows.Forms.ToolStripMenuItem ignoreContextMenuItem;
        private titleButton titleButton;
    }
}

