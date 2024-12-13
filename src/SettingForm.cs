using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Text.RegularExpressions;
using System.Globalization;
using Microsoft.Win32;
namespace BitcoinInfoMiner
{
    
    public partial class SettingForm : Form
    {
        BitcoinInfoMiner.MainWindow parent;
        public string userName;
        public string password;
        public string flyMiningUserName;
        public string flyMiningPassword;
        public int apiCheckStart;
        public bool apiCheckState;
        public bool onStartUp;
        public bool autoScan;
        public bool autoMonitoring;



        public int apiCheckTimeout;
        public int connectTimeout;

        public int abnormalTemp;

        public int minimalTemp;
        public int antMinerHashMin;

        public int monitoringTimeout;
        public int badCheckTimeout;

        public int badCheckFirstStart;
        public bool detectHighTemp;
        public SettingForm(BitcoinInfoMiner.MainWindow parent)
        {
            this.parent = parent;
            InitializeComponent();
            apiCheckStart = Settings.apiCheckStart;
            apiCheckState = Settings.apiCheckState;
            onStartUp = Settings.onStartUp;
            autoScan = Settings.autoScan;
            autoMonitoring = Settings.autoMonitoring;


            apiCheckTimeout = Settings.apiCheckTimeout;
            connectTimeout= WebCalls.connectTimeout;
            abnormalTemp = Settings.abnormalTemp;
            minimalTemp = Settings.minimalTemp;
            antMinerHashMin = Settings.antMinerHashMin;
            monitoringTimeout = Settings.monitoringTimeout;
            badCheckTimeout = Settings.badCheckTimeout;
            badCheckFirstStart = Settings.badCheckFirstStart;
            userName = WebCalls.minerLogin;
            password = WebCalls.minerPass;
            flyMiningPassword = Log.flyMiningPassword;
            flyMiningUserName = Log.flyMiningUserName;
            detectHighTemp = Settings.detectHighTemp;
            SetSettingOnUI();
        }
        private void GetSetting()
        {
            //Получаем настройки из MainWindow
            
        }
        private void SetSettingOnUI()
        {
            //Присваиваем настроки к ui
            isLogged();
            textBox6.Text = Convert.ToString(connectTimeout / 100, CultureInfo.InvariantCulture);
            textBoxApiStart.Text = Convert.ToString(apiCheckStart / (60 * 1000), CultureInfo.InvariantCulture);
            textBoxApiTimeout.Text = Convert.ToString(apiCheckTimeout / (60 * 1000), CultureInfo.InvariantCulture);
            checkBoxApiCheck.Checked = apiCheckState;
            chkStartUp.Checked = onStartUp;
            checkBoxAutoScan.Checked = autoScan;
            checkBoxMonitoringS.Checked = autoMonitoring;


            textBox3.Text = Convert.ToString(abnormalTemp, CultureInfo.InvariantCulture);
            textBox4.Text = Convert.ToString(minimalTemp, CultureInfo.InvariantCulture);
            textBox5.Text = Convert.ToString(antMinerHashMin, CultureInfo.InvariantCulture);
            textBox1.Text = Convert.ToString(monitoringTimeout / 1000, CultureInfo.InvariantCulture);
            textBox2.Text = Convert.ToString(badCheckTimeout / (60 * 1000), CultureInfo.InvariantCulture);
            textBoxLogin.Text = userName;
            textBoxPass.Text=password;
            textBoxFlyminingID.Text = flyMiningUserName;
            textBoxFlyMiningPass.Text = flyMiningPassword;
            checkBox1.Checked = detectHighTemp;
            if (Log.emailForBadStatus != null)
                textBox7.Text = String.Join("/", Log.emailForBadStatus);
        }

  
        private void button1_Click(object sender, EventArgs e)
        {
            //Присвоение переменных настроек
            WebCalls.connectTimeout = connectTimeout;
            Settings.apiCheckStart = apiCheckStart;
            Settings.apiCheckState = apiCheckState;
            Settings.onStartUp = onStartUp;
            Settings.autoScan = autoScan;
            Settings.autoMonitoring = autoMonitoring;
            Settings.apiCheckTimeout = apiCheckTimeout;
            Settings.abnormalTemp = abnormalTemp;
            Settings.minimalTemp = minimalTemp;
            Settings.antMinerHashMin = antMinerHashMin;
            Settings.monitoringTimeout = monitoringTimeout;
            Settings.badCheckTimeout = badCheckTimeout;
            Settings.badCheckFirstStart = badCheckFirstStart;
            WebCalls.minerLogin = userName;
            WebCalls.minerPass = password;
            Log.flyMiningUserName = flyMiningUserName;
            Log.flyMiningPassword = flyMiningPassword;
            Settings.detectHighTemp = detectHighTemp;
            SetStartup();
            Settings.saveSettings(parent);
            parent.Enabled = true;
            Settings.checkLogin();
            this.Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            parent.Enabled = true;
            this.Close();
        }

