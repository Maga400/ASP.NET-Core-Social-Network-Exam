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
    public class LikedCommentDAL : ILikedCommentDAL
    {
        private readonly SocialNetworkDbContext _context;
        public LikedCommentDAL(SocialNetworkDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(LikedComment value)
        {
            await _context.LikedComments.AddAsync(value);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(LikedComment value)
        {
            _context.LikedComments.Remove(value);
            await _context.SaveChangesAsync();
        }

        public async Task<List<LikedComment>> GetAllAsync()
        {
            var likedComments = await _context.LikedComments.Include(nameof(LikedComment.User)).Include(nameof(LikedComment.Comment)).ToListAsync();
            return likedComments;
        }

        public async Task<LikedComment> GetByIdAsync(int id)
        {
            var likedComment = await _context.LikedComments.Include(nameof(LikedComment.User)).Include(nameof(LikedComment.Comment)).FirstOrDefaultAsync(x => x.Id == id);
            return likedComment;
        }

        public async Task UpdateAsync(LikedComment value)
        {
            _context.LikedComments.Update(value);
            await _context.SaveChangesAsync();
        }
    }
}
