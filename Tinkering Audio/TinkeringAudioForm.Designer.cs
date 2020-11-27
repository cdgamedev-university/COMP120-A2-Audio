namespace TinkeringAudio {
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
            this.components = new System.ComponentModel.Container();
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
            this.waveTypeBindingSource = new System.Windows.Forms.BindingSource(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.waveTypeBindingSource)).BeginInit();
            this.SuspendLayout();
            // 
            // Villagebtn
            // 
            this.Villagebtn.Location = new System.Drawing.Point(121, 158);
            this.Villagebtn.Name = "Villagebtn";
            this.Villagebtn.Size = new System.Drawing.Size(130, 125);
            this.Villagebtn.TabIndex = 3;
            this.Villagebtn.Text = "Village";
            this.Villagebtn.UseVisualStyleBackColor = true;
            this.Villagebtn.Click += new System.EventHandler(this.Villagebtn_Click);
            // 
            // Forestbtn
            // 
            this.Forestbtn.Location = new System.Drawing.Point(260, 158);
            this.Forestbtn.Name = "Forestbtn";
            this.Forestbtn.Size = new System.Drawing.Size(130, 125);
            this.Forestbtn.TabIndex = 4;
            this.Forestbtn.Text = "Forest";
            this.Forestbtn.UseVisualStyleBackColor = true;
            this.Forestbtn.Click += new System.EventHandler(this.Forestbtn_Click);
            // 
            // Cavebtn
            // 
            this.Cavebtn.Location = new System.Drawing.Point(399, 158);
            this.Cavebtn.Name = "Cavebtn";
            this.Cavebtn.Size = new System.Drawing.Size(130, 125);
            this.Cavebtn.TabIndex = 5;
            this.Cavebtn.Text = "Cave";
            this.Cavebtn.UseVisualStyleBackColor = true;
            this.Cavebtn.Click += new System.EventHandler(this.Cavebtn_Click);
            // 
            // Oceanbtn
            // 
            this.Oceanbtn.Location = new System.Drawing.Point(538, 158);
            this.Oceanbtn.Name = "Oceanbtn";
            this.Oceanbtn.Size = new System.Drawing.Size(130, 125);
            this.Oceanbtn.TabIndex = 6;
            this.Oceanbtn.Text = "Ocean";
            this.Oceanbtn.UseVisualStyleBackColor = true;
            this.Oceanbtn.Click += new System.EventHandler(this.Oceanbtn_Click);
            // 
            // btn_LoadAudioFile
            // 
            this.btn_LoadAudioFile.Location = new System.Drawing.Point(261, 12);
            this.btn_LoadAudioFile.Name = "btn_LoadAudioFile";
            this.btn_LoadAudioFile.Size = new System.Drawing.Size(265, 23);
            this.btn_LoadAudioFile.TabIndex = 7;
            this.btn_LoadAudioFile.Text = "Load File";
            this.btn_LoadAudioFile.UseVisualStyleBackColor = true;
            this.btn_LoadAudioFile.Click += new System.EventHandler(this.btn_LoadAudioFile_Click);
            // 
            // btn_SaveAudioFile
            // 
            this.btn_SaveAudioFile.Location = new System.Drawing.Point(261, 395);
            this.btn_SaveAudioFile.Name = "btn_SaveAudioFile";
            this.btn_SaveAudioFile.Size = new System.Drawing.Size(265, 23);
            this.btn_SaveAudioFile.TabIndex = 8;
            this.btn_SaveAudioFile.Text = "Save File";
            this.btn_SaveAudioFile.UseVisualStyleBackColor = true;
            this.btn_SaveAudioFile.Click += new System.EventHandler(this.btn_SaveAudioFile_Click);
            // 
            // btn_PlayAudio
            // 
            this.btn_PlayAudio.Location = new System.Drawing.Point(261, 366);
            this.btn_PlayAudio.Name = "btn_PlayAudio";
            this.btn_PlayAudio.Size = new System.Drawing.Size(84, 23);
            this.btn_PlayAudio.TabIndex = 9;
            this.btn_PlayAudio.Text = "Play";
            this.btn_PlayAudio.UseVisualStyleBackColor = true;
            this.btn_PlayAudio.Click += new System.EventHandler(this.btn_PlayAudio_Click);
            // 
            // btn_StopAudio
            // 
            this.btn_StopAudio.Location = new System.Drawing.Point(442, 366);
            this.btn_StopAudio.Name = "btn_StopAudio";
            this.btn_StopAudio.Size = new System.Drawing.Size(84, 23);
            this.btn_StopAudio.TabIndex = 10;
            this.btn_StopAudio.Text = "Stop";
            this.btn_StopAudio.UseVisualStyleBackColor = true;
            this.btn_StopAudio.Click += new System.EventHandler(this.btn_StopAudio_Click);
            // 
            // btn_PauseAudio
            // 
            this.btn_PauseAudio.Location = new System.Drawing.Point(352, 366);
            this.btn_PauseAudio.Name = "btn_PauseAudio";
            this.btn_PauseAudio.Size = new System.Drawing.Size(84, 23);
            this.btn_PauseAudio.TabIndex = 11;
            this.btn_PauseAudio.Text = "Pause";
            this.btn_PauseAudio.UseVisualStyleBackColor = true;
            this.btn_PauseAudio.Click += new System.EventHandler(this.btn_PauseAudio_Click);
            // 
            // cbox_WaveType
            // 
            this.cbox_WaveType.FormattingEnabled = true;
            this.cbox_WaveType.Location = new System.Drawing.Point(261, 41);
            this.cbox_WaveType.Name = "cbox_WaveType";
            this.cbox_WaveType.Size = new System.Drawing.Size(265, 21);
            this.cbox_WaveType.TabIndex = 12;
            this.cbox_WaveType.SelectedIndexChanged += new System.EventHandler(this.cbox_WaveType_SelectedIndexChanged);
            // 
            // waveTypeBindingSource
            // 
            this.waveTypeBindingSource.DataSource = typeof(TinkeringAudio.TinkeringAudioForm.WaveType);
            // 
            // TinkeringAudioForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
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
            ((System.ComponentModel.ISupportInitialize)(this.waveTypeBindingSource)).EndInit();
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
        private System.Windows.Forms.BindingSource waveTypeBindingSource;
    }
}

