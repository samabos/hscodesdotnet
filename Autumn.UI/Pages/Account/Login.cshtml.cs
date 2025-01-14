using Auth0.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Autumn.UI.Pages.Account
{
    public class LoginModel : PageModel
    {
        public async Task OnGet(string returnUrl = "/admin")
        {
            var authenticationProperties = new LoginAuthenticationPropertiesBuilder()
               // Indicate here where Auth0 should redirect the user after a login.
               // Note that the resulting absolute Uri must be added to the
               // **Allowed Callback URLs** settings for the app.
               .WithRedirectUri(returnUrl)
               .Build();

            await HttpContext.ChallengeAsync(Auth0Constants.AuthenticationScheme, authenticationProperties);
        }
    }
}
