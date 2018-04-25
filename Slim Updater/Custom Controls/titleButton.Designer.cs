namespace SlimUpdater
{
    partial class TitleButton
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
            this.titleLabel = new System.Windows.Forms.Label();
            this.backArrowRight = new System.Windows.Forms.PictureBox();
            this.backArrowLeft = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.backArrowRight)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.backArrowLeft)).BeginInit();
            this.SuspendLayout();
            // 
            // titleLabel
            // 
            this.titleLabel.AutoSize = true;
            this.titleLabel.BackColor = System.Drawing.Color.Transparent;
            this.titleLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.titleLabel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(186)))), ((int)(((byte)(0)))));
            this.titleLabel.Location = new System.Drawing.Point(32, 4);
            this.titleLabel.Name = "titleLabel";
            this.titleLabel.Size = new System.Drawing.Size(66, 24);
            this.titleLabel.TabIndex = 1;
            this.titleLabel.Text = "Home";
            this.titleLabel.MouseEnter += new System.EventHandler(this.TitleLabel_MouseEnter);
            this.titleLabel.MouseLeave += new System.EventHandler(this.TitleLabel_MouseLeave);
            // 
            // backArrowRight
            // 
            this.backArrowRight.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.backArrowRight.Image = global::SlimUpdater.Properties.Resources.ArrowGreenRight;
            this.backArrowRight.Location = new System.Drawing.Point(95, 5);
            this.backArrowRight.Name = "backArrowRight";
            this.backArrowRight.Size = new System.Drawing.Size(32, 24);
            this.backArrowRight.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.backArrowRight.TabIndex = 4;
            this.backArrowRight.TabStop = false;
            // 
            // backArrowLeft
            // 
            this.backArrowLeft.Image = global::SlimUpdater.Properties.Resources.ArrowGreen;
            this.backArrowLeft.Location = new System.Drawing.Point(3, 4);
            this.backArrowLeft.Name = "backArrowLeft";
            this.backArrowLeft.Size = new System.Drawing.Size(32, 24);
            this.backArrowLeft.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.backArrowLeft.TabIndex = 3;
            this.backArrowLeft.TabStop = false;
            this.backArrowLeft.MouseEnter += new System.EventHandler(this.BackArrow_MouseEnter);
            this.backArrowLeft.MouseLeave += new System.EventHandler(this.BackArrow_MouseLeave);
            // 
            // TitleButton
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.BackColor = System.Drawing.Color.White;
            this.Controls.Add(this.backArrowRight);
            this.Controls.Add(this.backArrowLeft);
            this.Controls.Add(this.titleLabel);
            this.MinimumSize = new System.Drawing.Size(0, 33);
            this.Name = "TitleButton";
            this.Size = new System.Drawing.Size(130, 33);
            this.Click += new System.EventHandler(this.TitleButton_Click);
            this.MouseEnter += new System.EventHandler(this.TitleButton_MouseEnter);
            this.MouseLeave += new System.EventHandler(this.TitleButton_MouseLeave);
            ((System.ComponentModel.ISupportInitialize)(this.backArrowRight)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.backArrowLeft)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label titleLabel;
        private System.Windows.Forms.PictureBox backArrowLeft;
        private System.Windows.Forms.PictureBox backArrowRight;
    }
}
