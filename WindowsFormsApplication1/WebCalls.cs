using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Web.Script.Serialization;

namespace BitcoinInfoMiner
{
    static class WebCalls
    {
        const string defaultErrorState = "Failed to connect";
        const string noConnectErrorState = "No conncection";
        public static string minerLogin = "root";
        public static string minerPass = "root";
        public static int connectTimeout = 10;
        public static async Task<string> sendsocketJson2(string command, string arg = "", string host = "localhost", int port = 4028)
        {
            Socket socketJson = new Socket(
            AddressFamily.InterNetwork,
            SocketType.Stream,
            ProtocolType.Tcp);
            try
            {
                socketJson.ReceiveTimeout = connectTimeout;
                socketJson.SendTimeout = connectTimeout;
                TcpClient client = new TcpClient();
                await client.ConnectAsync(host, port); // соединение
                NetworkStream networkStream = client.GetStream();
                StreamWriter writer = new StreamWriter(networkStream);
                StreamReader reader = new StreamReader(networkStream);
                writer.AutoFlush = true;
                string json = "";
                if (arg != "")
                    json = new JavaScriptSerializer().Serialize(new
                    {
                        command = command,
                        parameter = arg
                    });
                else
                {
                    json = new JavaScriptSerializer().Serialize(new
                    {
                        command = command
                    });
                }
                byte[] msg = Encoding.UTF8.GetBytes(json);
                byte[] bytes = new byte[300];
                await writer.WriteLineAsync(json);
                string response = await reader.ReadLineAsync();
                client.Close();
                return Convert.ToString(response, CultureInfo.InvariantCulture);
            }
            catch (Exception e)
            {
                Log.logDebug(Convert.ToString(e, CultureInfo.InvariantCulture));
                return Convert.ToString(e, CultureInfo.InvariantCulture);
            }
        }
        //Отправка HttpcLient запроса через Request

        //Отправка Get запроса через Request
        public static async Task<string> sendJson(string command, string arg = "", string host = "localhost", int port = 4028)
        {
            
            var webRequest = (WebRequest)WebRequest.Create("http://" + minerLogin + ":" + minerPass + "@" + host + ":" + Convert.ToString(port, CultureInfo.InvariantCulture));
            webRequest.ContentType = "application/json";
            webRequest.Method = WebRequestMethods.Http.Post;
            try
            {
                using (var streamWriter = new StreamWriter(await webRequest.GetRequestStreamAsync()))
                {
                    string json = "";
                    if (arg != "")
                        json = new JavaScriptSerializer().Serialize(new
                        {
                            command = command,
                            parameter = arg
                        });
                    else
                    {
                        json = new JavaScriptSerializer().Serialize(new
                        {
                            command = command
                        });
                    }
                    streamWriter.Write(json);
                    streamWriter.Flush();
                    streamWriter.Close();
                }

                var httpResponse = await webRequest.GetResponseAsync();
                string result = "";
                using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                {
                    result = streamReader.ReadToEnd();
                }
                return result;
            }
            catch (Exception e)
            {
                Log.logDebug("SendJson " + Convert.ToString(e, CultureInfo.InvariantCulture));
                return Convert.ToString(e, CultureInfo.InvariantCulture);
            }
        }
        //Отправка Get запроса через Socket
        public static string sendsocketJson(string command, string arg = "", string host = "localhost", int port = 4028)
        {
            
            string result = "";
            Socket socketJson = new Socket(
            AddressFamily.InterNetwork,
            SocketType.Stream,
            ProtocolType.Tcp);
            try
            {
                socketJson.ReceiveTimeout = connectTimeout;
                socketJson.SendTimeout = connectTimeout;


                //ConnectAsync(host, port, socketJson);
                //await socketJson.ConnectAsync(temp);
                //await socketJson.ConnectAsync(host, port);
                IAsyncResult resultConnect = socketJson.BeginConnect(host, port, null, null);
                bool Success = resultConnect.AsyncWaitHandle.WaitOne(2000, true);//connectTimeout
                if (socketJson.Connected)
                {
                    socketJson.EndConnect(resultConnect);
                }
                else
                {
                    socketJson.Close();
                    throw new ApplicationException("Failed to connect server.");
                }

                string json = "";
                if (arg != "")
                    json = new JavaScriptSerializer().Serialize(new
                    {
                        command = command,
                        parameter = arg
                    });
                else
                {
                    json = new JavaScriptSerializer().Serialize(new
                    {
                        command = command
                    });
                }
                byte[] msg = Encoding.UTF8.GetBytes(json);
                byte[] bytes = new byte[300];

                //await Task.Factory.FromAsync<int>(
                //    socketJson.BeginSend(msg, 0, msg.Length, SocketFlags.None, null, socketJson),
                //    socketJson.EndSend);
                socketJson.Send(msg);

                //result += "End :" + Convert.ToString(0);  
                socketJson.Receive(bytes);


                result += Encoding.ASCII.GetString(bytes);
                int i = 0;
                while (i < 100)
                {
                    i++;
                    socketJson.Receive(bytes);

                    if (bytes != null && bytes.Any(b => b != 0))
                    {
                        result += Encoding.ASCII.GetString(bytes);
                        bytes = new byte[100];
                    }
                    else
                    {
                        break;
                    }
                }
                socketJson.Disconnect(false);


                return Convert.ToString(result, CultureInfo.InvariantCulture);
            }
            catch (Exception e)
            {
                //Log.logDebug("SendSocketJson "+command+ "  "+Convert.ToString(e, CultureInfo.InvariantCulture));
                return Convert.ToString(e, CultureInfo.InvariantCulture);
            }

        }


