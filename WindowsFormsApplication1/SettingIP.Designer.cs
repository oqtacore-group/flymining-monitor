namespace BitcoinInfoMiner
{
    partial class SettingIP
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SettingIP));
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.numericUpDown1 = new System.Windows.Forms.NumericUpDown();
            this.numericUpDown2 = new System.Windows.Forms.NumericUpDown();
            this.numericUpDown3 = new System.Windows.Forms.NumericUpDown();
            this.numericUpDown4 = new System.Windows.Forms.NumericUpDown();
            this.numericUpDown5 = new System.Windows.Forms.NumericUpDown();
            this.numericUpDown6 = new System.Windows.Forms.NumericUpDown();
            this.numericUpDown7 = new System.Windows.Forms.NumericUpDown();
            this.numericUpDown8 = new System.Windows.Forms.NumericUpDown();
            this.label2 = new System.Windows.Forms.Label();
            this.progressBar2 = new System.Windows.Forms.ProgressBar();
            this.label3 = new System.Windows.Forms.Label();
            this.textBoxHostName = new System.Windows.Forms.TextBox();
            this.textBoxNetmask = new System.Windows.Forms.TextBox();
            this.textBoxGateway = new System.Windows.Forms.TextBox();
            this.textBoxDnsservers = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.button3 = new System.Windows.Forms.Button();
            this.button4 = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown3)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown4)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown5)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown6)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown7)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown8)).BeginInit();
            this.SuspendLayout();
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(287, 328);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(97, 44);
            this.button1.TabIndex = 0;
            this.button1.Text = "Import .csv from Bitmain IP Report Tool";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // button2
            // 
            this.button2.Enabled = false;
            this.button2.Location = new System.Drawing.Point(399, 328);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(97, 44);
            this.button2.TabIndex = 1;
            this.button2.Text = "Apply settings";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(257, 274);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(13, 13);
            this.label1.TabIndex = 5;
            this.label1.Text = "~";
            // 
            // numericUpDown1
            // 
            this.numericUpDown1.Location = new System.Drawing.Point(19, 267);
            this.numericUpDown1.Maximum = new decimal(new int[] {
            255,
            0,
            0,
            0});
            this.numericUpDown1.Name = "numericUpDown1";
            this.numericUpDown1.Size = new System.Drawing.Size(48, 21);
            this.numericUpDown1.TabIndex = 6;
            this.numericUpDown1.ValueChanged += new System.EventHandler(this.numericUpDown1_ValueChanged);
            // 
            // numericUpDown2
            // 
            this.numericUpDown2.Location = new System.Drawing.Point(77, 267);
            this.numericUpDown2.Maximum = new decimal(new int[] {
            255,
            0,
            0,
            0});
            this.numericUpDown2.Name = "numericUpDown2";
            this.numericUpDown2.Size = new System.Drawing.Size(48, 21);
            this.numericUpDown2.TabIndex = 7;
            this.numericUpDown2.ValueChanged += new System.EventHandler(this.numericUpDown2_ValueChanged);
            // 
            // numericUpDown3
            // 
            this.numericUpDown3.Location = new System.Drawing.Point(136, 267);
            this.numericUpDown3.Maximum = new decimal(new int[] {
            255,
            0,
            0,
            0});
            this.numericUpDown3.Name = "numericUpDown3";
            this.numericUpDown3.Size = new System.Drawing.Size(48, 21);
            this.numericUpDown3.TabIndex = 9;
            this.numericUpDown3.ValueChanged += new System.EventHandler(this.numericUpDown3_ValueChanged);
            // 
            // numericUpDown4
            // 
            this.numericUpDown4.Location = new System.Drawing.Point(194, 267);
            this.numericUpDown4.Maximum = new decimal(new int[] {
            255,
            0,
            0,
            0});
            this.numericUpDown4.Name = "numericUpDown4";
            this.numericUpDown4.Size = new System.Drawing.Size(48, 21);
            this.numericUpDown4.TabIndex = 8;
            this.numericUpDown4.ValueChanged += new System.EventHandler(this.numericUpDown4_ValueChanged);
            // 
            // numericUpDown5
            // 
            this.numericUpDown5.Increment = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.numericUpDown5.Location = new System.Drawing.Point(284, 267);
            this.numericUpDown5.Maximum = new decimal(new int[] {
            255,
            0,
            0,
            0});
            this.numericUpDown5.Name = "numericUpDown5";
            this.numericUpDown5.ReadOnly = true;
            this.numericUpDown5.Size = new System.Drawing.Size(48, 21);
            this.numericUpDown5.TabIndex = 13;
            // 
            // numericUpDown6
            // 
            this.numericUpDown6.Increment = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.numericUpDown6.Location = new System.Drawing.Point(340, 267);
            this.numericUpDown6.Maximum = new decimal(new int[] {
            255,
            0,
            0,
            0});
            this.numericUpDown6.Name = "numericUpDown6";
            this.numericUpDown6.ReadOnly = true;
            this.numericUpDown6.Size = new System.Drawing.Size(48, 21);
            this.numericUpDown6.TabIndex = 12;
            // 
            // numericUpDown7
            // 
            this.numericUpDown7.Increment = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.numericUpDown7.Location = new System.Drawing.Point(394, 267);
            this.numericUpDown7.Maximum = new decimal(new int[] {
            255,
            0,
            0,
            0});
            this.numericUpDown7.Name = "numericUpDown7";
            this.numericUpDown7.ReadOnly = true;
            this.numericUpDown7.Size = new System.Drawing.Size(48, 21);
            this.numericUpDown7.TabIndex = 11;
            // 
            // numericUpDown8
            // 
            this.numericUpDown8.Increment = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.numericUpDown8.Location = new System.Drawing.Point(448, 267);
            this.numericUpDown8.Maximum = new decimal(new int[] {
            255,
            0,
            0,
            0});
            this.numericUpDown8.Name = "numericUpDown8";
            this.numericUpDown8.ReadOnly = true;
            this.numericUpDown8.Size = new System.Drawing.Size(48, 21);
            this.numericUpDown8.TabIndex = 10;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(225, 47);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(0, 13);
            this.label2.TabIndex = 14;
            // 
            // progressBar2
            // 
            this.progressBar2.Location = new System.Drawing.Point(19, 299);
            this.progressBar2.Name = "progressBar2";
            this.progressBar2.Size = new System.Drawing.Size(477, 23);
            this.progressBar2.TabIndex = 16;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(17, 9);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(497, 156);
            this.label3.TabIndex = 17;
            this.label3.Text = resources.GetString("label3.Text");
            // 
            // textBoxHostName
            // 
            this.textBoxHostName.Location = new System.Drawing.Point(77, 181);
            this.textBoxHostName.Name = "textBoxHostName";
            this.textBoxHostName.Size = new System.Drawing.Size(148, 21);
            this.textBoxHostName.TabIndex = 18;
            // 
            // textBoxNetmask
            // 
            this.textBoxNetmask.Location = new System.Drawing.Point(340, 181);
            this.textBoxNetmask.Name = "textBoxNetmask";
            this.textBoxNetmask.Size = new System.Drawing.Size(156, 21);
            this.textBoxNetmask.TabIndex = 19;
            // 
            // textBoxGateway
            // 
            this.textBoxGateway.Location = new System.Drawing.Point(340, 227);
            this.textBoxGateway.Name = "textBoxGateway";
            this.textBoxGateway.Size = new System.Drawing.Size(156, 21);
            this.textBoxGateway.TabIndex = 20;
            // 
            // textBoxDnsservers
            // 
            this.textBoxDnsservers.Location = new System.Drawing.Point(77, 227);
            this.textBoxDnsservers.Name = "textBoxDnsservers";
            this.textBoxDnsservers.Size = new System.Drawing.Size(148, 21);
            this.textBoxDnsservers.TabIndex = 21;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(11, 184);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(61, 13);
            this.label4.TabIndex = 22;
            this.label4.Text = "HostName:";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(3, 230);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(64, 13);
            this.label5.TabIndex = 23;
            this.label5.Text = "Dns Servers:";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(281, 184);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(52, 13);
            this.label6.TabIndex = 24;
            this.label6.Text = "NetMask:";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(276, 230);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(55, 13);
            this.label7.TabIndex = 25;
            this.label7.Text = "Gateaway:";
            // 
            // button3
            // 
            this.button3.Font = new System.Drawing.Font("Constantia", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button3.Location = new System.Drawing.Point(174, 328);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(97, 63);
            this.button3.TabIndex = 26;
            this.button3.Text = "Using .csv return miners to their status before this operation";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Visible = false;
            this.button3.Click += new System.EventHandler(this.button3_Click_1);
            // 
            // button4
            // 
            this.button4.Enabled = false;
            this.button4.Location = new System.Drawing.Point(399, 384);
            this.button4.Name = "button4";
            this.button4.Size = new System.Drawing.Size(97, 44);
            this.button4.TabIndex = 27;
            this.button4.Text = "Apply settings (FailSafe)";
            this.button4.UseVisualStyleBackColor = true;
            this.button4.Click += new System.EventHandler(this.button4_Click);
            // 
            // SettingIP
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(512, 440);
            this.Controls.Add(this.button4);
            this.Controls.Add(this.button3);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.textBoxDnsservers);
            this.Controls.Add(this.textBoxGateway);
            this.Controls.Add(this.textBoxNetmask);
            this.Controls.Add(this.textBoxHostName);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.progressBar2);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.numericUpDown5);
            this.Controls.Add(this.numericUpDown6);
            this.Controls.Add(this.numericUpDown7);
            this.Controls.Add(this.numericUpDown8);
            this.Controls.Add(this.numericUpDown3);
            this.Controls.Add(this.numericUpDown4);
            this.Controls.Add(this.numericUpDown2);
            this.Controls.Add(this.numericUpDown1);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button1);
            this.Font = new System.Drawing.Font("Constantia", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Name = "SettingIP";
            this.Text = "SettingIP";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.SettingIP_FormClosing);
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown3)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown4)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown5)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown6)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown7)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown8)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.NumericUpDown numericUpDown1;
        private System.Windows.Forms.NumericUpDown numericUpDown2;
        private System.Windows.Forms.NumericUpDown numericUpDown3;
        private System.Windows.Forms.NumericUpDown numericUpDown4;
        private System.Windows.Forms.NumericUpDown numericUpDown5;
        private System.Windows.Forms.NumericUpDown numericUpDown6;
        private System.Windows.Forms.NumericUpDown numericUpDown7;
        private System.Windows.Forms.NumericUpDown numericUpDown8;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ProgressBar progressBar2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox textBoxHostName;
        private System.Windows.Forms.TextBox textBoxNetmask;
        private System.Windows.Forms.TextBox textBoxGateway;
        private System.Windows.Forms.TextBox textBoxDnsservers;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.Button button4;
    }
}