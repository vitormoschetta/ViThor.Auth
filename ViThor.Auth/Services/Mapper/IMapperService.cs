using ViThor.Auth.Models;
using ViThor.Auth.Requests;

namespace ViThor.Auth.Services.Mapper
{
    public interface IMapperService<TUserBase, TUserRequestBase> 
                                        where TUserBase : UserBase, new() 
                                        where TUserRequestBase : CreateUserRequestBase, new()
    {
        TUserBase MapRequestToUser(TUserRequestBase request, string refrashToken, byte[] salt, string password);
    }
      
}