        //public static async Task<MinerModel> parseHttmlData(string ip)
        //{

        //    string responseString = "";
        //    try
        //    {
        //        var url = "http://" + WebCalls.minerLogin + ":" + WebCalls.minerPass + "@" + ip + "/cgi-bin/minerStatus.cgi";
        //        HttpClientHandler handler = new HttpClientHandler();
        //        handler.Credentials = new System.Net.NetworkCredential(WebCalls.minerLogin, WebCalls.minerPass);
        //        HttpClient client = new HttpClient(handler);
        //        client.Timeout = new TimeSpan(0, 0, Convert.ToInt32(WebCalls.connectTimeout / 100));
        //        var byteArray = Encoding.ASCII.GetBytes(WebCalls.minerLogin + ":" + WebCalls.minerPass);
        //        client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Basic", Convert.ToBase64String(byteArray));
        //        //client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Authorization", "Digest username=\"root\", realm=\"antMiner Configuration\",uri=\"/cgi-bin/minerStatus.cgi\"");
        //        client.DefaultRequestHeaders.Add("Accept", "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,image/apng,*/*;q=0.8");
        //        var response = await client.GetAsync(url);
        //        responseString = await response.Content.ReadAsStringAsync();
        //    }
        //    catch (System.Net.Http.HttpRequestException ex)
        //    {
        //        MinerModel temp = new MinerModel();

        //        temp.status = noConnectErrorState;
        //        return temp;
        //    }
        //    catch (Exception ex)
        //    {
        //        if (ex.Message == "A task was canceled." || ex.Message == "Отменена задача.")
        //        {

