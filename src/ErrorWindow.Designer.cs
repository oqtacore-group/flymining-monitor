namespace BitcoinInfoMiner
{
    partial class ErrorWindow
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
            this.components = new System.ComponentModel.Container();
            this.errorDataGridView = new System.Windows.Forms.DataGridView();
            this.fan1checkBox = new System.Windows.Forms.CheckBox();
            this.UniqueErrorcheckBox = new System.Windows.Forms.CheckBox();
            this.hashBoardErrorcheckBox = new System.Windows.Forms.CheckBox();
            this.fan2checkBox = new System.Windows.Forms.CheckBox();
            this.chipErrorcheckBox = new System.Windows.Forms.CheckBox();
            this.filterGroupBox = new System.Windows.Forms.GroupBox();
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.tableGroupBox = new System.Windows.Forms.GroupBox();
            this.label1 = new System.Windows.Forms.Label();
            this.localTimeLabel = new System.Windows.Forms.Label();
            this.lastUpdateTimer = new System.Windows.Forms.Timer(this.components);
            this.timeSincelabel = new System.Windows.Forms.Label();
            this.groupBoxMinerState = new System.Windows.Forms.GroupBox();
            this.label10 = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.label12 = new System.Windows.Forms.Label();
            this.label13 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.textBoxAverFreq = new System.Windows.Forms.TextBox();
            this.textBoxTotalHashrate = new System.Windows.Forms.TextBox();
            this.textBoxChipState = new System.Windows.Forms.TextBox();
            this.textBoxIdealHash = new System.Windows.Forms.TextBox();
            this.textBoxTempState = new System.Windows.Forms.TextBox();
            this.textBoxFanSpeed = new System.Windows.Forms.TextBox();
            this.textBoxCoreState = new System.Windows.Forms.TextBox();
            this.textBoxOffside = new System.Windows.Forms.TextBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.ChipsMissingtextBox = new System.Windows.Forms.TextBox();
            this.HashboardFailtextBox = new System.Windows.Forms.TextBox();
            this.replaceFantextBox = new System.Windows.Forms.TextBox();
            this.HardBoardMisstextBox = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.textBoxAverangeTemp = new System.Windows.Forms.TextBox();
            this.label14 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.errorDataGridView)).BeginInit();
            this.filterGroupBox.SuspendLayout();
            this.tableGroupBox.SuspendLayout();
            this.groupBoxMinerState.SuspendLayout();
            this.panel1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // errorDataGridView
            // 
            this.errorDataGridView.AllowUserToAddRows = false;
            this.errorDataGridView.AllowUserToDeleteRows = false;
            this.errorDataGridView.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.errorDataGridView.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells;
            this.errorDataGridView.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.AllCells;
            this.errorDataGridView.BackgroundColor = System.Drawing.SystemColors.ButtonFace;
            this.errorDataGridView.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Sunken;
            this.errorDataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.errorDataGridView.EditMode = System.Windows.Forms.DataGridViewEditMode.EditProgrammatically;
            this.errorDataGridView.Location = new System.Drawing.Point(6, 18);
            this.errorDataGridView.Name = "errorDataGridView";
            this.errorDataGridView.ReadOnly = true;
            this.errorDataGridView.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.AutoSizeToFirstHeader;
            this.errorDataGridView.Size = new System.Drawing.Size(1101, 299);
            this.errorDataGridView.TabIndex = 0;
            this.errorDataGridView.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.errorDataGridView_CellClick);
            this.errorDataGridView.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.errorDataGridView_CellDoubleClick);
            // 
            // fan1checkBox
            // 
            this.fan1checkBox.AutoSize = true;
            this.fan1checkBox.Checked = true;
            this.fan1checkBox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.fan1checkBox.Location = new System.Drawing.Point(26, 19);
            this.fan1checkBox.Name = "fan1checkBox";
            this.fan1checkBox.Size = new System.Drawing.Size(107, 17);
            this.fan1checkBox.TabIndex = 1;
            this.fan1checkBox.Text = "Error with first fan";
            this.fan1checkBox.UseVisualStyleBackColor = true;
            this.fan1checkBox.CheckedChanged += new System.EventHandler(this.checkBox1_CheckedChanged);
            // 
            // UniqueErrorcheckBox
            // 
            this.UniqueErrorcheckBox.AutoSize = true;
            this.UniqueErrorcheckBox.Location = new System.Drawing.Point(214, 52);
            this.UniqueErrorcheckBox.Name = "UniqueErrorcheckBox";
            this.UniqueErrorcheckBox.Size = new System.Drawing.Size(112, 17);
            this.UniqueErrorcheckBox.TabIndex = 2;
            this.UniqueErrorcheckBox.Text = "Error list not empty";
            this.UniqueErrorcheckBox.UseVisualStyleBackColor = true;
            this.UniqueErrorcheckBox.CheckedChanged += new System.EventHandler(this.UniqueErrorcheckBox_CheckedChanged);
            // 
            // hashBoardErrorcheckBox
            // 
            this.hashBoardErrorcheckBox.AutoSize = true;
            this.hashBoardErrorcheckBox.Checked = true;
            this.hashBoardErrorcheckBox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.hashBoardErrorcheckBox.Location = new System.Drawing.Point(26, 68);
            this.hashBoardErrorcheckBox.Name = "hashBoardErrorcheckBox";
            this.hashBoardErrorcheckBox.Size = new System.Drawing.Size(124, 17);
            this.hashBoardErrorcheckBox.TabIndex = 3;
            this.hashBoardErrorcheckBox.Text = "Error with hashBoard";
            this.hashBoardErrorcheckBox.UseVisualStyleBackColor = true;
            this.hashBoardErrorcheckBox.CheckedChanged += new System.EventHandler(this.hashBoardErrorcheckBox_CheckedChanged);
            // 
            // fan2checkBox
            // 
            this.fan2checkBox.AutoSize = true;
            this.fan2checkBox.Checked = true;
            this.fan2checkBox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.fan2checkBox.Location = new System.Drawing.Point(26, 44);
            this.fan2checkBox.Name = "fan2checkBox";
            this.fan2checkBox.Size = new System.Drawing.Size(129, 17);
            this.fan2checkBox.TabIndex = 4;
            this.fan2checkBox.Text = "Error with second fan ";
            this.fan2checkBox.UseVisualStyleBackColor = true;
            this.fan2checkBox.CheckedChanged += new System.EventHandler(this.fan2checkBox_CheckedChanged);
            // 
            // chipErrorcheckBox
            // 
            this.chipErrorcheckBox.AutoSize = true;
            this.chipErrorcheckBox.Checked = true;
            this.chipErrorcheckBox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chipErrorcheckBox.Location = new System.Drawing.Point(214, 19);
            this.chipErrorcheckBox.Name = "chipErrorcheckBox";
            this.chipErrorcheckBox.Size = new System.Drawing.Size(98, 17);
            this.chipErrorcheckBox.TabIndex = 5;
            this.chipErrorcheckBox.Text = "Error with chips";
            this.chipErrorcheckBox.UseVisualStyleBackColor = true;
            this.chipErrorcheckBox.CheckedChanged += new System.EventHandler(this.chipErrorcheckBox_CheckedChanged);
            // 
            // filterGroupBox
            // 
            this.filterGroupBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)));
            this.filterGroupBox.Controls.Add(this.button1);
            this.filterGroupBox.Controls.Add(this.button2);
            this.filterGroupBox.Controls.Add(this.chipErrorcheckBox);
            this.filterGroupBox.Controls.Add(this.fan1checkBox);
            this.filterGroupBox.Controls.Add(this.fan2checkBox);
            this.filterGroupBox.Controls.Add(this.UniqueErrorcheckBox);
            this.filterGroupBox.Controls.Add(this.hashBoardErrorcheckBox);
            this.filterGroupBox.Location = new System.Drawing.Point(5, 3);
            this.filterGroupBox.Name = "filterGroupBox";
            this.filterGroupBox.Size = new System.Drawing.Size(523, 104);
            this.filterGroupBox.TabIndex = 6;
            this.filterGroupBox.TabStop = false;
            this.filterGroupBox.Text = "FilterBox";
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(349, 8);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(162, 36);
            this.button1.TabIndex = 10;
            this.button1.Text = "Clear filters";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(349, 49);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(162, 36);
            this.button2.TabIndex = 9;
            this.button2.Text = "Refresh data";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // tableGroupBox
            // 
            this.tableGroupBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tableGroupBox.Controls.Add(this.errorDataGridView);
            this.tableGroupBox.Location = new System.Drawing.Point(49, 173);
            this.tableGroupBox.Name = "tableGroupBox";
            this.tableGroupBox.Size = new System.Drawing.Size(1113, 327);
            this.tableGroupBox.TabIndex = 7;
            this.tableGroupBox.TabStop = false;
            this.tableGroupBox.Text = "TableBox";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(46, 13);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(154, 13);
            this.label1.TabIndex = 8;
            this.label1.Text = "Data creation time in local time:";
            // 
            // localTimeLabel
            // 
            this.localTimeLabel.AutoSize = true;
            this.localTimeLabel.Location = new System.Drawing.Point(206, 13);
            this.localTimeLabel.Name = "localTimeLabel";
            this.localTimeLabel.Size = new System.Drawing.Size(91, 13);
            this.localTimeLabel.TabIndex = 9;
            this.localTimeLabel.Text = "Current local time:";
            // 
            // lastUpdateTimer
            // 
            this.lastUpdateTimer.Tick += new System.EventHandler(this.lastUpdateTimer_Tick);
            // 
            // timeSincelabel
            // 
            this.timeSincelabel.AutoSize = true;
            this.timeSincelabel.Location = new System.Drawing.Point(206, 15);
            this.timeSincelabel.Name = "timeSincelabel";
            this.timeSincelabel.Size = new System.Drawing.Size(0, 13);
            this.timeSincelabel.TabIndex = 10;
            // 
            // groupBoxMinerState
            // 
            this.groupBoxMinerState.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)));
            this.groupBoxMinerState.Controls.Add(this.label10);
            this.groupBoxMinerState.Controls.Add(this.label11);
            this.groupBoxMinerState.Controls.Add(this.label12);
            this.groupBoxMinerState.Controls.Add(this.label13);
            this.groupBoxMinerState.Controls.Add(this.label9);
            this.groupBoxMinerState.Controls.Add(this.label8);
            this.groupBoxMinerState.Controls.Add(this.label7);
            this.groupBoxMinerState.Controls.Add(this.label6);
            this.groupBoxMinerState.Controls.Add(this.textBoxAverFreq);
            this.groupBoxMinerState.Controls.Add(this.textBoxTotalHashrate);
            this.groupBoxMinerState.Controls.Add(this.textBoxChipState);
            this.groupBoxMinerState.Controls.Add(this.textBoxIdealHash);
            this.groupBoxMinerState.Controls.Add(this.textBoxTempState);
            this.groupBoxMinerState.Controls.Add(this.textBoxFanSpeed);
            this.groupBoxMinerState.Controls.Add(this.textBoxCoreState);
            this.groupBoxMinerState.Controls.Add(this.textBoxOffside);
            this.groupBoxMinerState.Location = new System.Drawing.Point(361, 533);
            this.groupBoxMinerState.Name = "groupBoxMinerState";
            this.groupBoxMinerState.Size = new System.Drawing.Size(801, 213);
            this.groupBoxMinerState.TabIndex = 11;
            this.groupBoxMinerState.TabStop = false;
            this.groupBoxMinerState.Text = "Selected Miner to see its State";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(446, 155);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(85, 13);
            this.label10.TabIndex = 20;
            this.label10.Text = "Offside situation:";
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(446, 23);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(77, 13);
            this.label11.TabIndex = 19;
            this.label11.Text = "Ideal hashrate:";
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(446, 67);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(60, 13);
            this.label12.TabIndex = 18;
            this.label12.Text = "Fan speed:";
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Location = new System.Drawing.Point(446, 114);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(102, 13);
            this.label13.TabIndex = 17;
            this.label13.Text = "Open core situation:";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(34, 154);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(70, 13);
            this.label9.TabIndex = 16;
            this.label9.Text = "Temperature:";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(34, 115);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(73, 13);
            this.label8.TabIndex = 15;
            this.label8.Text = "Missing chips:";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(34, 66);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(77, 13);
            this.label7.TabIndex = 14;
            this.label7.Text = "Averange freq:";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(34, 24);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(78, 13);
            this.label6.TabIndex = 12;
            this.label6.Text = "Total hashrate:";
            // 
            // textBoxAverFreq
            // 
            this.textBoxAverFreq.Location = new System.Drawing.Point(182, 63);
            this.textBoxAverFreq.Name = "textBoxAverFreq";
            this.textBoxAverFreq.ReadOnly = true;
            this.textBoxAverFreq.Size = new System.Drawing.Size(196, 20);
            this.textBoxAverFreq.TabIndex = 13;
            // 
            // textBoxTotalHashrate
            // 
            this.textBoxTotalHashrate.Location = new System.Drawing.Point(182, 19);
            this.textBoxTotalHashrate.Name = "textBoxTotalHashrate";
            this.textBoxTotalHashrate.ReadOnly = true;
            this.textBoxTotalHashrate.Size = new System.Drawing.Size(196, 20);
            this.textBoxTotalHashrate.TabIndex = 12;
            // 
            // textBoxChipState
            // 
            this.textBoxChipState.Location = new System.Drawing.Point(182, 110);
            this.textBoxChipState.Name = "textBoxChipState";
            this.textBoxChipState.ReadOnly = true;
            this.textBoxChipState.Size = new System.Drawing.Size(196, 20);
            this.textBoxChipState.TabIndex = 11;
            // 
            // textBoxIdealHash
            // 
            this.textBoxIdealHash.Location = new System.Drawing.Point(553, 19);
            this.textBoxIdealHash.Name = "textBoxIdealHash";
            this.textBoxIdealHash.ReadOnly = true;
            this.textBoxIdealHash.Size = new System.Drawing.Size(207, 20);
            this.textBoxIdealHash.TabIndex = 7;
            // 
            // textBoxTempState
            // 
            this.textBoxTempState.Location = new System.Drawing.Point(182, 151);
            this.textBoxTempState.Name = "textBoxTempState";
            this.textBoxTempState.ReadOnly = true;
            this.textBoxTempState.Size = new System.Drawing.Size(196, 20);
            this.textBoxTempState.TabIndex = 10;
            // 
            // textBoxFanSpeed
            // 
            this.textBoxFanSpeed.Location = new System.Drawing.Point(553, 65);
            this.textBoxFanSpeed.Name = "textBoxFanSpeed";
            this.textBoxFanSpeed.ReadOnly = true;
            this.textBoxFanSpeed.Size = new System.Drawing.Size(207, 20);
            this.textBoxFanSpeed.TabIndex = 9;
            // 
            // textBoxCoreState
            // 
            this.textBoxCoreState.Location = new System.Drawing.Point(553, 110);
            this.textBoxCoreState.Name = "textBoxCoreState";
            this.textBoxCoreState.ReadOnly = true;
            this.textBoxCoreState.Size = new System.Drawing.Size(207, 20);
            this.textBoxCoreState.TabIndex = 8;
            // 
            // textBoxOffside
            // 
            this.textBoxOffside.Location = new System.Drawing.Point(553, 153);
            this.textBoxOffside.Name = "textBoxOffside";
            this.textBoxOffside.ReadOnly = true;
            this.textBoxOffside.Size = new System.Drawing.Size(207, 20);
            this.textBoxOffside.TabIndex = 6;
            // 
            // panel1
            // 
            this.panel1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panel1.Controls.Add(this.filterGroupBox);
            this.panel1.Location = new System.Drawing.Point(38, 30);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1135, 122);
            this.panel1.TabIndex = 11;
            // 
            // groupBox2
            // 
            this.groupBox2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox2.Controls.Add(this.textBoxAverangeTemp);
            this.groupBox2.Controls.Add(this.label14);
            this.groupBox2.Controls.Add(this.ChipsMissingtextBox);
            this.groupBox2.Controls.Add(this.HashboardFailtextBox);
            this.groupBox2.Controls.Add(this.replaceFantextBox);
            this.groupBox2.Controls.Add(this.HardBoardMisstextBox);
            this.groupBox2.Controls.Add(this.label5);
            this.groupBox2.Controls.Add(this.label4);
            this.groupBox2.Controls.Add(this.label3);
            this.groupBox2.Controls.Add(this.label2);
            this.groupBox2.Location = new System.Drawing.Point(49, 533);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(306, 213);
            this.groupBox2.TabIndex = 12;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Statistic Data for all miners";
            // 
            // ChipsMissingtextBox
            // 
            this.ChipsMissingtextBox.Location = new System.Drawing.Point(189, 107);
            this.ChipsMissingtextBox.Name = "ChipsMissingtextBox";
            this.ChipsMissingtextBox.ReadOnly = true;
            this.ChipsMissingtextBox.Size = new System.Drawing.Size(100, 20);
            this.ChipsMissingtextBox.TabIndex = 7;
            // 
            // HashboardFailtextBox
            // 
            this.HashboardFailtextBox.Location = new System.Drawing.Point(189, 79);
            this.HashboardFailtextBox.Name = "HashboardFailtextBox";
            this.HashboardFailtextBox.ReadOnly = true;
            this.HashboardFailtextBox.Size = new System.Drawing.Size(100, 20);
            this.HashboardFailtextBox.TabIndex = 6;
            // 
            // replaceFantextBox
            // 
            this.replaceFantextBox.Location = new System.Drawing.Point(189, 26);
            this.replaceFantextBox.Name = "replaceFantextBox";
            this.replaceFantextBox.ReadOnly = true;
            this.replaceFantextBox.Size = new System.Drawing.Size(100, 20);
            this.replaceFantextBox.TabIndex = 5;
            // 
            // HardBoardMisstextBox
            // 
            this.HardBoardMisstextBox.Location = new System.Drawing.Point(189, 53);
            this.HardBoardMisstextBox.Name = "HardBoardMisstextBox";
            this.HardBoardMisstextBox.ReadOnly = true;
            this.HardBoardMisstextBox.Size = new System.Drawing.Size(100, 20);
            this.HardBoardMisstextBox.TabIndex = 4;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(36, 110);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(95, 13);
            this.label5.TabIndex = 3;
            this.label5.Text = "Asic chips missing:";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(36, 82);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(120, 13);
            this.label4.TabIndex = 2;
            this.label4.Text = "Hashboard with failures:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(36, 56);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(97, 13);
            this.label3.TabIndex = 1;
            this.label3.Text = "Hashboard Missing";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(36, 33);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(110, 13);
            this.label2.TabIndex = 0;
            this.label2.Text = "Replace Fan needed:";
            // 
            // textBoxAverangeTemp
            // 
            this.textBoxAverangeTemp.Location = new System.Drawing.Point(189, 136);
            this.textBoxAverangeTemp.Name = "textBoxAverangeTemp";
            this.textBoxAverangeTemp.ReadOnly = true;
            this.textBoxAverangeTemp.Size = new System.Drawing.Size(100, 20);
            this.textBoxAverangeTemp.TabIndex = 9;
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Location = new System.Drawing.Point(36, 139);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(115, 13);
            this.label14.TabIndex = 8;
            this.label14.Text = "Averange temperature:";
            // 
            // ErrorWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1185, 758);
            this.Controls.Add(this.groupBoxMinerState);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.timeSincelabel);
            this.Controls.Add(this.localTimeLabel);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.tableGroupBox);
            this.Name = "ErrorWindow";
            this.Text = "Error Window";
            ((System.ComponentModel.ISupportInitialize)(this.errorDataGridView)).EndInit();
            this.filterGroupBox.ResumeLayout(false);
            this.filterGroupBox.PerformLayout();
            this.tableGroupBox.ResumeLayout(false);
            this.groupBoxMinerState.ResumeLayout(false);
            this.groupBoxMinerState.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataGridView errorDataGridView;
        private System.Windows.Forms.CheckBox fan1checkBox;
        private System.Windows.Forms.CheckBox UniqueErrorcheckBox;
        private System.Windows.Forms.CheckBox hashBoardErrorcheckBox;
        private System.Windows.Forms.CheckBox fan2checkBox;
        private System.Windows.Forms.CheckBox chipErrorcheckBox;
        private System.Windows.Forms.GroupBox filterGroupBox;
        private System.Windows.Forms.GroupBox tableGroupBox;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label localTimeLabel;
        private System.Windows.Forms.Timer lastUpdateTimer;
        private System.Windows.Forms.Label timeSincelabel;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.GroupBox groupBoxMinerState;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.TextBox ChipsMissingtextBox;
        private System.Windows.Forms.TextBox HashboardFailtextBox;
        private System.Windows.Forms.TextBox replaceFantextBox;
        private System.Windows.Forms.TextBox HardBoardMisstextBox;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox textBoxAverFreq;
        private System.Windows.Forms.TextBox textBoxTotalHashrate;
        private System.Windows.Forms.TextBox textBoxChipState;
        private System.Windows.Forms.TextBox textBoxTempState;
        private System.Windows.Forms.TextBox textBoxFanSpeed;
        private System.Windows.Forms.TextBox textBoxCoreState;
        private System.Windows.Forms.TextBox textBoxIdealHash;
        private System.Windows.Forms.TextBox textBoxOffside;
        private System.Windows.Forms.TextBox textBoxAverangeTemp;
        private System.Windows.Forms.Label label14;
    }
}