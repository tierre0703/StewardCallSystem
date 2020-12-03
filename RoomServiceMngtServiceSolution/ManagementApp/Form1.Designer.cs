namespace ManagementApp
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.panel1 = new System.Windows.Forms.Panel();
            this.mainPanel = new System.Windows.Forms.Panel();
            this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
            this.panel5 = new System.Windows.Forms.Panel();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.panel3 = new System.Windows.Forms.Panel();
            this.employeeMngtBtn = new System.Windows.Forms.Button();
            this.roomMngtBtn = new System.Windows.Forms.Button();
            this.callBtn = new System.Windows.Forms.Button();
            this.mobileAppStatsBtn = new System.Windows.Forms.Button();
            this.repeaterStatBtn = new System.Windows.Forms.Button();
            this.panel4 = new System.Windows.Forms.Panel();
            this.btnEmail = new System.Windows.Forms.Button();
            this.panel1.SuspendLayout();
            this.flowLayoutPanel1.SuspendLayout();
            this.panel5.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(228)))), ((int)(((byte)(228)))), ((int)(((byte)(241)))));
            this.panel1.Controls.Add(this.mainPanel);
            this.panel1.Controls.Add(this.flowLayoutPanel1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1348, 633);
            this.panel1.TabIndex = 9;
            // 
            // mainPanel
            // 
            this.mainPanel.BackColor = System.Drawing.Color.White;
            this.mainPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.mainPanel.Location = new System.Drawing.Point(273, 0);
            this.mainPanel.Name = "mainPanel";
            this.mainPanel.Padding = new System.Windows.Forms.Padding(10);
            this.mainPanel.Size = new System.Drawing.Size(1075, 633);
            this.mainPanel.TabIndex = 10;
            // 
            // flowLayoutPanel1
            // 
            this.flowLayoutPanel1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(212)))), ((int)(((byte)(235)))), ((int)(((byte)(255)))));
            this.flowLayoutPanel1.Controls.Add(this.panel5);
            this.flowLayoutPanel1.Controls.Add(this.panel3);
            this.flowLayoutPanel1.Controls.Add(this.employeeMngtBtn);
            this.flowLayoutPanel1.Controls.Add(this.roomMngtBtn);
            this.flowLayoutPanel1.Controls.Add(this.callBtn);
            this.flowLayoutPanel1.Controls.Add(this.mobileAppStatsBtn);
            this.flowLayoutPanel1.Controls.Add(this.repeaterStatBtn);
            this.flowLayoutPanel1.Controls.Add(this.btnEmail);
            this.flowLayoutPanel1.Controls.Add(this.panel4);
            this.flowLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Left;
            this.flowLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.flowLayoutPanel1.Name = "flowLayoutPanel1";
            this.flowLayoutPanel1.Size = new System.Drawing.Size(273, 633);
            this.flowLayoutPanel1.TabIndex = 9;
            // 
            // panel5
            // 
            this.panel5.BackColor = System.Drawing.Color.Transparent;
            this.panel5.Controls.Add(this.pictureBox1);
            this.panel5.Location = new System.Drawing.Point(3, 3);
            this.panel5.Name = "panel5";
            this.panel5.Size = new System.Drawing.Size(269, 73);
            this.panel5.TabIndex = 9;
            // 
            // pictureBox1
            // 
            this.pictureBox1.BackColor = System.Drawing.Color.Transparent;
            this.pictureBox1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.pictureBox1.Image = global::ManagementApp.Properties.Resources.Final_Logo_AvsAzur_blue;
            this.pictureBox1.Location = new System.Drawing.Point(19, 6);
            this.pictureBox1.Margin = new System.Windows.Forms.Padding(1, 0, 0, 0);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(230, 60);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox1.TabIndex = 0;
            this.pictureBox1.TabStop = false;
            // 
            // panel3
            // 
            this.panel3.Location = new System.Drawing.Point(3, 82);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(278, 0);
            this.panel3.TabIndex = 8;
            // 
            // employeeMngtBtn
            // 
            this.employeeMngtBtn.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(7)))), ((int)(((byte)(72)))), ((int)(((byte)(120)))));
            this.employeeMngtBtn.FlatAppearance.BorderColor = System.Drawing.Color.White;
            this.employeeMngtBtn.FlatAppearance.BorderSize = 0;
            this.employeeMngtBtn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.employeeMngtBtn.Font = new System.Drawing.Font("Tahoma", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.employeeMngtBtn.ForeColor = System.Drawing.Color.White;
            this.employeeMngtBtn.Location = new System.Drawing.Point(3, 88);
            this.employeeMngtBtn.Name = "employeeMngtBtn";
            this.employeeMngtBtn.Size = new System.Drawing.Size(265, 40);
            this.employeeMngtBtn.TabIndex = 6;
            this.employeeMngtBtn.Text = "User Management";
            this.employeeMngtBtn.UseVisualStyleBackColor = false;
            this.employeeMngtBtn.Click += new System.EventHandler(this.employeeMngtBtn_Click);
            // 
            // roomMngtBtn
            // 
            this.roomMngtBtn.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(7)))), ((int)(((byte)(72)))), ((int)(((byte)(120)))));
            this.roomMngtBtn.FlatAppearance.BorderColor = System.Drawing.Color.White;
            this.roomMngtBtn.FlatAppearance.BorderSize = 0;
            this.roomMngtBtn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.roomMngtBtn.Font = new System.Drawing.Font("Tahoma", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.roomMngtBtn.ForeColor = System.Drawing.Color.White;
            this.roomMngtBtn.Location = new System.Drawing.Point(3, 134);
            this.roomMngtBtn.Name = "roomMngtBtn";
            this.roomMngtBtn.Size = new System.Drawing.Size(265, 40);
            this.roomMngtBtn.TabIndex = 2;
            this.roomMngtBtn.Text = "Room Management";
            this.roomMngtBtn.UseVisualStyleBackColor = false;
            this.roomMngtBtn.Click += new System.EventHandler(this.roomMngtBtn_Click);
            // 
            // callBtn
            // 
            this.callBtn.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(7)))), ((int)(((byte)(72)))), ((int)(((byte)(120)))));
            this.callBtn.FlatAppearance.BorderColor = System.Drawing.Color.White;
            this.callBtn.FlatAppearance.BorderSize = 0;
            this.callBtn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.callBtn.Font = new System.Drawing.Font("Tahoma", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.callBtn.ForeColor = System.Drawing.Color.White;
            this.callBtn.Location = new System.Drawing.Point(3, 180);
            this.callBtn.Name = "callBtn";
            this.callBtn.Size = new System.Drawing.Size(265, 40);
            this.callBtn.TabIndex = 10;
            this.callBtn.Text = "Call Management";
            this.callBtn.UseVisualStyleBackColor = false;
            this.callBtn.Click += new System.EventHandler(this.callBtn_Click);
            // 
            // mobileAppStatsBtn
            // 
            this.mobileAppStatsBtn.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(7)))), ((int)(((byte)(72)))), ((int)(((byte)(120)))));
            this.mobileAppStatsBtn.FlatAppearance.BorderColor = System.Drawing.Color.White;
            this.mobileAppStatsBtn.FlatAppearance.BorderSize = 0;
            this.mobileAppStatsBtn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.mobileAppStatsBtn.Font = new System.Drawing.Font("Tahoma", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.mobileAppStatsBtn.ForeColor = System.Drawing.Color.White;
            this.mobileAppStatsBtn.Location = new System.Drawing.Point(3, 226);
            this.mobileAppStatsBtn.Name = "mobileAppStatsBtn";
            this.mobileAppStatsBtn.Size = new System.Drawing.Size(265, 40);
            this.mobileAppStatsBtn.TabIndex = 11;
            this.mobileAppStatsBtn.Text = "Active Receivers";
            this.mobileAppStatsBtn.UseVisualStyleBackColor = false;
            this.mobileAppStatsBtn.Click += new System.EventHandler(this.mobileAppStatsBtn_Click);
            // 
            // repeaterStatBtn
            // 
            this.repeaterStatBtn.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(7)))), ((int)(((byte)(72)))), ((int)(((byte)(120)))));
            this.repeaterStatBtn.FlatAppearance.BorderColor = System.Drawing.Color.White;
            this.repeaterStatBtn.FlatAppearance.BorderSize = 0;
            this.repeaterStatBtn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.repeaterStatBtn.Font = new System.Drawing.Font("Tahoma", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.repeaterStatBtn.ForeColor = System.Drawing.Color.White;
            this.repeaterStatBtn.Location = new System.Drawing.Point(3, 272);
            this.repeaterStatBtn.Name = "repeaterStatBtn";
            this.repeaterStatBtn.Size = new System.Drawing.Size(265, 40);
            this.repeaterStatBtn.TabIndex = 12;
            this.repeaterStatBtn.Text = "Repeaters Management";
            this.repeaterStatBtn.UseVisualStyleBackColor = false;
            this.repeaterStatBtn.Click += new System.EventHandler(this.repeaterStatBtn_Click);
            // 
            // panel4
            // 
            this.panel4.Location = new System.Drawing.Point(3, 364);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(270, 230);
            this.panel4.TabIndex = 7;
            // 
            // btnEmail
            // 
            this.btnEmail.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(7)))), ((int)(((byte)(72)))), ((int)(((byte)(120)))));
            this.btnEmail.FlatAppearance.BorderColor = System.Drawing.Color.White;
            this.btnEmail.FlatAppearance.BorderSize = 0;
            this.btnEmail.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnEmail.Font = new System.Drawing.Font("Tahoma", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnEmail.ForeColor = System.Drawing.Color.White;
            this.btnEmail.Location = new System.Drawing.Point(3, 318);
            this.btnEmail.Name = "btnEmail";
            this.btnEmail.Size = new System.Drawing.Size(265, 40);
            this.btnEmail.TabIndex = 13;
            this.btnEmail.Text = "Email Management";
            this.btnEmail.UseVisualStyleBackColor = false;
            this.btnEmail.Click += new System.EventHandler(this.btnEmail_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1348, 633);
            this.Controls.Add(this.panel1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "Form1";
            this.Text = "Steward Call Management";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.panel1.ResumeLayout(false);
            this.flowLayoutPanel1.ResumeLayout(false);
            this.panel5.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.Button employeeMngtBtn;
        private System.Windows.Forms.Button roomMngtBtn;
        private System.Windows.Forms.Panel panel4;
        private System.Windows.Forms.Panel panel5;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Panel mainPanel;
        private System.Windows.Forms.Button callBtn;
        private System.Windows.Forms.Button mobileAppStatsBtn;
        private System.Windows.Forms.Button repeaterStatBtn;
        private System.Windows.Forms.Button btnEmail;
    }
}

