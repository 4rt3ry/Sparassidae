
namespace FinalProject_LevelEditor
{
    partial class LevelEditor
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
            System.Windows.Forms.Label HeightLabel;
            this.SaveButton = new System.Windows.Forms.Button();
            this.Level = new System.Windows.Forms.GroupBox();
            this.PlaceMenu = new System.Windows.Forms.GroupBox();
            this.WidthLabel = new System.Windows.Forms.Label();
            this.WidthTextBox = new System.Windows.Forms.TextBox();
            this.HeightTextBox = new System.Windows.Forms.TextBox();
            this.NewPieceButton = new System.Windows.Forms.Button();
            this.ExitLabel = new System.Windows.Forms.Label();
            this.ExitButton = new System.Windows.Forms.Button();
            this.ObjectiveLabel = new System.Windows.Forms.Label();
            this.ObjectiveButton = new System.Windows.Forms.Button();
            this.SpawnLabel = new System.Windows.Forms.Label();
            this.SpawnButton = new System.Windows.Forms.Button();
            this.EnemyLabel = new System.Windows.Forms.Label();
            this.EnemyButton = new System.Windows.Forms.Button();
            this.WallLabel = new System.Windows.Forms.Label();
            this.WallButton = new System.Windows.Forms.Button();
            this.EditMenu = new System.Windows.Forms.ListBox();
            HeightLabel = new System.Windows.Forms.Label();
            this.PlaceMenu.SuspendLayout();
            this.SuspendLayout();
            // 
            // HeightLabel
            // 
            HeightLabel.AutoSize = true;
            HeightLabel.Location = new System.Drawing.Point(12, 596);
            HeightLabel.Name = "HeightLabel";
            HeightLabel.Size = new System.Drawing.Size(43, 15);
            HeightLabel.TabIndex = 15;
            HeightLabel.Text = "Height";
            // 
            // SaveButton
            // 
            this.SaveButton.Location = new System.Drawing.Point(1064, 654);
            this.SaveButton.Name = "SaveButton";
            this.SaveButton.Size = new System.Drawing.Size(131, 55);
            this.SaveButton.TabIndex = 10;
            this.SaveButton.Text = "Save File";
            this.SaveButton.UseVisualStyleBackColor = true;
            this.SaveButton.Click += new System.EventHandler(this.SaveButton_Click);
            // 
            // Level
            // 
            this.Level.Location = new System.Drawing.Point(265, 12);
            this.Level.Name = "Level";
            this.Level.Size = new System.Drawing.Size(683, 697);
            this.Level.TabIndex = 2;
            this.Level.TabStop = false;
            this.Level.Enter += new System.EventHandler(this.Level_Enter);
            this.Level.PreviewKeyDown += new System.Windows.Forms.PreviewKeyDownEventHandler(this.Level_PreviewKeyDown);
            // 
            // PlaceMenu
            // 
            this.PlaceMenu.Controls.Add(HeightLabel);
            this.PlaceMenu.Controls.Add(this.WidthLabel);
            this.PlaceMenu.Controls.Add(this.WidthTextBox);
            this.PlaceMenu.Controls.Add(this.HeightTextBox);
            this.PlaceMenu.Controls.Add(this.NewPieceButton);
            this.PlaceMenu.Controls.Add(this.ExitLabel);
            this.PlaceMenu.Controls.Add(this.ExitButton);
            this.PlaceMenu.Controls.Add(this.ObjectiveLabel);
            this.PlaceMenu.Controls.Add(this.ObjectiveButton);
            this.PlaceMenu.Controls.Add(this.SpawnLabel);
            this.PlaceMenu.Controls.Add(this.SpawnButton);
            this.PlaceMenu.Controls.Add(this.EnemyLabel);
            this.PlaceMenu.Controls.Add(this.EnemyButton);
            this.PlaceMenu.Controls.Add(this.WallLabel);
            this.PlaceMenu.Controls.Add(this.WallButton);
            this.PlaceMenu.Location = new System.Drawing.Point(12, 12);
            this.PlaceMenu.Name = "PlaceMenu";
            this.PlaceMenu.Size = new System.Drawing.Size(247, 697);
            this.PlaceMenu.TabIndex = 3;
            this.PlaceMenu.TabStop = false;
            // 
            // WidthLabel
            // 
            this.WidthLabel.AutoSize = true;
            this.WidthLabel.Location = new System.Drawing.Point(13, 553);
            this.WidthLabel.Name = "WidthLabel";
            this.WidthLabel.Size = new System.Drawing.Size(39, 15);
            this.WidthLabel.TabIndex = 14;
            this.WidthLabel.Text = "Width";
            // 
            // WidthTextBox
            // 
            this.WidthTextBox.Location = new System.Drawing.Point(75, 550);
            this.WidthTextBox.Name = "WidthTextBox";
            this.WidthTextBox.Size = new System.Drawing.Size(100, 23);
            this.WidthTextBox.TabIndex = 13;
            this.WidthTextBox.Text = "1";
            // 
            // HeightTextBox
            // 
            this.HeightTextBox.Location = new System.Drawing.Point(75, 593);
            this.HeightTextBox.Name = "HeightTextBox";
            this.HeightTextBox.Size = new System.Drawing.Size(100, 23);
            this.HeightTextBox.TabIndex = 12;
            this.HeightTextBox.Text = "1";
            // 
            // NewPieceButton
            // 
            this.NewPieceButton.Location = new System.Drawing.Point(58, 636);
            this.NewPieceButton.Name = "NewPieceButton";
            this.NewPieceButton.Size = new System.Drawing.Size(131, 55);
            this.NewPieceButton.TabIndex = 11;
            this.NewPieceButton.Text = "Create Piece";
            this.NewPieceButton.UseVisualStyleBackColor = true;
            this.NewPieceButton.Click += new System.EventHandler(this.NewPieceButton_Click);
            // 
            // ExitLabel
            // 
            this.ExitLabel.AutoSize = true;
            this.ExitLabel.Location = new System.Drawing.Point(199, 455);
            this.ExitLabel.Name = "ExitLabel";
            this.ExitLabel.Size = new System.Drawing.Size(26, 15);
            this.ExitLabel.TabIndex = 9;
            this.ExitLabel.Text = "Exit";
            // 
            // ExitButton
            // 
            this.ExitButton.BackColor = System.Drawing.Color.Green;
            this.ExitButton.ForeColor = System.Drawing.SystemColors.ControlText;
            this.ExitButton.Location = new System.Drawing.Point(12, 425);
            this.ExitButton.Name = "ExitButton";
            this.ExitButton.Size = new System.Drawing.Size(75, 75);
            this.ExitButton.TabIndex = 8;
            this.ExitButton.UseVisualStyleBackColor = false;
            this.ExitButton.Click += new System.EventHandler(this.ExitButton_Click);
            // 
            // ObjectiveLabel
            // 
            this.ObjectiveLabel.AutoSize = true;
            this.ObjectiveLabel.Location = new System.Drawing.Point(168, 354);
            this.ObjectiveLabel.Name = "ObjectiveLabel";
            this.ObjectiveLabel.Size = new System.Drawing.Size(57, 15);
            this.ObjectiveLabel.TabIndex = 7;
            this.ObjectiveLabel.Text = "Objective";
            // 
            // ObjectiveButton
            // 
            this.ObjectiveButton.BackColor = System.Drawing.Color.SteelBlue;
            this.ObjectiveButton.ForeColor = System.Drawing.SystemColors.ControlText;
            this.ObjectiveButton.Location = new System.Drawing.Point(12, 324);
            this.ObjectiveButton.Name = "ObjectiveButton";
            this.ObjectiveButton.Size = new System.Drawing.Size(75, 75);
            this.ObjectiveButton.TabIndex = 6;
            this.ObjectiveButton.UseVisualStyleBackColor = false;
            this.ObjectiveButton.Click += new System.EventHandler(this.ObjectiveButton_Click);
            // 
            // SpawnLabel
            // 
            this.SpawnLabel.AutoSize = true;
            this.SpawnLabel.Location = new System.Drawing.Point(148, 254);
            this.SpawnLabel.Name = "SpawnLabel";
            this.SpawnLabel.Size = new System.Drawing.Size(77, 15);
            this.SpawnLabel.TabIndex = 5;
            this.SpawnLabel.Text = "Player Spawn";
            // 
            // SpawnButton
            // 
            this.SpawnButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(192)))), ((int)(((byte)(0)))));
            this.SpawnButton.ForeColor = System.Drawing.SystemColors.ControlText;
            this.SpawnButton.Location = new System.Drawing.Point(12, 224);
            this.SpawnButton.Name = "SpawnButton";
            this.SpawnButton.Size = new System.Drawing.Size(75, 75);
            this.SpawnButton.TabIndex = 4;
            this.SpawnButton.UseVisualStyleBackColor = false;
            this.SpawnButton.Click += new System.EventHandler(this.SpawnButton_Click);
            // 
            // EnemyLabel
            // 
            this.EnemyLabel.AutoSize = true;
            this.EnemyLabel.Location = new System.Drawing.Point(182, 153);
            this.EnemyLabel.Name = "EnemyLabel";
            this.EnemyLabel.Size = new System.Drawing.Size(43, 15);
            this.EnemyLabel.TabIndex = 3;
            this.EnemyLabel.Text = "Enemy";
            // 
            // EnemyButton
            // 
            this.EnemyButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.EnemyButton.ForeColor = System.Drawing.SystemColors.ControlText;
            this.EnemyButton.Location = new System.Drawing.Point(12, 123);
            this.EnemyButton.Name = "EnemyButton";
            this.EnemyButton.Size = new System.Drawing.Size(75, 75);
            this.EnemyButton.TabIndex = 2;
            this.EnemyButton.UseVisualStyleBackColor = false;
            this.EnemyButton.Click += new System.EventHandler(this.EnemyButton_Click);
            // 
            // WallLabel
            // 
            this.WallLabel.AutoSize = true;
            this.WallLabel.Location = new System.Drawing.Point(195, 52);
            this.WallLabel.Name = "WallLabel";
            this.WallLabel.Size = new System.Drawing.Size(30, 15);
            this.WallLabel.TabIndex = 1;
            this.WallLabel.Text = "Wall";
            // 
            // WallButton
            // 
            this.WallButton.BackColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.WallButton.ForeColor = System.Drawing.SystemColors.ControlText;
            this.WallButton.Location = new System.Drawing.Point(12, 22);
            this.WallButton.Name = "WallButton";
            this.WallButton.Size = new System.Drawing.Size(75, 75);
            this.WallButton.TabIndex = 0;
            this.WallButton.UseVisualStyleBackColor = false;
            this.WallButton.Click += new System.EventHandler(this.WallButton_Click);
            // 
            // EditMenu
            // 
            this.EditMenu.FormattingEnabled = true;
            this.EditMenu.ItemHeight = 15;
            this.EditMenu.Location = new System.Drawing.Point(954, 15);
            this.EditMenu.Name = "EditMenu";
            this.EditMenu.Size = new System.Drawing.Size(241, 634);
            this.EditMenu.Sorted = true;
            this.EditMenu.TabIndex = 11;
            this.EditMenu.SelectedIndexChanged += new System.EventHandler(this.EditMenu_SelectedIndexChanged);
            this.EditMenu.KeyDown += new System.Windows.Forms.KeyEventHandler(this.EditMenu_KeyDown);
            this.EditMenu.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.EditMenu_KeyPress);
            // 
            // LevelEditor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1207, 721);
            this.Controls.Add(this.EditMenu);
            this.Controls.Add(this.SaveButton);
            this.Controls.Add(this.PlaceMenu);
            this.Controls.Add(this.Level);
            this.Name = "LevelEditor";
            this.Text = "LevelEditor";
            this.PlaceMenu.ResumeLayout(false);
            this.PlaceMenu.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox PlacementMenu;
        private System.Windows.Forms.GroupBox Level;
        private System.Windows.Forms.GroupBox PlaceMenu;
        private System.Windows.Forms.Button WallButton;
        private System.Windows.Forms.Label WallLabel;
        private System.Windows.Forms.Label ExitLabel;
        private System.Windows.Forms.Button ExitButton;
        private System.Windows.Forms.Label ObjectiveLabel;
        private System.Windows.Forms.Button ObjectiveButton;
        private System.Windows.Forms.Label SpawnLabel;
        private System.Windows.Forms.Button SpawnButton;
        private System.Windows.Forms.Label EnemyLabel;
        private System.Windows.Forms.Button EnemyButton;
        private System.Windows.Forms.Button SaveButton;
        private System.Windows.Forms.Button NewPieceButton;
        private System.Windows.Forms.Label WidthLabel;
        private System.Windows.Forms.TextBox WidthTextBox;
        private System.Windows.Forms.TextBox HeightTextBox;
        private System.Windows.Forms.ListBox EditMenu;
    }
}