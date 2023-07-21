using System.ComponentModel.DataAnnotations;
namespace App.Models
{
    public class LoginModel
    {
        [Required(ErrorMessage = "This field cannot be left blank" )]
        [EmailAddress]
        public String? Email { get; set; }
        [Required ( ErrorMessage = "This field cannot be left blank" )]
        public String? Password { get; set; }=null;
        public bool AcceptConditions { get; set; }
        public String? DbCode { get; set; }
        public String? DbMsg { get;set; }
        public String ? Id { get; set;}
    }
}
