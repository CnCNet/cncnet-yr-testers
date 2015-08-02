namespace CnCNetTesters
{
    partial class DownloadUpdate
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
            this.testUpdate = new System.Windows.Forms.Button();
            this.progressBar = new System.Windows.Forms.ProgressBar();
            this.downloadSpeed = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.percent = new System.Windows.Forms.Label();
            this.downloadMb = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // testUpdate
            // 
            this.testUpdate.Location = new System.Drawing.Point(178, 231);
            this.testUpdate.Name = "testUpdate";
            this.testUpdate.Size = new System.Drawing.Size(102, 30);
            this.testUpdate.TabIndex = 0;
            this.testUpdate.Text = "Get Updates";
            this.testUpdate.UseVisualStyleBackColor = true;
            this.testUpdate.Click += new System.EventHandler(this.testUpdate_Click);
            // 
            // progressBar
            // 
            this.progressBar.Location = new System.Drawing.Point(12, 47);
            this.progressBar.Name = "progressBar";
            this.progressBar.Size = new System.Drawing.Size(268, 23);
            this.progressBar.TabIndex = 1;
            // 
            // downloadSpeed
            // 
            this.downloadSpeed.AutoSize = true;
            this.downloadSpeed.Location = new System.Drawing.Point(12, 17);
            this.downloadSpeed.Name = "downloadSpeed";
            this.downloadSpeed.Size = new System.Drawing.Size(0, 13);
            this.downloadSpeed.TabIndex = 2;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 57);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(0, 13);
            this.label1.TabIndex = 3;
            // 
            // percent
            // 
            this.percent.AutoSize = true;
            this.percent.Location = new System.Drawing.Point(12, 87);
            this.percent.Name = "percent";
            this.percent.Size = new System.Drawing.Size(0, 13);
            this.percent.TabIndex = 4;
            // 
            // downloadMb
            // 
            this.downloadMb.AutoSize = true;
            this.downloadMb.Location = new System.Drawing.Point(12, 115);
            this.downloadMb.Name = "downloadMb";
            this.downloadMb.Size = new System.Drawing.Size(0, 13);
            this.downloadMb.TabIndex = 5;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(12, 231);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(110, 30);
            this.button1.TabIndex = 6;
            this.button1.Text = "Launch CnCNet";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // DownloadUpdate
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(292, 273);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.downloadMb);
            this.Controls.Add(this.percent);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.downloadSpeed);
            this.Controls.Add(this.progressBar);
            this.Controls.Add(this.testUpdate);
            this.Name = "DownloadUpdate";
            this.Text = "CnCNet Testers";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button testUpdate;
        private System.Windows.Forms.ProgressBar progressBar;
        private System.Windows.Forms.Label downloadSpeed;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label percent;
        private System.Windows.Forms.Label downloadMb;
        private System.Windows.Forms.Button button1;
    }
}

