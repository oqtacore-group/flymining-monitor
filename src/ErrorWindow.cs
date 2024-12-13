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


    public partial class ErrorWindow : Form
    {
        private StatGroup statData = new StatGroup();
        private List<string> ipList = new List<string>();
        private  List<AsicState> asicList;
        private TimeSpan timeSinceLastUpdate = new TimeSpan(0, 0, 0);
        private DateTime lastUpdateTime = DateTime.Now;
        private decimal averangeTemp=0;
        public ErrorWindow(List<string> ipList)
        {
            InitializeComponent();
            localTimeLabel.Text = DateTime.Now.ToString();
            asicList = new List<AsicState>();
            this.ipList = ipList;
            //Setting app datagrid view
            errorDataGridView.Columns.Add("ID", "ID");
            errorDataGridView.Columns.Add("IP", "IP");
            errorDataGridView.Columns.Add("Fan1Status", "Fan 1 status");
            errorDataGridView.Columns.Add("Fan2Status", "Fan 2 status");
            errorDataGridView.Columns.Add("Asic Chip status", "Asic Chip status");
            errorDataGridView.Columns.Add("HashBoard1", "HashBoard 6");
            errorDataGridView.Columns.Add("HashBoard2", "HashBoard 7");
            errorDataGridView.Columns.Add("HashBoard3", "HashBoard 8");
            errorDataGridView.Columns.Add("Status", "Status");
            errorDataGridView.Columns.Add("Errors", "Error list");
            errorDataGridView.Columns[8].DefaultCellStyle.WrapMode = DataGridViewTriState.True;
            errorDataGridView.Columns[9].DefaultCellStyle.WrapMode = DataGridViewTriState.False;
            errorDataGridView.Columns[8].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            errorDataGridView.Columns[9].AutoSizeMode = DataGridViewAutoSizeColumnMode.ColumnHeader;

            DataGridViewButtonColumn rebootButtonColumn = new DataGridViewButtonColumn();
            rebootButtonColumn.Name = "rebootColumn";
            rebootButtonColumn.Text = "Reboot miner";
            errorDataGridView.Columns.Add(rebootButtonColumn);
            fillTable();
            lastUpdateTimer.Start();
        }


        /// <summary>
        /// Gets all data and fills table
        /// </summary>
        public async void fillTable()
        {
            button1.Enabled = false;
            button2.Enabled = false;
            lastUpdateTime = DateTime.Now;
            timeSincelabel.ForeColor = Color.Black;
            errorDataGridView.Rows.Clear();
            this.asicList = new List<AsicState>();
            List<Task> taskArray = new List<Task>();
            foreach(string ip in this.ipList)
            {
                taskArray.Add(Task.Run(async () =>
                    {
                        AsicState asic = new AsicState(Convert.ToString(ip));
                        if (await asic.initialization()!=null)
                            this.asicList.Add(asic);
                    }));

            }
            try
            {
                await Task.WhenAll(taskArray);
            }
            catch (Exception ex)
            {
                Log.logDebug("\n\r WhenAll fillTable " + Convert.ToString(ex));
            }
            averangeTemp = Math.Round(asicList.Average(t => t.hashBoardAverTemp));
            textBoxAverangeTemp.Text = averangeTemp.ToString();
            filterTable();
            button1.Enabled = true;
            button2.Enabled = true;
        }


        /// <summary>
        /// Filter table based on checkboxes
        /// </summary>
        /// <returns></returns>
        public void filterTable()
        {
            button1.Enabled = false;
            button2.Enabled = false;
            errorDataGridView.Rows.Clear();
            List<DataGridViewRow> rowList = new List<DataGridViewRow>();
            int i = 0;
            List<AsicState> filtered = this.asicList.Where(t => (fan1checkBox.Checked && t.firstFanWorking == false)
                || (fan2checkBox.Checked && t.secondFanWorking == false)
                || (chipErrorcheckBox.Checked && t.hashBoardSummaryChipsState == false)
                || (hashBoardErrorcheckBox.Checked && t.hashBoardSummaryHashrateState == false)
                || (UniqueErrorcheckBox.Checked && t.notableErrorsState == false)
                || (!fan1checkBox.Checked && !fan2checkBox.Checked && !chipErrorcheckBox.Checked && !hashBoardErrorcheckBox.Checked && !UniqueErrorcheckBox.Checked)
                ).OrderBy(t => t.Ip).ToList();
            this.statData = new StatGroup();

            foreach (AsicState asic in filtered)
            {
                DataGridViewRow row = new DataGridViewRow();
                row.CreateCells(errorDataGridView);
                row.Cells[0].Value = i++;
                row.Cells[1].Value = asic.Ip;

                row.Cells[2].Value = (asic.firstFanWorking ? "Ok" : "Bad")+" fannumber:"+asic.firstFan.index;
                row.Cells[2].Style.BackColor = asic.firstFanWorking ? Color.DarkGreen : Color.Red;
                row.Cells[3].Value = (asic.secondFanWorking ? "Ok" : "Bad") + " fannumber:" + asic.secondFan.index;
                row.Cells[3].Style.BackColor = asic.secondFanWorking ? Color.DarkGreen : Color.Red;
                this.statData.replaceFan += (asic.secondFanWorking ? 0 : 1) + (asic.firstFanWorking ? 0 : 1);
                row.Cells[4].Value = asic.hashBoardSummaryChips;
                row.Cells[4].Style.BackColor = asic.hashBoardSummaryChipsState ? Color.DarkGreen : Color.Red;
                this.statData.asicChipMiss += asic.hashBoardCountChips;
                foreach (HashBoardState state in asic.hashBoardList.OrderBy(t => t.Id))
                {
                    switch (state.hashrateState)
                    {
                        case hashrateEnum.Missing:
                            row.Cells[state.Id-1].Value = "Missing";
                            row.Cells[state.Id-1].Style.BackColor = Color.Red;
                            this.statData.missingHashboard++;
                            break;
                        case hashrateEnum.Bad:
                            if ((!asic.firstFanWorking || !asic.secondFanWorking))
                            {
                                row.Cells[state.Id-1].Value = hashrateEnum.None + "(" + state.hashrate + ")";
                                row.Cells[state.Id-1].Style.BackColor = Color.Gray;
                            }
                            else
                            {
                                row.Cells[state.Id-1].Value = state.hashrateState + "(" + state.hashrate + ")";
                                row.Cells[state.Id-1].Style.BackColor = Color.Red;
                                this.statData.hashboardFail++;
                            }
                            break;
                        case hashrateEnum.Half:
                            row.Cells[state.Id-1].Value = state.hashrateState + "(" + state.hashrate + ")";
                            row.Cells[state.Id-1].Style.BackColor = Color.Yellow;
                            break;
                        case hashrateEnum.Healthy:
                            row.Cells[state.Id-1].Value = state.hashrateState + "(" + state.hashrate + ")";
                            row.Cells[state.Id-1].Style.BackColor = Color.DarkGreen;
                            break;
                    }
                }
        
                row.Cells[8].Value = thisMinerStatus(asic);
                row.Cells[9].Value = asic.stringError;
                row.Cells[10].Value = "Reboot miner";
                rowList.Add(row);
            }
            fillTextBox();
            errorDataGridView.Rows.AddRange(rowList.ToArray());
            button2.Enabled = true;
            button1.Enabled = true;
        }
        private void fillTextBox()
        {
            HardBoardMisstextBox.Text=this.statData.missingHashboard.ToString();
            ChipsMissingtextBox.Text = this.statData.asicChipMiss.ToString();
            HashboardFailtextBox.Text = this.statData.hashboardFail.ToString();
            replaceFantextBox.Text = this.statData.replaceFan.ToString();
                
        }
        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            filterTable();
        }


        private void button2_Click(object sender, EventArgs e)
        {
            
           
            fillTable();
            
           
        }

        private void fan2checkBox_CheckedChanged(object sender, EventArgs e)
        {
            filterTable();
        }

        private void chipErrorcheckBox_CheckedChanged(object sender, EventArgs e)
        {
            filterTable();
        }

        private void hashBoardErrorcheckBox_CheckedChanged(object sender, EventArgs e)
        {
            filterTable();
        }

        private void UniqueErrorcheckBox_CheckedChanged(object sender, EventArgs e)
        {
            filterTable();
        }

        private void lastUpdateTimer_Tick(object sender, EventArgs e)
        {
            timeSinceLastUpdate=DateTime.Now-lastUpdateTime;
            if (timeSinceLastUpdate.Hours>0)
            {
                localTimeLabel.ForeColor = Color.Red;
                if (timeSinceLastUpdate.Hours > 6)
                    fillTable();
            }
            localTimeLabel.Text = lastUpdateTime.ToString()+ "  (" + timeSinceLastUpdate.ToString(@"hh\:mm\:ss") + " ago )";
        }

        private void button1_Click(object sender, EventArgs e)
        {
            fan1checkBox.Checked = false;
            fan2checkBox.Checked = false;
            hashBoardErrorcheckBox.Checked = false;
            chipErrorcheckBox.Checked = false;
            UniqueErrorcheckBox.Checked = false;
            filterTable();
        }


        /// <summary>
        /// Gets text for status column based on miner state
        /// </summary>
        /// <param name="miner">Miner state</param>
        /// <returns>What to do to fix</returns>
        ///  enum errorEnum { Patten, TempReadFailed, CresNotOpened, FanLost, Only1Fan, Only0Fan, SomeFanLost };
        private string thisMinerStatus(AsicState miner)
        {
            string result = "";
            List<errorEnum> errorList=miner.notableErrorEnumList;
            bool fanFailed = false;

            if ((errorList.Contains(errorEnum.Only1Fan) || errorList.Contains(errorEnum.Only0Fan) || errorList.Contains(errorEnum.SomeFanLost) || errorList.Contains(errorEnum.FanLost))
                && (!miner.secondFanWorking || !miner.firstFanWorking))
            {
                fanFailed = true;
                result += "Should replace bad fans\n\r ";
            }
            if (miner.hashBoardList.Any(t => t.hashrateState == hashrateEnum.Missing))
            {
                result += "Missing " + miner.hashBoardList.Count(t => t.hashrateState == hashrateEnum.Missing) + " hashboards\n\r ";
            }
            if (miner.hashBoardList.Any(t => t.fwError))
            {
                result += "Firmware error on chain ";
                foreach (HashBoardState state in miner.hashBoardList.Where(t=>t.fwError))
                {
                    result += state.Id+",";
                }
                result = result.Remove(result.Length - 1);
            }
            if (Math.Abs(miner.hashBoardAverTemp - this.averangeTemp) > 13 && Math.Abs(miner.firstFan.speed - miner.secondFan.speed)>1000)
            {
                result += "Strange average temperature and fan speed. Should check fan in the back, maybe he is working because of the front fan\n\r ";
            }
            if (!miner.hashBoardSummaryHashrateState)
            {
               
                if (fanFailed)
                {
                    result += "Hashboard not working because of the fan problem\n\r";
                  
                }                   
                else
                {
                    if (errorList.Contains(errorEnum.TempReadFailed) || errorList.Contains(errorEnum.CresNotOpened) )
                        result += "Probably hashboard not working because of the temperature problem\n\r";
                    if (!miner.hashBoardSummaryChipsState && miner.hashBoardList.Any(t => t.hashrateState != hashrateEnum.Missing))
                        result += "Hashboard not working because of the chip problem\n\r";
                    if (errorList.Contains(errorEnum.Patten))
                    {
                        result += "Patten error detected. Should reboot at least once. Maybe network Error\n\r";
                    }
                }
            }


            return result;
        }


        private void errorDataGridView_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (errorDataGridView.Columns[e.ColumnIndex].Name == "IP")
            {
                System.Diagnostics.Process.Start("http://" + WebCalls.minerLogin + ":" + WebCalls.minerPass + "@" + (string)errorDataGridView.Rows[e.RowIndex].Cells["IP"].Value);
            }
        }

        private async void errorDataGridView_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex != 10)
            {
                try
                {
                    string ip = Convert.ToString(errorDataGridView.Rows[e.RowIndex].Cells["IP"].Value);
                    AsicState selectedAsic = asicList.SingleOrDefault(t => t.Ip == ip);
                    if (selectedAsic == null) return;
                    groupBoxMinerState.Text = "Detailed state of " + ip + " miner";
                    textBoxAverFreq.Text = selectedAsic.hashBoardSummaryFreq;
                    textBoxChipState.Text = selectedAsic.hashBoardCountChips.ToString();
                    textBoxCoreState.Text = selectedAsic.hashBoardSummaryOpenCore;
                    textBoxFanSpeed.Text = selectedAsic.firstFan.speed + "\\" + selectedAsic.secondFan.speed;
                    textBoxIdealHash.Text = selectedAsic.hashBoardSummaryIdealHash;
                    textBoxOffside.Text = selectedAsic.hashBoardSummaryOffside;
                    textBoxTempState.Text = selectedAsic.hashBoardSummaryTemp;
                    textBoxTotalHashrate.Text = selectedAsic.hashBoardSummaryHashrate;
                }
                catch (Exception ex)
                {
                    Log.logDebugTest("CellCleck error " + Convert.ToString(ex));
                }
            }
            else
            {
                string ip = Convert.ToString(errorDataGridView.Rows[e.RowIndex].Cells["IP"].Value);
                await MinerFunc.rebootMiner(ip);
            }

        }
    }
}
