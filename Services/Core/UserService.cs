using AutoMapper;
using Confluent.Kafka;
using Data.Common.PaginationModel;
using Data.DataAccess;
using Data.DataAccess.Constant;
using Data.Entities;
using Data.Enums;
using Data.Model;
using Data.Models;
using Data.Utils;
using Data.Utils.Paging;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Services.Utils;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Services.Core;

public interface IUserService
{
    Task<ResultModel> Create(UserCreateModel model);
    Task<ResultModel> Login(LoginModel model);
    Task<ResultModel> Get(PagingParam<UserSortCriteria> paginationModel, UserSearchModel searchModel);
    Task<ResultModel> UpdateProfile(ProfileUpdateModel model, Guid userId);
    Task<ResultModel> ChangePassword(ChangePasswordModel model, Guid userId);
    Task<ResultModel> ResetPassword(ResetPasswordModel model);
    Task<ResultModel> ForgotPassword(ForgotPasswordModel model);
    Task<ResultModel> DeactivateUser(Guid userId);
    Task<ResultModel> ActivateUser(Guid userId);
    Task<ResultModel> AssignRole(AssignRoleModel model);
    Task<ResultModel> UnassignRole(AssignRoleModel model);
    Task<ResultModel> GetRoleOfUser(Guid userId);
    Task<ResultModel> GetUserRole(Guid userId);
    Task<ResultModel> GetUsersInRole(String name);
    Task<ResultModel> Profile(Guid id);
}
public class UserService : IUserService
{
    private readonly AppDbContext _dbContext;
    private readonly IMapper _mapper;
    private readonly IMailService _mailService;
    private readonly IConfiguration _configuration;
    private readonly UserManager<User> _userManager;
    private readonly SignInManager<User> _signInManager;
    private readonly RoleManager<Role> _roleManager;
    private readonly IProducer<Null, string> _producer;

    public UserService(AppDbContext dbContext, IMapper mapper, IConfiguration configuration, UserManager<User> userManager,
        SignInManager<User> signInManager, RoleManager<Role> roleManager,
        IMailService mailService, IProducer<Null, string> producer)
    {
        _dbContext = dbContext;
        _mapper = mapper;
        _configuration = configuration;
        _userManager = userManager;
        _signInManager = signInManager;
        _roleManager = roleManager;
        _mailService = mailService;
        _producer = producer;
    }

    public async Task<ResultModel> Login(LoginModel model)
    {

        var result = new ResultModel();
        try
        {
            var userByEmail = await _userManager.FindByEmailAsync(model.Email);
            if (userByEmail == null)
            {
                result.ErrorMessage = "Email not exists";
                result.Succeed = false;
                return result;
            }
            if (!userByEmail.IsActive)
            {
                result.Succeed = false;
                result.ErrorMessage = "User has been deactivated";
                return result;
            }
            var check = await _signInManager.CheckPasswordSignInAsync(userByEmail, model.Password, false);
            if (!check.Succeeded)
            {
                result.Succeed = false;
                result.ErrorMessage = "Password isn't correct";
                return result;
            }
            var userRoles = _dbContext.UserRoles.Where(ur => ur.UserId == userByEmail.Id).ToList();
            var roles = new List<string>();
            foreach (var userRole in userRoles)
            {
                var role = await _dbContext.Roles.FindAsync(userRole.RoleId);
                if (role != null) roles.Add(role.Name);
            }
            var token = await GetAccessToken(userByEmail, roles);
            result.Succeed = true;
            result.Data = token;
        }
        catch (Exception ex)
        {
            result.ErrorMessage = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
        }
        return result;
    }

