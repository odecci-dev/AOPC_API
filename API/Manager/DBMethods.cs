using AuthSystem.Models;
using AuthSystem.ViewModel;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Text;
using static AuthSystem.Data.Controller.ApiAuditTrailController;
using static AuthSystem.Data.Controller.ApiNotifcationController;

namespace AuthSystem.Manager
{
    public class DBMethods
    {
        string sql = "";
        DbManager db = new DbManager();


        public List<VendorVM> GetVendorDetails()
        {
            string sql = $@"SELECT   tbl_VendorModel.Id,tbl_VendorModel.VendorName, tbl_VendorModel.Description, tbl_VendorModel.Services, tbl_VendorModel.WebsiteUrl, tbl_VendorModel.FeatureImg, tbl_VendorModel.Gallery, tbl_VendorModel.Cno, tbl_VendorModel.Email, 
                         tbl_VendorModel.VideoUrl, tbl_VendorModel.VrUrl, tbl_VendorModel.DateCreated, tbl_VendorModel.VendorID, tbl_VendorModel.FileUrl, tbl_VendorModel.Map, tbl_VendorModel.VendorLogo, 
                         tbl_BusinessTypeModel.BusinessTypeName, tbl_BusinessLocationModel.Country, tbl_BusinessLocationModel.City, tbl_BusinessLocationModel.PostalCode, tbl_VendorModel.Address, tbl_VendorModel.Id, 
                         tbl_BusinessLocationModel.City AS location,  tbl_StatusModel.Name AS Status, tbl_VendorModel.BusinessLocationID , tbl_VendorModel.BusinessTypeId
FROM           tbl_VendorModel INNER JOIN
                         tbl_BusinessTypeModel ON tbl_VendorModel.BusinessTypeId = tbl_BusinessTypeModel.Id LEFT OUTER JOIN
                         tbl_BusinessLocationModel ON tbl_VendorModel.BusinessLocationID = tbl_BusinessLocationModel.Id LEFT OUTER JOIN
                         tbl_StatusModel ON tbl_VendorModel.Status = tbl_StatusModel.Id  
WHERE        (tbl_VendorModel.Status = 5)
ORDER BY tbl_VendorModel.Id DESC";
            var result = new List<VendorVM>();
            DataTable table = db.SelectDb(sql).Tables[0];
            foreach (DataRow dr in table.Rows)
            {
                var item = new VendorVM();
                item.VendorName = dr["VendorName"].ToString();
                item.Description = dr["Description"].ToString();
                item.Services = dr["Services"].ToString();
                item.WebsiteUrl = dr["WebsiteUrl"].ToString();
                item.FeatureImg = dr["FeatureImg"].ToString();
                item.Gallery = dr["Gallery"].ToString();
                item.Cno = dr["Cno"].ToString();
                item.Email = dr["Email"].ToString();
                item.VideoUrl = dr["VideoUrl"].ToString();
                item.VrUrl = dr["VrUrl"].ToString();
                item.VendorID = dr["VendorID"].ToString();
                item.FileUrl = dr["FileUrl"].ToString();
                item.Map = dr["Map"].ToString();
                item.VendorLogo = dr["VendorLogo"].ToString();
                item.BusinessTypeName = dr["BusinessTypeName"].ToString();
                item.Country = dr["Country"].ToString();
                item.City = dr["City"].ToString();
                item.PostalCode = dr["PostalCode"].ToString();
                //item.BusinessName = dr["BusinessName"].ToString();
                item.Address = dr["Address"].ToString();
                item.Id = dr["Id"].ToString();
                item.DateCreated = Convert.ToDateTime(dr["DateCreated"].ToString()).ToString("MM-dd-yyyy");
                item.Status = dr["Status"].ToString();
                item.Vendorlocation = dr["location"].ToString();
                item.BID = dr["BusinessLocationID"].ToString();
                item.BtypeID = dr["BusinessTypeId"].ToString();

                result.Add(item);
            }
            return result;
        }
        public List<NotificationVM> GetNotificationDetails()
        {
            string sql = $@"SELECT        tbl_NotificationModel.Details, tbl_NotificationModel.isRead, tbl_NotificationModel.DateCreated,Concat(UsersModel.Fname,' ', UsersModel.Lname) as Fullname, tbl_NotificationModel.Id, tbl_NotificationModel.EmployeeID, tbl_NotificationModel.Module, tbl_NotificationModel.ItemID, 
                         tbl_NotificationModel.EmailStatus
                         FROM            tbl_NotificationModel INNER JOIN
                         UsersModel ON tbl_NotificationModel.EmployeeID = UsersModel.EmployeeID order by id desc";
            var result = new List<NotificationVM>();
            DataTable table = db.SelectDb(sql).Tables[0];

            foreach (DataRow dr in table.Rows)
            {
                string read = dr["isRead"].ToString() == "1" ? "Read" : "Unread";

                var item = new NotificationVM();
                item.Id = dr["Id"].ToString();
                item.EmployeeID = dr["EmployeeID"].ToString();
                item.Fullname = dr["Fullname"].ToString();
                item.Details = dr["Details"].ToString();
                item.Module = dr["Module"].ToString();
                item.ItemID = dr["ItemID"].ToString();
                item.EmailStatus = dr["EmailStatus"].ToString();
                item.isRead = read;
                item.DateCreated = DateTime.Parse(dr["DateCreated"].ToString()).ToString("MM-dd-yyyy");

                result.Add(item);
            }

            return result;
        }

