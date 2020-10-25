namespace Taki
{
    partial class StartGameForm
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
            this.memeberLabel = new System.Windows.Forms.Label();
            this.StartButton = new System.Windows.Forms.Button();
            this.headerLabel = new System.Windows.Forms.Label();
            this.gameIDLabel = new System.Windows.Forms.Label();
            this.backgroundWorker1 = new System.ComponentModel.BackgroundWorker();
            this.listBox1 = new System.Windows.Forms.ListBox();
            this.SuspendLayout();
            // 
            // memeberLabel
            // 
            this.memeberLabel.AutoSize = true;
            this.memeberLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 26F);
            this.memeberLabel.Location = new System.Drawing.Point(379, 213);
            this.memeberLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.memeberLabel.Name = "memeberLabel";
            this.memeberLabel.Size = new System.Drawing.Size(290, 52);
            this.memeberLabel.TabIndex = 2;
            this.memeberLabel.Text = "Members: 1/4";
            this.memeberLabel.Click += new System.EventHandler(this.label1_Click);
            // 
            // StartButton
            // 
            this.StartButton.Enabled = false;
            this.StartButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 20F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(177)));
            this.StartButton.Location = new System.Drawing.Point(388, 287);
            this.StartButton.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.StartButton.Name = "StartButton";
            this.StartButton.Size = new System.Drawing.Size(297, 130);
            this.StartButton.TabIndex = 12;
            this.StartButton.Text = "Start game";
            this.StartButton.UseVisualStyleBackColor = true;
            this.StartButton.Click += new System.EventHandler(this.StartButton_Click);
            // 
            // headerLabel
            // 
            this.headerLabel.AutoSize = true;
            this.headerLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 30F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(177)));
            this.headerLabel.Location = new System.Drawing.Point(196, 34);
            this.headerLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.headerLabel.Name = "headerLabel";
            this.headerLabel.Size = new System.Drawing.Size(609, 58);
            this.headerLabel.TabIndex = 13;
            this.headerLabel.Text = "Waiting for other players...";
            this.headerLabel.Click += new System.EventHandler(this.label2_Click);
            // 
            // gameIDLabel
            // 
            this.gameIDLabel.AutoSize = true;
            this.gameIDLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 26F);
            this.gameIDLabel.Location = new System.Drawing.Point(379, 129);
            this.gameIDLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.gameIDLabel.Name = "gameIDLabel";
            this.gameIDLabel.Size = new System.Drawing.Size(222, 52);
            this.gameIDLabel.TabIndex = 14;
            this.gameIDLabel.Text = "Game ID: ";
            this.gameIDLabel.Click += new System.EventHandler(this.gameIDLabel_Click);
            // 
            // backgroundWorker1
            // 
            this.backgroundWorker1.DoWork += new System.ComponentModel.DoWorkEventHandler(this.backgroundWorker1_DoWork);
            // 
            // listBox1
            // 
            this.listBox1.Font = new System.Drawing.Font("Microsoft Sans Serif", 16.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.listBox1.FormattingEnabled = true;
            this.listBox1.ItemHeight = 31;
            this.listBox1.Location = new System.Drawing.Point(33, 95);
            this.listBox1.Name = "listBox1";
            this.listBox1.Size = new System.Drawing.Size(246, 283);
            this.listBox1.TabIndex = 16;
            // 
            // StartGameForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1067, 444);
            this.Controls.Add(this.listBox1);
            this.Controls.Add(this.gameIDLabel);
            this.Controls.Add(this.headerLabel);
            this.Controls.Add(this.StartButton);
            this.Controls.Add(this.memeberLabel);
            this.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.Name = "StartGameForm";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.StartGameForm_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label memeberLabel;
        private System.Windows.Forms.Button StartButton;
        private System.Windows.Forms.Label headerLabel;
        private System.Windows.Forms.Label gameIDLabel;
        private System.ComponentModel.BackgroundWorker backgroundWorker1;
        private System.Windows.Forms.ListBox listBox1;
    }
}