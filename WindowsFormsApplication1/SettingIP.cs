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
using System.Net;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Sockets;
using System.Diagnostics;
namespace BitcoinInfoMiner
{
    public partial class SettingIP : Form
    {
        private OpenFileDialog openFileDialog= new System.Windows.Forms.OpenFileDialog();
        string[] csvData;
        int SuccessCount = 0;
        int startCount = 0;
        int endCount = 0;
        public string logText = "";
        MainWindow parent;
        public static OperationStaticIpState staticReport;
        public class ipReproterLine
        {
            int id { get; set; }
            public string oldIp { get; set; }
            public string newIp { get; set; }
            string state { get; set; }
            public ipReproterLine(int id,string oldIp,string newIp)
            {
                this.id = id;
                this.oldIp = oldIp;
                this.newIp = newIp;
                this.state = "Not Started";
            }
            public void setState(bool result)
            {
                if (result)
                {
                    state = "Success";
                }
                else
                {
                    state = "Error";
                }
            }
            public string toStringLine()
            {
                return id + "," + oldIp + "," + newIp + "," + state + ",\n";
            }
        }
        public class OperationStaticIpState
        {
            public OperationStaticIpState(int count)
            {
                totalCount = count;
                errorList = new Dictionary<string, string>();
                rowList = new List<ipReproterLine>();
            }
            public Dictionary<string, string> errorList { get; set; }
            public int totalCount { get; set; }
            public List<ipReproterLine> rowList { get; set; }
            public int succesCount
            {
                get
                {
                    if (totalCount > 0)
                        return totalCount - errorCount;
                    else
                        return 0;
                }
                set
                {

                }
            }
            public int errorCount
            {
                get
                {
                    if (errorList != new Dictionary<string, string>())
                        return errorList.Keys.Count;
                    else
                        return 0;
                }
                set
                {

                }
            }
            public string errorString()
            {
                string result = "";
                foreach (String key in errorList.Keys)
                {
                    result += key + ":" + errorList[key] + "\n";
                }
                return result;
            }
            public void addError(string oldIp,string newIp, string msg)
            {
                rowList.Single(t => t.newIp == newIp && t.oldIp == oldIp).setState(false);
                string ip = "Old ip:" + oldIp + "\nNewIp:" + newIp;
                if (errorList.Keys.Contains(ip))
                    errorList[ip] = msg;
                else
                    errorList.Add(ip, msg);
            }
            public void setSuccess(string oldIp, string newIp)
            {
                rowList.Single(t => t.newIp == newIp && t.oldIp == oldIp).setState(true);
            }
            public void report()
            {
                if (errorCount > 0)
                {
                    Log.createLogOperation("ConfigStatic Ip FAIL "+errorCount+" ", errorString());
                }
                errorList.Clear();
                totalCount = 0;
            }
            public string reportSuccess(string log="")
            {
                string fileName=Application.StartupPath+"\\SUCCESSFUL"+succesCount+ " miners "+DateTime.Now.ToString("dd-MM-yyyy HH-mm")+".csv";
                while (File.Exists(fileName))
                {
                    fileName = fileName + "1";
                }
                string text = "Id,Olp IP, New IP,Success, You may Use this file to retry setting Static IP\n";
                foreach(ipReproterLine row in rowList)
                {
                    text += row.toStringLine();
                }
                text = text +log;
                File.WriteAllText(fileName, text, Encoding.Unicode);
                return fileName;
            }

        }
        public SettingIP(MainWindow parent)
        {
            InitializeComponent();
            this.parent = parent;
            textBoxDnsservers.Text = "8.8.8.8";
            textBoxGateway.Text = "192.168.1.1";
            textBoxHostName.Text = "antMiner";
            textBoxNetmask.Text = "255.255.255.0";
        }
        public void  parseCsvFile(string fileMame)
        {
            try
            {
                if (!string.IsNullOrWhiteSpace(openFileDialog.FileName) && File.Exists(openFileDialog.FileName) )
                {
                    string parseData = File.ReadAllText(fileMame);
                    csvData = parseData.Split(new char[] { '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries);

                    for (int iter = 1; iter < csvData.Length; iter++)
                    {
                        if (csvData[iter] != "")
                        {
                            string[] thisLineParse = csvData[iter].Split(new char[] { ',' });
                            csvData[iter] = thisLineParse[1];
                        }
                    }



                    //DialogResult errorWindow = MessageBox.Show("Ошибка загрузки файлов,возможно файл с таким названием уже присутствует на сервере"
                    //          , "Ошибка", MessageBoxButtons.OK);
                    int i = 1;
                    string[] ipParse = csvData[i].Split(new char[] { '.' }, StringSplitOptions.RemoveEmptyEntries);
                    while (ipParse.Length < 3)
                    {
                        i++;
                        ipParse = csvData[i].Split(new char[] { '.' }, StringSplitOptions.RemoveEmptyEntries);
                    }
                    numericUpDown1.Value = Convert.ToInt32(ipParse[0], new CultureInfo("en"));
                    numericUpDown2.Value = Convert.ToInt32(ipParse[1], new CultureInfo("en"));
                    numericUpDown3.Value = Convert.ToInt32(ipParse[2], new CultureInfo("en"));
                    numericUpDown4.Value = 2;//Convert.ToInt32(ipParse[3]);
                    label2.Text = "Miner count: " + Convert.ToString(csvData.Length - 1, new CultureInfo("en"));
                    button2.Enabled = true;
                    button4.Enabled = true;
                }
            }
            catch (Exception ex)
            {
                Log.logDebug("New Error csv import wtf ,error:" + Convert.ToString(ex));
            }

        }
        private void button1_Click(object sender, EventArgs e)
        {
            openFileDialog.ShowDialog();
            parseCsvFile(openFileDialog.FileName);


        }
        public void setNetwork()
        {

        }
        private async Task postToUrl(string ip,string newIP)
        {
            try
            {
               
                var values = new Dictionary<string, string>
            {
            { "_ant_conf_nettype", "Static" },
            { "_ant_conf_hostname", textBoxHostName.Text },
            { "_ant_conf_ipaddress", newIP },
            { "_ant_conf_netmask", textBoxNetmask.Text },
            { "_ant_conf_gateway", textBoxGateway.Text },
            { "_ant_conf_dnsservers", textBoxDnsservers.Text }
            };
                startCount++;
                string url = "http://" + WebCalls.minerLogin + ":" + WebCalls.minerPass + "@"  +ip + "/cgi-bin/set_network_conf.cgi";
                //var url = "http://root:root@192.168.1.229/cgi-bin/set_network_conf.cgi";
                HttpClientHandler handler = new HttpClientHandler();
                handler.Credentials = new System.Net.NetworkCredential(WebCalls.minerLogin ,WebCalls.minerPass);
                HttpClient client = new HttpClient(handler);
                System.Net.ServicePointManager.Expect100Continue = false;
                client.DefaultRequestHeaders.ExpectContinue = false;
                client.Timeout=new TimeSpan(0,0,20);
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Authorization", "Digest username=\"root\", realm=\"antMiner Configuration\",uri=\"/cgi-bin/set_network_conf.cgi\"");
                client.DefaultRequestHeaders.Add("Accept", "application/json, text/javascript, */*; q=0.01");
                var content = new FormUrlEncodedContent(values);
                content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/json");

                logText += url + "     " + content;
                var response = client.PostAsync(url, content).Result;

                var responseString = await response.Content.ReadAsStringAsync();
                logText += responseString;
                staticReport.setSuccess(ip, newIP);
                
               // return true;
            }
            catch (HttpRequestException exception)
            {
                staticReport.addError(ip, newIP, exception.InnerException.Message);
            }
            catch (TaskCanceledException exception)
            {
                staticReport.addError(ip, newIP, exception.InnerException.Message);
            }
            catch (Exception exception)
            {
                //Log.logDebug("Reboot Miner "+Convert.ToString(exception, new CultureInfo("en")));
                staticReport.addError(ip , newIP, Convert.ToString(exception, new CultureInfo("en")));
                logText += Convert.ToString(exception, new CultureInfo("en"));
            }
            endCount++;
        }


        public static async Task<bool> postToUrl(string ip, MinerModel model)
        {
            try
            {
                System.Net.ServicePointManager.Expect100Continue = false;
                var values = new Dictionary<string, string>
            {
            { "_ant_conf_nettype", "Static" },
            { "_ant_conf_hostname", model.conf_hostname },
            { "_ant_conf_ipaddress", model.ip },
            { "_ant_conf_netmask", model.conf_netmask },
            { "_ant_conf_gateway", model.conf_gateway },
            { "_ant_conf_dnsservers", model.conf_dnsservers }
            };

                string url = "http://" + WebCalls.minerLogin + ":" + WebCalls.minerPass + "@" + ip + "/cgi-bin/set_network_conf.cgi";
                HttpClientHandler handler = new HttpClientHandler();
                handler.Credentials = new System.Net.NetworkCredential(WebCalls.minerLogin, WebCalls.minerPass);
                HttpClient client = new HttpClient(handler);
                client.DefaultRequestHeaders.ExpectContinue = false;
                client.Timeout = new TimeSpan(0, 5, 0);
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Authorization", "Digest username=\"root\", realm=\"antMiner Configuration\",uri=\"/cgi-bin/set_network_conf.cgi\"");
                client.DefaultRequestHeaders.Add("Accept", "application/json, text/javascript, */*; q=0.01");
                var content = new FormUrlEncodedContent(values);
                content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/json");
                var response = client.PostAsync(url, content).Result;

                var responseString = await response.Content.ReadAsStringAsync();

                return true;
            }
            catch (HttpRequestException exception)
            {
                Log.logDebug("postToUrl " + Convert.ToString(exception, new CultureInfo("en")));
                return false;
            }
            catch (TaskCanceledException exception)
            {
                Log.logDebug("postToUrl " + Convert.ToString(exception, new CultureInfo("en")));
                return false;
            }
            catch (Exception exception)
            {
                Log.logDebug("postToUrl " + Convert.ToString(exception, new CultureInfo("en")));
                
                return false;
            }
          
        }



