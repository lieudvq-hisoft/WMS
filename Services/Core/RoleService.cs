using System;
using AutoMapper;
using Data.DataAccess;
using Data.Entities;
using Data.Model;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Services.Core
{
    public interface IRoleService
    {
        Task<ResultModel> GetForAssign();
    }
    public class RoleService : IRoleService
    {
        private readonly AppDbContext _dbContext;
        private readonly RoleManager<Role> _roleManager;
        private readonly IMapper _mapper;
        public RoleService(RoleManager<Role> roleManager, AppDbContext dbContext, IMapper mapper)
        {
            _roleManager = roleManager;
            _dbContext = dbContext;
            _mapper = mapper;
        }
        public async Task<ResultModel> GetForAssign()
        {
            ResultModel result = new ResultModel();
            try
            {
                var data = _dbContext.Role.Where(_ => !_.IsDeactive);
                var view = _mapper.ProjectTo<RoleModel>(data);
                result.Data = view;
                result.Succeed = true;
            }
            catch (Exception e)
            {
                result.ErrorMessage = e.InnerException != null ? e.InnerException.Message : e.Message;
            }
            return result;
        }
    }
}

