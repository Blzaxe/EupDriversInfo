using static EupDriversInfo.Models.EupDriverInfo.DriverInfoobj;
using static EupDriversInfo.Models.EupDriverInfo;
using System.Data.SqlClient;
using Dapper;
using static EupDriversInfo.Models.DriverSearch;
using System.Security.Principal;

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
                var sqlwhere = "WHERE 1 = 1";
                Dictionary<string, object> dict = new ();               
                if (!string.IsNullOrWhiteSpace(IDriverSearch.account))
                {
                    sqlwhere += Environment.NewLine + "AND account Like @Account";
                    dict.Add("Account", $"{IDriverSearch.account}%");
                }
                if (!string.IsNullOrWhiteSpace(IDriverSearch.driverName))
                {
                    sqlwhere += Environment.NewLine + "AND driverName Like @DriverName";
                    dict.Add("driverName", $"%{IDriverSearch.driverName}%");
                }
                var parameters = new DynamicParameters(dict);
                using SqlConnection conn = new(_connectString);

                var sql = "SELECT * FROM dbo.Driver";

                sql += Environment.NewLine + sqlwhere;
                IResult = conn.Query<DriverSearch.DriverItem>(sql, parameters).ToList();
            }
            catch (Exception ex)
            {

            }

            return IResult;
        }

        public DriverItem GetDrv(int id)
        {
            DriverItem IResult = new();

            try
            {
                using SqlConnection conn = new(_connectString);

                var sql = "SELECT * FROM dbo.Driver WHERE id = @id;";
                IResult = conn.QuerySingle<DriverItem>(sql, new { id });
            }
            catch (Exception ex)
            {

            }

            return IResult;
        }
    }
}
