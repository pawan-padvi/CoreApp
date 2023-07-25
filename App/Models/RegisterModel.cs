using JW;
using static App.Models.DropdownModel;

namespace App.Models
{
    public class RegisterModel
    {
        public String? FirstName { get; set; }
        public String? Id { get; set; } = null;
        public String? LastName { get; set; }
        public String? Email { get; set; } = null;
        public String? Password { get; set; }
        public String? PasswordConfirmed { get; set; }
        public String? DbCode { get; set; }
        public String? DbMsg { get; set; }
        public List<RegisterModel>? ListRegister { get; set; }
        public string? CreatedBy { get; set; }
        public IFormFile? imageProfile { get; set; }
        public string? Profile1Url { get; set; }
        public IFormFile? imageProfile2 { get; set; }
        public string? Profile2Url { get; set; }
        public Pager Pager { get; set; }
        public int PageSizeId { get; set; }
        public string PageSizeValue { get; set; }
        public string SearchString { get; set; }
        public List<PageSizeDdl> lstPageSizeDdl { get; set; }
    }
}
