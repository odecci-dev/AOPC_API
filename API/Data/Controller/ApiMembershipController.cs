using AuthSystem.Manager;
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
using AuthSystem.ViewModel;

namespace AuthSystem.Data.Controller
{
    //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [Authorize("ApiKey")]
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class ApiMembershipController : ControllerBase
    {
        DbManager db = new DbManager();
        private readonly AppSettings _appSettings;
        private ApplicationDbContext _context;
        private ApiGlobalModel _global = new ApiGlobalModel();
        private readonly JwtAuthenticationManager jwtAuthenticationManager;


        public ApiMembershipController(ApplicationDbContext context)
        {
   
            _context = context;
   
        }
      
        [HttpGet]
        public async Task<IActionResult> MembershipList()
        {
            string sql = $@"SELECT        tbl_MembershipModel.Id, tbl_MembershipModel.Name AS MembershipName, tbl_MembershipModel.Description, tbl_MembershipModel.DateUsed AS DateStarted, tbl_MembershipModel.DateEnded, 
                         tbl_MembershipModel.DateCreated, tbl_MembershipModel.MembershipID, tbl_StatusModel.Name AS Status, tbl_MembershipModel.UserCount, tbl_MembershipModel.VIPCount, tbl_MembershipModel.MembershipCard, 
                         tbl_MembershipModel.VIPCard, tbl_MembershipModel.QRFrame, tbl_MembershipModel.VIPBadge
FROM            tbl_MembershipModel INNER JOIN
                         tbl_StatusModel ON tbl_MembershipModel.Status = tbl_StatusModel.Id
WHERE        (tbl_MembershipModel.Status = 5)
ORDER BY tbl_MembershipModel.Id DESC";
            var result = new List<MembershipVM>();
            DataTable table = db.SelectDb(sql).Tables[0];
            foreach (DataRow dr in table.Rows)
            {
                string membership_path = "";
                string vipcard_path = "";
                string qrframe_path = "";
                string vipbadge_path = "";
                if (dr["MembershipCard"].ToString() == null || dr["MembershipCard"].ToString() == string.Empty) 
                {
                    membership_path = "https://www.svgrepo.com/download/83325/id-card.svg";
                }
                else
                {
                    membership_path = dr["MembershipCard"].ToString();
                }

                if (dr["VIPCard"].ToString() == null || dr["VIPCard"].ToString() == "")
                {
                    vipcard_path = "https://www.svgrepo.com/download/83325/id-card.svg";
                }
                else
                {
                    vipcard_path =  dr["VIPCard"].ToString();
                }
                if (dr["QRFrame"].ToString() == null || dr["QRFrame"].ToString() == string.Empty)
                {
                    qrframe_path = "https://www.svgrepo.com/download/83325/id-card.svg";
                }
                else
                {
                    qrframe_path = dr["QRFrame"].ToString();
                }
                if (dr["VIPBadge"].ToString() == null || dr["VIPBadge"].ToString() == string.Empty)
                {
                    vipbadge_path = "https://www.svgrepo.com/download/83325/id-card.svg";
                }
                else
                {
                    vipbadge_path = dr["VIPBadge"].ToString();
                }
                if (dr["MembershipName"].ToString() !="ALL TIER")
                {
                    var item = new MembershipVM();
                    item.Id = dr["Id"].ToString();
                    item.MembershipName = dr["MembershipName"].ToString();
                    item.Description = dr["Description"].ToString();
                    item.DateStarted = Convert.ToDateTime(dr["DateStarted"].ToString()).ToString("MM/dd/yyyy");
                    item.DateEnded = Convert.ToDateTime(dr["DateEnded"].ToString()).ToString("MM/dd/yyyy");
                    item.DateCreated = Convert.ToDateTime(dr["DateCreated"].ToString()).ToString("MM/dd/yyyy");
                    item.MembershipID = dr["MembershipID"].ToString();
                    item.Status = dr["Status"].ToString();
                    item.UserCount = int.Parse(dr["UserCount"].ToString());
                    item.VIPCount = int.Parse(dr["VIPCount"].ToString());
                    item.MembershipCard = membership_path;
                    item.VIPBadge = vipbadge_path;
                    item.VIPCard = vipcard_path;
                    item.QRFrame = qrframe_path;

                    result.Add(item);
                }
                
            }
            return Ok(result);
        }

        [HttpPost]
        public async  Task<IActionResult> SaveNewMembership(MembershipModel data)
        {
            try
            {
                string result = "";
                GlobalVariables gv = new GlobalVariables();
                _global.Status = gv.MemberShipRegistration(data,_context);
            }

            catch (Exception ex)
            {
                string status = ex.GetBaseException().ToString();
            }
             return Content(_global.Status);
        }
        public class MembershipID
        {
            public int Id { get; set; }

        }
        [HttpPost]
        public async  Task<IActionResult> MembershipFilterbyID(MembershipID data)
        {
            var result = new List<MembershipModelVM>();
            try
            {
                string sql = $@"SELECT        Id, Name AS MembershipName, Description, MembershipID, UserCount, VIPCount, DateCreated
                                FROM            tbl_MembershipModel
                                WHERE        (Id = '"+data.Id+"')";

                DataTable table = db.SelectDb(sql).Tables[0];
              
                if (table.Rows.Count != 0)
                {
           
                    foreach (DataRow dr in table.Rows)
                    {
                        var item = new MembershipModelVM();
                        item.Id = int.Parse(dr["Id"].ToString());
                        item.MembershipName = dr["MembershipName"].ToString();
                        item.Description = dr["Description"].ToString();
                        item.MembershipID = dr["MembershipID"].ToString();
                        item.UserCount = dr["UserCount"].ToString();
                        item.VIPCount = dr["VIPCount"].ToString();
                        item.DateCreated = Convert.ToDateTime(dr["DateCreated"].ToString()).ToString("MM/dd/yyyy") ;
                        result.Add(item);

                    }
                }
            }

            catch (Exception ex)
            {
                string status = ex.GetBaseException().ToString();
            }
            return Ok(result);
        }

        [HttpPut]
        public async Task<IActionResult> UpdateMembershipInfo(MembershipModel data)
        {
            try
            {
                string result = "";
                GlobalVariables gv = new GlobalVariables();
                _global.Status = gv.MembershipUpdateInfo(data ,_context);
            }

            catch (Exception ex)
            {
                string status = ex.GetBaseException().ToString();
            }
            return Content(_global.Status);
        }
        [HttpDelete]
        public async Task<IActionResult> DeleteMembershipInfo(int id)
        {
            try
            {
                var result = await _context.tbl_MembershipModel.FindAsync(id);
                _context.tbl_MembershipModel.Remove(result);
                await _context.SaveChangesAsync();
                _global.Status = "Successfully Deleted.";

            }
            catch (Exception ex)
            {
                _global.Status = ex.GetBaseException().ToString();
            }

            return Content(_global.Status);
        }
        public class Registerstats
        {
            public string Status { get; set; }

        }
        [HttpPost]
        public IActionResult SaveMembershipTier(MembershipVM data)
        {


            string sql = "";
            sql = $@"select * from tbl_MembershipModel where id ='" + data.Id + "'";
            DataTable dt = db.SelectDb(sql).Tables[0];
            var result = new Registerstats();
            string imgfile = "";
            string membership_path = "";
            string vipcard_path = "";
            string qrframe_path = "";
            string vipbadge_path = "";
            if (data.MembershipCard == null)
            {
                membership_path = "https://www.svgrepo.com/download/83325/id-card.svg";
            }
            else
            {
                membership_path = "https://www.alfardanoysterprivilegeclub.com/assets/img/MembershipCard/" + data.MembershipCard.Replace(" ", "%20");
            }

            if (data.VIPCard == null)
            {
                vipcard_path = "https://www.svgrepo.com/download/83325/id-card.svg";
            }
            else
            {
                vipcard_path = "https://www.alfardanoysterprivilegeclub.com/assets/img/VIPCard/" + data.VIPCard.Replace(" ", "%20");
            }
            if (data.QRFrame == null)
            {
                qrframe_path = "https://www.svgrepo.com/download/83325/id-card.svg";
            }
            else
            {
                qrframe_path = "https://www.alfardanoysterprivilegeclub.com/assets/img/QRFrame/" + data.QRFrame.Replace(" ", "%20");
            }
            if (data.VIPBadge == null)
            {
                vipbadge_path = "https://www.svgrepo.com/download/83325/id-card.svg";
            }
            else
            {
                vipbadge_path = "https://www.alfardanoysterprivilegeclub.com/assets/img/VIPBadge/" + data.VIPBadge.Replace(" ", "%20");
            }
            if (dt.Rows.Count == 0)
            {
                sql = $@"select * from tbl_MembershipModel where Name ='" + data.MembershipName + "'";
                DataTable dt2 = db.SelectDb(sql).Tables[0];
                if (dt2.Rows.Count == 0)
                {


           
                    string Insert = $@"insert into tbl_MembershipModel (Name,Description,DateUsed,DateEnded,UserCount,VIPCount,MembershipCard,VIPCard,QRFrame,VIPBadge,Status) values 
                                   ('" + data.MembershipName + "'," +
                                   "'" + data.Description + "'," +
                                   "'" + data.DateStarted + "'," +
                                   "'" + data.DateEnded + "'," +
                                   "'" + data.UserCount + "'," +
                                   "'" + data.VIPCount + "'," +
                                   "'" + membership_path+ "'," +
                                   "'" + vipbadge_path + "'," +
                                   "'" + qrframe_path + "'," +
                                   "'" + vipbadge_path + "'," +
                                   "5) ";
                    db.AUIDB_WithParam(Insert);
                    result.Status = "Successfully Added";

                    return Ok(result);
                }
                else
                {

                    result.Status = "Duplicate Entry";

                    return BadRequest(result);
                }
                   

            }
            else
            {

              
                string Update = $@"update tbl_MembershipModel
                                set Name='" + data.MembershipName + "', " +
                                "Description='" + data.Description + "' , " +
                                "DateUsed='" + data.DateStarted + "', " +
                                "MembershipCard='" + membership_path + "', " +
                                "VIPCard='" + vipcard_path + "', " +
                                "QRFrame='" + qrframe_path + "', " +
                                "VIPBadge='" + vipbadge_path + "', " +
                                "DateEnded='" + data.DateEnded +
                                "', UserCount='" + data.UserCount + "', " +
                                "VIPCount='" + data.VIPCount + "' " +
                                "where id='"+data.Id+"'";
                db.AUIDB_WithParam(Update);
                result.Status = "Successfully Updated";

                return Ok(result);
            }


            return Ok(result);
        }
        public class DeleteMem
        {

            public int Id { get; set; }
        }
        [HttpPost]
        public IActionResult DeleteMemship(DeleteMem data)
        {

            string sql = $@"select * from tbl_MembershipModel where id ='" + data.Id + "'";
            DataTable dt = db.SelectDb(sql).Tables[0];
            var result = new Registerstats();
            string imgfile = "";
            if (dt.Rows.Count != 0)
            {

                string OTPInsert = $@"update tbl_MembershipModel set Status = 6 where id ='" + data.Id + "'";
                db.AUIDB_WithParam(OTPInsert);
                result.Status = "Succesfully deleted";

                return Ok(result);

            }
            else
            {
                result.Status = "Error";

                return BadRequest(result);

            }

        }
    }
}
