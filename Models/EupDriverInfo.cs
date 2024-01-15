using Dapper;
using Newtonsoft.Json;
using System.Data.SqlClient;
using System.Net;
using System.Net.Http;
using System.Text;
using static EupDriversInfo.Models.EupDriverInfo.DriverInfoobj;

namespace EupDriversInfo.Models
{
    public class EupDriverInfo : BaseDrv
    {
        public EupDriverInfo(IConfiguration config) : base(config)
        {
            AccessToken = _config["EupInfo:EupToken"];
            string ApiServerUrl = _config["EupInfo:APServerUrl"];
            ApiUrl = $"{ApiServerUrl}{_config["EupInfo:SessionIdAction"]}";
            DriversinfoAPI = $"{ApiServerUrl}{_config["EupInfo:DriversinfoAction"]}";

        }
        public string? ApiUrl { get; set; }
        public string? AccessToken { get; set; }
        public string? SessionID { get; set; }
        public string? DriversinfoAPI { get; set; }
        public int DriversCnt { get; set; }

        /// <summary>
        /// 登入
        /// </summary>
        public class SessionIDobj
        {
            /// <summary>
            /// 訊息
            /// </summary>
            public string? responseMsg { get; set; }
            /// <summary>
            /// 更新失敗結果(僅在新增/修改/刪除時有用)
            /// </summary>
            public SessionID_Item? result { get; set; }
            /// <summary>
            /// 更新失敗結果(僅在新增/修改/刪除時有用)
            /// </summary>
            public string? failResult { get; set; }
            /// <summary>
            /// integer<int32>
            /// </summary>
            public int responseStatus { get; set; }

            public class SessionID_Item
            {
                /// <summary>
                /// token
                /// </summary>
                public string? token { get; set; }
                /// <summary>
                /// sessionId
                /// </summary>
                public string? sessionId { get; set; }
            }
        }

        /// <summary>
        /// 駕駛資訊
        /// </summary>
        public class DriverInfoobj
        {
            /// <summary>
            /// 訊息
            /// </summary>
            public string? responseMsg { get; set; }
            /// <summary>
            /// 更新失敗結果(僅在新增/修改/刪除時有用)
            /// </summary>
            public List<DriverInfo_Item> result { get; set; }
            /// <summary>
            /// 更新失敗結果(僅在新增/修改/刪除時有用)
            /// </summary>
            public string? failResult { get; set; }
            /// <summary>
            /// integer<int32>
            /// </summary>
            public int responseStatus { get; set; }

            public class DriverInfo_Item
            {
                /// <summary>
                /// 駕駛名稱
                /// </summary>
                public string? driverName { get; set; }
                /// <summary>
                /// 登入帳號
                /// </summary>
                public string? account { get; set; }
                /// <summary>
                /// 最後登入時間
                /// </summary>
                public DateTime? lastLoginTime { get; set; }
            }
        }

        /// <summary>
        /// Dapper ORM
        /// </summary>
        public class DriverData
        {
            /// <summary>
            /// 駕駛名稱
            /// </summary>
            public string? driverName { get; set; }
            /// <summary>
            /// 登入帳號
            /// </summary>
            public string? account { get; set; }
            /// <summary>
            /// 最後登入時間
            /// </summary>
            public DateTime? lastLoginTime { get; set; }
        }

