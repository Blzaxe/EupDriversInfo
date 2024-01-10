using Newtonsoft.Json;
using System.Net;
using System.Net.Http;
using System.Text;

namespace EupDriversInfo.Models
{
    public class EupDriverInfo
    {
        public string? ApiUrl { get; set; }
        public string? AccessToken { get; set; }
        public string? SessionID { get; set; }

        /// <summary>
        /// 登入
        /// </summary>
        public class SessionIDobj
        {
            /// <summary>
            /// 訊息
            /// </summary>
            public string responseMsg { get; set; }
            /// <summary>
            /// 更新失敗結果(僅在新增/修改/刪除時有用)
            /// </summary>
            public SessionID_Item result { get; set; }
            /// <summary>
            /// 更新失敗結果(僅在新增/修改/刪除時有用)
            /// </summary>
            public string failResult { get; set; }
            /// <summary>
            /// integer<int32>
            /// </summary>
            public int responseStatus { get; set; }

            public class SessionID_Item
            {
                /// <summary>
                /// token
                /// </summary>
                public string token { get; set; }
                /// <summary>
                /// sessionId
                /// </summary>
                public string sessionId { get; set; }
            }
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

        /// <summary>
        /// 以Token登入取得SESSION_ID
        /// </summary>
        public void GetSESSION_ID_V2()
        {
            string SionIdRsqMsg = "";
            HttpStatusCode SionIdRspStatusCode = HttpStatusCode.InternalServerError;
            try
            {
                ApiUrl += "?token=" + AccessToken;

                HttpClient _httpClient = new ();

                HttpWebRequest? SionIdRsq = WebRequest.Create(ApiUrl) as HttpWebRequest;
                SionIdRsq.Method = "POST";
                SionIdRsq.Timeout = 1000 * 60;

                byte[] paramData = Encoding.UTF8.GetBytes("");
                SionIdRsq.ContentLength = paramData.Length;

                SionIdRsq.ContentType = "application/json; charset=utf-8";

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
    }
}
