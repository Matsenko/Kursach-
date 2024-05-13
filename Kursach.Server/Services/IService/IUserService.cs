using Kursach.Models;
using Kursach.Shared.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace Kursach.Services.IService
{
    public interface IUserService
    {
        Task<UserModel> RegisterUser(UserDTO userModel);
        Task<UserModel> DeleteUser(string userId);
    }
}