        //        }
        //        else
        //        {
        //            Log.logDebug("parseHttmlData : " + Convert.ToString(ex));
        //        }
        //        MinerModel temp = new MinerModel();
        //        temp.status = defaultErrorState;
        //        return temp;
        //    }
        //    //string logDebugPath = Application.StartupPath + "\\new.txt";
        //    //responseString = File.ReadAllText(logDebugPath);
        //    string[] arrayData = responseString.Split(new char[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);
        //    MinerModel result = new MinerModel();
        //    result.ip = ip;
        //    result.type = "";
        //    try
        //    {
        //        for (int iter = 0; iter < arrayData.Length; iter++)
        //        {
        //            if (arrayData[iter].Contains("<div id=\"ant_elapsed\">"))
        //            {
        //                result.elapsed = arrayData[iter].Substring(arrayData[iter].IndexOf(">") + 1);
        //                result.elapsed = result.elapsed.Remove(result.elapsed.IndexOf("<"));
        //            }
        //            if (arrayData[iter].Contains("<div id=\"ant_ghs5s\">"))
        //            {
        //                result.hashRateRT = arrayData[iter].Substring(arrayData[iter].IndexOf(">") + 1);
        //                result.hashRateRT = result.hashRateRT.Remove(result.hashRateRT.IndexOf("<"));
        //            }
        //            if (arrayData[iter].Contains("<div id=\"ant_ghsav\">"))
        //            {
        //                result.hashRateAverage = arrayData[iter].Substring(arrayData[iter].IndexOf(">") + 1);
        //                result.hashRateAverage = result.hashRateAverage.Remove(result.hashRateAverage.IndexOf("<"));
        //            }
        //            if (arrayData[iter].Contains("<div id=\"cbi-table-1-url\">"))
        //            {
        //                if (result.pool1 == null || result.pool1 == "")
        //                {
        //                    result.pool1 = arrayData[iter].Substring(arrayData[iter].IndexOf(">") + 1);
        //                    result.pool1 = result.pool1.Remove(result.pool1.IndexOf("<"));
        //                }
        //                else
        //                {
        //                    if (result.pool2 == null || result.pool2 == "")
        //                    {
        //                        result.pool2 = arrayData[iter].Substring(arrayData[iter].IndexOf(">") + 1);
        //                        result.pool2 = result.pool2.Remove(result.pool2.IndexOf("<"));
        //                    }
        //                    else
        //                    {
        //                        if (result.pool3 == null || result.pool3 == "")
        //                        {
        //                            result.pool3 = arrayData[iter].Substring(arrayData[iter].IndexOf(">") + 1);
        //                            result.pool3 = result.pool3.Remove(result.pool3.IndexOf("<"));
        //                        }
        //                    }
        //                }
        //            }
        //            if (arrayData[iter].Contains("<div id=\"cbi-table-1-user\">"))
        //            {
        //                if (result.worker1 == null || result.worker1 == "")
        //                {
        //                    result.worker1 = arrayData[iter].Substring(arrayData[iter].IndexOf(">") + 1);
        //                    result.worker1 = result.worker1.Remove(result.worker1.IndexOf("<"));
        //                }
        //                else
        //                {
        //                    if (result.worker2 == null || result.worker2 == "")
        //                    {
        //                        result.worker2 = arrayData[iter].Substring(arrayData[iter].IndexOf(">") + 1);
        //                        result.worker2 = result.worker2.Remove(result.worker2.IndexOf("<"));
        //                    }
        //                    else
        //                    {
        //                        if (result.worker3 == null || result.worker3 == "")
        //                        {
        //                            result.worker3 = arrayData[iter].Substring(arrayData[iter].IndexOf(">") + 1);
        //                            result.worker3 = result.worker3.Remove(result.worker3.IndexOf("<"));
        //                        }
        //                    }
        //                }
        //            }
        //            //<td id="ant_fan
        //            if (arrayData[iter].Contains("<div id=\"cbi-table-1-temp2\">"))
        //            {
        //                string temp = arrayData[iter].Substring(arrayData[iter].IndexOf(">") + 1);
        //                temp = temp.Remove(temp.IndexOf("<"));
        //                if (temp != "")
        //                {
        //                    if (temp == "-")
        //                        temp = "0";
        //                }
        //            }
        //            if (arrayData[iter].Contains("<td id=\"ant_fan"))
        //            {
        //                string temp = arrayData[iter].Substring(arrayData[iter].IndexOf(">") + 1);
        //                temp = temp.Remove(temp.IndexOf("<"));
        //                if (temp != "0")
        //                {
        //                    if (temp == "-")
        //                        temp = "0";
        //                }
        //            }
        //            //if (arrayData[iter].Contains("<div id=\"cbi-table-1-rate\">"))
        //            //{
        //            //    result.elapsed = arrayData[iter].Substring(arrayData[iter].IndexOf(">" + 1));
        //            //    result.elapsed = result.elapsed.Remove(result.elapsed.IndexOf("<"));
        //            //}
        //            //if (arrayData[iter].Contains("<div id=\"cbi-table-1-rate2\">"))
        //            //{
        //            //    result.elapsed = arrayData[iter].Substring(arrayData[iter].IndexOf(">" + 1));
        //            //    result.elapsed = result.elapsed.Remove(result.elapsed.IndexOf("<"));
        //            //}
        //        }



