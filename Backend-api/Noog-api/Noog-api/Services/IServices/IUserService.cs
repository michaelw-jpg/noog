using Microsoft.AspNetCore.Identity;
using Noog_api.DTOs.UserDtos;
using Noog_api.Models;

namespace Noog_api.Services.IServices
{
    public interface IUserService<TUser> where TUser : class
    {
        Task<TUser?> FindByEmailAsync(string email);
        Task<TUser?> FindByIdAsync(Guid userId);
        Task<List<TUser>> AllUsersAsync();
        Task<IdentityResult> CreateAsync(TUser user, string password);
        Task<SignInResult> CheckPasswordAsync(TUser user, string password, bool lockoutOnFailure);
        Task<IList<string>> GetRolesAsync(TUser user);
        Task<IdentityResult> AddToRoleAsync(TUser user, string role);
        Task<IdentityResult> DeleteAsync(TUser user);
        Task<IdentityResult> DeleteByIdAsync(string userId);
        Task<IdentityResult> UpdateByIdAsync(string userId, TUser updatedUser);

    }
}
