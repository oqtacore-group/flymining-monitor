using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net.Sockets;
using System.IO;
namespace BitcoinInfoMiner
{
    #region Responce Classes
    public class OrderResponce
    {
        public bool success { get; set; }
        public string message { get; set; }
        public JArray result { get; set; }

    }
    public class OrderBody
    {
        public string wallet { get; set; }
        public string OrderUuid { get; set; }
        public string Limit { get; set; }
        public string Closed { get; set; }
        public string Opened { get; set; }
        public string Exchange { get; set; }
        public string OrderType { get; set; }
        public string Quantity { get; set; }
        public double PricePerUnit { get; set; }
        public string Price { get; set; }
        public string Commission { get; set; }
    }
    public class BittrexResponse
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public JToken Result { get; set; }
    }

    public class OperationArray
    {
        public JArray inputs { get; set; }
        public JArray Out { get; set; }
        public string result { get; set; }
        public string time { get; set; }
    }
    public class OperationInput
    {
        public JArray inputs { get; set; }
    }

    #endregion

    #region Classes ForParsing Responce
    public class BittrexBody
    {
        public string date { get; set; }
        public string TxId { get; set; }
        public double amount { get; set; }
        public string address { get; set; }
        public double TxCost { get; set; }
    }
    public class ETHResponce
    {
        public string status { get; set; }
        public JArray result { get; set; }
    }
    public class ETHBody
    {
        public string hash { get; set; }
        public long gasUsed { get; set; }
        public long gasPrice { get; set; }
        public long time { get; set; }
        public string from { get; set; }
        public string to { get; set; }
        public long value { get; set; }
        public double fee { get; set; }
    }
    public class LTCResponce
    {
        public string status { get; set; }
        //public double balance { get; set; }
        public JToken data { get; set; }
    }

    public class LTCBody
    {
        public string txid { get; set; }
        public string from { get; set; }
        public long time { get; set; }
        public JArray inputs { get; set; }
        public JArray outputs { get; set; }
        public double value { get; set; }
        public double fee { get; set; }
        public bool state { get; set; }
    }
    public class NiceHashResponce
    {
        public string method { get; set; }
        //public double balance { get; set; }
        public JToken result { get; set; }
    }
    public class NiceHashBody
    {
        public string txid { get; set; }
        public string time { get; set; }
        public string value { get; set; }
        public string fee { get; set; }
        public string type { get; set; }
    }
    public class OperationArrayParse
    {
        public long final_balance { get; set; }
        public JArray inputs { get; set; }
        public JArray Out { get; set; }
        public string result { get; set; }
        public long time { get; set; }
        public string hash { get; set; }
        public int size { get; set; }
    }
    public class OperationOut
    {
        public string addr { get; set; }
        public long value { get; set; }
        public bool spent { get; set; }
    }
    public class OperationInputBody
    {
        public string addr { get; set; }
        public long value { get; set; }
    }
    public class OperationResponse
    {
        public string hash160 { get; set; }
        public string address { get; set; }
        public string n_tx { get; set; }
        public string total_received { get; set; }
        public string total_sent { get; set; }
        public string final_balance { get; set; }
        public JArray txs { get; set; }
    }
    public class OperationAllResponse
    {
        public JArray txs { get; set; }
    }
    public class OperationAllData
    {
        public string hash { get; set; }
        public long fee { get; set; }
        public long result { get; set; }
        public long balance { get; set; }
        public long time { get; set; }
        public JArray inputs { get; set; }
        public JArray Out { get; set; }
        public int size { get; set; }
    }
    public struct OperationInfo
    {
        public long outAllSum;
        public long inputAllSum;
        public long outThisWallet;
        public long inputThisWallet;
        public long outSpent;
    }
    public class TransactionInfo
    {
        public Dictionary<int, String> inputs { get; set; }
        public Dictionary<int, String> Out { get; set; }
        public int size { get; set; }

    }
    #endregion
    public enum txType {orderSell,orderBuy,txD,txW }
    public class CSVData
    {
        public string Wallet { get; set; } //Wallet id
        public txType type { get; set; } //tx Type 
        public string Descrtion { get; set; } //Wallet description
        public double value { get; set; } //value of tx
        public double valueConvert { get; set; } //only for order.
        public double usdRate { get; set; } //usdRate for fee (yep)
        public string id { get; set; } //hash
        public double income { get; set; } // Calculated income. Only for tx
        public double fee { get; set; } //Fee
        public string dopData { get; set; }//Dop data if Order then (BTC-ETH) else mining column
        public string currency { get; set; }// Currency of value
        public string walletType { get; set; }// Currency of value
        public double rest { get; set; }//Остаток на балансе (value)
        public double restConvert { get; set; }//Остаток на балансе для второй части ордера(valueConvert)
        public string outputWallets { get; set; }
    }
    public class OperData
    {
        public string currency { get; set; }
        public double sum { get; set; }
        public DateTime date { get; set; }
    }

    class Wallets
    {
        public static Dictionary<string, double> walletsIncome = new Dictionary<string, double>();
        public static SortedDictionary<DateTime, CSVData> fullWalletData;
        public static Dictionary<string, double> historicalExcRate;
        public static Dictionary<int, OrderBody> orderHistory;
        public static string bittrexUri = "https://bittrex.com/api/v1.1/";
        public static string LTCUri = "https://chain.so/api/v2/";
        public static string blockChainUri = "https://blockchain.info/";
        public static string ethUri = "http://api.etherscan.io/api";
        public static string NiceHashUri = "https://api.nicehash.com/api";
        public static string stockMarketUri = "https://api.coinmarketcap.com/v1/ticker/";        
        public static string bittrexOldOrderHistory = "BittrexOrders";
        public  static string settingFileName = "logReport//HistoricalRates.txt";
        public static readonly Encoding encoding = Encoding.UTF8;
        private static HttpClient httpClient =new HttpClient();
        public static  Dictionary<string, double> marketInfo;
        public static Dictionary<string, string[]> walletInfo;
        public static Dictionary<string, string[]> walletInfoDescription;
        private static bool parseMarketState = false;
        //Send Bittrex Request without parameters
        public static async Task<string> sendBittrexRequest(string command, string apikey, string secret)
        {
            return await sendBittrexRequest(command, apikey, secret, new Dictionary<string, string>());
        }
        private static string byteToString(byte[] buff)
        {
            string sbinary = "";
            for (int i = 0; i < buff.Length; i++)
                sbinary += buff[i].ToString("X2"); /* hex format */
            return sbinary;
        }
        //Send Bittrex Request
        public static async Task<string> sendBittrexRequest(string command, string apikey, string secret, IDictionary<string, string> addParameters)
        {
            try
            {
                IDictionary<string, string> parameters = new Dictionary<string, string>(addParameters);
                var nonce = DateTime.Now.Ticks;
                parameters.Add("apikey", apikey);
                parameters.Add("nonce", nonce.ToString());
                var parameterString = convertParameterListToString(parameters);
                var completeUri = Wallets.bittrexUri + command + "?" + parameterString;
                var uriBytes = encoding.GetBytes(completeUri);
                var request = new HttpRequestMessage(HttpMethod.Get, completeUri);
                using (var hmac = new HMACSHA512(Encoding.UTF8.GetBytes(secret)))
                {
                    var hash = hmac.ComputeHash(uriBytes);
                    var hashText = byteToString(hash);
                    request.Headers.Add("apisign", hashText);
                }
                HttpResponseMessage response = await httpClient.SendAsync(request);
                if (!response.IsSuccessStatusCode)
                    return "False";
                var content = await response.Content.ReadAsStringAsync();

                return content;
            }
            catch (Exception e)
            {
                Log.logDebug("Bittrex request "+Convert.ToString(e, CultureInfo.InvariantCulture));
                return "False";
            }

        }
        //Send normal Request without args
        public static async Task<string> sendAsyncRequest(string command, string uri)
        {
            return await sendAsyncRequest(command, uri, new Dictionary<string, string>());
        }
        //Send normal Request 
        public  static async Task<string> sendAsyncRequest(string command, string uri, IDictionary<string, string> addParameters)
        {
            try
            {
                
                IDictionary<string, string> parameters = new Dictionary<string, string>(addParameters);
                var nonce = DateTime.Now.Ticks;
                var parameterString = convertParameterListToString(parameters);
                var completeUri = uri + command + "?" + parameterString;
                var uriBytes = encoding.GetBytes(completeUri);
                var request = new HttpRequestMessage(HttpMethod.Get, completeUri);
                HttpResponseMessage response = await httpClient.SendAsync(request).ConfigureAwait(false);
                var content = await response.Content.ReadAsStringAsync();
                if (!response.IsSuccessStatusCode)
                    return "Wait";


                return content;
            }
            catch (Exception e)
            {
                Log.logDebug("Async request "+Convert.ToString(e, CultureInfo.InvariantCulture));
                return "False";
            }

        }

//        public static  async void DoTesting()
//        {
//            string command; string uri; IDictionary<string, string> addParameters;
//            var completeUri = "https://payment.yandex.net/api/v3/payments/41001265598048";
//            var uriBytes = encoding.GetBytes(completeUri);
//            var request = new HttpRequestMessage(HttpMethod.Get, completeUri);
           
//    //        httpClient.DefaultRequestHeaders.Authorization =
//    //new AuthenticationHeaderValue(
//    //    "Basic",
//    //    Convert.ToBase64String(
//    //        System.Text.ASCIIEncoding.ASCII.GetBytes(
//    //            string.Format("{0}:{1}", "61329", "8oyURWCJALPc1r8IVVV2"))));
//            httpClient.DefaultRequestHeaders.Authorization =
//new AuthenticationHeaderValue(
//"Basic",
//    string.Format("{0}:{1}", "61329", "8oyURWCJALPc1r8IVVV2"));
//            HttpResponseMessage response = await httpClient.SendAsync(request);
//            var content = await response.Content.ReadAsStringAsync();
//            content += "";
//        }
        //Convert List to single string
        private static string convertParameterListToString(IDictionary<string, string> parameters)
        {
            if (parameters.Count == 0) return "";

            return parameters.Select(param => System.Uri.EscapeDataString(param.Key) + "=" + System.Uri.EscapeDataString(param.Value)).Aggregate((l, r) => l + "&" + r);
        }
        //ParsedAll blockchain data
        public static async Task<IList<OperationAllData>> getAllBlockchainOperation(string[] wallets)
        {
            try
            {
                Dictionary<string, string> args = new Dictionary<string, string>();
                args.Add("active", String.Join("|", wallets));
                args.Add("n", "300");

                string temop = await Wallets.sendAsyncRequest("ru/multiaddr", Wallets.blockChainUri, args);
                OperationAllResponse operationResponse = new OperationAllResponse();
                operationResponse = JsonConvert.DeserializeObject<OperationAllResponse>(temop);

                //JArray blogPostArray = JArray.Parse(operationResponse3.txs);

                IList<OperationAllData> mainBodyAll = operationResponse.txs.Select(p => new OperationAllData
                {

                    fee = (long)p["fee"],
                    hash = (string)p["hash"],
                    result = (long)p["result"],
                    time = (long)p["time"],
                    inputs = (JArray)p["inputs"],
                    Out = (JArray)p["out"],
                    balance = (long)p["balance"],
                    size = (int)p["size"]
                }).ToList();
                return mainBodyAll;
            }
            catch (Exception ex)
            {
                Log.logDebug("getAllBlockChainOperation " + Convert.ToString(ex));
                return new List<OperationAllData>();
            }
        }
        //Parse this blockchain wallet
        public static async Task<IList<OperationAllData>> getBlockchainOperation(string wallet)
        {
            try
            {
                Dictionary<string, string> args = new Dictionary<string, string>();
                args.Add("active", wallet);
                args.Add("n", "100");

                string temop = await Wallets.sendAsyncRequest("ru/multiaddr", Wallets.blockChainUri, args);
                OperationAllResponse operationResponse = new OperationAllResponse();
                operationResponse = JsonConvert.DeserializeObject<OperationAllResponse>(temop);

                //JArray blogPostArray = JArray.Parse(operationResponse3.txs);

                IList<OperationAllData> mainBodyAll = operationResponse.txs.Select(p => new OperationAllData
                {

                    fee = (long)p["fee"],
                    hash = (string)p["hash"],
                    result = (long)p["result"],
                    time = (long)p["time"],
                    inputs = (JArray)p["inputs"],
                    Out = (JArray)p["out"],
                    balance = (long)p["balance"],
                    size = (int)p["size"]
                }).ToList();
                return mainBodyAll;
            }
            catch (Exception ex)
            {
                Log.logDebug("getBlockChainOperation " + Convert.ToString(ex));
                return new List<OperationAllData>();
            }
        }
        //Get current currence rates
        public static async Task parseMarket()
        {
            string json = await Wallets.sendAsyncRequest("", Wallets.stockMarketUri);
            try
            {
                if (!parseMarketState&&json != "False")
                {
                    parseMarketState = true;
                    var marketResponse = JsonConvert.DeserializeObject<JToken>(json);
                    marketInfo = new Dictionary<string, double>();//[bittrexResponse.Result.Count()];
                    for (int iter = 0; iter < marketResponse.Count(); iter++)
                    {
                        if (!marketInfo.Keys.Contains(marketResponse[iter].First.Next.Next.First.ToString()))
                            marketInfo.Add(marketResponse[iter].First.Next.Next.First.ToString(),
                                Convert.ToDouble(marketResponse[iter].First.Next.Next.Next.Next.First.ToString(), CultureInfo.InvariantCulture));
                        //temp[iter] = bittrexResponse.Result[iter].First.Next.ToString();
                    }
                    parseMarketState = false;
                }
            }
            catch (Exception ex)
            {
                parseMarketState = false;
                Log.logDebug("parseMarket "+Convert.ToString(ex));
            }
        }
        //Parse this wallet info
        public static async Task<IList<ETHBody>> getETHOperation(string wallet)
        {
            return await getETHOperation(new string[] { wallet });
        }
        //Parse this wallets info
        public static async Task<IList<ETHBody>> getETHOperation(string[] wallets)
        {
            try
            {
                IList<ETHBody> mainBodyAll = new List<ETHBody>();
                //foreach (string wallet in wallets)
                //{
                //    Dictionary<string, string> args = new Dictionary<string, string>();
                //    args.Add("module", "account");
                //    args.Add("action", "txlistinternal");
                //    args.Add("address", wallet);
                //    args.Add("sort", "desc");
                //    string temop = await Wallets.sendAsyncRequest("", Wallets.ethUri, args);
                //    ETHResponce operationResponse = new ETHResponce();
                //    operationResponse = JsonConvert.DeserializeObject<ETHResponce>(temop);

                //    //JArray blogPostArray = JArray.Parse(operationResponse3.txs);

                //    IList<ETHBody> mainBody = operationResponse.result.Select(p => new ETHBody
                //    {
                //        hash = (string)p["hash"],
                //        from = (string)p["from"],
                //        //gasPrice = (long)p["gasPrice"],
                //        gasUsed = (long)p["gasUsed"],
                //        time = (long)p["timeStamp"],
                //        to = (string)p["to"],
                //        value = (long)p["value"],

                //    }).ToList();
                //    for (int iter = 0; iter < mainBody.Count;iter++ )
                //        mainBodyAll.Add(mainBody[iter]);
                //}
                foreach (string wallet in wallets)
                {
                    Dictionary<string, string> args = new Dictionary<string, string>();
                    args.Add("module", "account");
                    args.Add("action", "txlist");
                    args.Add("address", wallet);
                    args.Add("sort", "desc");
                    string temop = await Wallets.sendAsyncRequest("", Wallets.ethUri, args);
                    ETHResponce operationResponse = new ETHResponce();
                    operationResponse = JsonConvert.DeserializeObject<ETHResponce>(temop);

                    //JArray blogPostArray = JArray.Parse(operationResponse3.txs);

                    IList<ETHBody> mainBody = operationResponse.result.Select(p => new ETHBody
                    {
                        hash = (string)p["hash"],
                        from = (string)p["from"],
                        gasPrice = (long)p["gasPrice"],
                        gasUsed = (long)p["gasUsed"],
                        time = (long)p["timeStamp"],
                        to = (string)p["to"],
                        value = (long)p["value"],

                    }).ToList();
                    for (int iter = 0; iter < mainBody.Count; iter++)
                        mainBodyAll.Add(mainBody[iter]);
                }
                return mainBodyAll;
            }
            catch (Exception ex)
            {
                Log.logDebug("getETHOperation " + Convert.ToString(ex));
                return new List<ETHBody>();
            }
         
        }
        //Parse this wallet info
        public static async Task<IList<LTCBody>> getLTCOperation(string wallet)
        {
            return await getLTCOperation(new string[] { wallet });
        }
        //Parse this wallets info
        public static async Task<IList<LTCBody>> getLTCOperation(string[] wallets)
        {
            try
            {
                IList<LTCBody> mainBodyAll = new List<LTCBody>();
                foreach (string wallet in wallets)
                {
                    string temop = await Wallets.sendAsyncRequest("address/LTC/" + wallet, Wallets.LTCUri);
                    //operationResponse = new JToken();
                    LTCResponce operationResponse = JsonConvert.DeserializeObject<LTCResponce>(temop);
                    JToken temp = operationResponse.data["txs"];
                    //JArray blogPostArray = JArray.Parse(operationResponse3.txs);
                    IList<LTCBody> mainBody = new List<LTCBody>();
                    for (int iter = 0; iter < temp.Count(); iter++)
                    {
                        if (temp[iter].ToString().Contains("outgoing"))
                        {
                            double tempFee=(double)temp[iter]["outgoing"]["value"];
                            foreach (JToken token in (JArray)temp[iter]["outgoing"]["outputs"])
                            {
                                tempFee -= (double)token["value"];
                            }
                            if (tempFee < 0)
                            {
                                temop = await Wallets.sendAsyncRequest("tx/LTC/" + (string)temp[iter]["txid"], Wallets.LTCUri);
                                operationResponse = JsonConvert.DeserializeObject<LTCResponce>(temop);
                                tempFee = (double)operationResponse.data["sent_value"];
                                foreach (JToken token in (JArray)operationResponse.data["outputs"])
                                {
                                    tempFee -= (double)token["value"];
                                }
                                mainBody.Add(new LTCBody
                                {
                                    time = (long)temp[iter]["time"],
                                    value = (double)operationResponse.data["sent_value"],
                                    txid = (string)temp[iter]["txid"],
                                    outputs = (JArray)temp[iter]["outgoing"]["outputs"],
                                    state = false,
                                    from = wallet,
                                    fee = tempFee

                                });
                                //"https://chain.so/api/v2/"
                                //https://chain.so/api/v2/tx/DOGE/6f47f0b2e1ec762698a9b62fa23b98881b03d052c9d8cb1d16bb0b04eb3b7c5b
                            }
                            else
                            {
                                mainBody.Add(new LTCBody
                                {
                                    time = (long)temp[iter]["time"],
                                    value = (double)temp[iter]["outgoing"]["value"],
                                    txid = (string)temp[iter]["txid"],
                                    outputs = (JArray)temp[iter]["outgoing"]["outputs"],
                                    state = false,
                                    from = wallet,
                                    fee = tempFee

                                });
                            }
                        }
                        else
                        {
                            mainBody.Add(new LTCBody
                            {
                                time = (long)temp[iter]["time"],
                                value = (double)temp[iter]["incoming"]["value"],
                                txid = (string)temp[iter]["txid"],
                                inputs = (JArray)temp[iter]["incoming"]["inputs"],
                                state = true,
                                from = wallet
                                //fee=()

                            });
                        }
                    }
                    for (int iter = 0; iter < mainBody.Count; iter++)
                        mainBodyAll.Add(mainBody[iter]);
                }
                return mainBodyAll;
            }
            catch (Exception ex)
            {
                Log.logDebug("getLTCOperation " + Convert.ToString(ex));
                return new List<LTCBody>();
            }
        }
        //Parse this wallet info
        public static async Task<IList<NiceHashBody>> getNiceHashOperation(string wallet)
        {
            return await getNiceHashOperation(new string[] { wallet });
        }
        //Parse this wallets info
        public static async Task<IList<NiceHashBody>> getNiceHashOperation(string[] wallets)
        {
            try
            {
                IList<NiceHashBody> mainBodyAll = new List<NiceHashBody>();
                foreach (string wallet in wallets)
                {
                    Dictionary<string, string> args = new Dictionary<string, string>();
                    args.Add("method", "stats.provider.payments");
                    args.Add("addr", wallet);
                    string temop = await Wallets.sendAsyncRequest("", Wallets.NiceHashUri, args);
                    NiceHashResponce operationResponse = new NiceHashResponce();
                    operationResponse = JsonConvert.DeserializeObject<NiceHashResponce>(temop);

                    //JArray blogPostArray = JArray.Parse(operationResponse3.txs);
                    JArray payments =(JArray) operationResponse.result.First.Next.First;
                    IList<NiceHashBody> mainBody = payments.Select(p => new NiceHashBody
                    {                        
                        fee = (string)p["fee"],
                        time = (string)p["time"],
                        value = (string)p["amount"],                        
                    }).ToList();
                    for (int iter = 0; iter < mainBody.Count; iter++)
                        mainBodyAll.Add(mainBody[iter]);
                }
                return mainBodyAll;
            }
            catch (Exception ex)
            {
                Log.logDebug("getNiceHashOperation " + Convert.ToString(ex));
                return new List<NiceHashBody>();
            }
        }
        //Parse this wallet info
        public static async Task<List<BittrexBody>> getBittrexOperation(string wallet, string secret, string currency)
        {
            try
            {
                Dictionary<string, string> arg = new Dictionary<string, string>();
                arg.Add("currency", currency);
                string temW = "";
                temW = await Wallets.sendBittrexRequest("account/getwithdrawalhistory",
                     wallet, secret, arg);
                string temD = "";
                temD = await Wallets.sendBittrexRequest("account/getdeposithistory",
                             wallet, secret, arg);
                BittrexResponse operationResponse = JsonConvert.DeserializeObject<BittrexResponse>(temW);
                IList<BittrexBody> mainBodyW = operationResponse.Result.Select(p => new BittrexBody
                {
                    address = (string)p["Address"],
                    amount = (double)p["Amount"] + (double)p["TxCost"],
                    date = (string)p["Opened"],
                    TxCost = (double)p["TxCost"],
                    TxId = (string)p["TxId"]
                }).ToList();
                operationResponse = JsonConvert.DeserializeObject<BittrexResponse>(temD);
                if (currency == "ZEC")
                {
                    currency += "";
                }
                IList<BittrexBody> mainBodyD = operationResponse.Result.Select(p => new BittrexBody
                {
                    address = (string)p["CryptoAddress"],
                    amount = (double)p["Amount"],
                    date = (string)p["LastUpdated"],
                    TxCost = 0,
                    TxId = (string)p["TxId"]
                }).ToList();
                List<BittrexBody> mainBodyAll = new List<BittrexBody>(mainBodyW.Count +
                                    mainBodyD.Count);
                mainBodyAll.AddRange(mainBodyW);
                mainBodyAll.AddRange(mainBodyD);
                return mainBodyAll;
            }
            catch(Exception ex)
            {
                Log.logDebug("getBittrexOperation " + Convert.ToString(ex));
                return new List<BittrexBody>();
            }
        }
        //Find usd rate for this unix date
        public static async Task<double> getUSDrateDate(long unixDate, string coin = "BTC")
        {

            if (historicalExcRate.Keys.Contains(coin + ':' + Convert.ToString(unixDate, CultureInfo.InvariantCulture)))
            {
                return historicalExcRate[coin + ':' + Convert.ToString(unixDate, CultureInfo.InvariantCulture)];
            }
            else
            {
                try
                {
                    
                    Dictionary<string, string> temp = new Dictionary<string, string>();
                    temp.Add("start", Convert.ToString(unixDate, CultureInfo.InvariantCulture));
                    if (Wallets.ToUnixFromDateTime(DateTime.Now) * 1000 > unixDate + 120000)
                    {
                        temp.Add("end", Convert.ToString(unixDate + 120000, CultureInfo.InvariantCulture));
                    }
                    else
                        temp.Add("end", Convert.ToString(Wallets.ToUnixFromDateTime(DateTime.Now) * 1000, CultureInfo.InvariantCulture));
                    string json = "";
                    if (coin == "BCC")
                        json = await Wallets.sendAsyncRequest("", "https://api.bitfinex.com/v2/candles/trade:1m:tBCHUSD/hist", temp);
                    else
                        json= await Wallets.sendAsyncRequest("", "https://api.bitfinex.com/v2/candles/trade:1m:t" + coin + "USD/hist", temp);
                    if (json == "Wait")
                        return 0;
                    if (json =="[]")
                    {
                        saveMarketRate(Convert.ToString(unixDate, CultureInfo.InvariantCulture), coin, "0");
                        return 0;
                    }
                    string[] real = json.Split(new char[] { '{', '}', '[', ']', ',' }, StringSplitOptions.RemoveEmptyEntries);
                    if (real.Count() > 3)
                    {
                        saveMarketRate(Convert.ToString(unixDate, CultureInfo.InvariantCulture), coin, real[3]);
                        return Convert.ToDouble(real[3], CultureInfo.InvariantCulture);
                    }
                    else
                    {
                        return 0;
                    }
                }
                catch(Exception ex)
                {
                    Log.logDebug("UsdRate" + Convert.ToString(ex));
                    return 0;
                }
            }
            //textBox1.Text = json;
        }
        //From DateTime to Unix
        public static long ToUnixFromDateTime(DateTime dateTime)
        {
            var epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            var unixDateTime = Convert.ToInt64((dateTime.ToUniversalTime() - epoch).TotalSeconds);
            return unixDateTime;
        }
        //FromUnixto DateTime
        public static DateTime ToDateTimeFromUnix(long unixDateTime)
        {
            var timeSpan = TimeSpan.FromSeconds(unixDateTime);
            var epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            var utcDateTime = epoch.Add(timeSpan).ToUniversalTime();
            return utcDateTime;
        }
        //Add rate for this name currence for time date
        public static void saveMarketRate(string time, string name, string rate)
        {
            try
            {
                historicalExcRate.Add(name + ':' + time, Math.Round(Convert.ToDouble(rate, CultureInfo.InvariantCulture), 2));
            }
            catch(Exception ex)
            {
                Log.logDebug("saveMarketRate" + Convert.ToString(ex));
            }
        }
        //Save saved usd rates
        public static void saveHistoricalInFile()
        {
            try
            {
                string parsedText = "";
                foreach (string key in historicalExcRate.Keys)
                {
                    parsedText += key + '=' + Convert.ToString(Math.Round(historicalExcRate[key], 2), CultureInfo.InvariantCulture) + "\r\n";
                }
                File.WriteAllText(Application.StartupPath + "\\" + Wallets.settingFileName, parsedText, Encoding.Unicode);
            }
            catch(Exception ex)
            {
                Log.logDebug("SaveHistorical" + Convert.ToString(ex));
            }
        }
        //Parse saved historical rates
        public  static void parseHistoricalRateHistory()
        {
            try
            {
                historicalExcRate = new Dictionary<string, double>();
                if (File.Exists(Application.StartupPath + "\\" + Wallets.settingFileName))
                {
                    string parseString = File.ReadAllText(Application.StartupPath + "\\" + Wallets.settingFileName);
                    string[] settingsArray = parseString.Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);
                    for (int iter = 0; iter < settingsArray.Length; iter++)
                    {
                        if (settingsArray[iter].IndexOf(':') > 0)
                        {
                            string name = settingsArray[iter].Remove(settingsArray[iter].IndexOf(':'));
                            string time = settingsArray[iter].Substring(settingsArray[iter].IndexOf(':') + 1);
                            if (time.IndexOf('=') > 0)
                            {
                                string rate = time.Substring(time.IndexOf('=') + 1);
                                time = time.Remove(time.IndexOf('='));
                                saveMarketRate(time, name, rate);
                            }
                        }
                    }
                }
                else
                {
                    File.Create(Application.StartupPath + "\\" + Wallets.settingFileName);
                }
            }            
            catch(Exception ex)
            {
                Log.logDebug("parseHistoricalRateHistory" + Convert.ToString(ex));
            }
        }
        //Parse order history from file
        public static async Task parseOrderHistory(string wallet,string secret)
        {
            string fileName = Application.StartupPath + "//" + Wallets.bittrexOldOrderHistory + wallet + ".csv";
            if (!File.Exists(fileName))
            {
                using (var file = File.Create(fileName))
                { }
            }

            //string[] fileText = records.ToString().Split(new char[] { '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries);
            string[] fileText = File.ReadAllText(fileName, Encoding.Unicode).Split(new char[] { '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries);
            if (fileText.Count() > 0)
            {
                Dictionary<string, string[]> result = new Dictionary<string, string[]>();
                string[] columnName = fileText[0].Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                for (int iter = 0; iter < columnName.Count(); iter++)
                    result.Add(columnName[iter], new string[fileText.Count() - 1]);

                for (int iter = 1; iter < fileText.Count(); iter++)
                {
                    string[] columnValue = fileText[iter].Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                    for (int iterIner = 0; iterIner < columnValue.Count(); iterIner++)
                    {
                        result[columnName[iterIner]][iter - 1] = columnValue[iterIner];
                    }
                }
                Wallets.orderHistory.Clear();
                for (int iter = 0; iter < fileText.Count() - 1; iter++)
                {

                    if (result["Closed"][iter] != null)
                    {
                        DateTime temp = DateTime.ParseExact(result["Closed"][iter], "M/d/yyyy h:mm:ss tt", CultureInfo.InvariantCulture);
                        Wallets.orderHistory.Add(iter, new OrderBody
                        {
                            wallet=wallet,
                            Closed = result["Closed"][iter],
                            Exchange = result["Exchange"][iter],
                            OrderType = result["Type"][iter],
                            Quantity = result["Quantity"][iter],
                            PricePerUnit = 0,
                            Price = result["Price"][iter],
                            Commission = result["CommissionPaid"][iter],
                            OrderUuid = result["OrderUuid"][iter],
                            Opened = result["Opened"][iter],
                            Limit = result["Limit"][iter],
                        });
                    }
                }
            }
            await parseWallets(wallet,secret);

        }
        //Parse order history from api
        public static async Task parseWallets(string wallet,string secret)
        {
            string responce = "";
            responce = await Wallets.sendBittrexRequest("account/getorderhistory",
                         wallet, secret);

            OrderResponce operationResponse = new OrderResponce();
            operationResponse = JsonConvert.DeserializeObject<OrderResponce>(responce);
            if (operationResponse.success != false)
            {
                //JArray blogPostArray = JArray.Parse(operationResponse3.txs);

                IList<OrderBody> mainBodyAll = operationResponse.result.Select(p => new OrderBody
                {
                    Closed = (string)p["Closed"],
                    Exchange = (string)p["Exchange"],
                    OrderType = (string)p["OrderType"],
                    Quantity = (string)p["Quantity"],
                    PricePerUnit = (double)p["PricePerUnit"],
                    Price = (string)p["Price"],
                    Commission = (string)p["Commission"],
                    Opened = (string)p["TimeStamp"],
                    OrderUuid = (string)p["OrderUuid"],
                    Limit = (string)p["Limit"],
                }).ToList();

                for (int iter = 0; iter < mainBodyAll.Count; iter++)
                {
                    bool check = false;
                    try
                    {
                        mainBodyAll[iter].Closed = DateTime.ParseExact(mainBodyAll[iter].Closed, "MM'/'dd'/'yyyy HH:mm:ss", CultureInfo.InvariantCulture).ToString("M/d/yyyy h:mm:ss tt", CultureInfo.InvariantCulture);
                        mainBodyAll[iter].Opened = DateTime.ParseExact(mainBodyAll[iter].Opened, "MM'/'dd'/'yyyy HH:mm:ss", CultureInfo.InvariantCulture).ToString("M/d/yyyy h:mm:ss tt", CultureInfo.InvariantCulture);
                    }
                    catch(Exception ex)
                    {
                        Log.logDebug("parse Wallets Date Converion:" + Convert.ToString(ex));
                    }
                    for (int i = 0; i < Wallets.orderHistory.Count; i++)
                    {
                        if (Wallets.orderHistory[i].Closed == mainBodyAll[iter].Closed)
                            check = true;
                    }
                    if (!check)
                    {
                        try
                        {
                            Wallets.orderHistory.Add(Wallets.orderHistory.Count, mainBodyAll[iter]);
                        }
                        catch (Exception ex)
                        {
                            Log.logDebug("parseWallets " + Convert.ToString(ex));
                            check = true;
                        }
                    }


                }
            }
            
        }
        //Parse Setting to get descriptions
        public static void getWalletsDescriptions()
        {
            foreach (string key in walletInfo.Keys)
            {
                walletInfoDescription.Add(key,new string[walletInfo[key].Count()]);
                for(int iter=0;iter<walletInfo[key].Count();iter++)
                {
                    if (walletInfo[key][iter].IndexOf(":")>=0)
                    {                           
                        walletInfoDescription[key][iter]=walletInfo[key][iter].Remove(walletInfo[key][iter].IndexOf(":"));
                        walletInfo[key][iter] = walletInfo[key][iter].Substring(walletInfo[key][iter].IndexOf(":") + 1);
                        //settingsArray[iter].Substring(settingsArray[iter].IndexOf('=') + 1)
                    }
                    else
                    {
                        walletInfoDescription[key][iter] = "";
                    }
                }
            }
        }

        #region Wallet Functions
        //Calculates wallet income
        public static  async void getWalletsIncome(object obj = null)
        {
            await getFullWalletDate();
           
            Dictionary<string, OperData> operData = new Dictionary<string, OperData>();
            try
            {
                await Wallets.parseMarket();

            }
            catch (Exception ex)
            {
                Log.logDebug("Parse Market Error " + Convert.ToString(ex));
            }
            foreach (string key in Wallets.walletInfo.Keys)
            {
                if (Wallets.walletInfo[key].Length > 0)
                {
                    switch (key)
                    {
                        case "Bittrex":
                            try
                            {
                                var bittrexResponse = JsonConvert.DeserializeObject<BittrexResponse>(await Wallets.sendBittrexRequest("account/getbalances",
                                                Wallets.walletInfo[key][0], Wallets.walletInfo[key][1]));

                                string currence = "";
                                for (int iterMain = 0; iterMain < bittrexResponse.Result.Count(); iterMain++)
                                {
                                    currence = bittrexResponse.Result[iterMain].First.First.ToString();
                                    for (int iter = 0; iter < Wallets.walletInfo[key].Count(); iter += 2)
                                    {
                                        if (iter + 1 < Wallets.walletInfo[key].Count())
                                        {
                                            var bodyBTX = await Wallets.getBittrexOperation(Wallets.walletInfo[key][iter], Wallets.walletInfo[key][iter + 1], currence);
                                            if (bodyBTX.Count != 0)
                                            {
                                                if (!walletsIncome.Keys.Contains(currence))
                                                {
                                                    walletsIncome.Add(currence, 0);
                                                }
                                                for (int innerIter = 0; innerIter < bodyBTX.Count; innerIter++)
                                                {
                                                    try
                                                    {
                                                        if (bodyBTX[innerIter].TxCost == 0 && (DateTime.Now -
                                                            (Convert.ToDateTime(bodyBTX[innerIter].date, CultureInfo.InvariantCulture))).Days < 1)
                                                        {
                                                            walletsIncome[currence] += Math.Round(bodyBTX[innerIter].amount, 8);
                                                            operData.Add(bodyBTX[innerIter].TxId, new OperData
                                                            {
                                                                currency = currence,
                                                                date = Convert.ToDateTime(bodyBTX[innerIter].date, CultureInfo.InvariantCulture),
                                                                sum = Math.Round(bodyBTX[innerIter].amount, 8)
                                                            });
                                                        }
                                                    }
                                                    catch (Exception ex)
                                                    {
                                                        Log.logDebug(Convert.ToString(ex));
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                            catch (Exception ex)
                            {
                                Log.logDebug("Bittrex income Error " + Convert.ToString(ex));
                            }

                            break;
                        case "ETH":
                            try
                            {

                                var bodyETH = await Wallets.getETHOperation(Wallets.walletInfo[key]);
                                if (bodyETH.Count != 0)
                                {
                                    if (!walletsIncome.Keys.Contains("ETH"))
                                    {
                                        walletsIncome.Add("ETH", 0);
                                    }

                                    for (int innerIter = 0; innerIter < bodyETH.Count; innerIter++)
                                    {

                                        if (Wallets.walletInfo[key].Contains(bodyETH[innerIter].to) && DateTime.Now.Subtract
                                            (Wallets.ToDateTimeFromUnix(bodyETH[innerIter].time).ToLocalTime()).Days < 1)
                                        {
                                            walletsIncome["ETH"] += Math.Round(Convert.ToDouble(bodyETH[innerIter].value,
                                CultureInfo.InvariantCulture) / 1000000000000000000, 8);
                                            operData.Add(bodyETH[innerIter].hash, new OperData
                                            {
                                                currency = "ETH",
                                                date = Wallets.ToDateTimeFromUnix(bodyETH[innerIter].time).ToUniversalTime(),
                                                sum = Math.Round(Convert.ToDouble(bodyETH[innerIter].value,
                                CultureInfo.InvariantCulture) / 1000000000000000000, 8)
                                            });

                                        }
                                    }
                                }
                            }
                            catch (Exception ex)
                            {
                                Log.logDebug("ETH Income Error " + Convert.ToString(ex));
                            }
                            break;
                        case "LTC":
                            try
                            {
                                var bodyLTC = await Wallets.getLTCOperation(Wallets.walletInfo[key]);
                                if (bodyLTC.Count != 0)
                                {
                                    if (!walletsIncome.Keys.Contains("LTC"))
                                    {
                                        walletsIncome.Add("LTC", 0);
                                    }
                                    for (int innerIter = 0; innerIter < bodyLTC.Count; innerIter++)
                                    {

                                        if (bodyLTC[innerIter].state && DateTime.Now.Subtract
                                            (Wallets.ToDateTimeFromUnix(bodyLTC[innerIter].time).ToLocalTime()).Days < 1)
                                        {
                                            walletsIncome["LTC"] += Math.Round(Convert.ToDouble(bodyLTC[innerIter].value,
                                CultureInfo.InvariantCulture), 8);
                                            operData.Add(bodyLTC[innerIter].txid, new OperData
                                            {
                                                currency = "LTC",
                                                date = Wallets.ToDateTimeFromUnix(bodyLTC[innerIter].time).ToUniversalTime(),
                                                sum = Math.Round(Convert.ToDouble(bodyLTC[innerIter].value,
                                CultureInfo.InvariantCulture), 8)
                                            });
                                        }
                                    }
                                }
                            }
                            catch (Exception ex)
                            {
                                Log.logDebug("LTC Income Error " + Convert.ToString(ex));
                            }
                            break;
                        case "BTC cold wallet":
                            try
                            {
                                var bodyBTC = await Wallets.getAllBlockchainOperation(Wallets.walletInfo[key]);
                                if (bodyBTC.Count != 0)
                                {
                                    if (!walletsIncome.Keys.Contains("BTC"))
                                    {
                                        walletsIncome.Add("BTC", 0);
                                    }
                                    for (int innerIter = 0; innerIter < bodyBTC.Count; innerIter++)
                                    {
                                        if (bodyBTC[innerIter].result > 0 && DateTime.Now.Subtract
                                            (Wallets.ToDateTimeFromUnix(bodyBTC[innerIter].time).ToLocalTime()).Days < 1)
                                        {
                                            walletsIncome["BTC"] += Math.Round(Convert.ToDouble(bodyBTC[innerIter].result,
                                CultureInfo.InvariantCulture) / 100000000, 8);
                                            operData.Add(bodyBTC[innerIter].hash, new OperData
                                            {
                                                currency = "BTC",
                                                date = Convert.ToDateTime(Wallets.ToDateTimeFromUnix(bodyBTC[innerIter].time).ToUniversalTime(), CultureInfo.InvariantCulture),
                                                sum = Math.Round(Convert.ToDouble(bodyBTC[innerIter].result,
                                CultureInfo.InvariantCulture) / 100000000, 8)
                                            });

                                        }
                                    }
                                }
                            }
                            catch (Exception ex)
                            {
                                Log.logDebug("BTC Income Error " + Convert.ToString(ex));
                            }
                            break;

                    }
                }
            }

            //sentOperationData(operData);
            //getCSVData(false);//Send rs data without creating file

        }
        // Calculate Rest foe FullWalletDate
        public async Task calculateRestAll()
        {
            foreach (string key in Wallets.walletInfo.Keys)
            {
                if (Wallets.walletInfo[key].Length > 0)
                {
                    switch (key)
                    {
                        case "Bittrex":
                            for (int iter = 0; iter < Wallets.walletInfo[key].Count(); iter += 2)
                            {
                                var bittrexResponse = JsonConvert.DeserializeObject<BittrexResponse>(await Wallets.sendBittrexRequest("account/getbalances",
                                    Wallets.walletInfo[key][iter], Wallets.walletInfo[key][iter + 1]));
                                string currence = "";
                                for (int iterMain = 0; iterMain < bittrexResponse.Result.Count(); iterMain++)
                                {
                                    currence = bittrexResponse.Result[iterMain].First.First.ToString();
                                    if (iter + 1 < Wallets.walletInfo[key].Count())
                                    {
                                        calculateRestThisWallet(Wallets.walletInfo[key][iter], Convert.ToDouble(bittrexResponse.Result[iterMain].First.Next.First.ToString(), CultureInfo.InvariantCulture), currence);
                                    }
                                }
                            }
                            break;
                        case "ETH":
                            for (int iter = 0; iter < Wallets.walletInfo[key].Count(); iter++)
                            {
                                Dictionary<String, String> uriArgs = new Dictionary<string, string>();
                                uriArgs.Add("module", "account");
                                uriArgs.Add("action", "balance");
                                uriArgs.Add("address", Wallets.walletInfo[key][iter]);
                                JToken parsedBalance = JsonConvert.DeserializeObject<JToken>(await Wallets.sendAsyncRequest("", Wallets.ethUri, uriArgs));
                                calculateRestThisWallet(Wallets.walletInfo[key][iter], Convert.ToDouble(Math.Round(Convert.ToDecimal(parsedBalance.Last.Last.ToString(),
                                    CultureInfo.InvariantCulture) / 1000000000000000000, 12)), "ETH");
                            }
                            break;
                        case "LTC":
                            for (int iter = 0; iter < Wallets.walletInfo[key].Count(); iter++)
                            {

                                JToken parsedBalance = JsonConvert.DeserializeObject<JToken>(
                                    await Wallets.sendAsyncRequest("address/LTC/" + Wallets.walletInfo[key][iter], Wallets.LTCUri));
                                calculateRestThisWallet(Wallets.walletInfo[key][iter], Convert.ToDouble(parsedBalance.First.Next.Last.First.Next.Next.Last.ToString(), CultureInfo.InvariantCulture), "LTC");
                            }
                            break;
                        case "BTC cold wallet":
                            for (int iter = 0; iter < Wallets.walletInfo[key].Count(); iter++)
                            {
                                calculateRestThisWallet(Wallets.walletInfo[key][iter], Convert.ToDouble(await Wallets.sendAsyncRequest("q/addressbalance/" + Wallets.walletInfo[key][iter],
                                        Wallets.blockChainUri), CultureInfo.InvariantCulture) / 100000000, "BTC");
                            }
                            break;
                        default:
                            break;
                    }
                }
            }
        }
        //
        public void calculateRestThisWallet(string wallet, double balance, string currence)
        {
            for (int iter = fullWalletData.Count - 1; iter >= 0; iter--)
            {
                if (fullWalletData.ElementAt(iter).Value.type == txType.orderBuy || fullWalletData.ElementAt(iter).Value.type == txType.orderSell)
                {
                    if (fullWalletData.ElementAt(iter).Value.Wallet == wallet && currence == "BTC")
                    {
                        fullWalletData.ElementAt(iter).Value.rest = balance;
                        if (fullWalletData.ElementAt(iter).Value.type == txType.orderBuy)
                        {
                            balance = balance + Math.Abs(fullWalletData.ElementAt(iter).Value.value) + Math.Abs(fullWalletData.ElementAt(iter).Value.fee);
                        }
                        if (fullWalletData.ElementAt(iter).Value.type == txType.orderSell)
                        {
                            balance = balance - Math.Abs(fullWalletData.ElementAt(iter).Value.value) + Math.Abs(fullWalletData.ElementAt(iter).Value.fee);
                        }
                    }
                    else
                    {
                        if (fullWalletData.ElementAt(iter).Value.Wallet == wallet
                            && fullWalletData.ElementAt(iter).Value.dopData.Substring(fullWalletData.ElementAt(iter).Value.dopData.IndexOf('-') + 1) == currence)
                        {
                            fullWalletData.ElementAt(iter).Value.restConvert = balance;
                            if (fullWalletData.ElementAt(iter).Value.type == txType.orderBuy)
                            {
                                balance = balance - Math.Abs(fullWalletData.ElementAt(iter).Value.valueConvert);
                            }
                            if (fullWalletData.ElementAt(iter).Value.type == txType.orderSell)
                            {
                                balance = balance + Math.Abs(fullWalletData.ElementAt(iter).Value.valueConvert);
                            }


                        }
                    }
                }
                else
                {
                    if (fullWalletData.ElementAt(iter).Value.Wallet == wallet && fullWalletData.ElementAt(iter).Value.currency == currence)
                    {
                        fullWalletData.ElementAt(iter).Value.rest = balance;
                        if (fullWalletData.ElementAt(iter).Value.type == txType.txD)
                        {
                            balance = balance - Math.Abs(fullWalletData.ElementAt(iter).Value.value);
                        }
                        if (fullWalletData.ElementAt(iter).Value.type == txType.txW)
                        {
                            balance = balance + Math.Abs(fullWalletData.ElementAt(iter).Value.value);
                        }


                    }
                }
            }
        }
        //Gets data for csv
        public static async Task getFullWalletDate(object obj = null)
        {
            Settings.fullWalletInfoState = true;
            try
            {
                fullWalletData = new SortedDictionary<DateTime, CSVData>();
                await Wallets.parseMarket();
                foreach (string key in Wallets.walletInfo.Keys)
                {
                    Settings.fullWalletInfoState = true;
                    if (Wallets.walletInfo[key].Length > 0)
                    {
                        switch (key)
                        {
                            case "Bittrex":

                                for (int iter = 0; iter < Wallets.walletInfo[key].Count(); iter += 2)
                                {
                                    var bittrexResponse = JsonConvert.DeserializeObject<BittrexResponse>(await Wallets.sendBittrexRequest("account/getbalances",
                                        Wallets.walletInfo[key][iter], Wallets.walletInfo[key][iter + 1]));
                                    string currence = "";
                                    for (int iterMain = 0; iterMain < bittrexResponse.Result.Count(); iterMain++)
                                    {
                                        currence = bittrexResponse.Result[iterMain].First.First.ToString();
                                        if (iter + 1 < Wallets.walletInfo[key].Count())
                                        {
                                            var bodyBTX = await Wallets.getBittrexOperation(Wallets.walletInfo[key][iter], Wallets.walletInfo[key][iter + 1], currence);
                                            if (bodyBTX.Count != 0)
                                            {
                                                for (int innerIter = 0; innerIter < bodyBTX.Count; innerIter++)
                                                {
                                                    DateTime operationTime = Convert.ToDateTime(bodyBTX[innerIter].date, CultureInfo.InvariantCulture);
                                                    while (fullWalletData.Keys.Contains(operationTime))
                                                    {
                                                        //CSVData test = result[operationTime];
                                                        //test.fee = 0;
                                                        operationTime = operationTime.AddSeconds(1);
                                                    }
                                                    if (bodyBTX[innerIter].TxCost == 0)
                                                    {
                                                        fullWalletData.Add(operationTime,
                                                            new CSVData
                                                            {
                                                                Wallet = Wallets.walletInfo["Bittrex"][iter],
                                                                Descrtion = Wallets.walletInfoDescription[key][iter],
                                                                fee = bodyBTX[innerIter].TxCost,
                                                                id = bodyBTX[innerIter].TxId,
                                                                income = 0,
                                                                type = txType.txD,
                                                                usdRate = 0,
                                                                value = Math.Round(bodyBTX[innerIter].amount, 8),
                                                                valueConvert = 0,
                                                                dopData = currence + " mining",
                                                                currency = currence,
                                                                walletType = "C"
                                                            });

                                                    }
                                                    else
                                                    {
                                                        fullWalletData.Add(operationTime,
                                                                   new CSVData
                                                                   {
                                                                       Wallet = Wallets.walletInfo["Bittrex"][iter],
                                                                       Descrtion = Wallets.walletInfoDescription[key][iter],
                                                                       fee = bodyBTX[innerIter].TxCost,
                                                                       id = bodyBTX[innerIter].TxId,
                                                                       income = 0,
                                                                       type = txType.txW,
                                                                       usdRate = 0,
                                                                       value = Math.Round(-1 * bodyBTX[innerIter].amount, 8),
                                                                       valueConvert = 0,
                                                                       dopData = currence + " Withdraw",
                                                                       currency = currence,
                                                                       walletType = "C"
                                                                   });
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }

                                break;
                            case "ETH":
                                for (int iter = 0; iter < Wallets.walletInfo[key].Count(); iter++)
                                {
                                    var bodyETH = await Wallets.getETHOperation(Wallets.walletInfo[key][iter]);
                                    if (bodyETH.Count != 0)
                                    {

                                        for (int innerIter = 0; innerIter < bodyETH.Count; innerIter++)
                                        {
                                            DateTime operationTime = Wallets.ToDateTimeFromUnix(bodyETH[innerIter].time).ToUniversalTime();
                                            while (fullWalletData.Keys.Contains(operationTime))
                                            {
                                                //CSVData test = result[operationTime];
                                                //test.fee = 0;
                                                operationTime = operationTime.AddSeconds(1);
                                            }
                                            if (Wallets.walletInfo[key].Contains(bodyETH[innerIter].to))
                                            {
                                                double temp = Math.Round(Convert.ToDouble((bodyETH[innerIter].gasUsed * bodyETH[innerIter].gasPrice) / 1000000000), 12);
                                                bodyETH[innerIter].fee = temp / 1000000000;
                                                fullWalletData.Add(operationTime,
                                                    new CSVData
                                                    {
                                                        Wallet = Wallets.walletInfo[key][iter],
                                                        Descrtion = Wallets.walletInfoDescription[key][iter],
                                                        fee = 0,
                                                        id = bodyETH[innerIter].hash,
                                                        income = 0,
                                                        type = txType.txD,
                                                        usdRate = 0,
                                                        value = Math.Round(Convert.ToDouble(bodyETH[innerIter].value,
                                                        CultureInfo.InvariantCulture) / 1000000000000000000, 12),
                                                        valueConvert = 0,
                                                        dopData = "ETH mining",
                                                        currency = "ETH",
                                                        walletType = "C"
                                                    });

                                            }
                                            else
                                            {
                                                double temp = Math.Round(Convert.ToDouble((bodyETH[innerIter].gasUsed * bodyETH[innerIter].gasPrice) / 1000000000), 12);
                                                bodyETH[innerIter].fee = temp / 1000000000;
                                                fullWalletData.Add(operationTime,
                                                           new CSVData
                                                           {
                                                               Wallet = Wallets.walletInfo[key][iter],
                                                               Descrtion = Wallets.walletInfoDescription[key][iter],
                                                               fee = bodyETH[innerIter].fee,
                                                               id = bodyETH[innerIter].hash,
                                                               income = 0,
                                                               type = txType.txW,
                                                               usdRate = 0,
                                                               value = -1 * Math.Round(Convert.ToDouble(bodyETH[innerIter].value,
                                                               CultureInfo.InvariantCulture) / 1000000000000000000, 12) - bodyETH[innerIter].fee,
                                                               valueConvert = 0,
                                                               dopData = "ETH Withdraw",
                                                               currency = "ETH",
                                                               walletType = "C"
                                                           });
                                            }
                                        }
                                    }
                                }
                                break;
                            case "LTC":
                                for (int iter = 0; iter < Wallets.walletInfo[key].Count(); iter++)
                                {
                                    var bodyLTC = await Wallets.getLTCOperation(Wallets.walletInfo[key][iter]);
                                    if (bodyLTC.Count != 0)
                                    {
                                        for (int innerIter = 0; innerIter < bodyLTC.Count; innerIter++)
                                        {

                                            DateTime operationTime = Wallets.ToDateTimeFromUnix(bodyLTC[innerIter].time).ToUniversalTime();
                                            while (fullWalletData.Keys.Contains(operationTime))
                                            {
                                                //CSVData test = result[operationTime];
                                                //test.fee = 0;
                                                operationTime = operationTime.AddSeconds(1);
                                            }
                                            if (bodyLTC[innerIter].state)
                                            {
                                                fullWalletData.Add(operationTime,
                                                    new CSVData
                                                    {
                                                        Wallet = Wallets.walletInfo[key][iter],
                                                        Descrtion = Wallets.walletInfoDescription[key][iter],
                                                        fee = 0,
                                                        id = bodyLTC[innerIter].txid,
                                                        income = 0,
                                                        type = txType.txD,
                                                        usdRate = 0,
                                                        value = Math.Round(Convert.ToDouble(bodyLTC[innerIter].value,
                                                        CultureInfo.InvariantCulture), 8),
                                                        valueConvert = 0,
                                                        dopData = "LTC mining",
                                                        currency = "LTC",
                                                        walletType = "C"
                                                    });

                                            }
                                            else
                                            {
                                                fullWalletData.Add(operationTime,
                                                           new CSVData
                                                           {
                                                               Wallet = Wallets.walletInfo[key][iter],
                                                               Descrtion = Wallets.walletInfoDescription[key][iter],
                                                               fee = bodyLTC[innerIter].fee,
                                                               id = bodyLTC[innerIter].txid,
                                                               income = 0,
                                                               type = txType.txW,
                                                               usdRate = 0,
                                                               value = -1 * Math.Round(Convert.ToDouble(bodyLTC[innerIter].value,
                                                               CultureInfo.InvariantCulture), 8),
                                                               valueConvert = 0,
                                                               dopData = "LTC Withdraw",
                                                               currency = "LTC",
                                                               walletType = "C"
                                                           });
                                            }
                                        }
                                    }
                                }
                                break;
                            case "BTC cold wallet":
                                for (int iter = 0; iter < Wallets.walletInfo[key].Count(); iter++)
                                {
                                    var bodyBTC = await Wallets.getBlockchainOperation(Wallets.walletInfo[key][iter]);
                                    if (bodyBTC.Count != 0)
                                    {
                                        for (int innerIter = 0; innerIter < bodyBTC.Count; innerIter++)
                                        {
                                            DateTime operationTime = Wallets.ToDateTimeFromUnix(bodyBTC[innerIter].time).ToUniversalTime();
                                            while (fullWalletData.Keys.Contains(operationTime))
                                            {
                                                //CSVData test = result[operationTime];
                                                //test.fee = 0;
                                                operationTime = operationTime.AddSeconds(1);
                                            }
                                            if (bodyBTC[innerIter].result > 0)
                                            {
                                                fullWalletData.Add(operationTime,
                                                    new CSVData
                                                    {
                                                        Wallet = Wallets.walletInfo[key][iter],
                                                        Descrtion = Wallets.walletInfoDescription[key][iter],
                                                        fee = 0,
                                                        id = bodyBTC[innerIter].hash,
                                                        income = 0,
                                                        type = txType.txD,
                                                        usdRate = 0,
                                                        value = Math.Round(Convert.ToDouble(bodyBTC[innerIter].result,
                                                        CultureInfo.InvariantCulture) / 100000000, 8),
                                                        valueConvert = 0,
                                                        dopData = "BTC mining",
                                                        currency = "BTC",
                                                        walletType = "C"
                                                    });

                                            }
                                            else
                                            {
                                                fullWalletData.Add(operationTime,
                                                           new CSVData
                                                           {
                                                               Wallet = Wallets.walletInfo[key][iter],
                                                               Descrtion = Wallets.walletInfoDescription[key][iter],
                                                               fee = Math.Round(Convert.ToDouble(bodyBTC[innerIter].fee,
                                                        CultureInfo.InvariantCulture) / 100000000, 8),
                                                               id = bodyBTC[innerIter].hash,
                                                               income = 0,
                                                               type = txType.txW,
                                                               usdRate = 0,
                                                               value = Math.Round(Convert.ToDouble(bodyBTC[innerIter].result,
                                                               CultureInfo.InvariantCulture) / 100000000, 8),
                                                               valueConvert = 0,
                                                               dopData = "BTC Withdraw",
                                                               currency = "BTC",
                                                               walletType = "C"
                                                           });
                                            }
                                        }
                                    }
                                }
                                break;

                        }
                    }
                }
                for (int iter = 0; iter < Wallets.walletInfo["Bittrex"].Count(); iter += 2)
                {
                    if (iter + 1 < Wallets.walletInfo["Bittrex"].Count())
                    {
                        await Wallets.parseOrderHistory(Wallets.walletInfo["Bittrex"][iter], Wallets.walletInfo["Bittrex"][iter + 1]);
                        if (Wallets.orderHistory.Count != 0)
                        {
                            for (int innerIter = 0; innerIter < Wallets.orderHistory.Count; innerIter++)
                            {
                                DateTime operationTime = Convert.ToDateTime(Wallets.orderHistory[innerIter].Closed, CultureInfo.InvariantCulture);
                                while (fullWalletData.Keys.Contains(operationTime))
                                {
                                    //CSVData test = result[operationTime];
                                    //test.fee = 0;
                                    operationTime = operationTime.AddSeconds(1);
                                }
                                if (Wallets.orderHistory[innerIter].OrderType.Contains("SELL"))
                                {

                                    fullWalletData.Add(operationTime,
                                              new CSVData
                                              {
                                                  Wallet = Wallets.walletInfo["Bittrex"][iter],
                                                  Descrtion = "Test",
                                                  fee = Math.Round(Convert.ToDouble(Wallets.orderHistory[innerIter].Commission, CultureInfo.InvariantCulture), 8),
                                                  id = Wallets.orderHistory[innerIter].OrderUuid,
                                                  income = 0,
                                                  type = txType.orderSell,
                                                  usdRate = 0,
                                                  value = Math.Round(Convert.ToDouble(Wallets.orderHistory[innerIter].Price, CultureInfo.InvariantCulture), 8),
                                                  valueConvert = Math.Round(Convert.ToDouble(Wallets.orderHistory[innerIter].Quantity, CultureInfo.InvariantCulture), 8),
                                                  dopData = Wallets.orderHistory[innerIter].Exchange,
                                                  currency = Wallets.orderHistory[innerIter].Exchange.Substring
                                                  (Wallets.orderHistory[innerIter].Exchange.IndexOf('-') + 1),
                                                  walletType = "H"
                                              });

                                }
                                else
                                {
                                    fullWalletData.Add(operationTime,
                                                    new CSVData
                                                    {
                                                        Wallet = Wallets.walletInfo["Bittrex"][iter],
                                                        Descrtion = "Test",
                                                        fee = Math.Round(Convert.ToDouble(Wallets.orderHistory[innerIter].Commission, CultureInfo.InvariantCulture), 8),
                                                        id = Wallets.orderHistory[innerIter].OrderUuid,
                                                        income = 0,
                                                        type = txType.orderBuy,
                                                        usdRate = 0,
                                                        value = Math.Round(Convert.ToDouble(Wallets.orderHistory[innerIter].Price, CultureInfo.InvariantCulture), 8),
                                                        valueConvert = Math.Round(Convert.ToDouble(Wallets.orderHistory[innerIter].Quantity, CultureInfo.InvariantCulture), 8),
                                                        dopData = Wallets.orderHistory[innerIter].Exchange,
                                                        currency = Wallets.orderHistory[innerIter].Exchange.Substring
                                                        (Wallets.orderHistory[innerIter].Exchange.IndexOf('-') + 1),
                                                        walletType = "H"
                                                    });
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Log.logDebug("Wallet Data " + Convert.ToString(ex));
            }

            Settings.fullWalletInfoState = false;



        }
        public static async Task GetUsdRate(object obj = null)
        {
            if (Settings.fullWalletInfoState != true)
            {
                try
                {
                    foreach (DateTime date in fullWalletData.Keys)
                    {

                        if (fullWalletData[date].type != txType.orderBuy && fullWalletData[date].type != txType.orderSell)
                        {
                            string header = fullWalletData[date].currency + ":" + Wallets.ToUnixFromDateTime(date).ToString() + "000";
                            if (!Wallets.historicalExcRate.Keys.Contains(header))
                            {
                                await Wallets.getUSDrateDate(Wallets.ToUnixFromDateTime(date) * 1000, fullWalletData[date].currency);
                            }
                        }
                        else
                        {
                            try
                            {
                                string header = fullWalletData[date].dopData.Substring(fullWalletData[date].dopData.IndexOf('-') + 1)
                                    + ":" + Wallets.ToUnixFromDateTime(date).ToString() + "000";
                                if (!Wallets.historicalExcRate.Keys.Contains(header))
                                {
                                    await Wallets.getUSDrateDate(Wallets.ToUnixFromDateTime(date) * 1000, fullWalletData[date].dopData.Substring(fullWalletData[date].dopData.IndexOf('-')));
                                }
                                header = fullWalletData[date].dopData.Remove(fullWalletData[date].dopData.IndexOf('-'))
                                    + ":" + Wallets.ToUnixFromDateTime(date).ToString() + "000";
                                if (!Wallets.historicalExcRate.Keys.Contains(header))
                                {
                                    await Wallets.getUSDrateDate(Wallets.ToUnixFromDateTime(date) * 1000, fullWalletData[date].dopData.Remove(fullWalletData[date].dopData.IndexOf('-')));
                                }
                            }
                            catch (Exception ex)
                            {
                                Log.logDebug("Usd Rate For Orders:" + Convert.ToString(ex));
                            }
                        }
                    }
                    Wallets.saveHistoricalInFile();
                }
                catch (Exception) { }
            }

        }
        #endregion

    }

}
