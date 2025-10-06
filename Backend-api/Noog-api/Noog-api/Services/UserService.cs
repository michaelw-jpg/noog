using Microsoft.AspNetCore.Identity;
using Noog_api.Models;
using Noog_api.Services.IServices;

namespace Noog_api.Services
{
    public class UserService<TUser> : IUserService<TUser> where TUser : class
    {
        private readonly UserManager<TUser> _userManager;
        private readonly SignInManager<User> _signInManager;

        public UserService(UserManager<TUser> userManager, SignInManager<User> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        public async Task<TUser?> FindByEmailAsync(string email)
        {
            return await _userManager.FindByEmailAsync(email);
        }

        public async Task<IdentityResult> CreateAsync(TUser user, string password)
        {
            return await _userManager.CreateAsync(user, password);
        }

        public async Task<bool> CheckPasswordAsync(User user, string password, bool lockoutOnFailure)
        {
            var result = await _signInManager.CheckPasswordSignInAsync(user, password, lockoutOnFailure);
            return result.Succeeded;
        }

        public async Task<IList<string>> GetRolesAsync(TUser user)
        {
            return await _userManager.GetRolesAsync(user);
        }

        public async Task<IdentityResult> AddToRoleAsync(TUser user, string role)
        {
            return await _userManager.AddToRoleAsync(user, role);
        }
        public async Task<IdentityResult> DeleteAsync(TUser user)
        {
            return await _userManager.DeleteAsync(user);

        }
        public async Task<IdentityResult> DeleteByIdAsync(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user is null)
            {
                return IdentityResult.Failed(new IdentityError
                { Code = "UserNotFound", Description = "User not found" });
            }

            return await _userManager.DeleteAsync(user);
        }

    }
}
