using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace BitcoinInfoMiner
{
    static class MinerFunc
    {

        public static  async Task<bool> rebootMiner(string urlMiner)
        {

            try
            {

                System.Net.ServicePointManager.Expect100Continue = false;
                var url = "http://" + WebCalls.minerLogin + ":" + WebCalls.minerPass + "@" + urlMiner + "/cgi-bin/reboot.cgi";
                HttpClientHandler handler = new HttpClientHandler();
                handler.Credentials = new System.Net.NetworkCredential(WebCalls.minerLogin, WebCalls.minerPass);
                HttpClient client = new HttpClient(handler);
                client.Timeout = new TimeSpan(0, 1, 0);
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Authorization", "Digest username=\"" + WebCalls.minerLogin + "\", realm=\"antMiner Configuration\",uri=\"/cgi-bin/set_network_conf.cgi\"");
                client.DefaultRequestHeaders.Add("Accept", "application/json, text/javascript, */*; q=0.01");
                client.DefaultRequestHeaders.ExpectContinue = false;
                var response = await client.GetAsync(url);
                var responseString = await response.Content.ReadAsStringAsync();
                Log.logDebug(" rebootManuallyMiner " + responseString);
                return true;
            }
            catch (HttpRequestException exception)
            {

                return false;
            }
            catch (TaskCanceledException exception)
            {

                return false;
            }
            catch (Exception exception)
            {
                Log.logDebug("Reboot Miner " + Convert.ToString(exception, CultureInfo.InvariantCulture));
                return false;
            }

        }
    }
}
