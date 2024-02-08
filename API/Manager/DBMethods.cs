using AuthSystem.Models;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Text;
namespace AuthSystem.Manager
{
    public class DBMethods
    {
        string sql = "";
        DbManager db = new DbManager();


        public List<VendorVM> GetVendorDetails()
        {
            string sql = $@"SELECT  tbl_VendorModel.VendorName, tbl_VendorModel.Description, tbl_VendorModel.Services, tbl_VendorModel.WebsiteUrl, tbl_VendorModel.FeatureImg, tbl_VendorModel.Gallery, tbl_VendorModel.Cno, tbl_VendorModel.Email, 
                         tbl_VendorModel.VideoUrl, tbl_VendorModel.VrUrl, tbl_VendorModel.DateCreated, tbl_VendorModel.VendorID, tbl_VendorModel.FileUrl, tbl_VendorModel.Map, tbl_VendorModel.VendorLogo, 
                         tbl_BusinessTypeModel.BusinessTypeName, tbl_BusinessLocationModel.Country, tbl_BusinessLocationModel.City, tbl_BusinessLocationModel.PostalCode, tbl_VendorModel.Address, tbl_VendorModel.Id, 
                         tbl_BusinessModel.Address AS location, tbl_BusinessModel.BusinessName, tbl_StatusModel.Name AS Status, tbl_VendorModel.BusinessLocationID , tbl_VendorModel.BusinessTypeId
FROM            tbl_VendorModel INNER JOIN
                         tbl_BusinessTypeModel ON tbl_VendorModel.BusinessTypeId = tbl_BusinessTypeModel.Id LEFT OUTER JOIN
                         tbl_BusinessModel ON tbl_VendorModel.BusinessLocationID = tbl_BusinessModel.Id LEFT OUTER JOIN
                         tbl_BusinessLocationModel ON tbl_BusinessModel.LocationId = tbl_BusinessLocationModel.Id LEFT OUTER JOIN
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
                item.BusinessName = dr["BusinessName"].ToString();
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
        
    }
}
