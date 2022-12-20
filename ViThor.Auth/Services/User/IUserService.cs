using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using ViThor.Auth.Models;
using ViThor.Auth.Requests;

namespace ViThor.Auth.Services.User
{
    public interface IUserService<TUserBase> where TUserBase : UserBase, new()
    {
        Task Create(TUserBase user);
        Task Update(TUserBase user);
        Task<IEnumerable<TUserBase>> Get();
        Task<TUserBase?> GetById(Guid id);
        Task<TUserBase?> GetByUsername(string username);
        Task<TUserBase?> GetByEmail(string email);
        Task<Claim[]> GetClaim(TUserBase user);
    }
}