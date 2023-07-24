using Microsoft.AspNetCore.Routing.Constraints;

namespace App.Models
{
    public class FileUploadModel
    {
        public IFormFile? imageUpload { get; set; }
        public String? ImagePath { get; set; }
    }
}