    public async Task<ResultModel> Create(UserCreateModel model)
    {
        var result = new ResultModel();
        result.Succeed = false;
        try
        {
            var checkEmailExisted = await _userManager.FindByEmailAsync(model.Email);
            if (checkEmailExisted != null)
            {
                result.ErrorMessage = "Email already existed";
                result.Succeed = false;
                return result;
            }
            var checkUserNameExisted = await _userManager.FindByNameAsync(model.UserName);
            if (checkUserNameExisted != null)
            {
                result.ErrorMessage = "UserName already existed";
                result.Succeed = false;
                return result;
            }

            var userRole = new UserRole {};

            var role = await _dbContext.Roles.FirstOrDefaultAsync(r => r.NormalizedName == RoleNormalizedName.Staff);
            if (role == null)
            {
                var newRole = _mapper.Map<RoleCreateModel, Role>(new RoleCreateModel { Description = "Role for Staff", Name = "Staff", NormalizedName = RoleNormalizedName.Staff });
                _dbContext.Roles.Add(newRole);
                userRole.RoleId = newRole.Id;
            }
            else
            {
                userRole.RoleId = role.Id;
            }

            var user = _mapper.Map<UserCreateModel, User>(model);

            var checkCreateSuccess = await _userManager.CreateAsync(user, model.Password);

            if (!checkCreateSuccess.Succeeded)
            {
                result.ErrorMessage = checkCreateSuccess.ToString();
                result.Succeed = false;
                return result;
            }
            userRole.UserId = user.Id;
            _dbContext.UserRoles.Add(userRole);
            await _dbContext.SaveChangesAsync();
            var json = Newtonsoft.Json.JsonConvert.SerializeObject(_mapper.Map<UserModel>(user));
            await _producer.ProduceAsync("user-create-new", new Message<Null, string> { Value = json });
            _producer.Flush();

            result.Succeed = true;
            result.Data = user.Id;
        }
        catch (Exception ex)
        {
            result.ErrorMessage = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
        }
        return result;
    }

    public async Task<ResultModel> Get(PagingParam<UserSortCriteria> paginationModel, UserSearchModel searchModel)
    {
        ResultModel result = new ResultModel();
        try
        {
            var data = _dbContext.Users.Include(_ => _.UserRoles).ThenInclude(_ => _.Role).Where(delegate (User m)
            {
                if (
                    (MyFunction.ConvertToUnSign(m.FirstName ?? "").IndexOf(MyFunction.ConvertToUnSign(searchModel.SearchValue ?? ""), StringComparison.CurrentCultureIgnoreCase) >= 0) ||
                    (MyFunction.ConvertToUnSign(m.LastName ?? "").IndexOf(MyFunction.ConvertToUnSign(searchModel.SearchValue ?? ""), StringComparison.CurrentCultureIgnoreCase) >= 0) ||
                    (MyFunction.ConvertToUnSign(m.Address ?? "").IndexOf(MyFunction.ConvertToUnSign(searchModel.SearchValue ?? ""), StringComparison.CurrentCultureIgnoreCase) >= 0)
                    || (m.PhoneNumber.ToUpper().Contains(searchModel.SearchValue ?? "".ToUpper())
                    || (m.UserName.ToUpper().Contains(searchModel.SearchValue ?? "".ToUpper())
                    || (m.Email.ToUpper().Contains(Uri.UnescapeDataString(searchModel.SearchValue ?? "").ToUpper())
                    ))))
                    return true;
                else
                    return false;
            }).AsQueryable();

            var paging = new PagingModel(paginationModel.PageIndex, paginationModel.PageSize, data.Count());

            var uses = data.GetWithSorting(paginationModel.SortKey.ToString(), paginationModel.SortOrder);
            uses = uses.GetWithPaging(paginationModel.PageIndex, paginationModel.PageSize);
            var viewModels = _mapper.ProjectTo<UserModel>(uses);
            paging.Data = viewModels;
            result.Data = paging;
            result.Succeed = true;

        }
        catch (Exception e)
        {
            result.ErrorMessage = e.InnerException != null ? e.InnerException.Message : e.Message;
        }
        return result;
    }

    public async Task<ResultModel> UpdateProfile(ProfileUpdateModel model, Guid userId)
    {
        ResultModel result = new ResultModel();
        try
        {
            var data = _dbContext.User.Where(_ => _.Id == userId && !_.IsDeleted).FirstOrDefault();
            if (data == null)
            {
                result.ErrorMessage = "User not exists";
                result.Succeed = false;
                return result;
            }
            if (!data.IsActive)
            {
                result.Succeed = false;
                result.ErrorMessage = "User has been deactivated";
                return result;
            }
            if(model.FirstName != null)
            {
                data.FirstName = model.FirstName;
            }
            if (model.LastName != null)
            {
                data.LastName = model.LastName;
            }
            if (model.Address != null)
            {
                data.Address = model.Address;
            }
            if (model.PhoneNumber != null)
            {
                data.PhoneNumber = model.PhoneNumber;
            }
            data.DateUpdated = DateTime.Now;
            await _dbContext.SaveChangesAsync();

            var json = Newtonsoft.Json.JsonConvert.SerializeObject(_mapper.Map<UserModel>(data));
            await _producer.ProduceAsync("user-update", new Message<Null, string> { Value = json });
            _producer.Flush();
            result.Succeed = true;
            result.Data = _mapper.Map<UserModel>(data);
        }
        catch (Exception e)
        {
            result.ErrorMessage = e.InnerException != null ? e.InnerException.Message : e.Message;
        }
        return result;
    }

