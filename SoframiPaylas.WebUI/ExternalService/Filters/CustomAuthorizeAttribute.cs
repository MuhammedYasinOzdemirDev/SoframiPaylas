using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.ViewFeatures;

namespace SoframiPaylas.WebUI.ExternalService.Filters;
public class CustomAuthorizeAttribute : TypeFilterAttribute
{
    public CustomAuthorizeAttribute() : base(typeof(CustomAuthorizeFilter))
    {
    }
}

public class CustomAuthorizeFilter : IAuthorizationFilter
{

    public void OnAuthorization(AuthorizationFilterContext context)
    {
        if (context == null || context.HttpContext == null || context.HttpContext.User == null || context.HttpContext.User.Identity == null)
        {
            context.Result = new RedirectToActionResult("Login", "Auth", null);
            return;
        }
        if (!context.HttpContext.User.Identity.IsAuthenticated)
        {
            context.Result = new RedirectToActionResult("Login", "Auth", new { returnUrl = context.HttpContext.Request.Path, message = "Lütfen giriş yapın" });
        }
    }
}