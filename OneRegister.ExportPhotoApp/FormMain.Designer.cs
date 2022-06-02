namespace OneRegister.ExportPhotoApp
{
    partial class FormMain
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormMain));
            this.lblFilePath = new System.Windows.Forms.Label();
            this.tbxPath = new System.Windows.Forms.TextBox();
            this.btnBrowse = new System.Windows.Forms.Button();
            this.rtbxResult = new System.Windows.Forms.RichTextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.tbxUser = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.tbxPass = new System.Windows.Forms.TextBox();
            this.btnLogin = new System.Windows.Forms.Button();
            this.btnExport = new System.Windows.Forms.Button();
            this.progressBarExport = new System.Windows.Forms.ProgressBar();
            this.cboSchool = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.cboHomeroom = new System.Windows.Forms.ComboBox();
            this.label5 = new System.Windows.Forms.Label();
            this.cboYear = new System.Windows.Forms.ComboBox();
            this.label6 = new System.Windows.Forms.Label();
            this.cboClass = new System.Windows.Forms.ComboBox();
            this.tbxUrl = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.tbxIcNumber = new System.Windows.Forms.TextBox();
            this.btnSetFilter = new System.Windows.Forms.Button();
            this.lblVersion = new System.Windows.Forms.Label();
            this.btnGetClass = new System.Windows.Forms.Button();
            this.label9 = new System.Windows.Forms.Label();
            this.tbxDmsUrl = new System.Windows.Forms.TextBox();
            this.label10 = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.label12 = new System.Windows.Forms.Label();
            this.label13 = new System.Windows.Forms.Label();
            this.label14 = new System.Windows.Forms.Label();
            this.btnFilterById = new System.Windows.Forms.Button();
            this.pbLoading = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.pbLoading)).BeginInit();
            this.SuspendLayout();
            // 
            // lblFilePath
            // 
            this.lblFilePath.AutoSize = true;
            this.lblFilePath.Location = new System.Drawing.Point(58, 157);
            this.lblFilePath.Name = "lblFilePath";
            this.lblFilePath.Size = new System.Drawing.Size(72, 20);
            this.lblFilePath.TabIndex = 0;
            this.lblFilePath.Text = "Save Path";
            // 
            // tbxPath
            // 
            this.tbxPath.Location = new System.Drawing.Point(131, 153);
            this.tbxPath.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.tbxPath.Name = "tbxPath";
            this.tbxPath.ReadOnly = true;
            this.tbxPath.Size = new System.Drawing.Size(559, 27);
            this.tbxPath.TabIndex = 1;
            // 
            // btnBrowse
            // 
            this.btnBrowse.Location = new System.Drawing.Point(709, 152);
            this.btnBrowse.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btnBrowse.Name = "btnBrowse";
            this.btnBrowse.Size = new System.Drawing.Size(86, 31);
            this.btnBrowse.TabIndex = 2;
            this.btnBrowse.Text = "Browse";
            this.btnBrowse.UseVisualStyleBackColor = true;
            this.btnBrowse.Click += new System.EventHandler(this.btnBrowse_Click);
            // 
            // rtbxResult
            // 
            this.rtbxResult.Location = new System.Drawing.Point(131, 363);
            this.rtbxResult.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.rtbxResult.Name = "rtbxResult";
            this.rtbxResult.ReadOnly = true;
            this.rtbxResult.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.Vertical;
            this.rtbxResult.Size = new System.Drawing.Size(662, 187);
            this.rtbxResult.TabIndex = 3;
            this.rtbxResult.Text = "";
            this.rtbxResult.TextChanged += new System.EventHandler(this.rtbxResult_TextChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(50, 115);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(82, 20);
            this.label1.TabIndex = 4;
            this.label1.Text = "User Name";
            // 
            // tbxUser
            // 
            this.tbxUser.Location = new System.Drawing.Point(131, 111);
            this.tbxUser.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.tbxUser.Name = "tbxUser";
            this.tbxUser.Size = new System.Drawing.Size(211, 27);
            this.tbxUser.TabIndex = 5;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(384, 115);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(70, 20);
            this.label2.TabIndex = 6;
            this.label2.Text = "Password";
            // 
            // tbxPass
            // 
            this.tbxPass.Location = new System.Drawing.Point(456, 109);
            this.tbxPass.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.tbxPass.Name = "tbxPass";
            this.tbxPass.Size = new System.Drawing.Size(235, 27);
            this.tbxPass.TabIndex = 7;
            this.tbxPass.UseSystemPasswordChar = true;
            // 
            // btnLogin
            // 
            this.btnLogin.Location = new System.Drawing.Point(709, 109);
            this.btnLogin.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btnLogin.Name = "btnLogin";
            this.btnLogin.Size = new System.Drawing.Size(86, 31);
            this.btnLogin.TabIndex = 8;
            this.btnLogin.Text = "Login";
            this.btnLogin.UseVisualStyleBackColor = true;
            this.btnLogin.Click += new System.EventHandler(this.btnLogin_Click);
            // 
            // btnExport
            // 
            this.btnExport.Location = new System.Drawing.Point(709, 597);
            this.btnExport.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btnExport.Name = "btnExport";
            this.btnExport.Size = new System.Drawing.Size(86, 31);
            this.btnExport.TabIndex = 9;
            this.btnExport.Text = "Export";
            this.btnExport.UseVisualStyleBackColor = true;
            this.btnExport.Click += new System.EventHandler(this.btnExport_Click);
            // 
            // progressBarExport
            // 
            this.progressBarExport.Location = new System.Drawing.Point(131, 559);
            this.progressBarExport.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.progressBarExport.Name = "progressBarExport";
            this.progressBarExport.Size = new System.Drawing.Size(663, 31);
            this.progressBarExport.TabIndex = 10;
            // 
            // cboSchool
            // 
            this.cboSchool.FormattingEnabled = true;
            this.cboSchool.Location = new System.Drawing.Point(131, 212);
            this.cboSchool.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.cboSchool.Name = "cboSchool";
            this.cboSchool.Size = new System.Drawing.Size(355, 28);
            this.cboSchool.TabIndex = 11;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(75, 217);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(54, 20);
            this.label3.TabIndex = 12;
            this.label3.Text = "School";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(384, 268);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(90, 20);
            this.label4.TabIndex = 13;
            this.label4.Text = "HomeRoom";
            // 
            // cboHomeroom
            // 
            this.cboHomeroom.FormattingEnabled = true;
            this.cboHomeroom.Location = new System.Drawing.Point(470, 263);
            this.cboHomeroom.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.cboHomeroom.Name = "cboHomeroom";
            this.cboHomeroom.Size = new System.Drawing.Size(221, 28);
            this.cboHomeroom.TabIndex = 14;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(513, 223);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(37, 20);
            this.label5.TabIndex = 15;
            this.label5.Text = "Year";
            // 
            // cboYear
            // 
            this.cboYear.FormattingEnabled = true;
            this.cboYear.Location = new System.Drawing.Point(553, 213);
            this.cboYear.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.cboYear.Name = "cboYear";
            this.cboYear.Size = new System.Drawing.Size(138, 28);
            this.cboYear.TabIndex = 16;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(86, 268);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(42, 20);
            this.label6.TabIndex = 17;
            this.label6.Text = "Class";
            // 
            // cboClass
            // 
            this.cboClass.FormattingEnabled = true;
            this.cboClass.Location = new System.Drawing.Point(131, 263);
            this.cboClass.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.cboClass.Name = "cboClass";
            this.cboClass.Size = new System.Drawing.Size(231, 28);
            this.cboClass.TabIndex = 18;
            // 
            // tbxUrl
            // 
            this.tbxUrl.Location = new System.Drawing.Point(131, 7);
            this.tbxUrl.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.tbxUrl.Name = "tbxUrl";
            this.tbxUrl.Size = new System.Drawing.Size(559, 27);
            this.tbxUrl.TabIndex = 19;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(69, 12);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(61, 20);
            this.label7.TabIndex = 20;
            this.label7.Text = "API URL";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(56, 321);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(74, 20);
            this.label8.TabIndex = 21;
            this.label8.Text = "IcNumber";
            // 
            // tbxIcNumber
            // 
            this.tbxIcNumber.Location = new System.Drawing.Point(131, 311);
            this.tbxIcNumber.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.tbxIcNumber.Name = "tbxIcNumber";
            this.tbxIcNumber.Size = new System.Drawing.Size(282, 27);
            this.tbxIcNumber.TabIndex = 22;
            // 
            // btnSetFilter
            // 
            this.btnSetFilter.Location = new System.Drawing.Point(709, 263);
            this.btnSetFilter.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btnSetFilter.Name = "btnSetFilter";
            this.btnSetFilter.Size = new System.Drawing.Size(86, 31);
            this.btnSetFilter.TabIndex = 23;
            this.btnSetFilter.Text = "Set Filter";
            this.btnSetFilter.UseVisualStyleBackColor = true;
            this.btnSetFilter.Click += new System.EventHandler(this.btnSetFilter_Click);
            // 
            // lblVersion
            // 
            this.lblVersion.AutoSize = true;
            this.lblVersion.Location = new System.Drawing.Point(50, 617);
            this.lblVersion.Name = "lblVersion";
            this.lblVersion.Size = new System.Drawing.Size(59, 20);
            this.lblVersion.TabIndex = 24;
            this.lblVersion.Text = "V2.0.0.0";
            // 
            // btnGetClass
            // 
            this.btnGetClass.Location = new System.Drawing.Point(709, 212);
            this.btnGetClass.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btnGetClass.Name = "btnGetClass";
            this.btnGetClass.Size = new System.Drawing.Size(86, 31);
            this.btnGetClass.TabIndex = 25;
            this.btnGetClass.Text = "Get Classes";
            this.btnGetClass.UseVisualStyleBackColor = true;
            this.btnGetClass.Click += new System.EventHandler(this.btnGetClass_Click);
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(61, 63);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(71, 20);
            this.label9.TabIndex = 26;
            this.label9.Text = "DMS URL";
            // 
            // tbxDmsUrl
            // 
            this.tbxDmsUrl.Location = new System.Drawing.Point(131, 59);
            this.tbxDmsUrl.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.tbxDmsUrl.Name = "tbxDmsUrl";
            this.tbxDmsUrl.Size = new System.Drawing.Size(559, 27);
            this.tbxDmsUrl.TabIndex = 27;
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.BackColor = System.Drawing.SystemColors.HotTrack;
            this.label10.ForeColor = System.Drawing.SystemColors.ButtonHighlight;
            this.label10.Location = new System.Drawing.Point(840, 115);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(17, 20);
            this.label10.TabIndex = 28;
            this.label10.Text = "1";
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.BackColor = System.Drawing.SystemColors.HotTrack;
            this.label11.ForeColor = System.Drawing.SystemColors.ButtonHighlight;
            this.label11.Location = new System.Drawing.Point(840, 157);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(17, 20);
            this.label11.TabIndex = 29;
            this.label11.Text = "2";
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.BackColor = System.Drawing.SystemColors.HotTrack;
            this.label12.ForeColor = System.Drawing.SystemColors.ButtonHighlight;
            this.label12.Location = new System.Drawing.Point(840, 217);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(17, 20);
            this.label12.TabIndex = 30;
            this.label12.Text = "3";
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.BackColor = System.Drawing.SystemColors.HotTrack;
            this.label13.ForeColor = System.Drawing.SystemColors.ButtonHighlight;
            this.label13.Location = new System.Drawing.Point(840, 268);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(17, 20);
            this.label13.TabIndex = 31;
            this.label13.Text = "4";
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.BackColor = System.Drawing.SystemColors.HotTrack;
            this.label14.ForeColor = System.Drawing.SystemColors.ButtonHighlight;
            this.label14.Location = new System.Drawing.Point(840, 603);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(17, 20);
            this.label14.TabIndex = 32;
            this.label14.Text = "5";
            // 
            // btnFilterById
            // 
            this.btnFilterById.Location = new System.Drawing.Point(456, 311);
            this.btnFilterById.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btnFilterById.Name = "btnFilterById";
            this.btnFilterById.Size = new System.Drawing.Size(135, 31);
            this.btnFilterById.TabIndex = 33;
            this.btnFilterById.Text = "Filter By Identity";
            this.btnFilterById.UseVisualStyleBackColor = true;
            this.btnFilterById.Click += new System.EventHandler(this.btnFilterById_Click);
            // 
            // pbLoading
            // 
            this.pbLoading.Image = global::OneRegister.ExportPhotoApp.Properties.Resources.loading;
            this.pbLoading.Location = new System.Drawing.Point(436, 206);
            this.pbLoading.Name = "pbLoading";
            this.pbLoading.Size = new System.Drawing.Size(50, 50);
            this.pbLoading.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.pbLoading.TabIndex = 34;
            this.pbLoading.TabStop = false;
            this.pbLoading.UseWaitCursor = true;
            // 
            // FormMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(914, 653);
            this.Controls.Add(this.pbLoading);
            this.Controls.Add(this.btnFilterById);
            this.Controls.Add(this.label14);
            this.Controls.Add(this.label13);
            this.Controls.Add(this.label12);
            this.Controls.Add(this.label11);
            this.Controls.Add(this.label10);
            this.Controls.Add(this.tbxDmsUrl);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.btnGetClass);
            this.Controls.Add(this.lblVersion);
            this.Controls.Add(this.btnSetFilter);
            this.Controls.Add(this.tbxIcNumber);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.tbxUrl);
            this.Controls.Add(this.cboClass);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.cboYear);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.cboHomeroom);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.cboSchool);
            this.Controls.Add(this.progressBarExport);
            this.Controls.Add(this.btnExport);
            this.Controls.Add(this.btnLogin);
            this.Controls.Add(this.tbxPass);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.tbxUser);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.rtbxResult);
            this.Controls.Add(this.btnBrowse);
            this.Controls.Add(this.tbxPath);
            this.Controls.Add(this.lblFilePath);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.Name = "FormMain";
            this.Text = "OneRegister ExportPhoto App (by Nader)";
            this.Load += new System.EventHandler(this.FormMain_Load);
            ((System.ComponentModel.ISupportInitialize)(this.pbLoading)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblFilePath;
        private System.Windows.Forms.TextBox tbxPath;
        private System.Windows.Forms.Button btnBrowse;
        private System.Windows.Forms.RichTextBox rtbxResult;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox tbxUser;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox tbxPass;
        private System.Windows.Forms.Button btnLogin;
        private System.Windows.Forms.Button btnExport;
        private System.Windows.Forms.ProgressBar progressBarExport;
        private System.Windows.Forms.ComboBox cboSchool;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.ComboBox cboHomeroom;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.ComboBox cboYear;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.ComboBox cboClass;
        private System.Windows.Forms.TextBox tbxUrl;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.TextBox tbxIcNumber;
        private System.Windows.Forms.Button btnSetFilter;
        private System.Windows.Forms.Label lblVersion;
        private System.Windows.Forms.Button btnGetClass;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.TextBox tbxDmsUrl;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.Button btnFilterById;
        private System.Windows.Forms.PictureBox pbLoading;
    }
}

