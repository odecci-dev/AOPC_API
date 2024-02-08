﻿using AuthSystem.Manager;
using AuthSystem.Models;
using AuthSystem.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System;
using System.Data;
using System.Reflection;
using System.IO;
using AuthSystem.Data;
using AuthSystem.Data.Class;
using Newtonsoft.Json.Linq;
using Microsoft.Data.SqlClient;
using AuthSystem.ViewModel;
using static AuthSystem.Data.Controller.ApiRegisterController;
using System.Web.Http.Results;
using MimeKit;
using MailKit.Net.Smtp;
using System.Text;
using static AuthSystem.Data.Controller.ApiVendorController;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using static AuthSystem.Data.Controller.ApiUserAcessController;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
using static System.Net.Mime.MediaTypeNames;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Diagnostics.Metrics;
using System.Globalization;
using Newtonsoft.Json;

namespace AuthSystem.Data.Controller
{
    //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [Authorize("ApiKey")]
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class ApiSupportController : ControllerBase
    {
        DbManager db = new DbManager();
        private readonly AppSettings _appSettings;
        private ApplicationDbContext _context;
        private ApiGlobalModel _global = new ApiGlobalModel();
        private readonly JwtAuthenticationManager jwtAuthenticationManager;
        private readonly IWebHostEnvironment _environment;
        private readonly ILogger<ApiUserAcessController> _logger;
        public ApiSupportController(IOptions<AppSettings> appSettings, ApplicationDbContext context, ILogger<ApiUserAcessController> logger,
        JwtAuthenticationManager jwtAuthenticationManager, IWebHostEnvironment environment)
        {

            _context = context;
            _appSettings = appSettings.Value;
            _logger = logger;
            this.jwtAuthenticationManager = jwtAuthenticationManager;

        }


        [HttpGet]
        public async Task<IActionResult> GetSupportCountList()
        {
            GlobalVariables gv = new GlobalVariables();

            string sql = $@"SELECT COUNT(*) AS SuppportCnt FROM tbl_SupportModel INNER JOIN tbl_StatusModel ON tbl_SupportModel.Status = tbl_StatusModel.Id WHERE 
                         (tbl_SupportModel.Status = 14)";
            DataTable dt = db.SelectDb(sql).Tables[0];
            var result = new List<SupportModel>();
            foreach (DataRow dr in dt.Rows)
            {
                var item = new SupportModel();
                item.Supportcount = int.Parse(dr["SuppportCnt"].ToString());
                result.Add(item);
            }

            return Ok(result);
        }

        [HttpGet]
        public async Task<IActionResult> GetClickCountsListAll()
        {
            GlobalVariables gv = new GlobalVariables();

            string sql = $@"SELECT Business, Count(*) as count FROM tbl_audittrailModel
                         WHERE Actions LIKE '%view%'  and Module ='news' and Business <> '' GROUP BY Business order by count desc";
            DataTable dt = db.SelectDb(sql).Tables[0];
            var result = new List<ClicCountModel>();
            foreach (DataRow dr in dt.Rows)
            {
                var item = new ClicCountModel();
                item.Module = dr["Business"].ToString();
                item.Count = int.Parse(dr["count"].ToString());
                result.Add(item);
            }

            return Ok(result);
        }
        [HttpGet]
        public async Task<IActionResult> GetMostCickStoreList()
        {
            GlobalVariables gv = new GlobalVariables();

            string sql = $@"SELECT     Count(*)as count, Actions,Business,Module,tbl_audittrailModel.DateCreated
                         FROM         tbl_audittrailModel  WHERE Actions LIKE '%View%' and module ='Shops & Services' and  tbl_audittrailModel.DateCreated >= DATEADD(day,-7, GETDATE())
                         GROUP BY    Actions,Business,Module,tbl_audittrailModel.DateCreated order by count desc";
            DataTable dt = db.SelectDb(sql).Tables[0];
            var result = new List<MostClickStoreModel>();
            int total = 0;
            if (dt.Rows.Count != 0)
            {
                foreach (DataRow dr in dt.Rows)
                {
                    total += int.Parse(dr["count"].ToString());
                }
                foreach (DataRow dr in dt.Rows)
                {
                    var item = new MostClickStoreModel();
                    item.Actions = dr["Actions"].ToString();
                    item.Business = dr["Business"].ToString();
                    item.Module = dr["Module"].ToString();
                    item.DateCreated = DateTime.Parse(dr["DateCreated"].ToString()).ToString("MM-dd-yyyy");
                    item.count = int.Parse(dr["count"].ToString());
                    double val1 = double.Parse(dr["count"].ToString());
                    double val2 = double.Parse(total.ToString());

                    double results = val1 / val2 * 100;
                    item.Total = Math.Round(results, 2);
                    result.Add(item);
                }
            }
            else
            {
                for (int x = 0; x < 4; x++)
                {
                    var item = new MostClickStoreModel();
                    item.Actions = "No Data";
                    item.Business = "No Data";
                    item.Module = "No Data";
                    item.DateCreated = DateTime.Now.ToString("yyyy-MM-dd");
                    item.count = 0;
                    item.Total = 0.00;
                    result.Add(item);
                }
            }
            

            return Ok(result);
        }
        [HttpGet]
        public async Task<IActionResult> GetMostClickedHospitalityList()
        {
            GlobalVariables gv = new GlobalVariables();

            string sql = $@"SELECT     Count(*)as count, Actions,Business,Module,tbl_audittrailModel.DateCreated
                        FROM         tbl_audittrailModel  WHERE Actions LIKE '%Viewed%' and module ='Rooms & Suites' and  tbl_audittrailModel.DateCreated >= DATEADD(day,-7, GETDATE())
                        GROUP BY    Actions,Business,Module,tbl_audittrailModel.DateCreated order by count desc";
            DataTable dt = db.SelectDb(sql).Tables[0];
            int total = 0;
            var result = new List<MostClickHospitalityModel>();
            if (dt.Rows.Count != 0)
            {
                foreach (DataRow dr in dt.Rows)
                {
                    total += int.Parse(dr["count"].ToString());
                }
                foreach (DataRow dr in dt.Rows)
                {
                    var item = new MostClickHospitalityModel();
                    item.Actions = dr["Actions"].ToString();
                    item.Business = dr["Business"].ToString();
                    item.Module = dr["Module"].ToString();
                    item.DateCreated = DateTime.Parse(dr["DateCreated"].ToString()).ToString("MM-dd-yyyy");
                    item.count = int.Parse(dr["count"].ToString());
                    double val1 = double.Parse(dr["count"].ToString());
                    double val2 = double.Parse(total.ToString());

                    double results = Math.Abs(val1 / val2 * 100);
                    item.Total = Math.Round(results, 2);
                    result.Add(item);
                }

            }
            else
            {
                for (int x = 0; x < 4; x++)
                {
                    var item = new MostClickHospitalityModel();
                    item.Actions = "No Data";
                    item.Business = "No Data";
                    item.Module = "No Data";
                    item.DateCreated = DateTime.Now.ToString("yyyy-MM-dd");
                    item.count = 0;
                    item.Total = 0.00;
                    result.Add(item);
                }
            }
             
           
            return Ok(result);
        }

        [HttpGet]
        public async Task<IActionResult> GetMostClickRestaurantList()
        {
            GlobalVariables gv = new GlobalVariables();

            string sql = $@"SELECT     Count(*)as count, Actions,Business,Module,tbl_audittrailModel.DateCreated
                        FROM         tbl_audittrailModel  WHERE Actions LIKE '%Viewed%' and module ='Food & Beverage' and  tbl_audittrailModel.DateCreated >= DATEADD(day,-7, GETDATE())
                        GROUP BY    Actions,Business,Module,tbl_audittrailModel.DateCreated order by count desc";
            DataTable dt = db.SelectDb(sql).Tables[0];
              var result = new List<MostClickRestoModel>();
            if (dt.Rows.Count != 0)
            {
              
                int total = 0;
                foreach (DataRow dr in dt.Rows)
                {
                    total += int.Parse(dr["count"].ToString());
                }
                foreach (DataRow dr in dt.Rows)
                {
                    var item = new MostClickRestoModel();
                    item.Actions = dr["Actions"].ToString();
                    item.Business = dr["Business"].ToString();
                    item.Module = dr["Module"].ToString();
                    item.DateCreated = DateTime.Parse(dr["DateCreated"].ToString()).ToString("MM-dd-yyyy");
                    item.count = int.Parse(dr["count"].ToString());
                    double val1 = double.Parse(dr["count"].ToString());
                    double val2 = double.Parse(total.ToString());

                    double results = Math.Abs(val1 / val2 * 100);
                    item.Total = Math.Round(results, 2);
                    result.Add(item);
                }

          
            }
            else
            {
                for(int x =0; x < 4; x++)
                {
                    var item = new MostClickRestoModel();
                    item.Actions = "No Data";
                    item.Business = "No Data";
                    item.Module = "No Data";
                    item.DateCreated = DateTime.Now.ToString("yyyy-MM-dd");
                    item.count =0;
                    item.Total = 0.00;
                    result.Add(item);
                }
               
            }
            return Ok(result);

        }
        
        [HttpGet]
        public async Task<IActionResult> GetCallToActionsList()
        {

            string sql = $@"Select        Mail.Business, Mail.Email, Call.Call, Book.Book, Category.Module as Category ,Book.DateCreated
                            FROM            (SELECT        Business, COUNT(*) AS Email,DateCreated
                            FROM            tbl_audittrailModel
                            WHERE        (Module = 'Mail')
                            GROUP BY Business,DateCreated) AS Mail LEFT OUTER JOIN
                            (SELECT        Business, COUNT(*) AS Call,DateCreated
                            FROM            tbl_audittrailModel AS tbl_audittrailModel_1
                            WHERE        (Module = 'Call')
                            GROUP BY Business,DateCreated) AS Call ON Mail.Business = Call.Business LEFT OUTER JOIN
                            (SELECT        Business, COUNT(*)AS Book , DateCreated
                            FROM            tbl_audittrailModel AS tbl_audittrailModel_1
                            WHERE        (Module = 'Book') 
                            GROUP BY Business,DateCreated) AS Book ON Call.Business = Book.Business LEFT OUTER JOIN
                            (SELECT        Business, Module
                            FROM            tbl_audittrailModel AS tbl_audittrailModel_1   where Module='Hotel' or Module='Food & Beverage'  
                            GROUP BY Business, Module) AS Category ON Book.Business = Category.Business  order by Mail.Email desc";
            DataTable dt = db.SelectDb(sql).Tables[0];
            var result = new List<CallToActionsModel>();
            foreach (DataRow dr in dt.Rows)
            {

                string call = dr["Call"].ToString() == "" ? "0" : dr["Call"].ToString();
                string book = dr["Book"].ToString() == "" ? "0" : dr["Book"].ToString();
                string cat = dr["Category"].ToString() == "" ? "" : dr["Category"].ToString() == "Food & Beverage" ? "Restaurant" : dr["Category"].ToString() == "Hotel" ? "Hotel" : "";
                string mail = dr["Email"].ToString() == "" ? "0" : dr["Email"].ToString();
                var item = new CallToActionsModel();
                item.Business = dr["Business"].ToString();
                item.Category = cat;
                item.EmailCount = int.Parse(mail.ToString());
                item.CallCount = int.Parse(call.ToString());
                item.BookCount = int.Parse(book.ToString());
                result.Add(item);
            }

            return Ok(result);
        }
        [HttpPost]
        public async Task<IActionResult> PostCallToActionsList(UserFilterCatday data)
        {
            int daysLeft = new DateTime(DateTime.Now.Year, 12, 31).DayOfYear - DateTime.Now.DayOfYear;
            int day = data.day == 1 ? daysLeft : data.day;
            string sql = "";
            try
            {
                if (data.day == 0 && data.category == "0")
                {
                    sql = $@"Select        Mail.Business, Mail.Email, Call.Call, Book.Book, Category.Module as Category ,Book.DateCreated
                            FROM            (SELECT        Business, COUNT(*) AS Email,DateCreated
                            FROM            tbl_audittrailModel
                            WHERE        (Module = 'Mail')
                            GROUP BY Business,DateCreated) AS Mail LEFT OUTER JOIN
                            (SELECT        Business, COUNT(*) AS Call,DateCreated
                            FROM            tbl_audittrailModel AS tbl_audittrailModel_1
                            WHERE        (Module = 'Call')
                            GROUP BY Business,DateCreated) AS Call ON Mail.Business = Call.Business LEFT OUTER JOIN
                            (SELECT        Business, COUNT(*)AS Book , DateCreated
                            FROM            tbl_audittrailModel AS tbl_audittrailModel_1
                            WHERE        (Module = 'Book') 
                            GROUP BY Business,DateCreated) AS Book ON Call.Business = Book.Business LEFT OUTER JOIN
                            (SELECT        Business, Module
                            FROM            tbl_audittrailModel AS tbl_audittrailModel_1   where Module='Hotel' or Module='Food & Beverage'  
                            GROUP BY Business, Module) AS Category ON Book.Business = Category.Business order by Mail.Email desc ";
                }
                else if(data.day == 0 && data.category != "0" )
                {
                    sql = $@"Select        Mail.Business, Mail.Email, Call.Call, Book.Book, Category.Module as Category ,Book.DateCreated
                            FROM            (SELECT        Business, COUNT(*) AS Email,DateCreated
                            FROM            tbl_audittrailModel
                            WHERE        (Module = 'Mail')
                            GROUP BY Business,DateCreated) AS Mail LEFT OUTER JOIN
                            (SELECT        Business, COUNT(*) AS Call,DateCreated
                            FROM            tbl_audittrailModel AS tbl_audittrailModel_1
                            WHERE        (Module = 'Call')
                            GROUP BY Business,DateCreated) AS Call ON Mail.Business = Call.Business LEFT OUTER JOIN
                            (SELECT        Business, COUNT(*)AS Book , DateCreated
                            FROM            tbl_audittrailModel AS tbl_audittrailModel_1
                            WHERE        (Module = 'Book') 
                            GROUP BY Business,DateCreated) AS Book ON Call.Business = Book.Business LEFT OUTER JOIN
                            (SELECT        Business, Module
                            FROM            tbl_audittrailModel AS tbl_audittrailModel_1 where Module='Hotel' or Module='Food & Beverage'  
                            GROUP BY Business, Module) AS Category ON Book.Business = Category.Business where Category.Module = '"+data.category+"' order by Mail.Email desc ";
                }
                else if (data.day != 0 && data.category == "0" )
                {
                    sql = $@"Select        Mail.Business, Mail.Email, Call.Call, Book.Book, Category.Module as Category ,Book.DateCreated
                            FROM            (SELECT        Business, COUNT(*) AS Email,DateCreated
                            FROM            tbl_audittrailModel
                            WHERE        (Module = 'Mail')
                            GROUP BY Business,DateCreated) AS Mail LEFT OUTER JOIN
                            (SELECT        Business, COUNT(*) AS Call,DateCreated
                            FROM            tbl_audittrailModel AS tbl_audittrailModel_1
                            WHERE        (Module = 'Call')
                            GROUP BY Business,DateCreated) AS Call ON Mail.Business = Call.Business LEFT OUTER JOIN
                            (SELECT        Business, COUNT(*)AS Book , DateCreated
                            FROM            tbl_audittrailModel AS tbl_audittrailModel_1
                            WHERE        (Module = 'Book') and DateCreated >= DATEADD(day,-"+day+", GETDATE()) " +
                            "GROUP BY Business,DateCreated) AS Book ON Call.Business = Book.Business LEFT OUTER JOIN (SELECT        Business, Module " +
                            "FROM            tbl_audittrailModel AS tbl_audittrailModel_1 where Module='Hotel' or Module='Food & Beverage' " +
                            " GROUP BY Business, Module) AS Category ON Book.Business = Category.Business order by Mail.Email desc ";
                }
                else
                {
                    sql = $@"Select        Mail.Business, Mail.Email, Call.Call, Book.Book, Category.Module as Category ,Book.DateCreated
                            FROM            (SELECT        Business, COUNT(*) AS Email,DateCreated
                            FROM            tbl_audittrailModel
                            WHERE        (Module = 'Mail')
                            GROUP BY Business,DateCreated) AS Mail LEFT OUTER JOIN
                            (SELECT        Business, COUNT(*) AS Call,DateCreated
                            FROM            tbl_audittrailModel AS tbl_audittrailModel_1
                            WHERE        (Module = 'Call')
                            GROUP BY Business,DateCreated) AS Call ON Mail.Business = Call.Business LEFT OUTER JOIN
                            (SELECT        Business, COUNT(*)AS Book , DateCreated
                            FROM            tbl_audittrailModel AS tbl_audittrailModel_1
                            WHERE        (Module = 'Book') and DateCreated >= DATEADD(day,-" + day + ", GETDATE()) " +
                            "GROUP BY Business,DateCreated) AS Book ON Call.Business = Book.Business LEFT OUTER JOIN (SELECT        Business, Module " +
                            "FROM            tbl_audittrailModel AS tbl_audittrailModel_1 where Module='Hotel' or Module='Food & Beverage' " +
                            " GROUP BY Business, Module) AS Category ON Book.Business = Category.Business  where Category.Module = '"+data.category+"' order by Mail.Email desc ";
                }
                
            DataTable dt = db.SelectDb(sql).Tables[0];
            var result = new List<CallToActionsModel>();
            if (dt.Rows.Count > 0)
            {
                foreach (DataRow dr in dt.Rows)
                {

                    string call = dr["Call"].ToString() == "" ? "0" : dr["Call"].ToString();
                    string book = dr["Book"].ToString() == "" ? "0" : dr["Book"].ToString();
                    string cat = dr["Category"].ToString() == "" ? "" : dr["Category"].ToString() == "Food & Beverage" ? "Restaurant" : dr["Category"].ToString() == "Hotel" ? "Hotel" : "";
                    string mail = dr["Email"].ToString() == "" ? "0" : dr["Email"].ToString();
                    var item = new CallToActionsModel();
                    item.Business = dr["Business"].ToString();
                    item.Category = cat;
                    item.EmailCount = int.Parse(mail.ToString());
                    item.CallCount = int.Parse(call.ToString());
                    item.BookCount = int.Parse(book.ToString());
                    result.Add(item);
                }

                return Ok(result);
            }
            else
            {
                return BadRequest("ERROR");
            }
        }

            catch (Exception ex)
            {
                return BadRequest("ERROR");
    }
}
        [HttpGet]
        public async Task<IActionResult> GetCountAllUserlist()
        {
            GlobalVariables gv = new GlobalVariables();

            string sql = $@"select Count(*) as count from UsersModel where active=1";
            DataTable dt = db.SelectDb(sql).Tables[0];
            var result = new List<Usertotalcount>();

            foreach (DataRow dr in dt.Rows)
            {
                var item = new Usertotalcount();
                item.count = int.Parse(dr["count"].ToString());
                result.Add(item);
            }

            return Ok(result);
        }
        [HttpGet]
        public async Task<IActionResult> GetNewRegisteredWeekly()
        {
            int total = 0;
            string sqls = $@"select Count(*) as count from UsersModel where active=1";
            DataTable dts = db.SelectDb(sqls).Tables[0];

            foreach (DataRow dr in dts.Rows)
            {
                total = int.Parse(dr["count"].ToString());
            }


            string sql = $@"SELECT count(*) as count
                         FROM  UsersModel
                         WHERE DateCreated >= DATEADD(day,-7, GETDATE()) and active= 1";
            DataTable dt = db.SelectDb(sql).Tables[0];
            var result = new List<Usertotalcount>();

            foreach (DataRow dr in dt.Rows)
            {
                var item = new Usertotalcount();
                item.count = int.Parse(dr["count"].ToString());
                double val1 = double.Parse(dr["count"].ToString());
                double val2 = double.Parse(total.ToString());

                double results = Math.Abs(val1 / val2 * 100);
                item.percentage = results;
                result.Add(item);
            }

            return Ok(result);
        }
        [HttpGet]
        public async Task<IActionResult> GetSupportDetailsList()
        {
            GlobalVariables gv = new GlobalVariables();

            string sql = $@"SELECT        tbl_SupportModel.Id, tbl_SupportModel.Message, tbl_SupportModel.DateCreated, tbl_SupportModel.EmployeeID, CONCAT(UsersModel.Fname, ' ', UsersModel.Lname)  AS Fullname, tbl_StatusModel.Name AS Status
                         FROM            tbl_SupportModel INNER JOIN
                                                 UsersModel ON tbl_SupportModel.EmployeeID = UsersModel.EmployeeID INNER JOIN
                                                 tbl_StatusModel ON tbl_SupportModel.Status = tbl_StatusModel.Id order by id desc";
            DataTable dt = db.SelectDb(sql).Tables[0];
            var result = new List<SupportDetailModel>();

            foreach (DataRow dr in dt.Rows)
            {
                var item = new SupportDetailModel();
                item.Id = int.Parse(dr["Id"].ToString());
                item.Message = dr["Message"].ToString();
                item.Fullname = dr["Fullname"].ToString();
                item.EmployeeID = dr["EmployeeID"].ToString();
                item.Status = dr["Status"].ToString();
                item.DateCreated = DateTime.Parse(dr["DateCreated"].ToString()).ToString("MM/dd/yyyy hh:mm:ss tt");
                result.Add(item);
            }

            return Ok(result);
        }
        [HttpGet]
        public async Task<IActionResult> GetLineGraphCountList()
        {
            DateTime startDate = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd")).AddDays(-6);

            DateTime endDate = DateTime.Parse(DateTime.Now.ToString("yyyy-MM-dd"));
            List<DateTime> allDates = new List<DateTime>();
            var result = new List<UserCountLineGraphModel>();
            for (DateTime date = startDate; date <= endDate; date = date.AddDays(1))
            {
                //allDates.Add(date.Date);
                var dategen = date.Date.ToString("yyyy-MM-dd");
                string datecreated = "";
                int count_ = 0;
                string sql = $@"select DateCreated,Count(*) as count from UsersModel where active = 1 and DateCreated='" + dategen + "' group by DateCreated order by  DateCreated ";
                DataTable dt = db.SelectDb(sql).Tables[0];

                var item = new UserCountLineGraphModel();
                if (dt.Rows.Count == 0)
                {
                    datecreated = dategen;
                    count_ = 0;
                }
                else
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                        datecreated = dr["DateCreated"].ToString();
                        count_ = int.Parse(dr["count"].ToString());
                    }
                }

                item.DateCreated = DateTime.Parse(datecreated).ToString("dd");
                item.count = count_;
                result.Add(item);


            }


            return Ok(result);
        }
        #region POST METHOD

        [HttpPost]
        public async Task<IActionResult> PostClickCountsListAll(UserFilterday data)
        {
            int daysLeft = new DateTime(DateTime.Now.Year, 12, 31).DayOfYear - DateTime.Now.DayOfYear;
            int day = data.day == 1 ? daysLeft : data.day;

            string sql = $@"SELECT Business, Count(*) as count FROM tbl_audittrailModel
                         WHERE Actions LIKE '%Clicked%'  and Module <> 'AOPC APP' and  tbl_audittrailModel.DateCreated >= DATEADD(day,-"+ day + ", GETDATE()) " +
                         "GROUP BY Business order by count desc;";
            DataTable dt = db.SelectDb(sql).Tables[0];
            var result = new List<ClicCountModel>();
            if(dt.Rows.Count != 0)
            {
                foreach (DataRow dr in dt.Rows)
                {
                    var item = new ClicCountModel();
                    item.Module = dr["Business"].ToString();
                    item.Count = int.Parse(dr["count"].ToString());
                    result.Add(item);
                }

            }
            for (int x = 0; x < 4; x++)
            {
                var item = new ClicCountModel();
                item.Module = "No Data";
                item.Count = 0;
                result.Add(item);
            }

            return Ok(result);
        }
        [HttpPost]
        public async Task<IActionResult> PostMostClickRestaurantList(UserFilterday data)
        {
            int daysLeft = new DateTime(DateTime.Now.Year, 12, 31).DayOfYear - DateTime.Now.DayOfYear;
            int day = data.day == 1 ? daysLeft : data.day;
            try
            {

                string sql = $@"SELECT     Count(*)as count, Actions,Business,Module,tbl_audittrailModel.DateCreated
                        FROM         tbl_audittrailModel  WHERE Actions LIKE '%Viewed%' and module ='Food & Beverage' and  tbl_audittrailModel.DateCreated >= DATEADD(day,-" + day + ", GETDATE()) " +
                        "GROUP BY    Actions,Business,Module,tbl_audittrailModel.DateCreated order by count desc";
                DataTable dt = db.SelectDb(sql).Tables[0];
                var result = new List<MostClickRestoModel>();
                int total = 0;
                if (dt.Rows.Count > 0)
                {
              
                    foreach (DataRow dr in dt.Rows)
                    {
                        total += int.Parse(dr["count"].ToString());
                    }
                    foreach (DataRow dr in dt.Rows)
                    {
                        var item = new MostClickRestoModel();
                        item.Actions = dr["Actions"].ToString();
                        item.Business = dr["Business"].ToString();
                        item.Module = dr["Module"].ToString();
                        item.DateCreated = DateTime.Parse(dr["DateCreated"].ToString()).ToString("MM-dd-yyyy");
                        item.count = int.Parse(dr["count"].ToString());
                        double val1 = double.Parse(dr["count"].ToString());
                        double val2 = double.Parse(total.ToString());

                        double results = Math.Abs(val1 / val2 * 100);
                        item.Total = Math.Round(results, 2);
                        result.Add(item);
                    }

                
                }
                else
                {
                    for (int x = 0; x < 4; x++)
                    {
                        var item = new MostClickRestoModel();
                        item.Actions = "No Data";
                        item.Business = "No Data";
                        item.Module = "No Data";
                        item.DateCreated = DateTime.Now.ToString("yyyy-MM-dd");
                        item.count = 0;
                        item.Total = 0.00;
                        result.Add(item);
                    }

                }
                return Ok(result);
            }

            catch (Exception ex)
            {
                return BadRequest("ERROR");
            }
        }
        public static IEnumerable<dynamic> MonthsBetween(
            DateTime startDate,
            DateTime endDate)
            {
            DateTime iterator;
            DateTime limit;

            if (endDate > startDate)
            {
                iterator = new DateTime(startDate.Year, startDate.Month, 1);
                limit = endDate;
            }
            else
            {
                iterator = new DateTime(endDate.Year, endDate.Month, 1);
                limit = startDate;
            }

            var dateTimeFormat = CultureInfo.CurrentCulture.DateTimeFormat;
            while (iterator <= limit)
            {

                var firstDayOfMonth = new DateTime(iterator.Year, iterator.Month, 1);
                var lastDayOfMonth = firstDayOfMonth.AddMonths(1).AddDays(-1);

                yield return new
                {
                   label = dateTimeFormat.GetMonthName(iterator.Month)
                    
                };

                iterator = iterator.AddMonths(1);
            }
        }
        [HttpPost]
        public async Task<IActionResult> PostNewRegistered(UserFilterday data)
        {
            int total = 0;
            int daysLeft = new DateTime(DateTime.Now.Year, 12, 31).DayOfYear - DateTime.Now.DayOfYear;
            int day = data.day == 1 ? 334 : data.day;
            string datecreated = "";
            int count_ = 0;
            var result = new List<Usertotalcount>();
            try
            {
                string sqls = $@"select Count(*) as count from UsersModel where active=1";
                DataTable dts = db.SelectDb(sqls).Tables[0];

                foreach (DataRow dr in dts.Rows)
                {
                    total = int.Parse(dr["count"].ToString());
                }

                DateTime startDate = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd")).AddDays(-day);

                DateTime endDate = DateTime.Parse(DateTime.Now.ToString("yyyy-MM-dd"));

                var months = MonthsBetween(startDate, endDate).ToList();
                var items = new List<monthsdate>();
                var mo = JsonConvert.SerializeObject(months);
                var list = JsonConvert.DeserializeObject<List<monthsdate>>(mo);
                if (data.day == 1)
                {
                    for(int x= 0; x<list.Count; x++)
                    {
                        //var item = new monthsdate();
                        //var month = list[x].label;
                        //item.label = month;
                        //items.Add(item);
                        string sql1 = $@"SELECT  DATENAME(month,DateCreated)  AS month , count(*) as count from UsersModel where active = 1 and DATENAME(month,DateCreated) = '"+ list[x].label + "'   group by   DATENAME(month,DateCreated)   ";
                        DataTable dt1 = db.SelectDb(sql1).Tables[0];


                        if (dt1.Rows.Count == 0)
                        {
                            datecreated = list[x].label;
                            count_ = 0;
                        }
                        else
                        {
                            foreach (DataRow dr in dt1.Rows)
                            {
                                datecreated = dr["month"].ToString();
                                count_ = int.Parse(dr["count"].ToString());
                            }
                        }

                        string sql = $@"SELECT count(*) as count
                         FROM  UsersModel
                         WHERE DateCreated >= DATEADD(day,-" + day + ", GETDATE()) and active= 1";
                        DataTable dt = db.SelectDb(sql).Tables[0];
                        var item = new Usertotalcount();
                        if (dt.Rows.Count > 0)
                        {
                            foreach (DataRow dr in dt.Rows)
                            {
                                double val1 = double.Parse(dr["count"].ToString());
                                double val2 = double.Parse(total.ToString());

                                double results = Math.Abs(val1 / val2 * 100);
                                item.count = int.Parse(dr["count"].ToString());
                                item.Date = datecreated;
                                item.graph_count = count_;
                                item.percentage = results;
                                result.Add(item);

                            }


                        }
                        else
                        {
                            return BadRequest("ERROR");
                        }

                    }

                }
                else
                {
                    string query = $@"select Count(*) as count from UsersModel where active=1";
                    DataTable dtble = db.SelectDb(query).Tables[0];

                    foreach (DataRow dr in dtble.Rows)
                    {
                        total = int.Parse(dr["count"].ToString());
                    }
                    List<DateTime> allDates = new List<DateTime>();
                    
                    for (DateTime date = startDate; date <= endDate; date = date.AddDays(1))
                    {
                        //allDates.Add(date.Date);
                        var dategen = date.Date.ToString("yyyy-MM-dd");

                        string sql1 = $@"select DateCreated,Count(*) as count from UsersModel where active = 1 and DateCreated='" + dategen + "' group by DateCreated order by  DateCreated ";
                        DataTable dt1 = db.SelectDb(sql1).Tables[0];


                        if (dt1.Rows.Count == 0)
                        {
                            datecreated = dategen;
                            count_ = 0;
                        }
                        else
                        {
                            foreach (DataRow dr in dt1.Rows)
                            {
                                datecreated = dr["DateCreated"].ToString();
                                count_ = int.Parse(dr["count"].ToString());
                            }
                        }

                        string sql = $@"SELECT count(*) as count
                         FROM  UsersModel
                         WHERE DateCreated >= DATEADD(day,-" + day + ", GETDATE()) and active= 1";
                        DataTable dt = db.SelectDb(sql).Tables[0];
                        var item = new Usertotalcount();
                        if (dt.Rows.Count > 0)
                        {
                            foreach (DataRow dr in dt.Rows)
                            {
                                double val1 = double.Parse(dr["count"].ToString());
                                double val2 = double.Parse(total.ToString());

                                double results = Math.Abs(val1 / val2 * 100);

                                item.count = int.Parse(dr["count"].ToString());
                                item.Date = DateTime.Parse(datecreated).ToString("dd");
                                item.graph_count = count_;
                                item.percentage = results;
                                result.Add(item);

                            }


                        }
                        else
                        {
                            return BadRequest("ERROR");
                        }


                    }

                }


                return Ok(result);
            }

            catch (Exception ex)
            {
                return BadRequest("ERROR");
            }
        }

        [HttpPost]
        public async Task<IActionResult> PostMostClickedHospitalityList(UserFilterday data)
        {
            int daysLeft = new DateTime(DateTime.Now.Year, 12, 31).DayOfYear - DateTime.Now.DayOfYear;
            int day = data.day == 1 ? daysLeft : data.day;

            try
            {

                string sql = $@"SELECT     Count(*)as count, Actions,Business,Module,tbl_audittrailModel.DateCreated
                        FROM         tbl_audittrailModel  WHERE Actions LIKE '%Viewed%' and module ='Rooms & Suites' and  tbl_audittrailModel.DateCreated >= DATEADD(day,-"+day+", GETDATE()) " +
                        "GROUP BY    Actions,Business,Module,tbl_audittrailModel.DateCreated order by count desc";
                DataTable dt = db.SelectDb(sql).Tables[0];
                var result = new List<MostClickHospitalityModel>();
                if (dt.Rows.Count > 0)
                    {
                        int total = 0;
                    foreach (DataRow dr in dt.Rows)
                    {
                        total += int.Parse(dr["count"].ToString());
                    }
                    foreach (DataRow dr in dt.Rows)
                    {
                        var item = new MostClickHospitalityModel();
                        item.Actions = dr["Actions"].ToString();
                        item.Business = dr["Business"].ToString();
                        item.Module = dr["Module"].ToString();
                        item.DateCreated = DateTime.Parse(dr["DateCreated"].ToString()).ToString("MM-dd-yyyy");
                        item.count = int.Parse(dr["count"].ToString());
                        double val1 = double.Parse(dr["count"].ToString());
                        double val2 = double.Parse(total.ToString());

                        double results = Math.Abs(val1 / val2 * 100);
                        item.Total = Math.Round(results, 2);
                        result.Add(item);
                    }

             
                }
                else
                {
                    for (int x = 0; x < 4; x++)
                    {
                        var item = new MostClickHospitalityModel();
                        item.Actions = "No Data";
                        item.Business = "No Data";
                        item.Module = "No Data";
                        item.DateCreated = DateTime.Now.ToString("yyyy-MM-dd");
                        item.count = 0;
                        item.Total = 0.00;
                        result.Add(item);
                    }

                }
                return Ok(result);


            }

            catch (Exception ex)
            {
                return BadRequest("ERROR");
            }
        }

        [HttpPost]
        public async Task<IActionResult> PostMostCickStoreList(UserFilterday data)
        {
            int daysLeft = new DateTime(DateTime.Now.Year, 12, 31).DayOfYear - DateTime.Now.DayOfYear;
            int day = data.day == 1 ? daysLeft : data.day;

            try
            {
                string sql = $@"SELECT     Count(*)as count, Actions,Business,Module,tbl_audittrailModel.DateCreated
                         FROM         tbl_audittrailModel  WHERE Actions LIKE '%View%' and module ='Shops & Services' and  tbl_audittrailModel.DateCreated >= DATEADD(day,-" +day+", GETDATE())" +
                         " GROUP BY    Actions,Business,Module,tbl_audittrailModel.DateCreated order by count desc";
                DataTable dt = db.SelectDb(sql).Tables[0];
                List<MostClickStoreModel> result = new List<MostClickStoreModel>();
                List<MostClickStoreModel> result2 = new List<MostClickStoreModel>();
                int total = 0;
                double sub_total = 0;
                if (dt.Rows.Count > 0)
                {

                 
                    foreach (DataRow dr in dt.Rows)
                    {
                        total += int.Parse(dr["count"].ToString());
                    }
                    foreach (DataRow dr in dt.Rows)
                    {
                        var item = new MostClickStoreModel();
                        item.Actions = dr["Actions"].ToString();
                        item.Business = dr["Business"].ToString();
                        item.Module = dr["Module"].ToString();
                        item.DateCreated = DateTime.Parse(dr["DateCreated"].ToString()).ToString("MM-dd-yyyy");
                        item.count = int.Parse(dr["count"].ToString());
                        double val1 = double.Parse(dr["count"].ToString());
                        double val2 = double.Parse(total.ToString());

                        double results = val1 / val2 * 100;
                        item.Total = Math.Round(results, 2);
                        result.Add(item);

                    }
                    var total_res = Math.Abs(result.Count - 4);
                    if(result.Count != 4)
                    {
                        for (int x = 0; x < total_res; x++)
                        {
                            var item2 = new MostClickStoreModel();
                            item2.Actions = "No Data";
                            item2.Business = "No Data";
                            item2.Module = "No Data";
                            item2.DateCreated = DateTime.Now.ToString("yyyy-MM-dd");
                            item2.count = 0;
                            double results = sub_total - 100;
                            item2.Total = 0.01;
                            result2.Add(item2);
                        }
                    }
                    result.AddRange(result2);
                    return Ok(result);

               


                }
                else
                {
                    for (int x = 0; x < 4; x++)
                    {
                        var item = new MostClickStoreModel();
                        item.Actions = "No Data";
                        item.Business = "No Data";
                        item.Module = "No Data";
                        item.DateCreated = DateTime.Now.ToString("yyyy-MM-dd");
                        item.count = 0;
                        item.Total = 0.00;
                        result.Add(item);
                    }

                }
                return Ok(result);



            }

            catch (Exception ex)
            {
                return BadRequest("ERROR");
            }
        }
        #endregion
        #region Model
        public class UserFilterday
        {
            public int day { get; set; }

        }    
        public class UserFilterCatday
        {
            public int day { get; set; }
            public string category { get; set; }

        }
        public class SupportModel
        {
            public int Supportcount { get; set; }

        } public class monthsdate
        {
            public string label { get; set; }

        }
        public class Usertotalcount
        {
            public int count { get; set; }
            public int graph_count { get; set; }
            public double percentage { get; set; }
            public string Date { get; set; }

        }
        public class ClicCountModel
        {
            public string Module { get; set; }
            public int Count { get; set; }

        }
        public class CallToActionsModel
        {
            public string Business { get; set; }
            public string Category { get; set; }
            public int EmailCount { get; set; }
            public int CallCount { get; set; }
            public int BookCount { get; set; }

        }
        public class MostClickStoreModel
        {
            public string Actions { get; set; }
            public string Business { get; set; }
            public string Module { get; set; }
            public string DateCreated { get; set; }
            public int count { get; set; }
            public double Total { get; set; }

        }
        public class MostClickHospitalityModel
        {
            public string Actions { get; set; }
            public string Business { get; set; }
            public string Module { get; set; }
            public string DateCreated { get; set; }
            public int count { get; set; }
            public double Total { get; set; }

        }
        public class MostClickRestoModel
        {
            public string Actions { get; set; }
            public string Business { get; set; }
            public string Module { get; set; }
            public string DateCreated { get; set; }
            public int count { get; set; }
            public double Total { get; set; }

        }
        public class SupportDetailModel
        {
            public int Id { get; set; }
            public string Message { get; set; }
            public string EmployeeID { get; set; }
            public string Fullname { get; set; }
            public string Status { get; set; }
            public string DateCreated { get; set; }

        }
        public class UserCountLineGraphModel
        {
            public string DateCreated { get; set; }
            public int count { get; set; }

        }
        #endregion
    }
}