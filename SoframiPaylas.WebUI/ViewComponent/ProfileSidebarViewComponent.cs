namespace SoframiPaylas.WebUI.ViewComponent;
using Microsoft.AspNetCore.Mvc;
using SoframiPaylas.WebUI.Models;

public class ProfileSidebarViewComponent : ViewComponent
{
    public IViewComponentResult Invoke(string profilePicture, string userName, string activePage)
    {
        var model = new ProfileSidebarViewModel
        {
            ProfilePicture = profilePicture,
            UserName = userName,
            ActivePage = activePage
        };

        return View(model);
    }
}


