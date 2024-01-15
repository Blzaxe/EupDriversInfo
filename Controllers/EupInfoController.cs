using EupDriversInfo.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Net;
using System.Net.Mime;
using System.Reflection;

namespace EupDriversInfo.Controllers
{
    public class EupInfoController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IConfiguration _config;

        public EupInfoController(ILogger<HomeController> logger, IConfiguration config)
        {
            _logger = logger;
            _config = config;
        }

        [HttpGet]
        public IActionResult Index()
        {
            var Result = new ResultModel()
            {
                result = false,
                message = "等待呼叫中"
            };
            return View(Result);
        }

        [HttpGet]
        public IActionResult InsertData()
        {
            var Result = new ResultModel()
            {
                result = false,
                message = string.Empty
            };

            try
            {
                //呼叫外部api
                //string EupToken = "68a09f1f-84d2-4dc0-a369-d985df0b63fc";
                //string ApiServerUrl = "https://slt.eup.tw:8444/Eup_Servlet_API_SOAP";
                //string SessionIdAPI = $"{ApiServerUrl}/login/session";
                //string DriversinfoAPI = $"{ApiServerUrl}/drivers/info";
                //var model = new EupDriverInfo(_config)
                //{
                //    AccessToken = EupToken,
                //    ApiUrl = SessionIdAPI,
                //    DriversinfoAPI = DriversinfoAPI
                //};

                var model = new EupDriverInfo(_config) { };
                if (model.GetDriversInfo())
                {
                    Response.StatusCode = (int)HttpStatusCode.BadRequest;
                    Result.result = false;
                    Result.message = $"資料載入失敗";
                }
                else
                {
                    Result.result = true;
                    Result.message = $"資料已寫入 {model.DriversCnt} 筆";
                }


            }
            catch (Exception ex)
            {
                Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                Result.result = false;
                Result.message = ex.Message.ToString();
            }

            Response.ContentType = "application/json; charset=utf-8";
            return Json(Result);
        }

        [HttpGet]
        public IActionResult Search()
        {
            DriverSearch IDriverSearch = new()
            {
                driverName = "我是司機",
                account = "AABBCC",
                DataList = new()
            };

            return View("~/Views/EupInfo/Search.cshtml", IDriverSearch);
        }

        [HttpPost]
        public IActionResult Search(DriverSearch IDriverSearch)
        {
            Drivers DrvMethod = new(_config);
            IDriverSearch.DataList = DrvMethod.GetDrvs(IDriverSearch);
            return View(IDriverSearch);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}