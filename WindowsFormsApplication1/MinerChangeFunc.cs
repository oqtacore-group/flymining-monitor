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
    static class MinerChangeFunc
    {

        public static async Task<int> configMinerSql(MinerModel model)
        {
            try
            {
                System.Net.ServicePointManager.Expect100Continue = false;
                string ip = model.ip;
                try
                {
                    Settings.minerErrorState[ip] = Settings.configErrorState;
                }
                catch { }
              
                var values = new Dictionary<string, string>();



                values.Add("_ant_pool1url", model.pool1);
                values.Add("_ant_pool1user", model.worker1);
                values.Add("_ant_pool1pw", "123");

                values.Add("_ant_pool2url", model.pool2);
                values.Add("_ant_pool2user", model.worker2);
                values.Add("_ant_pool2pw", "123");

                values.Add("_ant_pool3url", model.pool3);
                values.Add("_ant_pool3user", model.worker3);
                values.Add("_ant_pool3pw", "123");

                values.Add("_ant_nobeeper", "false");

                values.Add("_ant_notempoverctrl", "false");
                values.Add("_ant_fan_customize_switch", "false");
                values.Add("_ant_fan_customize_value", "");
                values.Add("_ant_freq", "");
                values.Add("_ant_voltage", "0706");

                var url = "http://" + WebCalls.minerLogin + ":" + WebCalls.minerPass + "@" + ip + "/cgi-bin/set_miner_conf.cgi";
                HttpClientHandler handler = new HttpClientHandler();
                handler.Credentials = new System.Net.NetworkCredential(WebCalls.minerLogin, WebCalls.minerPass);
                HttpClient client = new HttpClient(handler);
                client.DefaultRequestHeaders.ExpectContinue = false;
                client.Timeout = new TimeSpan(0, 0, 50);               
                var content = new FormUrlEncodedContent(values);
                var response =  client.PostAsync(url, content).Result ;
                var responseString = await response.Content.ReadAsStringAsync() ;

              
                return 1;
            }
            catch (Exception exception)
            {
                Log.logDebug("configMiner" +Convert.ToString(exception));
                return 0;
            }
        }
        public static async Task configMiner(string ip, int id)
        {
            try
            {
                System.Net.ServicePointManager.Expect100Continue = false;
                int ipEnd = Convert.ToInt32(ip.Substring(ip.LastIndexOf(".") + 1));
                try
                {
                    Settings.minerErrorState[ip] = Settings.configErrorState;
                }
                catch { }
                MinerModel model = new MinerModel();
                model.ip = ip;
                var values = new Dictionary<string, string>();
                if (Settings.pool1.enabled)
                {
                    values.Add("_ant_pool1url", Settings.pool1.url);

                    if (Settings.pool1.postfixIp)
                    {
                        values.Add("_ant_pool1user", Settings.pool1.worker + "." + ipEnd.ToString("000"));
                    }
                    else
                        if (Settings.pool1.postfixNone)
                        {
                            values.Add("_ant_pool1user", Settings.pool1.worker);
                        }
                        else
                        {
                            values.Add("_ant_pool1user", Settings.pool1.worker + "." + id.ToString("000"));
                        }
                    values.Add("_ant_pool1pw", Settings.pool1.psw);
                    model.pool1 = values["_ant_pool1url"];
                    model.worker1 = values["_ant_pool1user"];
                }
                if (Settings.pool2.enabled)
                {
                    values.Add("_ant_pool2url", Settings.pool2.url);

                    if (Settings.pool2.postfixIp)
                    {
                        values.Add("_ant_pool2user", Settings.pool2.worker + "." + ipEnd.ToString("000"));
                    }
                    else
                        if (Settings.pool2.postfixNone)
                        {
                            values.Add("_ant_pool2user", Settings.pool2.worker);
                        }
                        else
                        {
                            values.Add("_ant_pool2user", Settings.pool2.worker + "." + id.ToString("000"));
                        }
                    values.Add("_ant_pool2pw", Settings.pool2.psw);
                    model.pool2 = values["_ant_pool2url"];
                    model.worker2 = values["_ant_pool2user"];
                }
                if (Settings.pool3.enabled)
                {
                    values.Add("_ant_pool3url", Settings.pool3.url);

                    if (Settings.pool3.postfixIp)
                    {
                        values.Add("_ant_pool3user", Settings.pool3.worker + "." + ipEnd.ToString("000"));
                    }
                    else
                        if (Settings.pool3.postfixNone)
                        {
                            values.Add("_ant_pool3user", Settings.pool3.worker);
                        }
                        else
                        {
                            values.Add("_ant_pool3user", Settings.pool3.worker + "." + id.ToString("000"));
                        }
                    values.Add("_ant_pool3pw", Settings.pool3.psw);
                    model.pool3 = values["_ant_pool3url"];
                    model.worker3 = values["_ant_pool3user"];
                }
                values.Add("_ant_notempoverctrl", "false" );
                values.Add("_ant_fan_customize_switch", "false" );
                values.Add("_ant_fan_customize_value", "");
                var url = "http://" + WebCalls.minerLogin + ":" + WebCalls.minerPass + "@" + ip + "/cgi-bin/set_miner_conf.cgi";
                HttpClientHandler handler = new HttpClientHandler();
                handler.Credentials = new System.Net.NetworkCredential(WebCalls.minerLogin, WebCalls.minerPass);
                HttpClient client = new HttpClient(handler);
                client.Timeout = new TimeSpan(0, 0, 50);
                client.DefaultRequestHeaders.ExpectContinue = false;
                var content = new FormUrlEncodedContent(values);
                var response = client.PostAsync(url, content).Result;
                var responseString = await response.Content.ReadAsStringAsync();
                Log.logDebug(" config " + responseString);





                //Log.logDebug(url+  " ConfigMiner " + string.Join(";", values.Select(x => x.Key + "=" + x.Value).ToArray()));
                Log.configReport.addSuccess(ip);
                Sql.updateDbMinerPoolStatus(model);
                return ;
            }
            catch (TaskCanceledException exception)
            {
                Log.logDebug("ConfigMiner Task Canceled" + Convert.ToString(exception, CultureInfo.InvariantCulture));
                if (exception.InnerException != null && exception.InnerException.Message != null)
                    Log.configReport.addError(ip, exception.InnerException.Message);
                else
                    Log.configReport.addError(ip, Convert.ToString(exception));
                
               
            }
            catch (Exception exception)
            {

                Log.logDebug("ConfigMiner " + Convert.ToString(exception, CultureInfo.InvariantCulture));
                if (exception.InnerException != null && exception.InnerException.Message != null)
                    Log.configReport.addError(ip, exception.InnerException.Message);
                else
                    Log.configReport.addError(ip, Convert.ToString(exception));

                return;
            }
           
        }
        //Rebooting miner with autoreboot
        public static async Task<bool> rebootMiner(string urlMiner)
        {


            try
            {
                Settings.minerErrorState[urlMiner] = Settings.rebootErrorState;
            }
            catch { }

            
            return await MinerFunc.rebootMiner(urlMiner);


        }

        //Reboot using button
        public static async Task<bool> rebootManuallyMiner(string urlMiner)
        {

            try
            {

                try
                {
                    Settings.minerErrorState[urlMiner] = Settings.rebootErrorState;
                }
                catch { }
                

               
                var url = "http://" + WebCalls.minerLogin + ":" + WebCalls.minerPass + "@" + urlMiner + "/cgi-bin/reboot.cgi";
                HttpClientHandler handler = new HttpClientHandler();
                handler.Credentials = new System.Net.NetworkCredential(WebCalls.minerLogin, WebCalls.minerPass);
                HttpClient client = new HttpClient(handler);
                client.Timeout = new TimeSpan(0, 1, 0);
                System.Net.ServicePointManager.Expect100Continue = false;
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Authorization", "Digest username=\"" + WebCalls.minerLogin + "\", realm=\"antMiner Configuration\",uri=\"/cgi-bin/set_network_conf.cgi\"");
                client.DefaultRequestHeaders.Add("Accept", "application/json, text/javascript, */*; q=0.01");
                client.DefaultRequestHeaders.ExpectContinue = false;
                var response = client.GetAsync(url).Result;
             
                var responseString = await response.Content.ReadAsStringAsync();
                //Log.logDebug(" rebootManuallyMiner " + responseString);
                Log.rebootReport.addSuccess(urlMiner);
                return true;
            }
            catch (HttpRequestException exception)
            {
                Log.rebootReport.addError(urlMiner, exception.InnerException.Message);
                return false;
            }
            catch (TaskCanceledException exception)
            {
                Log.rebootReport.addError(urlMiner, exception.InnerException.Message);
                return false;
            }
            catch (Exception exception)
            {
                //Log.logDebug("Reboot Miner "+Convert.ToString(exception, CultureInfo.InvariantCulture));
                Log.logDebug("Reboot Miner " + Convert.ToString(exception, CultureInfo.InvariantCulture));
                Log.rebootReport.addError(urlMiner, Convert.ToString(exception, CultureInfo.InvariantCulture));
                return false;
            }

        }
       
    }
}
