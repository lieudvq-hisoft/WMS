﻿using Data.Common.PaginationModel;
using Data.Enums;
using Data.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Services.Core;
using Services.Utils;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.Design;

namespace UserController.Controllers;

[Route("api/[controller]")]
[ApiController]
public class UserController : ControllerBase
{
    private readonly IUserService _userService;
    public UserController(IUserService userService)
    {
        _userService = userService;
    }

    [HttpPost("Login")]
    public async Task<ActionResult> Login([FromBody] LoginModel model)
    {
        var result = await _userService.Login(model);
        if (result.Succeed) return Ok(result.Data);
        return BadRequest(result.ErrorMessage);
    }

    [HttpPost("Register")]
    public async Task<ActionResult> Register([FromBody] UserCreateModel model)
    {
        var result = await _userService.Register(model);
        if (result.Succeed) return Ok(result.Data);
        return BadRequest(result.ErrorMessage);
    }

    [Authorize(AuthenticationSchemes = "Bearer")]
    [HttpGet]
    public async Task<ActionResult> Get([FromQuery] PagingParam<UserSortCriteria> paginationModel, [FromQuery] UserSearchModel searchModel)
    {
        var result = await _userService.Get(paginationModel, searchModel);
        if (result.Succeed) return Ok(result.Data);
        return BadRequest(result.ErrorMessage);
    }

    [Authorize(AuthenticationSchemes = "Bearer")]
    [HttpPut("Profile")]
    public async Task<ActionResult> UpdateProfile([FromBody] ProfileUpdateModel model)
    {
        var result = await _userService.UpdateProfile(model, Guid.Parse(User.GetId()));
        if (result.Succeed) return Ok(result.Data);
        return BadRequest(result.ErrorMessage);
    }
}
