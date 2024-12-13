using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Web;
using System.Net;
using System.IO;
using System.Net.Sockets;
using System.Threading;
using System.Text.RegularExpressions;
using BitcoinInfoMiner;
using Renci.SshNet;
using System.Collections.Specialized;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Globalization;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Collections.Concurrent;
using System.Reflection;
using System.Diagnostics;
using static BitcoinInfoMiner.Settings;

namespace BitcoinInfoMiner
{
    public partial class MainWindow : Form
    {
        #region property

        public class usPoolResponce
        {
            public int err_no { get; set; }
            public JToken data { get; set; }
        }
        public partial class RsOperationsData
        {
            public int rsID { get; set; }
            public int Id { get; set; }
            public System.DateTime Date { get; set; }
            public string TxID { get; set; }
            public string Oper_Type { get; set; }
            public string Wallet_Type { get; set; }
            public string Wallet { get; set; }
            public string Sum_Value { get; set; }
            public string Currency_Name { get; set; }
            public string Description { get; set; }
            public string Special_Kind { get; set; }
            public string Rest { get; set; }
        }
        static BackgroundWorker backgroundWorker1 = new BackgroundWorker();
        public static string pathSetting = Application.StartupPath + "\\flymining.ini";
        TaskScheduler uiScheduler;
        private static bool testBool = false;
        public readonly static string logTestPath = Application.StartupPath + "\\logReport\\Test.csv";
        public readonly static string logTestPath2 = Application.StartupPath + "\\logReport\\Test2.csv";
        public readonly static string logRsPath = Application.StartupPath + "\\logReport\\Rs.csv";
        //0- Stats
        //1- Pools
        //2 -Restart
        //3 -enablePool
        //4 -addpool
        //5 -removepool
        //6 -config?


        private static DataTable mainTable;
        private static DataTable editedTable;
        static TimerCallback usdRateCallBack = new TimerCallback(GetUsdRateTimer);
        static System.Threading.Timer monitoringTimer;
        static System.Threading.Timer reportTimer;
        static System.Threading.Timer reportNowTimer;
        static System.Threading.Timer walletIncomeTimer;
        //static System.Threading.Timer dbUpdateTimer;
        static System.Threading.Timer usdRateTimer;
        static System.Threading.Timer hourlyReport;
        private static BindingSource bs;
        static private List<string> selectedCells;
        private HttpServer myHttpServer = new HttpServer(58031);
        #endregion

        private  void selectCells()
        {
            try
            {
                if (selectedCells.Count > 0)
                {
                    foreach (string cell in selectedCells)
                    {
                        string[] data = cell.Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries);
                        dataGridView1.Rows[Convert.ToInt32(data[0])].Cells[Convert.ToInt32(data[1])].Selected = true;
                    }
                    selectedCells.Clear();
                }
            }
            catch(Exception ex)
            {
                selectedCells.Clear();
            }
        }
        private void selectCellsFill()
        {
            try
            {
                foreach(DataGridViewRow row in dataGridView1.Rows)
                {
                    foreach(DataGridViewCell cell in row.Cells)
                    {
                        if (cell.Selected)
                        {
                            selectedCells.Add(cell.RowIndex + ";" + cell.ColumnIndex);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                selectedCells.Clear();
            }
        }

        public MainWindow()
        {

            InitializeComponent();
            Settings.initSettingsProperty();
            setLogFiles();
            mainTable = new DataTable();
            editedTable = new DataTable();
            Settings.currentStatus = new Settings.MinerStatus();
            Wallets.orderHistory = new Dictionary<int, OrderBody>();
            Wallets.walletInfo = new Dictionary<string, string[]>();
            Wallets.walletInfoDescription = new Dictionary<string, string[]>();
            Wallets.parseHistoricalRateHistory();
            DataGridViewCheckBoxColumn checkColumn = new DataGridViewCheckBoxColumn();
            Settings.readSettings();//Считываем настройки
            selectedCells = new List<string>();
            DataColumn column;
            bs = new BindingSource();
         

            column = new DataColumn();
            column.DataType = System.Type.GetType("System.Boolean");
            column.ColumnName = "State";
            column.Caption = "AutoReboot on problem";
;

            mainTable.Columns.Add(column);

            column = new DataColumn();
            column.DataType = System.Type.GetType("System.Boolean");
            column.ColumnName = "StateMail";
            column.Caption = "Report on problem";

            mainTable.Columns.Add(column);

            column = new DataColumn();
            column.DataType = System.Type.GetType("System.Int32");
            column.ColumnName = "ID";
            column.Caption = "ID";
            column.AutoIncrement = true;
            column.Unique = true;
            mainTable.Columns.Add(column);

            column = new DataColumn();
            column.DataType = System.Type.GetType("System.String");
            column.ColumnName = "IP";
            column.Caption = "IP";
            column.Unique = true;
            mainTable.Columns.Add(column);


            column = new DataColumn();
            column.DataType = System.Type.GetType("System.String");
            column.ColumnName = "Status";
            column.Caption = "Status";
        
            mainTable.Columns.Add(column);

            column = new DataColumn();
            column.DataType = System.Type.GetType("System.String");
            column.ColumnName = "Type";
            column.Caption = "Type";
    
            mainTable.Columns.Add(column);

            column = new DataColumn();
            column.DataType = System.Type.GetType("System.String");
            column.ColumnName = "Hash Rate RT";
            column.Caption = "Hash Rate RT";
      
            mainTable.Columns.Add(column);

            column = new DataColumn();
            column.DataType = System.Type.GetType("System.String");
            column.ColumnName = "Hash Rate Average";
            column.Caption = "Hash Rate Average";
        
            mainTable.Columns.Add(column);
            column = new DataColumn();
            column.DataType = System.Type.GetType("System.String");
            column.ColumnName = "Temperature";
            column.Caption = "Temperature";
   
            mainTable.Columns.Add(column);

            column = new DataColumn();
            column.DataType = System.Type.GetType("System.String");
            column.ColumnName = "Fan Speed";
            column.Caption = "Fan Speed";
       
            mainTable.Columns.Add(column);

            column = new DataColumn();
            column.DataType = System.Type.GetType("System.String");
            column.ColumnName = "Elapsed";
            column.Caption = "Elapsed";
            
            mainTable.Columns.Add(column);

            column = new DataColumn();
            column.DataType = System.Type.GetType("System.String");
            column.ColumnName = "Pool1";
            column.Caption = "Pool1";
       
            mainTable.Columns.Add(column);

            column = new DataColumn();
            column.DataType = System.Type.GetType("System.String");
            column.ColumnName = "Worker1";
            column.Caption = "Worker1";
          
            mainTable.Columns.Add(column);

            column = new DataColumn();
            column.DataType = System.Type.GetType("System.String");
            column.ColumnName = "Pool2";
            column.Caption = "Pool2";
    
            mainTable.Columns.Add(column);

            column = new DataColumn();
            column.DataType = System.Type.GetType("System.String");
            column.ColumnName = "Worker2";
            column.Caption = "Worker2";
           
            mainTable.Columns.Add(column);

            column = new DataColumn();
            column.DataType = System.Type.GetType("System.String");
            column.ColumnName = "Pool3";
            column.Caption = "Pool3";
      
            mainTable.Columns.Add(column);

            column = new DataColumn();
            column.DataType = System.Type.GetType("System.String");
            column.ColumnName = "Worker3";
            column.Caption = "Worker3";
     
            mainTable.Columns.Add(column);


            //checkColumn.Name = "State";
            //checkColumn.HeaderText = "AutoReboot on problem";
            //checkColumn.ToolTipText = "AutoReboot on very high or very low Temp, or Low hashrate";
            //checkColumn.Width = 50;
            //checkColumn.ReadOnly = false;
            //checkColumn.SortMode = DataGridViewColumnSortMode.NotSortable;
            //checkColumn.FillWeight = 10; //if the datagridview is resized (on form resize) the checkbox won't take up too much; value is relative to the other columns' fill values
            //dataGridView1.Columns.Add(checkColumn); 

            //checkColumn = new DataGridViewCheckBoxColumn();
            //checkColumn.Name = "StateMail";
            //checkColumn.HeaderText = "Report on problem";
            //checkColumn.ToolTipText = "Report on very high or very low Temp, or Low hashrate";
            //checkColumn.Width = 50;
            //checkColumn.ReadOnly = false;
            //checkColumn.SortMode = DataGridViewColumnSortMode.NotSortable;
            //checkColumn.FillWeight = 10; //if the datagridview is resized (on form resize) the checkbox won't take up too much; value is relative to the other columns' fill values

            //dataGridView1.Columns.Add(checkColumn);
            //dataGridView1.Columns.Add("ID", "ID");
            //dataGridView1.Columns.Add("IP", "IP");
            //dataGridView1.Columns.Add("Status", "Status");
            //dataGridView1.Columns.Add("Type", "Type");
            //dataGridView1.Columns.Add("Hash Rate RT", "Hash Rate RT");
            //dataGridView1.Columns.Add("Hash Rate Average", "Hash Rate Average");
            //dataGridView1.Columns.Add("Temperature", "Temperature");
            //dataGridView1.Columns.Add("Fan Speed", "Fan Speed");
            //dataGridView1.Columns.Add("Elapsed", "Elapsed");
            //dataGridView1.Columns.Add("Pool1", "Pool1");
            //dataGridView1.Columns.Add("Worker1", "Worker");
            //dataGridView1.Columns.Add("Pool2", "Pool2");
            //dataGridView1.Columns.Add("Worker2", "Worker");
            //dataGridView1.Columns.Add("Pool3", "Pool3");
            //dataGridView1.Columns.Add("Worker3", "Worker");

            editedTable = mainTable.Copy();
            bs.DataSource = editedTable;
            dataGridView1.DataSource = bs;
            progressBar1.Minimum = 1;
            progressBar1.Step = 1;
            uiScheduler = TaskScheduler.FromCurrentSynchronizationContext();

            setRadioIpRange();
            try
            {
                Sql.populateBD();
            }
            catch (Exception ex)
            {
                Log.logDebug(Convert.ToString(ex));
            }

        }
        public static async void GetUsdRateTimer(object obj = null)
        {
            await Wallets.GetUsdRate();
        }
        public async void refreshDataTableTimer(object obj = null)
        {
            try
            {
                await refreshDataTable();


                updateUI();
            }
            catch (Exception ex)
            {
                Log.logDebug("Main pain in the ass error while scanning:" + Convert.ToString(ex));
            }

        }
        private async void hourlyReports(object obj)
        {
            //sentHourlyHashRate();
            if (DateTime.UtcNow.Minute <= 5)
            {
                await sentHourlyHashRate();

                TimerCallback thisCallBack = new TimerCallback(hourlyReports);
                hourlyReport = new System.Threading.Timer(thisCallBack, null,
                    1000 * 60 * 20, 1000 * 60);
            }
        }
        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            Wallets.saveHistoricalInFile();
            Settings.saveSettings(this);
            //myHttpServer.Close();
            //httpThread.Abort();
        }
        #region ssh
        private void executeSshCommand(string ip, string command)
        {

            // Execute a (SHELL) Command - prepare upload directory
            using (var sshclient = new SshClient(ip, "root", "admin"))
            {
                sshclient.Connect();
                using (var cmd = sshclient.CreateCommand(command))
                {
                    cmd.Execute();
                    Console.WriteLine("Command>" + cmd.CommandText);
                    Console.WriteLine("Return Value = {0}", cmd.ExitStatus);
                }
                sshclient.Disconnect();
            }
        }
        private void executeStfpCommand(string ip, string filename, string directory)
        {
            // Upload A File
            ConnectionInfo ConnNfo = new ConnectionInfo(ip, 22, "root",
                new AuthenticationMethod[]{

                // Pasword based Authentication
                new PasswordAuthenticationMethod("root","admin"),

                // Key Based Authentication (using keys in OpenSSH Format)
                //new PrivateKeyAuthenticationMethod("root",new PrivateKeyFile[]{ 
                //    new PrivateKeyFile(@"..\openssh.key","passphrase")
                //}),
            });
            using (var sftp = new SftpClient(ConnNfo))
            {

                sftp.Connect();
                sftp.ChangeDirectory(directory);
                using (var uplfileStream = System.IO.File.OpenRead(filename))
                {
                    sftp.UploadFile(uplfileStream, filename, true);
                }
                sftp.Disconnect();
            }
        }
        #endregion
        /////////////////////////////////////////////////////////   /////////////////////////////////////////////////////////   /////////////////////////////////////////////////////////   /////////////////////////////////////////////////////////
        #region Functions DataGridView


        //Event for autoreboot and autoreport checkbox column
        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (Settings.antiRebootArray == null)
            {
                Settings.antiRebootArray = new Dictionary<string, bool>();
            }
            if (Settings.antiReportArray == null)
            {
                Settings.antiReportArray = new Dictionary<string, bool>();
            }
            if (e.ColumnIndex >= 0 && e.RowIndex >= 0 && dataGridView1.Columns[e.ColumnIndex].Name == "State")
            {
                string ip = (string)dataGridView1.Rows[e.RowIndex].Cells["IP"].Value;
                if ((bool)(dataGridView1.Rows[e.RowIndex].Cells["State"] as DataGridViewCheckBoxCell).Value)
                {
                    (dataGridView1.Rows[e.RowIndex].Cells["State"] as DataGridViewCheckBoxCell).Value = false;

                    if (!Settings.antiRebootArray.Keys.Contains(ip))
                    {
                        Settings.antiRebootArray.Add(ip, false);

                    }
                    else
                    {
                        Settings.antiRebootArray[ip] = false;
                    }

                }
                else
                {
                    (dataGridView1.Rows[e.RowIndex].Cells["State"] as DataGridViewCheckBoxCell).Value = true;

                    if (!Settings.antiRebootArray.Keys.Contains(ip))
                    {
                        Settings.antiRebootArray.Add(ip, true);

                    }
                    else
                    {
                        Settings.antiRebootArray[ip] = true;
                    }

                }


            }
            if (e.ColumnIndex >= 0 && e.RowIndex >= 0 && dataGridView1.Columns[e.ColumnIndex].Name == "StateMail")
            {
                string ip = (string)dataGridView1.Rows[e.RowIndex].Cells["IP"].Value;
                if ((bool)(dataGridView1.Rows[e.RowIndex].Cells["StateMail"] as DataGridViewCheckBoxCell).Value)
                {
                    (dataGridView1.Rows[e.RowIndex].Cells["StateMail"] as DataGridViewCheckBoxCell).Value = false;
                    if (!Settings.antiReportArray.Keys.Contains(ip))
                    {
                        Settings.antiReportArray.Add(ip, false);

                    }
                    else
                    {
                        Settings.antiReportArray[ip] = false;
                    }

                }
                else
                {
                    (dataGridView1.Rows[e.RowIndex].Cells["StateMail"] as DataGridViewCheckBoxCell).Value = true;

                    if (!Settings.antiReportArray.Keys.Contains(ip))
                    {
                        Settings.antiReportArray.Add(ip, true);

                    }
                    else
                    {
                        Settings.antiReportArray[ip] = true;
                    }

                }


            }
        }
        //Opening site on doubleclick
        private void dataGridView1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if ((dataGridView1.Columns[e.ColumnIndex].Name != "State" || dataGridView1.Columns[e.ColumnIndex].Name != "StateMail") && e.RowIndex > 0)
            {
                System.Diagnostics.Process.Start("http://" + WebCalls.minerLogin + ":" + WebCalls.minerPass + "@" + (string)dataGridView1.Rows[e.RowIndex].Cells["IP"].Value);
            }
        }


