using Microsoft.EntityFrameworkCore;
using SocialNetwork.DataAccess.Data;
using SocialNetwork.DataAccess.Repositories.Abstracts;
using SocialNetwork.Entities.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialNetwork.DataAccess.Repositories.Concretes
{
    public class CustomIdentityUserDAL : ICustomIdentityUserDAL
    {
        private readonly SocialNetworkDbContext _context;
        public CustomIdentityUserDAL(SocialNetworkDbContext context)
        {
            _context = context;
        }
        public async Task AddAsync(CustomIdentityUser user)
        {
            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(CustomIdentityUser user)
        {
            _context.Users.Remove(user);
            await _context.SaveChangesAsync();
        }

        public async Task<List<CustomIdentityUser>> GetAllAsync()
        {
            return await _context.Users.ToListAsync();
        }

        public async Task<CustomIdentityUser> GetByIdAsync(string id)
        {
            return await _context.Users.FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task UpdateAsync(CustomIdentityUser user)
        {
            _context.Users.Update(user);
            await _context.SaveChangesAsync();
        }
    }
}
