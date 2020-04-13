using Core;
using Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure
{
    public class UserRepository : IUserRepository
    {
        private readonly CompanyContext _context;
        public UserRepository(CompanyContext context)
        {
            _context = context;
        }
        public Task<UserDto> GetUser(string username, string password)
        {
           return _context.Users.FirstOrDefaultAsync(u => u.UserName == username && u.Password == password);
        }
    }
}
