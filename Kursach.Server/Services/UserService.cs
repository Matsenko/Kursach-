using Kursach.Data;
using Kursach.Models;
using Kursach.Services.IService;
using Kursach.Shared.DTOs;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Kursach.Services
{
    public class UserService:IUserService
    {
        private readonly KursachContext _context;
        public UserService(KursachContext context)
        {
            _context = context;
        }
        public async Task<UserModel> RegisterUser(UserDTO userModel)
        {
            var user = new UserModel
            {
                UserId = userModel.UserId
            };
            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();
            return user;
        }
        public async Task<UserModel> DeleteUser(string userId)
        {
     
            var user = await _context.Users.FirstOrDefaultAsync(u => u.UserId == userId);
            if (user == null)
            {
                return null;
            }

            user.isDeleted = true;
            _context.Users.Update(user);
            await _context.SaveChangesAsync();

            return user;
        }
        public async Task<UserModel> GetUser(string userId)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.UserId == userId);
        }
        public async Task<UserModel> RequestCount(string userId)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.UserId == userId);
            if (user == null)
            {
                return null;
            }
            user.RequestsCount++;
            await _context.SaveChangesAsync();
            return user;
        }
    }
}