    public async Task<ResultModel> Profile(Guid id)
    {
        ResultModel result = new ResultModel();
        try
        {
            var data = _dbContext.User.Where(_ => _.Id == id && !_.IsDeleted).FirstOrDefault();
            if (data == null)
            {
                result.ErrorMessage = "User not exists";
                result.Succeed = false;
                return result;
            }
            if (!data.IsActive)
            {
                result.Succeed = false;
                result.ErrorMessage = "User has been deactivated";
                return result;
            }
            result.Succeed = true;
            var dataView = _mapper.Map<ProfileModel>(data);
            dataView.FullName = data.FirstName + " " + data.LastName;
            result.Data = dataView;
        }
        catch (Exception e)
        {
            result.ErrorMessage = e.InnerException != null ? e.InnerException.Message : e.Message;
        }
        return result;
    }

    public async Task<ResultModel> ChangePassword(ChangePasswordModel model, Guid userId)
    {
        var result = new ResultModel();

        try
        {
            var user = _dbContext.User.Where(_ => _.Email == model.Email && _.Id == userId && !_.IsDeleted).FirstOrDefault();

            if (!user.IsActive)
            {
                result.Succeed = false;
                result.ErrorMessage = "User has been deactivated";
                return result;
            }

            var check = await _userManager.ChangePasswordAsync(user, model.CurrentPassword, model.NewPassword);
            if (!check.Succeeded)
            {
                result.ErrorMessage = check.ToString() ?? "Change password failed";
                result.Succeed = false;
                return result;
            }
            result.Succeed = check.Succeeded;
            result.Data = "Change password successful";

        }
        catch (Exception e)
        {
            result.ErrorMessage = e.InnerException != null ? e.InnerException.Message : e.Message;
        }


        return result;
    }

    public async Task<ResultModel> ResetPassword(ResetPasswordModel model)
    {
        var result = new ResultModel();

        try
        {
            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user == null)
            {
                result.Succeed = false;
                result.ErrorMessage = "User not found";
                return result;
            }
            if (!user.IsActive)
            {
                result.Succeed = false;
                result.ErrorMessage = "User has been deactivated";
                return result;
            }
            var resetPassResult = await _userManager.ResetPasswordAsync(user, model.Token, model.Password);
            if (!resetPassResult.Succeeded)
            {
                result.Succeed = false;
                result.ErrorMessage = resetPassResult.ToString();
                return result;
            }
            result.Succeed = true;
            result.Data = "Reset password successful";
        }
        catch (Exception e)
        {
            result.ErrorMessage = e.InnerException != null ? e.InnerException.Message : e.Message;
        }

