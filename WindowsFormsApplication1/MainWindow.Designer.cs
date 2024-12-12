namespace BitcoinInfoMiner
{
    partial class MainWindow
    {
        /// <summary>
        /// Требуется переменная конструктора.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Освободить все используемые ресурсы.
        /// </summary>
        /// <param name="disposing">истинно, если управляемый ресурс должен быть удален; иначе ложно.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Код, автоматически созданный конструктором форм Windows

        /// <summary>
        /// Обязательный метод для поддержки конструктора - не изменяйте
        /// содержимое данного метода при помощи редактора кода.
        /// </summary>
        private void InitializeComponent()
        {
            this.ipRangeBox = new System.Windows.Forms.CheckedListBox();
            this.progressBar1 = new System.Windows.Forms.ProgressBar();
            this.buttonConfigAll = new System.Windows.Forms.Button();
            this.buttonConfigSelect = new System.Windows.Forms.Button();
            this.buttonRebootAll = new System.Windows.Forms.Button();
            this.buttonRebootSelect = new System.Windows.Forms.Button();
            this.buttonSettings = new System.Windows.Forms.Button();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.buttonRangeListMinus = new System.Windows.Forms.Button();
            this.buttonRangeListPlus = new System.Windows.Forms.Button();
            this.buttonScan = new System.Windows.Forms.Button();
            this.buttonMonitor = new System.Windows.Forms.Button();
            this.checkBoxIPRanges = new System.Windows.Forms.CheckBox();
            this.checkBoxSuccess = new System.Windows.Forms.CheckBox();
            this.button4 = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.checkBoxAutoReport = new System.Windows.Forms.CheckBox();
            this.checkBoxAutoReboot = new System.Windows.Forms.CheckBox();
            this.button3 = new System.Windows.Forms.Button();
            this.labelDollar = new System.Windows.Forms.Label();
            this.labelWallet = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.labelReboot = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.labelTemperature = new System.Windows.Forms.Label();
            this.labelHashrate = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.labelMinerNumber = new System.Windows.Forms.Label();
            this.button8 = new System.Windows.Forms.Button();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.button5 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.buttonTableSite = new System.Windows.Forms.Button();
            this.buttonLoadMinerState = new System.Windows.Forms.Button();
            this.buttonSaveMinerState = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // ipRangeBox
            // 
            this.ipRangeBox.Location = new System.Drawing.Point(10, 62);
            this.ipRangeBox.Name = "ipRangeBox";
            this.ipRangeBox.Size = new System.Drawing.Size(257, 132);
            this.ipRangeBox.TabIndex = 41;
            this.ipRangeBox.SelectedIndexChanged += new System.EventHandler(this.ipRangeBox_SelectedIndexChanged);
            this.ipRangeBox.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.ipRangeBox_MouseDoubleClick);
            // 
            // progressBar1
            // 
            this.progressBar1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.progressBar1.Location = new System.Drawing.Point(273, 62);
            this.progressBar1.Name = "progressBar1";
            this.progressBar1.Size = new System.Drawing.Size(709, 23);
            this.progressBar1.TabIndex = 1;
            // 
            // buttonConfigAll
            // 
            this.buttonConfigAll.Location = new System.Drawing.Point(512, 12);
            this.buttonConfigAll.Name = "buttonConfigAll";
            this.buttonConfigAll.Size = new System.Drawing.Size(70, 44);
            this.buttonConfigAll.TabIndex = 2;
            this.buttonConfigAll.Text = "Config All Miners";
            this.buttonConfigAll.UseVisualStyleBackColor = true;
            this.buttonConfigAll.Click += new System.EventHandler(this.buttonConfigAll_Click);
            // 
            // buttonConfigSelect
            // 
            this.buttonConfigSelect.Location = new System.Drawing.Point(593, 12);
            this.buttonConfigSelect.Name = "buttonConfigSelect";
            this.buttonConfigSelect.Size = new System.Drawing.Size(91, 44);
            this.buttonConfigSelect.TabIndex = 3;
            this.buttonConfigSelect.Text = "Config Selected Miners";
            this.buttonConfigSelect.UseVisualStyleBackColor = true;
            this.buttonConfigSelect.Click += new System.EventHandler(this.buttonConfigSelect_Click);
            // 
            // buttonRebootAll
            // 
            this.buttonRebootAll.Location = new System.Drawing.Point(690, 12);
            this.buttonRebootAll.Name = "buttonRebootAll";
            this.buttonRebootAll.Size = new System.Drawing.Size(70, 44);
            this.buttonRebootAll.TabIndex = 5;
            this.buttonRebootAll.Text = "Reboot All Miners";
            this.buttonRebootAll.UseVisualStyleBackColor = true;
            this.buttonRebootAll.Click += new System.EventHandler(this.buttonRebootAll_Click);
            // 
            // buttonRebootSelect
            // 
            this.buttonRebootSelect.Location = new System.Drawing.Point(766, 12);
            this.buttonRebootSelect.Name = "buttonRebootSelect";
            this.buttonRebootSelect.Size = new System.Drawing.Size(94, 44);
            this.buttonRebootSelect.TabIndex = 6;
            this.buttonRebootSelect.Text = "Reboot Selected Miners";
            this.buttonRebootSelect.UseVisualStyleBackColor = true;
            this.buttonRebootSelect.Click += new System.EventHandler(this.buttonReboot_Click);
            // 
            // buttonSettings
            // 
            this.buttonSettings.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonSettings.Location = new System.Drawing.Point(890, 12);
            this.buttonSettings.Name = "buttonSettings";
            this.buttonSettings.Size = new System.Drawing.Size(92, 44);
            this.buttonSettings.TabIndex = 8;
            this.buttonSettings.Text = "Settings";
            this.buttonSettings.UseVisualStyleBackColor = true;
            this.buttonSettings.Click += new System.EventHandler(this.buttonSettingWindow_Click);
            // 
            // dataGridView1
            // 
            this.dataGridView1.AllowUserToAddRows = false;
            this.dataGridView1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dataGridView1.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells;
            this.dataGridView1.BackgroundColor = System.Drawing.SystemColors.ButtonFace;
            this.dataGridView1.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Sunken;
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.EditMode = System.Windows.Forms.DataGridViewEditMode.EditProgrammatically;
            this.dataGridView1.Location = new System.Drawing.Point(8, 299);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.ReadOnly = true;
            this.dataGridView1.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.AutoSizeToFirstHeader;
            this.dataGridView1.Size = new System.Drawing.Size(974, 356);
            this.dataGridView1.TabIndex = 43;
            this.dataGridView1.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridView1_CellClick);
            this.dataGridView1.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridView1_CellDoubleClick);
            this.dataGridView1.SortCompare += new System.Windows.Forms.DataGridViewSortCompareEventHandler(this.dataGridView1_SortCompare);
            // 
            // buttonRangeListMinus
            // 
            this.buttonRangeListMinus.Location = new System.Drawing.Point(116, 12);
            this.buttonRangeListMinus.Name = "buttonRangeListMinus";
            this.buttonRangeListMinus.Size = new System.Drawing.Size(19, 23);
            this.buttonRangeListMinus.TabIndex = 44;
            this.buttonRangeListMinus.Text = "-";
            this.buttonRangeListMinus.UseVisualStyleBackColor = true;
            this.buttonRangeListMinus.Click += new System.EventHandler(this.buttonRangeListMinus_Click);
            // 
            // buttonRangeListPlus
            // 
            this.buttonRangeListPlus.Location = new System.Drawing.Point(90, 12);
            this.buttonRangeListPlus.Name = "buttonRangeListPlus";
            this.buttonRangeListPlus.Size = new System.Drawing.Size(20, 23);
            this.buttonRangeListPlus.TabIndex = 45;
            this.buttonRangeListPlus.Text = "+";
            this.buttonRangeListPlus.UseVisualStyleBackColor = true;
            this.buttonRangeListPlus.Click += new System.EventHandler(this.buttonRangeListPlus_Click);
            // 
            // buttonScan
            // 
            this.buttonScan.Location = new System.Drawing.Point(271, 12);
            this.buttonScan.Name = "buttonScan";
            this.buttonScan.Size = new System.Drawing.Size(70, 44);
            this.buttonScan.TabIndex = 47;
            this.buttonScan.Text = "Scan Miners";
            this.buttonScan.UseVisualStyleBackColor = true;
            this.buttonScan.Click += new System.EventHandler(this.buttonScan_Click);
            // 
            // buttonMonitor
            // 
            this.buttonMonitor.Location = new System.Drawing.Point(434, 12);
            this.buttonMonitor.Name = "buttonMonitor";
            this.buttonMonitor.Size = new System.Drawing.Size(70, 44);
            this.buttonMonitor.TabIndex = 48;
            this.buttonMonitor.Text = "Start Monitor";
            this.buttonMonitor.UseVisualStyleBackColor = true;
            this.buttonMonitor.Click += new System.EventHandler(this.buttonMonitoring_Click);
            // 
            // checkBoxIPRanges
            // 
            this.checkBoxIPRanges.AutoSize = true;
            this.checkBoxIPRanges.Location = new System.Drawing.Point(5, 16);
            this.checkBoxIPRanges.Name = "checkBoxIPRanges";
            this.checkBoxIPRanges.Size = new System.Drawing.Size(75, 17);
            this.checkBoxIPRanges.TabIndex = 53;
            this.checkBoxIPRanges.Text = "IP Ranges:";
            this.checkBoxIPRanges.UseVisualStyleBackColor = true;
            // 
            // checkBoxSuccess
            // 
            this.checkBoxSuccess.AutoSize = true;
            this.checkBoxSuccess.Location = new System.Drawing.Point(145, 16);
            this.checkBoxSuccess.Name = "checkBoxSuccess";
            this.checkBoxSuccess.Size = new System.Drawing.Size(122, 17);
            this.checkBoxSuccess.TabIndex = 54;
            this.checkBoxSuccess.Text = "Only Success Miners";
            this.checkBoxSuccess.UseVisualStyleBackColor = true;
            this.checkBoxSuccess.Visible = false;
            this.checkBoxSuccess.CheckedChanged += new System.EventHandler(this.checkBoxSuccess_CheckedChanged);
            // 
            // button4
            // 
            this.button4.Location = new System.Drawing.Point(100, 19);
            this.button4.Name = "button4";
            this.button4.Size = new System.Drawing.Size(87, 44);
            this.button4.TabIndex = 61;
            this.button4.Text = "Set Static IPs";
            this.button4.UseVisualStyleBackColor = true;
            this.button4.Click += new System.EventHandler(this.buttonSettingIP_Click);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(463, 20);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(84, 44);
            this.button1.TabIndex = 67;
            this.button1.Text = "Get Excel File";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Visible = false;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.Controls.Add(this.checkBoxAutoReport);
            this.groupBox1.Controls.Add(this.checkBoxAutoReboot);
            this.groupBox1.Controls.Add(this.button3);
            this.groupBox1.Controls.Add(this.labelDollar);
            this.groupBox1.Controls.Add(this.labelWallet);
            this.groupBox1.Controls.Add(this.dataGridView1);
            this.groupBox1.Controls.Add(this.label6);
            this.groupBox1.Controls.Add(this.labelReboot);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.Controls.Add(this.labelTemperature);
            this.groupBox1.Controls.Add(this.labelHashrate);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.labelMinerNumber);
            this.groupBox1.Controls.Add(this.checkBoxIPRanges);
            this.groupBox1.Controls.Add(this.progressBar1);
            this.groupBox1.Controls.Add(this.buttonConfigAll);
            this.groupBox1.Controls.Add(this.buttonConfigSelect);
            this.groupBox1.Controls.Add(this.buttonRebootAll);
            this.groupBox1.Controls.Add(this.buttonRebootSelect);
            this.groupBox1.Controls.Add(this.buttonSettings);
            this.groupBox1.Controls.Add(this.checkBoxSuccess);
            this.groupBox1.Controls.Add(this.buttonMonitor);
            this.groupBox1.Controls.Add(this.buttonScan);
            this.groupBox1.Controls.Add(this.buttonRangeListPlus);
            this.groupBox1.Controls.Add(this.buttonRangeListMinus);
            this.groupBox1.Controls.Add(this.ipRangeBox);
            this.groupBox1.Location = new System.Drawing.Point(13, 93);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(988, 666);
            this.groupBox1.TabIndex = 68;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Miner monitoring";
            // 
            // checkBoxAutoReport
            // 
            this.checkBoxAutoReport.AutoSize = true;
            this.checkBoxAutoReport.Location = new System.Drawing.Point(11, 230);
            this.checkBoxAutoReport.Name = "checkBoxAutoReport";
            this.checkBoxAutoReport.Size = new System.Drawing.Size(126, 17);
            this.checkBoxAutoReport.TabIndex = 81;
            this.checkBoxAutoReport.Text = "Check All Autoreport";
            this.checkBoxAutoReport.UseVisualStyleBackColor = true;
            this.checkBoxAutoReport.CheckedChanged += new System.EventHandler(this.checkBoxAutoReport_CheckedChanged);
            // 
            // checkBoxAutoReboot
            // 
            this.checkBoxAutoReboot.AutoSize = true;
            this.checkBoxAutoReboot.Location = new System.Drawing.Point(11, 212);
            this.checkBoxAutoReboot.Name = "checkBoxAutoReboot";
            this.checkBoxAutoReboot.Size = new System.Drawing.Size(128, 17);
            this.checkBoxAutoReboot.TabIndex = 80;
            this.checkBoxAutoReboot.Text = "Check All Autoreboot";
            this.checkBoxAutoReboot.UseVisualStyleBackColor = true;
            this.checkBoxAutoReboot.CheckedChanged += new System.EventHandler(this.checkBoxAutoReboot_CheckedChanged);
            // 
            // button3
            // 
            this.button3.Location = new System.Drawing.Point(347, 12);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(81, 44);
            this.button3.TabIndex = 79;
            this.button3.Text = "Refresh Miner Range";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // labelDollar
            // 
            this.labelDollar.AutoSize = true;
            this.labelDollar.Font = new System.Drawing.Font("Constantia", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelDollar.Location = new System.Drawing.Point(481, 230);
            this.labelDollar.Name = "labelDollar";
            this.labelDollar.Size = new System.Drawing.Size(124, 15);
            this.labelDollar.TabIndex = 78;
            this.labelDollar.Text = "Calculating income";
            // 
            // labelWallet
            // 
            this.labelWallet.AutoSize = true;
            this.labelWallet.Font = new System.Drawing.Font("Constantia", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelWallet.Location = new System.Drawing.Point(481, 211);
            this.labelWallet.Name = "labelWallet";
            this.labelWallet.Size = new System.Drawing.Size(124, 15);
            this.labelWallet.TabIndex = 77;
            this.labelWallet.Text = "Calculating income";
            this.labelWallet.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Constantia", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.Location = new System.Drawing.Point(290, 211);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(121, 15);
            this.label6.TabIndex = 76;
            this.label6.Text = "Income in last 24h:";
            // 
            // labelReboot
            // 
            this.labelReboot.AutoSize = true;
            this.labelReboot.Font = new System.Drawing.Font("Constantia", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelReboot.Location = new System.Drawing.Point(483, 187);
            this.labelReboot.Name = "labelReboot";
            this.labelReboot.Size = new System.Drawing.Size(190, 15);
            this.labelReboot.TabIndex = 75;
            this.labelReboot.Text = "Start monitoring to get Result";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Constantia", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(289, 189);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(85, 15);
            this.label4.TabIndex = 74;
            this.label4.Text = "Last Reboot:";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Constantia", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.Location = new System.Drawing.Point(289, 165);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(176, 15);
            this.label5.TabIndex = 73;
            this.label5.Text = "Min/Avg/Max Temperature:";
            // 
            // labelTemperature
            // 
            this.labelTemperature.AutoSize = true;
            this.labelTemperature.Font = new System.Drawing.Font("Constantia", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelTemperature.Location = new System.Drawing.Point(483, 165);
            this.labelTemperature.Name = "labelTemperature";
            this.labelTemperature.Size = new System.Drawing.Size(190, 15);
            this.labelTemperature.TabIndex = 72;
            this.labelTemperature.Text = "Start monitoring to get Result";
            // 
            // labelHashrate
            // 
            this.labelHashrate.AutoSize = true;
            this.labelHashrate.Font = new System.Drawing.Font("Constantia", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelHashrate.Location = new System.Drawing.Point(483, 112);
            this.labelHashrate.Name = "labelHashrate";
            this.labelHashrate.Size = new System.Drawing.Size(190, 15);
            this.labelHashrate.TabIndex = 71;
            this.labelHashrate.Text = "Start monitoring to get Result";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Constantia", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(289, 112);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(125, 15);
            this.label1.TabIndex = 70;
            this.label1.Text = "Total  Hashrate RT:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Constantia", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(289, 88);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(89, 15);
            this.label2.TabIndex = 69;
            this.label2.Text = "Miner Status:";
            // 
            // labelMinerNumber
            // 
            this.labelMinerNumber.AutoSize = true;
            this.labelMinerNumber.Font = new System.Drawing.Font("Constantia", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelMinerNumber.Location = new System.Drawing.Point(483, 88);
            this.labelMinerNumber.Name = "labelMinerNumber";
            this.labelMinerNumber.Size = new System.Drawing.Size(190, 15);
            this.labelMinerNumber.TabIndex = 68;
            this.labelMinerNumber.Text = "Start monitoring to get Result";
            // 
            // button8
            // 
            this.button8.Location = new System.Drawing.Point(10, 19);
            this.button8.Name = "button8";
            this.button8.Size = new System.Drawing.Size(84, 44);
            this.button8.TabIndex = 66;
            this.button8.Text = "WalletInfo";
            this.button8.UseVisualStyleBackColor = true;
            this.button8.Click += new System.EventHandler(this.button8_Click);
            // 
            // groupBox2
            // 
            this.groupBox2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox2.Controls.Add(this.buttonSaveMinerState);
            this.groupBox2.Controls.Add(this.buttonLoadMinerState);
            this.groupBox2.Controls.Add(this.button5);
            this.groupBox2.Controls.Add(this.button2);
            this.groupBox2.Controls.Add(this.buttonTableSite);
            this.groupBox2.Controls.Add(this.button8);
            this.groupBox2.Controls.Add(this.button4);
            this.groupBox2.Controls.Add(this.button1);
            this.groupBox2.Location = new System.Drawing.Point(13, 12);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(988, 75);
            this.groupBox2.TabIndex = 69;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Tools";
            this.groupBox2.Visible = false;
            // 
            // button5
            // 
            this.button5.Location = new System.Drawing.Point(283, 19);
            this.button5.Name = "button5";
            this.button5.Size = new System.Drawing.Size(84, 44);
            this.button5.TabIndex = 70;
            this.button5.Text = "All miner errors";
            this.button5.UseVisualStyleBackColor = true;
            this.button5.Click += new System.EventHandler(this.button5_Click);
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(373, 20);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(84, 44);
            this.button2.TabIndex = 69;
            this.button2.Text = "testing";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Visible = false;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // buttonTableSite
            // 
            this.buttonTableSite.Location = new System.Drawing.Point(193, 19);
            this.buttonTableSite.Name = "buttonTableSite";
            this.buttonTableSite.Size = new System.Drawing.Size(84, 44);
            this.buttonTableSite.TabIndex = 68;
            this.buttonTableSite.Text = "Open TableSite";
            this.buttonTableSite.UseVisualStyleBackColor = true;
            this.buttonTableSite.Click += new System.EventHandler(this.buttonTableSite_Click);
            // 
            // buttonLoadMinerState
            // 
            this.buttonLoadMinerState.Location = new System.Drawing.Point(643, 20);
            this.buttonLoadMinerState.Name = "buttonLoadMinerState";
            this.buttonLoadMinerState.Size = new System.Drawing.Size(84, 44);
            this.buttonLoadMinerState.TabIndex = 71;
            this.buttonLoadMinerState.Text = "Load Miner State";
            this.buttonLoadMinerState.UseVisualStyleBackColor = true;
            this.buttonLoadMinerState.Click += new System.EventHandler(this.buttonLoadMinerState_Click);
            // 
            // buttonSaveMinerState
            // 
            this.buttonSaveMinerState.Location = new System.Drawing.Point(553, 19);
            this.buttonSaveMinerState.Name = "buttonSaveMinerState";
            this.buttonSaveMinerState.Size = new System.Drawing.Size(84, 44);
            this.buttonSaveMinerState.TabIndex = 72;
            this.buttonSaveMinerState.Text = "Save miner state";
            this.buttonSaveMinerState.UseVisualStyleBackColor = true;
            this.buttonSaveMinerState.Click += new System.EventHandler(this.buttonSaveMinerState_Click);
            // 
            // MainWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1010, 768);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Font = new System.Drawing.Font("Constantia", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Name = "MainWindow";
            this.Text = "FlyMining";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
            this.Load += new System.EventHandler(this.MainWindow_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ProgressBar progressBar1;
        private System.Windows.Forms.Button buttonConfigAll;
        private System.Windows.Forms.Button buttonConfigSelect;
        private System.Windows.Forms.Button buttonRebootAll;
        private System.Windows.Forms.Button buttonRebootSelect;
        private System.Windows.Forms.Button buttonSettings;
        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.Button buttonRangeListMinus;
        private System.Windows.Forms.Button buttonRangeListPlus;
        private System.Windows.Forms.Button buttonScan;
        private System.Windows.Forms.Button buttonMonitor;
        private System.Windows.Forms.CheckBox checkBoxIPRanges;
        private System.Windows.Forms.CheckBox checkBoxSuccess;
        private System.Windows.Forms.Button button4;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label labelReboot;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label labelTemperature;
        private System.Windows.Forms.Label labelHashrate;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label labelMinerNumber;
        private System.Windows.Forms.Label labelWallet;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label labelDollar;
        private System.Windows.Forms.Button button8;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Button buttonTableSite;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.CheckBox checkBoxAutoReport;
        private System.Windows.Forms.CheckBox checkBoxAutoReboot;
        public System.Windows.Forms.CheckedListBox ipRangeBox;
        private System.Windows.Forms.Button button5;
        private System.Windows.Forms.Button buttonSaveMinerState;
        private System.Windows.Forms.Button buttonLoadMinerState;
    }
}

