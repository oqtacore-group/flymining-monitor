using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace BitcoinInfoMiner
{
    public class Stats
    {
        public string rate_5s { get; set; }
        public string rate_ideal { get; set; }
        public string rate_avg { get; set; }
        public List<string> fan { get; set; }
        public List<Chain> chain { get; set; }

    }

    public class Chain
    {
        public string rate_real { get; set; }
        public string asic_num { get; set; } // Number of Chips
        public string sn { get; set; } // serial number
        public List<string> temp_chip { get; set; } // temp of chips
        public List<string> temp_pcb { get; set; } // temp of board
        public List<string> temp_pic { get; set; } // temp of controllers
    }

    public class jsonMinerStatuses
    {
        public List<Stats> stats { get; set; }
    }

    public interface IAsicReader
    {
        Task <string> getAsicModelIfSupported(string ip);
        Task<jsonMinerStatus> getPoolsData(string ip);
        Task<jsonMinerNetworkStatus> getNetworkData(string ip);
        Task<jsonMinerStatuses> getStatsData(string ip);
        Task<jsonMinerStatus> getSummaryData(string ip);
        List<string> supportedModels();

    }

    //public class AvalonAsicReader : IAsicReader
    //{
    //    private static AvalonAsicReader _avalonAsicReader = null;

    //    private static string kernelPath = "/cgi-bin/get_kernel_log.cgi";
    //    private static string poolsPath = "/cgi-bin/pools.cgi";
    //    private static string statsPath = "/cgi-bin/stats.cgi";
    //    private static string summaryPath = "/cgi-bin/summary.cgi";
    //    private static string networkPath = "/cgi-bin/get_network_info.cgi";

    //    private static HttpClient client = new HttpClient();

    //    public async Task<string> getAsicModelIfSupported(string ip)
    //    {
    //        var url = "http://" + WebCalls.minerLogin + ":" + WebCalls.minerPass + "@" + ip + poolsPath;
    //        HttpClientHandler handler = new HttpClientHandler();
    //        handler.Credentials = new System.Net.NetworkCredential(WebCalls.minerLogin, WebCalls.minerPass);
    //        client = new HttpClient(handler);
    //        client.Timeout = new TimeSpan(0, 0, 10);
    //        var byteArray = Encoding.ASCII.GetBytes(WebCalls.minerLogin + ":" + WebCalls.minerPass);
    //        var response = await client.GetAsync(url).ConfigureAwait(false);

    //        if (response.IsSuccessStatusCode)
    //        {

    //            return "Avalon";
    //        }

    //        return null;
    //    }
    //}


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

            if (response.IsSuccessStatusCode)
            {

                return "Antminer";
            }

            return null;
        }

        public async Task<jsonMinerNetworkStatus> getNetworkData(string ip)
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

        public async Task<jsonMinerStatuses> getStatsData(string ip)
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
                jsonMinerStatuses result = JsonConvert.DeserializeObject<jsonMinerStatuses>(text);

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

        public async Task<jsonMinerStatus> getPoolsData(string ip)
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

    public class AsicReaderManager
    {
        List <IAsicReader> asicReaders;
        Dictionary <string, IAsicReader> modelReaders = new Dictionary<string, IAsicReader>();
   

        public async Task<jsonMinerStatus> getPoolsData(string ip, string model)
        {
            return await modelReaders[model].getPoolsData(ip);
        }
        public async Task<jsonMinerNetworkStatus> getNetworkData(string ip, string model)
        {
            return await modelReaders[model].getNetworkData(ip);
        }
        public async Task<jsonMinerStatuses> getStatsData(string ip, string model)
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
            var antminerReader = new AntminerAsicReader();
            asicReaders = new List<IAsicReader>();
            //asicReaders.Add(new AvalonAsicReader());
            asicReaders.Add(antminerReader);
            modelReaders.Add("Antminer", antminerReader);
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
            foreach (var reader in  asicReaders)
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