        /// <summary>
        /// 以Token登入取得SESSION_ID
        /// </summary>
        public void GetSESSION_ID()
        {
            string SionIdRsqMsg = "";
            HttpStatusCode SionIdRspStatusCode = HttpStatusCode.InternalServerError;
            try
            {
                ApiUrl += "?token=" + AccessToken;
                HttpWebRequest? SionIdRsq = WebRequest.Create(ApiUrl) as HttpWebRequest;
                SionIdRsq.Method = "POST";
                SionIdRsq.Timeout = 1000 * 60;

                byte[] paramData = Encoding.UTF8.GetBytes("");
                SionIdRsq.ContentLength = paramData.Length;

                SionIdRsq.ContentType = "application/json";

                // 發送請求並獲取響應
                using (HttpWebResponse? SionIdRsp = SionIdRsq.GetResponse() as HttpWebResponse)
                {
                    SionIdRspStatusCode = SionIdRsp.StatusCode;
                    using (StreamReader sr = new StreamReader(SionIdRsp.GetResponseStream()))
                    {
                        SionIdRsqMsg = sr.ReadToEnd();
                    }
                }

                SessionIDobj? _SessionID = JsonConvert.DeserializeObject<SessionIDobj>(SionIdRsqMsg);
                if (SionIdRspStatusCode == HttpStatusCode.OK && _SessionID.responseMsg == "SUCCESS")
                {
                    this.SessionID = _SessionID.result.sessionId;
                }
            }
            catch
            {
                throw;
            }

        }

        public bool GetDriversInfo()
        {
            bool result = false;
            string SionIdRsqMsg = "";
            HttpStatusCode SionIdRspStatusCode = HttpStatusCode.InternalServerError;

            try
            {

                GetSESSION_ID(); //取得SESSIONID授權

                if (string.IsNullOrEmpty(DriversinfoAPI))
                {
                    return false;
                }

                HttpWebRequest? request = WebRequest.Create(DriversinfoAPI) as HttpWebRequest;
                request.Method = "Get";
                request.Timeout = 1000 * 60; //逾時放寬至 2分鐘;

                WebHeaderCollection Header2Collection = request.Headers;
                Header2Collection.Add("Authorization", "Bearer " + SessionID); //Bearer Token 表頭帶入
                //byte[] paramData = Encoding.UTF8.GetBytes("");
                //ionIdRsq.ContentLength = paramData.Length;
                //SionIdRsq.ContentType = "application/json";

                // 發送請求並獲取響應
                using (HttpWebResponse? SionIdRsp = request.GetResponse() as HttpWebResponse)
                {
                    SionIdRspStatusCode = SionIdRsp.StatusCode;
                    using (StreamReader sr = new StreamReader(SionIdRsp.GetResponseStream()))
                    {
                        SionIdRsqMsg = sr.ReadToEnd();
                    }
                }

                DriverInfoobj? _DriverInfoobj = JsonConvert.DeserializeObject<DriverInfoobj>(SionIdRsqMsg);
                if (SionIdRspStatusCode == HttpStatusCode.OK && _DriverInfoobj.responseMsg == "SUCCESS")
                {
                    var Drvs = new List<DriverData>();
                    //ToDo
                    #region dapper 多筆寫入
                    foreach (var Item in _DriverInfoobj.result)
                    {
                        //資料處理
                        var Convertdata = ConvertDrvData(Item);
                        Drvs.Add(Convertdata);
                    }

                    using SqlConnection conn = new(_connectString);

                    var sql = @"
                                DBCC CHECKIDENT ('Driver', RESEED, 0);
                                DELETE FROM dbo.Driver;
                              ";
                    var delCnt = conn.Execute(sql);

                    if (delCnt >= 0)
                    {
                        sql =
                            @"
                                INSERT INTO dbo.Driver
                                (
                                    driverName,
                                    account,
                                    lastLoginTime
                                )
                                VALUES
                                ( 
                                    @driverName,
                                    @account,
                                    @lastLoginTime
                                );
                            ";
                        var execnum = conn.Execute(sql, Drvs);
                        this.DriversCnt = execnum;
                    }

                    #endregion
                }
            }
            catch
            {
                throw;
            }

            return result;
        }

        public DriverData ConvertDrvData(DriverInfo_Item DrvItem)
        {
            return new DriverData()
            {
                driverName = DrvItem.driverName,
                account = DrvItem.account?.Length < 6 ? DrvItem.account.PadLeft(6, '0') : DrvItem.account,
                lastLoginTime = DrvItem.lastLoginTime != null ? DrvItem.lastLoginTime : DateTime.Now,
            };
        }
    }
}
