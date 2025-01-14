using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Net.Mail;
using System.Security.Claims;
using System.Xml.Linq;

namespace Autumn.UI.Pages.Account
{
    [Authorize]
    public class ProfileModel : PageModel
    {
        public string? Name { get; set; }
        public string? EmailAddress { get; set; }
        public string? ProfileImage { get; set; }

        public void OnGet()
        {
            if (User != null && User.Identity != null)
            {
                Name = User.Identity.Name;
                EmailAddress = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value;
                ProfileImage = User.Claims.FirstOrDefault(c => c.Type == "picture")?.Value;
            }
        }
    }
}
