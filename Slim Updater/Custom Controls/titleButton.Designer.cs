namespace Slim_Updater
{
    partial class titleButton
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
            this.backArrow = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.backArrow)).BeginInit();
            this.SuspendLayout();
            // 
            // titleLabel
            // 
            this.titleLabel.AutoSize = true;
            this.titleLabel.BackColor = System.Drawing.Color.Transparent;
            this.titleLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.titleLabel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(186)))), ((int)(((byte)(0)))));
            this.titleLabel.Location = new System.Drawing.Point(30, 4);
            this.titleLabel.Name = "titleLabel";
            this.titleLabel.Size = new System.Drawing.Size(66, 24);
            this.titleLabel.TabIndex = 1;
            this.titleLabel.Text = "Home";
            this.titleLabel.MouseEnter += new System.EventHandler(this.titleLabel_MouseEnter);
            this.titleLabel.MouseLeave += new System.EventHandler(this.titleLabel_MouseLeave);
            // 
            // backArrow
            // 
            this.backArrow.Image = global::Slim_Updater.Properties.Resources.ArrowGreen;
            this.backArrow.Location = new System.Drawing.Point(3, 4);
            this.backArrow.Name = "backArrow";
            this.backArrow.Size = new System.Drawing.Size(32, 24);
            this.backArrow.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.backArrow.TabIndex = 3;
            this.backArrow.TabStop = false;
            this.backArrow.MouseEnter += new System.EventHandler(this.backArrow_MouseEnter);
            this.backArrow.MouseLeave += new System.EventHandler(this.backArrow_MouseLeave);
            // 
            // titleButton
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.Controls.Add(this.backArrow);
            this.Controls.Add(this.titleLabel);
            this.Name = "titleButton";
            this.Size = new System.Drawing.Size(95, 32);
            this.Click += new System.EventHandler(this.titleButton_Click);
            this.MouseEnter += new System.EventHandler(this.titleButton_MouseEnter);
            this.MouseLeave += new System.EventHandler(this.titleButton_MouseLeave);
            ((System.ComponentModel.ISupportInitialize)(this.backArrow)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label titleLabel;
        private System.Windows.Forms.PictureBox backArrow;
    }
}