        //        if (result.elapsed != null && result.elapsed != "")
        //        {
        //            string time = "";
        //            if (result.elapsed.IndexOf("d") > 0)
        //            {
        //                time = result.elapsed.Remove(result.elapsed.IndexOf("d"));
        //                if (time.Length == 1)
        //                    time = time.Insert(0, "0");
        //                time += ":";
        //                result.elapsed = result.elapsed.Substring(result.elapsed.IndexOf("d") + 1);
        //            }
        //            else
        //            {
        //                time = "00:";
        //            }
        //            if (result.elapsed.IndexOf("h") > 0)
        //            {
        //                string temptime = "";
        //                temptime = result.elapsed.Remove(result.elapsed.IndexOf("h"));
        //                if (temptime.Length == 1)
        //                    temptime = temptime.Insert(0, "0");
        //                temptime += ":";
        //                time += temptime;
        //                result.elapsed = result.elapsed.Substring(result.elapsed.IndexOf("h") + 1);
        //            }
        //            else
        //            {
        //                time += "00:";
        //            }
        //            if (result.elapsed.IndexOf("m") > 0)
        //            {
        //                string temptime = "";
        //                temptime = result.elapsed.Remove(result.elapsed.IndexOf("m"));
        //                if (temptime.Length == 1)
        //                    temptime = temptime.Insert(0, "0");
        //                temptime += ":";
        //                time += temptime;
        //            }
        //            else
        //            {
        //                time += "00:";
        //            }
        //            result.elapsed = time + "00";
        //        }
        //        else
        //            result.elapsed = "00:00:00:00";

        //        if (result.hashRateAverage != null && result.hashRateAverage != "")
        //        {
        //            if (result.hashRateAverage.IndexOf('.') > 0)
        //                result.hashRateAverage = result.hashRateAverage.Remove(result.hashRateAverage.IndexOf('.'));
        //            result.hashRateAverage = result.hashRateAverage.Replace(",", "");
        //        }
        //        else
        //            result.hashRateAverage = "0";

        //        if (result.hashRateRT != null && result.hashRateRT != "")
        //        {
        //            if (result.hashRateRT.IndexOf('.') > 0)
        //                result.hashRateRT = result.hashRateRT.Remove(result.hashRateRT.IndexOf('.'));
        //            result.hashRateRT = result.hashRateRT.Replace(",", "");
        //        }
        //        else
        //            result.hashRateRT = "0";
        //        result.status = "Success";
        //    }
        //    catch (Exception exception)
        //    {
        //        Log.logDebug("ParseHtml Parsing error:" + Convert.ToString(exception));
        //        MinerModel temp = new MinerModel();
        //        temp.status = defaultErrorState;
        //        return temp;
        //    }
        //    return result;

