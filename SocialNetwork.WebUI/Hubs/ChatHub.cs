using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.SignalR;
using SocialNetwork.Business.Services.Abstracts;
using SocialNetwork.Entities.Entities;

namespace SocialNetwork.WebUI.Hubs
{
    public class ChatHub : Hub
    {
        private readonly UserManager<CustomIdentityUser> _userManager;
        private IHttpContextAccessor _contextAccessor;
        private ICustomIdentityUserService _customIdentityUserService;
        public ChatHub(UserManager<CustomIdentityUser> userManager, IHttpContextAccessor contextAccessor, ICustomIdentityUserService customIdentityUserService)
        {
            _userManager = userManager;
            _contextAccessor = contextAccessor;
            _customIdentityUserService = customIdentityUserService;
        }

        public override async Task OnConnectedAsync()
        {
            var user = await _userManager.GetUserAsync(_contextAccessor.HttpContext.User);
            var userItem = await _customIdentityUserService.GetByIdAsync(user.Id);

            userItem.IsOnline = true;
            await _customIdentityUserService.UpdateAsync(userItem);

            string info = user.UserName + " connected successfuly";
            await Clients.Others.SendAsync("Connect",info);

        }
        public override async Task OnDisconnectedAsync(Exception? exception)
        {
            var user = await _userManager.GetUserAsync(_contextAccessor.HttpContext.User);
            var userItem = await _customIdentityUserService.GetByIdAsync(user.Id);

            userItem.IsOnline = false;
            await _customIdentityUserService.UpdateAsync(userItem);

            string info = user.UserName + " disconnected successfuly";
            await Clients.Others.SendAsync("Disconnect", info);

        }

    }
}
