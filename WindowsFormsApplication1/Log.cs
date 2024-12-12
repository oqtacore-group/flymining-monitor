using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Windows.Forms;
using System.Globalization;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Mail;
using System.Diagnostics;
namespace BitcoinInfoMiner
{
    class Log
    {
        public class OperationResult
        {
            public OperationResult(List<string> ipList)
            {
                totalCount = ipList.Count;
                ipResultList = new Dictionary<string, string>();
                foreach(string ip in ipList)
                {
                    ipResultList.Add(ip, "Do something");
                }
            }
            public int totalCount { get; set; }
            public Dictionary<string, string> ipResultList { get; set; }
            public int succesCount
            {
                get
                {
                    if (totalCount > 0)
                        return ipResultList.Where(t => t.Value == "").Count();
                    else
                        return 0;
                }

            }
            public int errorCount
            {
                get
                {
                    if (totalCount > 0)
                        return ipResultList.Where(t => t.Value != "").Count();
                    else
                        return 0;
                }
            }
            public string errorString()
            {
                string result = "";
                foreach (String key in ipResultList.Keys)
                {
                    result += key + ":" + ipResultList[key] + "\n\r";
                }
                return result;
            }
            public void addError(string ip, string msg)
            {
                if (ipResultList.Keys.Contains(ip))
                    ipResultList[ip] = msg;
            }
            public void addSuccess(string ip)
            {
                if (ipResultList.Keys.Contains(ip))
                    ipResultList[ip] = "";
            }
            public void report(string command)
            {
                if (errorCount > 0)
                {
                    Log.createLogOperation(command, errorString());
                }
            }

        }
        public static readonly string logDebugPath = Application.StartupPath + "\\logDebug\\logDebug.txt";
        public static readonly string logReportPath = Application.StartupPath + "\\logReport\\logReport.txt";
        public static readonly string logReportDir = Application.StartupPath + "\\logReport\\";
        public static readonly string log2ReportPath = Application.StartupPath + "\\logReport\\logReport2.txt";
        public static readonly string logArchivePath = Application.StartupPath + "\\logReport\\Archive\\";
        public static readonly string logArchivePathDebug = Application.StartupPath + "\\logDebug\\Archive\\";
        public class ipSituationClass
        {
            public ipSituationClass(string msg)
            {               
                this.msg = msg;
                time = DateTime.UtcNow;
            }           
            string msg { get; set; }
            public DateTime time { get; set; }
            public string toStringLine()
            {
                return  time.ToString(CultureInfo.InvariantCulture) + ";" + msg;
            }
        }

        public static Dictionary<string,ipSituationClass> ipSituation = new Dictionary<string,ipSituationClass>();
        public static string flyMiningUserName="";
        public static string flyMiningPassword="";
        public static OperationResult configReport;
        public static OperationResult rebootReport;
        


