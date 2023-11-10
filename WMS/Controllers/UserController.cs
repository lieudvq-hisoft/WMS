using Confluent.Kafka;
using Data.Common.PaginationModel;
using Data.Enums;
using Data.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Services.Core;
using Services.Utils;

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

    [Authorize(AuthenticationSchemes = "Bearer")]
    [Authorize(Roles = "Admin")]
    [HttpPost]
    public async Task<ActionResult> Create([FromBody] UserCreateModel model)
    {
        var result = await _userService.Create(model);
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
    [HttpGet("Profile")]
    public async Task<ActionResult> GetProfile()
    {
        var result = await _userService.Profile(Guid.Parse(User.GetId()));
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

    [Authorize(AuthenticationSchemes = "Bearer")]
    [HttpPut("ChangePassword")]
    public async Task<ActionResult> ChangePassword(ChangePasswordModel model)
    {
        var result = await _userService.ChangePassword(model, Guid.Parse(User.GetId()));
        if (result.Succeed) return Ok(result.Data);
        return BadRequest(result.ErrorMessage);
    }

    [HttpPost("ForgotPassword")]
    public async Task<ActionResult> ForgotPassword([FromBody] ForgotPasswordModel model)
    {
        var result = await _userService.ForgotPassword(model);
        if (result.Succeed) return Ok(result.Data);
        return BadRequest(result.ErrorMessage);
    }

    [HttpPost("ResetPassword")]
    public async Task<ActionResult> ResetPassword([FromBody] ResetPasswordModel model)
    {
        var result = await _userService.ResetPassword(model);
        if (result.Succeed) return Ok(result.Data);
        return BadRequest(result.ErrorMessage);
    }

    [Authorize(AuthenticationSchemes = "Bearer")]
    [Authorize(Roles = "Admin")]
    [HttpPut("Active/{userId}")]
    public async Task<ActionResult> ActiveUser(Guid userId)
    {
        var result = await _userService.ActivateUser(userId);
        if (result.Succeed) return Ok(result.Data);
        return BadRequest(result.ErrorMessage);
    }

    [Authorize(AuthenticationSchemes = "Bearer")]
    [Authorize(Roles = "Admin")]
    [HttpPut("Deactive/{userId}")]
    public async Task<ActionResult> DeactiveUser(Guid userId)
    {
        var result = await _userService.DeactivateUser(userId);
        if (result.Succeed) return Ok(result.Data);
        return BadRequest(result.ErrorMessage);
    }

    [Authorize(AuthenticationSchemes = "Bearer")]
    [Authorize(Roles = "Admin")]
    [HttpPost("AssignRole")]
    public async Task<ActionResult> AssignRole(AssignRoleModel model)
    {
        var result = await _userService.AssignRole(model);
        if (result.Succeed) return Ok(result.Data);
        return BadRequest(result.ErrorMessage);
    }

    [Authorize(AuthenticationSchemes = "Bearer")]
    [Authorize(Roles = "Admin")]
    [HttpPost("UnassignRole")]
    public async Task<ActionResult> UnassignRole(AssignRoleModel model)
    {
        var result = await _userService.UnassignRole(model);
        if (result.Succeed) return Ok(result.Data);
        return BadRequest(result.ErrorMessage);
    }

    [Authorize(AuthenticationSchemes = "Bearer")]
    [HttpGet("Roles")]
    public async Task<ActionResult> GetRoleOfUser()
    {
        var result = await _userService.GetRoleOfUser(Guid.Parse(User.GetId()));
        if (result.Succeed) return Ok(result.Data);
        return BadRequest(result.ErrorMessage);
    }

    [Authorize(AuthenticationSchemes = "Bearer")]
    [Authorize(Roles = "Admin")]
    [HttpGet("UsersInRole/{roleName}")]
    public async Task<ActionResult> GetUsersInRole(String roleName)
    {
        var result = await _userService.GetUsersInRole(roleName);
        if (result.Succeed) return Ok(result.Data);
        return BadRequest(result.ErrorMessage);
    }
}
