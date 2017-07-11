namespace Azar
{
    partial class AddParticipants
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(AddParticipants));
            this.richTextBox_participants = new System.Windows.Forms.RichTextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.button_Exit = new System.Windows.Forms.PictureBox();
            this.button_Mini = new System.Windows.Forms.PictureBox();
            this.topBorder = new System.Windows.Forms.Label();
            this.button_AddParticipants = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.button_Exit)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.button_Mini)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.button_AddParticipants)).BeginInit();
            this.SuspendLayout();
            // 
            // richTextBox_participants
            // 
            this.richTextBox_participants.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(56)))), ((int)(((byte)(56)))), ((int)(((byte)(64)))));
            this.richTextBox_participants.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.richTextBox_participants.ForeColor = System.Drawing.Color.White;
            this.richTextBox_participants.Location = new System.Drawing.Point(23, 56);
            this.richTextBox_participants.Name = "richTextBox_participants";
            this.richTextBox_participants.Size = new System.Drawing.Size(220, 259);
            this.richTextBox_participants.TabIndex = 0;
            this.richTextBox_participants.Text = "";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.ForeColor = System.Drawing.Color.White;
            this.label1.Location = new System.Drawing.Point(20, 40);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(100, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "Dodaj uczestników:";
            // 
            // button_Exit
            // 
            this.button_Exit.Image = global::Azar.Properties.Resources.exit1;
            this.button_Exit.Location = new System.Drawing.Point(215, 0);
            this.button_Exit.Name = "button_Exit";
            this.button_Exit.Size = new System.Drawing.Size(45, 20);
            this.button_Exit.TabIndex = 21;
            this.button_Exit.TabStop = false;
            this.button_Exit.MouseDown += new System.Windows.Forms.MouseEventHandler(this.button_Exit_MouseDown);
            this.button_Exit.MouseUp += new System.Windows.Forms.MouseEventHandler(this.button_Exit_MouseUp);
            // 
            // button_Mini
            // 
            this.button_Mini.Image = global::Azar.Properties.Resources.mini1;
            this.button_Mini.Location = new System.Drawing.Point(183, 0);
            this.button_Mini.Name = "button_Mini";
            this.button_Mini.Size = new System.Drawing.Size(26, 20);
            this.button_Mini.TabIndex = 20;
            this.button_Mini.TabStop = false;
            this.button_Mini.MouseDown += new System.Windows.Forms.MouseEventHandler(this.button_Mini_MouseDown);
            this.button_Mini.MouseUp += new System.Windows.Forms.MouseEventHandler(this.button_Mini_MouseUp);
            // 
            // topBorder
            // 
            this.topBorder.AutoSize = true;
            this.topBorder.BackColor = System.Drawing.Color.Transparent;
            this.topBorder.Location = new System.Drawing.Point(0, 0);
            this.topBorder.MaximumSize = new System.Drawing.Size(183, 31);
            this.topBorder.MinimumSize = new System.Drawing.Size(183, 31);
            this.topBorder.Name = "topBorder";
            this.topBorder.Size = new System.Drawing.Size(183, 31);
            this.topBorder.TabIndex = 22;
            this.topBorder.MouseDown += new System.Windows.Forms.MouseEventHandler(this.topBorder_MouseDown);
            this.topBorder.MouseMove += new System.Windows.Forms.MouseEventHandler(this.topBorder_MouseMove);
            this.topBorder.MouseUp += new System.Windows.Forms.MouseEventHandler(this.topBorder_MouseUp);
            // 
            // button_AddParticipants
            // 
            this.button_AddParticipants.Image = global::Azar.Properties.Resources.buttonAddParticipants;
            this.button_AddParticipants.Location = new System.Drawing.Point(23, 321);
            this.button_AddParticipants.Name = "button_AddParticipants";
            this.button_AddParticipants.Size = new System.Drawing.Size(220, 42);
            this.button_AddParticipants.TabIndex = 23;
            this.button_AddParticipants.TabStop = false;
            this.button_AddParticipants.MouseDown += new System.Windows.Forms.MouseEventHandler(this.button_AddParticipants_MouseDown);
            this.button_AddParticipants.MouseUp += new System.Windows.Forms.MouseEventHandler(this.button_AddParticipants_MouseUp);
            // 
            // AddParticipants
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(45)))), ((int)(((byte)(45)))), ((int)(((byte)(51)))));
            this.BackgroundImage = global::Azar.Properties.Resources.addParticipantsBackground;
            this.ClientSize = new System.Drawing.Size(266, 386);
            this.Controls.Add(this.button_AddParticipants);
            this.Controls.Add(this.topBorder);
            this.Controls.Add(this.button_Exit);
            this.Controls.Add(this.button_Mini);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.richTextBox_participants);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(266, 386);
            this.MinimumSize = new System.Drawing.Size(266, 386);
            this.Name = "AddParticipants";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Dodaj uczestników - Azar";
            this.TopMost = true;
            ((System.ComponentModel.ISupportInitialize)(this.button_Exit)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.button_Mini)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.button_AddParticipants)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.RichTextBox richTextBox_participants;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.PictureBox button_Exit;
        private System.Windows.Forms.PictureBox button_Mini;
        private System.Windows.Forms.Label topBorder;
        private System.Windows.Forms.PictureBox button_AddParticipants;
    }
}