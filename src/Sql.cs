using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BitcoinInfoMiner
{
    static class Sql
    {
        //private static List<MinerModel> _minersList;
        //private static object _listLock = new object();
        public static List<MinerModel> minersList = new List<MinerModel>();
        public static readonly Object insertLock =new Object();
        //
        //string.Format(@"SELECT 1 AS result FROM NetworkTableHistory WHERE Mac = '{0}';",model.Mac); 
        // string.Format(@"SELECT * FROM NetworkTableHistory WHERE Mac = '{0}' and Pool1='{1}'and Pool1='{2}'and Pool1='{3}'and Pool1='{4}';",""); 

        //string.Format(@"SELECT 1 AS result FROM NetworkTableHistory WHERE Mac = '{0}';",model.Mac); 
        private static string sqlconnectString = "URI=file:FlyMiningDB.sqlite";
        private static int executeCommand(string command)
        {
            using (SQLiteConnection con = new SQLiteConnection(sqlconnectString))
            {
                con.Open();
                SQLiteCommand cmd = con.CreateCommand();
                cmd.CommandText = command;

                try
                {
                    int result = cmd.ExecuteNonQuery();
                  
                    return result;
                }
                catch (Exception ex)
                {
                    Log.logDebug(Convert.ToString(ex));
                   
                    return 0;
                }
            }

        }
        public static string columnOrderQuery = @"AutoReboot,AutoReport,IP,Status,LastStatusChange,Mac,
Type,HashRateRT,HashRateAvg,Temperature1,Temperature2,Temperature3,
FanSpeed1,FanSpeed2,Elapsed,Pool1,Pool2,Pool3,Worker1,Worker2,Worker3";

        /// <summary>
        /// Parse reader and return MinerModel
        /// </summary>
        /// <param name="rdr">Sqlite reader</param>
        /// <returns></returns>
        private static MinerModel parseDbReaderForMiner(DbDataReader rdr)
        {
            if (rdr.Read())
            {
                try
                {
                    MinerModel row = new MinerModel()
                    {
                        autoreboot = rdr["AutoReboot"] != null ? Convert.ToBoolean(rdr["AutoReboot"]) : true,
                        autoreport = rdr["AutoReport"] != null ? Convert.ToBoolean(rdr["AutoReport"]) : true,
                        ip = rdr["IP"] != null ? rdr["IP"].ToString() : "",
                        status = rdr["Status"] != null ? rdr["Status"].ToString() : "",
                        lastStatusChangeDate = rdr["LastStatusChange"] != null ? Convert.ToDateTime(rdr["LastStatusChange"]) : DateTime.UtcNow,
                        Mac = rdr["Mac"] != null ? rdr["Mac"].ToString() : "",
                        type = rdr["Type"] != null ? rdr["Type"].ToString() : "",
                        hashRateRT = rdr["HashRateRT"] != null ? Convert.ToInt32(rdr["HashRateRT"]) : 0,
                        hashRateAverage = rdr["HashRateAvg"] != null ? Convert.ToInt32(rdr["HashRateAvg"]) : 0,
                        temperature1 = rdr["Temperature1"] != null ? Convert.ToInt32(rdr["Temperature1"]) : 0,
                        temperature2 = rdr["Temperature2"] != null ? Convert.ToInt32(rdr["Temperature2"]) : 0,
                        temperature3 = rdr["Temperature3"] != null ? Convert.ToInt32(rdr["Temperature3"]) : 0,
                        fanSpeed1 = rdr["FanSpeed1"] != null ? Convert.ToInt32(rdr["FanSpeed1"]) : 0,
                        fanSpeed2 = rdr["FanSpeed2"] != null ? Convert.ToInt32(rdr["FanSpeed2"]) : 0,
                        elapsed = rdr["Elapsed"] != null ? rdr["Elapsed"].ToString() : "",
                        pool1 = rdr["Pool1"] != null ? rdr["Pool1"].ToString() : "",
                        pool2 = rdr["Pool2"] != null ? rdr["Pool2"].ToString() : "",
                        pool3 = rdr["Pool3"] != null ? rdr["Pool3"].ToString() : "",
                        worker1 = rdr["Worker1"] != null ? rdr["Worker1"].ToString() : "",
                        worker2 = rdr["Worker2"] != null ? rdr["Worker2"].ToString() : "",
                        worker3 = rdr["Worker3"] != null ? rdr["Worker3"].ToString() : "",
                        conf_dnsservers = rdr["conf_dnsservers"] != null ? rdr["conf_dnsservers"].ToString() : "",
                        conf_gateway = rdr["conf_gateway"] != null ? rdr["conf_gateway"].ToString() : "",
                        conf_hostname = rdr["conf_hostname"] != null ? rdr["conf_hostname"].ToString() : "",
                        conf_netmask = rdr["conf_netmask"] != null ? rdr["conf_netmask"].ToString() : "",
                        conf_nettype = rdr["conf_nettype"] != null ? rdr["conf_nettype"].ToString() : ""
                    };
                    return row;
                }
                catch (Exception ex)
                {
                    return new MinerModel();
                }

            }
            else
                return null;
        }



        private static MinerModel parseDbReaderForPartialMiner(DbDataReader rdr)
        {
            if (rdr.Read())
            {
                try
                {
                    MinerModel row = new MinerModel()
                    {

                        ip = rdr["IP"] != null ? rdr["IP"].ToString() : "",
                        Mac = rdr["Mac"] != null ? rdr["Mac"].ToString() : "",
                        pool1 = rdr["Pool1"] != null ? rdr["Pool1"].ToString() : "",
                        pool2 = rdr["Pool2"] != null ? rdr["Pool2"].ToString() : "",
                        pool3 = rdr["Pool3"] != null ? rdr["Pool3"].ToString() : "",
                        worker1 = rdr["Worker1"] != null ? rdr["Worker1"].ToString() : "",
                        worker2 = rdr["Worker2"] != null ? rdr["Worker2"].ToString() : "",
                        worker3 = rdr["Worker3"] != null ? rdr["Worker3"].ToString() : "",
                        conf_dnsservers = rdr["conf_dnsservers"] != null ? rdr["conf_dnsservers"].ToString() : "",
                        conf_gateway = rdr["conf_gateway"] != null ? rdr["conf_gateway"].ToString() : "",
                        conf_hostname = rdr["conf_hostname"] != null ? rdr["conf_hostname"].ToString() : "",
                        conf_netmask = rdr["conf_netmask"] != null ? rdr["conf_netmask"].ToString() : "",
                        conf_nettype = rdr["conf_nettype"] != null ? rdr["conf_nettype"].ToString() : ""
                    };
                    return row;
                }
                catch (Exception ex)
                {
                    return new MinerModel();
                }

            }
            else
                return null;
        }


        /// <summary>
        /// Read result from query.And parses it
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        public async static Task<List<MinerModel>> executeCommandAndReadMinerModel(string command)
        {
            List<MinerModel> result = new List<MinerModel>();
            using (SQLiteConnection con = new SQLiteConnection(sqlconnectString))
            {
                con.Open();
                using (SQLiteCommand cmd = new SQLiteCommand(command, con))
                {
                    using (DbDataReader rdr = await cmd.ExecuteReaderAsync())
                    {

                        result.Add(parseDbReaderForMiner(rdr));

                    }
                  
                    return result;
                }
            }
        }



        public async static Task<MinerModel> executeCommandAndReadPartialMinerModel(string command)
        {
            List<MinerModel> result = new List<MinerModel>();
            using (SQLiteConnection con = new SQLiteConnection(sqlconnectString))
            {
                con.Open();
                using (SQLiteCommand cmd = new SQLiteCommand(command, con))
                {
                    using (DbDataReader rdr = await cmd.ExecuteReaderAsync())
                    {

                        result.Add(parseDbReaderForPartialMiner(rdr));

                    }
                  
                    return result.First();
                }
            }
        }


        /// <summary>
        /// Check if database contains row with such mac
        /// </summary>
        /// <param name="Mac">Miner mac-address</param>
        /// <returns></returns>
        public static bool containsMac(string Mac)
        {
            bool result = false;
            using (SQLiteConnection con = new SQLiteConnection(sqlconnectString))
            {
                con.Open();
                string command = string.Format(@"SELECT 1  FROM NetworkTableHistory WHERE Mac = '{0}';", Mac);
                using (SQLiteCommand cmd = new SQLiteCommand(command, con))
                {
                    using (DbDataReader rdr = cmd.ExecuteReader())
                    {
                        if (rdr.Read() && rdr.HasRows)
                            result = rdr.GetBoolean(0);

                    }
               
                    return result;
                }
            }
        }


        /// <summary>
        /// Check if database contains row with such mac
        /// </summary>
        /// <param name="Mac">Miner mac-address</param>
        /// <returns></returns>
        public static bool containsMacSave(string Mac)
        {
            bool result = false;
            using (SQLiteConnection con = new SQLiteConnection(sqlconnectString))
            {
                con.Open();
                string command = string.Format(@"SELECT 1  FROM SaveLoadTableHistory WHERE Mac = '{0}';", Mac);
                using (SQLiteCommand cmd = new SQLiteCommand(command, con))
                {
                    using (DbDataReader rdr = cmd.ExecuteReader())
                    {
                        if (rdr.Read() && rdr.HasRows)
                            result = rdr.GetBoolean(0);

                    }
              
                    return result;
                }
            }
        }



        /// <summary>
        /// Check if database contains row with such mac and pool settings.Should be used after mac check
        /// </summary>
        /// <param name="Mac">Miner model</param>
        /// <returns></returns>
        public static bool correctPoolState(MinerModel model)
        {
            bool result = false;
            using (SQLiteConnection con = new SQLiteConnection(sqlconnectString))
            {
                con.Open();
                string command = string.Format(@"SELECT 1 FROM NetworkTableHistory WHERE Mac like '%{0}%' and Worker1 like '%{1}%'and Worker2 like '%{2}%'and Worker3 like '%{3}%'
and Pool1 like '%{4}%'and Pool2 like '%{5}%'and Pool3 like '%{6}%';", model.Mac, model.worker1, model.worker2, model.worker3, model.pool1, model.pool2, model.pool3);
                using (SQLiteCommand cmd = new SQLiteCommand(command, con))
                {
                    using (DbDataReader rdr = cmd.ExecuteReader())
                    {
                        if (rdr.Read() && rdr.HasRows)
                            result = rdr.GetBoolean(0);
                    }
                 
                    return result;
                }
            }
        }


        /// <summary>
        /// Update row with new data based on mac address. Without  pool and worker data
        /// </summary>
        /// <param name="model"></param>
        private static void updateDbMinerStatus(MinerModel model)
        {

            executeCommand(string.Format(@"Update  NetworkTableHistory Set AutoReboot='{0}',AutoReport='{1}',IP='{2}',Status='{3}',Type='{4}',HashRateRT='{5}',
LastStatusChange=CASE  
                        WHEN Status !='{3}' THEN '{6}'
					 ELSE LastStatusChange
                    END ,
HashRateAvg='{7}',Temperature1='{8}',
Temperature2='{9}',Temperature3='{10}',FanSpeed1='{11}',FanSpeed2='{12}',Elapsed='{13}',Date='{14}' Where Mac='{15}'"
, true, true, model.ip, model.status, model.type, model.hashRateRT, DateTime.UtcNow.ToString(), model.hashRateAverage, model.temperature1, model.temperature2, model.temperature3,
model.fanSpeed1, model.fanSpeed2, model.elapsed, DateTime.UtcNow.ToString(), model.Mac));

        }


        /// <summary>
        /// Update pool info.Used only during config!
        /// </summary>
        /// <param name="model"></param>
        public static void updateDbMinerPoolStatus(MinerModel model)
        {

            executeCommand(string.Format(@"Update  SaveLoadTableHistory Set Pool1='{0}',Pool2='{1}',Pool3='{2}',Worker1='{3}',Worker2='{4}',Worker3='{5}',LastStatusChange='{6}',IP='{7}',
conf_netmask='{8}',conf_gateway='{9}',conf_dnsservers='{10}',conf_nettype='{11}',conf_hostname='{12}'
Where Mac='{13}'"
, model.pool1, model.pool2, model.pool3, model.worker1, model.worker2, model.worker3, DateTime.UtcNow.ToString(), model.ip, model.conf_netmask, model.conf_gateway, model.conf_dnsservers
            , model.conf_nettype, model.conf_hostname, model.Mac));

        }


        /// <summary>
        /// Should be used if there are new mac-address
        /// </summary>
        /// <param name="model"></param>
        private static void insertDbMinerStatus(MinerModel model)
        {
            if (model.Mac != null && model.Mac != "")
            {

                executeCommand(string.Format(@"Insert into NetworkTableHistory(" + columnOrderQuery +
    @",Date)Values ('{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}','{10}','{11}','{12}','{13}','{14}','{15}','{16}','{17}','{18}','{19}','{20}','{21}')"
    , model.autoreboot, model.autoreport, model.ip, model.status, DateTime.UtcNow, model.Mac, model.type, model.hashRateRT, model.hashRateAverage, model.temperature1, model.temperature2, model.temperature3,
    model.fanSpeed1, model.fanSpeed2, model.elapsed, UtilityFunc.ReplaceWhitespace(model.pool1, ""), UtilityFunc.ReplaceWhitespace(model.pool2, ""), UtilityFunc.ReplaceWhitespace(model.pool3, ""),
    UtilityFunc.ReplaceWhitespace(model.worker1, ""), UtilityFunc.ReplaceWhitespace(model.worker2, ""), UtilityFunc.ReplaceWhitespace(model.worker3, ""), DateTime.UtcNow.ToString()));

            }

        }


        /// <summary>
        /// Right now depricated.
        /// </summary>
        /// <param name="model"></param>
        private static void inserDbMinerHistory(MinerModel model)
        {

            executeCommand(string.Format(@"Insert into MiningTableHistory(AutoReboot,AutoReport,IP,Status,Type,HashRateRT,HashRateAvg,Temperature1,
Temperature2,Temperature3,FanSpeed1,FanSpeed2,Elapsed,Pool1,Worker1,Pool2,Worker2,Pool3,Worker3,Date)
Values ('{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}','{10}','{11}','{12}','{13}','{14}','{15}','{16}','{17}','{18}','{19}')"
, true, true, model.ip, model.status, model.type, model.hashRateRT, model.hashRateAverage, model.temperature1, model.temperature2, model.temperature3,
model.fanSpeed1, model.fanSpeed2, model.elapsed, UtilityFunc.ReplaceWhitespace(model.pool1, ""),
UtilityFunc.ReplaceWhitespace(model.worker1, ""), UtilityFunc.ReplaceWhitespace(model.pool2, ""), UtilityFunc.ReplaceWhitespace(model.worker2, ""),
UtilityFunc.ReplaceWhitespace(model.pool3, ""), UtilityFunc.ReplaceWhitespace(model.worker3, ""), DateTime.UtcNow.ToString()));

        }


        private static void inserSaveHistory(MinerModel model)
        {

            executeCommand(string.Format(@"Insert into SaveLoadTableHistory(Pool1,Pool2,Pool3,Worker1,Worker2,Worker3,LastStatusChange,IP,
conf_netmask,conf_gateway,conf_dnsservers,conf_nettype,conf_hostname,Mac)
Values ('{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}','{10}','{11}','{12}','{13}')"
, model.pool1, model.pool2, model.pool3, model.worker1, model.worker2, model.worker3, DateTime.UtcNow.ToString(), model.ip, model.conf_netmask, model.conf_gateway, model.conf_dnsservers
            , model.conf_nettype, model.conf_hostname, model.Mac));
        }



        public static async void saveMinerState(List<MinerModel> minerList)
        {
            foreach (MinerModel miner in minerList.Where(t => t.Mac != ""))
            {
                if (containsMacSave(miner.Mac))
                    updateDbMinerPoolStatus(miner);
                else
                {
                    inserSaveHistory(miner);
                }

            }
        }


        /// <summary>
        /// Return true if same info
        /// </summary>
        /// <param name="oldModel"></param>
        /// <param name="newModel"></param>
        /// <returns></returns>
        public static bool compareMinerModel(MinerModel oldModel, MinerModel newModel)
        {
            if (oldModel.Mac == newModel.Mac && oldModel.pool1 == newModel.pool1 && oldModel.pool2 == newModel.pool2 && oldModel.pool3 == newModel.pool3 &&
                oldModel.worker1 == newModel.worker1 && oldModel.worker2 == newModel.worker2 && oldModel.worker3 == newModel.worker3 && oldModel.ip == newModel.ip
                && oldModel.conf_dnsservers == newModel.conf_dnsservers && oldModel.conf_gateway == newModel.conf_gateway && oldModel.conf_hostname == newModel.conf_hostname && oldModel.conf_netmask == newModel.conf_netmask && oldModel.conf_nettype == newModel.conf_nettype)
                return true;
            else
                return false;
        }


        public static async Task<bool> loadMinerState(List<MinerModel> minerList)
        {



            //Loading network settings
            foreach (MinerModel miner in minerList)
            {
                try
                {
                    if (containsMacSave(miner.Mac))
                    {
                        MinerModel oldMiner = await Sql.executeCommandAndReadPartialMinerModel(string.Format(@" Select * from SaveLoadTableHistory WHERE Mac = '{0}';", miner.Mac));
                        if (!compareMinerModel(oldMiner, miner))
                        {
                            Log.logDebug("loadMinerState Loadding ip:" + miner.ip);

                            await SettingIP.postToUrl(miner.ip, oldMiner);
                        }
                        else
                        {
                            Log.logDebug("loadMinerState All ok ip:" + miner.ip);
                        }
                    }

                }
                catch (Exception ex)
                {
                    Log.logDebug("loadMinerState error1:" + Convert.ToString(ex));
                }
            }
            foreach (MinerModel miner in minerList)
            {
                try
                {
                    if (containsMacSave(miner.Mac))
                    {

                        MinerModel oldMiner = await Sql.executeCommandAndReadPartialMinerModel(string.Format(@" Select * from SaveLoadTableHistory WHERE Mac = '{0}';", miner.Mac));
                        if (!compareMinerModel(oldMiner, miner))
                        {
                            Log.logDebug("loadMinerState Loadding ip:" + miner.ip);

                            await MinerChangeFunc.configMinerSql(oldMiner);
                        }
                        else
                        {
                            Log.logDebug("loadMinerState All ok ip:" + miner.ip);
                        }

                    }

                }
                catch (Exception ex)
                {
                    Log.logDebug("loadMinerState error2:" + Convert.ToString(ex));
                }
            }
            Log.logDebug("loadMinerState end");
            return true;
        }

        public static async void updateDB(MinerModel model)
        {

            lock (minersList)
            {

                if (model.Mac != null && model.Mac != "")
                {
                    try
                    {
                        if (containsMac(model.Mac))
                        {
                            //Update DataBase
                            updateDbMinerStatus(model);
                            //
                        }
                    }
                    catch (Exception ex)
                    {
                        Log.logDebug("updateDB " + model.pool1 + " error " + Convert.ToString(ex));
                    }
                }
                else
                {

                    //insert new row
                    insertDbMinerStatus(model);
                }


                if (minersList.Any(t => t.ip == model.ip))
                    minersList[minersList.FindIndex(t => t.ip == model.ip)] = model;
                else
                    minersList.Add(model);
            }

        }
        public static void populateBD()
        {

            try
            {
                SQLiteConnectionStringBuilder connectString = new SQLiteConnectionStringBuilder();
                connectString.DataSource = "FlyMiningDB.sqlite";
                connectString.ForeignKeys = false;
                connectString.JournalMode = SQLiteJournalModeEnum.Default;
                sqlconnectString = connectString.ToString();

                SQLiteDataAdapter sqlAdapter = new SQLiteDataAdapter();
                using (SQLiteConnection con = new SQLiteConnection(sqlconnectString))
                {
                    con.Open();
                    int result = executeCommand(@"CREATE TABLE IF NOT EXISTS
  [NetworkTableHistory] (
  [AutoReboot] BOOL NOT NULL,
    [AutoReport] BOOL NOT NULL,
    [IP] NVARCHAR(50) NOT NULL,
    [Status] NVARCHAR(50) NOT NULL,
[LastStatusChange] DATETIME NOT NULL,
[Mac] NVARCHAR(100) NOT NULL  PRIMARY KEY ,
 [Type] NVARCHAR(50) ,
    [HashRateRT] DECIMAL(18,2) ,
 [HashRateAvg] DECIMAL(18,2) ,
 [Temperature1] INT ,
 [Temperature2] INT ,
 [Temperature3] INT ,
 [FanSpeed1] INT ,
 [FanSpeed2] INT,
 [Elapsed] NVARCHAR(50) ,
 [Pool1] NVARCHAR(200) ,
    [Worker1] NVARCHAR(50) ,
    [Pool2] NVARCHAR(200) ,
    [Worker2] NVARCHAR(50) ,
    [Pool3] NVARCHAR(200),
    [Worker3] NVARCHAR(50),
[Date] DateTime )



");



                    result = executeCommand(@"CREATE TABLE IF NOT EXISTS
  [SaveLoadTableHistory] (
[IP] NVARCHAR(50) NOT NULL,
[Mac] NVARCHAR(100) NOT NULL  PRIMARY KEY ,
[LastStatusChange] DATETIME NOT NULL,
 [Pool1] NVARCHAR(200) ,
    [Worker1] NVARCHAR(100) ,
    [Pool2] NVARCHAR(200) ,
    [Worker2] NVARCHAR(100) ,
    [Pool3] NVARCHAR(200),
    [Worker3] NVARCHAR(100),
[Date] DateTime,
       conf_netmask NVARCHAR(200) ,
        conf_gateway NVARCHAR(200) ,
        conf_dnsservers NVARCHAR(200) ,
        conf_nettype NVARCHAR(200) ,
        conf_hostname NVARCHAR(200) )
");




                    Log.logDebug("PopulateBD result:" + result);

                    result = executeCommand(@"CREATE TABLE IF NOT EXISTS
  [MiningTableHistory] (
  [Id]     INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT,
  [AutoReboot] BOOL NOT NULL,
    [AutoReport] BOOL NOT NULL,
    [IP] NVARCHAR(50) NOT NULL,
    [Status] NVARCHAR(50) NOT NULL,

 [Type] NVARCHAR(50) ,
    [HashRateRT] DECIMAL(18,2) ,
 [HashRateAvg] DECIMAL(18,2) ,
 [Temperature1] INT ,
 [Temperature2] INT ,
 [Temperature3] INT ,
 [FanSpeed1] INT ,
 [FanSpeed2] INT,
 [Elapsed] NVARCHAR(50) ,
 [Pool1] NVARCHAR(200) ,
    [Worker1] NVARCHAR(50) ,
    [Pool2] NVARCHAR(200) ,
    [Worker2] NVARCHAR(50) ,
    [Pool3] NVARCHAR(200),
    [Worker3] NVARCHAR(50),
[Date] DateTime)
");
                    Log.logDebug("PopulateBD result:" + result);
                }
            }
            catch (Exception ex)
            {
                Log.logDebug("PopulateBD " + Convert.ToString(ex));
            }


        }


    }
}
//        public void cleanDB()
//        {
//            //sql_connect.Open();
//             SQLiteCommand cmd = sql_connect.CreateCommand();
//             cmd.CommandText = "DELETE FROM MiningTable";
//             cmd.ExecuteNonQuery();
//            //sql_connect.Open();

//        }
//        public void insertNewRowsDB(int id,bool autoreboot,bool autoreport,string ip)
//        {
//            //sql_connect.Open();
//            SQLiteCommand cmd = sql_connect.CreateCommand();
//            cmd.CommandText = string.Format(@"Insert into MiningTable(id,AutoReboot,AutoReport,IP,Status)
//Values ('{0}','{1}','{2}','{3}','{4}')",id,autoreboot,autoreport,ip,"Created");
//            cmd.ExecuteNonQuery();
//            //sql_connect.Open();
//        }