        private void numericUpDown2_ValueChanged(object sender, EventArgs e)
        {
            numericUpDown6.Value = numericUpDown2.Value;
        }

        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {
            numericUpDown5.Value = numericUpDown1.Value;
        }

        private void numericUpDown3_ValueChanged(object sender, EventArgs e)
        {
            if ((csvData.Length + numericUpDown4.Value) < 256)
                numericUpDown7.Value = numericUpDown3.Value;
            else
            {
                numericUpDown7.Value = numericUpDown3.Value + (int)(csvData.Length/255);
            }
        }

        private void numericUpDown4_ValueChanged(object sender, EventArgs e)
        {
            if ((numericUpDown4.Value + csvData.Length - 1)<255)
                numericUpDown8.Value = numericUpDown4.Value+csvData.Length-2;
            else
            {
                numericUpDown8.Value = numericUpDown4.Value + (csvData.Length - ((int)(csvData.Length/255))*255) - 2;
            }
            if ((csvData.Length + numericUpDown4.Value) < 256)
                numericUpDown7.Value = numericUpDown3.Value;
            else
            {
                numericUpDown7.Value = numericUpDown3.Value + (int)(csvData.Length / 255);
            }
        }

        private async void button2_Click(object sender, EventArgs e)
        {
            try
            {
                DialogResult errorWindow = MessageBox.Show("Do you want to set Static ip for  " + Convert.ToString(csvData.Length - 1, new CultureInfo("en")) + " miners", "Set static ip", MessageBoxButtons.YesNo);
                string[] tempData = new string[csvData.Length - 1];
                staticReport = new OperationStaticIpState(csvData.Length - 1);
                if (errorWindow == DialogResult.Yes)
                {
                    if (numericUpDown3.Value != numericUpDown7.Value)
                    {

                        DialogResult thisWindow = MessageBox.Show("iP Range overFlow", "ERROR", MessageBoxButtons.OK);
                        return;
                    }
                    button2.Enabled = false;
                    this.Enabled = false;
                    progressBar2.Maximum = csvData.Length - 1;


                    DialogResult checkWindow = DialogResult.OK;
                    while (checkWindow == DialogResult.OK)
                        await setIp().ContinueWith(async (x) =>
                        {
                            string filename = staticReport.reportSuccess(logText);

                            try
                            {
                            if (staticReport.errorCount > 0)
                            {
                                checkWindow = MessageBox.Show("Result: Success - " + staticReport.succesCount + "; From selected " + staticReport.totalCount + " miners.\n Repeat operation for failed?"
                              , "Result", MessageBoxButtons.OKCancel);
                                if (checkWindow == DialogResult.OK)
                                {
                                    await setNewIpStandart().ConfigureAwait(false);
                                }
                                else
                                {
                                    Process.Start(filename);
                                    DialogResult resultWindow = MessageBox.Show("You may see all new ip in " + filename, "Result", MessageBoxButtons.OK);

                                    staticReport.report();
                                }
                            }
                            else
                            {
                                checkWindow = DialogResult.Cancel;
                                Process.Start(filename);
                                DialogResult resultWindow = MessageBox.Show("You may see all new ip in " + filename, "Result", MessageBoxButtons.OK);

                                staticReport.report();
                            }
                            }
                            catch { }

                        });


                }
                startCount = 0;
                SuccessCount = 0;
                endCount = 0;
                button2.Enabled = true;
                this.Enabled = true;
                parent.addRangeIp("IPReporter:" + tempData[0] + "-" + tempData[tempData.Count() - 1]);
                //LAN:192.168.1.2-192.168.1.100
            }
            catch(Exception ex)
            {
                Log.logDebug("Apply setting ip, Error"+Convert.ToString(ex));
            }
            
        }
        public async Task setIp()
        {
            string[] tempData = new string[csvData.Length - 1];
            Task[] taskArray = new Task[csvData.Length - 1];
            for (int iter = 0; iter < csvData.Length - 1; iter++)
            {
                if (csvData[iter + 1] != "")
                {

                    taskArray[iter] = new Task(async (Object obj) =>
                    {
                        int iterIner = Convert.ToInt32(obj);
                        string newIP = "";
                        try
                        {
                            int parsedIter = (iterIner + (int)numericUpDown4.Value - ((int)((iterIner + numericUpDown4.Value) / 255)) * 255);
                            //Надо пересмотреть на ошибки.
                            while (parsedIter == 0 || parsedIter == 255 || (parsedIter == 1 && (numericUpDown3.Value + (int)((iterIner + numericUpDown4.Value) / 255)) == 1))
                            {
                                numericUpDown4.Value++;
                                parsedIter = (iterIner + (int)numericUpDown4.Value - ((int)((iterIner + numericUpDown4.Value) / 255)) * 255);
                            }
                            newIP = Convert.ToString(numericUpDown1.Value, new CultureInfo("en")) + "."
                                + Convert.ToString(numericUpDown2.Value, new CultureInfo("en")) + "."
                                + Convert.ToString(numericUpDown3.Value + (int)((iterIner + numericUpDown4.Value) / 255), new CultureInfo("en"))
                                + "." + Convert.ToString(parsedIter, new CultureInfo("en"));
                            staticReport.rowList.Add(new ipReproterLine(iter + 1, csvData[iterIner + 1], newIP));
                            await postToUrl(csvData[iterIner + 1], newIP).ConfigureAwait(false);
                            tempData[iterIner] = newIP;
                        }
                        catch (Exception exception)
                        {
                            staticReport.addError(csvData[iterIner + 1], newIP, exception.InnerException.Message);
                            Log.logDebug("Setting Static ip " + Convert.ToString(exception));
                            logText += Convert.ToString(exception, new CultureInfo("en"));
                        }
                    }, iter);
                    taskArray[iter].Start();
                }
                else
                {

                    int parsedIter = (iter + (int)numericUpDown4.Value - ((int)((iter + numericUpDown4.Value) / 255)) * 255);
                    while (parsedIter == 0 || parsedIter == 255 || (parsedIter == 1 && (numericUpDown3.Value + (int)((iter + numericUpDown4.Value) / 255)) == 1))
                    {
                        numericUpDown4.Value++;
                        parsedIter = (iter + (int)numericUpDown4.Value - ((int)((iter + numericUpDown4.Value) / 255)) * 255);
                    }
                    string newIP = Convert.ToString(numericUpDown1.Value, new CultureInfo("en")) + "."
                        + Convert.ToString(numericUpDown2.Value, new CultureInfo("en")) + "."
                        + Convert.ToString(numericUpDown3.Value + (int)((iter + numericUpDown4.Value) / 255), new CultureInfo("en"))
                        + "." + Convert.ToString(parsedIter, new CultureInfo("en"));
                    staticReport.addError(csvData[iter + 1], newIP, "OldIp empty");
                    taskArray[iter] = Task.FromResult(false);
                }
            }
            
            await Task.WhenAll(taskArray);


            
        }




