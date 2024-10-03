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
    public class CommentDAL : ICommentDAL
    {
        private readonly SocialNetworkDbContext _context;
        public CommentDAL(SocialNetworkDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(Comment comment) 
        {
            await _context.Comments.AddAsync(comment);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Comment comment)
        {
            _context.Comments.Remove(comment);
            await _context.SaveChangesAsync();
        }

        public async Task<List<Comment>> GetAllAsync()
        {
            var comments = await _context.Comments.Include(nameof(Comment.Sender)).Include(nameof(Comment.Post)).ToListAsync();
            return comments;
        }

        public async Task<Comment> GetByIdAsync(int id)
        {
            var comment = await _context.Comments.Include(nameof(Comment.Sender)).Include(nameof(Comment.Post)).FirstOrDefaultAsync(x => x.Id == id);
            return comment;
        }

        public async Task UpdateAsync(Comment comment)
        {
            _context.Comments.Update(comment);
            await _context.SaveChangesAsync();
        }
    }
}