        private void SettingForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            parent.Enabled = true;
        }

        private void textBox6_TextChanged(object sender, EventArgs e)
        {
            if (Regex.IsMatch(textBox6.Text, @"^\d+$") && textBox6.Text != "")
                connectTimeout = Convert.ToInt32(textBox6.Text, CultureInfo.InvariantCulture) * 100;
            else
            {
                textBox6.Text = Convert.ToString(WebCalls.connectTimeout / 100, CultureInfo.InvariantCulture);
                DialogResult errorWindow = MessageBox.Show("Для ввода доступны только целые числа"
                                    , "Предупреждение", MessageBoxButtons.OK);
            }
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            if (Regex.IsMatch(textBox1.Text, @"^\d+$") && textBox1.Text != "")
                monitoringTimeout = Convert.ToInt32(textBox1.Text, CultureInfo.InvariantCulture) * 1000;
            else
            {
                textBox1.Text = Convert.ToString(Settings.monitoringTimeout / 1000, CultureInfo.InvariantCulture);
                DialogResult errorWindow = MessageBox.Show("Для ввода доступны только целые числа"
                                    , "Предупреждение", MessageBoxButtons.OK);
            }
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            if (Regex.IsMatch(textBox2.Text, @"^\d+$") && textBox2.Text != "")
                badCheckTimeout = Convert.ToInt32(textBox2.Text, CultureInfo.InvariantCulture) * 60 * 1000;
            else
            {
                textBox2.Text = Convert.ToString(Settings.badCheckTimeout / (60 * 1000), CultureInfo.InvariantCulture);
                DialogResult errorWindow = MessageBox.Show("Для ввода доступны только целые числа"
                                    , "Предупреждение", MessageBoxButtons.OK);
            }
        }