        public async Task setIpStandart()
        {
            string[] tempData = new string[csvData.Length - 1];          
            for (int iter = 0; iter < csvData.Length - 1; iter++)
            {
                if (csvData[iter + 1] != "")
                {
                        string newIP = "";
                        try
                        {
                            int parsedIter = (iter + (int)numericUpDown4.Value - ((int)((iter + numericUpDown4.Value) / 255)) * 255);
                            //Надо пересмотреть на ошибки.
                            while (parsedIter == 0 || parsedIter == 255 || (parsedIter == 1 && (numericUpDown3.Value + (int)((iter + numericUpDown4.Value) / 255)) == 1))
                            {
                                numericUpDown4.Value++;
                                parsedIter = (iter + (int)numericUpDown4.Value - ((int)((iter + numericUpDown4.Value) / 255)) * 255);
                            }
                            newIP = Convert.ToString(numericUpDown1.Value,new CultureInfo("en")) + "."
                                + Convert.ToString(numericUpDown2.Value, new CultureInfo("en")) + "."
                                + Convert.ToString(numericUpDown3.Value + (int)((iter + numericUpDown4.Value) / 255), new CultureInfo("en"))
                                + "." + Convert.ToString(parsedIter, new CultureInfo("en"));
                            staticReport.rowList.Add(new ipReproterLine(iter + 1, csvData[iter + 1], newIP));
                            await postToUrl(csvData[iter + 1], newIP);
                            tempData[iter] = newIP;
                        }
                        catch (Exception exception)
                        {
                            staticReport.addError(csvData[iter + 1], newIP, exception.InnerException.Message);
                            Log.logDebug("Setting Static ip " + Convert.ToString(exception));
                            logText += Convert.ToString(exception, new CultureInfo("en"));
                        }

                }
                else
                {

                    int parsedIter = (iter + (int)numericUpDown4.Value - ((int)((iter + numericUpDown4.Value) / 255)) * 255);
                    while (parsedIter == 0 || parsedIter == 255 || (parsedIter == 1 && (numericUpDown3.Value + (int)((iter + numericUpDown4.Value) / 255)) == 1))
                    {
                        numericUpDown4.Value++;
                        parsedIter = (iter + (int)numericUpDown4.Value - ((int)((iter + numericUpDown4.Value) / 255)) * 255);
                    }
                    string newIP = Convert.ToString(numericUpDown1.Value, new CultureInfo("en")) + "."
                        + Convert.ToString(numericUpDown2.Value, new CultureInfo("en")) + "."
                        + Convert.ToString(numericUpDown3.Value + (int)((iter + numericUpDown4.Value) / 255), new CultureInfo("en"))
                        + "." + Convert.ToString(parsedIter, new CultureInfo("en"));
                    staticReport.addError(csvData[iter + 1], newIP, "OldIp empty");
                }
            }
         



        }
        public void createSuccessFile()
        {

        }
        private bool checkSuccess(string[] ipData)
        {
            for (int iter = 0; iter < ipData.Length;iter++ )
            {

                //http://192.168.1.221/cgi-bin/get_network_info.cgi
                System.Net.ServicePointManager.Expect100Continue = false;
                var url = "http://"+WebCalls.minerLogin + ":" + WebCalls.minerPass+"@" + ipData[iter] + "/cgi-bin/get_network_info.cgi";
                var webRequest = (HttpWebRequest)HttpWebRequest.Create(url);
                webRequest.ContentType = "application/json";
                webRequest.Method = "GET";
                webRequest.Credentials = new System.Net.NetworkCredential(WebCalls.minerLogin, WebCalls.minerPass);
                webRequest.Timeout = 300;
                try
                {
                    var httpResponse = (WebResponse)webRequest.GetResponse();
                    string result = "";
                    using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                    {
                        result = streamReader.ReadToEnd();
                    }
                    SuccessCount++;
                }
                catch (Exception exception)
                {
                    Log.logDebug("Setting Static ip|Check Succes " + Convert.ToString(exception));
                    logText += Convert.ToString(exception, new CultureInfo("en"));
                }

               
            }


            MessageBox.Show("Result. Success:" + Convert.ToString(SuccessCount, new CultureInfo("en")) + "/"
                + Convert.ToString(ipData.Length, new CultureInfo("en")), "Result", MessageBoxButtons.OK);
            return true;
        }
        private void button3_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void SettingIP_FormClosing(object sender, FormClosingEventArgs e)
        {
            File.WriteAllText(Application.StartupPath + "\\logError.txt", logText, Encoding.Unicode);
        }

