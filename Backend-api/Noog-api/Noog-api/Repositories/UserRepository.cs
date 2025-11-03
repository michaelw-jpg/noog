using Microsoft.AspNetCore.Identity;
using Noog_api.Models;
using Noog_api.Repositories.IRepositories;

namespace Noog_api.Repositories
{
    public class UserRepository<TUser> : IUserRepository<TUser> where TUser : class
    {

    }
}
