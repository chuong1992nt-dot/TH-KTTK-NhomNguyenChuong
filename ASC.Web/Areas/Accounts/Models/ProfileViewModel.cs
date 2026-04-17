using Microsoft.AspNetCore.Http;

namespace ASC.Web.Areas.Accounts.Models
{
    public class ProfileViewModel
    {
        public string Email { get; set; }
        public string Name { get; set; }
        public string PhoneNumber { get; set; }
        public IFormFile ProfilePhoto { get; set; }
        public string ProfilePictureUrl { get; set; }
    }
}