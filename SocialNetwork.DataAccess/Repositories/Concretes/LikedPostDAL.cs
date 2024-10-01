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
    public class LikedPostDAL : ILikedPostDAL
    {
        private readonly SocialNetworkDbContext _context;
        public LikedPostDAL(SocialNetworkDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(LikedPost value)
        {
            await _context.LikedPosts.AddAsync(value);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(LikedPost value)
        {
            _context.LikedPosts.Remove(value);
            await _context.SaveChangesAsync();
        }

        public async Task<List<LikedPost>> GetAllAsync()
        {
            var likedPosts = await _context.LikedPosts.Include(nameof(LikedPost.User)).Include(nameof(LikedPost.Post)).ToListAsync();
            return likedPosts;
        }

        public async Task<LikedPost> GetByIdAsync(int id)
        {
            var likedPosts = await _context.LikedPosts.Include(nameof(LikedPost.User)).Include(nameof(LikedPost.Post)).FirstOrDefaultAsync(x => x.Id == id);
            return likedPosts;
        }

        public async Task UpdateAsync(LikedPost value)
        {
            _context.LikedPosts.Update(value);
            await _context.SaveChangesAsync();
        }
    }
}
