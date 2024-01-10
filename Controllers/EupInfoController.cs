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

        public EupInfoController(ILogger<HomeController> logger)
        {
            _logger = logger;
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
                string EupToken = "68a09f1f-84d2-4dc0-a369-d985df0b63fc";
                string ApiServerUrl = "https://slt.eup.tw:8444/Eup_Servlet_API_SOAP";
                string SessionIdAPI = $"{ApiServerUrl}/login/session";
                string DriversWorkHourAPI = $"{ApiServerUrl}/drivers/info";
                var model = new EupDriverInfo()
                {
                    AccessToken = EupToken,
                    ApiUrl = SessionIdAPI,
                };
                model.GetSESSION_ID();

                Result.result = true;
                Result.message = $"資料已寫入{model.SessionID}";
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
            return View();
        }

        [HttpPost]
        public IActionResult Search(string a)
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}