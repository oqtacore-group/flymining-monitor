using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using System.Globalization;
namespace BitcoinInfoMiner
{

    public class MinerModel
    {
      
        public int id;
        public string ip;
        public string status;
        public string type;
        public int hashRateRT;
        public int hashRateAverage;
        public string temperatureString
        {
            get
            {
                return this.temperature1.ToString() + "/" + this.temperature2.ToString() + "/" + this.temperature3.ToString();
            }
        }
        public int temperature1;
        public int temperature2;
        public int temperature3;
        public string fanSpeedString
        {
            get {
                return  this.fanSpeed1.ToString() + "/" + this.fanSpeed2.ToString(); 
            }
        }
        public bool selected;
        public int fanSpeed1;
        public int fanSpeed2;
        public string elapsed;
        public string pool1;
        public string pool2;
        public string pool3;
        public string worker1;
        public string worker2;
        public string worker3;
        public string Mac;
        public string conf_netmask;
        public string conf_gateway;
        public string conf_dnsservers;
        public string conf_nettype;
        public string conf_hostname;
        public DateTime lastStatusChangeDate;
        public DateTime created;
        public bool autoreboot;
        public bool autoreport;
        public string toString()
        {
            return this.ip +"//// "+ this.status +"//// "+ this.pool1 +"//// "+ this.pool2 +"//// "+ this.pool3 +"//// "+ this.worker1 +"//// "+ this.worker2 +"//// "+ this.worker3 +"//// "+ this.Mac;
        }
        public MinerModel()
        {
        }
        public MinerModel(DataGridViewRow row)
        {
            created = DateTime.UtcNow;
            ip = Convert.ToString(row.Cells["IP"].Value);
            status = Convert.ToString(row.Cells["Status"].Value);
            type = Convert.ToString(row.Cells["Type"].Value);
            if ((string)row.Cells["Hash Rate RT"].Value != "")
            {
                hashRateRT = Convert.ToInt32(row.Cells["Hash Rate RT"].Value);
            }
            else
                hashRateRT = 0;
            if ((string)row.Cells["Hash Rate Average"].Value != "")
            {
                hashRateAverage = Convert.ToInt32(row.Cells["Hash Rate Average"].Value);
            }
            else
                hashRateAverage = 0;
            String[] temper = Convert.ToString(row.Cells["Temperature"].Value).Split(new char[] { }, StringSplitOptions.RemoveEmptyEntries);
            String[] fan = Convert.ToString(row.Cells["Fan Speed"].Value).Split(new char[] { '\\','/' }, StringSplitOptions.RemoveEmptyEntries);
            temperature1 = temper.Count() > 0 ? Convert.ToInt32(temper[0]) : 0;
            temperature2 = temper.Count() > 1 ? Convert.ToInt32(temper[1]) : 0;
            temperature3 = temper.Count() > 2 ? Convert.ToInt32(temper[2]) : 0;
            fanSpeed1 = fan.Count() > 0 ? Convert.ToInt32(fan[0]) : 0;
            fanSpeed2 = fan.Count() > 1 ? Convert.ToInt32(fan[1]) : 0;

            elapsed = Convert.ToString(row.Cells["Elapsed"].Value);
            pool1 = Convert.ToString(row.Cells["Pool1"].Value);
            worker1 = Convert.ToString(row.Cells["Worker1"].Value);
            pool2 = Convert.ToString(row.Cells["Pool2"].Value);
            worker2 = Convert.ToString(row.Cells["Worker2"].Value);
            pool3 = Convert.ToString(row.Cells["Pool3"].Value);
            worker3 = Convert.ToString(row.Cells["Worker3"].Value);
            selected = row.Selected;
        }
        public MinerModel(string ip,jsonMinerStatus minerStatus,jsonMinerNetworkStatus networkStatus, jsonMinerStatus minerAllStats, jsonMinerStatus summary)
        {
            this.ip=ip;
            try
            {
                if (minerStatus != null) //  && minerStatus.devs!=null && minerStatus.devs.Count > 0
                {
                    foreach (jsonDevsMember member in minerAllStats.stats)
                    {
                        member.parseFreq();
                    }
                    this.type = "Antminer";
                    try
                    {
                        this.hashRateRT = Convert.ToInt32(minerAllStats.stats.Sum(t => t.chain_rate), CultureInfo.InvariantCulture);
                    }
                    catch
                    {
                        this.hashRateRT = 0;
                    }
                    try
                    {
                        this.hashRateAverage = Convert.ToInt32(minerAllStats.stats.Sum(t => t.chain_rateideal), CultureInfo.InvariantCulture);
                    }
                    catch
                    {
                        this.hashRateAverage = 0;
                    }

                    //this.temperatureString 
                    try
                    {
                        this.temperature1 = Convert.ToInt32(minerAllStats.stats[0].temp);
                        this.temperature2 = Convert.ToInt32(minerAllStats.stats[1].temp);
                        this.temperature3 = Convert.ToInt32(minerAllStats.stats[2].temp);
                    }
                    catch
                    {
                        this.temperature1 = 0;
                        this.temperature2 = 0;
                        this.temperature3 = 0;
                    }
                    try
                    {
                        this.fanSpeed1 = minerStatus.stats[0].fanSpeedList[0].speed;
                        this.fanSpeed2 = minerStatus.stats[0].fanSpeedList[0].speed;
                    }
                    catch
                    {
                        this.fanSpeed1 = 0;
                        this.fanSpeed2 = 0;
                    }

                    try
                    {
                        this.type = minerStatus.info.type;
                        this.pool1 = minerStatus.pools[0].url;
                        this.pool2 = minerStatus.pools[1].url;
                        this.pool3 = minerStatus.pools[2].url;
                        this.worker1 = minerStatus.pools[0].user;
                        this.worker2 = minerStatus.pools[1].user;
                        this.worker3 = minerStatus.pools[2].user;
                        this.elapsed = TimeSpan.FromSeconds(Convert.ToInt64(summary.summary[0]["elapsed"])).ToString(@"dd\:hh\:mm\:ss");
                    }
                    catch (Exception ex)
                    {
                        this.elapsed = "";
                        this.pool1 = "";
                        this.pool2 = "";
                        this.pool3 = "";
                        this.worker1 = "";
                        this.worker2 = "";
                        this.worker3 = Convert.ToString(ex);
                    }
                    this.status = "Success";

                }
                else
                {
                    this.status = "Not connected";
                    initEmpty();

                }
            }
            catch (Exception ex)
            {
                Log.logDebug("Damn" +Convert.ToString(ex));
            }
            try
            {
                if (Settings.antiReportArray.Keys.Contains(ip))
                    this.autoreboot = Settings.antiReportArray[ip];
                else
                    this.autoreboot = false;
                if (Settings.antiRebootArray.Keys.Contains(ip))
                    this.autoreport = Settings.antiRebootArray[ip];
                else
                    this.autoreport = false;
                if (networkStatus != null)
                {

                    this.Mac = networkStatus.macaddr;
                    this.conf_netmask = networkStatus.conf_netmask;
                    this.conf_gateway = networkStatus.conf_gateway;
                    this.conf_dnsservers = networkStatus.conf_dnsservers;
                    this.conf_nettype = networkStatus.conf_nettype;
                    this.conf_hostname = networkStatus.conf_hostname;
                }
                this.created = DateTime.UtcNow;
            }
            catch(Exception ex)
            {
                Log.logDebug("Wut&" +Convert.ToString(ex));
            }


     

        }
        public void initEmpty()
        {
            
            type = "";
            hashRateRT = 0;
            hashRateAverage = 0;
            temperature1 = 0;
            temperature2 = 0;
            temperature3 = 0;
            fanSpeed1 = 0;
            fanSpeed2 = 0;
            elapsed = "";
            pool1 = "";
            worker1 = "";
            pool2 = "";
            worker2 = "";
            pool3 = "";
            worker3 = "";
            Mac = null;
            created = DateTime.UtcNow;
        }
        private static Regex digitsOnly = new Regex(@"[^\d]");


    }


}
