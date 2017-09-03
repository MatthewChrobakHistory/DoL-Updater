namespace DivisionOfLifeUpdater
{
    partial class Menu
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing) {
            if (disposing && (components != null)) {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent() {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Menu));
            this.webNews = new System.Windows.Forms.WebBrowser();
            this.picBanner = new System.Windows.Forms.PictureBox();
            this.picTab = new System.Windows.Forms.PictureBox();
            this.picMinimize = new System.Windows.Forms.PictureBox();
            this.picClose = new System.Windows.Forms.PictureBox();
            this.prgCurrent = new System.Windows.Forms.ProgressBar();
            this.prgTotal = new System.Windows.Forms.ProgressBar();
            this.lblDescription = new System.Windows.Forms.Label();
            this.lblTotal = new System.Windows.Forms.Label();
            this.lblCurrent = new System.Windows.Forms.Label();
            this.lblPlay = new System.Windows.Forms.Label();
            this.lblRetry = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.picBanner)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picTab)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picMinimize)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picClose)).BeginInit();
            this.SuspendLayout();
            // 
            // webNews
            // 
            this.webNews.Location = new System.Drawing.Point(12, 52);
            this.webNews.MinimumSize = new System.Drawing.Size(20, 20);
            this.webNews.Name = "webNews";
            this.webNews.Size = new System.Drawing.Size(388, 365);
            this.webNews.TabIndex = 4;
            this.webNews.Url = new System.Uri("http://www.divisionoflife.com/launchernews.html", System.UriKind.Absolute);
            // 
            // picBanner
            // 
            this.picBanner.BackgroundImage = global::DivisionOfLifeUpdater.Properties.Resources.banner;
            this.picBanner.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.picBanner.Location = new System.Drawing.Point(417, 38);
            this.picBanner.Name = "picBanner";
            this.picBanner.Size = new System.Drawing.Size(371, 91);
            this.picBanner.TabIndex = 7;
            this.picBanner.TabStop = false;
            // 
            // picTab
            // 
            this.picTab.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.picTab.Image = ((System.Drawing.Image)(resources.GetObject("picTab.Image")));
            this.picTab.Location = new System.Drawing.Point(0, 0);
            this.picTab.Name = "picTab";
            this.picTab.Size = new System.Drawing.Size(800, 32);
            this.picTab.TabIndex = 8;
            this.picTab.TabStop = false;
            // 
            // picMinimize
            // 
            this.picMinimize.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.picMinimize.Image = global::DivisionOfLifeUpdater.Properties.Resources.Minimize;
            this.picMinimize.Location = new System.Drawing.Point(744, 5);
            this.picMinimize.Name = "picMinimize";
            this.picMinimize.Size = new System.Drawing.Size(20, 20);
            this.picMinimize.TabIndex = 10;
            this.picMinimize.TabStop = false;
            // 
            // picClose
            // 
            this.picClose.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.picClose.Image = global::DivisionOfLifeUpdater.Properties.Resources.Maximize;
            this.picClose.Location = new System.Drawing.Point(770, 5);
            this.picClose.Name = "picClose";
            this.picClose.Size = new System.Drawing.Size(20, 20);
            this.picClose.TabIndex = 9;
            this.picClose.TabStop = false;
            // 
            // prgCurrent
            // 
            this.prgCurrent.Location = new System.Drawing.Point(10, 553);
            this.prgCurrent.Name = "prgCurrent";
            this.prgCurrent.Size = new System.Drawing.Size(778, 35);
            this.prgCurrent.TabIndex = 11;
            // 
            // prgTotal
            // 
            this.prgTotal.Location = new System.Drawing.Point(10, 470);
            this.prgTotal.Name = "prgTotal";
            this.prgTotal.Size = new System.Drawing.Size(778, 35);
            this.prgTotal.TabIndex = 12;
            // 
            // lblDescription
            // 
            this.lblDescription.Font = new System.Drawing.Font("Bell MT", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblDescription.ForeColor = System.Drawing.SystemColors.Control;
            this.lblDescription.Location = new System.Drawing.Point(423, 132);
            this.lblDescription.Name = "lblDescription";
            this.lblDescription.Size = new System.Drawing.Size(365, 285);
            this.lblDescription.TabIndex = 13;
            // 
            // lblTotal
            // 
            this.lblTotal.AccessibleRole = System.Windows.Forms.AccessibleRole.None;
            this.lblTotal.Font = new System.Drawing.Font("Bell MT", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTotal.ForeColor = System.Drawing.SystemColors.Control;
            this.lblTotal.Location = new System.Drawing.Point(12, 444);
            this.lblTotal.Name = "lblTotal";
            this.lblTotal.Size = new System.Drawing.Size(776, 23);
            this.lblTotal.TabIndex = 14;
            this.lblTotal.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblCurrent
            // 
            this.lblCurrent.Font = new System.Drawing.Font("Bell MT", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblCurrent.ForeColor = System.Drawing.SystemColors.Control;
            this.lblCurrent.Location = new System.Drawing.Point(12, 527);
            this.lblCurrent.Name = "lblCurrent";
            this.lblCurrent.Size = new System.Drawing.Size(776, 23);
            this.lblCurrent.TabIndex = 15;
            this.lblCurrent.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblPlay
            // 
            this.lblPlay.AccessibleRole = System.Windows.Forms.AccessibleRole.None;
            this.lblPlay.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblPlay.Font = new System.Drawing.Font("Bell MT", 34F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Underline))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblPlay.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(210)))), ((int)(((byte)(202)))), ((int)(((byte)(166)))));
            this.lblPlay.Location = new System.Drawing.Point(271, 477);
            this.lblPlay.Name = "lblPlay";
            this.lblPlay.Size = new System.Drawing.Size(301, 105);
            this.lblPlay.TabIndex = 16;
            this.lblPlay.Text = "Play";
            this.lblPlay.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.lblPlay.Visible = false;
            // 
            // lblRetry
            // 
            this.lblRetry.AccessibleRole = System.Windows.Forms.AccessibleRole.None;
            this.lblRetry.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblRetry.Font = new System.Drawing.Font("Bell MT", 34F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Underline))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblRetry.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(210)))), ((int)(((byte)(202)))), ((int)(((byte)(166)))));
            this.lblRetry.Location = new System.Drawing.Point(271, 477);
            this.lblRetry.Name = "lblRetry";
            this.lblRetry.Size = new System.Drawing.Size(301, 105);
            this.lblRetry.TabIndex = 17;
            this.lblRetry.Text = "Retry";
            this.lblRetry.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.lblRetry.Visible = false;
            // 
            // Menu
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(18)))), ((int)(((byte)(14)))), ((int)(((byte)(12)))));
            this.ClientSize = new System.Drawing.Size(800, 600);
            this.Controls.Add(this.lblRetry);
            this.Controls.Add(this.lblPlay);
            this.Controls.Add(this.lblCurrent);
            this.Controls.Add(this.lblTotal);
            this.Controls.Add(this.lblDescription);
            this.Controls.Add(this.prgTotal);
            this.Controls.Add(this.prgCurrent);
            this.Controls.Add(this.picMinimize);
            this.Controls.Add(this.picClose);
            this.Controls.Add(this.picTab);
            this.Controls.Add(this.picBanner);
            this.Controls.Add(this.webNews);
            this.ForeColor = System.Drawing.SystemColors.ControlText;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "Menu";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            ((System.ComponentModel.ISupportInitialize)(this.picBanner)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picTab)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picMinimize)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picClose)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.WebBrowser webNews;
        private System.Windows.Forms.PictureBox picBanner;
        private System.Windows.Forms.PictureBox picTab;
        private System.Windows.Forms.PictureBox picMinimize;
        private System.Windows.Forms.PictureBox picClose;
        private System.Windows.Forms.Label lblDescription;
        public System.Windows.Forms.Label lblTotal;
        public System.Windows.Forms.Label lblCurrent;
        public System.Windows.Forms.ProgressBar prgCurrent;
        public System.Windows.Forms.ProgressBar prgTotal;
        public System.Windows.Forms.Label lblPlay;
        public System.Windows.Forms.Label lblRetry;

    }
}

