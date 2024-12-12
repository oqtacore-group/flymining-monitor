using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Globalization;
namespace BitcoinInfoMiner
{
    public partial class AddIpRangeForm : Form
    {

        BitcoinInfoMiner.MainWindow parent;
        string editingSave = "";
        public AddIpRangeForm(BitcoinInfoMiner.MainWindow parent)
        {
            InitializeComponent();
            editingSave = "";
            this.parent = parent;
            parent.Enabled = false;
        }
        public AddIpRangeForm(BitcoinInfoMiner.MainWindow parent,string text)
        {
            InitializeComponent();
            editingSave = text;
            textBox1.Text = text;
            this.parent = parent;
            parent.Enabled = false;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (editingSave!="")
                parent.addRangeIp(editingSave);
            parent.Visible = true;
            this.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            //parent.Visible = true;
            DialogResult errorWindow;
            
            if (parseIPRange(textBox1.Text))
            {
                if (editingSave != "")
                    errorWindow = MessageBox.Show("Successfully edited  ipRange"
                                    , "Result", MessageBoxButtons.OK);
                else
                    errorWindow = MessageBox.Show("Successfully added new ipRange"
                                    , "Result", MessageBoxButtons.OK);
                parent.addRangeIp(textBox1.Text);
                parent.Enabled = true;
                this.Close();
            }
            else
                errorWindow = MessageBox.Show("Error in ip range format "
                                    , "Result", MessageBoxButtons.OK);
            //Сохраняем добавленые ip


           
        }
        private bool parseIPRange(string ipRange)
        {
            //LAN:192.168.1.101-192.168.1.220
            string[] parse = ipRange.Split(new char[] { ':', '-' }, StringSplitOptions.None);
            //Проверка парсинга
            if (parse.Length != 3 || !parse[1].Contains("192.168.") || !parse[2].Contains("192.168.") || parse[1] == "192.168.1.1" || parse[2] == "192.168.1.1")
                return false;
            string[] parseIP = parse[1].Split(new char[] { '.' }, StringSplitOptions.RemoveEmptyEntries);
            //Проверка,чтобы каждое число было меьше 1000, было числом без ошибочных знаков и чтобы в конце не было 255||0
            if (parseIP.Length != 4 || parseIP[0].Length > 3 || parseIP[1].Length > 3 || parseIP[2].Length > 3 || parseIP[3].Length > 3
                || !parseIP[0].All(char.IsDigit) || !parseIP[1].All(char.IsDigit) || !parseIP[2].All(char.IsDigit)
                || !parseIP[3].All(char.IsDigit) || parseIP[3] == "255" || parseIP[3] == "0")
                return false;
            //Проверка,чтобы каждое число было меньше 255
            if (Convert.ToInt32(parseIP[0], CultureInfo.InvariantCulture) > 255 || Convert.ToInt32(parseIP[1], CultureInfo.InvariantCulture) > 255
                || Convert.ToInt32(parseIP[2], CultureInfo.InvariantCulture) > 255 || Convert.ToInt32(parseIP[3], CultureInfo.InvariantCulture) > 255)
                return false;
            string[] parseIPSave = parseIP;
            parseIP = parse[2].Split(new char[] { '.' }, StringSplitOptions.RemoveEmptyEntries);
            //Проверка,чтобы каждое число было меьше 1000, было числом без ошибочных знаков и чтобы в конце не было 255||0
            if (parseIP.Length != 4 || parseIP[0].Length > 3 || parseIP[1].Length > 3 || parseIP[2].Length > 3 || parseIP[3].Length > 3
                || !parseIP[0].All(char.IsDigit) || !parseIP[1].All(char.IsDigit) || !parseIP[2].All(char.IsDigit)
                || !parseIP[3].All(char.IsDigit) || parseIP[3] == "255" || parseIP[3] == "0")
                return false;
            //Проверка,чтобы каждое число было меньше 255
            if (Convert.ToInt32(parseIP[0], CultureInfo.InvariantCulture) > 255 || Convert.ToInt32(parseIP[1], CultureInfo.InvariantCulture) > 255
                || Convert.ToInt32(parseIP[2], CultureInfo.InvariantCulture) > 255 || Convert.ToInt32(parseIP[3], CultureInfo.InvariantCulture) > 255)
                return false;
            //Проверка,чтобы ip первой части был раньше ip второй части.
            if (Convert.ToInt32(parseIPSave[0], CultureInfo.InvariantCulture) != Convert.ToInt32(parseIP[0], CultureInfo.InvariantCulture)
                || Convert.ToInt32(parseIPSave[1], CultureInfo.InvariantCulture) != Convert.ToInt32(parseIP[1], CultureInfo.InvariantCulture)
                || Convert.ToInt32(parseIPSave[2], CultureInfo.InvariantCulture) != Convert.ToInt32(parseIP[2], CultureInfo.InvariantCulture)
                || Convert.ToInt32(parseIPSave[3], CultureInfo.InvariantCulture) >= Convert.ToInt32(parseIP[3], CultureInfo.InvariantCulture))
                return false;
            return true;

        }




        private void button3_Click_1(object sender, EventArgs e)
        {
            DialogResult errorWindow;
            if (parseIPRange(textBox1.Text))
            {
                if (editingSave != "")
                    errorWindow = MessageBox.Show("Successfully edited  ipRange"
                                    , "Result", MessageBoxButtons.OK);
                else
                    errorWindow = MessageBox.Show("Successfully added new ipRange"
                                    , "Result", MessageBoxButtons.OK);
            }
            else
                errorWindow = MessageBox.Show("Error in ip range format "
                                    , "Result", MessageBoxButtons.OK);
            //if (checkedListBox1.SelectedItem != null)
            //{
            //    if (!textBox1.Text.Contains(':'))
            //    {
            //        DialogResult errorWindow = MessageBox.Show("Перед ip адрессом должны быть : "
            //                        , "Предупреждение", MessageBoxButtons.OK);
            //        return;
            //    }
            //    checkedListBox1.Items.Remove(checkedListBox1.SelectedItem);
            //    checkedListBox1.Items.Add(textBox1.Text, true);
            //}
        }

        private void button4_Click_1(object sender, EventArgs e)
        {
            textBox1.Text = "LAN:192.168.1.2-192.168.1.100";
        }

        private void AddIpRangeForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            parent.Enabled = true;
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }



    }
}
