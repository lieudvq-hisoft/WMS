using AutoMapper;
using Data.DataAccess;
using Data.DataAccess.Constant;
using Data.Entities;
using Data.Model;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Services.Core;

public interface IUserService
{
    Task<ResultModel> Register(UserCreateModel model);
    Task<ResultModel> Login(LoginModel model);
}
public class UserService : IUserService
{
    private readonly AppDbContext _dbContext;
    private readonly IMapper _mapper;
    private readonly IConfiguration _configuration;
    private readonly UserManager<User> _userManager;
    private readonly SignInManager<User> _signInManager;
    private readonly RoleManager<Role> _roleManager;

    public UserService(AppDbContext dbContext, IMapper mapper, IConfiguration configuration, UserManager<User> userManager, SignInManager<User> signInManager, RoleManager<Role> roleManager)
    {
        _dbContext = dbContext;
        _mapper = mapper;
        _configuration = configuration;
        _userManager = userManager;
        _signInManager = signInManager;
        _roleManager = roleManager;
    }

    public async Task<ResultModel> Login(LoginModel model)
    {

        var result = new ResultModel();
        try
        {
        }
        catch (Exception ex)
        {
            result.ErrorMessage = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
        }
        return result;
    }

    public async Task<ResultModel> Register(UserCreateModel model)
    {
        var result = new ResultModel();
        result.Succeed = false;
        try
        {
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
            AccessToken = serializedToken,
            TokenType = "Bearer",
            ExpiresIn = int.Parse(_configuration["Jwt:ExpireTimes"]) * 3600,
            UserID = user.Id.ToString(),
            UserName = user.UserName,
            PhoneNumber = user.PhoneNumber,
            UserAva = user.UserAva,
            CurrenNoticeCount = user.CurrenNoticeCount,
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
