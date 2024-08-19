using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace GroupMeetingASP.NETCoreWebApp.Filters
{
    public class SessionAuthorizeAttribute : Attribute, IAuthorizationFilter
    {
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            var user = context.HttpContext.Session.GetString("User");
            if (user == null)
            {
                context.Result = new RedirectToActionResult("Login", "RegisterUser", null);
            }
        }
    }
}
