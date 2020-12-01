namespace Tinkering_Audio {
    partial class TinkeringAudioForm {
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
            this.Villagebtn = new System.Windows.Forms.Button();
            this.Forestbtn = new System.Windows.Forms.Button();
            this.Cavebtn = new System.Windows.Forms.Button();
            this.Oceanbtn = new System.Windows.Forms.Button();
            this.btn_LoadAudioFile = new System.Windows.Forms.Button();
            this.btn_SaveAudioFile = new System.Windows.Forms.Button();
            this.btn_PlayAudio = new System.Windows.Forms.Button();
            this.btn_StopAudio = new System.Windows.Forms.Button();
            this.btn_PauseAudio = new System.Windows.Forms.Button();
            this.cbox_WaveType = new System.Windows.Forms.ComboBox();
            this.cbox_Bitrate = new System.Windows.Forms.ComboBox();
            this.lbl_SavingBitrate = new System.Windows.Forms.Label();
            this.lbl_ChangeWaveType = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // Villagebtn
            // 
            this.Villagebtn.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.Villagebtn.Location = new System.Drawing.Point(165, 150);
            this.Villagebtn.Name = "Villagebtn";
            this.Villagebtn.Size = new System.Drawing.Size(150, 125);
            this.Villagebtn.TabIndex = 3;
            this.Villagebtn.Text = "Village";
            this.Villagebtn.UseVisualStyleBackColor = true;
            this.Villagebtn.Click += new System.EventHandler(this.Villagebtn_Click);
            // 
            // Forestbtn
            // 
            this.Forestbtn.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.Forestbtn.Location = new System.Drawing.Point(325, 150);
            this.Forestbtn.Name = "Forestbtn";
            this.Forestbtn.Size = new System.Drawing.Size(150, 125);
            this.Forestbtn.TabIndex = 4;
            this.Forestbtn.Text = "Forest";
            this.Forestbtn.UseVisualStyleBackColor = true;
            this.Forestbtn.Click += new System.EventHandler(this.Forestbtn_Click);
            // 
            // Cavebtn
            // 
            this.Cavebtn.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.Cavebtn.Location = new System.Drawing.Point(485, 150);
            this.Cavebtn.Name = "Cavebtn";
            this.Cavebtn.Size = new System.Drawing.Size(150, 125);
            this.Cavebtn.TabIndex = 5;
            this.Cavebtn.Text = "Cave";
            this.Cavebtn.UseVisualStyleBackColor = true;
            this.Cavebtn.Click += new System.EventHandler(this.Cavebtn_Click);
            // 
            // Oceanbtn
            // 
            this.Oceanbtn.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.Oceanbtn.Location = new System.Drawing.Point(645, 150);
            this.Oceanbtn.Name = "Oceanbtn";
            this.Oceanbtn.Size = new System.Drawing.Size(150, 125);
            this.Oceanbtn.TabIndex = 6;
            this.Oceanbtn.Text = "Ocean";
            this.Oceanbtn.UseVisualStyleBackColor = true;
            this.Oceanbtn.Click += new System.EventHandler(this.Oceanbtn_Click);
            // 
            // btn_LoadAudioFile
            // 
            this.btn_LoadAudioFile.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btn_LoadAudioFile.Location = new System.Drawing.Point(25, 350);
            this.btn_LoadAudioFile.Name = "btn_LoadAudioFile";
            this.btn_LoadAudioFile.Size = new System.Drawing.Size(250, 25);
            this.btn_LoadAudioFile.TabIndex = 7;
            this.btn_LoadAudioFile.Text = "Load File";
            this.btn_LoadAudioFile.UseVisualStyleBackColor = true;
            this.btn_LoadAudioFile.Click += new System.EventHandler(this.btn_LoadAudioFile_Click);
            // 
            // btn_SaveAudioFile
            // 
            this.btn_SaveAudioFile.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btn_SaveAudioFile.Location = new System.Drawing.Point(675, 350);
            this.btn_SaveAudioFile.Name = "btn_SaveAudioFile";
            this.btn_SaveAudioFile.Size = new System.Drawing.Size(250, 25);
            this.btn_SaveAudioFile.TabIndex = 8;
            this.btn_SaveAudioFile.Text = "Save File";
            this.btn_SaveAudioFile.UseVisualStyleBackColor = true;
            this.btn_SaveAudioFile.Click += new System.EventHandler(this.btn_SaveAudioFile_Click);
            // 
            // btn_PlayAudio
            // 
            this.btn_PlayAudio.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.btn_PlayAudio.Location = new System.Drawing.Point(430, 25);
            this.btn_PlayAudio.Name = "btn_PlayAudio";
            this.btn_PlayAudio.Size = new System.Drawing.Size(100, 25);
            this.btn_PlayAudio.TabIndex = 9;
            this.btn_PlayAudio.Text = "Play";
            this.btn_PlayAudio.UseVisualStyleBackColor = true;
            this.btn_PlayAudio.Click += new System.EventHandler(this.btn_PlayAudio_Click);
            // 
            // btn_StopAudio
            // 
            this.btn_StopAudio.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.btn_StopAudio.Location = new System.Drawing.Point(555, 25);
            this.btn_StopAudio.Name = "btn_StopAudio";
            this.btn_StopAudio.Size = new System.Drawing.Size(100, 25);
            this.btn_StopAudio.TabIndex = 10;
            this.btn_StopAudio.Text = "Stop";
            this.btn_StopAudio.UseVisualStyleBackColor = true;
            this.btn_StopAudio.Click += new System.EventHandler(this.btn_StopAudio_Click);
            // 
            // btn_PauseAudio
            // 
            this.btn_PauseAudio.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.btn_PauseAudio.Location = new System.Drawing.Point(305, 25);
            this.btn_PauseAudio.Name = "btn_PauseAudio";
            this.btn_PauseAudio.Size = new System.Drawing.Size(100, 25);
            this.btn_PauseAudio.TabIndex = 11;
            this.btn_PauseAudio.Text = "Pause";
            this.btn_PauseAudio.UseVisualStyleBackColor = true;
            this.btn_PauseAudio.Click += new System.EventHandler(this.btn_PauseAudio_Click);
            // 
            // cbox_WaveType
            // 
            this.cbox_WaveType.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.cbox_WaveType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbox_WaveType.FormattingEnabled = true;
            this.cbox_WaveType.Location = new System.Drawing.Point(355, 90);
            this.cbox_WaveType.Name = "cbox_WaveType";
            this.cbox_WaveType.Size = new System.Drawing.Size(250, 21);
            this.cbox_WaveType.TabIndex = 12;
            this.cbox_WaveType.SelectedIndexChanged += new System.EventHandler(this.cbox_WaveType_SelectedIndexChanged);
            // 
            // cbox_Bitrate
            // 
            this.cbox_Bitrate.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.cbox_Bitrate.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbox_Bitrate.FormattingEnabled = true;
            this.cbox_Bitrate.Location = new System.Drawing.Point(675, 410);
            this.cbox_Bitrate.Name = "cbox_Bitrate";
            this.cbox_Bitrate.Size = new System.Drawing.Size(250, 21);
            this.cbox_Bitrate.TabIndex = 13;
            this.cbox_Bitrate.SelectedIndexChanged += new System.EventHandler(this.cbox_Bitrate_SelectedIndexChanged);
            // 
            // lbl_SavingBitrate
            // 
            this.lbl_SavingBitrate.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.lbl_SavingBitrate.Location = new System.Drawing.Point(675, 380);
            this.lbl_SavingBitrate.Name = "lbl_SavingBitrate";
            this.lbl_SavingBitrate.Size = new System.Drawing.Size(250, 25);
            this.lbl_SavingBitrate.TabIndex = 14;
            this.lbl_SavingBitrate.Text = "Saving Bitrate:";
            this.lbl_SavingBitrate.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lbl_ChangeWaveType
            // 
            this.lbl_ChangeWaveType.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lbl_ChangeWaveType.Location = new System.Drawing.Point(355, 60);
            this.lbl_ChangeWaveType.Name = "lbl_ChangeWaveType";
            this.lbl_ChangeWaveType.Size = new System.Drawing.Size(250, 25);
            this.lbl_ChangeWaveType.TabIndex = 15;
            this.lbl_ChangeWaveType.Text = "Change Wave Type:";
            this.lbl_ChangeWaveType.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // TinkeringAudioForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(944, 501);
            this.Controls.Add(this.lbl_ChangeWaveType);
            this.Controls.Add(this.lbl_SavingBitrate);
            this.Controls.Add(this.cbox_Bitrate);
            this.Controls.Add(this.cbox_WaveType);
            this.Controls.Add(this.btn_PauseAudio);
            this.Controls.Add(this.btn_StopAudio);
            this.Controls.Add(this.btn_PlayAudio);
            this.Controls.Add(this.btn_SaveAudioFile);
            this.Controls.Add(this.btn_LoadAudioFile);
            this.Controls.Add(this.Oceanbtn);
            this.Controls.Add(this.Cavebtn);
            this.Controls.Add(this.Forestbtn);
            this.Controls.Add(this.Villagebtn);
            this.Name = "TinkeringAudioForm";
            this.Text = "Tinkering Audio";
            this.Load += new System.EventHandler(this.TinkeringAudioForm_Load);
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Button Villagebtn;
        private System.Windows.Forms.Button Forestbtn;
        private System.Windows.Forms.Button Cavebtn;
        private System.Windows.Forms.Button Oceanbtn;
        private System.Windows.Forms.Button btn_LoadAudioFile;
        private System.Windows.Forms.Button btn_SaveAudioFile;
        private System.Windows.Forms.Button btn_PlayAudio;
        private System.Windows.Forms.Button btn_StopAudio;
        private System.Windows.Forms.Button btn_PauseAudio;
        private System.Windows.Forms.ComboBox cbox_WaveType;
        private System.Windows.Forms.ComboBox cbox_Bitrate;
        private System.Windows.Forms.Label lbl_SavingBitrate;
        private System.Windows.Forms.Label lbl_ChangeWaveType;
    }
}

