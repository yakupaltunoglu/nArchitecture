namespace TimeAndDate
{
    partial class Form1
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
            this.button1 = new System.Windows.Forms.Button();
            this.dateTimePicker1 = new System.Windows.Forms.DateTimePicker();
            this.label1 = new System.Windows.Forms.Label();
            this.tb_accesskey = new System.Windows.Forms.TextBox();
            this.tb_secretkey = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.lb_readexcell = new System.Windows.Forms.Label();
            this.lb_service_count = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(115, 146);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(136, 23);
            this.button1.TabIndex = 0;
            this.button1.Text = "Time Service";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // dateTimePicker1
            // 
            this.dateTimePicker1.Location = new System.Drawing.Point(79, 107);
            this.dateTimePicker1.Name = "dateTimePicker1";
            this.dateTimePicker1.Size = new System.Drawing.Size(200, 20);
            this.dateTimePicker1.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(60, 27);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(66, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "AccessKey :";
            // 
            // tb_accesskey
            // 
            this.tb_accesskey.Location = new System.Drawing.Point(131, 24);
            this.tb_accesskey.Name = "tb_accesskey";
            this.tb_accesskey.Size = new System.Drawing.Size(148, 20);
            this.tb_accesskey.TabIndex = 3;
            this.tb_accesskey.Text = "F8F1be1336";
            // 
            // tb_secretkey
            // 
            this.tb_secretkey.Location = new System.Drawing.Point(131, 60);
            this.tb_secretkey.Name = "tb_secretkey";
            this.tb_secretkey.Size = new System.Drawing.Size(148, 20);
            this.tb_secretkey.TabIndex = 5;
            this.tb_secretkey.Text = "pHYL2400YdHi8Tg6pm1M";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(64, 63);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(62, 13);
            this.label2.TabIndex = 4;
            this.label2.Text = "SecretKey :";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(112, 188);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(83, 13);
            this.label3.TabIndex = 6;
            this.label3.Text = "Veriler okunuyor";
            this.label3.Visible = false;
            // 
            // lb_readexcell
            // 
            this.lb_readexcell.AutoSize = true;
            this.lb_readexcell.Location = new System.Drawing.Point(205, 188);
            this.lb_readexcell.Name = "lb_readexcell";
            this.lb_readexcell.Size = new System.Drawing.Size(13, 13);
            this.lb_readexcell.TabIndex = 7;
            this.lb_readexcell.Text = "0";
            this.lb_readexcell.Visible = false;
            // 
            // lb_service_count
            // 
            this.lb_service_count.AutoSize = true;
            this.lb_service_count.Location = new System.Drawing.Point(205, 210);
            this.lb_service_count.Name = "lb_service_count";
            this.lb_service_count.Size = new System.Drawing.Size(13, 13);
            this.lb_service_count.TabIndex = 9;
            this.lb_service_count.Text = "0";
            this.lb_service_count.Visible = false;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(80, 210);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(117, 13);
            this.label5.TabIndex = 8;
            this.label5.Text = "Servisten veriler alınıyor";
            this.label5.Visible = false;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(364, 246);
            this.Controls.Add(this.lb_service_count);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.lb_readexcell);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.tb_secretkey);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.tb_accesskey);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.dateTimePicker1);
            this.Controls.Add(this.button1);
            this.Name = "Form1";
            this.Text = "Form1";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.DateTimePicker dateTimePicker1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox tb_accesskey;
        private System.Windows.Forms.TextBox tb_secretkey;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label lb_readexcell;
        private System.Windows.Forms.Label lb_service_count;
        private System.Windows.Forms.Label label5;
    }
}

