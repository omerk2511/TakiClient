namespace Taki
{
    partial class WaitGameForm
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
            this.headerLabel = new System.Windows.Forms.Label();
            this.memeberLabel = new System.Windows.Forms.Label();
            this.listBox1 = new System.Windows.Forms.ListBox();
            this.SuspendLayout();
            // 
            // headerLabel
            // 
            this.headerLabel.AutoSize = true;
            this.headerLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 30F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(177)));
            this.headerLabel.Location = new System.Drawing.Point(136, 45);
            this.headerLabel.Name = "headerLabel";
            this.headerLabel.Size = new System.Drawing.Size(530, 46);
            this.headerLabel.TabIndex = 14;
            this.headerLabel.Text = "Waiting for the host to start...";
            // 
            // memeberLabel
            // 
            this.memeberLabel.AutoSize = true;
            this.memeberLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 26F);
            this.memeberLabel.Location = new System.Drawing.Point(285, 128);
            this.memeberLabel.Name = "memeberLabel";
            this.memeberLabel.Size = new System.Drawing.Size(230, 39);
            this.memeberLabel.TabIndex = 15;
            this.memeberLabel.Text = "Members: 2/4";
            this.memeberLabel.Click += new System.EventHandler(this.memeberLabel_Click);
            // 
            // listBox1
            // 
            this.listBox1.Font = new System.Drawing.Font("Microsoft Sans Serif", 16.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.listBox1.FormattingEnabled = true;
            this.listBox1.ItemHeight = 26;
            this.listBox1.Location = new System.Drawing.Point(9, 101);
            this.listBox1.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.listBox1.Name = "listBox1";
            this.listBox1.Size = new System.Drawing.Size(240, 160);
            this.listBox1.TabIndex = 17;
            // 
            // WaitGameForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 290);
            this.Controls.Add(this.listBox1);
            this.Controls.Add(this.memeberLabel);
            this.Controls.Add(this.headerLabel);
            this.Name = "WaitGameForm";
            this.Text = "WaitGameForm";
            this.Load += new System.EventHandler(this.WaitGameForm_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label headerLabel;
        private System.Windows.Forms.Label memeberLabel;
        private System.Windows.Forms.ListBox listBox1;
    }
}