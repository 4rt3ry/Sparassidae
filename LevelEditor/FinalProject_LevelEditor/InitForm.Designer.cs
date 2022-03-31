
namespace FinalProject_LevelEditor
{
    partial class InitForm
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
            this.LoadFileButton = new System.Windows.Forms.Button();
            this.CreateLevelButton = new System.Windows.Forms.Button();
            this.WidthTextBox = new System.Windows.Forms.TextBox();
            this.HeightTextBox = new System.Windows.Forms.TextBox();
            this.WidthLabel = new System.Windows.Forms.Label();
            this.HeightLabel = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // LoadFileButton
            // 
            this.LoadFileButton.Location = new System.Drawing.Point(12, 329);
            this.LoadFileButton.Name = "LoadFileButton";
            this.LoadFileButton.Size = new System.Drawing.Size(135, 63);
            this.LoadFileButton.TabIndex = 0;
            this.LoadFileButton.Text = "Choose File";
            this.LoadFileButton.UseVisualStyleBackColor = true;
            this.LoadFileButton.Click += new System.EventHandler(this.LoadFileButton_Click);
            // 
            // CreateLevelButton
            // 
            this.CreateLevelButton.Location = new System.Drawing.Point(203, 329);
            this.CreateLevelButton.Name = "CreateLevelButton";
            this.CreateLevelButton.Size = new System.Drawing.Size(135, 63);
            this.CreateLevelButton.TabIndex = 1;
            this.CreateLevelButton.Text = "Create Level";
            this.CreateLevelButton.UseVisualStyleBackColor = true;
            this.CreateLevelButton.Click += new System.EventHandler(this.CreateLevelButton_Click);
            // 
            // WidthTextBox
            // 
            this.WidthTextBox.Location = new System.Drawing.Point(99, 53);
            this.WidthTextBox.Name = "WidthTextBox";
            this.WidthTextBox.Size = new System.Drawing.Size(146, 23);
            this.WidthTextBox.TabIndex = 3;
            this.WidthTextBox.Text = "50";
            // 
            // HeightTextBox
            // 
            this.HeightTextBox.Location = new System.Drawing.Point(99, 114);
            this.HeightTextBox.Name = "HeightTextBox";
            this.HeightTextBox.Size = new System.Drawing.Size(146, 23);
            this.HeightTextBox.TabIndex = 4;
            this.HeightTextBox.Text = "50";
            // 
            // WidthLabel
            // 
            this.WidthLabel.AutoSize = true;
            this.WidthLabel.Location = new System.Drawing.Point(150, 35);
            this.WidthLabel.Name = "WidthLabel";
            this.WidthLabel.Size = new System.Drawing.Size(39, 15);
            this.WidthLabel.TabIndex = 5;
            this.WidthLabel.Text = "Width";
            // 
            // HeightLabel
            // 
            this.HeightLabel.AutoSize = true;
            this.HeightLabel.Location = new System.Drawing.Point(146, 96);
            this.HeightLabel.Name = "HeightLabel";
            this.HeightLabel.Size = new System.Drawing.Size(43, 15);
            this.HeightLabel.TabIndex = 6;
            this.HeightLabel.Text = "Height";
            // 
            // InitForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(350, 404);
            this.Controls.Add(this.HeightLabel);
            this.Controls.Add(this.WidthLabel);
            this.Controls.Add(this.HeightTextBox);
            this.Controls.Add(this.WidthTextBox);
            this.Controls.Add(this.CreateLevelButton);
            this.Controls.Add(this.LoadFileButton);
            this.Name = "InitForm";
            this.Text = "Create or open a level";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button LoadFileButton;
        private System.Windows.Forms.Button CreateLevelButton;
        private System.Windows.Forms.TextBox WidthTextBox;
        private System.Windows.Forms.TextBox HeightTextBox;
        private System.Windows.Forms.Label WidthLabel;
        private System.Windows.Forms.Label HeightLabel;
    }
}

