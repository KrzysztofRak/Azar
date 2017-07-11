namespace Azar
{
    partial class AddAwards
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(AddAwards));
            this.label_AwardName = new System.Windows.Forms.Label();
            this.label_AwardAmount = new System.Windows.Forms.Label();
            this.label_AwardPriority = new System.Windows.Forms.Label();
            this.textBox_AwardName = new System.Windows.Forms.TextBox();
            this.numericUpDown_AwardAmount = new System.Windows.Forms.NumericUpDown();
            this.numericUpDown_AwardValue = new System.Windows.Forms.NumericUpDown();
            this.button_Exit = new System.Windows.Forms.PictureBox();
            this.button_Mini = new System.Windows.Forms.PictureBox();
            this.topBorder = new System.Windows.Forms.Label();
            this.button_AddAward = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown_AwardAmount)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown_AwardValue)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.button_Exit)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.button_Mini)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.button_AddAward)).BeginInit();
            this.SuspendLayout();
            // 
            // label_AwardName
            // 
            this.label_AwardName.AutoSize = true;
            this.label_AwardName.ForeColor = System.Drawing.Color.White;
            this.label_AwardName.Location = new System.Drawing.Point(21, 40);
            this.label_AwardName.Name = "label_AwardName";
            this.label_AwardName.Size = new System.Drawing.Size(84, 13);
            this.label_AwardName.TabIndex = 0;
            this.label_AwardName.Text = "Nazwa nagrody:";
            // 
            // label_AwardAmount
            // 
            this.label_AwardAmount.AutoSize = true;
            this.label_AwardAmount.ForeColor = System.Drawing.Color.White;
            this.label_AwardAmount.Location = new System.Drawing.Point(186, 84);
            this.label_AwardAmount.Name = "label_AwardAmount";
            this.label_AwardAmount.Size = new System.Drawing.Size(35, 13);
            this.label_AwardAmount.TabIndex = 1;
            this.label_AwardAmount.Text = "Ilość: ";
            // 
            // label_AwardPriority
            // 
            this.label_AwardPriority.AutoSize = true;
            this.label_AwardPriority.ForeColor = System.Drawing.Color.White;
            this.label_AwardPriority.Location = new System.Drawing.Point(274, 84);
            this.label_AwardPriority.Name = "label_AwardPriority";
            this.label_AwardPriority.Size = new System.Drawing.Size(39, 13);
            this.label_AwardPriority.TabIndex = 2;
            this.label_AwardPriority.Text = "Waga:";
            // 
            // textBox_AwardName
            // 
            this.textBox_AwardName.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(56)))), ((int)(((byte)(56)))), ((int)(((byte)(64)))));
            this.textBox_AwardName.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.textBox_AwardName.ForeColor = System.Drawing.Color.White;
            this.textBox_AwardName.Location = new System.Drawing.Point(24, 56);
            this.textBox_AwardName.Name = "textBox_AwardName";
            this.textBox_AwardName.Size = new System.Drawing.Size(300, 20);
            this.textBox_AwardName.TabIndex = 4;
            // 
            // numericUpDown_AwardAmount
            // 
            this.numericUpDown_AwardAmount.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.numericUpDown_AwardAmount.Location = new System.Drawing.Point(189, 100);
            this.numericUpDown_AwardAmount.Maximum = new decimal(new int[] {
            9999,
            0,
            0,
            0});
            this.numericUpDown_AwardAmount.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numericUpDown_AwardAmount.Name = "numericUpDown_AwardAmount";
            this.numericUpDown_AwardAmount.Size = new System.Drawing.Size(44, 20);
            this.numericUpDown_AwardAmount.TabIndex = 5;
            this.numericUpDown_AwardAmount.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // numericUpDown_AwardValue
            // 
            this.numericUpDown_AwardValue.Location = new System.Drawing.Point(277, 100);
            this.numericUpDown_AwardValue.Maximum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.numericUpDown_AwardValue.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numericUpDown_AwardValue.Name = "numericUpDown_AwardValue";
            this.numericUpDown_AwardValue.Size = new System.Drawing.Size(44, 20);
            this.numericUpDown_AwardValue.TabIndex = 6;
            this.numericUpDown_AwardValue.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // button_Exit
            // 
            this.button_Exit.Image = global::Azar.Properties.Resources.exit1;
            this.button_Exit.Location = new System.Drawing.Point(295, 0);
            this.button_Exit.Name = "button_Exit";
            this.button_Exit.Size = new System.Drawing.Size(45, 20);
            this.button_Exit.TabIndex = 23;
            this.button_Exit.TabStop = false;
            this.button_Exit.MouseDown += new System.Windows.Forms.MouseEventHandler(this.button_Exit_MouseDown);
            this.button_Exit.MouseUp += new System.Windows.Forms.MouseEventHandler(this.button_Exit_MouseUp);
            // 
            // button_Mini
            // 
            this.button_Mini.Image = global::Azar.Properties.Resources.mini1;
            this.button_Mini.Location = new System.Drawing.Point(263, 0);
            this.button_Mini.Name = "button_Mini";
            this.button_Mini.Size = new System.Drawing.Size(26, 20);
            this.button_Mini.TabIndex = 22;
            this.button_Mini.TabStop = false;
            this.button_Mini.MouseDown += new System.Windows.Forms.MouseEventHandler(this.button_Mini_MouseDown);
            this.button_Mini.MouseUp += new System.Windows.Forms.MouseEventHandler(this.button_Mini_MouseUp);
            // 
            // topBorder
            // 
            this.topBorder.AutoSize = true;
            this.topBorder.BackColor = System.Drawing.Color.Transparent;
            this.topBorder.Location = new System.Drawing.Point(0, 0);
            this.topBorder.MaximumSize = new System.Drawing.Size(263, 31);
            this.topBorder.MinimumSize = new System.Drawing.Size(263, 31);
            this.topBorder.Name = "topBorder";
            this.topBorder.Size = new System.Drawing.Size(263, 31);
            this.topBorder.TabIndex = 24;
            this.topBorder.MouseDown += new System.Windows.Forms.MouseEventHandler(this.topBorder_MouseDown);
            this.topBorder.MouseMove += new System.Windows.Forms.MouseEventHandler(this.topBorder_MouseMove);
            this.topBorder.MouseUp += new System.Windows.Forms.MouseEventHandler(this.topBorder_MouseUp);
            // 
            // button_AddAward
            // 
            this.button_AddAward.Image = global::Azar.Properties.Resources.addAward;
            this.button_AddAward.Location = new System.Drawing.Point(24, 85);
            this.button_AddAward.Name = "button_AddAward";
            this.button_AddAward.Size = new System.Drawing.Size(153, 36);
            this.button_AddAward.TabIndex = 25;
            this.button_AddAward.TabStop = false;
            this.button_AddAward.MouseDown += new System.Windows.Forms.MouseEventHandler(this.button_AddAward_MouseDown);
            this.button_AddAward.MouseUp += new System.Windows.Forms.MouseEventHandler(this.button_AddAward_MouseUp);
            // 
            // AddAwards
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(45)))), ((int)(((byte)(45)))), ((int)(((byte)(51)))));
            this.BackgroundImage = global::Azar.Properties.Resources.addAwardsBackground;
            this.ClientSize = new System.Drawing.Size(346, 144);
            this.Controls.Add(this.button_AddAward);
            this.Controls.Add(this.topBorder);
            this.Controls.Add(this.button_Exit);
            this.Controls.Add(this.button_Mini);
            this.Controls.Add(this.numericUpDown_AwardValue);
            this.Controls.Add(this.numericUpDown_AwardAmount);
            this.Controls.Add(this.textBox_AwardName);
            this.Controls.Add(this.label_AwardPriority);
            this.Controls.Add(this.label_AwardAmount);
            this.Controls.Add(this.label_AwardName);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(346, 144);
            this.MinimumSize = new System.Drawing.Size(346, 144);
            this.Name = "AddAwards";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Dodaj nagrody - Azar";
            this.TopMost = true;
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown_AwardAmount)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown_AwardValue)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.button_Exit)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.button_Mini)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.button_AddAward)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label_AwardName;
        private System.Windows.Forms.Label label_AwardAmount;
        private System.Windows.Forms.Label label_AwardPriority;
        private System.Windows.Forms.TextBox textBox_AwardName;
        private System.Windows.Forms.NumericUpDown numericUpDown_AwardAmount;
        private System.Windows.Forms.NumericUpDown numericUpDown_AwardValue;
        private System.Windows.Forms.PictureBox button_Exit;
        private System.Windows.Forms.PictureBox button_Mini;
        private System.Windows.Forms.Label topBorder;
        private System.Windows.Forms.PictureBox button_AddAward;
    }
}