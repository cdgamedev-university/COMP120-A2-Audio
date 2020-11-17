﻿namespace TinkeringAudio {
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
            this.btn_GenerateMelody = new System.Windows.Forms.Button();
            this.btn_SaveMelody = new System.Windows.Forms.Button();
            this.btn_GenerateWhiteNoise = new System.Windows.Forms.Button();
            this.Villagebtn = new System.Windows.Forms.Button();
            this.Forestbtn = new System.Windows.Forms.Button();
            this.Cavebtn = new System.Windows.Forms.Button();
            this.Oceanbtn = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // btn_GenerateMelody
            // 
            this.btn_GenerateMelody.Location = new System.Drawing.Point(261, 12);
            this.btn_GenerateMelody.Name = "btn_GenerateMelody";
            this.btn_GenerateMelody.Size = new System.Drawing.Size(265, 23);
            this.btn_GenerateMelody.TabIndex = 0;
            this.btn_GenerateMelody.Text = "Generate Melody";
            this.btn_GenerateMelody.UseVisualStyleBackColor = true;
            this.btn_GenerateMelody.Click += new System.EventHandler(this.btn_GenerateMelody_Click);
            // 
            // btn_SaveMelody
            // 
            this.btn_SaveMelody.Location = new System.Drawing.Point(261, 70);
            this.btn_SaveMelody.Name = "btn_SaveMelody";
            this.btn_SaveMelody.Size = new System.Drawing.Size(265, 23);
            this.btn_SaveMelody.TabIndex = 1;
            this.btn_SaveMelody.Text = "Save Melody";
            this.btn_SaveMelody.UseVisualStyleBackColor = true;
            this.btn_SaveMelody.Click += new System.EventHandler(this.btn_SaveMelody_Click);
            // 
            // btn_GenerateWhiteNoise
            // 
            this.btn_GenerateWhiteNoise.Location = new System.Drawing.Point(261, 41);
            this.btn_GenerateWhiteNoise.Name = "btn_GenerateWhiteNoise";
            this.btn_GenerateWhiteNoise.Size = new System.Drawing.Size(265, 23);
            this.btn_GenerateWhiteNoise.TabIndex = 2;
            this.btn_GenerateWhiteNoise.Text = "Generate White Noise";
            this.btn_GenerateWhiteNoise.UseVisualStyleBackColor = true;
            this.btn_GenerateWhiteNoise.Click += new System.EventHandler(this.btn_GenerateWhiteNoise_Click);
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
            // TinkeringAudioForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.Oceanbtn);
            this.Controls.Add(this.Cavebtn);
            this.Controls.Add(this.Forestbtn);
            this.Controls.Add(this.Villagebtn);
            this.Controls.Add(this.btn_GenerateWhiteNoise);
            this.Controls.Add(this.btn_SaveMelody);
            this.Controls.Add(this.btn_GenerateMelody);
            this.Name = "TinkeringAudioForm";
            this.Text = "Tinkering Audio";
            this.Load += new System.EventHandler(this.TinkeringAudioForm_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btn_GenerateMelody;
        private System.Windows.Forms.Button btn_SaveMelody;
        private System.Windows.Forms.Button btn_GenerateWhiteNoise;
        private System.Windows.Forms.Button Villagebtn;
        private System.Windows.Forms.Button Forestbtn;
        private System.Windows.Forms.Button Cavebtn;
        private System.Windows.Forms.Button Oceanbtn;
    }
}

