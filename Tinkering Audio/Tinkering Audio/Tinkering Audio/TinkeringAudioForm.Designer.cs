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
            this.btn_GenerateMelody = new System.Windows.Forms.Button();
            this.btn_SaveMelody = new System.Windows.Forms.Button();
            this.btn_GenerateWhiteNoise = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // btn_GenerateMelody
            // 
            this.btn_GenerateMelody.Location = new System.Drawing.Point(275, 215);
            this.btn_GenerateMelody.Name = "btn_GenerateMelody";
            this.btn_GenerateMelody.Size = new System.Drawing.Size(265, 23);
            this.btn_GenerateMelody.TabIndex = 0;
            this.btn_GenerateMelody.Text = "Generate Melody";
            this.btn_GenerateMelody.UseVisualStyleBackColor = true;
            this.btn_GenerateMelody.Click += new System.EventHandler(this.btn_GenerateMelody_Click);
            // 
            // btn_SaveMelody
            // 
            this.btn_SaveMelody.Location = new System.Drawing.Point(275, 273);
            this.btn_SaveMelody.Name = "btn_SaveMelody";
            this.btn_SaveMelody.Size = new System.Drawing.Size(265, 23);
            this.btn_SaveMelody.TabIndex = 1;
            this.btn_SaveMelody.Text = "Save Melody";
            this.btn_SaveMelody.UseVisualStyleBackColor = true;
            this.btn_SaveMelody.Click += new System.EventHandler(this.btn_SaveMelody_Click);
            // 
            // btn_GenerateWhiteNoise
            // 
            this.btn_GenerateWhiteNoise.Location = new System.Drawing.Point(275, 244);
            this.btn_GenerateWhiteNoise.Name = "btn_GenerateWhiteNoise";
            this.btn_GenerateWhiteNoise.Size = new System.Drawing.Size(265, 23);
            this.btn_GenerateWhiteNoise.TabIndex = 2;
            this.btn_GenerateWhiteNoise.Text = "Generate White Noise";
            this.btn_GenerateWhiteNoise.UseVisualStyleBackColor = true;
            this.btn_GenerateWhiteNoise.Click += new System.EventHandler(this.btn_GenerateWhiteNoise_Click);
            // 
            // TinkeringAudioForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
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
    }
}

