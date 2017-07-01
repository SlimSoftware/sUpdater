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
            this.updateContentPanel = new System.Windows.Forms.Panel();
            this.updateContextMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.ignoreUpdateToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.detailsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.updatePage = new System.Windows.Forms.Panel();
            this.installUpdatesButton = new System.Windows.Forms.Button();
            this.refreshButton = new System.Windows.Forms.Button();
            this.selectAllUpdatesCheckBox = new System.Windows.Forms.CheckBox();
            this.aboutPage = new System.Windows.Forms.Panel();
            this.siteLink = new System.Windows.Forms.LinkLabel();
            this.slimsoftwareLabel = new System.Windows.Forms.Label();
            this.versionLabel = new System.Windows.Forms.Label();
            this.aboutTitle = new System.Windows.Forms.Label();
            this.slimUpdaterLogo = new System.Windows.Forms.PictureBox();
            this.detailsPage = new System.Windows.Forms.Panel();
            this.detailText = new System.Windows.Forms.RichTextBox();
            this.actionLink = new System.Windows.Forms.LinkLabel();
            this.detailLabel = new System.Windows.Forms.Label();
            this.titleButton = new Slim_Updater.titleButton();
            this.settingsTile = new Slim_Updater.Custom_Controls.flatTile();
            this.getNewAppsTile = new Slim_Updater.Custom_Controls.flatTile();
            this.portableAppsTile = new Slim_Updater.Custom_Controls.flatTile();
            this.updaterTile = new Slim_Updater.Custom_Controls.flatTile();
            this.startPage.SuspendLayout();
            this.topBar.SuspendLayout();
            this.updateContextMenu.SuspendLayout();
            this.updatePage.SuspendLayout();
            this.aboutPage.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.slimUpdaterLogo)).BeginInit();
            this.detailsPage.SuspendLayout();
            this.SuspendLayout();
            // 
            // startPage
            // 
            this.startPage.Controls.Add(this.settingsTile);
            this.startPage.Controls.Add(this.getNewAppsTile);
            this.startPage.Controls.Add(this.portableAppsTile);
            this.startPage.Controls.Add(this.updaterTile);
            this.startPage.Location = new System.Drawing.Point(0, 34);
            this.startPage.Name = "startPage";
            this.startPage.Size = new System.Drawing.Size(785, 425);
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
            this.topBar.Size = new System.Drawing.Size(793, 35);
            this.topBar.TabIndex = 5;
            // 
            // aboutLabel
            // 
            this.aboutLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.aboutLabel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(186)))), ((int)(((byte)(0)))));
            this.aboutLabel.Location = new System.Drawing.Point(729, 9);
            this.aboutLabel.Name = "aboutLabel";
            this.aboutLabel.Size = new System.Drawing.Size(50, 17);
            this.aboutLabel.TabIndex = 1;
            this.aboutLabel.Text = "About";
            this.aboutLabel.Click += new System.EventHandler(this.aboutLabel_Click);
            this.aboutLabel.MouseEnter += new System.EventHandler(this.aboutLabel_MouseEnter);
            this.aboutLabel.MouseLeave += new System.EventHandler(this.aboutLabel_MouseLeave);
            // 
            // updateContentPanel
            // 
            this.updateContentPanel.AutoScroll = true;
            this.updateContentPanel.ContextMenuStrip = this.updateContextMenu;
            this.updateContentPanel.Location = new System.Drawing.Point(-3, 20);
            this.updateContentPanel.Name = "updateContentPanel";
            this.updateContentPanel.Size = new System.Drawing.Size(790, 365);
            this.updateContentPanel.TabIndex = 0;
            // 
            // updateContextMenu
            // 
            this.updateContextMenu.BackColor = System.Drawing.SystemColors.ButtonHighlight;
            this.updateContextMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ignoreUpdateToolStripMenuItem,
            this.detailsToolStripMenuItem});
            this.updateContextMenu.Name = "contextMenu";
            this.updateContextMenu.RenderMode = System.Windows.Forms.ToolStripRenderMode.System;
            this.updateContextMenu.Size = new System.Drawing.Size(110, 48);
            // 
            // ignoreUpdateToolStripMenuItem
            // 
            this.ignoreUpdateToolStripMenuItem.Name = "ignoreUpdateToolStripMenuItem";
            this.ignoreUpdateToolStripMenuItem.Size = new System.Drawing.Size(109, 22);
            this.ignoreUpdateToolStripMenuItem.Text = "&Ignore";
            // 
            // detailsToolStripMenuItem
            // 
            this.detailsToolStripMenuItem.Name = "detailsToolStripMenuItem";
            this.detailsToolStripMenuItem.Size = new System.Drawing.Size(109, 22);
            this.detailsToolStripMenuItem.Text = "&Details";
            // 
            // updatePage
            // 
            this.updatePage.BackColor = System.Drawing.SystemColors.ButtonHighlight;
            this.updatePage.Controls.Add(this.installUpdatesButton);
            this.updatePage.Controls.Add(this.refreshButton);
            this.updatePage.Controls.Add(this.selectAllUpdatesCheckBox);
            this.updatePage.Controls.Add(this.updateContentPanel);
            this.updatePage.Location = new System.Drawing.Point(0, 35);
            this.updatePage.Name = "updatePage";
            this.updatePage.Size = new System.Drawing.Size(785, 425);
            this.updatePage.TabIndex = 6;
            // 
            // installUpdatesButton
            // 
            this.installUpdatesButton.Location = new System.Drawing.Point(398, 394);
            this.installUpdatesButton.Name = "installUpdatesButton";
            this.installUpdatesButton.Size = new System.Drawing.Size(93, 23);
            this.installUpdatesButton.TabIndex = 2;
            this.installUpdatesButton.Text = "Install Selected";
            this.installUpdatesButton.UseVisualStyleBackColor = true;
            this.installUpdatesButton.Click += new System.EventHandler(this.installUpdatesButton_Click);
            // 
            // refreshButton
            // 
            this.refreshButton.Location = new System.Drawing.Point(294, 394);
            this.refreshButton.Name = "refreshButton";
            this.refreshButton.Size = new System.Drawing.Size(75, 23);
            this.refreshButton.TabIndex = 0;
            this.refreshButton.Text = "Refresh";
            this.refreshButton.UseVisualStyleBackColor = true;
            this.refreshButton.Click += new System.EventHandler(this.refreshButton_Click);
            // 
            // selectAllUpdatesCheckBox
            // 
            this.selectAllUpdatesCheckBox.AutoSize = true;
            this.selectAllUpdatesCheckBox.Checked = true;
            this.selectAllUpdatesCheckBox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.selectAllUpdatesCheckBox.Location = new System.Drawing.Point(4, 3);
            this.selectAllUpdatesCheckBox.Name = "selectAllUpdatesCheckBox";
            this.selectAllUpdatesCheckBox.Size = new System.Drawing.Size(82, 17);
            this.selectAllUpdatesCheckBox.TabIndex = 1;
            this.selectAllUpdatesCheckBox.Text = "Unselect All";
            this.selectAllUpdatesCheckBox.UseVisualStyleBackColor = true;
            this.selectAllUpdatesCheckBox.Click += new System.EventHandler(this.selectAllUpdatesCheckBox_Click);
            // 
            // aboutPage
            // 
            this.aboutPage.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(186)))), ((int)(((byte)(0)))));
            this.aboutPage.Controls.Add(this.siteLink);
            this.aboutPage.Controls.Add(this.slimsoftwareLabel);
            this.aboutPage.Controls.Add(this.versionLabel);
            this.aboutPage.Controls.Add(this.aboutTitle);
            this.aboutPage.Controls.Add(this.slimUpdaterLogo);
            this.aboutPage.Location = new System.Drawing.Point(0, 34);
            this.aboutPage.Name = "aboutPage";
            this.aboutPage.Size = new System.Drawing.Size(785, 425);
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
            // slimUpdaterLogo
            // 
            this.slimUpdaterLogo.Image = global::Slim_Updater.Properties.Resources.SlimUpdater_new;
            this.slimUpdaterLogo.Location = new System.Drawing.Point(311, 19);
            this.slimUpdaterLogo.Name = "slimUpdaterLogo";
            this.slimUpdaterLogo.Size = new System.Drawing.Size(128, 128);
            this.slimUpdaterLogo.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.slimUpdaterLogo.TabIndex = 0;
            this.slimUpdaterLogo.TabStop = false;
            // 
            // detailsPage
            // 
            this.detailsPage.BackColor = System.Drawing.SystemColors.ButtonHighlight;
            this.detailsPage.Controls.Add(this.detailText);
            this.detailsPage.Controls.Add(this.actionLink);
            this.detailsPage.Controls.Add(this.detailLabel);
            this.detailsPage.Location = new System.Drawing.Point(0, 34);
            this.detailsPage.Name = "detailsPage";
            this.detailsPage.Size = new System.Drawing.Size(785, 425);
            this.detailsPage.TabIndex = 10;
            // 
            // detailText
            // 
            this.detailText.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.detailText.BackColor = System.Drawing.Color.White;
            this.detailText.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.detailText.Location = new System.Drawing.Point(6, 27);
            this.detailText.Name = "detailText";
            this.detailText.ReadOnly = true;
            this.detailText.Size = new System.Drawing.Size(776, 377);
            this.detailText.TabIndex = 15;
            this.detailText.Text = "";
            // 
            // actionLink
            // 
            this.actionLink.AutoSize = true;
            this.actionLink.LinkColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(186)))), ((int)(((byte)(0)))));
            this.actionLink.Location = new System.Drawing.Point(3, 405);
            this.actionLink.Name = "actionLink";
            this.actionLink.Size = new System.Drawing.Size(37, 13);
            this.actionLink.TabIndex = 13;
            this.actionLink.TabStop = true;
            this.actionLink.Text = "Ingore";
            // 
            // detailLabel
            // 
            this.detailLabel.AutoSize = true;
            this.detailLabel.BackColor = System.Drawing.Color.Transparent;
            this.detailLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.detailLabel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(186)))), ((int)(((byte)(0)))));
            this.detailLabel.Location = new System.Drawing.Point(3, 4);
            this.detailLabel.Name = "detailLabel";
            this.detailLabel.Size = new System.Drawing.Size(85, 17);
            this.detailLabel.TabIndex = 12;
            this.detailLabel.Text = "Changelog";
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
            this.ClientSize = new System.Drawing.Size(784, 461);
            this.Controls.Add(this.updatePage);
            this.Controls.Add(this.topBar);
            this.Controls.Add(this.startPage);
            this.Controls.Add(this.detailsPage);
            this.Controls.Add(this.aboutPage);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "MainWindow";
            this.Text = "Slim Updater";
            this.startPage.ResumeLayout(false);
            this.topBar.ResumeLayout(false);
            this.topBar.PerformLayout();
            this.updateContextMenu.ResumeLayout(false);
            this.updatePage.ResumeLayout(false);
            this.updatePage.PerformLayout();
            this.aboutPage.ResumeLayout(false);
            this.aboutPage.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.slimUpdaterLogo)).EndInit();
            this.detailsPage.ResumeLayout(false);
            this.detailsPage.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Panel startPage;
        private System.Windows.Forms.Panel topBar;
        private System.Windows.Forms.Label aboutLabel;
        private System.Windows.Forms.Panel updatePage;
        private titleButton titleButton;
        private Custom_Controls.flatTile updaterTile;
        private Custom_Controls.flatTile settingsTile;
        private Custom_Controls.flatTile getNewAppsTile;
        private Custom_Controls.flatTile portableAppsTile;
        private System.Windows.Forms.Panel aboutPage;
        private System.Windows.Forms.PictureBox slimUpdaterLogo;
        private System.Windows.Forms.Label aboutTitle;
        private System.Windows.Forms.LinkLabel siteLink;
        private System.Windows.Forms.Label slimsoftwareLabel;
        private System.Windows.Forms.Label versionLabel;
        private System.Windows.Forms.Panel updateContentPanel;
        private System.Windows.Forms.CheckBox selectAllUpdatesCheckBox;
        private System.Windows.Forms.Button installUpdatesButton;
        private System.Windows.Forms.Button refreshButton;
        private System.Windows.Forms.ContextMenuStrip updateContextMenu;
        private System.Windows.Forms.ToolStripMenuItem ignoreUpdateToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem detailsToolStripMenuItem;
        private System.Windows.Forms.Panel detailsPage;
        private System.Windows.Forms.LinkLabel actionLink;
        private System.Windows.Forms.Label detailLabel;
        private System.Windows.Forms.RichTextBox detailText;
    }
}