        /// <summary>
        /// Fill datagrid with data from Sql.minerList
        /// </summary>
        private void fillDataGridFromMinerList2()
        {
            //dataGridView1.Invoke(new MethodInvoker(delegate
            //       {
                       List<DataRow> rowForDel = new List<DataRow>();

                       foreach (DataRow row in mainTable.Rows)
                       {


                           string ip = (string)row["IP"];
                           MinerModel minerModel = Sql.minersList.SingleOrDefault(t => t.ip == ip);
                           //if (minerModel == null)
                           //    minerModel = new MinerModel() { ip = ip };
                           if (minerModel != null)
                           {
                               try
                               {


                                   row["Status"] = minerModel.status;
                                   row["Pool1"] = minerModel.pool1;
                                   row["Worker1"] = minerModel.worker1;
                                   row["Pool2"] = minerModel.pool2;
                                   row["Worker2"] = minerModel.worker2;
                                   row["Pool3"] = minerModel.pool3;
                                   row["Worker3"] = minerModel.worker3;
                                   row["Type"] = minerModel.type;
                                   row["Hash Rate RT"] = minerModel.hashRateRT;
                                   row["Hash Rate Average"] = minerModel.hashRateAverage;
                                   row["Temperature"] = minerModel.temperatureString;
                                   row["Fan Speed"] = minerModel.fanSpeedString;
                                   row["Elapsed"] = minerModel.elapsed;
                                   this.Invoke(new System.Windows.Forms.MethodInvoker(delegate
                                         {
                                             progressBar1.PerformStep();
                                         }));
                                   //if (checkBoxSuccess.Checked && minerModel.status != "Success")
                                   //{
                                   //    row.Visible = false;
                                   //}
                                   //else
                                   //{
                                   //    row.Visible = true;
                                   //}
                                  
                                   //rowStatus(minerModel);
                               }
                               catch (Exception ex)
                               {
                                   Log.logDebug("fillDataGridFromMinerList " + Convert.ToString(ex));
                               }

                           }
                           else
                           {
                               rowForDel.Add(row);

                           }
                       }
                       for (int iter = rowForDel.Count - 1; iter >= 0; iter--)
                       {
                           mainTable.Rows.Remove(rowForDel[iter]);
                       }
                       this.Invoke(new System.Windows.Forms.MethodInvoker(delegate
                       {
                           editedTable.Rows.Clear();
                           foreach (DataRow dr in mainTable.Rows)
                           {
                               editedTable.Rows.Add(dr.ItemArray);
                           }
                           foreach (DataGridViewRow row in dataGridView1.Rows)
                           {
                               string ip = (string)row.Cells["IP"].Value;
                               MinerModel minerModel = Sql.minersList.SingleOrDefault(t => t.ip == ip);
                               rowStatus(row, minerModel);
                           }
                       }));


                  // }));
        }
        /// <summary>
        /// Fill datagrid with data from Sql.minerList
        /// </summary>
        private void fillDataGridFromMinerList()
        {
            //dataGridView1.Invoke(new MethodInvoker(delegate
            //       {
            List<DataRow> rowForDel = new List<DataRow>();
           
           
            //dataGridView1.SuspendLayout();
           
                if (!testBool)
                {
                    testBool = true;
                    int saveRow = 0;
                    selectCellsFill();
                    if (dataGridView1.Rows.Count > 0 && dataGridView1.FirstDisplayedCell != null)
                        saveRow = dataGridView1.FirstDisplayedCell.RowIndex;
                    this.Invoke(new System.Windows.Forms.MethodInvoker(delegate
                    {

                        dataGridView1.DataSource = null;
                        bs.SuspendBinding();

                    }));

                    try
                    {
                        foreach (DataRow row in editedTable.Rows)
                        {


                            string ip = (string)row["IP"];
                            MinerModel minerModel = Sql.minersList.SingleOrDefault(t => t.ip == ip);
                            //if (minerModel == null)
                            //    minerModel = new MinerModel() { ip = ip };
                            if (minerModel != null)
                            {
                                try
                                {

                                    row["Status"] = minerModel.status;
                                    row["Pool1"] = minerModel.pool1;
                                    row["Worker1"] = minerModel.worker1;
                                    row["Pool2"] = minerModel.pool2;
                                    row["Worker2"] = minerModel.worker2;
                                    row["Pool3"] = minerModel.pool3;
                                    row["Worker3"] = minerModel.worker3;
                                    row["Type"] = minerModel.type;
                                    row["Hash Rate RT"] = minerModel.hashRateRT;
                                    row["Hash Rate Average"] = minerModel.hashRateAverage;
                                    row["Temperature"] = minerModel.temperatureString;
                                    row["Fan Speed"] = minerModel.fanSpeedString;
                                    row["Elapsed"] = minerModel.elapsed;

                                    //this.Invoke(new MethodInvoker(delegate
                                    //{

                                    //}));

                                }

                                catch (Exception ex)
                                {
                                    Log.logDebug("fillDataGridFromMinerList " + Convert.ToString(ex));
                                }

                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Log.logDebug("pain 111..." + Convert.ToString(ex));
                    }

                    try
                    {

                        //dataGridView1.ResumeLayout();
                        this.Invoke(new System.Windows.Forms.MethodInvoker(delegate
                        {
                            bs.ResumeBinding();
                            dataGridView1.DataSource = bs;
                            foreach (DataGridViewRow row in dataGridView1.Rows)
                            {
                                string ip = (string)row.Cells["IP"].Value;
                                MinerModel minerModel = Sql.minersList.SingleOrDefault(t => t.ip == ip);
                                rowStatus(row, minerModel);
                                progressBar1.PerformStep();
                            }

                            try
                            {
                                if (saveRow != 0 && saveRow < dataGridView1.Rows.Count)
                                    dataGridView1.FirstDisplayedScrollingRowIndex = saveRow;
                            }
                            catch { }
                            selectCells();
                        }));
                    }
                    catch (Exception ex)
                    {
                        Log.logDebug("pain 222..." + Convert.ToString(ex));
                    }
                    testBool = false;
                }
            

            // }));
        }


        //Парсит ipRange и рисует по ней скелет таблицы.
        private void fillRowIpArray()
        {
            Settings.minerErrorState = new Dictionary<string, string>();
            progressBar1.Maximum = 1;
            Settings.ipList = new List<string>();
            //dataGridView1.Rows.Clear();
            mainTable.Rows.Clear();
            this.Invoke(new System.Windows.Forms.MethodInvoker(delegate
            {
                editedTable.Rows.Clear();
                foreach (DataRow dr in mainTable.Rows)
                {
                    editedTable.Rows.Add(dr.ItemArray);
                }
               
            }));
          
            for (int iter = 0; iter < ipRangeBox.CheckedItems.Count; iter++)
            {
                Settings.ipRange thisRange = new Settings.ipRange();
                string tempRange = ipRangeBox.CheckedItems[iter].ToString();
                tempRange = tempRange.Substring(tempRange.IndexOf(':') + 1);
                string[] tempRangeArray = tempRange.Split(new[] { '-' }, StringSplitOptions.RemoveEmptyEntries);
                if (tempRangeArray.Length < 2 || tempRangeArray[0] == "192.168.1.1")
                {
                    ipRangeBox.SetItemChecked(ipRangeBox.CheckedIndices[iter], false);
                    continue;
                }
                string[] parseRangeArray = tempRangeArray[1].Split(new[] { '.' }, StringSplitOptions.RemoveEmptyEntries);
                if (parseRangeArray.Length < 4)
                {
                    ipRangeBox.SetItemChecked(ipRangeBox.CheckedIndices[iter], false);
                    continue;
                }
                thisRange.highMaxIP = Convert.ToInt32(parseRangeArray[2], CultureInfo.InvariantCulture);
                thisRange.lowMaxIP = Convert.ToInt32(parseRangeArray[3], CultureInfo.InvariantCulture);
                parseRangeArray = tempRangeArray[0].Split(new[] { '.' }, StringSplitOptions.RemoveEmptyEntries);
                if (parseRangeArray.Length < 4)
                {
                    ipRangeBox.SetItemChecked(ipRangeBox.CheckedIndices[iter], false);
                    continue;
                }
                thisRange.highMinIP = Convert.ToInt32(parseRangeArray[2], CultureInfo.InvariantCulture);
                thisRange.lowMinIP = Convert.ToInt32(parseRangeArray[3], CultureInfo.InvariantCulture);

                if (thisRange.lowMaxIP == 255 || thisRange.lowMinIP == 0)
                {
                    ipRangeBox.SetItemChecked(ipRangeBox.CheckedIndices[iter], false);
                    continue;
                }
                if (thisRange.highMaxIP != thisRange.highMinIP)
                {
                    ipRangeBox.SetItemChecked(ipRangeBox.CheckedIndices[iter], false);
                    continue;
                }
                else
                {

                    for (int i = 0; i <= (thisRange.lowMaxIP - thisRange.lowMinIP); i++)
                    {
                        string newIp = parseRangeArray[0] + "." + parseRangeArray[1] + "." + parseRangeArray[2] + "." + Convert.ToString(thisRange.lowMinIP + i, CultureInfo.InvariantCulture);
                        if (!Settings.ipList.Contains(newIp))
                        {

                            Settings.ipList.Add(newIp);
                        }
                    }
                    //thisRange.diffHigh = false;
                }

            }
            //Занесли в массив все новые ip
            //dataGridView1.Rows.Add(ipList.Count);
            progressBar1.Maximum = Settings.ipList.Count;
            //cleanDB();
            //List<DataGridViewRow> rows = new List<DataGridViewRow>();
            for (int iter = 0; iter < Settings.ipList.Count; iter++)
            {
                // Rows.Add в сотни раз быстрее рисование информации по id 
                DataRow row = mainTable.NewRow();
                row[3]= Settings.ipList[iter];

               // row[0].ToolTipText = "AutoReboot on very high or very low Temp, or Low hashrate";
                //row.Cells[1].ToolTipText = "Report on very high or very low Temp, or Low hashrate";
                if (Settings.antiRebootArray != null && Settings.antiRebootArray.Keys.Contains(Settings.ipList[iter]))
                    row[0] = Settings.antiRebootArray[Settings.ipList[iter]];
                else
                    row[0] = false;
                if (Settings.antiReportArray != null && Settings.antiReportArray.Keys.Contains(Settings.ipList[iter]))
                    row[1] = Settings.antiReportArray[Settings.ipList[iter]];
                else
                    row[1] = false;
                //insertNewRowsDB(iter+1, !Settings.antiRebootArray.Contains(ipList[iter]), !Settings.antiReportArray.Contains(ipList[iter]), ipList[iter]);
                Settings.minerErrorState.Add(Convert.ToString(Settings.ipList[iter]), Settings.defaultErrorState);
                row[4] = Settings.minerErrorState[Settings.ipList[iter]];
                row[5] = "";
                row[6] = "";
                row[7] = "";
                row[8] = "";
                row[9] = "";
                row[10] = "";
                row[11] = "";
                row[12] = "";
                row[13] = "";
                row[14] = "";
                row[15] = "";
                row[16] = "";
                try
                {
                    mainTable.Rows.Add(row);
                }
                catch(Exception ex)
                {
                    Log.logDebug(Convert.ToString(ex));
                }
            }

            this.Invoke(new System.Windows.Forms.MethodInvoker(delegate
            {
                editedTable.Rows.Clear();
                foreach (DataRow dr in mainTable.Rows)
                {
                    editedTable.Rows.Add(dr.ItemArray);
                }
              
            }));
            //dataGridView1.Invoke(new MethodInvoker(delegate
            //{
            //    dataGridView1.Rows.AddRange(rows.ToArray());
            //}));




        }
        //Изменяет метод сортировки для Температуры и скорости вентилятора
        private async void dataGridView1_SortCompare(object sender, DataGridViewSortCompareEventArgs e)
        {
            // Try to sort based on the cells in the current column.
            while (Settings.stopSorting == true)
            {
                await Task.Delay(1000); ;
            }
            if (e.Column == dataGridView1.Columns["Temperature"] || e.Column == dataGridView1.Columns["Fan Speed"])
            {
                e.SortResult = System.String.Compare(
                    e.CellValue1.ToString(), e.CellValue2.ToString());
                string[] temp = e.CellValue1.ToString().Split(new char[] { '/' }, StringSplitOptions.RemoveEmptyEntries);
                int[] temp1 = new int[temp.Length];
                for (int iter = 0; iter < temp.Length; iter++)
                {
                    temp1[iter] = Convert.ToInt32(temp[iter], CultureInfo.InvariantCulture);
                }
                for (int iter = 1; iter < temp1.Length; iter++)
                {

                    if (temp1[0] < temp1[iter])
                    {
                        temp1[0] = temp1[iter];
                    }
                }
                temp = e.CellValue2.ToString().Split(new char[] { '/' }, StringSplitOptions.RemoveEmptyEntries);
                int[] temp2 = new int[temp.Length];
                for (int iter = 0; iter < temp.Length; iter++)
                {
                    if (temp[iter] != "-")
                        temp2[iter] = Convert.ToInt32(temp[iter], CultureInfo.InvariantCulture);
                }
                for (int iter = 1; iter < temp2.Length; iter++)
                {
                    if (temp2[0] < temp2[iter])
                    {
                        temp2[0] = temp2[iter];
                    }
                }
                if (temp1.Length == 0)
                {
                    e.SortResult = 1;
                }
                else
                    if (temp2.Length == 0)
                    {
                        e.SortResult = -1;
                    }
                    else
                    {
                        e.SortResult = temp1[0] > temp2[0] ? -1 : temp1[0] == temp2[0] ? 0 : 1;
                    }
                // If the cells are equal, sort based on the ID column.
                if (e.SortResult == 0 && e.Column.Name != "ID")
                {
                    e.SortResult = System.String.Compare(
                        dataGridView1.Rows[e.RowIndex1].Cells["ID"].Value.ToString(),
                        dataGridView1.Rows[e.RowIndex2].Cells["ID"].Value.ToString());
                }

                e.Handled = true;
            }
            else
            {
                if (e.Column == dataGridView1.Columns["ID"])
                {

                    e.SortResult = Convert.ToInt32(e.CellValue1).CompareTo(Convert.ToInt32(e.CellValue2));

                    // If the cells are equal, sort based on the ID column.
                    e.Handled = true;
                }
                else
                {
                    if (e.Column == dataGridView1.Columns["Hash Rate RT"] || e.Column == dataGridView1.Columns["Hash Rate Average"])
                    {
                        String temp1, temp2;
                        temp1 = e.CellValue1.ToString();
                        temp2 = e.CellValue2.ToString();

                        if (temp1 == "")
                        {
                            temp1 = "0";
                        }
                        if (temp2 == "")
                        {
                            temp2 = "0";
                        }
                        e.SortResult = Convert.ToInt32(temp1).CompareTo(Convert.ToInt32(temp2));

                        if (e.SortResult == 0 && e.Column.Name != "ID")
                        {
                            e.SortResult = System.String.Compare(
                                dataGridView1.Rows[e.RowIndex1].Cells["ID"].Value.ToString(),
                                dataGridView1.Rows[e.RowIndex2].Cells["ID"].Value.ToString());
                        }
                        // If the cells are equal, sort based on the ID column.
                        e.Handled = true;
                    }
                    else
                    {
                        if (e.Column == dataGridView1.Columns["IP"])
                        {
                            //if (e.CellValue1.ToString().Length != e.CellValue2.ToString().Length)
                            //{
                            //    e.SortResult = e.CellValue1.ToString().Length.CompareTo(e.CellValue2.ToString().Length);
                            //    e.Handled = true;
                            //}
                            //else
                            //{
                            long temp1, temp2;
                            temp1 = Convert.ToInt64(e.CellValue1.ToString().Replace(".", ""));
                            temp2 = Convert.ToInt64(e.CellValue2.ToString().Replace(".", ""));

                            e.SortResult = temp1.CompareTo(temp2);

                            if (e.SortResult == 0 && e.Column.Name != "ID")
                            {
                                e.SortResult = System.String.Compare(
                                    dataGridView1.Rows[e.RowIndex1].Cells["ID"].Value.ToString(),
                                    dataGridView1.Rows[e.RowIndex2].Cells["ID"].Value.ToString());
                            }
                            // If the cells are equal, sort based on the ID column.
                            e.Handled = true;
                            //}
                        }
                        else
                        {
                            e.SortResult = System.String.Compare(
                                e.CellValue1.ToString(), e.CellValue2.ToString());

                            // If the cells are equal, sort based on the ID column.
                            if (e.SortResult == 0 && e.Column.Name != "ID")
                            {
                                e.SortResult = System.String.Compare(
                                    dataGridView1.Rows[e.RowIndex1].Cells["ID"].Value.ToString(),
                                    dataGridView1.Rows[e.RowIndex2].Cells["ID"].Value.ToString());
                            }
                            e.Handled = true;
                        }
                    }
                }
            }
        }
        //Проверяет на неполадки. По дефолту раз в полчаса,при отключенном мониторинге.

        //Функция очистки string from nonDigital
        //Check if all needed dires exist
        public void setLogFiles()
        {
            if (!Directory.Exists(Application.StartupPath + "\\logDebug\\"))
                Directory.CreateDirectory(Application.StartupPath + "\\logDebug\\");
            if (!Directory.Exists(Application.StartupPath + "\\logReport\\"))
                Directory.CreateDirectory(Application.StartupPath + "\\logReport\\");
            if (!Directory.Exists(Log.logArchivePath))
                Directory.CreateDirectory(Log.logArchivePath);
            if (!Directory.Exists(Log.logArchivePathDebug))
                Directory.CreateDirectory(Log.logArchivePathDebug);
            if (!File.Exists(Log.logDebugPath))
                using (var file = File.Create(Log.logDebugPath))
                { }
            if (!File.Exists(pathSetting))
            {
                using (var file = File.Create(pathSetting))
                { }
                Settings.saveSettings(this);
            }
            try
            {
                if (new FileInfo(Log.logDebugPath).Length != 0)
                {
                    System.IO.File.WriteAllText(Log.logDebugPath, string.Empty, Encoding.Unicode);//Обнуляем прошлый дебаг
                }
                if (!File.Exists(Log.logReportPath))
                    using (var file = File.Create(Log.logReportPath))
                    { }
            }
            catch (Exception ex)
            {
                Log.logDebug(Convert.ToString(ex));
            }

        }
        //Debug log       
        public void parseReportFileNow(object obj = null)
        {
            try
            {
                if (new FileInfo(Log.logReportPath).Length != 0 && !Settings.parseReportNowTimeout)
                {
                    string[] parseArray = File.ReadAllText(Log.logReportPath, Encoding.Unicode).Split(new char[] { '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries);
                    Dictionary<String, int> parsedReport = new Dictionary<string, int>();
                    for (int iter = 0; iter < parseArray.Length; iter++)
                    {
                        if (parseArray[iter].IndexOf("Problem with") > 0)
                        {
                            string parsedString = parseArray[iter].Substring(parseArray[iter].IndexOf("Problem with"));
                            if (!parsedReport.Keys.Contains(parsedString))
                            {
                                parsedReport[parsedString] = 0;
                            }
                            else
                            {
                                parsedReport[parsedString]++;
                            }
                        }
                    }
                    // 

                    //if (!File.Exists(logReportPath+".txt"))
                    //    File.Create(logReportPath + ".txt");
                    StringBuilder builder = new StringBuilder();
                    StringBuilder builderFull = new StringBuilder();
                    foreach (KeyValuePair<string, int> pair in parsedReport)
                    {
                        string forParsing = "Problem with ";
                        string testing = pair.Key.Substring(forParsing.Length);
                        testing = testing.Remove(testing.IndexOf(':'));
                        if (Settings.antiReportArray == null)
                        {
                            Settings.antiReportArray = new Dictionary<string, bool>();
                        }
                        if (pair.Value >= 5)
                        {

                            string parsedStart = "", parsedEnd = "";
                            for (int iter = 0; iter < parseArray.Length; iter++)
                            {
                                if (parseArray[iter].Contains(pair.Key) && parsedStart == "")
                                {
                                    parsedStart = parseArray[iter].Remove(parseArray[iter].IndexOf(pair.Key));
                                }
                                else
                                {
                                    if (parseArray[iter].Contains(pair.Key))
                                    {
                                        parsedEnd = parseArray[iter].Remove(parseArray[iter].IndexOf(pair.Key));
                                    }
                                }
                            }
                            if (Settings.antiReportArray.Keys.Contains(testing) && Settings.antiReportArray[testing])
                            {
                                builder.Append(pair.Key).Append(":").Append('\n').Append(parsedStart).Append(" - ").Append(parsedEnd).Append('\n');
                            }
                            builderFull.Append(pair.Key).Append(":").Append('\n').Append(parsedStart).Append(" - ").Append(parsedEnd).Append('\n');
                        }
                    }
                    bool checkIfReboot = false;
                    if (builderFull.ToString() != "")
                    {
                        foreach (KeyValuePair<string, int> pair in parsedReport)
                        {
                            string forParsing = "Problem with ";
                            string testing = pair.Key.Substring(forParsing.Length);
                            testing = testing.Remove(testing.IndexOf(':'));
                            if (Settings.antiRebootArray == null)
                            {
                                Settings.antiRebootArray = new Dictionary<string, bool>();
                            }
                            if (pair.Value >= 3 && !pair.Key.Contains("Problem with connection") && !pair.Key.Contains("Rebooting miner")
                                && !pair.Key.Contains("Configurating miner"))
                            {
                                if (pair.Key.IndexOf("192.168") > 0)
                                {
                                    if (pair.Key.IndexOf(":") > 0)
                                    {
                                        if (Settings.antiRebootArray.Keys.Contains(testing) && Settings.antiRebootArray[testing])
                                        {

                                            checkIfReboot = true;
                                            MinerChangeFunc.rebootMiner(testing);
                                        }

                                    }
                                }
                            }
                        }
                    }
                    if (checkIfReboot)
                        Log.logJustArchive();
                    string result = builder.ToString();
                    if (result != "")
                    {
                        Settings.parseReportNowTimeout = true;
                        TimerCallback checkNowBack = new TimerCallback(this.parsingReportNowTimeoutEnd);
                        reportNowTimer = new System.Threading.Timer(checkNowBack, Settings.stopSorting, Settings.badCheckTimeout, Settings.badCheckTimeout);
                    }
                }
            }
            catch (Exception except)
            {
                Log.logDebug("ParseReportFileNow " + Convert.ToString(except, CultureInfo.InvariantCulture));
            }
        }
        //Event on report Timeout timer end
        public void parsingReportNowTimeoutEnd(object obj)
        {
            Settings.parseReportNowTimeout = false;
            Log.logJustArchive();//Переносит report в архив, чтобы не шла бесконечная перезагрузка.
            reportNowTimer.Change(Timeout.Infinite, Timeout.Infinite);
        }

        //Открытие окна настроек
        private void buttonSettingWindow_Click(object sender, EventArgs e)
        {
            SettingForm settingWindow = new SettingForm(this);
            this.Enabled = false;
            settingWindow.Show();

        }
        public void updateUI()
        {
            Stopwatch newWatch = new Stopwatch();
            newWatch = new Stopwatch();
            newWatch.Start();

            fillDataGridFromMinerList();
            newWatch.Stop();
            Log.logDebug("\n\rrefreshDataTable async 2 part:" + newWatch.Elapsed);
            if (InvokeRequired)
            {

                if (dataGridView1.SortedColumn != null)
                {
                    if (dataGridView1.SortOrder == SortOrder.Descending)

                        this.Invoke(new System.Windows.Forms.MethodInvoker(delegate
                        {
                            dataGridView1.Sort(dataGridView1.SortedColumn, ListSortDirection.Descending);
                        }));
                    else
                        this.Invoke(new System.Windows.Forms.MethodInvoker(delegate
                        {
                            dataGridView1.Sort(dataGridView1.SortedColumn, ListSortDirection.Ascending);
                        }));
                }

            }
            else
            {
                if (dataGridView1.SortedColumn != null)
                {
                    if (dataGridView1.SortOrder == SortOrder.Descending)
                    {
                        dataGridView1.Sort(dataGridView1.SortedColumn, ListSortDirection.Descending);
                    }
                    else
                    {
                        dataGridView1.Sort(dataGridView1.SortedColumn, ListSortDirection.Ascending);
                    }
                }
            }


            Settings.stopSorting = false;
            Settings.connectingToSocketsStatus = false;

            newWatch = new Stopwatch();
            newWatch.Start();
            getGeneralMinerInfo();
            newWatch.Stop();
            Log.logDebug("\n\rrefreshDataTable async 4 part:" + newWatch.Elapsed);
          

            parseReportFileNow();

            //refreshDataGrid();
            //getDataForSite();
            if (!Settings.monitoringStatus)
            {
                int workingMinnersCount = 0;
                for (int iter = 0; iter < mainTable.Rows.Count; iter++)
                {
                    if (Convert.ToString(mainTable.Rows[iter]["Status"]).Contains("Success"))
                        workingMinnersCount++;
                }
                DialogResult confirmationWindow = MessageBox.Show("На данный момент работает " + workingMinnersCount
                    + " майнеров из " + mainTable.Rows.Count
                   , "Статус майнеров", MessageBoxButtons.OK);
            }
        }


        //Обновляет таблицу. Мониторинг
        public async Task refreshDataTable(object obj = null)
        {
            //bool tempBool=(bool) obj;
            try
            {
                if (!Settings.connectingToSocketsStatus && mainTable.Rows.Count > 0)
                {
                   
                    Settings.connectingToSocketsStatus = true;               
                    List<string> ipArray = new List<string>();
                    // Set Maximum to the total number of files to copy.               
                    // Set the initial value of the ProgressBar.

                    // Set the Step property to a value of 1 to represent each file being copied.
                    if (InvokeRequired)
                        this.Invoke(new System.Windows.Forms.MethodInvoker(delegate { progressBar1.Value = 1; }));
                    else
                        progressBar1.Value = 1;
                    Settings.stopSorting = true;
                    for (int iter = 0; iter < mainTable.Rows.Count; iter++)
                    {
                        ipArray.Add((string)mainTable.Rows[iter]["IP"]);
                    }

                    Task[] taskArray = new Task[ipArray.Count];
                    for (int iter = 0; iter < ipArray.Count; iter++)
                    {
                      

                        taskArray[iter] = Task.Factory.StartNew(async (Object objec) =>
                        {
                            int iterIner = Convert.ToInt32(objec);
                            await addMiner(ipArray[iterIner]);
                        }, iter).Unwrap();
                    }
                    try
                    {
                        await Task.WhenAll(taskArray);
                    }
                    catch (Exception ex)
                    {
                        Log.logDebug("\n\rWhenAllTask Error " + Convert.ToString(ex));
                    }
                    Log.logDebug("\n\r We Did it");
                    await sentDataTableData();


                }
            }
            catch (Exception ex)
            {
                Settings.stopSorting = false;
                Settings.connectingToSocketsStatus = false;
                Log.logDebug("refreshDataTable error " + Convert.ToString(ex));
            }
        }
        //Operation Data
        public async Task sentHourlyHashRate()
        {
            Settings.stopSorting = true;
            Log.logArchive();
            if (Settings.logged)
            {
                int hashrate = 0;
                foreach (MinerModel model in Sql.minersList)
                {
                    if (model.hashRateRT > 0)
                    {
                        hashrate += model.hashRateRT;
                    }
                }
                //Log.logDebug("\n\rSendJson \n\r\n\r" + result + "\n\r");
                var values = new Dictionary<string, string>
            {
            { "hashrate", hashrate.ToString()},
                { "AsikAllCount", Sql.minersList.Count().ToString()},
                    { "AsikWorkingCount",  Sql.minersList.Count(t=>t.hashRateRT>0).ToString()}
            };
                HttpClient client = new HttpClient();
                var content = new FormUrlEncodedContent(values);
                try
                {
                    var response = await client.PostAsync(Settings.siteuser + "Mining//hashrateHistory?id=" + Log.flyMiningUserName + "&key=" + Log.flyMiningPassword, content);
                    var responseString = await response.Content.ReadAsStringAsync();
                    responseString += "";
                }
                catch (Exception ex)
                {
                    Log.logDebug("sentHourlyHashRate  : " + Convert.ToString(ex));
                }
            }
            Settings.stopSorting = false;
        }





        //Send data to server for site table
        public async Task sentDataTableData()
        {
            if (Settings.logged)
            {
                string result = "";
                try
                {
                    string[][] tableData = new string[editedTable.Rows.Count][];
                    for (int iter = 0; iter < editedTable.Rows.Count; iter++)
                    {
                        tableData[iter] = new string[editedTable.Columns.Count - 3];
                        //Begin with 3 because of AutoReboot,AutoReport and id colmns
                        for (int colmnIter = 3; colmnIter < editedTable.Columns.Count; colmnIter++)
                        {
                            tableData[iter][colmnIter - 3] = editedTable.Rows[iter][colmnIter].ToString();
                            if (editedTable.Rows[iter][colmnIter].ToString() != "")
                                result += UtilityFunc.ReplaceWhitespace(editedTable.Rows[iter][colmnIter].ToString(), " ") + ";";
                            else
                                result += "No Data;";
                        }
                        result = result.Remove(result.Length - 1);
                        result += "\\";
                    }

                }
                catch (Exception ex)
                {
                    Log.logDebug("Table data parse : " + Convert.ToString(ex));
                    return;
                }
                using (HttpClient client = new HttpClient())
                {
                    HttpResponseMessage response = new HttpResponseMessage();
                    try
                    {
                        string reportProblemString = "";
                        if (Log.ipSituation != null)
                        {
                            foreach (string key in Log.ipSituation.Keys)
                            {
                                reportProblemString += key + ";" + Log.ipSituation[key].toStringLine() + "\\";
                            }
                        }
                        client.Timeout = new TimeSpan(0, 0, 10);
                        response = await client.GetAsync(Settings.siteuser + "Mining//programStatus?id=" + Log.flyMiningUserName + "&key=" + Log.flyMiningPassword);
                        var options = new
                        {
                            data = result,
                            minerStatus = labelMinerNumber.Text,
                            totalHash = labelHashrate.Text,
                            totalTemp = labelTemperature.Text,
                            lastReboot = labelReboot.Text,
                            income = labelWallet.Text,
                            incomeDol = labelDollar.Text,
                            emails = String.Join(";", Log.emailForBadStatus),
                            checkBool = Settings.apiCheckState.ToString(),
                            checkStart = Settings.apiCheckStart.ToString(),
                            checkTimeout = Settings.apiCheckTimeout.ToString(),
                            workingMiners = Settings.currentStatus.workingMiners,
                            allMiners = Settings.currentStatus.allMiners,
                            minTemp = Settings.currentStatus.minTemp,
                            avgTemp = Settings.currentStatus.avgTemp,
                            maxTemp = Settings.currentStatus.MaxTemp,
                            hashrate = Settings.currentStatus.allHashrate,
                            lastRebootDate = Settings.currentStatus.lastReboot

                        };

                        var stringPayload = JsonConvert.SerializeObject(options);

                        var content = new StringContent(stringPayload, Encoding.UTF8, "application/json");


                        //var content = new FormUrlEncodedContent(values);

                        response = await client.PostAsync(Settings.siteuser + "Mining//getTableData?id=" + Log.flyMiningUserName + "&key=" + Log.flyMiningPassword, content);
                        var responseString = await response.Content.ReadAsStringAsync();
                        Log.logDebugTest(responseString + "\n\r");
                        var problems = new
                        {
                            msg = reportProblemString
                        };
                        var stringProblemload = JsonConvert.SerializeObject(problems);
                        content = new StringContent(stringProblemload, Encoding.UTF8, "application/json");
                        response = await client.PostAsync(Settings.siteuser + "Mining//parseReportProblems?id=" + Log.flyMiningUserName + "&key=" + Log.flyMiningPassword, content);
                    }
                    catch (Exception ex)
                    {
                        Log.logDebug("Table data send : " + Convert.ToString(ex));
                    }
                }
            }
        }

        //Main function for parsing data and filling table
        public async Task<MinerModel> populateMinerRow(string ip)
        {
            try
            {
                int iter = 1;
                jsonMinerStatus pools = null;
                jsonMinerNetworkStatus netStatus = null;
                jsonMinerStatus minerAllStats = null;
                jsonMinerStatus summaryStatus = null;
                while (pools == null && iter < 3)
                {
                    pools = await Kernel.getStatusData(ip);
                    netStatus = await Kernel.getNetworkData(ip);
                    minerAllStats = await Kernel.getStatsData(ip);
                    summaryStatus = await Kernel.getSummaryData(ip);
                    iter++;
                }
                MinerModel minerModel = new MinerModel(ip, pools, netStatus, minerAllStats, summaryStatus);


                Sql.updateDB(minerModel);
                return minerModel;
            }
            catch (Exception ex)
            {
                Log.logDebug("Main " + Convert.ToString(ex));
                return null;
            }

        }






        //Check miner state
        public string minerCurrentState(DataGridViewRow row)
        {



            return "NotSuccess";
        }
        //Заносит данные в таблицу
        // Добавляет новую строчку в таблицу. Вызвывает функции наполнения
        public async Task addMiner(string ip)
        {
            await populateMinerRow(ip);

        }
        // Функция проверки состояния строки на неполадки
        public string rowStatus(DataGridViewRow row, MinerModel model)
        {
            int partDebug = 0;
            try
            {
                if (model == null)
                    return "";
                string result = "";
                switch (model.status??"")
                {
                    case Settings.defaultErrorState:
                        row.Cells["Status"].Style.BackColor = Color.Orange;
                        Log.logReport("Problem with connection to miner\n\r", model);
                        return "Problem with connection to miner\n\r";
                    case Settings.rebootErrorState:
                        row.Cells["Status"].Style.BackColor = Color.Empty;
                        Log.logReport("Rebooting miner\n\r", model);
                        return "Rebooting miner\n\r";
                    case Settings.configErrorState:
                        row.Cells["Status"].Style.BackColor = Color.Empty;
                        Log.logReport("Configurating miner\n\r", model);
                        return "Configurating miner\n\r";
                    default:
                        row.Cells["Status"].Style.BackColor = Color.Empty;
                        break;
                }
                partDebug++;
                try
                {
                    decimal temp = model.hashRateRT;
                    if (temp == 0 || Convert.ToInt32(temp, CultureInfo.InvariantCulture) < Settings.antMinerHashMin)
                    {
                        result += "Low  Hashrate;";
                        row.Cells["Hash Rate Average"].Style.BackColor = Color.Orange;
                        row.Cells["Hash Rate RT"].Style.BackColor = Color.Orange;
                    }
                    else if (Convert.ToInt32(temp, CultureInfo.InvariantCulture) > 20000)
                    {
                        result += "High  Hashrate;";
                        row.Cells["Hash Rate Average"].Style.BackColor = Color.Orange;
                        row.Cells["Hash Rate RT"].Style.BackColor = Color.Orange;
                    }
                    else
                    {
                        row.Cells["Hash Rate Average"].Style.BackColor = Color.Empty;
                        row.Cells["Hash Rate RT"].Style.BackColor = Color.Empty;
                    }
                }
                catch (Exception)
                {
                    result += "Low  Hashrate;";
                    row.Cells["Hash Rate Average"].Style.BackColor = Color.Orange;
                    row.Cells["Hash Rate RT"].Style.BackColor = Color.Orange;
                }
                partDebug++;
                if (Settings.detectHighTemp)
                {


                    List<int> tempArray = new List<int>();
                    tempArray.Add(model.temperature1);
                    tempArray.Add(model.temperature2);
                    tempArray.Add(model.temperature3);
                    for (int iter = 0; iter < tempArray.Count; iter++)
                    {
                        if (tempArray[iter] >= Settings.abnormalTemp)
                        {
                            result += "High  temperature " + tempArray[iter] + ";";
                            row.Cells["Temperature"].Style.BackColor = Color.Orange;
                            break;
                        }
                        else
                        {
                            if (tempArray[iter] <= Settings.minimalTemp)
                            {
                                result += "Low  temperature " + tempArray[iter] + ";";
                                row.Cells["Temperature"].Style.BackColor = Color.Orange;
                                break;
                            }
                            else
                                row.Cells["Temperature"].Style.BackColor = Color.Empty;
                        }
                    }

                }
                partDebug++;
                if (row.Cells["Pool1"].Value != null && row.Cells["Pool2"].Value != null && row.Cells["Pool3"].Value != null)
                {
                    partDebug++;
                    if ((string)row.Cells["Pool1"].Value == "" ||
                        (string)row.Cells["Pool2"].Value == "" ||
                        (string)row.Cells["Pool3"].Value == "")
                    {
                        partDebug++;
                        row.Cells["Pool1"].Style.BackColor = Color.Orange;
                        row.Cells["Pool2"].Style.BackColor = Color.Orange;
                        row.Cells["Pool3"].Style.BackColor = Color.Orange;
                        row.Cells["Worker1"].Style.BackColor = Color.Orange;
                        row.Cells["Worker2"].Style.BackColor = Color.Orange;
                        row.Cells["Worker3"].Style.BackColor = Color.Orange;
                        partDebug++;
                        Log.logReport("Empty pool", model);
                        partDebug++;
                    }
                    else
                    {
                        row.Cells["Pool1"].Style.BackColor = Color.Empty;
                        row.Cells["Pool2"].Style.BackColor = Color.Empty;
                        row.Cells["Pool3"].Style.BackColor = Color.Empty;
                        row.Cells["Worker1"].Style.BackColor = Color.Empty;
                        row.Cells["Worker2"].Style.BackColor = Color.Empty;
                        row.Cells["Worker3"].Style.BackColor = Color.Empty;
                    }
                }
                else
                {
                    row.Cells["Pool1"].Style.BackColor = Color.Orange;
                    row.Cells["Pool2"].Style.BackColor = Color.Orange;
                    row.Cells["Pool3"].Style.BackColor = Color.Orange;
                    row.Cells["Worker1"].Style.BackColor = Color.Orange;
                    row.Cells["Worker2"].Style.BackColor = Color.Orange;
                    row.Cells["Worker3"].Style.BackColor = Color.Orange;
                }
              
                if (result != "")
                {
                    Log.logReport(result + "\r\n", model);
                    return result;
                }
                else
                {
                    row.DefaultCellStyle.BackColor = Color.Empty;
                    return "ОК";
                }

            }
            catch (Exception e)
            {
                Log.logDebug("\n RowStatus "+ partDebug+" " + Convert.ToString(e, CultureInfo.InvariantCulture));
                return Convert.ToString(e, CultureInfo.InvariantCulture);
            }
        }
        /////////////////////////////////////////////////////////   /////////////////////////////////////////////////////////   /////////////////////////////////////////////////////////   /////////////////////////////////////////////////////////
        #region buttons DataGridView
        //Показывать/Не показывать не загруженные майнеры
        private void checkBoxSuccess_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBoxSuccess.Checked)
            {
                for (int iter = 0; iter < mainTable.Rows.Count; iter++)
                    if (!Convert.ToString(mainTable.Rows[iter]["Status"]).Contains("Success"))
                        dataGridView1.Rows[iter].Visible = false;
            }
            else
            {
                for (int iter = 0; iter < dataGridView1.Rows.Count; iter++)
                    dataGridView1.Rows[iter].Visible = true;
            }
        }
        //Monitoring
        private void monitorMiners()
        {
            if (Log.flyMiningPassword == "" || Log.flyMiningUserName == "")
            {
                DialogResult errorWindow = MessageBox.Show("Attention! Without valid Flymining Login and Key \n\r Email reports and online table won't work   "
                                       , "Information", MessageBoxButtons.OK);
            }
            TimerCallback tm = new TimerCallback(refreshDataTableTimer);
            if (Settings.monitoringStatus)
            {
                enableConfigButtons(true);
                enableRebootButtons(true);
                Settings.monitoringStatus = false;
                Settings.connectingToSocketsStatus = false;
                monitoringTimer.Change(Timeout.Infinite, Timeout.Infinite);
                if (InvokeRequired)
                    this.Invoke(new System.Windows.Forms.MethodInvoker(delegate
                    {
                        buttonMonitor.Text = "Start monitoring";
                    }));
                else
                {
                    buttonMonitor.Text = "Start monitoring";
                }

                //progressBar1.Value = 1;

            }
            else
            {
                enableConfigButtons(false);
                enableRebootButtons(false);
                Settings.monitoringStatus = true;
                if (InvokeRequired)
                    this.Invoke(new System.Windows.Forms.MethodInvoker(delegate
                    {
                        buttonMonitor.Text = "Stop monitoring";
                        progressBar1.Maximum = mainTable.Rows.Count;
                    }));
                else
                {
                    buttonMonitor.Text = "Stop monitoring";
                    progressBar1.Maximum = mainTable.Rows.Count;
                }
                monitoringTimer = new System.Threading.Timer(tm, Settings.connectingToSocketsStatus, 0, Settings.monitoringTimeout);
            }
        }
        //Начать мониторинг. По дефолту раз в 30 секунд
        private void buttonMonitoring_Click(object sender, EventArgs e)
        {

            monitorMiners();

        }

        #endregion

        #endregion
        /////////////////////////////////////////////////////////   /////////////////////////////////////////////////////////   /////////////////////////////////////////////////////////   /////////////////////////////////////////////////////////

        /////////////////////////////////////////////////////////   /////////////////////////////////////////////////////////   /////////////////////////////////////////////////////////   /////////////////////////////////////////////////////////
        #region Functions Network
        //Testing function. Not used

        //Проверка,если сообщение успешно.
        public bool checkIfSuccess(string msg, int msgCode)
        {
            //0- Stats
            //1- Pools
            //2 -Restart
            //3 -enablePool
            //4 -addpool
            //5 -removepool
            //6 -config?


            return true;
        }

        /////////////////////////////////////////////////////////   /////////////////////////////////////////////////////////   /////////////////////////////////////////////////////////   /////////////////////////////////////////////////////////
        #region buttons Network

        private async void buttonReboot_Click(object sender, EventArgs e)
        {
            List<DataGridViewRow> selectedRows = dataGridView1.SelectedCells.Cast<DataGridViewCell>()
                .Select(cell => cell.OwningRow)
                .Distinct().ToList();
            if (selectedRows.Count == 0)
            {
                return;
            }
            DialogResult confirmationWindow = MessageBox.Show("Do you want to reboot selected miners ?" +
                "(Right now selected " + selectedRows.Count + " miners)"
                                   , "Reboot selected miners", MessageBoxButtons.YesNo);

            if (confirmationWindow == DialogResult.Yes)
            {
                enableConfigButtons(false);
                enableRebootButtons(false);
                DialogResult errorWindow = DialogResult.OK;
                List<string> iplist = new List<string>();
                foreach (DataGridViewRow row in selectedRows)
                {
                    iplist.Add((string)row.Cells["IP"].Value);
                }
                Log.rebootReport = new Log.OperationResult(iplist);
                int tryAmount = 0;
                while (errorWindow == DialogResult.OK)
                {
                    tryAmount++;
                    await rebootSelected().ContinueWith((x) =>
                    {
                        if (Log.rebootReport.errorCount > 0)
                        {
                            if (tryAmount > 4)
                            {
                                errorWindow = MessageBox.Show("Result: Success - " + Log.rebootReport.succesCount + "; From selected " + Log.rebootReport.totalCount + " miners.\n Repeat operation for failed?"
                              , "Result", MessageBoxButtons.OKCancel);
                                if (errorWindow != DialogResult.OK)
                                {

                                    Log.rebootReport.report("reboot");

                                }
                            }
                        }
                        else
                        {
                            errorWindow = DialogResult.Cancel;
                            MessageBox.Show(" Result: Success - " + Log.rebootReport.succesCount + "; From selected " + Log.rebootReport.totalCount + " miners."
                          , "Result", MessageBoxButtons.OK);
                            Log.rebootReport.report("reboot");
                        }

                    });
                }
                //enableConfigButtons(true);
                enableConfigButtons(true);
                enableRebootButtons(true);
            }
        }
        private async Task rebootSelected()
        {





            List<Task> taskArray = new List<Task>();
            foreach (string ip in Log.rebootReport.ipResultList.Where(t => t.Value != "").ToDictionary(t => t.Key, t => t.Value).Keys)
            {
                Task task = new Task(async (object obj) =>
                {
                    string ipIner = Convert.ToString(obj);

                    await MinerChangeFunc.rebootManuallyMiner(ipIner);
                }, ip);
                //task.Start();
                taskArray.Add(task);
                task.Start();
            }

            try
            {
                await Task.WhenAll(taskArray);
                
            }
            catch (Exception ex)
            {
                Log.logDebug("\n\r WhenAll Reboot " + Convert.ToString(ex));
            }
        }
        private async void buttonRebootAll_Click(object sender, EventArgs e)
        {
            DialogResult confirmationWindow = MessageBox.Show("Do you want to reboot all miners ?" +
                "(Right now there is " + mainTable.Rows.Count + " miners)"
                                   , "Reboot all miners", MessageBoxButtons.YesNo);
            if (confirmationWindow == DialogResult.Yes)
            {

                enableConfigButtons(false);
                enableRebootButtons(false);

                List<string> iplist = new List<string>();
                foreach (DataGridViewRow row in dataGridView1.Rows)
                {
                    iplist.Add((string)row.Cells["IP"].Value);
                }
                Log.rebootReport = new Log.OperationResult(iplist);
                DialogResult errorWindow = DialogResult.OK;
                int tryAmount = 0;
                while (errorWindow == DialogResult.OK)
                {
                    tryAmount++;
                    await rebootSelected().ContinueWith((x) =>
                    {
                        if (Log.rebootReport.errorCount > 0)
                        {
                            if (tryAmount > 4)
                            {
                                errorWindow = MessageBox.Show("Result: Success - " + Log.rebootReport.succesCount + "; From selected " + Log.rebootReport.totalCount + " miners.\n Repeat operation for failed?"
                              , "Result", MessageBoxButtons.OKCancel);
                                if (errorWindow != DialogResult.OK)
                                {

                                    Log.rebootReport.report("reboot");

                                }
                            }
                        }
                        else
                        {
                            errorWindow = DialogResult.Cancel;
                            MessageBox.Show(" Result: Success - " + Log.rebootReport.succesCount + "; From selected " + Log.rebootReport.totalCount + " miners."
                          , "Result", MessageBoxButtons.OK);
                            Log.rebootReport.report("reboot");
                        }

                    });
                }                //Task lastTask = new Task(() => { });

                enableConfigButtons(true);
                enableRebootButtons(true);
            }

        }
        public void enableConfigButtons(bool state)
        {
            if (InvokeRequired)
                this.Invoke(new System.Windows.Forms.MethodInvoker(delegate
                {
                    buttonConfigSelect.Enabled = state;
                    buttonConfigAll.Enabled = state;
                }));
            else
            {
                buttonConfigSelect.Enabled = state;
                buttonConfigAll.Enabled = state;

            }

        }
        public void enableMonitorButtons(bool state)
        {
            if (InvokeRequired)
                this.Invoke(new System.Windows.Forms.MethodInvoker(delegate
                {

                    buttonMonitor.Enabled = state;
                    button3.Enabled = state;
                    buttonScan.Enabled = state;
                }));
            else
            {
                buttonMonitor.Enabled = state;
                button3.Enabled = state;
                buttonScan.Enabled = state;
            }

        }
        public void enableRebootButtons(bool state)
        {
            if (InvokeRequired)
                this.Invoke(new System.Windows.Forms.MethodInvoker(delegate
                {
                    buttonRebootSelect.Enabled = state;
                    buttonRebootAll.Enabled = state;
                }));
            else
            {
                buttonRebootSelect.Enabled = state;
                buttonRebootAll.Enabled = state;
            }
        }
        private void buttonConfigSelect_Click(object sender, EventArgs e)
        {
            //int SuccessCount = 0;
            List<DataGridViewRow> selectedRows = dataGridView1.SelectedCells.Cast<DataGridViewCell>()
                .Select(cell => cell.OwningRow)
                .Distinct().ToList();
            if (selectedRows.Count == 0)
            {
                return;
            }

            enableConfigButtons(false);
            enableRebootButtons(false);
            enableMonitorButtons(false);
            List<string> iplist = new List<string>();
            foreach (DataGridViewRow row in selectedRows)
            {
                iplist.Add((string)row.Cells["IP"].Value);
            }
            Log.configReport = new Log.OperationResult(iplist);

            //Task lastTask = new Task(() => { });
            List<string> selectedIP = new List<string>();
            List<int> selectedID = new List<int>();
            for (int iter = 0; iter < selectedRows.Count(); iter++)
            {
                //if (Convert.ToString(selectedRows[iter].Cells["Status"].Value).Contains("Success"))
                // {
                selectedID.Add((int)selectedRows[iter].Cells["ID"].Value);
                selectedIP.Add((string)selectedRows[iter].Cells["IP"].Value);
                // }

            }
            ConfigForm configForm = new ConfigForm(this, selectedIP.ToArray());
            //this.Visible = false;
            configForm.ShowDialog();




        }
        private void buttonConfigAll_Click(object sender, EventArgs e)
        {

            enableConfigButtons(false);
            enableRebootButtons(false);
            enableMonitorButtons(false);


            List<string> iplist = new List<string>();
            foreach (DataGridViewRow row in dataGridView1.Rows)
            {
                iplist.Add((string)row.Cells["IP"].Value);
            }
            Log.configReport = new Log.OperationResult(iplist);
            List<string> selectedIP = new List<string>();
            List<int> selectedID = new List<int>();
            for (int iter = 0; iter < dataGridView1.Rows.Count; iter++)
            {
                //if (Convert.ToString(dataGridView1.Rows[iter].Cells["Status"].Value).Contains("Success"))
                //{
                selectedID.Add((int)dataGridView1.Rows[iter].Cells["ID"].Value);
                selectedIP.Add((string)dataGridView1.Rows[iter].Cells["IP"].Value);
                //}



            }
            ConfigForm configForm = new ConfigForm(this, selectedIP.ToArray());
            //this.Visible = false;
            configForm.ShowDialog();

        }

        #endregion

        #endregion
        /////////////////////////////////////////////////////////   /////////////////////////////////////////////////////////   /////////////////////////////////////////////////////////   /////////////////////////////////////////////////////////
        #region UI Functions
        //Заполнение общих сведений
        private void getGeneralMinerInfo()
        {
          
            double tempHashrate = 0;
            int tempTemper = 0;
            int tempTemperMax = 0;
            int tempTemperMin = 150;
            int tempTemperNumber = 0;
            int minerNumber = 0;
            TimeSpan lastTime = new TimeSpan(100, 0, 0, 0, 0);
            try
            {
                foreach (MinerModel model in Sql.minersList)
                {
                    if (model.status != null && model.status.Contains("Success"))
                    {
                        minerNumber++;
                        //Parsing Hashrate
                        tempHashrate += Convert.ToDouble(model.hashRateRT);
                        //Parsing Elapsed
                        try
                        {
                            TimeSpan parsedTime = TimeSpan.ParseExact(model.elapsed ?? "00:00:00:00", "dd\\:hh\\:mm\\:ss", CultureInfo.InvariantCulture);
                            if (parsedTime != new TimeSpan(0, 0, 0) && TimeSpan.Compare(lastTime, parsedTime) > 0)
                            {
                                lastTime = parsedTime;
                            }
                        }
                        catch (Exception ex)
                        {
                            Log.logDebug("Error with convertin Timespan " + Convert.ToString(ex));
                        }


                        List<int> tempArray = new List<int>();
                        tempArray.Add(model.temperature1);
                        tempArray.Add(model.temperature2);
                        tempArray.Add(model.temperature3);
                        tempTemperNumber += 3;

                        for (int i = 0; i < tempArray.Count; i++)
                        {

                            int thisTemp = tempArray[i];
                            if (thisTemp != 0)
                            {
                                if (thisTemp > tempTemperMax)
                                    tempTemperMax = thisTemp;
                                if (thisTemp < tempTemperMin)
                                    tempTemperMin = thisTemp;
                                tempTemper += thisTemp;
                            }

                        }
                    }
                }
                Settings.currentStatus.workingMiners = minerNumber;
                Settings.currentStatus.allMiners = mainTable.Rows.Count;
                Settings.currentStatus.minTemp = tempTemperMin;
                if (tempTemperNumber != 0)
                {
                    Settings.currentStatus.avgTemp = Convert.ToInt32(tempTemper / tempTemperNumber);
                }
                else
                    Settings.currentStatus.avgTemp = 0;
                Settings.currentStatus.MaxTemp = tempTemperMax;
                Settings.currentStatus.allHashrate = Convert.ToDecimal(tempHashrate);
                Settings.currentStatus.lastReboot = DateTime.UtcNow.Add(-lastTime);

            }
            catch(Exception ex)
            {
                Log.logDebug("Error with currentStatus " + Convert.ToString(ex));
            }
            this.BeginInvoke(new System.Windows.Forms.MethodInvoker(delegate
            {
                Task.Run(() =>
                {
                    try
                    {
                        labelHashrate.Text = Convert.ToString(Math.Round(Settings.currentStatus.allHashrate, 2)) + " GH/s\r\n" +
                        Convert.ToString(Math.Round(Settings.currentStatus.allHashrate / 1000, 2)) + " TH/s\r\n" +
                        Convert.ToString(Math.Round(Settings.currentStatus.allHashrate / 1000000, 2)) + " PH/s\r\n";
                        if (tempTemperNumber != 0)
                        {
                            labelTemperature.Text = Convert.ToString(Settings.currentStatus.minTemp) + "/" +
                                Convert.ToString(Settings.currentStatus.avgTemp) + "/" +
                                Convert.ToString(Settings.currentStatus.MaxTemp);
                        }
                        labelMinerNumber.Text = Settings.currentStatus.workingMiners + "/" + Settings.currentStatus.allMiners;
                        labelReboot.Text = string.Format("{0:D2}:{1:D2}:{2:D2} {3:00} Days ago", lastTime.Hours, lastTime.Minutes, lastTime.Seconds, lastTime.Days);
                    }
                    catch (Exception ex)
                    {
                        Log.logDebug("Invoke " + Convert.ToString(ex));
                    }
                });
            }));

        }
        //Добавление ip строки майнеров. 
        public void addRangeIp(string newRange)
        {
            ipRangeBox.Items.Add(newRange, true);
        }
        //Парсинг из настроек и выставление на ui
        public void setRadioIpRange()
        {
            string settingString = Settings.ipRangeString;
            string[] rangeArray = settingString.Split(new[] { ',', '"', ';' }, StringSplitOptions.RemoveEmptyEntries);
            for (int iter = 0; iter < rangeArray.Length; iter++)
            {
                if (rangeArray[iter][0] != '!')
                    ipRangeBox.Items.Add(rangeArray[iter], true);
                else
                {
                    ipRangeBox.Items.Add(rangeArray[iter].Substring(1), false);
                }
            }
            // ipRangeBox.Items.Add(settingString, true);
        }
        //Открывается форма назначения ip
        private void buttonSettingIP_Click(object sender, EventArgs e)
        {

            SettingIP ipForm = new SettingIP(this);
            ipForm.Show();
        }
        //Открывает AddIpRangeForm
        private void buttonRangeListPlus_Click(object sender, EventArgs e)
        {
            AddIpRangeForm rangeWindow = new AddIpRangeForm(this);
            rangeWindow.Show();
            //this.Visible = false;
            this.Enabled = false;
        }
        //Удаление строки ip
        private void buttonRangeListMinus_Click(object sender, EventArgs e)
        {
            if (ipRangeBox.SelectedItem != null)
            {
                ipRangeBox.Items.Remove(ipRangeBox.SelectedItem);
            }
            else
            {
                DialogResult errorWindow = MessageBox.Show("You need to select iprange before deleting"
                                    , "Error", MessageBoxButtons.OK);
            }
        }
        // Button WalletInfo. Opening form with wallet info
        private void button8_Click(object sender, EventArgs e)
        {
            BTCBalanceForm btcForm = new BTCBalanceForm(this);
            this.Visible = false;
            btcForm.Show();
        }
        //Refresh miners
        private async Task refreshMiners()
        {
            enableAllButtons(false);
            //if (mainTable.Rows.Count == 0) return;
            if (Settings.monitoringStatus)
            {
                monitoringTimer.Change(Timeout.Infinite, Timeout.Infinite);
                if (InvokeRequired)
                    this.Invoke(new System.Windows.Forms.MethodInvoker(delegate
                    {
                        buttonMonitor.Text = "Start monitoring";
                    }));
                else
                {
                    buttonMonitor.Text = "Start monitoring";
                }
            }

            this.Invoke(new System.Windows.Forms.MethodInvoker(delegate
            {
                editedTable.Rows.Clear();
                foreach (DataRow dr in mainTable.Rows)
                {
                    editedTable.Rows.Add(dr.ItemArray);
                }
               
            }));
            fillRowIpArray();

            Settings.stopSorting = true;
            //sql_connect.Open();
            Task[] taskArray = new Task[mainTable.Rows.Count];
            progressBar1.Value = 1;

            List<string> ipArray = new List<string>();
            for (int iter = 0; iter < mainTable.Rows.Count; iter++)
            {
                ipArray.Add((string)mainTable.Rows[iter]["IP"]);
            }
            for (int iter = 0; iter < mainTable.Rows.Count; iter++)
            {

                taskArray[iter] = Task.Factory.StartNew(async (Object objec) =>
                {
                    int iterIner = Convert.ToInt32(objec);
                    await addMiner(ipArray[iterIner]);
                }, iter).Unwrap();

            }
            try
            {
                await Task.WhenAll(taskArray);

            }
            catch (Exception ex)
            {
                Log.logDebug("\n\r WhenAll Reboot " + Convert.ToString(ex));
            }
            fillDataGridFromMinerList();
            var tit = Task.WhenAll(taskArray).Exception;
            Log.logDebug("\n\r WhenAll Reboot " + Convert.ToString(tit));
            Settings.stopSorting = false;
            enableAllButtons(true);
        }

        //Refresh miners button
        private async void button3_Click(object sender, EventArgs e)
        {
            await refreshMiners();
        }
        //Scan Miners
        private async Task scanMiners()
        {
            if (Settings.monitoringStatus)
            {
                monitoringTimer.Change(Timeout.Infinite, Timeout.Infinite);
                if (InvokeRequired)
                    this.Invoke(new System.Windows.Forms.MethodInvoker(delegate
                    {
                        buttonMonitor.Text = "Start monitoring";
                    }));
                else
                {
                    buttonMonitor.Text = "Start monitoring";
                }
            }
            while (Settings.connectingToSocketsStatus)
            {
                await Task.Delay(10000);
                Settings.connectingToSocketsStatus = false;
            }
            Settings.stopSorting = true;

            fillRowIpArray();
            progressBar1.Value = 1;





            List<string> ipArray = new List<string>();

            // Set Maximum to the total number of files to copy.               
            // Set the initial value of the ProgressBar.

            // Set the Step property to a value of 1 to represent each file being copied.
            if (InvokeRequired)
                this.Invoke(new System.Windows.Forms.MethodInvoker(delegate { progressBar1.Value = 1; }));
            else
                progressBar1.Value = 1;
            Settings.stopSorting = true;
            for (int iter = 0; iter < mainTable.Rows.Count; iter++)
            {
                ipArray.Add((string)mainTable.Rows[iter]["IP"]);
            }
            Settings.connectingToSocketsStatus = true;
            Task[] taskArray = new Task[ipArray.Count];
            for (int iter = 0; iter < ipArray.Count; iter++)
            {
            

                taskArray[iter] = Task.Factory.StartNew(async (Object objec) =>
                {
                    int iterIner = Convert.ToInt32(objec);
                    await addMiner(ipArray[iterIner]);
                }, iter).Unwrap();
            }
            try
            {
                await Task.WhenAll(taskArray);
            }
            catch (Exception ex)
            {
                Log.logDebug("\n\rWhenAllTask Error " + Convert.ToString(ex));
            }

            Settings.connectingToSocketsStatus = false;


            //Значительно быстрее 
            try
            {
                lock (Sql.minersList)
                {
                    Sql.minersList.RemoveAll(t => t.status != "Success");
                }
            }
            catch (Exception ex)
            {
                Log.logDebug(Convert.ToString(ex));
            }
            fillDataGridFromMinerList();
        }
        //Single tick of monitoring. Button "Scan Miners"
        private async void buttonScan_Click(object sender, EventArgs e)
        {
            //backgroundWorker.DoWork += new DoWorkEventHandler(backgroundWorker_DoWork);
            //backgroundWorker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(backgroundWorker_Completed);
            enableAllButtons(false);
            await scanMiners();
            enableAllButtons(true);
            //refreshDataTable();

        }
        //Open browser with table site
        private void buttonTableSite_Click(object sender, EventArgs e)
        {
            //api.Flysecure.ru
            if (Settings.logged)
                System.Diagnostics.Process.Start(Settings.siteuser + "Mining//Tabledata?userid=" + Log.flyMiningUserName + "&key=" + Log.flyMiningPassword);
        }
        //Button for creating csv
        private async void button1_Click(object sender, EventArgs e)
        {

            List<MinerModel> list = await Sql.executeCommandAndReadMinerModel(string.Format(@" Select " + Sql.columnOrderQuery + @" from NetworkTableHistory Where Mac='{0}'", "00:57:49:8F:88:BD"));//"00:5B:93:4C:F9:A3"));
            if (list.Count > 0)
            {
                MinerModel model = list.First();
                if (model != null)
                {
                    Log.logDebug("updateDB Changed config " + list.Single(t => t.Mac == "00:57:49:8F:88:BD").toString());

                    Task test = new Task(async () =>
       {
           await MinerChangeFunc.configMinerSql(list.Single(t => t.Mac == "00:57:49:8F:88:BD"));
       });
                    test.Start();
                    await test;

                }
            }

        }
        #endregion
        /////////////////////////////////////////////////////////   /////////////////////////////////////////////////////////   /////////////////////////////////////////////////////////   /////////////////////////////////////////////////////////




        public async void button2_Click(object sender, EventArgs e)
        {


            List<MinerModel> list = await Sql.executeCommandAndReadMinerModel(string.Format(@" Select " + Sql.columnOrderQuery + @" from NetworkTableHistory Where Mac='{0}'", "00:57:49:8F:88:BD"));//"00:5B:93:4C:F9:A3"));
            if (list.Count > 0)
            {
                MinerModel model = list.First();
                if (model != null)
                {
                    Log.logDebug("updateDB Changed config " + list.Single(t => t.Mac == "00:57:49:8F:88:BD").toString());
                    MinerChangeFunc.configMinerSql(list.Single(t => t.Mac == "00:57:49:8F:88:BD")).Wait();
                }
            }


        }




        private void checkBoxAutoReport_CheckedChanged(object sender, EventArgs e)
        {
            Settings.stopSorting = true;
            if (Settings.antiReportArray == null)
            {
                Settings.antiReportArray = new Dictionary<string, bool>();
            }

            if (!checkBoxAutoReport.Checked)
            {
                for (int iter = 0; iter < dataGridView1.Rows.Count; iter++)
                {
                    string ip = mainTable.Rows[iter]["IP"].ToString();
                    mainTable.Rows[iter]["StateMail"]  = false;

                    if (!Settings.antiReportArray.Keys.Contains(ip))
                    {
                        Settings.antiReportArray.Add(ip, false);

                    }
                    else
                    {
                        Settings.antiReportArray[ip] = false;
                    }
                }
            }
            else
            {
                for (int iter = 0; iter < dataGridView1.Rows.Count; iter++)
                {
                    (dataGridView1.Rows[iter].Cells["StateMail"] as DataGridViewCheckBoxCell).Value = true;

                    string ip = dataGridView1.Rows[iter].Cells["IP"].Value.ToString();
                    if (!Settings.antiReportArray.Keys.Contains(ip))
                    {
                        Settings.antiReportArray.Add(ip, true);

                    }
                    else
                    {
                        Settings.antiReportArray[ip] = true;
                    }


                }
            }
            Settings.stopSorting = false;
        }

        private void checkBoxAutoReboot_CheckedChanged(object sender, EventArgs e)
        {



            Settings.stopSorting = true;
            if (Settings.antiRebootArray == null)
            {
                Settings.antiRebootArray = new Dictionary<string, bool>();
            }
            if (!checkBoxAutoReboot.Checked)
            {
                for (int iter = 0; iter < dataGridView1.Rows.Count; iter++)
                {
                    string ip = (string)dataGridView1.Rows[iter].Cells["IP"].Value;
                    (dataGridView1.Rows[iter].Cells["State"] as DataGridViewCheckBoxCell).Value = false;

                    if (!Settings.antiRebootArray.Keys.Contains(ip))
                    {
                        Settings.antiRebootArray.Add(ip, false);

                    }
                    else
                    {
                        Settings.antiRebootArray[ip] = false;
                    }

                }

            }
            else
            {
                for (int iter = 0; iter < dataGridView1.Rows.Count; iter++)
                {
                    string ip = (string)dataGridView1.Rows[iter].Cells["IP"].Value;
                    (dataGridView1.Rows[iter].Cells["State"] as DataGridViewCheckBoxCell).Value = true;


                    if (!Settings.antiRebootArray.Keys.Contains(ip))
                    {
                        Settings.antiRebootArray.Add(ip, true);

                    }
                    else
                    {
                        Settings.antiRebootArray[ip] = true;
                    }


                }
            }
            Settings.stopSorting = false;
        }


        public class OperationBTCResponse
        {
            public JArray txs { get; set; }
        }
        private void enableAllButtons(bool state)
        {
            enableConfigButtons(state);
            enableRebootButtons(state);
            enableMonitorButtons(state);
        }



        private async void MainWindow_Load(object sender, EventArgs e)
        {
            enableConfigButtons(false);
            enableRebootButtons(false);
            enableMonitorButtons(false);
            if (await Settings.checkLogin())
            {
                groupBox2.Visible = true;
                await Wallets.parseMarket();

                TimerCallback walletIncomeCallBack = new TimerCallback(Wallets.getWalletsIncome);
                walletIncomeTimer = new System.Threading.Timer(walletIncomeCallBack, null,
                   0, Settings.badCheckTimeout);
                TimeSpan timeToUtc00 = DateTime.UtcNow.AddHours(1).AddMinutes(-DateTime.UtcNow.Minute).AddSeconds(-DateTime.UtcNow.Second) - DateTime.UtcNow;
                walletIncomeCallBack = new TimerCallback(hourlyReports);
                hourlyReport = new System.Threading.Timer(walletIncomeCallBack, null,
                    Convert.ToInt64(timeToUtc00.TotalMilliseconds), 60000);

                usdRateTimer = new System.Threading.Timer(usdRateCallBack, null,
                    Convert.ToInt64(timeToUtc00.TotalMilliseconds), 60000);
            }
            //Wallets.DoTesting();

            Wallets.getWalletsDescriptions();
            //Считываем range ip из ini. Создаем checkedButtons

            Type dgvType = dataGridView1.GetType();
            PropertyInfo pi = dgvType.GetProperty("DoubleBuffered",
              BindingFlags.Instance | BindingFlags.NonPublic);
            pi.SetValue(dataGridView1, true, null);

            //fillRowIpArray();
            //backgroundWorker1.RunWorkerAsync();
            await refreshMiners();
            //sentHourlyHashRate();

            if (Settings.autoScan)
            {
                await scanMiners();
            }
            else
            {
                if (Settings.autoMonitoring)
                {
                    monitorMiners();
                }
                else
                {
                    enableConfigButtons(true);
                    enableRebootButtons(true);
                }
            }
          
            enableMonitorButtons(true);
        }

        private void getWalletIncome(object obj = null)
        {
            Wallets.getWalletsIncome();
            try
            {


                if (InvokeRequired)
                    this.Invoke(new System.Windows.Forms.MethodInvoker(delegate
                    {
                        labelWallet.Text = "";
                        labelDollar.Text = "";
                        double dollarSum = 0;
                        foreach (string key in Wallets.walletsIncome.Keys)
                        {
                            if (Wallets.walletsIncome[key] > 0)
                            {
                                labelWallet.Text += key + ":" + Wallets.walletsIncome[key].ToString() + ";";
                                dollarSum += Math.Round(Wallets.walletsIncome[key] * Wallets.marketInfo[key], 2);
                            }
                        }
                        labelDollar.Text += dollarSum.ToString() + " $    ";
                        if (labelWallet.Text == "")
                        {
                            labelWallet.Text = "No income in last 24 hours";
                            labelDollar.Text = "";
                        }

                    }));
                else
                {
                    labelWallet.Text = "";
                    labelDollar.Text = "";
                    double dollarSum = 0;
                    foreach (string key in Wallets.walletsIncome.Keys)
                    {
                        if (Wallets.walletsIncome[key] > 0)
                        {
                            labelWallet.Text += key + ":" + Wallets.walletsIncome[key].ToString() + "    ";
                            dollarSum += Math.Round(Wallets.walletsIncome[key] * Wallets.marketInfo[key], 2);
                        }
                    }
                    labelDollar.Text += dollarSum.ToString() + " $    ";
                    if (labelWallet.Text == "")
                    {
                        labelWallet.Text = "No income in last 24 hours";
                        labelDollar.Text = "";
                    }
                }
            }
            catch (Exception ex)
            {
                Log.logDebug("Income Error" + Convert.ToString(ex) + "\r\n");
            }
        }


        private void ipRangeBox_MouseDoubleClick(object sender, MouseEventArgs e)
        {

            int index = ipRangeBox.IndexFromPoint(e.Location);

            if (index != System.Windows.Forms.ListBox.NoMatches)
            {
                AddIpRangeForm rangeWindow = new AddIpRangeForm(this, ipRangeBox.Items[index].ToString());
                ipRangeBox.Items.RemoveAt(index);
                rangeWindow.Show();


                //do your stuff here

            }
        }

        private void ipRangeBox_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void button5_Click(object sender, EventArgs e)
        {
            ErrorWindow errorForm = new ErrorWindow(Settings.ipList);

            errorForm.Show();
        }

        private void buttonSaveMinerState_Click(object sender, EventArgs e)
        {
            Sql.saveMinerState(Sql.minersList);
        }

        private void buttonLoadMinerState_Click(object sender, EventArgs e)
        {
            Sql.loadMinerState(Sql.minersList);
        }

        private void label2_Click(object sender, EventArgs e)
        {

        }
    }
}
