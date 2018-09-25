namespace SlimUpdater
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
            this.offlineRetryLink = new System.Windows.Forms.LinkLabel();
            this.offlineLabel = new System.Windows.Forms.Label();
            this.topBar = new System.Windows.Forms.Panel();
            this.logButton = new System.Windows.Forms.Label();
            this.aboutButton = new System.Windows.Forms.Label();
            this.selectAllUpdatesCheckBox = new System.Windows.Forms.CheckBox();
            this.updateContentPanel = new System.Windows.Forms.Panel();
            this.updatePage = new System.Windows.Forms.Panel();
            this.updatesStatusLabel = new System.Windows.Forms.Label();
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
            this.newAppsStatusLabel = new System.Windows.Forms.Label();
            this.selectAllAppsCheckBox = new System.Windows.Forms.CheckBox();
            this.installAppsButton = new System.Windows.Forms.Button();
            this.refreshAppsButton = new System.Windows.Forms.Button();
            this.getNewAppsContentPanel = new System.Windows.Forms.Panel();
            this.installedPortableAppsPage = new System.Windows.Forms.Panel();
            this.refreshPortableButton = new System.Windows.Forms.Button();
            this.installedPortableContentPanel = new System.Windows.Forms.Panel();
            this.setPortableAppFolderPage = new System.Windows.Forms.Panel();
            this.paFolderNotWriteableLabel2 = new System.Windows.Forms.Label();
            this.browseButton2 = new System.Windows.Forms.Button();
            this.locationBox2 = new System.Windows.Forms.TextBox();
            this.instructionLabel = new System.Windows.Forms.Label();
            this.setPortableAppFolderButton = new System.Windows.Forms.Button();
            this.settingsPage = new System.Windows.Forms.Panel();
            this.autoStartCheckBox = new System.Windows.Forms.CheckBox();
            this.minimizeToTrayCheckBox = new System.Windows.Forms.CheckBox();
            this.dataFolderLabel = new System.Windows.Forms.Label();
            this.dataLocationBox = new System.Windows.Forms.TextBox();
            this.dataBrowseButton = new System.Windows.Forms.Button();
            this.openDataDirButton = new System.Windows.Forms.Button();
            this.dataFolderNotWriteableLabel = new System.Windows.Forms.Label();
            this.paFolderLocationLabel = new System.Windows.Forms.Label();
            this.paLocationBox = new System.Windows.Forms.TextBox();
            this.paBrowseButton = new System.Windows.Forms.Button();
            this.openPAFolderButton = new System.Windows.Forms.Button();
            this.paFolderNotWriteableLabel = new System.Windows.Forms.Label();
            this.defenitionsGroupBox = new System.Windows.Forms.GroupBox();
            this.warningLabel = new System.Windows.Forms.Label();
            this.customURLTextBox = new System.Windows.Forms.TextBox();
            this.customDefenRadioBtn = new System.Windows.Forms.RadioButton();
            this.officialDefenRadioBtn = new System.Windows.Forms.RadioButton();
            this.saveButton = new System.Windows.Forms.Button();
            this.resetButton = new System.Windows.Forms.Button();
            this.getPortableAppsPage = new System.Windows.Forms.Panel();
            this.selectAllPortableCheckBox = new System.Windows.Forms.CheckBox();
            this.portableStatusLabel = new System.Windows.Forms.Label();
            this.downloadPortableButton = new System.Windows.Forms.Button();
            this.refreshPortableButton2 = new System.Windows.Forms.Button();
            this.getPortableContentPanel = new System.Windows.Forms.Panel();
            this.trayIcon = new System.Windows.Forms.NotifyIcon(this.components);
            this.trayIconContextMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.checkUpdatesTrayIconMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.openTrayIconMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.settingsTrayIconMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.exitTrayIconMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.logPage = new System.Windows.Forms.Panel();
            this.logTextBox = new System.Windows.Forms.RichTextBox();
            this.titleButtonLeft = new SlimUpdater.TitleButton();
            this.titleButtonRight = new SlimUpdater.TitleButton();
            this.updaterTile = new SlimUpdater.flatTile();
            this.getNewAppsTile = new SlimUpdater.flatTile();
            this.portableAppsTile = new SlimUpdater.flatTile();
            this.settingsTile = new SlimUpdater.flatTile();
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
            this.trayIconContextMenu.SuspendLayout();
            this.logPage.SuspendLayout();
            this.SuspendLayout();
            // 
            // startPage
            // 
            this.startPage.BackColor = System.Drawing.SystemColors.ButtonHighlight;
            this.startPage.Controls.Add(this.offlineRetryLink);
            this.startPage.Controls.Add(this.offlineLabel);
            this.startPage.Controls.Add(this.updaterTile);
            this.startPage.Controls.Add(this.getNewAppsTile);
            this.startPage.Controls.Add(this.portableAppsTile);
            this.startPage.Controls.Add(this.settingsTile);
            this.startPage.ForeColor = System.Drawing.SystemColors.Control;
            this.startPage.Location = new System.Drawing.Point(0, 42);
            this.startPage.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.startPage.Name = "startPage";
            this.startPage.Size = new System.Drawing.Size(981, 534);
            this.startPage.TabIndex = 1;
            // 
            // offlineRetryLink
            // 
            this.offlineRetryLink.AutoSize = true;
            this.offlineRetryLink.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.offlineRetryLink.ForeColor = System.Drawing.Color.Red;
            this.offlineRetryLink.LinkColor = System.Drawing.Color.Red;
            this.offlineRetryLink.Location = new System.Drawing.Point(600, 12);
            this.offlineRetryLink.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.offlineRetryLink.Name = "offlineRetryLink";
            this.offlineRetryLink.Size = new System.Drawing.Size(176, 20);
            this.offlineRetryLink.TabIndex = 10;
            this.offlineRetryLink.TabStop = true;
            this.offlineRetryLink.Text = "Click here to retry...";
            this.offlineRetryLink.Visible = false;
            this.offlineRetryLink.VisitedLinkColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(186)))), ((int)(((byte)(0)))));
            this.offlineRetryLink.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.OfflineRetryLink_LinkClicked);
            // 
            // offlineLabel
            // 
            this.offlineLabel.AutoSize = true;
            this.offlineLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.offlineLabel.ForeColor = System.Drawing.Color.Red;
            this.offlineLabel.Location = new System.Drawing.Point(205, 12);
            this.offlineLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.offlineLabel.Name = "offlineLabel";
            this.offlineLabel.Size = new System.Drawing.Size(367, 20);
            this.offlineLabel.TabIndex = 9;
            this.offlineLabel.Text = "Could not connect to the defenition server.";
            this.offlineLabel.Visible = false;
            // 
            // topBar
            // 
            this.topBar.BackColor = System.Drawing.SystemColors.ButtonHighlight;
            this.topBar.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.topBar.Controls.Add(this.logButton);
            this.topBar.Controls.Add(this.titleButtonLeft);
            this.topBar.Controls.Add(this.aboutButton);
            this.topBar.Controls.Add(this.titleButtonRight);
            this.topBar.Location = new System.Drawing.Point(-9, -1);
            this.topBar.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.topBar.Name = "topBar";
            this.topBar.Size = new System.Drawing.Size(997, 44);
            this.topBar.TabIndex = 5;
            // 
            // logButton
            // 
            this.logButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.logButton.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(186)))), ((int)(((byte)(0)))));
            this.logButton.Location = new System.Drawing.Point(862, 11);
            this.logButton.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.logButton.Name = "logButton";
            this.logButton.Size = new System.Drawing.Size(44, 21);
            this.logButton.TabIndex = 5;
            this.logButton.Text = "Log";
            this.logButton.Click += new System.EventHandler(this.LogButton_Click);
            this.logButton.MouseEnter += new System.EventHandler(this.LogButton_MouseEnter);
            this.logButton.MouseLeave += new System.EventHandler(this.LogButton_MouseLeave);
            // 
            // aboutButton
            // 
            this.aboutButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.aboutButton.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(186)))), ((int)(((byte)(0)))));
            this.aboutButton.Location = new System.Drawing.Point(911, 11);
            this.aboutButton.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.aboutButton.Name = "aboutButton";
            this.aboutButton.Size = new System.Drawing.Size(62, 21);
            this.aboutButton.TabIndex = 1;
            this.aboutButton.Text = "About";
            this.aboutButton.Click += new System.EventHandler(this.AboutLabel_Click);
            this.aboutButton.MouseEnter += new System.EventHandler(this.AboutLabel_MouseEnter);
            this.aboutButton.MouseLeave += new System.EventHandler(this.AboutLabel_MouseLeave);
            // 
            // selectAllUpdatesCheckBox
            // 
            this.selectAllUpdatesCheckBox.AutoSize = true;
            this.selectAllUpdatesCheckBox.Checked = true;
            this.selectAllUpdatesCheckBox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.selectAllUpdatesCheckBox.Location = new System.Drawing.Point(8, 2);
            this.selectAllUpdatesCheckBox.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.selectAllUpdatesCheckBox.Name = "selectAllUpdatesCheckBox";
            this.selectAllUpdatesCheckBox.Size = new System.Drawing.Size(104, 21);
            this.selectAllUpdatesCheckBox.TabIndex = 1;
            this.selectAllUpdatesCheckBox.Text = "Unselect All";
            this.selectAllUpdatesCheckBox.UseVisualStyleBackColor = true;
            this.selectAllUpdatesCheckBox.Click += new System.EventHandler(this.SelectAllUpdatesCheckBox_Click);
            // 
            // updateContentPanel
            // 
            this.updateContentPanel.AutoScroll = true;
            this.updateContentPanel.Location = new System.Drawing.Point(0, 25);
            this.updateContentPanel.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.updateContentPanel.Name = "updateContentPanel";
            this.updateContentPanel.Size = new System.Drawing.Size(981, 456);
            this.updateContentPanel.TabIndex = 0;
            // 
            // updatePage
            // 
            this.updatePage.BackColor = System.Drawing.SystemColors.ButtonHighlight;
            this.updatePage.Controls.Add(this.updatesStatusLabel);
            this.updatePage.Controls.Add(this.selectAllUpdatesCheckBox);
            this.updatePage.Controls.Add(this.installUpdatesButton);
            this.updatePage.Controls.Add(this.refreshUpdatesButton);
            this.updatePage.Controls.Add(this.updateContentPanel);
            this.updatePage.Location = new System.Drawing.Point(0, 44);
            this.updatePage.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.updatePage.Name = "updatePage";
            this.updatePage.Size = new System.Drawing.Size(981, 534);
            this.updatePage.TabIndex = 6;
            // 
            // updatesStatusLabel
            // 
            this.updatesStatusLabel.AutoSize = true;
            this.updatesStatusLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold);
            this.updatesStatusLabel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(186)))), ((int)(((byte)(0)))));
            this.updatesStatusLabel.Location = new System.Drawing.Point(340, 2);
            this.updatesStatusLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.updatesStatusLabel.Name = "updatesStatusLabel";
            this.updatesStatusLabel.Size = new System.Drawing.Size(281, 20);
            this.updatesStatusLabel.TabIndex = 3;
            this.updatesStatusLabel.Text = "Succesfully installed all updates";
            this.updatesStatusLabel.Visible = false;
            // 
            // installUpdatesButton
            // 
            this.installUpdatesButton.Location = new System.Drawing.Point(498, 492);
            this.installUpdatesButton.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.installUpdatesButton.Name = "installUpdatesButton";
            this.installUpdatesButton.Size = new System.Drawing.Size(116, 29);
            this.installUpdatesButton.TabIndex = 2;
            this.installUpdatesButton.Text = "Install Selected";
            this.installUpdatesButton.UseVisualStyleBackColor = true;
            this.installUpdatesButton.Click += new System.EventHandler(this.InstallUpdatesButton_Click);
            // 
            // refreshUpdatesButton
            // 
            this.refreshUpdatesButton.Location = new System.Drawing.Point(368, 492);
            this.refreshUpdatesButton.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.refreshUpdatesButton.Name = "refreshUpdatesButton";
            this.refreshUpdatesButton.Size = new System.Drawing.Size(94, 29);
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
            this.aboutPage.Location = new System.Drawing.Point(0, 42);
            this.aboutPage.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.aboutPage.Name = "aboutPage";
            this.aboutPage.Size = new System.Drawing.Size(981, 534);
            this.aboutPage.TabIndex = 9;
            // 
            // siteLink
            // 
            this.siteLink.ActiveLinkColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(106)))), ((int)(((byte)(0)))));
            this.siteLink.AutoSize = true;
            this.siteLink.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.siteLink.LinkColor = System.Drawing.SystemColors.ButtonHighlight;
            this.siteLink.Location = new System.Drawing.Point(428, 484);
            this.siteLink.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.siteLink.Name = "siteLink";
            this.siteLink.Size = new System.Drawing.Size(125, 20);
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
            this.slimsoftwareLabel.Location = new System.Drawing.Point(242, 456);
            this.slimsoftwareLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.slimsoftwareLabel.Name = "slimsoftwareLabel";
            this.slimsoftwareLabel.Size = new System.Drawing.Size(492, 24);
            this.slimsoftwareLabel.TabIndex = 3;
            this.slimsoftwareLabel.Text = "Open Source Software developped by SlimSoftware\r\n";
            // 
            // versionLabel
            // 
            this.versionLabel.AutoSize = true;
            this.versionLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.versionLabel.ForeColor = System.Drawing.SystemColors.ButtonHighlight;
            this.versionLabel.Location = new System.Drawing.Point(419, 256);
            this.versionLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.versionLabel.Name = "versionLabel";
            this.versionLabel.Size = new System.Drawing.Size(140, 25);
            this.versionLabel.TabIndex = 2;
            this.versionLabel.Text = "Version 3.2.8";
            // 
            // aboutTitle
            // 
            this.aboutTitle.AutoSize = true;
            this.aboutTitle.Font = new System.Drawing.Font("Microsoft Sans Serif", 16F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.aboutTitle.ForeColor = System.Drawing.SystemColors.ButtonHighlight;
            this.aboutTitle.Location = new System.Drawing.Point(396, 215);
            this.aboutTitle.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.aboutTitle.Name = "aboutTitle";
            this.aboutTitle.Size = new System.Drawing.Size(182, 31);
            this.aboutTitle.TabIndex = 1;
            this.aboutTitle.Text = "Slim Updater";
            // 
            // slimUpdaterLogo
            // 
            this.slimUpdaterLogo.Image = global::SlimUpdater.Properties.Resources.SlimUpdater;
            this.slimUpdaterLogo.Location = new System.Drawing.Point(410, 24);
            this.slimUpdaterLogo.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.slimUpdaterLogo.Name = "slimUpdaterLogo";
            this.slimUpdaterLogo.Size = new System.Drawing.Size(160, 160);
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
            this.detailsPage.Location = new System.Drawing.Point(0, 41);
            this.detailsPage.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.detailsPage.Name = "detailsPage";
            this.detailsPage.Size = new System.Drawing.Size(981, 534);
            this.detailsPage.TabIndex = 10;
            // 
            // detailText
            // 
            this.detailText.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.detailText.BackColor = System.Drawing.Color.White;
            this.detailText.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.detailText.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.detailText.Location = new System.Drawing.Point(8, 36);
            this.detailText.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.detailText.Name = "detailText";
            this.detailText.ReadOnly = true;
            this.detailText.Size = new System.Drawing.Size(974, 499);
            this.detailText.TabIndex = 15;
            this.detailText.Text = "";
            // 
            // actionLink
            // 
            this.actionLink.AutoSize = true;
            this.actionLink.LinkColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(186)))), ((int)(((byte)(0)))));
            this.actionLink.Location = new System.Drawing.Point(4, 506);
            this.actionLink.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.actionLink.Name = "actionLink";
            this.actionLink.Size = new System.Drawing.Size(48, 17);
            this.actionLink.TabIndex = 13;
            this.actionLink.TabStop = true;
            this.actionLink.Text = "Ingore";
            this.actionLink.Visible = false;
            // 
            // detailLabel
            // 
            this.detailLabel.AutoSize = true;
            this.detailLabel.BackColor = System.Drawing.Color.Transparent;
            this.detailLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.detailLabel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(186)))), ((int)(((byte)(0)))));
            this.detailLabel.Location = new System.Drawing.Point(4, 5);
            this.detailLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.detailLabel.Name = "detailLabel";
            this.detailLabel.Size = new System.Drawing.Size(112, 24);
            this.detailLabel.TabIndex = 12;
            this.detailLabel.Text = "Changelog";
            // 
            // getNewAppsPage
            // 
            this.getNewAppsPage.BackColor = System.Drawing.SystemColors.ButtonHighlight;
            this.getNewAppsPage.Controls.Add(this.newAppsStatusLabel);
            this.getNewAppsPage.Controls.Add(this.selectAllAppsCheckBox);
            this.getNewAppsPage.Controls.Add(this.installAppsButton);
            this.getNewAppsPage.Controls.Add(this.refreshAppsButton);
            this.getNewAppsPage.Controls.Add(this.getNewAppsContentPanel);
            this.getNewAppsPage.Location = new System.Drawing.Point(0, 44);
            this.getNewAppsPage.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.getNewAppsPage.Name = "getNewAppsPage";
            this.getNewAppsPage.Size = new System.Drawing.Size(981, 534);
            this.getNewAppsPage.TabIndex = 11;
            // 
            // newAppsStatusLabel
            // 
            this.newAppsStatusLabel.AutoSize = true;
            this.newAppsStatusLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold);
            this.newAppsStatusLabel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(186)))), ((int)(((byte)(0)))));
            this.newAppsStatusLabel.Location = new System.Drawing.Point(340, 2);
            this.newAppsStatusLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.newAppsStatusLabel.Name = "newAppsStatusLabel";
            this.newAppsStatusLabel.Size = new System.Drawing.Size(316, 20);
            this.newAppsStatusLabel.TabIndex = 4;
            this.newAppsStatusLabel.Text = "Succesfully installed all applications";
            this.newAppsStatusLabel.Visible = false;
            // 
            // selectAllAppsCheckBox
            // 
            this.selectAllAppsCheckBox.AutoSize = true;
            this.selectAllAppsCheckBox.Location = new System.Drawing.Point(8, 2);
            this.selectAllAppsCheckBox.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.selectAllAppsCheckBox.Name = "selectAllAppsCheckBox";
            this.selectAllAppsCheckBox.Size = new System.Drawing.Size(88, 21);
            this.selectAllAppsCheckBox.TabIndex = 1;
            this.selectAllAppsCheckBox.Text = "Select All";
            this.selectAllAppsCheckBox.UseVisualStyleBackColor = true;
            this.selectAllAppsCheckBox.Click += new System.EventHandler(this.SelectAllAppsCheckBox_Click);
            // 
            // installAppsButton
            // 
            this.installAppsButton.Location = new System.Drawing.Point(498, 492);
            this.installAppsButton.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.installAppsButton.Name = "installAppsButton";
            this.installAppsButton.Size = new System.Drawing.Size(116, 29);
            this.installAppsButton.TabIndex = 2;
            this.installAppsButton.Text = "Install Selected";
            this.installAppsButton.UseVisualStyleBackColor = true;
            this.installAppsButton.Click += new System.EventHandler(this.InstallAppsButton_Click);
            // 
            // refreshAppsButton
            // 
            this.refreshAppsButton.Location = new System.Drawing.Point(368, 492);
            this.refreshAppsButton.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.refreshAppsButton.Name = "refreshAppsButton";
            this.refreshAppsButton.Size = new System.Drawing.Size(94, 29);
            this.refreshAppsButton.TabIndex = 0;
            this.refreshAppsButton.Text = "Refresh";
            this.refreshAppsButton.UseVisualStyleBackColor = true;
            this.refreshAppsButton.Click += new System.EventHandler(this.RefreshAppsButton_Click);
            // 
            // getNewAppsContentPanel
            // 
            this.getNewAppsContentPanel.AutoScroll = true;
            this.getNewAppsContentPanel.Location = new System.Drawing.Point(0, 25);
            this.getNewAppsContentPanel.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.getNewAppsContentPanel.Name = "getNewAppsContentPanel";
            this.getNewAppsContentPanel.Size = new System.Drawing.Size(981, 456);
            this.getNewAppsContentPanel.TabIndex = 0;
            // 
            // installedPortableAppsPage
            // 
            this.installedPortableAppsPage.BackColor = System.Drawing.SystemColors.ButtonHighlight;
            this.installedPortableAppsPage.Controls.Add(this.refreshPortableButton);
            this.installedPortableAppsPage.Controls.Add(this.installedPortableContentPanel);
            this.installedPortableAppsPage.Location = new System.Drawing.Point(0, 44);
            this.installedPortableAppsPage.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.installedPortableAppsPage.Name = "installedPortableAppsPage";
            this.installedPortableAppsPage.Size = new System.Drawing.Size(981, 534);
            this.installedPortableAppsPage.TabIndex = 12;
            // 
            // refreshPortableButton
            // 
            this.refreshPortableButton.Location = new System.Drawing.Point(444, 492);
            this.refreshPortableButton.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.refreshPortableButton.Name = "refreshPortableButton";
            this.refreshPortableButton.Size = new System.Drawing.Size(94, 29);
            this.refreshPortableButton.TabIndex = 0;
            this.refreshPortableButton.Text = "Refresh";
            this.refreshPortableButton.UseVisualStyleBackColor = true;
            this.refreshPortableButton.Click += new System.EventHandler(this.RefreshPortableButton_Click);
            // 
            // installedPortableContentPanel
            // 
            this.installedPortableContentPanel.AutoScroll = true;
            this.installedPortableContentPanel.Location = new System.Drawing.Point(0, 0);
            this.installedPortableContentPanel.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.installedPortableContentPanel.Name = "installedPortableContentPanel";
            this.installedPortableContentPanel.Size = new System.Drawing.Size(981, 482);
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
            this.setPortableAppFolderPage.Location = new System.Drawing.Point(0, 42);
            this.setPortableAppFolderPage.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.setPortableAppFolderPage.Name = "setPortableAppFolderPage";
            this.setPortableAppFolderPage.Size = new System.Drawing.Size(981, 534);
            this.setPortableAppFolderPage.TabIndex = 13;
            // 
            // paFolderNotWriteableLabel2
            // 
            this.paFolderNotWriteableLabel2.AutoSize = true;
            this.paFolderNotWriteableLabel2.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.paFolderNotWriteableLabel2.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(186)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.paFolderNotWriteableLabel2.Location = new System.Drawing.Point(115, 259);
            this.paFolderNotWriteableLabel2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.paFolderNotWriteableLabel2.Name = "paFolderNotWriteableLabel2";
            this.paFolderNotWriteableLabel2.Size = new System.Drawing.Size(606, 18);
            this.paFolderNotWriteableLabel2.TabIndex = 7;
            this.paFolderNotWriteableLabel2.Text = "This folder is not writeable by the current user, please choose a different folde" +
    "r.";
            this.paFolderNotWriteableLabel2.Visible = false;
            // 
            // browseButton2
            // 
            this.browseButton2.Location = new System.Drawing.Point(784, 229);
            this.browseButton2.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.browseButton2.Name = "browseButton2";
            this.browseButton2.Size = new System.Drawing.Size(94, 29);
            this.browseButton2.TabIndex = 6;
            this.browseButton2.Text = "Browse";
            this.browseButton2.UseVisualStyleBackColor = true;
            this.browseButton2.Click += new System.EventHandler(this.BrowseButton2_Click);
            // 
            // locationBox2
            // 
            this.locationBox2.Location = new System.Drawing.Point(111, 231);
            this.locationBox2.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.locationBox2.Name = "locationBox2";
            this.locationBox2.Size = new System.Drawing.Size(649, 22);
            this.locationBox2.TabIndex = 5;
            this.locationBox2.TextChanged += new System.EventHandler(this.LocationBox2_TextChanged);
            // 
            // instructionLabel
            // 
            this.instructionLabel.AutoSize = true;
            this.instructionLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.instructionLabel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(186)))), ((int)(((byte)(0)))));
            this.instructionLabel.Location = new System.Drawing.Point(126, 50);
            this.instructionLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.instructionLabel.Name = "instructionLabel";
            this.instructionLabel.Size = new System.Drawing.Size(692, 54);
            this.instructionLabel.TabIndex = 3;
            this.instructionLabel.Text = resources.GetString("instructionLabel.Text");
            // 
            // setPortableAppFolderButton
            // 
            this.setPortableAppFolderButton.Location = new System.Drawing.Point(444, 491);
            this.setPortableAppFolderButton.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.setPortableAppFolderButton.Name = "setPortableAppFolderButton";
            this.setPortableAppFolderButton.Size = new System.Drawing.Size(92, 29);
            this.setPortableAppFolderButton.TabIndex = 4;
            this.setPortableAppFolderButton.Text = "OK";
            this.setPortableAppFolderButton.UseVisualStyleBackColor = true;
            this.setPortableAppFolderButton.Click += new System.EventHandler(this.SetPortableAppFolderButton_Click);
            // 
            // settingsPage
            // 
            this.settingsPage.BackColor = System.Drawing.SystemColors.ButtonHighlight;
            this.settingsPage.Controls.Add(this.autoStartCheckBox);
            this.settingsPage.Controls.Add(this.minimizeToTrayCheckBox);
            this.settingsPage.Controls.Add(this.dataFolderLabel);
            this.settingsPage.Controls.Add(this.dataLocationBox);
            this.settingsPage.Controls.Add(this.dataBrowseButton);
            this.settingsPage.Controls.Add(this.openDataDirButton);
            this.settingsPage.Controls.Add(this.dataFolderNotWriteableLabel);
            this.settingsPage.Controls.Add(this.paFolderLocationLabel);
            this.settingsPage.Controls.Add(this.paLocationBox);
            this.settingsPage.Controls.Add(this.paBrowseButton);
            this.settingsPage.Controls.Add(this.openPAFolderButton);
            this.settingsPage.Controls.Add(this.paFolderNotWriteableLabel);
            this.settingsPage.Controls.Add(this.defenitionsGroupBox);
            this.settingsPage.Controls.Add(this.saveButton);
            this.settingsPage.Controls.Add(this.resetButton);
            this.settingsPage.Location = new System.Drawing.Point(0, 42);
            this.settingsPage.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.settingsPage.Name = "settingsPage";
            this.settingsPage.Size = new System.Drawing.Size(981, 534);
            this.settingsPage.TabIndex = 14;
            // 
            // autoStartCheckBox
            // 
            this.autoStartCheckBox.AutoSize = true;
            this.autoStartCheckBox.Location = new System.Drawing.Point(15, 8);
            this.autoStartCheckBox.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.autoStartCheckBox.Name = "autoStartCheckBox";
            this.autoStartCheckBox.Size = new System.Drawing.Size(314, 21);
            this.autoStartCheckBox.TabIndex = 4;
            this.autoStartCheckBox.Text = "Auto-start Slim Updater as a system tray icon";
            this.autoStartCheckBox.UseVisualStyleBackColor = true;
            // 
            // minimizeToTrayCheckBox
            // 
            this.minimizeToTrayCheckBox.AutoSize = true;
            this.minimizeToTrayCheckBox.Location = new System.Drawing.Point(15, 38);
            this.minimizeToTrayCheckBox.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.minimizeToTrayCheckBox.Name = "minimizeToTrayCheckBox";
            this.minimizeToTrayCheckBox.Size = new System.Drawing.Size(418, 21);
            this.minimizeToTrayCheckBox.TabIndex = 5;
            this.minimizeToTrayCheckBox.Text = "Keep running as a system tray icon when I close Slim Updater";
            this.minimizeToTrayCheckBox.UseVisualStyleBackColor = true;
            // 
            // dataFolderLabel
            // 
            this.dataFolderLabel.AutoSize = true;
            this.dataFolderLabel.Location = new System.Drawing.Point(11, 74);
            this.dataFolderLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.dataFolderLabel.Name = "dataFolderLabel";
            this.dataFolderLabel.Size = new System.Drawing.Size(431, 17);
            this.dataFolderLabel.TabIndex = 13;
            this.dataFolderLabel.Text = "Data Folder Location (for storing application installers and settings)";
            // 
            // dataLocationBox
            // 
            this.dataLocationBox.Location = new System.Drawing.Point(15, 95);
            this.dataLocationBox.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.dataLocationBox.Name = "dataLocationBox";
            this.dataLocationBox.Size = new System.Drawing.Size(722, 22);
            this.dataLocationBox.TabIndex = 15;
            // 
            // dataBrowseButton
            // 
            this.dataBrowseButton.Location = new System.Drawing.Point(760, 92);
            this.dataBrowseButton.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.dataBrowseButton.Name = "dataBrowseButton";
            this.dataBrowseButton.Size = new System.Drawing.Size(94, 29);
            this.dataBrowseButton.TabIndex = 14;
            this.dataBrowseButton.Text = "Browse";
            this.dataBrowseButton.UseVisualStyleBackColor = true;
            this.dataBrowseButton.Click += new System.EventHandler(this.DataBrowseButton_Click);
            // 
            // openDataDirButton
            // 
            this.openDataDirButton.Location = new System.Drawing.Point(871, 92);
            this.openDataDirButton.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.openDataDirButton.Name = "openDataDirButton";
            this.openDataDirButton.Size = new System.Drawing.Size(94, 29);
            this.openDataDirButton.TabIndex = 17;
            this.openDataDirButton.Text = "Open Folder";
            this.openDataDirButton.UseVisualStyleBackColor = true;
            this.openDataDirButton.Click += new System.EventHandler(this.OpenDataDirButton_Click);
            // 
            // dataFolderNotWriteableLabel
            // 
            this.dataFolderNotWriteableLabel.AutoSize = true;
            this.dataFolderNotWriteableLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dataFolderNotWriteableLabel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(186)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.dataFolderNotWriteableLabel.Location = new System.Drawing.Point(59, 121);
            this.dataFolderNotWriteableLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.dataFolderNotWriteableLabel.Name = "dataFolderNotWriteableLabel";
            this.dataFolderNotWriteableLabel.Size = new System.Drawing.Size(606, 18);
            this.dataFolderNotWriteableLabel.TabIndex = 16;
            this.dataFolderNotWriteableLabel.Text = "This folder is not writeable by the current user, please choose a different folde" +
    "r.";
            this.dataFolderNotWriteableLabel.Visible = false;
            // 
            // paFolderLocationLabel
            // 
            this.paFolderLocationLabel.AutoSize = true;
            this.paFolderLocationLabel.Location = new System.Drawing.Point(11, 144);
            this.paFolderLocationLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.paFolderLocationLabel.Name = "paFolderLocationLabel";
            this.paFolderLocationLabel.Size = new System.Drawing.Size(203, 17);
            this.paFolderLocationLabel.TabIndex = 9;
            this.paFolderLocationLabel.Text = "Portable Apps Folder Location:";
            // 
            // paLocationBox
            // 
            this.paLocationBox.Location = new System.Drawing.Point(15, 165);
            this.paLocationBox.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.paLocationBox.Name = "paLocationBox";
            this.paLocationBox.Size = new System.Drawing.Size(722, 22);
            this.paLocationBox.TabIndex = 11;
            // 
            // paBrowseButton
            // 
            this.paBrowseButton.Location = new System.Drawing.Point(760, 165);
            this.paBrowseButton.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.paBrowseButton.Name = "paBrowseButton";
            this.paBrowseButton.Size = new System.Drawing.Size(94, 29);
            this.paBrowseButton.TabIndex = 10;
            this.paBrowseButton.Text = "Browse";
            this.paBrowseButton.UseVisualStyleBackColor = true;
            this.paBrowseButton.Click += new System.EventHandler(this.PA_BrowseButton_Click);
            // 
            // openPAFolderButton
            // 
            this.openPAFolderButton.Location = new System.Drawing.Point(871, 165);
            this.openPAFolderButton.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.openPAFolderButton.Name = "openPAFolderButton";
            this.openPAFolderButton.Size = new System.Drawing.Size(94, 29);
            this.openPAFolderButton.TabIndex = 18;
            this.openPAFolderButton.Text = "Open Folder";
            this.openPAFolderButton.UseVisualStyleBackColor = true;
            this.openPAFolderButton.Click += new System.EventHandler(this.OpenPAFolderButton_Click);
            // 
            // paFolderNotWriteableLabel
            // 
            this.paFolderNotWriteableLabel.AutoSize = true;
            this.paFolderNotWriteableLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.paFolderNotWriteableLabel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(186)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.paFolderNotWriteableLabel.Location = new System.Drawing.Point(59, 192);
            this.paFolderNotWriteableLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.paFolderNotWriteableLabel.Name = "paFolderNotWriteableLabel";
            this.paFolderNotWriteableLabel.Size = new System.Drawing.Size(606, 18);
            this.paFolderNotWriteableLabel.TabIndex = 12;
            this.paFolderNotWriteableLabel.Text = "This folder is not writeable by the current user, please choose a different folde" +
    "r.";
            this.paFolderNotWriteableLabel.Visible = false;
            // 
            // defenitionsGroupBox
            // 
            this.defenitionsGroupBox.Controls.Add(this.warningLabel);
            this.defenitionsGroupBox.Controls.Add(this.customURLTextBox);
            this.defenitionsGroupBox.Controls.Add(this.customDefenRadioBtn);
            this.defenitionsGroupBox.Controls.Add(this.officialDefenRadioBtn);
            this.defenitionsGroupBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.defenitionsGroupBox.Location = new System.Drawing.Point(15, 212);
            this.defenitionsGroupBox.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.defenitionsGroupBox.Name = "defenitionsGroupBox";
            this.defenitionsGroupBox.Padding = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.defenitionsGroupBox.Size = new System.Drawing.Size(951, 135);
            this.defenitionsGroupBox.TabIndex = 1;
            this.defenitionsGroupBox.TabStop = false;
            this.defenitionsGroupBox.Text = "Defenitions";
            // 
            // warningLabel
            // 
            this.warningLabel.AutoSize = true;
            this.warningLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.warningLabel.Location = new System.Drawing.Point(8, 110);
            this.warningLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.warningLabel.Name = "warningLabel";
            this.warningLabel.Size = new System.Drawing.Size(405, 17);
            this.warningLabel.TabIndex = 3;
            this.warningLabel.Text = "Make sure to only use custom Defenitions from trusted sources";
            // 
            // customURLTextBox
            // 
            this.customURLTextBox.Enabled = false;
            this.customURLTextBox.Location = new System.Drawing.Point(8, 81);
            this.customURLTextBox.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.customURLTextBox.Name = "customURLTextBox";
            this.customURLTextBox.Size = new System.Drawing.Size(362, 23);
            this.customURLTextBox.TabIndex = 2;
            // 
            // customDefenRadioBtn
            // 
            this.customDefenRadioBtn.AutoSize = true;
            this.customDefenRadioBtn.Location = new System.Drawing.Point(8, 52);
            this.customDefenRadioBtn.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.customDefenRadioBtn.Name = "customDefenRadioBtn";
            this.customDefenRadioBtn.Size = new System.Drawing.Size(240, 21);
            this.customDefenRadioBtn.TabIndex = 1;
            this.customDefenRadioBtn.Text = "Use the following Defenition URL:";
            this.customDefenRadioBtn.UseVisualStyleBackColor = true;
            this.customDefenRadioBtn.Click += new System.EventHandler(this.CustomDefenRadioBtn_Click);
            // 
            // officialDefenRadioBtn
            // 
            this.officialDefenRadioBtn.AutoSize = true;
            this.officialDefenRadioBtn.Checked = true;
            this.officialDefenRadioBtn.Location = new System.Drawing.Point(8, 24);
            this.officialDefenRadioBtn.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.officialDefenRadioBtn.Name = "officialDefenRadioBtn";
            this.officialDefenRadioBtn.Size = new System.Drawing.Size(386, 21);
            this.officialDefenRadioBtn.TabIndex = 0;
            this.officialDefenRadioBtn.TabStop = true;
            this.officialDefenRadioBtn.Text = "Use the official Slim Updater Defenitions (recommended)";
            this.officialDefenRadioBtn.UseVisualStyleBackColor = true;
            this.officialDefenRadioBtn.Click += new System.EventHandler(this.OfficialDefenRadioBtn_Click);
            // 
            // saveButton
            // 
            this.saveButton.Location = new System.Drawing.Point(389, 491);
            this.saveButton.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.saveButton.Name = "saveButton";
            this.saveButton.Size = new System.Drawing.Size(94, 29);
            this.saveButton.TabIndex = 2;
            this.saveButton.Text = "Save";
            this.saveButton.UseVisualStyleBackColor = true;
            this.saveButton.Click += new System.EventHandler(this.SaveButton_Click);
            // 
            // resetButton
            // 
            this.resetButton.Location = new System.Drawing.Point(499, 491);
            this.resetButton.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.resetButton.Name = "resetButton";
            this.resetButton.Size = new System.Drawing.Size(94, 29);
            this.resetButton.TabIndex = 19;
            this.resetButton.Text = "Reset";
            this.resetButton.UseVisualStyleBackColor = true;
            this.resetButton.Click += new System.EventHandler(this.ResetButton_Click);
            // 
            // getPortableAppsPage
            // 
            this.getPortableAppsPage.BackColor = System.Drawing.SystemColors.ButtonHighlight;
            this.getPortableAppsPage.Controls.Add(this.selectAllPortableCheckBox);
            this.getPortableAppsPage.Controls.Add(this.portableStatusLabel);
            this.getPortableAppsPage.Controls.Add(this.downloadPortableButton);
            this.getPortableAppsPage.Controls.Add(this.refreshPortableButton2);
            this.getPortableAppsPage.Controls.Add(this.getPortableContentPanel);
            this.getPortableAppsPage.Location = new System.Drawing.Point(0, 44);
            this.getPortableAppsPage.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.getPortableAppsPage.Name = "getPortableAppsPage";
            this.getPortableAppsPage.Size = new System.Drawing.Size(981, 534);
            this.getPortableAppsPage.TabIndex = 15;
            // 
            // selectAllPortableCheckBox
            // 
            this.selectAllPortableCheckBox.AutoSize = true;
            this.selectAllPortableCheckBox.Location = new System.Drawing.Point(8, 2);
            this.selectAllPortableCheckBox.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.selectAllPortableCheckBox.Name = "selectAllPortableCheckBox";
            this.selectAllPortableCheckBox.Size = new System.Drawing.Size(88, 21);
            this.selectAllPortableCheckBox.TabIndex = 1;
            this.selectAllPortableCheckBox.Text = "Select All";
            this.selectAllPortableCheckBox.UseVisualStyleBackColor = true;
            this.selectAllPortableCheckBox.Click += new System.EventHandler(this.SelectAllPortableCheckBox_CheckedChanged);
            // 
            // portableStatusLabel
            // 
            this.portableStatusLabel.AutoSize = true;
            this.portableStatusLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold);
            this.portableStatusLabel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(186)))), ((int)(((byte)(0)))));
            this.portableStatusLabel.Location = new System.Drawing.Point(312, 2);
            this.portableStatusLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.portableStatusLabel.Name = "portableStatusLabel";
            this.portableStatusLabel.Size = new System.Drawing.Size(333, 20);
            this.portableStatusLabel.TabIndex = 3;
            this.portableStatusLabel.Text = "Succesfully installed all Portable Apps";
            this.portableStatusLabel.Visible = false;
            // 
            // downloadPortableButton
            // 
            this.downloadPortableButton.Location = new System.Drawing.Point(482, 492);
            this.downloadPortableButton.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.downloadPortableButton.Name = "downloadPortableButton";
            this.downloadPortableButton.Size = new System.Drawing.Size(146, 29);
            this.downloadPortableButton.TabIndex = 2;
            this.downloadPortableButton.Text = "Download Selected";
            this.downloadPortableButton.UseVisualStyleBackColor = true;
            this.downloadPortableButton.Click += new System.EventHandler(this.DownloadPortableButton_Click);
            // 
            // refreshPortableButton2
            // 
            this.refreshPortableButton2.Location = new System.Drawing.Point(351, 492);
            this.refreshPortableButton2.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.refreshPortableButton2.Name = "refreshPortableButton2";
            this.refreshPortableButton2.Size = new System.Drawing.Size(94, 29);
            this.refreshPortableButton2.TabIndex = 0;
            this.refreshPortableButton2.Text = "Refresh";
            this.refreshPortableButton2.UseVisualStyleBackColor = true;
            this.refreshPortableButton2.Click += new System.EventHandler(this.RefreshPortableButton2_Click);
            // 
            // getPortableContentPanel
            // 
            this.getPortableContentPanel.AutoScroll = true;
            this.getPortableContentPanel.Location = new System.Drawing.Point(0, 25);
            this.getPortableContentPanel.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.getPortableContentPanel.Name = "getPortableContentPanel";
            this.getPortableContentPanel.Size = new System.Drawing.Size(981, 456);
            this.getPortableContentPanel.TabIndex = 0;
            // 
            // trayIcon
            // 
            this.trayIcon.ContextMenuStrip = this.trayIconContextMenu;
            this.trayIcon.Icon = ((System.Drawing.Icon)(resources.GetObject("trayIcon.Icon")));
            this.trayIcon.Text = "Slim Updater";
            this.trayIcon.MouseClick += new System.Windows.Forms.MouseEventHandler(this.TrayIcon_MouseClick);
            // 
            // trayIconContextMenu
            // 
            this.trayIconContextMenu.BackColor = System.Drawing.Color.White;
            this.trayIconContextMenu.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.trayIconContextMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.checkUpdatesTrayIconMenuItem,
            this.openTrayIconMenuItem,
            this.settingsTrayIconMenuItem,
            this.exitTrayIconMenuItem});
            this.trayIconContextMenu.Name = "trayIconContextMenu";
            this.trayIconContextMenu.RenderMode = System.Windows.Forms.ToolStripRenderMode.Professional;
            this.trayIconContextMenu.Size = new System.Drawing.Size(198, 100);
            // 
            // checkUpdatesTrayIconMenuItem
            // 
            this.checkUpdatesTrayIconMenuItem.BackColor = System.Drawing.SystemColors.ButtonHighlight;
            this.checkUpdatesTrayIconMenuItem.Name = "checkUpdatesTrayIconMenuItem";
            this.checkUpdatesTrayIconMenuItem.Size = new System.Drawing.Size(197, 24);
            this.checkUpdatesTrayIconMenuItem.Text = "Check for &updates";
            this.checkUpdatesTrayIconMenuItem.Click += new System.EventHandler(this.CheckUpdatesTrayIconMenuItem_Click);
            // 
            // openTrayIconMenuItem
            // 
            this.openTrayIconMenuItem.BackColor = System.Drawing.SystemColors.ButtonHighlight;
            this.openTrayIconMenuItem.Name = "openTrayIconMenuItem";
            this.openTrayIconMenuItem.Size = new System.Drawing.Size(197, 24);
            this.openTrayIconMenuItem.Text = "&Open ";
            this.openTrayIconMenuItem.Click += new System.EventHandler(this.OpenTrayIconMenuItem_Click);
            // 
            // settingsTrayIconMenuItem
            // 
            this.settingsTrayIconMenuItem.BackColor = System.Drawing.SystemColors.ButtonHighlight;
            this.settingsTrayIconMenuItem.Name = "settingsTrayIconMenuItem";
            this.settingsTrayIconMenuItem.Size = new System.Drawing.Size(197, 24);
            this.settingsTrayIconMenuItem.Text = "&Settings";
            this.settingsTrayIconMenuItem.Click += new System.EventHandler(this.SettingsTrayIconMenuItem_Click);
            // 
            // exitTrayIconMenuItem
            // 
            this.exitTrayIconMenuItem.BackColor = System.Drawing.SystemColors.ButtonHighlight;
            this.exitTrayIconMenuItem.Name = "exitTrayIconMenuItem";
            this.exitTrayIconMenuItem.Size = new System.Drawing.Size(197, 24);
            this.exitTrayIconMenuItem.Text = "E&xit";
            this.exitTrayIconMenuItem.Click += new System.EventHandler(this.ExitTrayIconMenuItem_Click);
            // 
            // logPage
            // 
            this.logPage.BackColor = System.Drawing.SystemColors.ButtonHighlight;
            this.logPage.Controls.Add(this.logTextBox);
            this.logPage.Location = new System.Drawing.Point(0, 42);
            this.logPage.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.logPage.Name = "logPage";
            this.logPage.Size = new System.Drawing.Size(981, 534);
            this.logPage.TabIndex = 16;
            // 
            // logTextBox
            // 
            this.logTextBox.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.logTextBox.Location = new System.Drawing.Point(-1, 0);
            this.logTextBox.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.logTextBox.Name = "logTextBox";
            this.logTextBox.ReadOnly = true;
            this.logTextBox.Size = new System.Drawing.Size(982, 532);
            this.logTextBox.TabIndex = 0;
            this.logTextBox.Text = "";
            // 
            // titleButtonLeft
            // 
            this.titleButtonLeft.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.titleButtonLeft.ArrowLeft = false;
            this.titleButtonLeft.ArrowRight = false;
            this.titleButtonLeft.AutoSize = true;
            this.titleButtonLeft.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.titleButtonLeft.BackColor = System.Drawing.Color.Transparent;
            this.titleButtonLeft.Location = new System.Drawing.Point(6, 4);
            this.titleButtonLeft.Margin = new System.Windows.Forms.Padding(5);
            this.titleButtonLeft.MinimumSize = new System.Drawing.Size(0, 41);
            this.titleButtonLeft.Name = "titleButtonLeft";
            this.titleButtonLeft.Size = new System.Drawing.Size(91, 41);
            this.titleButtonLeft.TabIndex = 2;
            this.titleButtonLeft.Click += new System.EventHandler(this.TitleButtonLeft_Click);
            // 
            // titleButtonRight
            // 
            this.titleButtonRight.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.titleButtonRight.ArrowLeft = false;
            this.titleButtonRight.ArrowRight = true;
            this.titleButtonRight.AutoSize = true;
            this.titleButtonRight.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.titleButtonRight.BackColor = System.Drawing.Color.White;
            this.titleButtonRight.Location = new System.Drawing.Point(721, 4);
            this.titleButtonRight.Margin = new System.Windows.Forms.Padding(5);
            this.titleButtonRight.MinimumSize = new System.Drawing.Size(0, 41);
            this.titleButtonRight.Name = "titleButtonRight";
            this.titleButtonRight.Size = new System.Drawing.Size(268, 41);
            this.titleButtonRight.TabIndex = 3;
            this.titleButtonRight.Visible = false;
            this.titleButtonRight.Click += new System.EventHandler(this.TitleButtonRight_Click);
            // 
            // updaterTile
            // 
            this.updaterTile.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(186)))), ((int)(((byte)(0)))));
            this.updaterTile.Image = global::SlimUpdater.Properties.Resources.Updates_Icon;
            this.updaterTile.Location = new System.Drawing.Point(81, 58);
            this.updaterTile.Margin = new System.Windows.Forms.Padding(5);
            this.updaterTile.Name = "updaterTile";
            this.updaterTile.Size = new System.Drawing.Size(375, 188);
            this.updaterTile.TabIndex = 5;
            this.updaterTile.Text = "No updates available";
            this.updaterTile.Click += new System.EventHandler(this.UpdaterTile_Click);
            // 
            // getNewAppsTile
            // 
            this.getNewAppsTile.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(186)))), ((int)(((byte)(0)))));
            this.getNewAppsTile.Image = global::SlimUpdater.Properties.Resources.GetNewApps_Icon;
            this.getNewAppsTile.Location = new System.Drawing.Point(524, 58);
            this.getNewAppsTile.Margin = new System.Windows.Forms.Padding(5);
            this.getNewAppsTile.Name = "getNewAppsTile";
            this.getNewAppsTile.Size = new System.Drawing.Size(375, 188);
            this.getNewAppsTile.TabIndex = 7;
            this.getNewAppsTile.Text = "Get New Applications";
            this.getNewAppsTile.Click += new System.EventHandler(this.GetNewAppsTile_Click);
            // 
            // portableAppsTile
            // 
            this.portableAppsTile.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(186)))), ((int)(((byte)(0)))));
            this.portableAppsTile.Image = global::SlimUpdater.Properties.Resources.PortableApps_Icon;
            this.portableAppsTile.Location = new System.Drawing.Point(81, 285);
            this.portableAppsTile.Margin = new System.Windows.Forms.Padding(5);
            this.portableAppsTile.Name = "portableAppsTile";
            this.portableAppsTile.Size = new System.Drawing.Size(375, 188);
            this.portableAppsTile.TabIndex = 6;
            this.portableAppsTile.Text = "Portable Apps";
            this.portableAppsTile.Click += new System.EventHandler(this.PortableAppsTile_Click);
            // 
            // settingsTile
            // 
            this.settingsTile.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(186)))), ((int)(((byte)(0)))));
            this.settingsTile.Image = global::SlimUpdater.Properties.Resources.Settings_Icon;
            this.settingsTile.Location = new System.Drawing.Point(524, 285);
            this.settingsTile.Margin = new System.Windows.Forms.Padding(5);
            this.settingsTile.Name = "settingsTile";
            this.settingsTile.Size = new System.Drawing.Size(375, 188);
            this.settingsTile.TabIndex = 8;
            this.settingsTile.Text = "Settings";
            this.settingsTile.Click += new System.EventHandler(this.SettingsTile_Click);
            // 
            // MainWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(120F, 120F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.BackColor = System.Drawing.SystemColors.ButtonHighlight;
            this.ClientSize = new System.Drawing.Size(980, 576);
            this.Controls.Add(this.topBar);
            this.Controls.Add(this.startPage);
            this.Controls.Add(this.updatePage);
            this.Controls.Add(this.getNewAppsPage);
            this.Controls.Add(this.installedPortableAppsPage);
            this.Controls.Add(this.getPortableAppsPage);
            this.Controls.Add(this.setPortableAppFolderPage);
            this.Controls.Add(this.detailsPage);
            this.Controls.Add(this.settingsPage);
            this.Controls.Add(this.logPage);
            this.Controls.Add(this.aboutPage);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.MaximizeBox = false;
            this.Name = "MainWindow";
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Slim Updater";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainWindow_FormClosing);
            this.Load += new System.EventHandler(this.MainWindow_Load);
            this.Shown += new System.EventHandler(this.MainWindow_Shown);
            this.startPage.ResumeLayout(false);
            this.startPage.PerformLayout();
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
            this.setPortableAppFolderPage.ResumeLayout(false);
            this.setPortableAppFolderPage.PerformLayout();
            this.settingsPage.ResumeLayout(false);
            this.settingsPage.PerformLayout();
            this.defenitionsGroupBox.ResumeLayout(false);
            this.defenitionsGroupBox.PerformLayout();
            this.getPortableAppsPage.ResumeLayout(false);
            this.getPortableAppsPage.PerformLayout();
            this.trayIconContextMenu.ResumeLayout(false);
            this.logPage.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Panel startPage;
        private System.Windows.Forms.Panel topBar;
        private System.Windows.Forms.Label aboutButton;
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
        private System.Windows.Forms.Label updatesStatusLabel;
        private System.Windows.Forms.Panel getNewAppsPage;
        private System.Windows.Forms.Button installAppsButton;
        private System.Windows.Forms.Button refreshAppsButton;
        private System.Windows.Forms.CheckBox selectAllAppsCheckBox;
        private System.Windows.Forms.Panel getNewAppsContentPanel;
        private System.Windows.Forms.Panel installedPortableAppsPage;
        private System.Windows.Forms.Button refreshPortableButton;
        private System.Windows.Forms.Panel installedPortableContentPanel;
        private System.Windows.Forms.Panel setPortableAppFolderPage;
        private System.Windows.Forms.Button browseButton2;
        private System.Windows.Forms.TextBox locationBox2;
        private System.Windows.Forms.Label instructionLabel;
        private System.Windows.Forms.Button setPortableAppFolderButton;
        private System.Windows.Forms.Label paFolderNotWriteableLabel2;
        private System.Windows.Forms.Panel settingsPage;
        private System.Windows.Forms.Button saveButton;
        private System.Windows.Forms.GroupBox defenitionsGroupBox;
        private System.Windows.Forms.Label warningLabel;
        private System.Windows.Forms.TextBox customURLTextBox;
        private System.Windows.Forms.RadioButton customDefenRadioBtn;
        private System.Windows.Forms.RadioButton officialDefenRadioBtn;
        private System.Windows.Forms.CheckBox minimizeToTrayCheckBox;
        private System.Windows.Forms.CheckBox autoStartCheckBox;
        private System.Windows.Forms.Button paBrowseButton;
        private System.Windows.Forms.Label paFolderLocationLabel;
        private System.Windows.Forms.TextBox paLocationBox;
        private System.Windows.Forms.Label paFolderNotWriteableLabel;
        private TitleButton titleButtonLeft;
        private TitleButton titleButtonRight;
        private System.Windows.Forms.Panel getPortableAppsPage;
        private System.Windows.Forms.CheckBox selectAllPortableCheckBox;
        private System.Windows.Forms.Label portableStatusLabel;
        private System.Windows.Forms.Button downloadPortableButton;
        private System.Windows.Forms.Button refreshPortableButton2;
        private System.Windows.Forms.Panel getPortableContentPanel;
        private System.Windows.Forms.ContextMenuStrip trayIconContextMenu;
        private System.Windows.Forms.ToolStripMenuItem openTrayIconMenuItem;
        private System.Windows.Forms.ToolStripMenuItem settingsTrayIconMenuItem;
        private System.Windows.Forms.ToolStripMenuItem exitTrayIconMenuItem;
        private System.Windows.Forms.Label offlineLabel;
        private System.Windows.Forms.LinkLabel offlineRetryLink;
        private System.Windows.Forms.ToolStripMenuItem checkUpdatesTrayIconMenuItem;
        private System.Windows.Forms.Panel logPage;
        private System.Windows.Forms.RichTextBox logTextBox;
        private System.Windows.Forms.Label newAppsStatusLabel;
        private System.Windows.Forms.Label logButton;
        public System.Windows.Forms.NotifyIcon trayIcon;
        private System.Windows.Forms.Label dataFolderLabel;
        private System.Windows.Forms.TextBox dataLocationBox;
        private System.Windows.Forms.Button dataBrowseButton;
        private System.Windows.Forms.Label dataFolderNotWriteableLabel;
        private System.Windows.Forms.Button openDataDirButton;
        private System.Windows.Forms.Button openPAFolderButton;
        private System.Windows.Forms.Button resetButton;
    }
}

