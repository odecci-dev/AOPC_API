namespace API.Models
{
    public class FamilyMemberpagedModel
    {
        public string CurrentPage { get; set; }
        public string NextPage { get; set; }
        public string PrevPage { get; set; }
        public string TotalPage { get; set; }
        public string PageSize { get; set; }
        public string TotalRecord { get; set; }
        public List<FamilyMemberModel> data {  get; set; }
    }
}