        private void button3_Click_1(object sender, EventArgs e)
        {
            InputDialogForm testDialog = new InputDialogForm(this,
                "Write temporary ip range for this operation\r\n Example: if you write 192.168.2.2 ,then all ip\n\r from 192.168.2.2 till 192.168.2.254 should be empty",
                "Set temporary ip range");

            // Show testDialog as a modal dialog and determine if DialogResult = OK.
            if (testDialog.ShowDialog(this) == DialogResult.OK)
            {
                // Read the contents of testDialog's TextBox.
                string result = testDialog.textBoxResult.Text;
                if (result == "" || !checkIP(result))
                {
                    MessageBox.Show("Invalid ip format", "Error", MessageBoxButtons.OK);
                    testDialog.Dispose();
                    return;
                }
                else
                {
                    returnToDefault(result);
                }
            }
            testDialog.Dispose();
        }
        private bool checkIP(string ip)
        {
            return true;
        }
        private void returnToDefault(string ip)
        {

        }

        private async Task setNewIpStandart()
        {
            try
            {
                DialogResult errorWindow = MessageBox.Show("Do you want to set Static ip for  " + Convert.ToString(csvData.Length - 1, new CultureInfo("en")) + " miners. This may take some time", "Set static ip", MessageBoxButtons.YesNo);
                string[] tempData = new string[csvData.Length - 1];
                staticReport = new OperationStaticIpState(csvData.Length - 1);
                if (errorWindow == DialogResult.Yes)
                {
                    if (numericUpDown3.Value != numericUpDown7.Value)
                    {

                        DialogResult thisWindow = MessageBox.Show("iP Range overFlow", "ERROR", MessageBoxButtons.OK);
                        return;
                    }
                    button2.Enabled = false;
                    this.Enabled = false;
                    progressBar2.Maximum = csvData.Length - 1;


                    DialogResult checkWindow = DialogResult.OK;
                    while (checkWindow == DialogResult.OK)
                        await setIpStandart().ContinueWith(async (x) =>
                        {
                            string filename = staticReport.reportSuccess(logText);

                            try
                            {
                            if (staticReport.errorCount > 0)
                            {
                                checkWindow = MessageBox.Show("Result: Success - " + staticReport.succesCount + "; From selected " + staticReport.totalCount + " miners.\n Repeat operation for failed?"
                              , "Result", MessageBoxButtons.OKCancel);
                                if (checkWindow == DialogResult.OK)
                                {
                                    await setNewIpStandart();
                                }
                                else
                                {
                                    Process.Start(filename);
                                    DialogResult resultWindow = MessageBox.Show("You may see all new ip in " + filename, "Result", MessageBoxButtons.OK);

                                    staticReport.report();
                                }
                            }
                            else
                            {
                                checkWindow = DialogResult.Cancel;
                                Process.Start(filename);
                                DialogResult resultWindow = MessageBox.Show("You may see all new ip in " + filename, "Result", MessageBoxButtons.OK);

                                staticReport.report();
                            }
                            }
                            catch { }

                        });


                }




                startCount = 0;
                SuccessCount = 0;
                endCount = 0;
                button2.Enabled = true;
                this.Enabled = true;
                parent.addRangeIp("IPReporter:" + tempData[0] + "-" + tempData[tempData.Count() - 1]);
                //LAN:192.168.1.2-192.168.1.100
            }
            catch (Exception ex)
            {
                Log.logDebug("Apply setting ip, Error" + Convert.ToString(ex));
            }
            
        }


        private async void button4_Click(object sender, EventArgs e)
        {

            await setNewIpStandart();
        
        }



    }
}