        public List<Audittrailvm> GetAuditTrailList()
        {

            string sql = $@"SELECT  top(1300)      tbl_audittrailModel.Id, tbl_audittrailModel.Actions, tbl_audittrailModel.Module, tbl_audittrailModel.DateCreated, tbl_StatusModel.Name AS status, 
                        case when UsersModel.EmployeeID is null then 'Alfardan-Admin' else UsersModel.EmployeeID end as EmployeeID , 
                        case when UsersModel.Fname is null then 'Alfardan' else UsersModel.Fname end Fname, 
                        case when UsersModel.Lname is null then 'Administrator' else UsersModel.Lname end Lname, 
                        case when tbl_PositionModel.Name is null then 'System Administrator' else tbl_PositionModel.Name end AS PositionName, 
                        case when tbl_CorporateModel.CorporateName is null then 'Alfardan Oyster Privilege Club' else  tbl_CorporateModel.CorporateName end CorporateName, 
                        case when tbl_UserTypeModel.UserType is null then 'ADMIN' else tbl_UserTypeModel.UserType end UserType
                         FROM            tbl_audittrailModel LEFT OUTER JOIN
                         tbl_StatusModel ON tbl_audittrailModel.status = tbl_StatusModel.Id LEFT OUTER JOIN
                         UsersModel ON tbl_audittrailModel.EmployeeID = UsersModel.EmployeeID LEFT OUTER JOIN
                         tbl_PositionModel ON UsersModel.PositionID = tbl_PositionModel.Id LEFT OUTER JOIN
                         tbl_CorporateModel ON UsersModel.CorporateID = tbl_CorporateModel.Id LEFT OUTER JOIN
                         tbl_UserTypeModel ON UsersModel.Type = tbl_UserTypeModel.Id order by id desc";
            var result = new List<Audittrailvm>();
            DataTable table = db.SelectDb(sql).Tables[0];

            foreach (DataRow dr in table.Rows)
            {
                var item = new Audittrailvm();
                item.Id = int.Parse(dr["id"].ToString());
                item.Actions = dr["Actions"].ToString();
                item.Module = dr["Module"].ToString();
                item.DateCreated = Convert.ToDateTime(dr["DateCreated"].ToString()).ToString("MM/dd/yyyy hh:mm:ss tt");
                item.status = dr["status"].ToString();
                item.EmployeeID = dr["EmployeeID"].ToString();
                item.FullName = dr["Fname"].ToString() + " " + dr["Lname"].ToString();
                item.PositionName = dr["PositionName"].ToString();
                item.CorporateName = dr["CorporateName"].ToString();
                item.UserType = dr["UserType"].ToString();
                result.Add(item);
            }

            return result;
        }

        public List<UserVM> GetCorporateAdminUserList(string id)
        {



            string sql = $@"SELECT        UsersModel.Username, UsersModel.Fname, UsersModel.Lname, UsersModel.Email, UsersModel.Gender, UsersModel.EmployeeID, tbl_PositionModel.Name AS Position, tbl_CorporateModel.CorporateName, 
                         tbl_UserTypeModel.UserType, UsersModel.Fullname, UsersModel.Id, UsersModel.DateCreated, tbl_PositionModel.Id AS PositionID, tbl_CorporateModel.Id AS CorporateID, tbl_StatusModel.Name AS status, UsersModel.isVIP, 
                         UsersModel.FilePath
                         FROM            UsersModel LEFT OUTER JOIN
                         tbl_CorporateModel ON UsersModel.CorporateID = tbl_CorporateModel.Id LEFT OUTER JOIN
                         tbl_PositionModel ON UsersModel.PositionID = tbl_PositionModel.Id LEFT OUTER JOIN
                         tbl_UserTypeModel ON UsersModel.Type = tbl_UserTypeModel.Id LEFT OUTER JOIN
                         tbl_StatusModel ON UsersModel.Active = tbl_StatusModel.Id
                         WHERE        (UsersModel.Active IN (1, 2, 9, 10)) AND (UsersModel.Type = 3) AND (UsersModel.CorporateID = '" + id + "') " +
                         "order by UsersModel.Id desc";
            var result = new List<UserVM>();
            DataTable table = db.SelectDb(sql).Tables[0];

            foreach (DataRow dr in table.Rows)
            {
                var item = new UserVM();
                item.Id = int.Parse(dr["id"].ToString());
                item.Fullname = dr["Fname"].ToString() + " " + dr["Lname"].ToString();
                item.Username = dr["Username"].ToString();
                item.Fname = dr["Fname"].ToString();
                item.Lname = dr["Lname"].ToString();
                item.Email = dr["Email"].ToString();
                item.Gender = dr["Gender"].ToString();
                item.EmployeeID = dr["EmployeeID"].ToString();
                item.Position = dr["Position"].ToString();
                item.Corporatename = dr["Corporatename"].ToString();
                item.UserType = dr["UserType"].ToString();
                item.DateCreated = Convert.ToDateTime(dr["DateCreated"].ToString()).ToString("MM/dd/yyyy");
                item.CorporateID = dr["CorporateID"].ToString();
                item.PositionID = dr["PositionID"].ToString();
                item.status = dr["status"].ToString();
                item.FilePath = dr["FilePath"].ToString();
                item.isVIP = dr["isVIP"].ToString();

                result.Add(item);
            }

            return result;
        }
    }
}
