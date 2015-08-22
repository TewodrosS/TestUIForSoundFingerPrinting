namespace WindowsFormsClient
{
    partial class SearchUsingService
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
            this._btnBrowse = new System.Windows.Forms.Button();
            this.textBoxFilePath = new System.Windows.Forms.TextBox();
            this.textBoxUrl = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this._btnSend = new System.Windows.Forms.Button();
            this.resultLable = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // _btnBrowse
            // 
            this._btnBrowse.Location = new System.Drawing.Point(12, 266);
            this._btnBrowse.Name = "_btnBrowse";
            this._btnBrowse.Size = new System.Drawing.Size(194, 59);
            this._btnBrowse.TabIndex = 0;
            this._btnBrowse.Text = "Browse";
            this._btnBrowse.UseVisualStyleBackColor = true;
            this._btnBrowse.Click += new System.EventHandler(this.Browse_Click);
            // 
            // textBoxFilePath
            // 
            this.textBoxFilePath.Location = new System.Drawing.Point(12, 210);
            this.textBoxFilePath.Name = "textBoxFilePath";
            this.textBoxFilePath.Size = new System.Drawing.Size(550, 31);
            this.textBoxFilePath.TabIndex = 1;
            // 
            // textBoxUrl
            // 
            this.textBoxUrl.Location = new System.Drawing.Point(12, 128);
            this.textBoxUrl.Name = "textBoxUrl";
            this.textBoxUrl.Size = new System.Drawing.Size(550, 31);
            this.textBoxUrl.TabIndex = 2;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(7, 70);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(196, 25);
            this.label1.TabIndex = 3;
            this.label1.Text = "Web API base URL";
            // 
            // _btnSend
            // 
            this._btnSend.Location = new System.Drawing.Point(268, 266);
            this._btnSend.Name = "_btnSend";
            this._btnSend.Size = new System.Drawing.Size(194, 59);
            this._btnSend.TabIndex = 4;
            this._btnSend.Text = "Send";
            this._btnSend.UseVisualStyleBackColor = true;
            this._btnSend.Click += new System.EventHandler(this.Send_Click);
            // 
            // resultLable
            // 
            this.resultLable.AutoSize = true;
            this.resultLable.Location = new System.Drawing.Point(69, 402);
            this.resultLable.Name = "resultLable";
            this.resultLable.Size = new System.Drawing.Size(126, 25);
            this.resultLable.TabIndex = 5;
            this.resultLable.Text = "ResultLable";
            // 
            // SearchUsingService
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(12F, 25F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1014, 864);
            this.Controls.Add(this.resultLable);
            this.Controls.Add(this._btnSend);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.textBoxUrl);
            this.Controls.Add(this.textBoxFilePath);
            this.Controls.Add(this._btnBrowse);
            this.Name = "SearchUsingService";
            this.Text = "Search Using Service Call";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button _btnBrowse;
        private System.Windows.Forms.TextBox textBoxFilePath;
        private System.Windows.Forms.TextBox textBoxUrl;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button _btnSend;
        private System.Windows.Forms.Label resultLable;
    }
}

