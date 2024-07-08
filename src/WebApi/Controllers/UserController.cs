using CleanArch.eCode.Shared.Authorization;
using CleanArch.eCode.WebApi.Models;
using Light.ActiveDirectory.Interfaces;
using Light.Identity;
using Microsoft.AspNetCore.Mvc;

namespace CleanArch.eCode.WebApi.Controllers;

[MustHavePermission(Permissions.Users.View)]
public class UserController(
    IUserService userService,
    IActiveDirectoryService activeDirectoryService) : VersionedApiController
{
    [HttpGet]
    public async Task<IActionResult> GetAsync()
    {
        var res = await userService.GetAllAsync();
        return res.Ok();
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetAsync(string id)
    {
        var res = await userService.GetByIdAsync(id);
        return res.Ok();
    }

    [HttpGet("by_username/{username}")]
    public async Task<IActionResult> GetByUsernameAsync(string username)
    {
        var res = await userService.GetByUserNameAsync(username);
        return res.Ok();
    }

    [HttpPost]
    [MustHavePermission(Permissions.Users.Create)]
    public async Task<IActionResult> PostAsync(CreateUserRequest request)
    {
        var res = await userService.CreateAsync(request);
        return res.Ok();
    }

    [HttpPut("{id}")]
    [MustHavePermission(Permissions.Users.Update)]
    public async Task<IActionResult> PutAsync(string id, UserDto request)
    {
        if (id != request.Id)
        {
            return Result.BadRequest("Validate User ID not match").Ok();
        }

        var res = await userService.UpdateAsync(request);
        return res.Ok();
    }

    [HttpDelete("{id}")]
    [MustHavePermission(Permissions.Users.Delete)]
    public async Task<IActionResult> DeleteAsync(string id)
    {
        var res = await userService.DeleteAsync(id);
        return res.Ok();
    }

    [HttpPut("force_password")]
    [MustHavePermission(Permissions.Users.Update)]
    public async Task<IActionResult> ForcePasswordAsync(ForcePasswordRequest request)
    {
        var res = await userService.ForcePasswordAsync(request.Id, request.Password);
        return res.Ok();
    }

    [HttpGet("get_domain_user/{userName}")]
    public async Task<IActionResult> GetDomainUserAsync([FromRoute] string userName)
    {
        var res = await activeDirectoryService.GetByUserNameAsync(userName);
        return res.Ok();
    }
}
