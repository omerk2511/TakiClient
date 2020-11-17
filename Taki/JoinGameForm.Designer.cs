namespace Taki
{
    partial class JoinGameForm
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
            this.label3 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.GameIDInput = new System.Windows.Forms.TextBox();
            this.JoinButton = new System.Windows.Forms.Button();
            this.label4 = new System.Windows.Forms.Label();
            this.PasswordInput = new System.Windows.Forms.TextBox();
            this.ErrorLabel = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(177)));
            this.label3.Location = new System.Drawing.Point(22, 127);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(290, 29);
            this.label3.TabIndex = 7;
            this.label3.Text = "Please enter the game ID:";
            this.label3.Click += new System.EventHandler(this.label3_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 30F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(177)));
            this.label1.Location = new System.Drawing.Point(284, 35);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(237, 46);
            this.label1.TabIndex = 8;
            this.label1.Text = "Join a game";
            // 
            // GameIDInput
            // 
            this.GameIDInput.Font = new System.Drawing.Font("Microsoft Sans Serif", 16F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(177)));
            this.GameIDInput.Location = new System.Drawing.Point(400, 127);
            this.GameIDInput.Name = "GameIDInput";
            this.GameIDInput.Size = new System.Drawing.Size(393, 32);
            this.GameIDInput.TabIndex = 10;
            // 
            // JoinButton
            // 
            this.JoinButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 20F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(177)));
            this.JoinButton.Location = new System.Drawing.Point(271, 276);
            this.JoinButton.Name = "JoinButton";
            this.JoinButton.Size = new System.Drawing.Size(241, 109);
            this.JoinButton.TabIndex = 11;
            this.JoinButton.Text = "Join a game";
            this.JoinButton.UseVisualStyleBackColor = true;
            this.JoinButton.Click += new System.EventHandler(this.JoinButton_Click);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(177)));
            this.label4.Location = new System.Drawing.Point(22, 174);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(372, 29);
            this.label4.TabIndex = 12;
            this.label4.Text = "Please enter the game password:";
            // 
            // PasswordInput
            // 
            this.PasswordInput.Font = new System.Drawing.Font("Microsoft Sans Serif", 16F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(177)));
            this.PasswordInput.Location = new System.Drawing.Point(400, 174);
            this.PasswordInput.Name = "PasswordInput";
            this.PasswordInput.Size = new System.Drawing.Size(393, 32);
            this.PasswordInput.TabIndex = 13;
            // 
            // ErrorLabel
            // 
            this.ErrorLabel.AutoSize = true;
            this.ErrorLabel.BackColor = System.Drawing.Color.Red;
            this.ErrorLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(177)));
            this.ErrorLabel.Location = new System.Drawing.Point(22, 219);
            this.ErrorLabel.Name = "ErrorLabel";
            this.ErrorLabel.Size = new System.Drawing.Size(0, 29);
            this.ErrorLabel.TabIndex = 14;
            // 
            // JoinGameForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 406);
            this.Controls.Add(this.ErrorLabel);
            this.Controls.Add(this.PasswordInput);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.JoinButton);
            this.Controls.Add(this.GameIDInput);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.label3);
            this.Name = "JoinGameForm";
            this.Text = "Join Game";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox GameIDInput;
        private System.Windows.Forms.Button JoinButton;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox PasswordInput;
        private System.Windows.Forms.Label ErrorLabel;
    }
}