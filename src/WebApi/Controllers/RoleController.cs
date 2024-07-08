﻿using CleanArch.eCode.Shared.Authorization;
using Light.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CleanArch.eCode.WebApi.Controllers;

[MustHavePermission(Permissions.Roles.View)]
public class RoleController(IRoleService roleService) : VersionedApiController
{
    [HttpGet]
    public async Task<IActionResult> GetAsync(CancellationToken cancellationToken)
    {
        return Ok(await roleService.GetAllAsync(cancellationToken));
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetAsync([FromRoute] string id)
    {
        var result = await roleService.GetByIdAsync(id);
        return result.Ok();
    }

    [HttpPost]
    [Authorize(Policy = Permissions.Roles.Create)]
    public async Task<IActionResult> CreateAsync([FromBody] CreateRoleRequest request)
    {
        var res = await roleService.CreateAsync(request);
        return res.Ok();
    }

    [HttpPut]
    [Authorize(Policy = Permissions.Roles.Update)]
    public async Task<IActionResult> UpdateAsync([FromBody] RoleDto request)
    {
        var res = await roleService.UpdateAsync(request);
        return res.Ok();
    }

    [HttpDelete("{id}")]
    [Authorize(Policy = Permissions.Roles.Delete)]
    public async Task<IActionResult> DeleteAsync([FromRoute] string id)
    {
        return Ok(await roleService.DeleteAsync(id));
    }
}
