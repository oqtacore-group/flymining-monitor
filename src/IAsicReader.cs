using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace BitcoinInfoMiner
{
    public class AntminerStats
    {
        public string rate_5s { get; set; }
        public string rate_ideal { get; set; }
        public string rate_avg { get; set; }
        public List<string> fan { get; set; }
        public List<AntminerChain> chain { get; set; }

    }

    public class AntminerChain
    {
        public string rate_real { get; set; }
        public string asic_num { get; set; } // Number of Chips
        public string sn { get; set; } // serial number
        public List<string> temp_chip { get; set; } // temp of chips
        public List<string> temp_pcb { get; set; } // temp of board
        public List<string> temp_pic { get; set; } // temp of controllers
    }

    public class jsonAntminerStatuses
    {
        public List<AntminerStats> stats { get; set; }
    }

    public class jsonAvalonStatuses
    {
        public string elapsed { get; set; }
        public string av { get; set; }
        public string Url { get; set; }
        public string Worker { get; set; }
        public string Accepted { get; set; }
        public string Reject { get; set; }
        public string Temperature { get; set; }
        public string TemperatureF { get; set; }
        public string Fan { get; set; }
        public string Fanr { get; set; }
        public string hash_5m { get; set; }
        public string sys_status { get; set; }
        public string MinerStatus { get; set; }
        public string RejectedPercentage { get; set; }
        public string Ping { get; set; }
        public string fan1 { get; set; }
        public string fan2 { get; set; }
        public string fan3 { get; set; }
        public string fan4 { get; set; }
        public string MTavg1 { get; set; }
        public string MTavg2 { get; set; }
        public string MTavg3 { get; set; }
        public string Mtavg1f { get; set; }
        public string Mtavg2f { get; set; }
        public string Mtavg3f { get; set; }
        public string Hashboard { get; set; }
    }

    public class AvalonPools
    {
        public string pool1 { get; set; }
        public string worker1 { get; set; }
        public string pool2 { get; set; }
        public string worker2 { get; set; }
        public string pool3 { get; set; }
        public string worker3 { get; set; }
    }

    public class AvalonInfo
    {
        public string version { get; set; }
        public string mac { get; set; }
        public string hwtype { get; set; }

    }

    public interface IAsicReader
    {
        Task<string> getAsicModelIfSupported(string ip);
        Task<object> getPoolsData(string ip);
        Task<object> getNetworkData(string ip);
        Task<object> getStatsData(string ip);
        Task<jsonMinerStatus> getSummaryData(string ip);
        List<string> supportedModels();

    }

    public class AntminerAsicReader : IAsicReader
    {
        private static string poolsPath = "/cgi-bin/pools.cgi";
        private static string statsPath = "/cgi-bin/stats.cgi";
        private static string summaryPath = "/cgi-bin/summary.cgi";
        private static string networkPath = "/cgi-bin/get_network_info.cgi";

        private static AntminerAsicReader _antminerAsicReader = null;

        private static HttpClient client = new HttpClient();

        public async Task<string> getAsicModelIfSupported(string ip)
        {
            var url = "http://" + WebCalls.minerLogin + ":" + WebCalls.minerPass + "@" + ip + poolsPath;
            HttpClientHandler handler = new HttpClientHandler();
            handler.Credentials = new System.Net.NetworkCredential(WebCalls.minerLogin, WebCalls.minerPass);
            client = new HttpClient(handler);
            client.Timeout = new TimeSpan(0, 0, 10);
            var byteArray = Encoding.ASCII.GetBytes(WebCalls.minerLogin + ":" + WebCalls.minerPass);
            var response = await client.GetAsync(url).ConfigureAwait(false);
            string text = await response.Content.ReadAsStringAsync().ConfigureAwait(false);

            if (text.Length > 0)
            {

                return "Antminer";
            }

            return null;
        }

        public async Task<object> getNetworkData(string ip)
        {
            try
            {
                var url = "http://" + WebCalls.minerLogin + ":" + WebCalls.minerPass + "@" + ip + networkPath;
                HttpClientHandler handler = new HttpClientHandler();
                handler.Credentials = new System.Net.NetworkCredential(WebCalls.minerLogin, WebCalls.minerPass);
                client = new HttpClient(handler);
                client.Timeout = new TimeSpan(0, 0, 10);
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

        public async Task<object> getStatsData(string ip)
        {
            try
            {
                var url = "http://" + WebCalls.minerLogin + ":" + WebCalls.minerPass + "@" + ip + statsPath;
                HttpClientHandler handler = new HttpClientHandler();
                handler.Credentials = new System.Net.NetworkCredential(WebCalls.minerLogin, WebCalls.minerPass);
                client = new HttpClient(handler);
                client.Timeout = new TimeSpan(0, 0, 10);
                var byteArray = Encoding.ASCII.GetBytes(WebCalls.minerLogin + ":" + WebCalls.minerPass);
                var response = await client.GetAsync(url).ConfigureAwait(false);
                string text = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                jsonAntminerStatuses result = JsonConvert.DeserializeObject<jsonAntminerStatuses>(text);

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
                Log.logDebug("getStatsyData" + Convert.ToString(ex));
                return null;
            }
        }

        public async Task<object> getPoolsData(string ip)
        {
            try
            {
                var url = "http://" + WebCalls.minerLogin + ":" + WebCalls.minerPass + "@" + ip + poolsPath;
                HttpClientHandler handler = new HttpClientHandler();
                handler.Credentials = new System.Net.NetworkCredential(WebCalls.minerLogin, WebCalls.minerPass);
                client = new HttpClient(handler);
                client.Timeout = new TimeSpan(0, 0, 10);
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
            catch (Exception ex)
            {
                Log.logDebug("getStatusData" + Convert.ToString(ex));
                return null;
            }
        }

        public async Task<jsonMinerStatus> getSummaryData(string ip)
        {
            try
            {
                var url = "http://" + WebCalls.minerLogin + ":" + WebCalls.minerPass + "@" + ip + summaryPath;
                HttpClientHandler handler = new HttpClientHandler();
                handler.Credentials = new System.Net.NetworkCredential(WebCalls.minerLogin, WebCalls.minerPass);
                using HttpClient client = new HttpClient(handler)
                {
                    Timeout = new TimeSpan(0, 0, 10)
                };
                var byteArray = Encoding.ASCII.GetBytes($"{WebCalls.minerLogin}:{WebCalls.minerPass}");
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
            catch (Exception ex)
            {
                Log.logDebug("getSummaryData" + Convert.ToString(ex));
                return null;
            }
        }

        public List<string> supportedModels()
        {
            var list = new List<string>();
            list.Add("Antminer");

            return list;
        }
    }

    public class AvalonAsicReader : IAsicReader
    {
        private static AvalonAsicReader _avalonAsicReader = null;

        private static string poolsPath = "/updatecgconf.cgi";
        private static string statsPath = "/get_home.cgi";
        //private static string summaryPath = "/summary.cgi";
        private static string networkPath = "/get_minerinfo.cgi";

        private static HttpClient client = new HttpClient();

        public async Task<string> getAsicModelIfSupported(string ip)
        {
            var url = "http://" + WebCalls.minerLogin + ":" + WebCalls.minerPass + "@" + ip + poolsPath;
            HttpClientHandler handler = new HttpClientHandler();
            handler.Credentials = new System.Net.NetworkCredential(WebCalls.minerLogin, WebCalls.minerPass);
            client = new HttpClient(handler);
            client.Timeout = new TimeSpan(0, 0, 10);
            var byteArray = Encoding.ASCII.GetBytes(WebCalls.minerLogin + ":" + WebCalls.minerPass);
            var response = await client.GetAsync(url).ConfigureAwait(false);
            string text = await response.Content.ReadAsStringAsync().ConfigureAwait(false);

            if (text.Length > 0)
            {

                return "Avalon";
            }

            return null;
        }

        public async Task<object> getNetworkData(string ip)
        {
            try
            {
                var url = "http://" + WebCalls.minerLogin + ":" + WebCalls.minerPass + "@" + ip + networkPath;
                HttpClientHandler handler = new HttpClientHandler();
                handler.Credentials = new System.Net.NetworkCredential(WebCalls.minerLogin, WebCalls.minerPass);
                client = new HttpClient(handler);
                client.Timeout = new TimeSpan(0, 0, 10);
                var byteArray = Encoding.ASCII.GetBytes(WebCalls.minerLogin + ":" + WebCalls.minerPass);
                var response = await client.GetAsync(url).ConfigureAwait(false);
                string text = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                string pattern = @"minerinfoCallback\((.*)\)\s*;";
                Match match = Regex.Match(text, pattern, RegexOptions.Singleline);
                if (match.Success)
                {
                    text = match.Groups[1].Value;
                }
                AvalonInfo result = JsonConvert.DeserializeObject<AvalonInfo>(text);

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

        public async Task<object> getPoolsData(string ip)
        {
            try
            {
                var url = "http://" + WebCalls.minerLogin + ":" + WebCalls.minerPass + "@" + ip + poolsPath;
                HttpClientHandler handler = new HttpClientHandler();
                handler.Credentials = new System.Net.NetworkCredential(WebCalls.minerLogin, WebCalls.minerPass);
                client = new HttpClient(handler);
                client.Timeout = new TimeSpan(0, 0, 10);
                var byteArray = Encoding.ASCII.GetBytes(WebCalls.minerLogin + ":" + WebCalls.minerPass);
                var response = await client.GetAsync(url).ConfigureAwait(false);
                string text = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                string pattern = @"CGConfCallback\((.*)\)\s*;";
                Match match = Regex.Match(text, pattern, RegexOptions.Singleline);
                if (match.Success)
                {
                    text = match.Groups[1].Value;
                }
                AvalonPools result = JsonConvert.DeserializeObject<AvalonPools>(text);

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
                Log.logDebug("getStatusData" + Convert.ToString(ex));
                return null;
            }
        }

        public async Task<object> getStatsData(string ip)
        {
            try
            {
                var url = "http://" + WebCalls.minerLogin + ":" + WebCalls.minerPass + "@" + ip + statsPath;
                HttpClientHandler handler = new HttpClientHandler();
                handler.Credentials = new System.Net.NetworkCredential(WebCalls.minerLogin, WebCalls.minerPass);
                client = new HttpClient(handler);
                client.Timeout = new TimeSpan(0, 0, 10);
                var byteArray = Encoding.ASCII.GetBytes(WebCalls.minerLogin + ":" + WebCalls.minerPass);
                var response = await client.GetAsync(url).ConfigureAwait(false);
                var text = await response.Content.ReadAsStringAsync().ConfigureAwait(false);

                string pattern = @"homeCallback\((.*?)\);";
                Match match = Regex.Match(text, pattern, RegexOptions.Singleline);
                if (match.Success)
                {
                    text = match.Groups[1].Value;
                }
                else
                {
                    throw new Exception("No match found for homeCallback pattern.");
                }

                var result = JsonConvert.DeserializeObject<jsonAvalonStatuses>(text);
                
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
                Log.logDebug("getStatsByData" + Convert.ToString(ex));
                return null;
            }
        }

        public async Task<jsonMinerStatus> getSummaryData(string ip)
        {
            try
            {
                var url = "http://" + WebCalls.minerLogin + ":" + WebCalls.minerPass + "@" + ip;
                HttpClientHandler handler = new HttpClientHandler();
                handler.Credentials = new System.Net.NetworkCredential(WebCalls.minerLogin, WebCalls.minerPass);
                using HttpClient client = new HttpClient(handler)
                {
                    Timeout = new TimeSpan(0, 0, 10)
                };
                var byteArray = Encoding.ASCII.GetBytes($"{WebCalls.minerLogin}:{WebCalls.minerPass}");
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
            catch (Exception ex)
            {
                Log.logDebug("getSummaryData" + Convert.ToString(ex));
                return null;
            }
        }

        public List<string> supportedModels()
        {
            var list = new List<string>();
            list.Add("Avalon");

            return list;
        }
    }

    public class AsicReaderManager
    {
        List <IAsicReader> asicReaders;
        Dictionary <string, IAsicReader> modelReaders = new Dictionary<string, IAsicReader>();

        public async Task<object> getPoolsData(string ip, string model)
        {
            return await modelReaders[model].getPoolsData(ip);
        }
        public async Task<object> getNetworkData(string ip, string model)
        {
            return await modelReaders[model].getNetworkData(ip);
        }
        public async Task<object> getStatsData(string ip, string model)
        {
            return await modelReaders[model].getStatsData(ip);
        }
        public async Task<jsonMinerStatus> getSummaryData(string ip, string model)
        {
            return await modelReaders[model].getSummaryData(ip);
        }

        private static AsicReaderManager instance = null;
        private AsicReaderManager() 
        {
            asicReaders = new List<IAsicReader>();

            RegisterAsicReader(new AntminerAsicReader(), "Antminer");
            RegisterAsicReader(new AvalonAsicReader(), "Avalon");
        }

        private void RegisterAsicReader(IAsicReader reader, string model)
        {
            asicReaders.Add(reader);
            modelReaders.Add(model, reader);
        }

        public static AsicReaderManager Instance() 
        { 
            if(instance == null)
            {  
                instance = new AsicReaderManager();
            }

            return instance;
        }

        public async Task<string> getAsicModel(string ip)
        {
            foreach (var reader in asicReaders)
            {
                var model = await reader.getAsicModelIfSupported(ip);
                if (model != null)
                {
                    return model;
                }

            }

            return null;
        }
        
    }
}
