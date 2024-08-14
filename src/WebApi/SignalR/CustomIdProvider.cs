using Light.Identity;
using Microsoft.AspNetCore.SignalR;

namespace CleanArch.WebApi.SignalR;

public class CustomIdProvider : IUserIdProvider
{
    public string? GetUserId(HubConnectionContext connection)
    {
        var userId = connection.User?.FindFirst(ClaimTypes.UserId)?.Value;
        return userId;
    }
}