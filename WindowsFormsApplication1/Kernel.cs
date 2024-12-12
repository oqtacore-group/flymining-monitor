using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace BitcoinInfoMiner
{

    #region Enums
    /// <summary>
    /// Error codes for parsed error
    /// </summary>
    enum problemsEnum { patten, only1fan, only0fan, fanError };

    /// <summary>
    /// Enum for hashBoard hashrate State
    /// </summary>
    enum hashrateEnum { Bad, Half, Healthy,None,Missing };

    /// <summary>
    /// Enum for hashBoard temo State
    /// </summary>
    enum temperatureEnum { Error, Low, Good, Overheat };

    /// Enum for error from kernel 
    /// </summary>
    enum errorEnum { Patten, TempReadFailed, CresNotOpened, FanLost, Only1Fan, Only0Fan, SomeFanLost,OxOO,restoreFail };
    #endregion


    #region jSonClass


    public class jsonMinerNetworkStatus
    {
        public string nettype { get; set; }
        public string netdevice { get; set; }
        public string macaddr { get; set; }
        public string ipaddress { get; set; }
        public string netmask { get; set; }
        public string conf_nettype { get; set; }
        public string conf_hostname { get; set; }
        public string conf_ipaddress { get; set; }
        public string conf_netmask { get; set; }
        public string conf_gateway { get; set; }
        public string conf_dnsservers { get; set; }
    }
    public class fanInfo
    {
        public int speed { get; set; }
        public int index { get; set; }
        public fanInfo()
        {
            this.speed = 0;
            this.index = 0;
        }
    }



    /// <summary>
    /// json class for miner status responce
    /// </summary>
    public class jsonMinerStatus
    {
        public JToken summary { get; set; }
        public List<jsonMinerPoolStatus> pools { get; set; }
        public List<jsonDevsMember> devs { get; set; }
    }


    public class jsonMinerPoolStatus
    {
       public int index {get;set;}
 public string url {get;set;}
public string user {get;set;}
public string status {get;set;}
    }


    /// <summary>
    /// json class for dev array ofminer status responce
    /// </summary>
    public class jsonDevsMember
    {
        /// <summary>
        /// Parse by , and = and create dictionary based on this
        /// </summary>
        /// <param name="text">unparsed text</param>
        /// <returns>parsed dictionary</returns>
        public Dictionary<String, String> paymentResponceParsing(string text)
        {
            String[] parsedText = text.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
            Dictionary<string, string> result = new Dictionary<string, string>();
            foreach (string row in parsedText)
            {
                string[] parsedRow = row.Split(new char[] { '=' }, StringSplitOptions.RemoveEmptyEntries);
                if (parsedRow != null && parsedRow.Count() > 1 && !result.Keys.Contains(parsedRow[0]))
                    result.Add(parsedRow[0], parsedRow[1]);
            }
            return result;
        }
        public int index { get; set; }
        public int chain_acn { get; set; }
        public string freq { get; set; }
        public int temp { get; set; }
        public decimal freqReal { get; set; }
        /// <summary>
        /// Defacto initialization. There is a bug and freq property is big unparsed string.This parses it.
        /// </summary>
        public void parseFreq()
        {

          
            try
            {
                if (freq.Length == 0)
                    return;
                String firstpart = freq.Substring(0, freq.IndexOf(','));
                this.freqReal = Convert.ToDecimal(firstpart, CultureInfo.InvariantCulture);
                this.freq = freq.Substring(freq.IndexOf(','));
                parsedDict = paymentResponceParsing(this.freq);
                this.temp_num = Convert.ToInt32(parsedDict["temp_num"]);
                this.fan_num = Convert.ToInt32(parsedDict["fan_num"]);
                this.fanSpeedList = new List<fanInfo>();
                for (int iter = 0; iter < 9; iter++)
                {
                    if (parsedDict.Keys.Contains("fan" + iter) && parsedDict["fan" + iter] != "0")
                    {
                        this.fanSpeedList.Add(new fanInfo()
                        {
                            speed = Convert.ToInt32(parsedDict["fan" + iter]),
                            index = iter
                        });
                    }
                }
                while (this.fanSpeedList.Count<2)
                {
                    this.fanSpeedList.Add(new fanInfo()
                    {
                        speed = 0,
                        index = 10
                    });
                }
            }
            catch (Exception ex)
            {

                Log.logDebugTest("!6!" + Convert.ToString(ex) + "\n\r");
            }
            try
            {
                this.total_freqavg = Convert.ToDecimal(parsedDict["total_freqavg"],CultureInfo.InvariantCulture);
                this.total_acn = Convert.ToDecimal(parsedDict["total_acn"], CultureInfo.InvariantCulture);
                this.total_rate = Convert.ToDecimal(parsedDict["total_rate"], CultureInfo.InvariantCulture);

            }
            catch (Exception ex)
            {
                this.total_freqavg = 0;
                this.total_acn = 0;
                this.total_rate = 0;
                Log.logDebugTest("!1!" + Convert.ToString(ex) + "\n\r");
            }
            try
            {
                if (parsedDict.Keys.Contains("chain_hw" + this.index))
                {
                    this.chain_hw = Convert.ToInt32(parsedDict["chain_hw" + this.index], CultureInfo.InvariantCulture);
                }
            }
            catch (Exception ex)
            {
                Log.logDebugTest("!4!" + Convert.ToString(ex) + "\n\r");
            }
            try
            {
                if (parsedDict.Keys.Contains("chain_rate" + this.index))
                {
                    try
                    {
                        this.chain_rate = Convert.ToDecimal(parsedDict["chain_rate" + this.index], CultureInfo.InvariantCulture);
                    }
                    catch
                    {
                        this.chain_rate = 0;
                        Log.logDebugTest("!2!" + parsedDict["chain_rate" + this.index]);
                    }
                }
                if (parsedDict.Keys.Contains("chain_rateideal" + this.index))
                {
                    try
                    {
                        this.chain_rateideal = Convert.ToDecimal(parsedDict["chain_rateideal" + this.index], CultureInfo.InvariantCulture);
                    }
                    catch
                    {
                        this.chain_rateideal = 0;
                        Log.logDebugTest("!2!" + parsedDict["chain_rateideal" + this.index]);
                    }
                }
            }
            catch (Exception ex)
            {
                Log.logDebugTest("!44!" + Convert.ToString(ex) + "\n\r");
            }
            try
            {
                if (parsedDict.Keys.Contains("chain_offside_" + this.index))
                {
                    this.chain_offside = Convert.ToInt32(parsedDict["chain_offside_" + this.index], CultureInfo.InvariantCulture);
                }
                if (parsedDict.Keys.Contains("chain_opencore_" + this.index))
                {
                    this.chain_opencore = Convert.ToInt32(parsedDict["chain_opencore_" + this.index], CultureInfo.InvariantCulture);
                }
            }
            catch (Exception ex)
            {
                Log.logDebugTest("!3!" + Convert.ToString(ex) + "\n\r");
            }

        }
        public Dictionary<String, String> parsedDict { get; set; }
        public int fan_num { get; set; }
        public List<fanInfo> fanSpeedList { get; set; }
        public int temp_num { get; set; }
        public List<int> fan_numbers { get; set; }
        public decimal total_freqavg { get; set; }
        public decimal total_acn { get; set; }
        public decimal total_rate { get; set; }

        public int chain_hw { get; set; }
        public decimal chain_rate { get; set; }
        public decimal chain_rateideal { get; set; }
        
        public int chain_offside { get; set; }
        public int chain_opencore { get; set; }

    }

    #endregion
    public class StatGroup
    {
        public StatGroup()
        {
            replaceFan = 0;
            missingHashboard = 0;
            hashboardFail = 0;
            asicChipMiss = 0;
        }
        public int replaceFan { get; set; }
        public int missingHashboard { get; set; }
        public int hashboardFail { get; set; }
        public int asicChipMiss { get; set; }
    }

    /// <summary>
    /// Class for asic state
    /// </summary>
    class AsicState
    {

        public AsicState()
        {
            this.hashBoardList = new List<HashBoardState>();
            this.notableErrorList = new List<string>();
            this.notableErrorEnumList = new List<errorEnum>();
            hashBoardList.Add(new HashBoardState(6));
            hashBoardList.Add(new HashBoardState(7));
            hashBoardList.Add(new HashBoardState(8));
        }

        public AsicState(string ip)
        {
            this.Ip = ip;
            this.hashBoardList = new List<HashBoardState>();
            this.notableErrorList = new List<string>();
             this.notableErrorEnumList = new List<errorEnum>();
            
            //jsonMinerStatus status = Kernel.getStatusDataText("E://testingParsing.txt").Result;

        }


        public void addNotableEnumError(errorEnum error)
        {
            if (!this.notableErrorEnumList.Contains(error))
                this.notableErrorEnumList.Add(error);
        }

        /// <summary>
        /// Нужно запускать после конструктора. Получает все данные
        /// </summary>
        /// <returns>Возвращает этот элемент.При ошибки возвращает элемент== конструктор с аргументом ip</returns>
        public async Task<AsicState> initialization()
        {
            jsonMinerStatus status = new jsonMinerStatus();
            try
            {
               //status = await Kernel.getStatusDataText("E://testingParsing.txt");
                status = await Kernel.getStatusData(this.Ip);
               
            }
            catch (Exception ex)
            {
                Log.logDebugTest("!getStatusData?!" + Convert.ToString(ex) + "\n\r");
                string error = Convert.ToString(ex);
                //this.secondFan = 3000;
            }
            try
            {


                
                if (status.devs != null)
                {
                    foreach (jsonDevsMember member in status.devs)
                    {
                        member.parseFreq();
                        hashBoardList.Add(new HashBoardState(member));
                    }
                    if (status.devs.Count>0)
                        this.getFanStatus(status.devs[0]);
                    for (int iter=6;iter<9;iter++)
                    {
                        if (!hashBoardList.Any(t=>t.Id==iter))
                            hashBoardList.Add(new HashBoardState(iter));
                    }
                }
                else
                {
                    for (int iter = 6; iter < 9; iter++)
                    {
                        if (!hashBoardList.Any(t => t.Id == iter))
                            hashBoardList.Add(new HashBoardState(iter));
                    }
                    //while (this.fanSpeedList.Count < 2)
                    //{
                    //    this.fanSpeedList.Add(new fanInfo()
                    //    {
                    //        speed = 0,
                    //        index = 10
                    //    });
                    //}
                }
                try
                {
                    await parseKernelText();
                }
                catch (Exception ex)
                {
                    Log.logDebugTest("!Kernel?!" + Convert.ToString(ex) + "\n\r");
                }
                return this;
            }
            catch (Exception ex)
            {
                Log.logDebugTest("!0!"+Convert.ToString(ex) + "\n\r");
                //this.secondFan = 3000;
                return null;
            }
           

        }


        /// <summary>
        /// Fills fan speed property using parsed dev array from miner status responce
        /// </summary>
        /// <param name="member"></param>
        private void getFanStatus(jsonDevsMember member)
        {

                //HashBoardState slate = this.hashBoardList[0];
                switch(member.fan_num)
                {
                    case 0:
                        break;
                    case 1:
                        this.firstFan = member.fanSpeedList.OrderBy(t => t.index).First();
                        break;
                    case 2:
                        this.firstFan = member.fanSpeedList.OrderBy(t=>t.index).First();
                        this.secondFan = member.fanSpeedList.OrderBy(t => t.index).Last();
                        break;
                    default:
                        break;
                }
            
        }

        /// <summary>
        /// get and parse get_kernel_log responce
        /// </summary>
        /// <returns></returns>
        public async Task parseKernelText()
        {
            string unparsedText = "";
            try
            {
                unparsedText = await Kernel.getKernelData(this.Ip);
                //unparsedText = await Kernel.getKernelDataText("E://testKernel.txt");
            }
            catch (Exception ex)
            {
                Log.logDebugTest("!parsed!"+Convert.ToString(ex));
                return;
            }
            //First find first chain
            try
            {
                int firstChainIndex = 0;
                while (firstChainIndex < 10)
                {
                    firstChainIndex++;
                    if (unparsedText.Contains("chain[" + firstChainIndex + "]"))
                    {
                        break;
                    }
                }



                //Check chain[5] PIC fw version=0x00
                
                //Second parse Test patten(shoe) boys! 
                for (int i = 0; i < this.hashBoardList.Count; i++)
                {
                    if (unparsedText.Contains("Test Patten on chain[" + (i + firstChainIndex).ToString() + "]: FAILED!"))
                    {
                        //Saving problem
                        this.hashBoardList[i ].notableErrorList.Add("Test Patten on chain[" + (i + firstChainIndex).ToString()+ "]: FAILED!");
                        this.addNotableEnumError(errorEnum.Patten);

                    }
                    if (unparsedText.Contains("read failed, old value: Chain[" + (i + firstChainIndex).ToString() + "]"))
                    {
                        //Saving problem
                        this.hashBoardList[i ].notableErrorList.Add("Temp read failed on chain[" + (i + firstChainIndex).ToString()+ "]");
                        this.addNotableEnumError(errorEnum.TempReadFailed);

                    }   
                    if (unparsedText.Contains("chain[" + (i + firstChainIndex).ToString() + "]: some chip cores are not opened"))
                    {
                        //Saving problem
                        this.hashBoardList[i].notableErrorList.Add("Some chip cores are not opened on chain[" + (i + firstChainIndex).ToString() + "]");
                        this.addNotableEnumError(errorEnum.CresNotOpened);
                    }
                    if (unparsedText.Contains("Check chain[" + (i + firstChainIndex).ToString() + "] PIC fw version=0x00"))
                    {
                        //Saving problem
                        this.hashBoardList[i].notableErrorList.Add("Firmware error on chain[" + (i + firstChainIndex).ToString() + "]");
                        this.addNotableEnumError(errorEnum.OxOO);
                    }

                    if (unparsedText.Contains("After restore: chain[" + (i + firstChainIndex).ToString() + "] PIC fw version=0x00"))
                    {
                        //Saving problem
                        this.hashBoardList[i].notableErrorList.Add("Firmware errored restore on chain[" + (i + firstChainIndex).ToString() + "]");
                        this.addNotableEnumError(errorEnum.restoreFail);
                    }  
                       /// <summary>

                }

                //Fatal Error: Fan lost!
                //check FAN ERROR: fan num=1 , ought to be 2
                if (unparsedText.Contains("Fatal Error: Fan lost!"))
                {
                    this.notableErrorList.Add("Fatal Error: Fan lost!");
                    this.addNotableEnumError(errorEnum.FanLost);
                    //do something
                }


                //Fatal Error: some Fan lost or Fan speed low!
                if (unparsedText.Contains("check FAN ERROR: fan num=1 , ought to be 2"))
                {
                    this.notableErrorList.Add("check FAN ERROR: fan num=1 , ought to be 2");
                    this.addNotableEnumError(errorEnum.Only1Fan);
                    //do something
                }
                if (unparsedText.Contains("check FAN ERROR: fan num=0 , ought to be 2"))
                {
                    this.notableErrorList.Add("check FAN ERROR: fan num=0 , ought to be 2");
                    this.addNotableEnumError(errorEnum.Only0Fan);
                    //do something
                }

                if (unparsedText.Contains("Fatal Error: some Fan lost or Fan speed low!"))
                {
                    this.notableErrorList.Add("Fatal Error: some Fan lost or Fan speed low!");
                    this.addNotableEnumError(errorEnum.SomeFanLost);
                }

            }
            catch (Exception ex)
            {
                Log.logDebugTest("!IFparsedIF!" + Convert.ToString(ex));
                return;
            }

        }





        /// <summary>
        /// local ip for this asic
        /// </summary>
        public string Ip { get; set; }
        public fanInfo firstFan { get; set; }
        public fanInfo secondFan { get; set; }
        public bool firstFanWorking
        {

            get
            {
                if (firstFan == null)
                    this.firstFan = new fanInfo();
                return firstFan.speed > 2000;
            }
        }
        public bool secondFanWorking
        {

            get
            {
                if (secondFan == null)
                    this.secondFan = new fanInfo();
                return secondFan.speed > 2000;
            }
        }
        public List<string> notableErrorList { get; set; }
        public List<errorEnum> notableErrorEnumList { get; set; }
        public bool notableErrorsState
        {
            get
            {
                //if ()
                //    return false;
                //foreach (HashBoardState state in hashBoardList)
                //{
                //    if (state.notableErrorList.Count != 0)
                //        return false;
                //}
                return notableErrorEnumList.Count == 0;
            }
        }
        /// <summary>
        /// HashBoard of this asic
        /// </summary>
        public List<HashBoardState> hashBoardList { get; set; }


        public string stringError
        {
            get
            {
                string result = "";
                if (notableErrorList!=null && notableErrorList.Count > 0)
                    result += String.Join("\n\r", this.notableErrorList);
                foreach (HashBoardState slate in this.hashBoardList)
                {
                    if (slate.notableErrorList!=null)
                    {
                        result += "\n\r" + String.Join("\\", slate.notableErrorList);
                    }
                }
                return result;
            }
        }


        /// <summary>
        /// Create string from hashBoard openCore
        /// </summary>
        public string hashBoardSummaryOpenCore
        {
            get
            {
                List<string> result = new List<string>();
                foreach (HashBoardState slate in hashBoardList)
                {
                    result.Add(slate.chain_opencore.ToString());
                }
                return String.Join("\\", result);
            }
        }


        /// <summary>
        /// Create string from hashBoard Offside
        /// </summary>
        public string hashBoardSummaryOffside
        {
            get
            {
                List<string> result = new List<string>();
                foreach (HashBoardState slate in hashBoardList)
                {
                    result.Add(slate.chain_offside.ToString());
                }
                return String.Join("\\", result);
            }
        }


        /// <summary>
        /// Create string from hashBoard freq
        /// </summary>
        public string hashBoardSummaryFreq
        {
            get
            {
                return Math.Round(hashBoardList.Where(t => t.freq.HasValue).ToList().Sum(t => t.freq).Value / hashBoardList.Count).ToString();
            }
        }


        /// <summary>
        /// Create string from hashBoard ideal
        /// </summary>
        public string hashBoardSummaryIdealHash
        {
            get
            {
                return hashBoardList.Where(t=>t.chain_rateideal.HasValue).Sum(t=>t.chain_rateideal.Value).ToString();
            }
        }


        /// <summary>
        /// Create string from hashBoard temp
        /// </summary>
        public string hashBoardSummaryTemp
        {
            get
            {
                List<string> result = new List<string>();
                foreach (HashBoardState slate in hashBoardList)
                {
                    result.Add(slate.temp.ToString());
                }
                return String.Join("\\", result);
            }
        }


        /// <summary>
        /// averange hashBoard temp
        /// </summary>
        public decimal hashBoardAverTemp
        {
            get
            {
                decimal result = 0;
                int i=0;
                foreach (HashBoardState state in hashBoardList)
                {
                    if (state.temp.HasValue && state.temp.Value!=0)
                    {
                        result += state.temp.Value;
                        i++;
                    }
                }
                return i==0?0:Convert.ToDecimal(Math.Round(result/i));
            }
        }


        /// <summary>
        /// Create string from hashBoard hashrate
        /// </summary>
        public string hashBoardSummaryHashrate
        {
            get
            {
                return   hashBoardList.Where(t=>t.hashrate.HasValue).Sum(t=>t.hashrate.Value).ToString();
            }
        }


        /// <summary>
        /// For filter
        /// </summary>
        public bool hashBoardSummaryHashrateState
        {
            get
            {
                List<string> result = new List<string>();
                if (hashBoardList.Count != 3)
                    return false;
                foreach (HashBoardState slate in hashBoardList)
                {
                    if (slate.hashrateState!=hashrateEnum.Healthy)
                        return false;
                }
                return true;
            }
        }


        /// <summary>
        /// number of missing chips
        /// </summary>
        public int  hashBoardCountChips
        {
            get
            {
                int result = 0;
                foreach (HashBoardState slate in hashBoardList)
                {
                    if (slate.chips.HasValue)
                        result+=63-slate.chips.Value;
                }
                return result;
            }
        }


        /// <summary>
        /// Create string from hashBoard chips
        /// </summary>
        public string hashBoardSummaryChips
        {
            get
            {
                List<string> result = new List<string>();
                foreach (HashBoardState slate in hashBoardList.OrderBy(t=>t.Id))
                {
                    result.Add(slate.chips.ToString());
                }
                return String.Join("\\", result);
            }
        }

        /// <summary>
        /// For filter
        /// </summary>
        public bool hashBoardSummaryChipsState
        {
            get
            {
                List<string> result = new List<string>();
                if (hashBoardList.Count != 3)
                    return false;
                foreach (HashBoardState slate in hashBoardList)
                {
                    if (!slate.chips.HasValue || slate.chips.Value != 63)
                        return false;
                }
                return true;
            }
        }

    }


    /// <summary>
    /// Sstate properties of hashBoard
    /// </summary>
    class HashBoardState
    {

        public HashBoardState()
        {
            this.notableErrorList = new List<string>();
            this.notableErrorEnumList = new List<errorEnum>();
        }
        public HashBoardState(int id)
        {
            this.notableErrorList = new List<string>();
            this.notableErrorEnumList = new List<errorEnum>();
            this.Id = id;
        }
        public HashBoardState(jsonDevsMember member)
        {
            this.notableErrorList = new List<string>();
            this.notableErrorEnumList = new List<errorEnum>();
            this.Id = member.index;
            this.temp =  member.temp;
            this.hashrate = Math.Round( member.chain_rate);
            this.chips =  member.chain_acn;
            this.chain_offside = member.chain_offside;
            this.chain_opencore =  member.chain_opencore;
            this.freq =  member.freqReal;
            this.chain_rateideal = Math.Round(member.chain_rateideal);
        }
        public int Id { get; set; }
        public int? temp { get; set; }
        public decimal? hashrate { get; set; }
        public int? chips { get; set; }
        public int? chain_offside { get; set; }
        public decimal? chain_rateideal { get; set; }
        public int? chain_opencore { get; set; }
        public decimal? freq { get; set; }
        public List<string> notableErrorList { get; set; }
        public List<errorEnum> notableErrorEnumList { get; set; }
        /// <summary>
        /// HashBoard hashrate state
        /// </summary>
        public hashrateEnum hashrateState
        {
            get
            {
               
                if (!hashrate.HasValue)
                    return hashrateEnum.Missing;
                if (hashrate.Value < 1000)
                    return hashrateEnum.Bad;
                if (hashrate.Value > 4000)
                    return hashrateEnum.Healthy;
                return hashrateEnum.Half;
            }
        }
        /// <summary>
        /// HashBoard temperature state
        /// </summary>
        public temperatureEnum tempState
        {
            get
            {
                if (!temp.HasValue)
                    return temperatureEnum.Error;
                if (temp.Value < 40)
                    return temperatureEnum.Low;
                if (temp.Value > 90)
                    return temperatureEnum.Overheat;
                return temperatureEnum.Good;
            }
        }

        public bool fwError
        {
            get
            {
                return (notableErrorEnumList.Contains(errorEnum.OxOO) && notableErrorEnumList.Contains(errorEnum.restoreFail));
            }
        }

    }

    /// <summary>
    ///static  Class about working with kernel log. 
    /// </summary>
    static class Kernel
    {
        /// <summary>
        /// Path to kernel log
        /// </summary>
        private static string kernelPath = "/cgi-bin/get_kernel_log.cgi";
        private static string statusPath = "/cgi-bin/get_miner_status.cgi";
        private static string networkPath = "/cgi-bin/get_network_info.cgi";
        /// <summary>
        /// Http client for kernel access
        /// </summary>
        private static HttpClient client = new HttpClient();



        /// <summary>
        /// Gets network data info
        /// </summary>
        /// <param name="ip"></param>
        /// <returns></returns>
        public static async Task<jsonMinerNetworkStatus> getNetworkData(string ip)
        {
            try
            {
                var url = "http://" + WebCalls.minerLogin + ":" + WebCalls.minerPass + "@" + ip + networkPath;
                HttpClientHandler handler = new HttpClientHandler();
                handler.Credentials = new System.Net.NetworkCredential(WebCalls.minerLogin, WebCalls.minerPass);
                client = new HttpClient(handler);
                client.Timeout = new TimeSpan(0, 0,10);
                var byteArray = Encoding.ASCII.GetBytes(WebCalls.minerLogin + ":" + WebCalls.minerPass);
                var response = await client.GetAsync(url).ConfigureAwait(false);
                string text = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                jsonMinerNetworkStatus result = JsonConvert.DeserializeObject<jsonMinerNetworkStatus>(text);

                return result;
            }
            catch (System.Net.Http.HttpRequestException ex)
            {
               
                return null;
            }
            catch (System.Threading.Tasks.TaskCanceledException)
            {
                return null;
            }
            catch (Exception ex)
            {
                Log.logDebug("getNetworkData" + Convert.ToString(ex));
                return null;
            }
        }


        /// <summary>
        /// Gets kernel log info
        /// </summary>
        /// <param name="ip"></param>
        /// <returns></returns>
        public static async Task<string> getKernelData(string ip)
        {
            try
            {
                var url = "http://" + WebCalls.minerLogin + ":" + WebCalls.minerPass + "@" + ip + kernelPath;
                HttpClientHandler handler = new HttpClientHandler();
                handler.Credentials = new System.Net.NetworkCredential(WebCalls.minerLogin, WebCalls.minerPass);
                client = new HttpClient(handler);
                client.Timeout = new TimeSpan(0, 0, 10);
                var byteArray = Encoding.ASCII.GetBytes(WebCalls.minerLogin + ":" + WebCalls.minerPass);
                var response = await client.GetAsync(url).ConfigureAwait(false);
                return await response.Content.ReadAsStringAsync().ConfigureAwait(false); ;
            }
            catch (System.Net.Http.HttpRequestException)
            {

                return "";
            }
        }

        /// <summary>
        /// Get status data 
        /// </summary>
        /// <param name="ip">Asic ip</param>
        /// <returns>returns Deserialized json object</returns>
        public static async Task<jsonMinerStatus> getStatusData(string ip)
        {
            try
            {
                var url = "http://" + WebCalls.minerLogin + ":" + WebCalls.minerPass + "@" + ip + statusPath;
                HttpClientHandler handler = new HttpClientHandler();
                handler.Credentials = new System.Net.NetworkCredential(WebCalls.minerLogin, WebCalls.minerPass);
                client = new HttpClient(handler);
                client.Timeout = new TimeSpan(0, 0,10);
                var byteArray = Encoding.ASCII.GetBytes(WebCalls.minerLogin + ":" + WebCalls.minerPass);
                var response = await client.GetAsync(url).ConfigureAwait(false);
                string text = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                jsonMinerStatus result = JsonConvert.DeserializeObject<jsonMinerStatus>(text);

                return result;
            }
            catch (System.Net.Http.HttpRequestException ex)
            {

                return null;
            }
            catch (System.Threading.Tasks.TaskCanceledException)
            {
                return null;
            }
            catch(Exception ex)
            {
                Log.logDebug("getStatusData" + Convert.ToString(ex));
                return null;
            }
        }


        /// <summary>
        /// For testing only
        /// </summary>
        /// <param name="textFile"></param>
        /// <returns></returns>
        public static string getKernelDataText(string textFile)
        {
            try
            {
                string text;
                var fileStream = new FileStream(textFile, FileMode.Open,
                FileAccess.Read); //open text file
                //vvv read text file (or however you implement it like here vvv
                using (var streamReader = new StreamReader(fileStream, Encoding.UTF8))
                {
                    text = streamReader.ReadToEnd();
                }
                //finally, close text file
                fileStream.Close();
                return text;
            }
            catch (System.Net.Http.HttpRequestException )
            {

                return "";
            }
        }


        /// <summary>
        /// For testing only
        /// </summary>
        /// <param name="textFile"></param>
        /// <returns></returns>
        public static jsonMinerStatus getStatusDataText(string textFile)
        {
            try
            {
                string text;
                var fileStream = new FileStream(textFile, FileMode.Open,
                FileAccess.Read); //open text file
                //vvv read text file (or however you implement it like here vvv
                using (var streamReader = new StreamReader(fileStream, Encoding.UTF8))
                {
                    text = streamReader.ReadToEnd();
                }
                //finally, close text file
                fileStream.Close();
                return JsonConvert.DeserializeObject<jsonMinerStatus>(text);
            }
            catch (System.Net.Http.HttpRequestException )
            {

                return new jsonMinerStatus();
            }
        }





    }
}
