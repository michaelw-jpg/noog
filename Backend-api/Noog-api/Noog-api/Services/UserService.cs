using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Noog_api.Models;
using Noog_api.Services.IServices;

namespace Noog_api.Services
{
    public class UserService<TUser> : IUserService<TUser> where TUser : class
    {
        private readonly UserManager<TUser> _userManager;
        private readonly SignInManager<TUser> _signInManager;

        public UserService(UserManager<TUser> userManager, SignInManager<TUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        public async Task<TUser?> FindByEmailAsync(string email)
        {
            return await _userManager.FindByEmailAsync(email);
        }
        public async Task<List<TUser>> AllUsersAsync()
        {
            return await _userManager.Users
                .AsNoTracking().ToListAsync();
        }

        public async Task<IdentityResult> CreateAsync(TUser user, string password)
        {
            IdentityResult results = await _userManager.CreateAsync(user, password);

            if (!results.Succeeded)
            {
                return (IdentityResult)Results.BadRequest(results.Errors);
            }

            return (IdentityResult)Results.Ok(user);
        }

        public async Task<SignInResult> CheckPasswordAsync(TUser user, string password, bool lockoutOnFailure)
        {
            return await _signInManager.CheckPasswordSignInAsync(user, password, lockoutOnFailure);
        }
            


        public async Task<IList<string>> GetRolesAsync(TUser user)
        {
            return await _userManager.GetRolesAsync(user);
        }

        public async Task<IdentityResult> AddToRoleAsync(TUser user, string role)
        {
            var result = await _userManager.AddToRoleAsync(user, role);

            if (!result.Succeeded)
            {
                return (IdentityResult)Results.BadRequest(result.Errors);
            }

            return (IdentityResult)Results.Ok();
        }
        public async Task<IdentityResult> DeleteAsync(TUser user)
        {
            var result = await _userManager.DeleteAsync(user);

            if (!result.Succeeded)
            {
                return (IdentityResult)Results.BadRequest(result.Errors);
            }

            return (IdentityResult)Results.Ok();
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
