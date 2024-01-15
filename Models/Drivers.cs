using static EupDriversInfo.Models.EupDriverInfo.DriverInfoobj;
using static EupDriversInfo.Models.EupDriverInfo;
using System.Data.SqlClient;
using Dapper;

namespace EupDriversInfo.Models
{
    public class Drivers : BaseDrv
    {
        public Drivers(IConfiguration config) : base(config) { }

        public List<DriverSearch.DriverItem> GetDrvs(DriverSearch IDriverSearch)
        {
            var IResult = new List<DriverSearch.DriverItem>();

            try
            {
                using SqlConnection conn = new(_connectString);

                var sql = "SELECT * FROM dbo.Driver;";
                IResult = conn.Query<DriverSearch.DriverItem>(sql).ToList();
            }
            catch (Exception ex)
            {

            }

            return IResult;
        }
    }
}
