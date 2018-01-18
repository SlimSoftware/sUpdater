﻿namespace Slim_Updater
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainWindow));
            this.startPage = new System.Windows.Forms.Panel();
            this.updaterTile = new Slim_Updater.flatTile();
            this.getNewAppsTile = new Slim_Updater.flatTile();
            this.portableAppsTile = new Slim_Updater.flatTile();
            this.settingsTile = new Slim_Updater.flatTile();
            this.topBar = new System.Windows.Forms.Panel();
            this.titleButtonRight = new Slim_Updater.TitleButton();
            this.titleButtonLeft = new Slim_Updater.TitleButton();
            this.aboutLabel = new System.Windows.Forms.Label();
            this.selectAllUpdatesCheckBox = new System.Windows.Forms.CheckBox();
            this.updateContentPanel = new System.Windows.Forms.Panel();
            this.updatePage = new System.Windows.Forms.Panel();
            this.failedUpdateLabel = new System.Windows.Forms.Label();
            this.installUpdatesButton = new System.Windows.Forms.Button();
            this.refreshUpdatesButton = new System.Windows.Forms.Button();
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
            this.getNewAppsPage = new System.Windows.Forms.Panel();
            this.selectAllAppsCheckBox = new System.Windows.Forms.CheckBox();
            this.failedAppInstallLabel = new System.Windows.Forms.Label();
            this.installAppsButton = new System.Windows.Forms.Button();
            this.refreshAppsButton = new System.Windows.Forms.Button();
            this.appContentPanel = new System.Windows.Forms.Panel();
            this.installedPortableAppsPage = new System.Windows.Forms.Panel();
            this.selectAllPortableCheckBox1 = new System.Windows.Forms.CheckBox();
            this.failedPortableInstallLabel1 = new System.Windows.Forms.Label();
            this.refreshPortableButton = new System.Windows.Forms.Button();
            this.installedPortableContentPanel = new System.Windows.Forms.Panel();
            this.setPortableAppFolderPage = new System.Windows.Forms.Panel();
            this.paFolderNotWriteableLabel2 = new System.Windows.Forms.Label();
            this.browseButton2 = new System.Windows.Forms.Button();
            this.locationBox2 = new System.Windows.Forms.TextBox();
            this.instructionLabel = new System.Windows.Forms.Label();
            this.setPortableAppFolderButton = new System.Windows.Forms.Button();
            this.settingsPage = new System.Windows.Forms.Panel();
            this.trayIconCheckBox = new System.Windows.Forms.CheckBox();
            this.autoStartCheckBox = new System.Windows.Forms.CheckBox();
            this.paFolderLocationLabel = new System.Windows.Forms.Label();
            this.locationBox1 = new System.Windows.Forms.TextBox();
            this.browseButton1 = new System.Windows.Forms.Button();
            this.paFolderNotWriteableLabel1 = new System.Windows.Forms.Label();
            this.defenitionsGroupBox = new System.Windows.Forms.GroupBox();
            this.warningLabel = new System.Windows.Forms.Label();
            this.customURLTextBox = new System.Windows.Forms.TextBox();
            this.customDefenRadioBtn = new System.Windows.Forms.RadioButton();
            this.officialDefenRadioBtn = new System.Windows.Forms.RadioButton();
            this.resetButton = new System.Windows.Forms.Button();
            this.saveButton = new System.Windows.Forms.Button();
            this.getPortableAppsPage = new System.Windows.Forms.Panel();
            this.selectAllPortableCheckBox2 = new System.Windows.Forms.CheckBox();
            this.failedPortableInstallLabel2 = new System.Windows.Forms.Label();
            this.downloadPortableButton = new System.Windows.Forms.Button();
            this.refreshPortableButton2 = new System.Windows.Forms.Button();
            this.getPortableContentPanel = new System.Windows.Forms.Panel();
            this.startPage.SuspendLayout();
            this.topBar.SuspendLayout();
            this.updatePage.SuspendLayout();
            this.aboutPage.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.slimUpdaterLogo)).BeginInit();
            this.detailsPage.SuspendLayout();
            this.getNewAppsPage.SuspendLayout();
            this.installedPortableAppsPage.SuspendLayout();
            this.setPortableAppFolderPage.SuspendLayout();
            this.settingsPage.SuspendLayout();
            this.defenitionsGroupBox.SuspendLayout();
            this.getPortableAppsPage.SuspendLayout();
            this.SuspendLayout();
            // 
            // startPage
            // 
            this.startPage.Controls.Add(this.updaterTile);
            this.startPage.Controls.Add(this.getNewAppsTile);
            this.startPage.Controls.Add(this.portableAppsTile);
            this.startPage.Controls.Add(this.settingsTile);
            this.startPage.Location = new System.Drawing.Point(0, 34);
            this.startPage.Name = "startPage";
            this.startPage.Size = new System.Drawing.Size(785, 425);
            this.startPage.TabIndex = 1;
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
            this.updaterTile.Click += new System.EventHandler(this.UpdaterTile_Click);
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
            this.getNewAppsTile.Click += new System.EventHandler(this.GetNewAppsTile_Click);
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
            this.portableAppsTile.Click += new System.EventHandler(this.PortableAppsTile_Click);
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
            this.settingsTile.Click += new System.EventHandler(this.SettingsTile_Click);
            // 
            // topBar
            // 
            this.topBar.BackColor = System.Drawing.SystemColors.ButtonHighlight;
            this.topBar.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.topBar.Controls.Add(this.titleButtonRight);
            this.topBar.Controls.Add(this.titleButtonLeft);
            this.topBar.Controls.Add(this.aboutLabel);
            this.topBar.Location = new System.Drawing.Point(-7, -1);
            this.topBar.Name = "topBar";
            this.topBar.Size = new System.Drawing.Size(798, 35);
            this.topBar.TabIndex = 5;
            // 
            // titleButtonRight
            // 
            this.titleButtonRight.ArrowLeft = false;
            this.titleButtonRight.ArrowRight = true;
            this.titleButtonRight.AutoSize = true;
            this.titleButtonRight.BackColor = System.Drawing.Color.White;
            this.titleButtonRight.Location = new System.Drawing.Point(577, 2);
            this.titleButtonRight.Name = "titleButtonRight";
            this.titleButtonRight.Size = new System.Drawing.Size(214, 31);
            this.titleButtonRight.TabIndex = 3;
            this.titleButtonRight.Visible = false;
            this.titleButtonRight.Click += new System.EventHandler(this.TitleButtonRight_Click);
            // 
            // titleButtonLeft
            // 
            this.titleButtonLeft.ArrowLeft = false;
            this.titleButtonLeft.ArrowRight = false;
            this.titleButtonLeft.AutoSize = true;
            this.titleButtonLeft.BackColor = System.Drawing.Color.Transparent;
            this.titleButtonLeft.Location = new System.Drawing.Point(5, 2);
            this.titleButtonLeft.Name = "titleButtonLeft";
            this.titleButtonLeft.Size = new System.Drawing.Size(99, 32);
            this.titleButtonLeft.TabIndex = 2;
            this.titleButtonLeft.Click += new System.EventHandler(this.TitleButtonLeft_Click);
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
            this.aboutLabel.Click += new System.EventHandler(this.AboutLabel_Click);
            this.aboutLabel.MouseEnter += new System.EventHandler(this.AboutLabel_MouseEnter);
            this.aboutLabel.MouseLeave += new System.EventHandler(this.AboutLabel_MouseLeave);
            // 
            // selectAllUpdatesCheckBox
            // 
            this.selectAllUpdatesCheckBox.AutoSize = true;
            this.selectAllUpdatesCheckBox.Checked = true;
            this.selectAllUpdatesCheckBox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.selectAllUpdatesCheckBox.Location = new System.Drawing.Point(6, 2);
            this.selectAllUpdatesCheckBox.Name = "selectAllUpdatesCheckBox";
            this.selectAllUpdatesCheckBox.Size = new System.Drawing.Size(82, 17);
            this.selectAllUpdatesCheckBox.TabIndex = 1;
            this.selectAllUpdatesCheckBox.Text = "Unselect All";
            this.selectAllUpdatesCheckBox.UseVisualStyleBackColor = true;
            this.selectAllUpdatesCheckBox.Click += new System.EventHandler(this.SelectAllUpdatesCheckBox_Click);
            // 
            // updateContentPanel
            // 
            this.updateContentPanel.AutoScroll = true;
            this.updateContentPanel.Location = new System.Drawing.Point(0, 20);
            this.updateContentPanel.Name = "updateContentPanel";
            this.updateContentPanel.Size = new System.Drawing.Size(785, 365);
            this.updateContentPanel.TabIndex = 0;
            // 
            // updatePage
            // 
            this.updatePage.BackColor = System.Drawing.SystemColors.ButtonHighlight;
            this.updatePage.Controls.Add(this.selectAllUpdatesCheckBox);
            this.updatePage.Controls.Add(this.failedUpdateLabel);
            this.updatePage.Controls.Add(this.installUpdatesButton);
            this.updatePage.Controls.Add(this.refreshUpdatesButton);
            this.updatePage.Controls.Add(this.updateContentPanel);
            this.updatePage.Location = new System.Drawing.Point(0, 35);
            this.updatePage.Name = "updatePage";
            this.updatePage.Size = new System.Drawing.Size(785, 425);
            this.updatePage.TabIndex = 6;
            // 
            // failedUpdateLabel
            // 
            this.failedUpdateLabel.AutoSize = true;
            this.failedUpdateLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.failedUpdateLabel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(186)))), ((int)(((byte)(0)))));
            this.failedUpdateLabel.Location = new System.Drawing.Point(164, 2);
            this.failedUpdateLabel.Name = "failedUpdateLabel";
            this.failedUpdateLabel.Size = new System.Drawing.Size(457, 15);
            this.failedUpdateLabel.TabIndex = 3;
            this.failedUpdateLabel.Text = "Some updates failed to install. Would you like to retry installing these?";
            this.failedUpdateLabel.Visible = false;
            // 
            // installUpdatesButton
            // 
            this.installUpdatesButton.Location = new System.Drawing.Point(398, 394);
            this.installUpdatesButton.Name = "installUpdatesButton";
            this.installUpdatesButton.Size = new System.Drawing.Size(93, 23);
            this.installUpdatesButton.TabIndex = 2;
            this.installUpdatesButton.Text = "Install Selected";
            this.installUpdatesButton.UseVisualStyleBackColor = true;
            this.installUpdatesButton.Click += new System.EventHandler(this.InstallUpdatesButton_Click);
            // 
            // refreshUpdatesButton
            // 
            this.refreshUpdatesButton.Location = new System.Drawing.Point(294, 394);
            this.refreshUpdatesButton.Name = "refreshUpdatesButton";
            this.refreshUpdatesButton.Size = new System.Drawing.Size(75, 23);
            this.refreshUpdatesButton.TabIndex = 0;
            this.refreshUpdatesButton.Text = "Refresh";
            this.refreshUpdatesButton.UseVisualStyleBackColor = true;
            this.refreshUpdatesButton.Click += new System.EventHandler(this.RefreshUpdatesButton_Click);
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
            this.siteLink.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.SiteLink_LinkClicked);
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
            // getNewAppsPage
            // 
            this.getNewAppsPage.BackColor = System.Drawing.SystemColors.ButtonHighlight;
            this.getNewAppsPage.Controls.Add(this.selectAllAppsCheckBox);
            this.getNewAppsPage.Controls.Add(this.failedAppInstallLabel);
            this.getNewAppsPage.Controls.Add(this.installAppsButton);
            this.getNewAppsPage.Controls.Add(this.refreshAppsButton);
            this.getNewAppsPage.Controls.Add(this.appContentPanel);
            this.getNewAppsPage.Location = new System.Drawing.Point(0, 35);
            this.getNewAppsPage.Name = "getNewAppsPage";
            this.getNewAppsPage.Size = new System.Drawing.Size(785, 425);
            this.getNewAppsPage.TabIndex = 11;
            // 
            // selectAllAppsCheckBox
            // 
            this.selectAllAppsCheckBox.AutoSize = true;
            this.selectAllAppsCheckBox.Location = new System.Drawing.Point(6, 2);
            this.selectAllAppsCheckBox.Name = "selectAllAppsCheckBox";
            this.selectAllAppsCheckBox.Size = new System.Drawing.Size(70, 17);
            this.selectAllAppsCheckBox.TabIndex = 1;
            this.selectAllAppsCheckBox.Text = "Select All";
            this.selectAllAppsCheckBox.UseVisualStyleBackColor = true;
            this.selectAllAppsCheckBox.Click += new System.EventHandler(this.SelectAllAppsCheckBox_Click);
            // 
            // failedAppInstallLabel
            // 
            this.failedAppInstallLabel.AutoSize = true;
            this.failedAppInstallLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.failedAppInstallLabel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(186)))), ((int)(((byte)(0)))));
            this.failedAppInstallLabel.Location = new System.Drawing.Point(150, 2);
            this.failedAppInstallLabel.Name = "failedAppInstallLabel";
            this.failedAppInstallLabel.Size = new System.Drawing.Size(484, 15);
            this.failedAppInstallLabel.TabIndex = 3;
            this.failedAppInstallLabel.Text = "Some applications failed to install. Would you like to retry installing these?";
            this.failedAppInstallLabel.Visible = false;
            // 
            // installAppsButton
            // 
            this.installAppsButton.Location = new System.Drawing.Point(398, 394);
            this.installAppsButton.Name = "installAppsButton";
            this.installAppsButton.Size = new System.Drawing.Size(93, 23);
            this.installAppsButton.TabIndex = 2;
            this.installAppsButton.Text = "Install Selected";
            this.installAppsButton.UseVisualStyleBackColor = true;
            this.installAppsButton.Click += new System.EventHandler(this.InstallAppsButton_Click);
            // 
            // refreshAppsButton
            // 
            this.refreshAppsButton.Location = new System.Drawing.Point(294, 394);
            this.refreshAppsButton.Name = "refreshAppsButton";
            this.refreshAppsButton.Size = new System.Drawing.Size(75, 23);
            this.refreshAppsButton.TabIndex = 0;
            this.refreshAppsButton.Text = "Refresh";
            this.refreshAppsButton.UseVisualStyleBackColor = true;
            this.refreshAppsButton.Click += new System.EventHandler(this.RefreshAppsButton_Click);
            // 
            // appContentPanel
            // 
            this.appContentPanel.AutoScroll = true;
            this.appContentPanel.Location = new System.Drawing.Point(0, 20);
            this.appContentPanel.Name = "appContentPanel";
            this.appContentPanel.Size = new System.Drawing.Size(785, 365);
            this.appContentPanel.TabIndex = 0;
            // 
            // installedPortableAppsPage
            // 
            this.installedPortableAppsPage.BackColor = System.Drawing.SystemColors.ButtonHighlight;
            this.installedPortableAppsPage.Controls.Add(this.selectAllPortableCheckBox1);
            this.installedPortableAppsPage.Controls.Add(this.failedPortableInstallLabel1);
            this.installedPortableAppsPage.Controls.Add(this.refreshPortableButton);
            this.installedPortableAppsPage.Controls.Add(this.installedPortableContentPanel);
            this.installedPortableAppsPage.Location = new System.Drawing.Point(0, 35);
            this.installedPortableAppsPage.Name = "installedPortableAppsPage";
            this.installedPortableAppsPage.Size = new System.Drawing.Size(785, 425);
            this.installedPortableAppsPage.TabIndex = 12;
            // 
            // selectAllPortableCheckBox1
            // 
            this.selectAllPortableCheckBox1.AutoSize = true;
            this.selectAllPortableCheckBox1.Location = new System.Drawing.Point(6, 2);
            this.selectAllPortableCheckBox1.Name = "selectAllPortableCheckBox1";
            this.selectAllPortableCheckBox1.Size = new System.Drawing.Size(70, 17);
            this.selectAllPortableCheckBox1.TabIndex = 1;
            this.selectAllPortableCheckBox1.Text = "Select All";
            this.selectAllPortableCheckBox1.UseVisualStyleBackColor = true;
            this.selectAllPortableCheckBox1.CheckedChanged += new System.EventHandler(this.SelectAllPortableCheckBox1_CheckedChanged);
            // 
            // failedPortableInstallLabel1
            // 
            this.failedPortableInstallLabel1.AutoSize = true;
            this.failedPortableInstallLabel1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.failedPortableInstallLabel1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(186)))), ((int)(((byte)(0)))));
            this.failedPortableInstallLabel1.Location = new System.Drawing.Point(145, 2);
            this.failedPortableInstallLabel1.Name = "failedPortableInstallLabel1";
            this.failedPortableInstallLabel1.Size = new System.Drawing.Size(495, 15);
            this.failedPortableInstallLabel1.TabIndex = 3;
            this.failedPortableInstallLabel1.Text = "Some Portable Apps failed to install. Would you like to retry installing these?";
            this.failedPortableInstallLabel1.Visible = false;
            // 
            // refreshPortableButton
            // 
            this.refreshPortableButton.Location = new System.Drawing.Point(355, 394);
            this.refreshPortableButton.Name = "refreshPortableButton";
            this.refreshPortableButton.Size = new System.Drawing.Size(75, 23);
            this.refreshPortableButton.TabIndex = 0;
            this.refreshPortableButton.Text = "Refresh";
            this.refreshPortableButton.UseVisualStyleBackColor = true;
            this.refreshPortableButton.Click += new System.EventHandler(this.RefreshPortableButton_Click);
            // 
            // installedPortableContentPanel
            // 
            this.installedPortableContentPanel.AutoScroll = true;
            this.installedPortableContentPanel.Location = new System.Drawing.Point(0, 20);
            this.installedPortableContentPanel.Name = "installedPortableContentPanel";
            this.installedPortableContentPanel.Size = new System.Drawing.Size(785, 365);
            this.installedPortableContentPanel.TabIndex = 0;
            // 
            // setPortableAppFolderPage
            // 
            this.setPortableAppFolderPage.BackColor = System.Drawing.SystemColors.ButtonHighlight;
            this.setPortableAppFolderPage.Controls.Add(this.paFolderNotWriteableLabel2);
            this.setPortableAppFolderPage.Controls.Add(this.browseButton2);
            this.setPortableAppFolderPage.Controls.Add(this.locationBox2);
            this.setPortableAppFolderPage.Controls.Add(this.instructionLabel);
            this.setPortableAppFolderPage.Controls.Add(this.setPortableAppFolderButton);
            this.setPortableAppFolderPage.Location = new System.Drawing.Point(0, 34);
            this.setPortableAppFolderPage.Name = "setPortableAppFolderPage";
            this.setPortableAppFolderPage.Size = new System.Drawing.Size(785, 425);
            this.setPortableAppFolderPage.TabIndex = 13;
            // 
            // paFolderNotWriteableLabel2
            // 
            this.paFolderNotWriteableLabel2.AutoSize = true;
            this.paFolderNotWriteableLabel2.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.paFolderNotWriteableLabel2.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(186)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.paFolderNotWriteableLabel2.Location = new System.Drawing.Point(92, 207);
            this.paFolderNotWriteableLabel2.Name = "paFolderNotWriteableLabel2";
            this.paFolderNotWriteableLabel2.Size = new System.Drawing.Size(515, 15);
            this.paFolderNotWriteableLabel2.TabIndex = 7;
            this.paFolderNotWriteableLabel2.Text = "This folder is not writeable by the current user, please choose a different folde" +
    "r.";
            this.paFolderNotWriteableLabel2.Visible = false;
            // 
            // browseButton2
            // 
            this.browseButton2.Location = new System.Drawing.Point(627, 183);
            this.browseButton2.Name = "browseButton2";
            this.browseButton2.Size = new System.Drawing.Size(75, 23);
            this.browseButton2.TabIndex = 6;
            this.browseButton2.Text = "Browse";
            this.browseButton2.UseVisualStyleBackColor = true;
            this.browseButton2.Click += new System.EventHandler(this.BrowseButton2_Click);
            // 
            // locationBox2
            // 
            this.locationBox2.Location = new System.Drawing.Point(89, 185);
            this.locationBox2.Name = "locationBox2";
            this.locationBox2.Size = new System.Drawing.Size(520, 20);
            this.locationBox2.TabIndex = 5;
            this.locationBox2.TextChanged += new System.EventHandler(this.LocationBox2_TextChanged);
            // 
            // instructionLabel
            // 
            this.instructionLabel.AutoSize = true;
            this.instructionLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.instructionLabel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(186)))), ((int)(((byte)(0)))));
            this.instructionLabel.Location = new System.Drawing.Point(101, 40);
            this.instructionLabel.Name = "instructionLabel";
            this.instructionLabel.Size = new System.Drawing.Size(582, 45);
            this.instructionLabel.TabIndex = 3;
            this.instructionLabel.Text = resources.GetString("instructionLabel.Text");
            // 
            // setPortableAppFolderButton
            // 
            this.setPortableAppFolderButton.Location = new System.Drawing.Point(355, 393);
            this.setPortableAppFolderButton.Name = "setPortableAppFolderButton";
            this.setPortableAppFolderButton.Size = new System.Drawing.Size(74, 23);
            this.setPortableAppFolderButton.TabIndex = 4;
            this.setPortableAppFolderButton.Text = "OK";
            this.setPortableAppFolderButton.UseVisualStyleBackColor = true;
            this.setPortableAppFolderButton.Click += new System.EventHandler(this.SetPortableAppFolderButton_Click);
            // 
            // settingsPage
            // 
            this.settingsPage.BackColor = System.Drawing.SystemColors.ButtonHighlight;
            this.settingsPage.Controls.Add(this.trayIconCheckBox);
            this.settingsPage.Controls.Add(this.autoStartCheckBox);
            this.settingsPage.Controls.Add(this.paFolderLocationLabel);
            this.settingsPage.Controls.Add(this.locationBox1);
            this.settingsPage.Controls.Add(this.browseButton1);
            this.settingsPage.Controls.Add(this.paFolderNotWriteableLabel1);
            this.settingsPage.Controls.Add(this.defenitionsGroupBox);
            this.settingsPage.Controls.Add(this.resetButton);
            this.settingsPage.Controls.Add(this.saveButton);
            this.settingsPage.Location = new System.Drawing.Point(0, 34);
            this.settingsPage.Name = "settingsPage";
            this.settingsPage.Size = new System.Drawing.Size(785, 425);
            this.settingsPage.TabIndex = 14;
            // 
            // trayIconCheckBox
            // 
            this.trayIconCheckBox.AutoSize = true;
            this.trayIconCheckBox.Location = new System.Drawing.Point(12, 30);
            this.trayIconCheckBox.Name = "trayIconCheckBox";
            this.trayIconCheckBox.Size = new System.Drawing.Size(316, 17);
            this.trayIconCheckBox.TabIndex = 5;
            this.trayIconCheckBox.Text = "Keep running as a system tray icon when I close Slim Updater";
            this.trayIconCheckBox.UseVisualStyleBackColor = true;
            // 
            // autoStartCheckBox
            // 
            this.autoStartCheckBox.AutoSize = true;
            this.autoStartCheckBox.Location = new System.Drawing.Point(12, 6);
            this.autoStartCheckBox.Name = "autoStartCheckBox";
            this.autoStartCheckBox.Size = new System.Drawing.Size(235, 17);
            this.autoStartCheckBox.TabIndex = 4;
            this.autoStartCheckBox.Text = "Auto-start Slim Updater as a system tray icon";
            this.autoStartCheckBox.UseVisualStyleBackColor = true;
            // 
            // paFolderLocationLabel
            // 
            this.paFolderLocationLabel.AutoSize = true;
            this.paFolderLocationLabel.Location = new System.Drawing.Point(9, 61);
            this.paFolderLocationLabel.Name = "paFolderLocationLabel";
            this.paFolderLocationLabel.Size = new System.Drawing.Size(152, 13);
            this.paFolderLocationLabel.TabIndex = 9;
            this.paFolderLocationLabel.Text = "Portable Apps Folder Location:";
            // 
            // locationBox1
            // 
            this.locationBox1.Location = new System.Drawing.Point(12, 78);
            this.locationBox1.Name = "locationBox1";
            this.locationBox1.Size = new System.Drawing.Size(659, 20);
            this.locationBox1.TabIndex = 11;
            this.locationBox1.TextChanged += new System.EventHandler(this.LocationBox1_TextChanged);
            // 
            // browseButton1
            // 
            this.browseButton1.Location = new System.Drawing.Point(687, 77);
            this.browseButton1.Name = "browseButton1";
            this.browseButton1.Size = new System.Drawing.Size(75, 23);
            this.browseButton1.TabIndex = 10;
            this.browseButton1.Text = "Browse";
            this.browseButton1.UseVisualStyleBackColor = true;
            this.browseButton1.Click += new System.EventHandler(this.BrowseButton1_Click);
            // 
            // paFolderNotWriteableLabel1
            // 
            this.paFolderNotWriteableLabel1.AutoSize = true;
            this.paFolderNotWriteableLabel1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.paFolderNotWriteableLabel1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(186)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.paFolderNotWriteableLabel1.Location = new System.Drawing.Point(93, 99);
            this.paFolderNotWriteableLabel1.Name = "paFolderNotWriteableLabel1";
            this.paFolderNotWriteableLabel1.Size = new System.Drawing.Size(515, 15);
            this.paFolderNotWriteableLabel1.TabIndex = 12;
            this.paFolderNotWriteableLabel1.Text = "This folder is not writeable by the current user, please choose a different folde" +
    "r.";
            this.paFolderNotWriteableLabel1.Visible = false;
            // 
            // defenitionsGroupBox
            // 
            this.defenitionsGroupBox.Controls.Add(this.warningLabel);
            this.defenitionsGroupBox.Controls.Add(this.customURLTextBox);
            this.defenitionsGroupBox.Controls.Add(this.customDefenRadioBtn);
            this.defenitionsGroupBox.Controls.Add(this.officialDefenRadioBtn);
            this.defenitionsGroupBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.defenitionsGroupBox.Location = new System.Drawing.Point(12, 122);
            this.defenitionsGroupBox.Name = "defenitionsGroupBox";
            this.defenitionsGroupBox.Size = new System.Drawing.Size(761, 108);
            this.defenitionsGroupBox.TabIndex = 1;
            this.defenitionsGroupBox.TabStop = false;
            this.defenitionsGroupBox.Text = "Defenitions";
            // 
            // warningLabel
            // 
            this.warningLabel.AutoSize = true;
            this.warningLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.warningLabel.Location = new System.Drawing.Point(6, 88);
            this.warningLabel.Name = "warningLabel";
            this.warningLabel.Size = new System.Drawing.Size(302, 13);
            this.warningLabel.TabIndex = 3;
            this.warningLabel.Text = "Make sure to only use custom Defenitions from trusted sources";
            // 
            // customURLTextBox
            // 
            this.customURLTextBox.Enabled = false;
            this.customURLTextBox.Location = new System.Drawing.Point(6, 65);
            this.customURLTextBox.Name = "customURLTextBox";
            this.customURLTextBox.Size = new System.Drawing.Size(290, 20);
            this.customURLTextBox.TabIndex = 2;
            // 
            // customDefenRadioBtn
            // 
            this.customDefenRadioBtn.AutoSize = true;
            this.customDefenRadioBtn.Location = new System.Drawing.Point(6, 42);
            this.customDefenRadioBtn.Name = "customDefenRadioBtn";
            this.customDefenRadioBtn.Size = new System.Drawing.Size(185, 17);
            this.customDefenRadioBtn.TabIndex = 1;
            this.customDefenRadioBtn.Text = "Use the following Defenition URL:";
            this.customDefenRadioBtn.UseVisualStyleBackColor = true;
            this.customDefenRadioBtn.Click += new System.EventHandler(this.CustomDefenRadioBtn_Click);
            // 
            // officialDefenRadioBtn
            // 
            this.officialDefenRadioBtn.AutoSize = true;
            this.officialDefenRadioBtn.Checked = true;
            this.officialDefenRadioBtn.Location = new System.Drawing.Point(6, 19);
            this.officialDefenRadioBtn.Name = "officialDefenRadioBtn";
            this.officialDefenRadioBtn.Size = new System.Drawing.Size(290, 17);
            this.officialDefenRadioBtn.TabIndex = 0;
            this.officialDefenRadioBtn.TabStop = true;
            this.officialDefenRadioBtn.Text = "Use the official Slim Updater Defenitions (recommended)";
            this.officialDefenRadioBtn.UseVisualStyleBackColor = true;
            this.officialDefenRadioBtn.Click += new System.EventHandler(this.OfficialDefenRadioBtn_Click);
            // 
            // resetButton
            // 
            this.resetButton.Location = new System.Drawing.Point(402, 393);
            this.resetButton.Name = "resetButton";
            this.resetButton.Size = new System.Drawing.Size(75, 23);
            this.resetButton.TabIndex = 3;
            this.resetButton.Text = "Reset";
            this.resetButton.UseVisualStyleBackColor = true;
            // 
            // saveButton
            // 
            this.saveButton.Location = new System.Drawing.Point(302, 393);
            this.saveButton.Name = "saveButton";
            this.saveButton.Size = new System.Drawing.Size(75, 23);
            this.saveButton.TabIndex = 2;
            this.saveButton.Text = "Save";
            this.saveButton.UseVisualStyleBackColor = true;
            this.saveButton.Click += new System.EventHandler(this.SaveButton_Click);
            // 
            // getPortableAppsPage
            // 
            this.getPortableAppsPage.BackColor = System.Drawing.SystemColors.ButtonHighlight;
            this.getPortableAppsPage.Controls.Add(this.selectAllPortableCheckBox2);
            this.getPortableAppsPage.Controls.Add(this.failedPortableInstallLabel2);
            this.getPortableAppsPage.Controls.Add(this.downloadPortableButton);
            this.getPortableAppsPage.Controls.Add(this.refreshPortableButton2);
            this.getPortableAppsPage.Controls.Add(this.getPortableContentPanel);
            this.getPortableAppsPage.Location = new System.Drawing.Point(0, 35);
            this.getPortableAppsPage.Name = "getPortableAppsPage";
            this.getPortableAppsPage.Size = new System.Drawing.Size(785, 425);
            this.getPortableAppsPage.TabIndex = 15;
            // 
            // selectAllPortableCheckBox2
            // 
            this.selectAllPortableCheckBox2.AutoSize = true;
            this.selectAllPortableCheckBox2.Location = new System.Drawing.Point(6, 2);
            this.selectAllPortableCheckBox2.Name = "selectAllPortableCheckBox2";
            this.selectAllPortableCheckBox2.Size = new System.Drawing.Size(70, 17);
            this.selectAllPortableCheckBox2.TabIndex = 1;
            this.selectAllPortableCheckBox2.Text = "Select All";
            this.selectAllPortableCheckBox2.UseVisualStyleBackColor = true;
            this.selectAllPortableCheckBox2.Click += new System.EventHandler(this.SelectAllPortableCheckBox2_CheckedChanged);
            // 
            // failedPortableInstallLabel2
            // 
            this.failedPortableInstallLabel2.AutoSize = true;
            this.failedPortableInstallLabel2.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.failedPortableInstallLabel2.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(186)))), ((int)(((byte)(0)))));
            this.failedPortableInstallLabel2.Location = new System.Drawing.Point(145, 2);
            this.failedPortableInstallLabel2.Name = "failedPortableInstallLabel2";
            this.failedPortableInstallLabel2.Size = new System.Drawing.Size(495, 15);
            this.failedPortableInstallLabel2.TabIndex = 3;
            this.failedPortableInstallLabel2.Text = "Some Portable Apps failed to install. Would you like to retry installing these?";
            this.failedPortableInstallLabel2.Visible = false;
            // 
            // downloadPortableButton
            // 
            this.downloadPortableButton.Location = new System.Drawing.Point(386, 394);
            this.downloadPortableButton.Name = "downloadPortableButton";
            this.downloadPortableButton.Size = new System.Drawing.Size(117, 23);
            this.downloadPortableButton.TabIndex = 2;
            this.downloadPortableButton.Text = "Download Selected";
            this.downloadPortableButton.UseVisualStyleBackColor = true;
            this.downloadPortableButton.Click += new System.EventHandler(this.DownloadPortableButton_Click);
            // 
            // refreshPortableButton2
            // 
            this.refreshPortableButton2.Location = new System.Drawing.Point(281, 394);
            this.refreshPortableButton2.Name = "refreshPortableButton2";
            this.refreshPortableButton2.Size = new System.Drawing.Size(75, 23);
            this.refreshPortableButton2.TabIndex = 0;
            this.refreshPortableButton2.Text = "Refresh";
            this.refreshPortableButton2.UseVisualStyleBackColor = true;
            this.refreshPortableButton2.Click += new System.EventHandler(this.RefreshPortableButton2_Click);
            // 
            // getPortableContentPanel
            // 
            this.getPortableContentPanel.AutoScroll = true;
            this.getPortableContentPanel.Location = new System.Drawing.Point(0, 20);
            this.getPortableContentPanel.Name = "getPortableContentPanel";
            this.getPortableContentPanel.Size = new System.Drawing.Size(785, 365);
            this.getPortableContentPanel.TabIndex = 0;
            // 
            // MainWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.BackColor = System.Drawing.SystemColors.ButtonHighlight;
            this.ClientSize = new System.Drawing.Size(784, 461);
            this.Controls.Add(this.topBar);
            this.Controls.Add(this.startPage);
            this.Controls.Add(this.updatePage);
            this.Controls.Add(this.getNewAppsPage);
            this.Controls.Add(this.installedPortableAppsPage);
            this.Controls.Add(this.getPortableAppsPage);
            this.Controls.Add(this.setPortableAppFolderPage);
            this.Controls.Add(this.settingsPage);
            this.Controls.Add(this.detailsPage);
            this.Controls.Add(this.aboutPage);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "MainWindow";
            this.Text = "Slim Updater";
            this.Shown += new System.EventHandler(this.MainWindow_Shown);
            this.startPage.ResumeLayout(false);
            this.topBar.ResumeLayout(false);
            this.topBar.PerformLayout();
            this.updatePage.ResumeLayout(false);
            this.updatePage.PerformLayout();
            this.aboutPage.ResumeLayout(false);
            this.aboutPage.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.slimUpdaterLogo)).EndInit();
            this.detailsPage.ResumeLayout(false);
            this.detailsPage.PerformLayout();
            this.getNewAppsPage.ResumeLayout(false);
            this.getNewAppsPage.PerformLayout();
            this.installedPortableAppsPage.ResumeLayout(false);
            this.installedPortableAppsPage.PerformLayout();
            this.setPortableAppFolderPage.ResumeLayout(false);
            this.setPortableAppFolderPage.PerformLayout();
            this.settingsPage.ResumeLayout(false);
            this.settingsPage.PerformLayout();
            this.defenitionsGroupBox.ResumeLayout(false);
            this.defenitionsGroupBox.PerformLayout();
            this.getPortableAppsPage.ResumeLayout(false);
            this.getPortableAppsPage.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Panel startPage;
        private System.Windows.Forms.Panel topBar;
        private System.Windows.Forms.Label aboutLabel;
        private System.Windows.Forms.Panel updatePage;
        private flatTile updaterTile;
        private flatTile settingsTile;
        private flatTile getNewAppsTile;
        private flatTile portableAppsTile;
        private System.Windows.Forms.Panel aboutPage;
        private System.Windows.Forms.PictureBox slimUpdaterLogo;
        private System.Windows.Forms.Label aboutTitle;
        private System.Windows.Forms.LinkLabel siteLink;
        private System.Windows.Forms.Label slimsoftwareLabel;
        private System.Windows.Forms.Label versionLabel;
        private System.Windows.Forms.Panel updateContentPanel;
        private System.Windows.Forms.CheckBox selectAllUpdatesCheckBox;
        private System.Windows.Forms.Button installUpdatesButton;
        private System.Windows.Forms.Button refreshUpdatesButton;
        private System.Windows.Forms.Panel detailsPage;
        private System.Windows.Forms.LinkLabel actionLink;
        private System.Windows.Forms.Label detailLabel;
        private System.Windows.Forms.RichTextBox detailText;
        private System.Windows.Forms.Label failedUpdateLabel;
        private System.Windows.Forms.Panel getNewAppsPage;
        private System.Windows.Forms.Label failedAppInstallLabel;
        private System.Windows.Forms.Button installAppsButton;
        private System.Windows.Forms.Button refreshAppsButton;
        private System.Windows.Forms.CheckBox selectAllAppsCheckBox;
        private System.Windows.Forms.Panel appContentPanel;
        private System.Windows.Forms.Panel installedPortableAppsPage;
        private System.Windows.Forms.Label failedPortableInstallLabel1;
        private System.Windows.Forms.Button refreshPortableButton;
        private System.Windows.Forms.CheckBox selectAllPortableCheckBox1;
        private System.Windows.Forms.Panel installedPortableContentPanel;
        private System.Windows.Forms.Panel setPortableAppFolderPage;
        private System.Windows.Forms.Button browseButton2;
        private System.Windows.Forms.TextBox locationBox2;
        private System.Windows.Forms.Label instructionLabel;
        private System.Windows.Forms.Button setPortableAppFolderButton;
        private System.Windows.Forms.Label paFolderNotWriteableLabel2;
        private System.Windows.Forms.Panel settingsPage;
        private System.Windows.Forms.Button resetButton;
        private System.Windows.Forms.Button saveButton;
        private System.Windows.Forms.GroupBox defenitionsGroupBox;
        private System.Windows.Forms.Label warningLabel;
        private System.Windows.Forms.TextBox customURLTextBox;
        private System.Windows.Forms.RadioButton customDefenRadioBtn;
        private System.Windows.Forms.RadioButton officialDefenRadioBtn;
        private System.Windows.Forms.CheckBox trayIconCheckBox;
        private System.Windows.Forms.CheckBox autoStartCheckBox;
        private System.Windows.Forms.Button browseButton1;
        private System.Windows.Forms.Label paFolderLocationLabel;
        private System.Windows.Forms.TextBox locationBox1;
        private System.Windows.Forms.Label paFolderNotWriteableLabel1;
        private TitleButton titleButtonLeft;
        private TitleButton titleButtonRight;
        private System.Windows.Forms.Panel getPortableAppsPage;
        private System.Windows.Forms.CheckBox selectAllPortableCheckBox2;
        private System.Windows.Forms.Label failedPortableInstallLabel2;
        private System.Windows.Forms.Button downloadPortableButton;
        private System.Windows.Forms.Button refreshPortableButton2;
        private System.Windows.Forms.Panel getPortableContentPanel;
    }
}

