namespace TestUI.FingerPrinting
{
    partial class Form1
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
            this._tbSingleFile = new System.Windows.Forms.TextBox();
            this.BtnBrowse = new System.Windows.Forms.Button();
            this._btnStart = new System.Windows.Forms.Button();
            this._tbRootFolder = new System.Windows.Forms.TextBox();
            this._btnLoad = new System.Windows.Forms.Button();
            this._dgvFillDatabase = new System.Windows.Forms.DataGridView();
            this.Artist = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Title = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ISRC = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Length = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.SubFingerprints = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this._dgvFillDatabase)).BeginInit();
            this.SuspendLayout();
            // 
            // _tbSingleFile
            // 
            this._tbSingleFile.Location = new System.Drawing.Point(40, 60);
            this._tbSingleFile.Name = "_tbSingleFile";
            this._tbSingleFile.Size = new System.Drawing.Size(657, 31);
            this._tbSingleFile.TabIndex = 0;
            // 
            // BtnBrowse
            // 
            this.BtnBrowse.Location = new System.Drawing.Point(40, 111);
            this.BtnBrowse.Name = "BtnBrowse";
            this.BtnBrowse.Size = new System.Drawing.Size(156, 45);
            this.BtnBrowse.TabIndex = 1;
            this.BtnBrowse.Text = "Browse";
            this.BtnBrowse.UseVisualStyleBackColor = true;
            this.BtnBrowse.Click += new System.EventHandler(this.BtnBrowse_Click);
            // 
            // _btnStart
            // 
            this._btnStart.Location = new System.Drawing.Point(263, 111);
            this._btnStart.Name = "_btnStart";
            this._btnStart.Size = new System.Drawing.Size(156, 45);
            this._btnStart.TabIndex = 2;
            this._btnStart.Text = "Start";
            this._btnStart.UseVisualStyleBackColor = true;
            this._btnStart.Click += new System.EventHandler(this.BtnStartClick);
            // 
            // _tbRootFolder
            // 
            this._tbRootFolder.Location = new System.Drawing.Point(40, 284);
            this._tbRootFolder.Name = "_tbRootFolder";
            this._tbRootFolder.Size = new System.Drawing.Size(657, 31);
            this._tbRootFolder.TabIndex = 88;
            this._tbRootFolder.TextChanged += new System.EventHandler(this._tbRootFolder_TextChanged);
            this._tbRootFolder.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.TbRootFolderMouseDoubleClick);
            // 
            // _btnLoad
            // 
            this._btnLoad.Location = new System.Drawing.Point(40, 337);
            this._btnLoad.Name = "_btnLoad";
            this._btnLoad.Size = new System.Drawing.Size(167, 74);
            this._btnLoad.TabIndex = 89;
            this._btnLoad.Text = "Load";
            this._btnLoad.UseVisualStyleBackColor = true;
            this._btnLoad.Click += new System.EventHandler(this._btnLoad_Click);
            // 
            // _dgvFillDatabase
            // 
            this._dgvFillDatabase.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this._dgvFillDatabase.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this._dgvFillDatabase.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Artist,
            this.Title,
            this.ISRC,
            this.Length,
            this.SubFingerprints});
            this._dgvFillDatabase.Location = new System.Drawing.Point(40, 420);
            this._dgvFillDatabase.Margin = new System.Windows.Forms.Padding(6);
            this._dgvFillDatabase.Name = "_dgvFillDatabase";
            this._dgvFillDatabase.ReadOnly = true;
            this._dgvFillDatabase.Size = new System.Drawing.Size(1384, 355);
            this._dgvFillDatabase.TabIndex = 90;
            // 
            // Artist
            // 
            this.Artist.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.Artist.HeaderText = "Artist";
            this.Artist.Name = "Artist";
            this.Artist.ReadOnly = true;
            // 
            // Title
            // 
            this.Title.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.Title.FillWeight = 200F;
            this.Title.HeaderText = "Title";
            this.Title.Name = "Title";
            this.Title.ReadOnly = true;
            // 
            // ISRC
            // 
            this.ISRC.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.ISRC.HeaderText = "ISRC";
            this.ISRC.Name = "ISRC";
            this.ISRC.ReadOnly = true;
            // 
            // Length
            // 
            this.Length.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.Length.FillWeight = 50F;
            this.Length.HeaderText = "Length";
            this.Length.Name = "Length";
            this.Length.ReadOnly = true;
            // 
            // SubFingerprints
            // 
            this.SubFingerprints.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.SubFingerprints.FillWeight = 50F;
            this.SubFingerprints.HeaderText = "SubFingerprints";
            this.SubFingerprints.Name = "SubFingerprints";
            this.SubFingerprints.ReadOnly = true;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(40, 13);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(322, 25);
            this.label1.TabIndex = 91;
            this.label1.Text = "Browse a file you want to search";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(35, 244);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(310, 25);
            this.label2.TabIndex = 92;
            this.label2.Text = "Doble click to load music folder";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(12F, 25F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1477, 984);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this._dgvFillDatabase);
            this.Controls.Add(this._btnLoad);
            this.Controls.Add(this._tbRootFolder);
            this.Controls.Add(this._btnStart);
            this.Controls.Add(this.BtnBrowse);
            this.Controls.Add(this._tbSingleFile);
            this.Name = "Form1";
            this.Text = "SoundFingerprint";
            ((System.ComponentModel.ISupportInitialize)(this._dgvFillDatabase)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox _tbSingleFile;
        private System.Windows.Forms.Button BtnBrowse;
        private System.Windows.Forms.Button _btnStart;
        private System.Windows.Forms.TextBox _tbRootFolder;
        private System.Windows.Forms.Button _btnLoad;
        private System.Windows.Forms.DataGridView _dgvFillDatabase;
        private System.Windows.Forms.DataGridViewTextBoxColumn Artist;
        private System.Windows.Forms.DataGridViewTextBoxColumn Title;
        private System.Windows.Forms.DataGridViewTextBoxColumn ISRC;
        private System.Windows.Forms.DataGridViewTextBoxColumn Length;
        private System.Windows.Forms.DataGridViewTextBoxColumn SubFingerprints;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
    }
}