        //}
        public static async Task<MinerModel> parseHttmlDataOnlyPools(string ip, MinerModel result)
        {

            string responseString = "";
            try
            {
                var url = "http://" + WebCalls.minerLogin + ":" + WebCalls.minerPass + "@" + ip + "/cgi-bin/minerConfiguration.cgi";
                HttpClientHandler handler = new HttpClientHandler();
                handler.Credentials = new System.Net.NetworkCredential(WebCalls.minerLogin, WebCalls.minerPass);
                HttpClient client = new HttpClient(handler);
                client.Timeout = new TimeSpan(0, 0, Convert.ToInt32(WebCalls.connectTimeout / 100));
                var byteArray = Encoding.ASCII.GetBytes(WebCalls.minerLogin + ":" + WebCalls.minerPass);
                client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Basic", Convert.ToBase64String(byteArray));
                //client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Authorization", "Digest username=\"root\", realm=\"antMiner Configuration\",uri=\"/cgi-bin/minerStatus.cgi\"");
                client.DefaultRequestHeaders.Add("Accept", "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,image/apng,*/*;q=0.8");
                var response = await client.GetAsync(url);
                responseString = await response.Content.ReadAsStringAsync();
            }
            catch (System.Net.Http.HttpRequestException )
            {
                MinerModel temp = new MinerModel();

                temp.status = noConnectErrorState;
                return temp;
            }
            catch (Exception ex)
            {
                if (ex.Message == "A task was canceled." || ex.Message == "Отменена задача.")
                {

                    //Log.logDebug("parseHttmlData increse connection Timeout :" + connectTimeout);
                }
                else
                {
                    Log.logDebug("parseHttmlData : " + Convert.ToString(ex));
                }
                return result;
            }

            string[] arrayData = responseString.Split(new char[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);
            result.ip = ip;

            //for (int iter = 0; iter < arrayData.Length; iter++)
            //{
            //    if (arrayData[iter].Contains("<div id=\"cbi-table-1-url\">"))
            //    {
            //        if (result.pool1 == null || result.pool1 == "")
            //        {
            //            result.pool1 = arrayData[iter].Substring(arrayData[iter].IndexOf(">") + 1);
            //            result.pool1 = result.pool1.Remove(result.pool1.IndexOf("<"));
            //        }
            //        else
            //        {
            //            if (result.pool2 == null || result.pool2 == "")
            //            {
            //                result.pool2 = arrayData[iter].Substring(arrayData[iter].IndexOf(">") + 1);
            //                result.pool2 = result.pool2.Remove(result.pool2.IndexOf("<"));
            //            }
            //            else
            //            {
            //                if (result.pool3 == null || result.pool3 == "")
            //                {
            //                    result.pool3 = arrayData[iter].Substring(arrayData[iter].IndexOf(">") + 1);
            //                    result.pool3 = result.pool3.Remove(result.pool3.IndexOf("<"));
            //                }
            //            }
            //        }
            //    }
            //    if (arrayData[iter].Contains("<div id=\"cbi-table-1-user\">"))
            //    {
            //        if (result.worker1 == null || result.worker1 == "")
            //        {
            //            result.worker1 = arrayData[iter].Substring(arrayData[iter].IndexOf(">") + 1);
            //            result.worker1 = result.worker1.Remove(result.worker1.IndexOf("<"));
            //        }
            //        else
            //        {
            //            if (result.worker2 == null || result.worker2 == "")
            //            {
            //                result.worker2 = arrayData[iter].Substring(arrayData[iter].IndexOf(">") + 1);
            //                result.worker2 = result.worker2.Remove(result.worker2.IndexOf("<"));
            //            }
            //            else
            //            {
            //                if (result.worker3 == null || result.worker3 == "")
            //                {
            //                    result.worker3 = arrayData[iter].Substring(arrayData[iter].IndexOf(">") + 1);
            //                    result.worker3 = result.worker3.Remove(result.worker3.IndexOf("<"));
            //                }
            //            }
            //        }
            //    }
            //}
            for (int iter = 0; iter < arrayData.Length; iter++)
            {
                if (arrayData[iter].Contains("\"url\" : \""))
                {
                    if (result.pool1 == null || result.pool1 == "")
                    {
                        result.pool1 = arrayData[iter].Substring(arrayData[iter].IndexOf("\"url\" : \"") + 9);
                        result.pool1 = result.pool1.Remove(result.pool1.IndexOf("\""));
                    }
                    else
                    {
                        if (result.pool2 == null || result.pool2 == "")
                        {
                            result.pool2 = arrayData[iter].Substring(arrayData[iter].IndexOf("\"url\" : \"") + 9);
                            result.pool2 = result.pool2.Remove(result.pool2.IndexOf("\""));
                        }
                        else
                        {
                            if (result.pool3 == null || result.pool3 == "")
                            {
                                result.pool3 = arrayData[iter].Substring(arrayData[iter].IndexOf("\"url\" : \"") + 9);
                                result.pool3 = result.pool3.Remove(result.pool3.IndexOf("\""));
                            }
                        }
                    }
                }
                if (arrayData[iter].Contains("\"user\" : \""))
                {
                    if (result.worker1 == null || result.worker1 == "")
                    {
                        result.worker1 = arrayData[iter].Substring(arrayData[iter].IndexOf("\"user\" : \"") + 10);
                        result.worker1 = result.worker1.Remove(result.worker1.IndexOf("\""));
                    }
                    else
                    {
                        if (result.worker2 == null || result.worker2 == "")
                        {
                            result.worker2 = arrayData[iter].Substring(arrayData[iter].IndexOf("\"user\" : \"") + 10);
                            result.worker2 = result.worker2.Remove(result.worker2.IndexOf("\""));
                        }
                        else
                        {
                            if (result.worker3 == null || result.worker3 == "")
                            {
                                result.worker3 = arrayData[iter].Substring(arrayData[iter].IndexOf("\"user\" : \"") + 10);
                                result.worker3 = result.worker3.Remove(result.worker3.IndexOf("\""));
                            }
                        }
                    }
                }
            }
            return result;

        }
    }
}
