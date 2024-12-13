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

        public int id { get; set; }
        public string ip { get; set; }
        public string status { get; set; }
        public string type { get; set; }
        public decimal hashRateRT { get; set; }
        public decimal hashRateAverage { get; set; }
        public int temperature1 { get; set; }
        public int temperature2 { get; set; }
        public int temperature3 { get; set; }
        public List<int> fanSpeeds { get; set; }
        public int fanSpeed1 { get; set; }
        public int fanSpeed2 { get; set; }
        public int fanSpeed3 { get; set; }
        public int fanSpeed4 { get; set; }


        public string temperatureString
        {
            get
            {
                return this.temperature1.ToString() + "/" + this.temperature2.ToString() + "/" + this.temperature3.ToString();
            }
        }

        public string fanSpeedString
        {
            get
            {
                return this.fanSpeed1.ToString() + "/" + this.fanSpeed2.ToString() + "/" + this.fanSpeed3.ToString() + "/" + this.fanSpeed4.ToString();
            }
        }

        public bool selected;
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
            fanSpeed3 = fan.Count() > 1 ? Convert.ToInt32(fan[2]) : 0;
            fanSpeed4 = fan.Count() > 1 ? Convert.ToInt32(fan[3]) : 0;

            elapsed = Convert.ToString(row.Cells["Elapsed"].Value);
            pool1 = Convert.ToString(row.Cells["Pool1"].Value);
            worker1 = Convert.ToString(row.Cells["Worker1"].Value);
            pool2 = Convert.ToString(row.Cells["Pool2"].Value);
            worker2 = Convert.ToString(row.Cells["Worker2"].Value);
            pool3 = Convert.ToString(row.Cells["Pool3"].Value);
            worker3 = Convert.ToString(row.Cells["Worker3"].Value);
            selected = row.Selected;
        }
        public MinerModel(string ip,jsonMinerStatus minerPools,jsonMinerNetworkStatus networkStatus, jsonMinerStatuses minerAllStats, jsonMinerStatus summary)
        {
            this.ip=ip;
            try
            {
                if (minerPools != null) //  && minerStatus.devs!=null && minerStatus.devs.Count > 0
                {
                    this.type = "Antminer";
                    try
                    {
                        this.hashRateRT = minerAllStats.stats.Count > 0 ? decimal.Parse(minerAllStats.stats[0].rate_5s) : 0 ;
                    }
                    catch
                    {
                        this.hashRateRT = 0;
                    }
                    try
                    {
                        this.hashRateAverage = minerAllStats.stats.Count > 0 ? decimal.Parse(minerAllStats.stats[0].rate_avg) : 0;
                    }
                    catch
                    {
                        this.hashRateAverage = 0;
                    }

                    //this.temperatureString 
                    try
                    {
                        this.temperature1 = minerAllStats.stats.Count > 0 ? Convert.ToInt32(minerAllStats.stats[0].chain[0].temp_chip[0], CultureInfo.InvariantCulture) : 0;
                        this.temperature2 = minerAllStats.stats.Count > 0 ? Convert.ToInt32(minerAllStats.stats[0].chain[1].temp_chip[1], CultureInfo.InvariantCulture) : 0;
                        this.temperature3 = minerAllStats.stats.Count > 0 ? Convert.ToInt32(minerAllStats.stats[0].chain[2].temp_chip[2], CultureInfo.InvariantCulture) : 0;
                    }
                    catch
                    {
                        this.temperature1 = 0;
                        this.temperature2 = 0;
                        this.temperature3 = 0;
                    }
                    try
                    {
                        this.fanSpeed1 = minerAllStats.stats.Count > 0 && minerAllStats.stats[0].fan.Count > 0
                            ? Convert.ToInt32(minerAllStats.stats[0].fan[0], CultureInfo.InvariantCulture)
                            : 0;
                        this.fanSpeed2 = minerAllStats.stats.Count > 0 && minerAllStats.stats[0].fan.Count > 1
                            ? Convert.ToInt32(minerAllStats.stats[0].fan[1], CultureInfo.InvariantCulture)
                            : 0;
                        this.fanSpeed3 = minerAllStats.stats.Count > 0 && minerAllStats.stats[0].fan.Count > 0
                            ? Convert.ToInt32(minerAllStats.stats[0].fan[2], CultureInfo.InvariantCulture)
                            : 0;
                        this.fanSpeed4 = minerAllStats.stats.Count > 0 && minerAllStats.stats[0].fan.Count > 1
                            ? Convert.ToInt32(minerAllStats.stats[0].fan[3], CultureInfo.InvariantCulture)
                            : 0;
                    }
                    catch
                    {
                        this.fanSpeed1 = 0;
                        this.fanSpeed2 = 0;
                        this.fanSpeed3 = 0;
                        this.fanSpeed4 = 0;
                    }

                    try
                    {
                        this.type = minerPools.info.type;
                        this.pool1 = minerPools.pools[0].url;
                        this.pool2 = minerPools.pools[1].url;
                        this.pool3 = minerPools.pools[2].url;
                        this.worker1 = minerPools.pools[0].user;
                        this.worker2 = minerPools.pools[1].user;
                        this.worker3 = minerPools.pools[2].user;
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
            fanSpeeds.Add(0);
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
