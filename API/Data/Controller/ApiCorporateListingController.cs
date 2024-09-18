using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using AuthSystem.Manager;
using AuthSystem.Models;
using AuthSystem.Services;
using Microsoft.Extensions.Options;
using AuthSystem.ViewModel;
using System.Data;
using API.ViewModel;
using Microsoft.EntityFrameworkCore;
using static AuthSystem.Data.Controller.ApiPaginationController;

namespace API.Data.Controller
{
    [Authorize("ApiKey")]
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class ApiCorporateListingController : ControllerBase
    {
        DbManager db = new DbManager();
        private readonly AppSettings _appSettings;
        private ApplicationDbContext _context;
        private ApiGlobalModel _global = new ApiGlobalModel();
        private readonly JwtAuthenticationManager jwtAuthenticationManager;
        DBMethods dbmet = new DBMethods();


        public ApiCorporateListingController(IOptions<AppSettings> appSettings, ApplicationDbContext context, JwtAuthenticationManager jwtAuthenticationManager)
        {

            _context = context;
            _appSettings = appSettings.Value;
            this.jwtAuthenticationManager = jwtAuthenticationManager;

        }

        [HttpGet]
        public async Task<IActionResult> CorporateRegularUserCount()
        {
            string sql = $@"SELECT CorporateName,COUNT(*)as count from UsersModel
                         inner join tbl_CorporateModel ON tbl_CorporateModel.Id = CorporateID where Active = 1
                         group by CorporateName";

            DataTable table = db.SelectDb(sql).Tables[0];
            var result = new List<CorporateListing>();
            foreach (DataRow dr in table.Rows)
            {
                var item = new CorporateListing();
                item.Company = dr["CorporateName"].ToString();
                item.UserCount = int.Parse(dr["count"].ToString());
                result.Add(item);
            }

            return Ok(result);
        }

        [HttpGet]
        public async Task<IActionResult> CorporateVIPUserCount()
        {
            string sql = $@"SELECT COUNT(*)as count,CorporateName from UsersModel
            inner join tbl_CorporateModel ON tbl_CorporateModel.Id = CorporateID
            where isVIP = 1 and Active = 1
            group by CorporateName";

            DataTable table = db.SelectDb(sql).Tables[0];
            var result = new List<CorporateListing>();
            foreach (DataRow dr in table.Rows)
            {
                var item = new CorporateListing();
                item.Company = dr["CorporateName"].ToString();
                item.UserCount = int.Parse(dr["count"].ToString());
                result.Add(item);
            }

            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> UserCountWithFilter(UserListFilter data)
        {

            int pageSize = 10;
            //var model_result = (dynamic)null;
            var items = (dynamic)null;
            int totalItems = 0;
            int totalPages = 0;
            int totalVIP = 0;
            string page_size = pageSize == 0 ? "10" : pageSize.ToString();

            if (data.Corporatename.Equals("0") && data.Status.Equals("0"))
            {
                var Member = dbmet.GetUserList().ToList();
                totalItems = Member.Count;
                totalVIP = Member.Where(a => a.isVIP == "1").ToList().Count;
                totalPages = (int)Math.Ceiling((double)totalItems / int.Parse(page_size.ToString()));
                items = Member.Skip((data.page - 1) * int.Parse(page_size.ToString())).Take(int.Parse(page_size.ToString())).ToList();
            }
            else if (!data.Corporatename.Equals("0") && data.Status.Equals("0"))
            {
                var Member = dbmet.GetUserList().Where(a => a.Corporatename.ToLower() == data.Corporatename.ToLower()).ToList();
                totalItems = Member.Count;
                totalVIP = Member.Where(a => a.isVIP == "1").ToList().Count;
                totalPages = (int)Math.Ceiling((double)totalItems / int.Parse(page_size.ToString()));
                items = Member.Skip((data.page - 1) * int.Parse(page_size.ToString())).Take(int.Parse(page_size.ToString())).ToList();
            }
            else if (data.Corporatename.Equals("0") && !data.Status.Equals("0"))
            {
                var Member = dbmet.GetUserList().Where(a => a.status.ToLower() == data.Status.ToLower()).ToList();
                totalItems = Member.Count;
                totalVIP = Member.Where(a => a.isVIP == "1").ToList().Count;
                totalPages = (int)Math.Ceiling((double)totalItems / int.Parse(page_size.ToString()));
                items = Member.Skip((data.page - 1) * int.Parse(page_size.ToString())).Take(int.Parse(page_size.ToString())).ToList();
            }
            else
            {
                var Member = dbmet.GetUserList().Where(a => a.status.ToLower() == data.Status.ToLower() && a.Corporatename.ToLower() == data.Corporatename.ToLower()).ToList();
                totalItems = Member.Count;
                totalVIP = Member.Where(a => a.isVIP == "1").ToList().Count;
                totalPages = (int)Math.Ceiling((double)totalItems / int.Parse(page_size.ToString()));
                items = Member.Skip((data.page - 1) * int.Parse(page_size.ToString())).Take(int.Parse(page_size.ToString())).ToList();
            }
            
            var result = new List<PaginationCorpUserModel>();
            var item = new PaginationCorpUserModel();
            int pages = data.page == 0 ? 1 : data.page;
            item.CurrentPage = data.page == 0 ? "1" : data.page.ToString();

            int page_prev = pages - 1;
            //int t_record = int.Parse(items.Count.ToString()) / int.Parse(page_size);

            double t_records = Math.Ceiling(double.Parse(totalItems.ToString()) / double.Parse(page_size));
            int page_next = data.page >= t_records ? 0 : pages + 1;
            item.NextPage = items.Count % int.Parse(page_size) >= 0 ? page_next.ToString() : "0";
            item.PrevPage = pages == 1 ? "0" : page_prev.ToString();
            item.TotalPage = t_records.ToString();
            item.PageSize = page_size;
            item.TotalVIP = totalVIP.ToString();
            item.TotalRecord = totalItems.ToString();
            item.items = items;
            result.Add(item);

            return Ok(result);
        }

        public class UserListFilter
        {
            public string Corporatename { get; set; }
            public string Status { get; set; }
            public string? FilterName { get; set; }
            public int page { get; set; }
        }
        
    }
}
