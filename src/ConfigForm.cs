using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BitcoinInfoMiner
{
    public partial class ConfigForm : Form
    {
        MainWindow parent;
        string [] minerIp;
        public ConfigForm(MainWindow parent, string []ip)
        {
            InitializeComponent();
            this.parent = parent;
            minerIp = ip;
            getDefaultPool();
        }
        public void getDefaultPool()
        {
            pool1RadioButtonMain.Checked = Settings.pool1.enabled;
            pool1textBoxPool.Text = Settings.pool1.url;
            pool1textBoxPWD.Text = Settings.pool1.psw;
            pool1textBoxSub.Text = Settings.pool1.worker;
            pool1postfixIp.Checked = Settings.pool1.postfixIp;
            pool1postfixNoChange.Checked = Settings.pool1.postfixNoChange;
            pool1postfixNone.Checked = Settings.pool1.postfixNone;
            //Pool2
            pool2RadioButtonMain.Checked = Settings.pool2.enabled;
            pool2textBoxPool.Text = Settings.pool2.url;
            pool2textBoxPWD.Text = Settings.pool2.psw;
            pool2textBoxSub.Text = Settings.pool2.worker;
            pool2postfixIp.Checked = Settings.pool2.postfixIp;
            pool2postfixNoChange.Checked = Settings.pool2.postfixNoChange;
            pool2postfixNone.Checked = Settings.pool2.postfixNone;
            //Pool3
            pool3RadioButtonMain.Checked = Settings.pool3.enabled;
            pool3textBoxPool.Text = Settings.pool3.url;
            pool3textBoxPWD.Text = Settings.pool3.psw;
            pool3textBoxSub.Text = Settings.pool3.worker;
            pool3postfixIp.Checked = Settings.pool3.postfixIp;
            pool3postfixNoChange.Checked = Settings.pool3.postfixNoChange;
            pool3postfixNone.Checked = Settings.pool3.postfixNone;
        }
        public void readPoolInfo()
        {
            //Pool1
            Settings.pool1.enabled = pool1RadioButtonMain.Checked;
            Settings.pool1.url = pool1textBoxPool.Text;
            Settings.pool1.psw = pool1textBoxPWD.Text;
            Settings.pool1.worker = pool1textBoxSub.Text;
            Settings.pool1.postfixIp = pool1postfixIp.Checked;
            Settings.pool1.postfixNoChange = pool1postfixNoChange.Checked;
            Settings.pool1.postfixNone = pool1postfixNone.Checked;
            //Pool2
            Settings.pool2.enabled = pool2RadioButtonMain.Checked;
            Settings.pool2.url = pool2textBoxPool.Text;
            Settings.pool2.psw = pool2textBoxPWD.Text;
            Settings.pool2.worker = pool2textBoxSub.Text;
            Settings.pool2.postfixIp = pool2postfixIp.Checked;
            Settings.pool2.postfixNoChange = pool2postfixNoChange.Checked;
            Settings.pool2.postfixNone = pool2postfixNone.Checked;
            //Pool3
            Settings.pool3.enabled = pool3RadioButtonMain.Checked;
            Settings.pool3.url = pool3textBoxPool.Text;
            Settings.pool3.psw = pool3textBoxPWD.Text;
            Settings.pool3.worker = pool3textBoxSub.Text;
            Settings.pool3.postfixIp = pool3postfixIp.Checked;
            Settings.pool3.postfixNoChange = pool3postfixNoChange.Checked;
            Settings.pool3.postfixNone = pool3postfixNone.Checked;
        }
        public async Task configAction()
        {
            parent.Visible = true;
            this.Visible = false;
            parent.enableConfigButtons(false);
            parent.enableRebootButtons(false);
            parent.enableMonitorButtons(false);
            int limit = 20;
            Task[] taskArray = new Task[limit];
            
            int curLimit = 0;
            int lowLimit = 0;
            try
            {
                while (curLimit < minerIp.Length)
                {
                    lowLimit = curLimit;
                    curLimit += limit;
                    for (int iter = 0; (iter + lowLimit) < minerIp.Length && iter < limit; iter++)
                    {
                        //taskArray[iter] = parent.configMiner(minerIp[iter]);
                        taskArray[iter] = new Task(async (object obj) =>
                        {
                            int iterIner = Convert.ToInt32(obj);
                            await MinerChangeFunc.configMiner(minerIp[iterIner], iterIner).ConfigureAwait(false);
                        }, iter + lowLimit);
                        taskArray[iter].Start();
                    }
                    try
                    {
                        await Task.WhenAll(taskArray);

                    }
                    catch (Exception ex)
                    {
                        Log.logDebug("\n\r WhenAll Config " + Convert.ToString(ex));
                    }
                }
            }
            catch(Exception ex)
            {
                lowLimit = 0;
            }
        }
        private async  void button1_Click(object sender, EventArgs e)
        {
            readPoolInfo();
            DialogResult confirmationWindow = MessageBox.Show("Do you want to config selected miners ?" +
                "(Right now selected " + minerIp.Length + "miners)"
                                   , "Config selected miners", MessageBoxButtons.YesNo);
            int tryAmount = 0;
            if (confirmationWindow == DialogResult.Yes)
            {
                DialogResult errorWindow = DialogResult.OK;
                Log.configReport = new Log.OperationResult(minerIp.ToList());
                while (errorWindow == DialogResult.OK)
                    await configAction().ContinueWith((x) =>
                    {
                        tryAmount++;
                        if (Log.configReport.errorCount > 0)
                        {


                                errorWindow = MessageBox.Show("Result: Success - " + Log.configReport.succesCount + "; From selected " + Log.configReport.totalCount + " miners.\n Repeat operation for failed?"
                              , "Result", MessageBoxButtons.OKCancel);
                                if (errorWindow != DialogResult.OK)
                                {

                                    Log.configReport.report("config");

                                }
                            
                        }
                        else
                        {
                            errorWindow = DialogResult.Cancel;
                            MessageBox.Show(" Result: Success - " + Log.configReport.succesCount + "; From selected " + Log.configReport.totalCount + " miners."
                          , "Result", MessageBoxButtons.OK);
                            Log.configReport.report("config");
                        }
                        
                        //if (Log.configReport.errorList.Keys.Count > 0)
                        //{

                        //        errorWindow = MessageBox.Show("Result: Success - " + Log.configReport.succesCount + " Ignored- " + Log.configReport.noCount + "; From selected " + Log.configReport.totalCount + " miners.\n Repeat operation for failed?"
                        //      , "Result", MessageBoxButtons.OKCancel);
                        //        if (errorWindow == DialogResult.OK)
                        //        {
                        //            string[] newArray = Log.configReport.errorList.Keys.ToArray();
                        //            minerIp = new string[Log.configReport.errorList.Keys.Count];
                        //            for (int i = 0; i < minerIp.Length; i++)
                        //            {
                        //                minerIp[i] = newArray[i];
                        //            }
                        //            Log.configReport = new Log.OperationResult(newArray.Length);
                        //        }
                        //        else
                        //        {
                        //            Log.configReport.report("config");

                        //        }
                        //    //}
                        //}
                        //else
                        //{
                        //    errorWindow = DialogResult.Cancel;
                        //    MessageBox.Show("RTT Result: Success - " + Log.configReport.succesCount + " Ignored- " + Log.configReport.noCount + "; From selected " + Log.configReport.totalCount + " miners."
                        //  , "Result", MessageBoxButtons.OK);
                        //    Log.configReport.report("config");
                        //}

                    });

                parent.enableConfigButtons(true);
                parent.enableRebootButtons(true);
                parent.enableMonitorButtons(true);
                this.Close();
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            parent.Visible = true;
            parent.enableConfigButtons(true);
            parent.enableRebootButtons(true);
            parent.enableMonitorButtons(true);
            this.Close();
        }

        private void ConfigForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            parent.Visible = true;

            readPoolInfo();
        }

        private void ConfigForm_Load(object sender, EventArgs e)
        {

        }
    }
}
