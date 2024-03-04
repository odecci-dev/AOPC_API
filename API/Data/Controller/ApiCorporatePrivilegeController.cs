using AuthSystem.Manager;
using AuthSystem.Models;
using AuthSystem.ViewModel;
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
using Newtonsoft.Json;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace AuthSystem.Data.Controller
{
    //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [Authorize("ApiKey")]
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class ApiCorporatePrivilegeController : ControllerBase
    {
        DbManager db = new DbManager();
        private readonly AppSettings _appSettings;
        private ApplicationDbContext _context;
        private ApiGlobalModel _global = new ApiGlobalModel();
        private readonly JwtAuthenticationManager jwtAuthenticationManager;


        public ApiCorporatePrivilegeController(ApplicationDbContext context)
        {

            _context = context;

        }


        [HttpGet]
        public async Task<IActionResult> CorporatePrivilegeLsit()
        {

            var result = new List<CorporatePrivilegeVM>();
            DataTable table = db.SelectDb_SP("Corporate_SP").Tables[0];

            foreach (DataRow dr in table.Rows)
            {
                var item = new CorporatePrivilegeVM();
                item.Id = int.Parse(dr["id"].ToString());
                item.Fullname = dr["Fullname"].ToString();
                item.Businessname = dr["Businessname"].ToString();
                item.Vendorname = dr["Vendorname"].ToString();
                item.Corporatename = dr["Corporatename"].ToString();
                item.Privilegename = dr["Privilegename"].ToString();
                item.Country = dr["Country"].ToString();
                item.Businesstype = dr["Businesstypename"].ToString();
                item.NoOfVisit = int.Parse(dr["No_Of_visit"].ToString());
                result.Add(item);
            }

            return Ok(result);
        }
        public class PrivCorp
        {
            public string? privilegeID { get; set; }
            public string? usercount { get; set; }
            public string? vipcount { get; set; }
            public string? CorporateID { get; set; }
            public string? status { get; set; }
            public string? stats { get; set; }
        }
        public class PrivCorpisVIP
        {
            public string? privilegeID { get; set; }
            public string? usercount { get; set; }
            public string? vipcount { get; set; }
            public string? CorporateID { get; set; }
            public string? status { get; set; }
            public string? stats { get; set; }
            public string? vipstats { get; set; }
        }
        public class Registerstats
        {
            public string Status { get; set; }

        }
        public class PrivCorpListItem
        {
            public string Id { get; set; }
            public string Title { get; set; }
            public string PrivilegeID { get; set; }
            public string CorporateID { get; set; }
            public string CorporateName { get; set; }
            public string UserCount { get; set; }
            public string VIPCount { get; set; }
            public string Status { get; set; }
            public string isVIP { get; set; }

        }
        public class privIDs
        {
            public string Id { get; set; }
            public string memid { get; set; }

        }
        [HttpPost]
        public async Task<IActionResult> PrivilegeCorporateList(privIDs data)
        {

            var result = new List<PrivCorpListItem>();
            var stats = "";
            var isvip = "";
            string sql2 = $@"SELECT Title, Id AS PrivilegeID FROM   tbl_PrivilegeModel WHERE   (Active = 5)";
            DataTable dt2 = db.SelectDb(sql2).Tables[0];

            foreach (DataRow dr in dt2.Rows)
            {
                string sqls = $@"SELECT       PrivilegeID, CorporateID, Count, VipCount,isVIP
                                FROM            tbl_CorporatePrivilegeTierModel
                                WHERE        (CorporateID = '" + data.Id + "') ";
                DataTable dts = db.SelectDb(sqls).Tables[0];

                if (dts.Rows.Count == 0)
                {

                    string sql_ = $@"SELECT      PrivilegeID, MembershipID, Count, VipCount
                                FROM            tbl_MembershipPrivilegeModel
                                WHERE        (MembershipID = '" + data.memid + "') AND (PrivilegeID = '" + dr["PrivilegeID"].ToString() + "')";
                    DataTable dt_ = db.SelectDb(sql_).Tables[0];
                    if (dt_.Rows.Count != 0)
                    {
                        stats = "1";
                        isvip = "0";

                    }
                    else
                    {
                        stats = "0";
                        isvip = "0";
                    }
                    var item = new PrivCorpListItem();
                    item.Title = dr["Title"].ToString();
                    item.PrivilegeID = dr["PrivilegeID"].ToString();
                    item.CorporateID = data.Id;
                    item.VIPCount = "0";
                    item.UserCount = "0";
                    item.Status = stats;
                    item.isVIP = isvip;
                    result.Add(item);
                }
                else
                {
                    string sql = $@"SELECT       PrivilegeID, CorporateID, Count, VipCount,isVIP
                                FROM            tbl_CorporatePrivilegeTierModel
                                WHERE        (CorporateID = '" + data.Id + "') AND (PrivilegeID = '" + dr["PrivilegeID"].ToString() + "')";
                    DataTable dt = db.SelectDb(sql).Tables[0];

                    if (dt.Rows.Count != 0)
                    {
                        stats = "1";
                        isvip = dt.Rows[0]["isVIP"].ToString();
                    }
                    else
                    {
                        stats = "0";
                        isvip = "0";
                    }
                    var item = new PrivCorpListItem();
                    item.Title = dr["Title"].ToString();
                    item.PrivilegeID = dr["PrivilegeID"].ToString();
                    item.CorporateID = data.Id;
                    item.VIPCount = "0";
                    item.UserCount = "0";
                    item.Status = stats;
                    item.isVIP = isvip;
                    result.Add(item);

                }


            }
            return Ok(result);
        }
        [HttpPost]
        public IActionResult SaveCorporatePrivilegeList(List<PrivCorp> IdList)
        {
            string delete = $@"delete tbl_CorporatePrivilegeTierModel where CorporateID='" + IdList[0].CorporateID + "'";
            db.AUIDB_WithParam(delete);
            var result = new Registerstats();
            string imgfile = "";

            foreach (var emp in IdList)
            {
                if (emp.privilegeID != null)
                {
                    string sql = $@"SELECT        tbl_CorporatePrivilegeTierModel.Id, tbl_CorporatePrivilegeTierModel.PrivilegeID, tbl_CorporatePrivilegeTierModel.CorporateID, tbl_CorporatePrivilegeTierModel.Count, tbl_CorporatePrivilegeTierModel.VipCount, 
                             tbl_PrivilegeModel.Title
                             FROM            tbl_CorporatePrivilegeTierModel INNER JOIN
                             tbl_PrivilegeModel ON tbl_CorporatePrivilegeTierModel.PrivilegeID = tbl_PrivilegeModel.Id where    (tbl_CorporatePrivilegeTierModel.PrivilegeID = '" + emp.privilegeID + "')  and  (tbl_CorporatePrivilegeTierModel.CorporateID = '" + emp.CorporateID + "') ";
                    DataTable dt = db.SelectDb(sql).Tables[0];
                    if (dt.Rows.Count == 0)
                    {
                        if (emp.stats == "1")
                        {


                            string insert = $@"insert into tbl_CorporatePrivilegeTierModel (PrivilegeID,CorporateID,Count,VipCount) values 
                                             ('" + emp.privilegeID + "','" + emp.CorporateID + "','" + emp.usercount + "','" + emp.vipcount + "')";
                            db.AUIDB_WithParam(insert);

                        }

                    }
                    else
                    {


                    }
                }
                result.Status = "Successfully Added";

            }

            return Ok(result);
        }
        [HttpPost]
        public IActionResult SaveCorporatePrivilegeListisVIP(List<PrivCorpisVIP> IdList)
        {
            //string delete = $@"delete tbl_CorporatePrivilegeTierModel where CorporateID='" + IdList[0].CorporateID + "'";
            //db.AUIDB_WithParam(delete);
            var result = new Registerstats();
            string imgfile = "";

            foreach (var emp in IdList)
            {
                if (emp.privilegeID != null)
                {
                    string sql = $@"SELECT        tbl_CorporatePrivilegeTierModel.Id, tbl_CorporatePrivilegeTierModel.PrivilegeID, tbl_CorporatePrivilegeTierModel.CorporateID, tbl_CorporatePrivilegeTierModel.Count, tbl_CorporatePrivilegeTierModel.VipCount, 
                             tbl_PrivilegeModel.Title
                             FROM            tbl_CorporatePrivilegeTierModel INNER JOIN
                             tbl_PrivilegeModel ON tbl_CorporatePrivilegeTierModel.PrivilegeID = tbl_PrivilegeModel.Id where    (tbl_CorporatePrivilegeTierModel.PrivilegeID = '" + emp.privilegeID + "')  and  (tbl_CorporatePrivilegeTierModel.CorporateID = '" + emp.CorporateID + "') ";
                    DataTable dt = db.SelectDb(sql).Tables[0];
                    if (dt.Rows.Count != 0)
                    {
                        if (emp.stats == "1")
                        {

                            string Update = $@"UPDATE [dbo].[tbl_CorporatePrivilegeTierModel]
                                           SET [isVIP] = '" + emp.stats + "' " +
                                         "WHERE PrivilegeID ='" + emp.privilegeID + "' and CorporateID='" + emp.CorporateID + "' ";
                            db.AUIDB_WithParam(Update);

                        }

                    }
                    else
                    {
                        string Update = $@"UPDATE [dbo].[tbl_CorporatePrivilegeTierModel]
                                           SET [isVIP] = '0' " +
                                        "WHERE PrivilegeID ='" + emp.privilegeID + "' and CorporateID='" + emp.CorporateID + "' ";
                        db.AUIDB_WithParam(Update);

                    }
                }
                result.Status = "Successfully Added";

            }

            return Ok(result);
        }

    }
}
