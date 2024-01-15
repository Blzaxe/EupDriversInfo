namespace EupDriversInfo.Models
{
    public class BaseDrv
    {
        /// <summary>
        /// 連線字串
        /// </summary>
        //public readonly string _connectString = "Server=ITTC-04509-0050\\SQLEXPRESS;Database=EupData;User ID=sa;Password=ssmsLion220090;";
        public readonly string _connectString = string.Empty;

        public readonly IConfiguration _config;
        public BaseDrv(IConfiguration config)
        {
            _config = config;
            _connectString = _config["DBConfig:ConnectionStrings"];
        }
    }
}