        return result;
    }

    public async Task<ResultModel> ForgotPassword(ForgotPasswordModel model)
    {
        var result = new ResultModel();

        try
        {
            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user == null)
            {
                result.Succeed = false;
                result.ErrorMessage = "User not found";
                return result;
            }
            if (!user.IsActive)
            {
                result.Succeed = false;
                result.ErrorMessage = "User has been deactivated";
                return result;
            }
            var token = await _userManager.GeneratePasswordResetTokenAsync(user);

            //string url = $"https://digitalcapstone.hisoft.vn/resetPassword?token={token}";
            //var confirmTokenLink = $"<a href={url}>Please click for reset password</a><div></div>";

            var email = new EmailInfoModel { Subject = "Reset password", To = model.Email, Text = token };
            result.Succeed = await _mailService.SendEmail(email);
            result.Data = result.Succeed ? "Password reset email has been sent" : "Password reset email sent failed";
        }
        catch (Exception e)
        {
            result.ErrorMessage = e.InnerException != null ? e.InnerException.Message : e.Message;
        }

        return result;
    }

    public async Task<ResultModel> DeactivateUser(Guid userId)
    {
        var result = new ResultModel();

        try
        {
            var user = _dbContext.User.Where(_ => _.Id == userId && !_.IsDeleted).FirstOrDefault();
            if (user == null)
            {
                result.Succeed = false;
                result.ErrorMessage = "User not found";
                return result;
            }

            if(!user.IsActive)
            {
                result.Succeed = false;
                result.ErrorMessage = "User has been deactivated";
                return result;
            }
            user.IsActive = false;
            user.DateUpdated = DateTime.Now;
            await _dbContext.SaveChangesAsync();
            result.Succeed = true;
            result.Data = user.Id;
        }
        catch (Exception e)
        {
            result.ErrorMessage = e.InnerException != null ? e.InnerException.Message : e.Message;
        }

        return result;
    }

    public async Task<ResultModel> ActivateUser(Guid userId)
    {
        var result = new ResultModel();

        try
        {
            var user = _dbContext.User.Where(_ => _.Id == userId && !_.IsDeleted).FirstOrDefault();
            if (user == null)
            {
                result.Succeed = false;
                result.ErrorMessage = "User not found";
                return result;
            }

            if (user.IsActive)
            {
                result.Succeed = false;
                result.ErrorMessage = "User has been activated";
                return result;
            }
            user.IsActive = true;
            user.DateUpdated = DateTime.Now;
            await _dbContext.SaveChangesAsync();
            result.Succeed = true;
            result.Data = user.Id;
        }
        catch (Exception e)
        {
            result.ErrorMessage = e.InnerException != null ? e.InnerException.Message : e.Message;
        }

        return result;
    }

    public async Task<ResultModel> AssignRole(AssignRoleModel model)
    {
        ResultModel result = new ResultModel();
        try
        {
            var user = await _userManager.FindByIdAsync(model.UserId.ToString());
            if (user == null)
            {
                result.ErrorMessage = "User not exists";
                result.Succeed = false;
                return result;
            }
            if (!user.IsActive)
            {
                result.Succeed = false;
                result.ErrorMessage = "User has been deactivated";
                return result;
            }
            var role = _dbContext.Role.Where(_ => _.Id == model.RoleId).FirstOrDefault();
            if (role == null)
            {
                result.ErrorMessage = "Role not exists";
                result.Succeed = false;
                return result;
            }
            await _userManager.AddToRoleAsync(user, role.NormalizedName);
            result.Succeed = true;
            result.Data = _mapper.Map<Role, RoleModel>(role);
        }
        catch (Exception e)
        {
            result.ErrorMessage = e.InnerException != null ? e.InnerException.Message : e.Message;
        }
        return result;
    }

    public async Task<ResultModel> UnassignRole(AssignRoleModel model)
    {
        ResultModel result = new ResultModel();
        try
        {
            var user = await _userManager.FindByIdAsync(model.UserId.ToString());
            if (user == null)
            {
                result.ErrorMessage = "User not exists";
                result.Succeed = false;
                return result;
            }
            if (user.UserName == "admin")
            {
                result.ErrorMessage = "You cannot unassign this user";
                result.Succeed = false;
                return result;
            }
            if (!user.IsActive)
            {
                result.Succeed = false;
                result.ErrorMessage = "User has been deactivated";
                return result;
            }
            var role = _dbContext.Role.Where(_ => _.Id == model.RoleId).FirstOrDefault();
            if (role == null)
            {
                result.ErrorMessage = "Role not exists";
                result.Succeed = false;
                return result;
            }
            await _userManager.RemoveFromRoleAsync(user, role.NormalizedName);
            result.Succeed = true;
            result.Data = _mapper.Map<Role, RoleModel>(role);
        }
        catch (Exception e)
        {
            result.ErrorMessage = e.InnerException != null ? e.InnerException.Message : e.Message;
        }
        return result;
    }

    public async Task<ResultModel> GetRoleOfUser(Guid userId)
    {
        ResultModel result = new ResultModel();
        try
        {
            var user = _dbContext.Users.Include(_ => _.UserRoles).ThenInclude(_ => _.Role).Where(_ => _.Id == userId && !_.IsDeleted).FirstOrDefault();
            if (user == null)
            {
                result.ErrorMessage = "User not exists";
                result.Succeed = false;
                return result;
            }
            if (!user.IsActive)
            {
                result.Succeed = false;
                result.ErrorMessage = "User has been deactivated";
                return result;
            }
            result.Succeed = true;
            result.Data = user.UserRoles.Select(_ => new RoleModel { Id = _.Role.Id, Name = _.Role.Name, Description = _.Role.Description, IsDeactive = _.Role.IsDeactive});
        }
        catch (Exception e)
        {
            result.ErrorMessage = e.InnerException != null ? e.InnerException.Message : e.Message;
        }
        return result;
    }

    public async Task<ResultModel> GetUsersInRole(String name)
    {
        ResultModel result = new ResultModel();
        try
        {
            var users = await _userManager.GetUsersInRoleAsync(name);
            result.Succeed = true;
            result.Data = users.Select(_ =>
            new UserModel
            {
                Id = _.Id,
                Address = _.Address,
                Email = _.Email!,
                FirstName = _.FirstName,
                LastName = _.LastName,
                PhoneNumber = _.PhoneNumber,
                UserName = _.UserName!, 
            });
        }
        catch (Exception e)
        {
            result.ErrorMessage = e.InnerException != null ? e.InnerException.Message : e.Message;
        }
        return result;
    }
    public async Task<ResultModel> GetUserRole(Guid userId)
    {
        ResultModel result = new ResultModel();
        try
        {
            var user = _dbContext.Users.Include(_ => _.UserRoles).ThenInclude(_ => _.Role).Where(_ => _.Id == userId && !_.IsDeleted).FirstOrDefault();
            if (user == null)
            {
                result.ErrorMessage = "User not exists";
                result.Succeed = false;
                return result;
            }
            if (!user.IsActive)
            {
                result.Succeed = false;
                result.ErrorMessage = "User has been deactivated";
                return result;
            }
            var role = _dbContext.UserRoles.Where(s => s.UserId == userId).FirstOrDefault();
            if (role != null)
            {
                var roleID = role.RoleId;
                var data = _dbContext.Roles.Where(s => s.Id == roleID).FirstOrDefault();

                if (data != null)
                {   
                    
                    result.Data = data;
                    result.Succeed = true;
                }
                else
                {
                    result.ErrorMessage = "Role" + ErrorMessage.ID_NOT_EXISTED;
                    result.Succeed = false;
                }
            }
            else
            {
                result.ErrorMessage = "UserRole" + ErrorMessage.ID_NOT_EXISTED;
                result.Succeed = false;
            }
        }
        catch (Exception ex)
        {
            result.ErrorMessage = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
        }
        return result;

    }

    private async Task<Token> GetAccessToken(User user, List<string> role)
    {
        List<Claim> claims = GetClaims(user, role);
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
        var token = new JwtSecurityToken(_configuration["Jwt:Issuer"],
          _configuration["Jwt:Issuer"],
          claims,
          expires: DateTime.Now.AddHours(int.Parse(_configuration["Jwt:ExpireTimes"])),
          //int.Parse(_configuration["Jwt:ExpireTimes"]) * 3600
          signingCredentials: creds);

        var serializedToken = new JwtSecurityTokenHandler().WriteToken(token);

        return new Token
        {
            Access_token = serializedToken,
            TokenType = "Bearer",
            ExpiresIn = int.Parse(_configuration["Jwt:ExpireTimes"]) * 3600,
            UserID = user.Id.ToString(),
            UserName = user.UserName,
            PhoneNumber = user.PhoneNumber,
            UserAva = user.UserAva,
            FirstName = user.FirstName,
            LastName = user.LastName,
            CurrenNoticeCount = user.CurrenNoticeCount,
            Roles = role
        };
    }

    private List<Claim> GetClaims(User user, List<string> roles)
    {
        IdentityOptions _options = new IdentityOptions();
        var claims = new List<Claim> {
                new Claim("UserId", user.Id.ToString()),
                new Claim("Email", user.Email),
                new Claim("FullName", user.FirstName + user.LastName),

                new Claim("UserName", user.UserName)
            };
        foreach (var role in roles)
        {
            claims.Add(new Claim(ClaimTypes.Role, role));
        }
        if (!string.IsNullOrEmpty(user.PhoneNumber)) claims.Add(new Claim("PhoneNumber", user.PhoneNumber));
        return claims;
    }
}
