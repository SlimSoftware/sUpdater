namespace SlimUpdater
{
    partial class flatTile
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.statusLabel = new System.Windows.Forms.Label();
            this.tileIcon = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.tileIcon)).BeginInit();
            this.SuspendLayout();
            // 
            // statusLabel
            // 
            this.statusLabel.AutoSize = true;
            this.statusLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.statusLabel.ForeColor = System.Drawing.SystemColors.ButtonHighlight;
            this.statusLabel.Location = new System.Drawing.Point(70, 122);
            this.statusLabel.Name = "statusLabel";
            this.statusLabel.Size = new System.Drawing.Size(161, 17);
            this.statusLabel.TabIndex = 4;
            this.statusLabel.Text = "No updates available";
            this.statusLabel.SizeChanged += new System.EventHandler(this.statusLabel_SizeChanged);
            this.statusLabel.MouseEnter += new System.EventHandler(this.statusLabel_MouseEnter);
            this.statusLabel.MouseLeave += new System.EventHandler(this.statusLabel_MouseLeave);
            // 
            // tileIcon
            // 
            this.tileIcon.Image = global::SlimUpdater.Properties.Resources.Updates_Icon;
            this.tileIcon.Location = new System.Drawing.Point(118, 22);
            this.tileIcon.Name = "tileIcon";
            this.tileIcon.Size = new System.Drawing.Size(64, 64);
            this.tileIcon.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.tileIcon.TabIndex = 5;
            this.tileIcon.TabStop = false;
            this.tileIcon.MouseEnter += new System.EventHandler(this.tileIcon_MouseEnter);
            this.tileIcon.MouseLeave += new System.EventHandler(this.tileIcon_MouseLeave);
            // 
            // flatTile
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(186)))), ((int)(((byte)(0)))));
            this.Controls.Add(this.statusLabel);
            this.Controls.Add(this.tileIcon);
            this.Name = "flatTile";
            this.Size = new System.Drawing.Size(300, 150);
            this.MouseEnter += new System.EventHandler(this.flatTile_MouseEnter);
            this.MouseLeave += new System.EventHandler(this.flatTile_MouseLeave);
            ((System.ComponentModel.ISupportInitialize)(this.tileIcon)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label statusLabel;
        private System.Windows.Forms.PictureBox tileIcon;
    }
}
