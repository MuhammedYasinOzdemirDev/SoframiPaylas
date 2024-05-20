namespace SoframiPaylas.WebUI.ViewComponent;
using Microsoft.AspNetCore.Mvc;
using SoframiPaylas.WebUI.ExternalService.StorageService;



public class UserProfileViewComponent : ViewComponent
{
    private readonly IUserService _userService;

    public UserProfileViewComponent(IUserService userService)
    {
        _userService = userService;
    }

    public async Task<IViewComponentResult> InvokeAsync()
    {
        var url = _userService.GetProfilePicture();
        return View((object)url);
    }
}
