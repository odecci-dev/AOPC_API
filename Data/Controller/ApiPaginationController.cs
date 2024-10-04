using API.ViewModel;
using AuthSystem.Manager;
using AuthSystem.Models;
using AuthSystem.Services;
using AuthSystem.ViewModel;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Data;
using System.Net;
using static AuthSystem.Data.Controller.ApiAuditTrailController;
using static AuthSystem.Data.Controller.ApiNotifcationController;

namespace AuthSystem.Data.Controller
{
    //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class ApiPaginationController : ControllerBase
    {
        DbManager db = new DbManager();
        DBMethods dbmet = new DBMethods();
        private readonly AppSettings _appSettings;
        private ApplicationDbContext _context;
        private ApiGlobalModel _global = new ApiGlobalModel();
        private readonly JwtAuthenticationManager jwtAuthenticationManager;

        public ApiPaginationController(JwtAuthenticationManager jwtAuthenticationManager, ApplicationDbContext context)
        {
            this.jwtAuthenticationManager = jwtAuthenticationManager;
            _context = context;
        }
        public class Registerstats
        {
            public string Status { get; set; }

        }
        public class PaginationModel
        {
            public string? CurrentPage { get; set; }
            public string? NextPage { get; set; }
            public string? PrevPage { get; set; }
            public string? TotalPage { get; set; }
            public string? PageSize { get; set; }
            public string? TotalRecord { get; set; }
            public List<VendorVM> items { get; set; }


        }
        public class NotificationPaginateModel
        {
            public string? CurrentPage { get; set; }
            public string? NextPage { get; set; }
            public string? PrevPage { get; set; }
            public string? TotalPage { get; set; }
            public string? PageSize { get; set; }
            public string? TotalRecord { get; set; }
            public List<NotificationVM> items { get; set; }


        }
        public class AuditTrailPaginateModel
        {
            public string? CurrentPage { get; set; }
            public string? NextPage { get; set; }
            public string? PrevPage { get; set; }
            public string? TotalPage { get; set; }
            public string? PageSize { get; set; }
            public string? TotalRecord { get; set; }
            public List<Audittrailvm> items { get; set; }


        }
        public class PaginationCorpUserModel
        {
            public string? CurrentPage { get; set; }
            public string? NextPage { get; set; }
            public string? PrevPage { get; set; }
            public string? TotalPage { get; set; }
            public string? PageSize { get; set; }
            public string? TotalRecord { get; set; }
            public string? TotalVIP { get; set; }
            public List<UserVM> items { get; set; }


        }

        public class PaginationCorpUserCountModel
        {
            public string? CurrentPage { get; set; }
            public string? NextPage { get; set; }
            public string? PrevPage { get; set; }
            public string? TotalPage { get; set; }
            public string? PageSize { get; set; }
            public string? TotalRecord { get; set; }
            public string? TotalVIP { get; set; }
            public List<CorporateUserCountVM> items { get; set; }


        }
        public class paginate
        {
            public string? FilterName { get; set; }
            public int page { get; set; }
        }
        [HttpPost]
        public async Task<IActionResult> DisplayListPaginate(paginate data )
        {
          
            string module = "Vendor";
            string status = "ACTIVE";
            int pageSize = 25;
            //var model_result = (dynamic)null;
            var items = (dynamic)null;
            int totalItems = 0;
            int totalPages = 0;
            string page_size = pageSize == 0 ? "10" : pageSize.ToString();
            try
            {

                switch (module)
                {
                    case "Vendor":
                        //model_result = new PaginationModel<VendorVM>();

                        if (data.FilterName == null && status == null)
                        {

                            var Member = dbmet.GetVendorDetails().ToList();
                            totalItems = Member.Count;
                            totalPages = (int)Math.Ceiling((double)totalItems / int.Parse(page_size.ToString()));

                            items = Member.Skip((data.page - 1) * int.Parse(page_size.ToString())).Take(int.Parse(page_size.ToString())).ToList();
                        }
                        else if (data.FilterName != null && status == null)
                        {
                            var Member = dbmet.GetVendorDetails().Where(a => a.VendorName.ToUpper().Contains(data.FilterName.ToUpper())).ToList();
                            totalItems = Member.Count;
                            totalPages = (int)Math.Ceiling((double)totalItems / int.Parse(page_size.ToString()));

                            items = Member.Skip((data.page - 1) * int.Parse(page_size.ToString())).Take(int.Parse(page_size.ToString())).ToList();
                        }
                        else if (data.FilterName == null && status != null)
                        {
                            var Member = dbmet.GetVendorDetails().Where(a => a.Status == status).ToList();
                            totalItems = Member.Count;
                            totalPages = (int)Math.Ceiling((double)totalItems / int.Parse(page_size.ToString()));

                            items = Member.Skip((data.page - 1) * int.Parse(page_size.ToString())).Take(int.Parse(page_size.ToString())).ToList();
                        }
                        else
                        {

                            var Member = dbmet.GetVendorDetails().Where(a => a.VendorName.ToUpper().Contains(data.FilterName.ToUpper()) && a.Status == status).ToList();
                            totalItems = Member.Count;
                            totalPages = (int)Math.Ceiling((double)totalItems / int.Parse(int.Parse(page_size.ToString()).ToString()));

                            items = Member.Skip((data.page - 1) * int.Parse(page_size.ToString())).Take(int.Parse(page_size.ToString())).ToList();
                        }
                        break;
                    
                    default:
                        items = null;
                        break;
                }
                var result = new List<PaginationModel>();
                var item = new PaginationModel();
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
                item.TotalRecord = totalItems.ToString();
                item.items = items;
                result.Add(item);
                return Ok(result);


            }

            catch (Exception ex)
            {
                return BadRequest("ERROR");
            }
        }

        [HttpPost]
        public async Task<IActionResult> NotificationPaginate(paginate data)
        {

            //string module = "Vendor";
            //string status = "ACTIVE";
            int pageSize = 25;
            //var model_result = (dynamic)null;
            var items = (dynamic)null;
            int totalItems = 0;
            int totalPages = 0;
            string page_size = pageSize == 0 ? "10" : pageSize.ToString();
            try
            {

                var Member = dbmet.GetNotificationDetails().ToList();
                totalItems = Member.Count;
                totalPages = (int)Math.Ceiling((double)totalItems / int.Parse(page_size.ToString()));

                items = Member.Skip((data.page - 1) * int.Parse(page_size.ToString())).Take(int.Parse(page_size.ToString())).ToList();

                var result = new List<NotificationPaginateModel>();
                var item = new NotificationPaginateModel();
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
                item.TotalRecord = totalItems.ToString();
                item.items = items;
                result.Add(item);
                return Ok(result);


            }

            catch (Exception ex)
            {
                return BadRequest("ERROR");
            }
        }

        [HttpPost]
        public async Task<IActionResult> AuditTrailPaginate(paginate data)
        {

            //string module = "Vendor";
            //string status = "ACTIVE";
            int pageSize = 25;
            //var model_result = (dynamic)null;
            var items = (dynamic)null;
            int totalItems = 0;
            int totalPages = 0;
            string page_size = pageSize == 0 ? "10" : pageSize.ToString();
            try
            {

                var Member = dbmet.GetAuditTrailList().ToList();
                totalItems = Member.Count;
                totalPages = (int)Math.Ceiling((double)totalItems / int.Parse(page_size.ToString()));

                items = Member.Skip((data.page - 1) * int.Parse(page_size.ToString())).Take(int.Parse(page_size.ToString())).ToList();

                var result = new List<AuditTrailPaginateModel>();
                var item = new AuditTrailPaginateModel();
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
                item.TotalRecord = totalItems.ToString();
                item.items = items;
                result.Add(item);
                return Ok(result);


            }

            catch (Exception ex)
            {
                return BadRequest("ERROR");
            }
        }

        public class paginateCorpUser
        {
            public string? CorpId { get; set; }
            public string? PosId { get; set; }
            public string? FilterName { get; set; }
            public int page { get; set; }
        }
        [HttpPost]
        public async Task<IActionResult> DisplayCorporateUser(paginateCorpUser data)
        {

           
            
            string status = "ACTIVE";
            int pageSize = 10;
            //var model_result = (dynamic)null;
            var items = (dynamic)null;
            int totalItems = 0;
            int totalPages = 0;
            int totalVIP = 0;
            string page_size = pageSize == 0 ? "10" : pageSize.ToString();
            try
            {

              
               
                        //model_result = new PaginationModel<VendorVM>();

                        if (data.FilterName == null && data.PosId == "0" || data.PosId == null)
                        {

                            var Member = dbmet.GetCorporateAdminUserList().Where(a=>a.CorporateID == data.CorpId).ToList();
                            totalItems = Member.Count;
                            totalVIP = Member.Where(a=>a.isVIP == "1").ToList().Count;
                            totalPages = (int)Math.Ceiling((double)totalItems / int.Parse(page_size.ToString()));

                            items = Member.Skip((data.page - 1) * int.Parse(page_size.ToString())).Take(int.Parse(page_size.ToString())).ToList();
                        }  
                        else if(data.PosId != "0"  && data.PosId != null && data.FilterName != null)
                        {
                            var Member = dbmet.GetCorporateAdminUserList().Where(a => a.Username.ToUpper().Contains(data.FilterName.ToUpper()) && a.PositionID == data.PosId &&  a.CorporateID == data.CorpId).ToList();
                            totalItems = Member.Count;
                      totalVIP = Member.Where(a=>a.isVIP == "1").ToList().Count;
                            totalPages = (int)Math.Ceiling((double)totalItems / int.Parse(page_size.ToString()));

                            items = Member.Skip((data.page - 1) * int.Parse(page_size.ToString())).Take(int.Parse(page_size.ToString())).ToList();
                        }
                        else if(data.PosId != "0" && data.PosId != null )
                        {
                            var Member = dbmet.GetCorporateAdminUserList().Where(a =>a.PositionID == data.PosId && a.CorporateID == data.CorpId).ToList();
                            totalItems = Member.Count;
                      totalVIP = Member.Where(a=>a.isVIP == "1").ToList().Count;
                            totalPages = (int)Math.Ceiling((double)totalItems / int.Parse(page_size.ToString()));

                            items = Member.Skip((data.page - 1) * int.Parse(page_size.ToString())).Take(int.Parse(page_size.ToString())).ToList();
                        }
                     
                        else 
                        {
                            var Member = dbmet.GetCorporateAdminUserList().Where(a => a.Username.ToUpper().Contains(data.FilterName.ToUpper()) && a.CorporateID == data.CorpId ).ToList();
                            totalItems = Member.Count;
                      totalVIP = Member.Where(a=>a.isVIP == "1").ToList().Count;
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

            catch (Exception ex)
            {
                return BadRequest("ERROR");
            }
        }


        [HttpPost]
        public async Task<IActionResult> DisplayRegistrationList(paginateCorpUser data)
        {



            string status = "ACTIVE";
            int pageSize = 10;
            //var model_result = (dynamic)null;
            var items = (dynamic)null;
            int totalItems = 0;
            int totalVIP = 0;
            int totalPages = 0;
            string page_size = pageSize == 0 ? "10" : pageSize.ToString();
            try
            {



                //model_result = new PaginationModel<VendorVM>();

                if (data.FilterName == null &&  data.PosId == null && data.CorpId == null )
                {

                    var Member = dbmet.GetCorporateAdminUserList().ToList();
                    totalItems = Member.Count;
                    totalVIP = Member.Where(a=>a.isVIP == "1").ToList().Count;
                    totalPages = (int)Math.Ceiling((double)totalItems / int.Parse(page_size.ToString()));

                    items = Member.Skip((data.page - 1) * int.Parse(page_size.ToString())).Take(int.Parse(page_size.ToString())).ToList();
                }
                else if (data.PosId != "0" && data.PosId != null && data.FilterName != null && data.CorpId != null)
                {
                    var Member = dbmet.GetCorporateAdminUserList().Where(a => a.Username.ToUpper().Contains(data.FilterName.ToUpper()) && a.PositionID == data.PosId && a.CorporateID == data.CorpId).ToList();
                    totalItems = Member.Count;
                    totalVIP = Member.Where(a => a.isVIP == "1").ToList().Count;
                    totalPages = (int)Math.Ceiling((double)totalItems / int.Parse(page_size.ToString()));

                    items = Member.Skip((data.page - 1) * int.Parse(page_size.ToString())).Take(int.Parse(page_size.ToString())).ToList();
                }
                else if ( data.PosId != null  && data.CorpId != null)
                {
                    var Member = dbmet.GetCorporateAdminUserList().Where(a =>  a.PositionID == data.PosId && a.CorporateID == data.CorpId).ToList();
                    totalItems = Member.Count;
                    totalVIP = Member.Where(a => a.isVIP == "1").ToList().Count;
                    totalPages = (int)Math.Ceiling((double)totalItems / int.Parse(page_size.ToString()));

                    items = Member.Skip((data.page - 1) * int.Parse(page_size.ToString())).Take(int.Parse(page_size.ToString())).ToList();
                }
                else if (data.PosId != null && data.FilterName != null)
                {
                    var Member = dbmet.GetCorporateAdminUserList().Where(a => a.PositionID == data.PosId && a.Username.ToUpper().Contains(data.FilterName.ToUpper())).ToList();
                    totalItems = Member.Count;
                    totalVIP = Member.Where(a => a.isVIP == "1").ToList().Count;
                    totalPages = (int)Math.Ceiling((double)totalItems / int.Parse(page_size.ToString()));

                    items = Member.Skip((data.page - 1) * int.Parse(page_size.ToString())).Take(int.Parse(page_size.ToString())).ToList();
                }
                else if (data.CorpId != null && data.FilterName != null)
                {
                    var Member = dbmet.GetCorporateAdminUserList().Where(a => a.CorporateID == data.CorpId && a.Username.ToUpper().Contains(data.FilterName.ToUpper())).ToList();
                    totalItems = Member.Count;
                    totalVIP = Member.Where(a => a.isVIP == "1").ToList().Count;
                    totalPages = (int)Math.Ceiling((double)totalItems / int.Parse(page_size.ToString()));

                    items = Member.Skip((data.page - 1) * int.Parse(page_size.ToString())).Take(int.Parse(page_size.ToString())).ToList();
                }
                else if ( data.PosId != null)
                {
                    var Member = dbmet.GetCorporateAdminUserList().Where(a => a.PositionID == data.PosId).ToList();
                    totalItems = Member.Count;
                    totalVIP = Member.Where(a => a.isVIP == "1").ToList().Count;
                    totalPages = (int)Math.Ceiling((double)totalItems / int.Parse(page_size.ToString()));

                    items = Member.Skip((data.page - 1) * int.Parse(page_size.ToString())).Take(int.Parse(page_size.ToString())).ToList();
                }
                else if (data.CorpId != null)
                {
                    var Member = dbmet.GetCorporateAdminUserList().Where(a => a.CorporateID == data.CorpId).ToList();
                    totalItems = Member.Count;
                    totalVIP = Member.Where(a => a.isVIP == "1").ToList().Count;
                    totalPages = (int)Math.Ceiling((double)totalItems / int.Parse(page_size.ToString()));

                    items = Member.Skip((data.page - 1) * int.Parse(page_size.ToString())).Take(int.Parse(page_size.ToString())).ToList();
                }
                else
                {
                    var Member = dbmet.GetCorporateAdminUserList().Where(a => a.Username.ToUpper().Contains(data.FilterName.ToUpper()) && a.CorporateID == data.CorpId).ToList();
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
                item.TotalRecord = totalItems.ToString();
                item.TotalVIP = totalVIP.ToString();
                item.items = items;
                result.Add(item);
                return Ok(result);


            }

            catch (Exception ex)
            {
                return BadRequest("ERROR");
            }
        }

    }
}
