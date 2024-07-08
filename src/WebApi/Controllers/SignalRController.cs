using CleanArch.eCode.Shared.Authorization;
using CleanArch.eCode.WebApi.SignalR.UseCases;
using Microsoft.AspNetCore.Mvc;

namespace CleanArch.eCode.WebApi.Controllers;

public class SignalrController(ICurrentUser currentUser) : VersionedApiController
{
    [HttpGet]
    public async Task<IActionResult> GetAsync([FromQuery] GetNotifications request)
    {
        var canViewAll = currentUser.HasPermission(Permissions.System.Notification);

        if (!canViewAll) // view all when uses can send notify
        {
            request.ToUser = currentUser.UserId;
        }

        var res = await Mediator.Send(request);
        return res.Ok();
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetAsync(string id)
    {
        var res = await Mediator.Send(new GetNotificationById(id));

        if (res.Succeeded && res.Data.ToUserId == currentUser.UserId)
            await Mediator.Send(new MarkNotificationAsRead(id));

        return res.Ok();
    }

    [HttpGet("{userId}/count_unread")]
    public async Task<IActionResult> CountUnreadAsync(string userId)
    {
        var res = await Mediator.Send(new CountUnreadUserNotification(userId));
        return res.Ok();
    }

    [HttpPost]
    public async Task<IActionResult> PostAsync([FromBody] AddNotification request)
    {
        var res = await Mediator.Send(request);
        return res.Ok();
    }
}