        private void textBox7_TextChanged(object sender, EventArgs e)
        {
            if (textBox7.Text != "")
                Log.emailForBadStatus = textBox7.Text.Split(new[] { '/' }, StringSplitOptions.RemoveEmptyEntries);
        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {
            if (Regex.IsMatch(textBox3.Text, @"^\d+$") && textBox3.Text!="")
                abnormalTemp = Convert.ToInt32(textBox3.Text, CultureInfo.InvariantCulture);
            else
            {
                textBox3.Text = Convert.ToString(Settings.abnormalTemp, CultureInfo.InvariantCulture);
                DialogResult errorWindow = MessageBox.Show("Для ввода доступны только целые числа"
                                    , "Предупреждение", MessageBoxButtons.OK);
            }
        }

        private void textBox4_TextChanged(object sender, EventArgs e)
        {
            if (Regex.IsMatch(textBox4.Text, @"^\d+$") && textBox4.Text != "")
                minimalTemp = Convert.ToInt32(textBox4.Text, CultureInfo.InvariantCulture);
            else
            {
                textBox4.Text = Convert.ToString(Settings.minimalTemp, CultureInfo.InvariantCulture);
                DialogResult errorWindow = MessageBox.Show("Для ввода доступны только целые числа"
                                    , "Предупреждение", MessageBoxButtons.OK);
            }
        }

        private void textBox5_TextChanged(object sender, EventArgs e)
        {
            if (Regex.IsMatch(textBox5.Text, @"^\d+$") && textBox5.Text != "")
                antMinerHashMin = Convert.ToInt32(textBox5.Text, CultureInfo.InvariantCulture);
            else
            {
                textBox5.Text = Convert.ToString(Settings.antMinerHashMin, CultureInfo.InvariantCulture);
                DialogResult errorWindow = MessageBox.Show("Для ввода доступны только целые числа"
                                    , "Предупреждение", MessageBoxButtons.OK);
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            connectTimeout = 1000;
            apiCheckStart = 30;
            apiCheckState = true;
            onStartUp=false;
            autoScan=false;
            autoMonitoring=false;
            apiCheckTimeout = 60;
            abnormalTemp = 90;
            minimalTemp = 10;
            antMinerHashMin = 10000; 
            monitoringTimeout = 30000; 
            badCheckTimeout = 1800000;
            badCheckFirstStart = 10000;
            password = "root";
            userName = "root";
            flyMiningPassword = "";
            flyMiningUserName = "";
            detectHighTemp = true;
            //Log.emailForBadStatus = new string[] { "support@flysecure.ru" };
            SetSettingOnUI();
        }

        private void textBoxLogin_TextChanged(object sender, EventArgs e)
        {
            userName = textBoxLogin.Text;
        }

        private void textBoxPass_TextChanged(object sender, EventArgs e)
        {
            password = textBoxPass.Text;
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            detectHighTemp = checkBox1.Checked;
        }

        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {

        }

        private void textBox8_TextChanged(object sender, EventArgs e)
        {
            flyMiningUserName = textBoxFlyminingID.Text;
        }

        private void SettingForm_Load(object sender, EventArgs e)
        {

        }

        private void textBoxFlyMiningPass_TextChanged(object sender, EventArgs e)
        {
            flyMiningPassword = textBoxFlyMiningPass.Text;
        }


        private void textBoxApiTimeout_TextChanged(object sender, EventArgs e)
        {
            if (Regex.IsMatch(textBoxApiTimeout.Text, @"^\d+$") && textBoxApiTimeout.Text != "")
                apiCheckTimeout = Convert.ToInt32(textBoxApiTimeout.Text, CultureInfo.InvariantCulture) * 60 * 1000;
            else
            {
                textBoxApiTimeout.Text = Convert.ToString(Settings.apiCheckTimeout / (60 * 1000), CultureInfo.InvariantCulture);
                DialogResult errorWindow = MessageBox.Show("Для ввода доступны только целые числа"
                                    , "Предупреждение", MessageBoxButtons.OK);
            }
        }

        private void textBoxApiStart_TextChanged(object sender, EventArgs e)
        {
            if (Regex.IsMatch(textBoxApiStart.Text, @"^\d+$") && textBoxApiStart.Text != "")
                apiCheckStart = Convert.ToInt32(textBoxApiStart.Text, CultureInfo.InvariantCulture) * 60 * 1000;
            else
            {
                textBoxApiStart.Text = Convert.ToString(Settings.apiCheckStart / (60 * 1000), CultureInfo.InvariantCulture);
                DialogResult errorWindow = MessageBox.Show("Для ввода доступны только целые числа"
                                    , "Предупреждение", MessageBoxButtons.OK);
            }
        }

        private void checkBoxApiCheck_CheckedChanged(object sender, EventArgs e)
        {
            apiCheckState = checkBoxApiCheck.Checked;
        }

           
        private void SetStartup()
        {
            RegistryKey rk = Registry.CurrentUser.OpenSubKey
                ("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run", true);
            if (chkStartUp.Checked)
                rk.SetValue("FlyMining", Application.ExecutablePath);
            else
                rk.DeleteValue("FlyMining", false);            
    }

        private void label22_Click(object sender, EventArgs e)
        {

        }

        private void chkStartUp_CheckedChanged(object sender, EventArgs e)
        {
            onStartUp = chkStartUp.Checked;
        }

        private void checkBoxMonitoringS_CheckedChanged(object sender, EventArgs e)
        {
            autoMonitoring = checkBoxMonitoringS.Checked;
        }

        private void checkBoxAutoScan_CheckedChanged(object sender, EventArgs e)
        {
            autoScan = checkBoxAutoScan.Checked;
        }
        private void isLogged(bool button=false)
        {
            if (Settings.logged)
            {
                buttonLogin.Visible = false;
                LoginInfoLabel.Visible = true;
                if (button)
                    LoginInfoLabel.Text = "Successfully logged.\n\r Please Restart Application";
                else
                    LoginInfoLabel.Text = "";
            }
            else
            {
                buttonLogin.Visible = true;
                LoginInfoLabel.Visible = true;
                LoginInfoLabel.Text = "Try Logging";
            }
        }

        private async void buttonLogin_Click(object sender, EventArgs e)
        {
            Log.flyMiningUserName = flyMiningUserName;
            Log.flyMiningPassword = flyMiningPassword;
            await Settings.checkLogin();
            isLogged(true);
        }

        private void label21_Click(object sender, EventArgs e)
        {

        }

        private void label8_Click(object sender, EventArgs e)
        {

        }
    }
}
