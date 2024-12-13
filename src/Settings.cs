using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BitcoinInfoMiner
{
    class Settings
    {
        public string settingFileName = "flymining.ini";
        public static string siteuser = "https://flymining.cloud//app//";
        //public static string siteuser = "http://localhost:51068//";
        public static bool logged = false;
        public const string defaultErrorState = "Failed to connect";
        public  const string noConnectErrorState = "No conncection";
        public const string configErrorState = "Config miner...";
        public const string rebootErrorState = "Reboot miner...";
        public static  List<string> ipList = new List<string>();
        public static string ipRangeString="";
         public class MinerStatus
        {
            public int workingMiners;
            public int allMiners;
            public decimal allHashrate;
            public int minTemp;
            public int avgTemp;
            public int MaxTemp;
            public DateTime lastReboot;
        }
        public static MinerStatus currentStatus=new MinerStatus();
        public static bool stopSorting = false;
        public static bool fullWalletInfoState = false;
        public struct Pool
        {
            public bool enabled;
            public string url;
            public string worker;
            public string psw;
            public bool postfixIp;
            public bool postfixNoChange;
            public bool postfixNone;
        }
        public struct ipRange
        {
            //public bool diffHigh;
            public int lowMinIP;
            public int lowMaxIP;
            public int highMinIP;
            public int highMaxIP;
        }
        public static Dictionary<string, string> minerErrorState;
        enum  jsonMsgReply { Stats, Pools, Restart, EnablePool, Addpool, Removepool, Config, DisablePool };
        public static Pool pool1, pool2, pool3;
        public static bool showOnlySuccess = true;
        public static Dictionary<string, bool> antiRebootArray = new Dictionary<string, bool>();
        public static Dictionary<string,bool> antiReportArray=new Dictionary<string,bool>();
        public static bool parseReportNowTimeout = false;

        public static int abnormalTemp = 90;
        public static int minimalTemp = 10;
        public static int dbbackupTimer = 5;
        public static int apiCheckTimeout = 1800000;
        public static int apiCheckStart = 1800000;
        public static bool apiCheckState = true, HourlyReportFound = false;
        public static bool onStartUp = false;
        public static bool autoScan = false;
        public static bool autoMonitoring = false;
        public static int antMinerHashMin = 10000;
        public static int monitoringTimeout = 30000; // Частота рефреша при мониторинге
        public static int badCheckTimeout = 1800000; // Частота проверки статусов (При отключенном мониторинге)
        public static int badCheckFirstStart = 10000; // Первая проверка статуса майнеров
        public static bool detectHighTemp;


        public static bool monitoringStatus = false;// Автообновления состояние майнеров.
        public static bool connectingToSocketsStatus = false;// True, если в процессе обновления статусов майнеров.
        #region Settings || Logs
        //Sets value,before parsing settings
        public static  void initSettingsProperty()
        {
            pool1.enabled = false;
            pool2.enabled = false;
            pool3.enabled = false;
            pool1.url = "";
            pool2.url = "";
            pool3.url = "";
            pool1.worker = "";
            pool2.worker = "";
            pool3.worker = "";
            pool1.psw = "";
            pool2.psw = "";
            pool3.psw = "";
            pool1.postfixIp = true;
            pool2.postfixIp = true;
            pool3.postfixIp = true;
            pool1.postfixNoChange = false;
            pool2.postfixNoChange = false;
            pool3.postfixNoChange = false;
            pool1.postfixNone = false;
            pool2.postfixNone = false;
            pool3.postfixNone = false;

        }
        public static  async Task<bool> checkLogin()
        {
            Dictionary<string, string> addParameters = new Dictionary<string, string>();
            addParameters.Add("login", Log.flyMiningUserName);
            addParameters.Add("key", Log.flyMiningPassword);
            string result = await Wallets.sendAsyncRequest("checkLogin", Settings.siteuser + "Mining//", addParameters).ConfigureAwait(false);
            if ( UtilityFunc.ReplaceWhitespace(result.ToLower(),"") == "true")
                logged= true;
            else
                logged = false;
            return logged;
        }


        //Парсит файл настроек
        public static void parseSettings()
        {

            if (File.Exists(MainWindow.pathSetting))
            {
                string settingsData = File.ReadAllText(MainWindow.pathSetting, Encoding.Unicode);
                string temp;
                string[] tempArray;
                string[] settingsArray = settingsData.Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);
                for (int iter = 0; iter < settingsArray.Length; )
                {
                    if (settingsArray[iter][0] == '[')
                    {
                        switch (settingsArray[iter])
                        {
                            case "[ui]":
                                iter++;
                                while (iter < settingsArray.Length && settingsArray[iter][0] != '[')
                                {
                                    temp = settingsArray[iter].Remove(settingsArray[iter].IndexOf('='));
                                    switch (settingsArray[iter].Remove(settingsArray[iter].IndexOf('=')))
                                    {
                                        case "onlySuccessMiners":
                                            temp = settingsArray[iter].Substring
                                                (settingsArray[iter].IndexOf('=') + 1);
                                            showOnlySuccess = Convert.ToBoolean(settingsArray[iter].Substring
                                                (settingsArray[iter].IndexOf('=') + 1), CultureInfo.InvariantCulture);
                                            iter++;
                                            break;
                                        case "pool1Enabled":
                                            pool1.enabled = Convert.ToBoolean(settingsArray[iter].Substring
                                                (settingsArray[iter].IndexOf('=') + 1), CultureInfo.InvariantCulture);
                                            iter++;
                                            break;
                                        case "pool1Url":
                                            pool1.url = Convert.ToString(settingsArray[iter].Substring
                                                (settingsArray[iter].IndexOf('=') + 1), CultureInfo.InvariantCulture);
                                            iter++;
                                            break;
                                        case "pool1Worker":
                                            pool1.worker = Convert.ToString(settingsArray[iter].Substring
                                                (settingsArray[iter].IndexOf('=') + 1), CultureInfo.InvariantCulture);
                                            iter++;
                                            break;
                                        case "pool1Pwd":
                                            pool1.psw = Convert.ToString(settingsArray[iter].Substring
                                                (settingsArray[iter].IndexOf('=') + 1), CultureInfo.InvariantCulture);
                                            iter++;
                                            break;
                                        case "pool1PostfixIp":
                                            pool1.postfixIp = Convert.ToBoolean(settingsArray[iter].Substring
                                                (settingsArray[iter].IndexOf('=') + 1), CultureInfo.InvariantCulture);
                                            iter++;
                                            break;
                                        case "pool1PostfixNoChange":
                                            pool1.postfixNoChange = Convert.ToBoolean(settingsArray[iter].Substring
                                                (settingsArray[iter].IndexOf('=') + 1), CultureInfo.InvariantCulture);
                                            iter++;
                                            break;
                                        case "pool1PostfixNone":
                                            pool1.postfixNone = Convert.ToBoolean(settingsArray[iter].Substring
                                                (settingsArray[iter].IndexOf('=') + 1), CultureInfo.InvariantCulture);
                                            iter++;
                                            break;
                                        case "pool2Enabled":
                                            pool2.enabled = Convert.ToBoolean(settingsArray[iter].Substring
                                                (settingsArray[iter].IndexOf('=') + 1), CultureInfo.InvariantCulture);
                                            iter++;
                                            break;
                                        case "pool2Url":
                                            pool2.url = Convert.ToString(settingsArray[iter].Substring
                                                (settingsArray[iter].IndexOf('=') + 1), CultureInfo.InvariantCulture);
                                            iter++;
                                            break;
                                        case "pool2Worker":
                                            pool2.worker = Convert.ToString(settingsArray[iter].Substring
                                                (settingsArray[iter].IndexOf('=') + 1), CultureInfo.InvariantCulture);
                                            iter++;
                                            break;
                                        case "pool2Pwd":
                                            pool2.psw = Convert.ToString(settingsArray[iter].Substring
                                                (settingsArray[iter].IndexOf('=') + 1), CultureInfo.InvariantCulture);
                                            iter++;
                                            break;
                                        case "pool2PostfixIp":
                                            pool2.postfixIp = Convert.ToBoolean(settingsArray[iter].Substring
                                                (settingsArray[iter].IndexOf('=') + 1), CultureInfo.InvariantCulture);
                                            iter++;
                                            break;
                                        case "pool2PostfixNoChange":
                                            pool2.postfixNoChange = Convert.ToBoolean(settingsArray[iter].Substring
                                                (settingsArray[iter].IndexOf('=') + 1), CultureInfo.InvariantCulture);
                                            iter++;
                                            break;
                                        case "pool2PostfixNone":
                                            pool2.postfixNone = Convert.ToBoolean(settingsArray[iter].Substring
                                                (settingsArray[iter].IndexOf('=') + 1), CultureInfo.InvariantCulture);
                                            iter++;
                                            break;
                                        case "pool3Enabled":
                                            pool3.enabled = Convert.ToBoolean(settingsArray[iter].Substring
                                                (settingsArray[iter].IndexOf('=') + 1), CultureInfo.InvariantCulture);
                                            iter++;
                                            break;
                                        case "pool3Url":
                                            pool3.url = Convert.ToString(settingsArray[iter].Substring
                                                (settingsArray[iter].IndexOf('=') + 1), CultureInfo.InvariantCulture);
                                            iter++;
                                            break;
                                        case "pool3Worker":
                                            pool3.worker = Convert.ToString(settingsArray[iter].Substring
                                                (settingsArray[iter].IndexOf('=') + 1), CultureInfo.InvariantCulture);
                                            iter++;
                                            break;
                                        case "pool3Pwd":
                                            pool3.psw = Convert.ToString(settingsArray[iter].Substring
                                                (settingsArray[iter].IndexOf('=') + 1), CultureInfo.InvariantCulture);
                                            iter++;
                                            break;
                                        case "pool3PostfixIp":
                                            pool3.postfixIp = Convert.ToBoolean(settingsArray[iter].Substring
                                                (settingsArray[iter].IndexOf('=') + 1), CultureInfo.InvariantCulture);
                                            iter++;
                                            break;
                                        case "pool3PostfixNoChange":
                                            pool3.postfixNoChange = Convert.ToBoolean(settingsArray[iter].Substring
                                                (settingsArray[iter].IndexOf('=') + 1), CultureInfo.InvariantCulture);
                                            iter++;
                                            break;
                                        case "pool3PostfixNone":
                                            pool3.postfixNone = Convert.ToBoolean(settingsArray[iter].Substring
                                                (settingsArray[iter].IndexOf('=') + 1), CultureInfo.InvariantCulture);
                                            iter++;
                                            break;
                                        case "ipRangeGroups":
                                            ipRangeString=(Convert.ToString(settingsArray[iter].Substring
                                                (settingsArray[iter].IndexOf('=') + 1), CultureInfo.InvariantCulture));
                                            iter++;
                                            break;
                                        default:
                                            iter++;
                                            break;
                                    }
                                }
                                break;
                            case "[scanner]":
                                iter++;
                                while (iter < settingsArray.Length && settingsArray[iter][0] != '[')
                                {
                                    string row = settingsArray[iter].Substring(settingsArray[iter].IndexOf('=') + 1);
                                    switch (settingsArray[iter].Remove(settingsArray[iter].IndexOf('=')))
                                    {
                                        case "sessionTimeout":
                                            WebCalls.connectTimeout = Convert.ToInt32(UtilityFunc.CleanStringOfNonDigits
                                                (settingsArray[iter].Substring(settingsArray[iter].IndexOf('=') + 1)), CultureInfo.InvariantCulture) * 100;
                                            iter++;
                                            break;
                                        case "AntiAutoReboot":
                                            try
                                            {
                                                if (row.Length>1)
                                                antiRebootArray = (settingsArray[iter].Substring(settingsArray[iter].IndexOf('=') + 1))
                                                    .Split(';').Select(s => s.Split(',')).ToDictionary(p => p[0].Trim(), p => bool.Parse(p[1]));
                                                //.Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries);
                                            }
                                            catch (Exception )
                                            {
                                                int t = 35;
                                                t++;
                                            }
                                            //str
                                            iter++;
                                            break;
                                        case "AntiAutoReport":
                                            try
                                            {
                                                if (row.Length > 1)
                                                antiReportArray = settingsArray[iter].Substring(settingsArray[iter].IndexOf('=') + 1)
                                                    .Split(';').Select(s => s.Split(',')).ToDictionary(p => p[0].Trim(), p => bool.Parse(p[1]));
                                            }
                                            catch (Exception )
                                            {
                                                int t = 35;
                                                t++;
                                            }
                                            iter++;
                                            break;
                                        default:
                                            iter++;
                                            break;
                                    }
                                }
                                break;
                            case "[Wallet]":
                                iter++;
                                while (iter < settingsArray.Length && settingsArray[iter][0] != '[')
                                {
                                    switch (settingsArray[iter].Remove(settingsArray[iter].IndexOf('=')))
                                    {
                                        case "Bittrex":
                                            tempArray = settingsArray[iter].Substring(settingsArray[iter].IndexOf('=') + 1)
                                               .Split(new char[] { '\\' }, StringSplitOptions.RemoveEmptyEntries).Select(s => s.ToLowerInvariant()).ToArray(); ;
                                            Wallets.walletInfo.Add("Bittrex", tempArray);
                                            iter++;
                                            break;
                                        case "ETH":
                                            tempArray = settingsArray[iter].Substring(settingsArray[iter].IndexOf('=') + 1)
                                               .Split(new char[] { '\\' }, StringSplitOptions.RemoveEmptyEntries).Select(s => s.ToLowerInvariant()).ToArray(); ;
                                            Wallets.walletInfo.Add("ETH", tempArray);
                                            iter++;
                                            break;
                                        case "LTC":
                                            tempArray = settingsArray[iter].Substring(settingsArray[iter].IndexOf('=') + 1)
                                               .Split(new char[] { '\\' }, StringSplitOptions.RemoveEmptyEntries);
                                            Wallets.walletInfo.Add("LTC", tempArray);
                                            iter++;
                                            break;
                                        case "NiceHash":
                                            tempArray = settingsArray[iter].Substring(settingsArray[iter].IndexOf('=') + 1)
                                               .Split(new char[] { '\\' }, StringSplitOptions.RemoveEmptyEntries);
                                            Wallets.walletInfo.Add("NiceHash", tempArray);
                                            iter++;
                                            break;
                                        case "BTC cold wallet":
                                            tempArray = settingsArray[iter].Substring(settingsArray[iter].IndexOf('=') + 1)
                                                .Split(new char[] { '\\' }, StringSplitOptions.RemoveEmptyEntries);
                                            Wallets.walletInfo.Add("BTC cold wallet", tempArray);
                                            iter++;
                                            break;
                                        default:
                                            iter++;
                                            break;
                                    }
                                }
                                break;
                            case "[configurator]":
                                iter++;
                                while (iter < settingsArray.Length && settingsArray[iter][0] != '[')
                                {
                                    switch (settingsArray[iter].Remove(settingsArray[iter].IndexOf('=')))
                                    {
                                        case "sessionTimeout":
                                            monitoringTimeout = Convert.ToInt32(UtilityFunc.CleanStringOfNonDigits
                                                (settingsArray[iter].Substring(settingsArray[iter].IndexOf('=') + 1)), CultureInfo.InvariantCulture) * 1000;
                                            iter++;
                                            break;
                                        case "workerNameIpParts":
                                            //pool1.enabled = Convert.ToBoolean(settingsArray[iter].Substring(settingsArray[iter].IndexOf('=') + 1));
                                            iter++;
                                            break;
                                        default:
                                            iter++;
                                            break;
                                    }
                                }
                                break;
                            case "[rebooter]":
                                iter++;
                                while (iter < settingsArray.Length && settingsArray[iter][0] != '[')
                                {
                                    switch (settingsArray[iter].Remove(settingsArray[iter].IndexOf('=')))
                                    {
                                        case "sessionTimeout":
                                            badCheckTimeout = Convert.ToInt32(UtilityFunc.CleanStringOfNonDigits
                                                (settingsArray[iter].Substring(settingsArray[iter].IndexOf('=') + 1)), CultureInfo.InvariantCulture) * 1000 * 60;
                                            iter++;
                                            break;
                                        default:
                                            iter++;
                                            break;
                                    }
                                }
                                break;
                            case "[highlight]":
                                iter++;
                                while (iter < settingsArray.Length && settingsArray[iter][0] != '[')
                                {
                                    switch (settingsArray[iter].Remove(settingsArray[iter].IndexOf('=')))
                                    {
                                        case "highlightTemperatureMoreThan":
                                            abnormalTemp = Convert.ToInt32(UtilityFunc.CleanStringOfNonDigits
                                                (settingsArray[iter].Substring(settingsArray[iter].IndexOf('=') + 1)), CultureInfo.InvariantCulture);
                                            iter++;
                                            break;
                                        case "highlightTemperatureLessThan":
                                            minimalTemp = Convert.ToInt32(UtilityFunc.CleanStringOfNonDigits
                                                (settingsArray[iter].Substring(settingsArray[iter].IndexOf('=') + 1)), CultureInfo.InvariantCulture);
                                            iter++;
                                            break;
                                        case "highlightTempState":
                                            if (settingsArray[iter].Substring(settingsArray[iter].IndexOf('=') + 1) != "")
                                            {
                                                detectHighTemp = Convert.ToBoolean(settingsArray[iter].Substring
                                                    (settingsArray[iter].IndexOf('=') + 1), CultureInfo.InvariantCulture);
                                            }
                                            iter++;
                                            break;
                                        case "highlightLowHashrates":
                                            antMinerHashMin = Convert.ToInt32(settingsArray[iter].Substring
                                                (settingsArray[iter].IndexOf('=') + 1), CultureInfo.InvariantCulture);
                                            iter++;
                                            break;
                                        case "badStatusEmail":
                                            Log.emailForBadStatus = Convert.ToString(settingsArray[iter].Substring
                                                (settingsArray[iter].IndexOf('=') + 1), CultureInfo.InvariantCulture).Split(new char[] { '/' }, StringSplitOptions.RemoveEmptyEntries);
                                            iter++;
                                            break;
                                        default:
                                            iter++;
                                            break;
                                    }
                                }
                                break;
                            case "[login]":
                                iter++;
                                while (iter < settingsArray.Length && settingsArray[iter][0] != '[')
                                {
                                    switch (settingsArray[iter].Remove(settingsArray[iter].IndexOf('=')))
                                    {
                                        case "login":
                                            WebCalls.minerLogin = Convert.ToString(settingsArray[iter].Substring
                                                (settingsArray[iter].IndexOf('=') + 1), CultureInfo.InvariantCulture);
                                            if (WebCalls.minerLogin == "")
                                                WebCalls.minerLogin = "root";
                                            iter++;
                                            break;
                                        case "minerPasswords":
                                            WebCalls.minerPass = Convert.ToString(settingsArray[iter].Substring
                                                (settingsArray[iter].IndexOf('=') + 1), CultureInfo.InvariantCulture);
                                            if (WebCalls.minerPass == "")
                                                WebCalls.minerPass = "root";
                                            iter++;
                                            break;
                                        case "FlyMiningLogin":
                                            Log.flyMiningUserName = Convert.ToString(settingsArray[iter].Substring
                                                (settingsArray[iter].IndexOf('=') + 1), CultureInfo.InvariantCulture);
                                            iter++;
                                            break;
                                        case "FlyMiningPasswords":
                                            Log.flyMiningPassword = Convert.ToString(settingsArray[iter].Substring
                                                (settingsArray[iter].IndexOf('=') + 1), CultureInfo.InvariantCulture);
                                            iter++;
                                            break;
                                        default:
                                            iter++;
                                            break;
                                    }
                                }
                                break;
                            case "[feature]":
                                iter++;
                                while (iter < settingsArray.Length && settingsArray[iter][0] != '[')
                                {
                                    switch (settingsArray[iter].Remove(settingsArray[iter].IndexOf('=')))
                                    {
                                        case "openMinerCPWithPassword":
                                            iter++;
                                            break;
                                        case "apiCheckTimeout":
                                            apiCheckTimeout = Convert.ToInt32(settingsArray[iter].Substring
                                                (settingsArray[iter].IndexOf('=') + 1), CultureInfo.InvariantCulture);
                                            iter++;
                                            break;
                                        case "apiCheckStart":
                                            apiCheckStart = Convert.ToInt32(settingsArray[iter].Substring
                                                (settingsArray[iter].IndexOf('=') + 1), CultureInfo.InvariantCulture);
                                            iter++;
                                            break;
                                        case "apiCheckState":
                                            apiCheckState = Convert.ToBoolean(settingsArray[iter].Substring
                                                (settingsArray[iter].IndexOf('=') + 1), CultureInfo.InvariantCulture);
                                            iter++;
                                            break;
                                        case "onStartUp":
                                            onStartUp = Convert.ToBoolean(settingsArray[iter].Substring
                                                (settingsArray[iter].IndexOf('=') + 1), CultureInfo.InvariantCulture);
                                            iter++;
                                            break;
                                        case "autoScan":
                                            autoScan = Convert.ToBoolean(settingsArray[iter].Substring
                                                (settingsArray[iter].IndexOf('=') + 1), CultureInfo.InvariantCulture);
                                            iter++;
                                            break;
                                        case "autoMonitoring":
                                            autoMonitoring = Convert.ToBoolean(settingsArray[iter].Substring
                                                (settingsArray[iter].IndexOf('=') + 1), CultureInfo.InvariantCulture);
                                            iter++;
                                            break;
                                        default:
                                            iter++;
                                            break;
                                    }
                                }
                                break;
                            default:
                                iter++;
                                break;
                        }
                    }
                    else
                    {
                        iter++;
                    }
                }
            }

        }
        //Присваивает настройки из файла настроек
        public static async void readSettings()
        {
            //Считываем настройки
            //
              antiRebootArray = new Dictionary<string, bool>();
      antiReportArray=new Dictionary<string,bool>();
            parseSettings();
            await checkLogin();

        }
        // Сохранение настроек
        public static  void saveSettings(MainWindow window)
        {
           
            if (!File.Exists(MainWindow.pathSetting))
            {
                using (var file = File.Create(MainWindow.pathSetting))
                { }
            }
            string[] writeSettings = new string[100];
            //[ui]
            //
            int iter = 0;
            writeSettings[iter++] = "[ui]";            
            writeSettings[iter++] = "pool1Enabled=" + pool1.enabled.ToString();
            writeSettings[iter++] = "pool2Enabled=" + pool2.enabled.ToString();
            writeSettings[iter++] = "pool3Enabled=" + pool3.enabled.ToString();
            writeSettings[iter++] = "pool1Url=" + pool1.url;
            writeSettings[iter++] = "pool2Url=" + pool2.url;
            writeSettings[iter++] = "pool3Url=" + pool3.url;
            writeSettings[iter++] = "pool1Worker=" + pool1.worker;
            writeSettings[iter++] = "pool2Worker=" + pool2.worker;
            writeSettings[iter++] = "pool3Worker=" + pool3.worker;
            writeSettings[iter++] = "pool1Pwd=" + pool1.psw;
            writeSettings[iter++] = "pool2Pwd=" + pool2.psw;
            writeSettings[iter++] = "pool3Pwd=" + pool3.psw;
            writeSettings[iter++] = "pool1PostfixIp=" + pool1.postfixIp.ToString();
            writeSettings[iter++] = "pool2PostfixIp=" + pool2.postfixIp.ToString();
            writeSettings[iter++] = "pool3PostfixIp=" + pool3.postfixIp.ToString();
            writeSettings[iter++] = "pool1PostfixNoChange=" + pool1.postfixNoChange.ToString();
            writeSettings[iter++] = "pool2PostfixNoChange=" + pool2.postfixNoChange.ToString();
            writeSettings[iter++] = "pool3PostfixNoChange=" + pool3.postfixNoChange.ToString();
            writeSettings[iter++] = "pool1PostfixNone=" + pool1.postfixNone.ToString();
            writeSettings[iter++] = "pool2PostfixNone=" + pool2.postfixNone.ToString();
            writeSettings[iter++] = "pool3PostfixNone=" + pool3.postfixNone.ToString();
            string tempIpRange = "";
            for (int i = 0; i < window.ipRangeBox.Items.Count; i++)
            {
                if (window.ipRangeBox.CheckedItems.Contains(window.ipRangeBox.Items[i]))
                    tempIpRange += window.ipRangeBox.Items[i].ToString() + ",";
                else
                {
                    tempIpRange += "!" + window.ipRangeBox.Items[i].ToString() + ",";
                }
            }
            if (tempIpRange != "")
                writeSettings[iter++] = "ipRangeGroups=" + tempIpRange.Remove(tempIpRange.Length - 1);//Обрезаем запятую
            //ipRangeGroups="LAN:192.168.1.101-192.168.1.220,!192.168.56.0-...255;#:192.168.1.2-..1.100"
            //[scanner]
            iter++;
            writeSettings[iter++] = "[scanner]";
            writeSettings[iter++] = "sessionTimeout=" + (WebCalls.connectTimeout / 100).ToString();
            if (antiRebootArray != null)
            {
                writeSettings[iter++] = "AntiAutoReboot=" + string.Join("; ",
                    antiRebootArray.Select(p => string.Format("{0}, {1}", p.Key, p.Value))); 
            }
            else
                writeSettings[iter++] = "AntiAutoReboot=" + "";
            if (antiReportArray != null)
            {
                writeSettings[iter++] = "AntiAutoReport=" + string.Join("; ", 
                    antiReportArray.Select(p => string.Format("{0}, {1}", p.Key, p.Value))); 
            }
            else
                writeSettings[iter++] = "AntiAutoReport=" + "";
            //[Wallet]
            iter++;
            writeSettings[iter++] = "[Wallet]";


            if (Wallets.walletInfo != null && Wallets.walletInfo.Keys.Contains("Bittrex"))
            {
                string temp = "";
                for (int iterW = 0; iterW < Wallets.walletInfo["Bittrex"].Count(); iterW++)
                {
                    temp += Wallets.walletInfoDescription["Bittrex"][iterW] + ":" + Wallets.walletInfo["Bittrex"][iterW] + "\\";
                }
                writeSettings[iter++] = "Bittrex=" + temp;
            }
            else
                writeSettings[iter++] = "Bittrex=";

            if (Wallets.walletInfo != null && Wallets.walletInfo.Keys.Contains("LTC"))
            {
                string temp = "";
                for (int iterW = 0; iterW < Wallets.walletInfo["LTC"].Count(); iterW++)
                {
                    temp += Wallets.walletInfoDescription["LTC"][iterW] + ":" + Wallets.walletInfo["LTC"][iterW] + "\\";
                }
                writeSettings[iter++] = "LTC=" + temp;
            }
            else
                writeSettings[iter++] = "LTC=";
            if (Wallets.walletInfo != null && Wallets.walletInfo.Keys.Contains("NiceHash"))
            {
                string temp = "";
                for (int iterW = 0; iterW < Wallets.walletInfo["NiceHash"].Count(); iterW++)
                {
                    temp += Wallets.walletInfoDescription["NiceHash"][iterW] + ":" + Wallets.walletInfo["NiceHash"][iterW] + "\\";
                }
                writeSettings[iter++] = "NiceHash=" + temp;
            }
            else
                writeSettings[iter++] = "NiceHash=";
            if (Wallets.walletInfo != null && Wallets.walletInfo.Keys.Contains("ETH"))
            {
                string temp = "";
                for (int iterW = 0; iterW < Wallets.walletInfo["ETH"].Count(); iterW++)
                {
                    temp += Wallets.walletInfoDescription["ETH"][iterW] + ":" + Wallets.walletInfo["ETH"][iterW] + "\\";
                }
                writeSettings[iter++] = "ETH=" + temp;
            }
            else
                writeSettings[iter++] = "ETH=";
            if (Wallets.walletInfo != null && Wallets.walletInfo.Keys.Contains("BTC cold wallet"))
            {
                string temp = "";
                for (int iterW = 0; iterW < Wallets.walletInfo["BTC cold wallet"].Count(); iterW++)
                {
                    temp += Wallets.walletInfoDescription["BTC cold wallet"][iterW] + ":" + Wallets.walletInfo["BTC cold wallet"][iterW] + "\\";
                }
                writeSettings[iter++] = "BTC cold wallet=" + temp;
            }
            else
                writeSettings[iter++] = "BTC cold wallet=";
            //[configurator]
            iter++;
            writeSettings[iter++] = "[configurator]";
            writeSettings[iter++] = "sessionTimeout=" + (monitoringTimeout / 1000).ToString();
            writeSettings[iter++] = "workerNameIpParts=" + "";

            //[rebooter]
            iter++;
            writeSettings[iter++] = "[rebooter]";
            writeSettings[iter++] = "sessionTimeout=" + (badCheckTimeout / 60000).ToString();

            //[highlight]

            iter++;
            writeSettings[iter++] = "[highlight]";
            writeSettings[iter++] = "highlightTemperatureMoreThan=" + abnormalTemp.ToString();
            writeSettings[iter++] = "highlightTemperatureLessThan=" + minimalTemp.ToString();
            writeSettings[iter++] = "highlightTempState=" + detectHighTemp.ToString();
            writeSettings[iter++] = "highlightLowHashrates=" + antMinerHashMin.ToString();
            try
            {
                if (Log.emailForBadStatus != null || Log.emailForBadStatus.Length > 0)
                    writeSettings[iter++] = "badStatusEmail=" + String.Join("/", Log.emailForBadStatus);
            }
            catch (Exception ex)
            {
                writeSettings[iter++] = "badStatusEmail=";
                Log.logDebug("emailForBadStatus Error\n\r" + Convert.ToString(ex));
            }

            //isHighlightTemperature=true
            //highlightTemperatureMoreThan=90
            //highlightTemperatureLessThan=0
            //isHighlightWrongWorkerName=true
            //isHighlightLowHashrate=true
            //highlightLowHashrates=10001

            //[login]
            iter++;
            writeSettings[iter++] = "[login]";
            writeSettings[iter++] = "login=" + WebCalls.minerLogin;
            writeSettings[iter++] = "minerPasswords=" + WebCalls.minerPass;
            writeSettings[iter++] = "FlyMiningLogin=" + Log.flyMiningUserName;
            writeSettings[iter++] = "FlyMiningPasswords=" + Log.flyMiningPassword;
            //minerPasswords="QW50bWluZXI=:cm9vdA==:cm9vdA==&QXZhbG9u:cm9vdA==:"

            //[feature]
            iter++;
            writeSettings[iter++] = "[feature]";
            writeSettings[iter++] = "openMinerCPWithPassword=" + "";
            writeSettings[iter++] = "apiCheckTimeout=" + apiCheckTimeout.ToString();
            writeSettings[iter++] = "apiCheckStart=" + apiCheckStart.ToString();
            writeSettings[iter++] = "apiCheckState=" + apiCheckState.ToString();
            writeSettings[iter++] = "onStartUp=" + onStartUp.ToString();
            writeSettings[iter++] = "autoScan=" + autoScan.ToString();
            writeSettings[iter++] = "autoMonitoring=" + autoMonitoring.ToString();

            //openMinerCPWithPassword=true
            File.WriteAllLines(MainWindow.pathSetting, writeSettings, Encoding.Unicode);

        }

        #endregion
    }
}