        public static  string[] emailForBadStatus;//Эмейлы на который будут отправлятся отчеты
        public static void logDebug(string msg)
        {
            try
            {
                if (!msg.Contains("\n\r"))
                    msg += "\n\r";
                File.AppendAllText(logDebugPath, msg, Encoding.Unicode);
            }
            catch { }
        }
        public static void logDebugTest(string msg)
        {
            try
            {
                File.AppendAllText( Application.StartupPath + "\\logDebug\\logDebug2.txt", msg, Encoding.Unicode);
            }
            catch { }
        }
        public static void logReport(string msg, MinerModel model)
        {
            try
            {
                string ip = model.ip;
                if (!msg.Contains("\n\r"))
                    msg += "\n\r";
                if (!ipSituation.Keys.Contains(ip))
                    ipSituation.Add(ip, new ipSituationClass(msg));
                else
                    ipSituation[ip] = new ipSituationClass(msg);

                msg = DateTime.Now.ToString() + " Problem with " + ip + ":" + msg;
                File.AppendAllText(logReportPath, msg, Encoding.Unicode);
            }
            catch (Exception e)
            {
                logDebug("LogReport " + Convert.ToString(e, CultureInfo.InvariantCulture));
            }
        }
        //Archiving log
        public static void logArchive()
        {
            try
            {
                if (new FileInfo(logReportPath).Length != 0)
                {
                    parseReportFile();
                    string archivePath = logArchivePath + "logArchive" + DateTime.Now.ToString("dd.MM.yyyy") + ".txt";
                    string archiveDebug = logArchivePathDebug + "logArchiveDebug" + DateTime.Now.ToString("dd.MM.yyyy") + ".txt";
                    if (File.Exists(archivePath))
                    {
                        File.AppendAllText(archivePath, File.ReadAllText(logReportPath), Encoding.Unicode);
                    }
                    else
                    {
                        File.Move(logReportPath, archivePath);
                    }
                    if (File.Exists(archiveDebug))
                    {
                        File.AppendAllText(archiveDebug, File.ReadAllText(logDebugPath), Encoding.Unicode);
                    }
                    else
                    {
                        File.Move(logDebugPath, archiveDebug);
                    }
                    using (var file = File.Create(logReportPath)) { }
                    using (var file = File.Create(logDebugPath)) { }
                    
                }
            }
            catch (Exception e)
            {
                logDebug("LogArchive " + Convert.ToString(e, CultureInfo.InvariantCulture));
            }
        }
        public static async void logArchive(string parsedString)
        {
            try
            {
                if (parsedString != "")
                {
                    await sendEmail(parsedString);
                    string archivePath = logArchivePath + "logArchive" + DateTime.Now.ToString("dd.MM.yyyy") + ".txt";
                    string archiveDebug = logArchivePathDebug + "logArchiveDebug" + DateTime.Now.ToString("dd.MM.yyyy") + ".txt";
                    if (File.Exists(archivePath))
                    {
                        File.AppendAllText(archivePath, File.ReadAllText(logReportPath), Encoding.Unicode);
                    }
                    else
                    {
                        File.Move(logReportPath, archivePath);
                    }
                    if (File.Exists(archiveDebug))
                    {
                        File.AppendAllText(archiveDebug, File.ReadAllText(logDebugPath), Encoding.Unicode);
                    }
                    else
                    {
                        File.Move(logDebugPath, archiveDebug);
                    }
                    using (var file = File.Create(logReportPath)) { }
                    using (var file = File.Create(logDebugPath)) { }
                }
            }
            catch (Exception e)
            {
                logDebug("LogArchive " + Convert.ToString(e, CultureInfo.InvariantCulture));
            }

        }
        public static void logJustArchive()
        {
            try
            {
                //sendEmail(parsedString);
                string archivePath = logArchivePath + "logArchive" + DateTime.Now.ToString("dd.MM.yyyy") + ".txt";
                string archiveDebug = logArchivePathDebug + "logArchiveDebug" + DateTime.Now.ToString("dd.MM.yyyy") + ".txt";
                if (File.Exists(archivePath))
                {
                    File.AppendAllText(archivePath, File.ReadAllText(logReportPath), Encoding.Unicode);
                }
                else
                {
                    File.Move(logReportPath, archivePath);
                }
                if (File.Exists(archiveDebug))
                {
                    File.AppendAllText(archiveDebug, File.ReadAllText(logDebugPath), Encoding.Unicode);
                }
                else
                {
                    File.Move(logDebugPath, archiveDebug);
                }
                using (var file = File.Create(logReportPath)) { }
                using (var file = File.Create(logDebugPath)) { }
            }
            catch (Exception e)
            {
                logDebug("LogJustArhive " + Convert.ToString(e, CultureInfo.InvariantCulture));
            }

        }
        public static void  createLogOperation(string name, string text)
        {
            if (text != "")
            {
                string logFilePath = Application.StartupPath + "\\logReport\\" + name + DateTime.Now.ToString("HH.mm.ss.dd.MM.yyyy") + ".txt";

                while (File.Exists(logFilePath))
                {
                    logFilePath += ".0";
                }
                try
                {
                    using (var file = File.Create(logFilePath)) { }

                    File.AppendAllText(logFilePath, text, Encoding.Unicode);
                    Process.Start(logFilePath);
                    DialogResult resultWindow = MessageBox.Show("Error log for operation " + name
                          , "Result", MessageBoxButtons.OK);

                }
                catch (Exception ex)
                {
                    logDebug("createLogOperation tex:" + name + ":\n\r" + text + "\n\rError:" + Convert.ToString(ex));
                    createLogOperation(name, text);
                }
            }
            
        }
        public static async Task sendEmail(string msg)
        {
            if (Settings.logged)
            {
                try
                {
                    for (int iter = 0; iter < emailForBadStatus.Length; iter++)
                    {
                        using (var client = new HttpClient())
                        {
                            var values = new Dictionary<string, string>
                        {
                        { "msg", msg}
                        };
                            var content = new FormUrlEncodedContent(values);
                            try
                            {
                                var response = client.PostAsync(Settings.siteuser+"Mining//SendReportmailPost?id=" + Log.flyMiningUserName + "&key="
                                    + Log.flyMiningPassword + "&email=" + emailForBadStatus[iter], content).Result;
                                var responseString = await response.Content.ReadAsStringAsync();
                                if (responseString.Contains("Error"))
                                    iter--;

                            }
                            catch (Exception ex)
                            {
                                Log.logDebug("Send email error : " + Convert.ToString(ex));
                            }
                        }
                    }
                }
                catch (Exception e)
                {
                    logDebug("Send Mail " + Convert.ToString(e, CultureInfo.InvariantCulture));
                }
            }

        }
        public static void parseReportFile()
        {
            string text;
            var fileStream = new FileStream(logReportPath, FileMode.Open,
            FileAccess.Read); //open text file
            //vvv read text file (or however you implement it like here vvv
            using (var streamReader = new StreamReader(fileStream, Encoding.UTF8))
            {
                text = streamReader.ReadToEnd();
            }
            //finally, close text file
            fileStream.Close();
            string[] parseArray = text.Split(new char[] { '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries);
            Dictionary<String, int> parsedReport = new Dictionary<string, int>();
            for (int iter = 0; iter < parseArray.Length; iter++)
            {
                if (parseArray[iter].IndexOf("Problem with") > 0)
                {
                    string parsedString = parseArray[iter].Substring(parseArray[iter].IndexOf("Problem with"));
                    if (!parsedReport.Keys.Contains(parsedString))
                    {
                        parsedReport[parsedString] = 0;
                    }
                    else
                    {
                        parsedReport[parsedString]++;
                    }
                }
            }
            //if (!File.Exists(logReportPath+".txt"))
            //    File.Create(logReportPath + ".txt");
            StringBuilder builder = new StringBuilder();
            foreach (KeyValuePair<string, int> pair in parsedReport)
            {
                if (pair.Value >= 5)
                {
                    string parsedStart = "", parsedEnd = "";
                    for (int iter = 0; iter < parseArray.Length; iter++)
                    {
                        if (parseArray[iter].Contains(pair.Key) && parsedStart == "")
                        {
                            parsedStart = parseArray[iter].Remove(parseArray[iter].IndexOf(pair.Key));
                        }
                        else
                        {
                            if (parseArray[iter].Contains(pair.Key))
                            {
                                parsedEnd = parseArray[iter].Remove(parseArray[iter].IndexOf(pair.Key));
                            }
                        }
                    }
                    builder.Append(pair.Key).Append(":").Append('\n').Append(parsedStart).Append(" - ").Append(parsedEnd).Append('\n');
                }
            }
            string result = builder.ToString();
            try
            {
                File.WriteAllText(logReportPath, result, Encoding.Unicode);
            }
            catch (Exception e)
            {
                logDebug("WriteParseReport " + Convert.ToString(e, CultureInfo.InvariantCulture));
            }

        }
    }
}
