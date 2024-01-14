using static EupDriversInfo.Models.EupDriverInfo.DriverInfoobj;
using static EupDriversInfo.Models.EupDriverInfo;

namespace EupDriversInfo.Models
{
    public class DriverSearch : BaseDrv
    {
        public DriverSearch(IConfiguration config) : base(config) { }
        public string? driverName { get; set; }

        public string? account { get; set; }
        public List<DriverItem>? DataList { get; set; }

        public class DriverItem
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

        public bool GetDrvs(DriverSearch IDriverSearch)
        {
            return true;
        }
    }
}
