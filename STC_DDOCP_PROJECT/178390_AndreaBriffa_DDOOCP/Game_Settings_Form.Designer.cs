namespace _178390_AndreaBriffa_DDOOCP
{
    partial class Game_Settings_Form
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
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.lbl_Min = new System.Windows.Forms.Label();
            this.lbl_Max = new System.Windows.Forms.Label();
            this.tf_Input_Pairs = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.cmb_Lang1 = new System.Windows.Forms.ComboBox();
            this.cmb_Lang2 = new System.Windows.Forms.ComboBox();
            this.btn_Submit_Settings = new System.Windows.Forms.Button();
            this.lbl_Alert = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.Color.White;
            this.label1.Location = new System.Drawing.Point(40, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(239, 25);
            this.label1.TabIndex = 0;
            this.label1.Text = "Select Game Settings";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.ForeColor = System.Drawing.Color.White;
            this.label2.Location = new System.Drawing.Point(12, 103);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(119, 16);
            this.label2.TabIndex = 1;
            this.label2.Text = "Number of Pairs";
            // 
            // lbl_Min
            // 
            this.lbl_Min.AutoSize = true;
            this.lbl_Min.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbl_Min.ForeColor = System.Drawing.Color.White;
            this.lbl_Min.Location = new System.Drawing.Point(137, 104);
            this.lbl_Min.Name = "lbl_Min";
            this.lbl_Min.Size = new System.Drawing.Size(27, 15);
            this.lbl_Min.TabIndex = 2;
            this.lbl_Min.Text = "1 <";
            // 
            // lbl_Max
            // 
            this.lbl_Max.AutoSize = true;
            this.lbl_Max.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbl_Max.ForeColor = System.Drawing.Color.White;
            this.lbl_Max.Location = new System.Drawing.Point(235, 104);
            this.lbl_Max.Name = "lbl_Max";
            this.lbl_Max.Size = new System.Drawing.Size(54, 15);
            this.lbl_Max.TabIndex = 3;
            this.lbl_Max.Text = "<= max";
            // 
            // tf_Input_Pairs
            // 
            this.tf_Input_Pairs.BackColor = System.Drawing.Color.White;
            this.tf_Input_Pairs.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.tf_Input_Pairs.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tf_Input_Pairs.ForeColor = System.Drawing.Color.RoyalBlue;
            this.tf_Input_Pairs.Location = new System.Drawing.Point(174, 104);
            this.tf_Input_Pairs.Multiline = true;
            this.tf_Input_Pairs.Name = "tf_Input_Pairs";
            this.tf_Input_Pairs.Size = new System.Drawing.Size(55, 16);
            this.tf_Input_Pairs.TabIndex = 1;
            this.tf_Input_Pairs.Text = "2";
            this.tf_Input_Pairs.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.ForeColor = System.Drawing.Color.White;
            this.label3.Location = new System.Drawing.Point(13, 156);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(111, 16);
            this.label3.TabIndex = 5;
            this.label3.Text = "First Language";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.ForeColor = System.Drawing.Color.White;
            this.label4.Location = new System.Drawing.Point(171, 156);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(134, 16);
            this.label4.TabIndex = 6;
            this.label4.Text = "Second Language";
            // 
            // cmb_Lang1
            // 
            this.cmb_Lang1.BackColor = System.Drawing.Color.LightGray;
            this.cmb_Lang1.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmb_Lang1.ForeColor = System.Drawing.Color.Black;
            this.cmb_Lang1.FormattingEnabled = true;
            this.cmb_Lang1.Location = new System.Drawing.Point(16, 191);
            this.cmb_Lang1.Name = "cmb_Lang1";
            this.cmb_Lang1.Size = new System.Drawing.Size(131, 21);
            this.cmb_Lang1.TabIndex = 2;
            // 
            // cmb_Lang2
            // 
            this.cmb_Lang2.BackColor = System.Drawing.Color.LightGray;
            this.cmb_Lang2.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmb_Lang2.ForeColor = System.Drawing.Color.Black;
            this.cmb_Lang2.FormattingEnabled = true;
            this.cmb_Lang2.Location = new System.Drawing.Point(174, 191);
            this.cmb_Lang2.Name = "cmb_Lang2";
            this.cmb_Lang2.Size = new System.Drawing.Size(131, 21);
            this.cmb_Lang2.TabIndex = 3;
            // 
            // btn_Submit_Settings
            // 
            this.btn_Submit_Settings.BackColor = System.Drawing.Color.White;
            this.btn_Submit_Settings.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btn_Submit_Settings.ForeColor = System.Drawing.Color.RoyalBlue;
            this.btn_Submit_Settings.Location = new System.Drawing.Point(15, 238);
            this.btn_Submit_Settings.Name = "btn_Submit_Settings";
            this.btn_Submit_Settings.Size = new System.Drawing.Size(290, 29);
            this.btn_Submit_Settings.TabIndex = 4;
            this.btn_Submit_Settings.Text = "Submit Settings";
            this.btn_Submit_Settings.UseVisualStyleBackColor = false;
            // 
            // lbl_Alert
            // 
            this.lbl_Alert.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.lbl_Alert.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbl_Alert.ForeColor = System.Drawing.Color.Red;
            this.lbl_Alert.Location = new System.Drawing.Point(16, 125);
            this.lbl_Alert.Name = "lbl_Alert";
            this.lbl_Alert.Size = new System.Drawing.Size(289, 21);
            this.lbl_Alert.TabIndex = 10;
            this.lbl_Alert.Text = "// alert label";
            this.lbl_Alert.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // Game_Settings_Form
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Tan;
            this.ClientSize = new System.Drawing.Size(317, 283);
            this.Controls.Add(this.lbl_Alert);
            this.Controls.Add(this.btn_Submit_Settings);
            this.Controls.Add(this.cmb_Lang2);
            this.Controls.Add(this.cmb_Lang1);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.tf_Input_Pairs);
            this.Controls.Add(this.lbl_Max);
            this.Controls.Add(this.lbl_Min);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.MaximumSize = new System.Drawing.Size(333, 322);
            this.MinimumSize = new System.Drawing.Size(333, 322);
            this.Name = "Game_Settings_Form";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Game_Settings_Form";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label lbl_Min;
        private System.Windows.Forms.Label lbl_Max;
        private System.Windows.Forms.TextBox tf_Input_Pairs;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.ComboBox cmb_Lang1;
        private System.Windows.Forms.ComboBox cmb_Lang2;
        private System.Windows.Forms.Button btn_Submit_Settings;
        private System.Windows.Forms.Label lbl_Alert;
    }
}