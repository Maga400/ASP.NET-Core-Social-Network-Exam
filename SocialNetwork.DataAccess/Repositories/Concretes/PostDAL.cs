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
    public class PostDAL : IPostDAL
    {
        private readonly SocialNetworkDbContext _context;
        public PostDAL(SocialNetworkDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(Post post)
        {
            await _context.Posts.AddAsync(post);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Post post)
        {
            _context.Posts.Remove(post);
            await _context.SaveChangesAsync();
        }

        public async Task<List<Post>> GetAllAsync()
        {
            var posts = await _context.Posts.Include(nameof(Post.Comments)).Include(nameof(Post.Sender)).ToListAsync();
            return posts;
        }

        public async Task<Post> GetByIdAsync(int id)
        {
            var post = await _context.Posts.Include(nameof(Post.Comments)).Include(nameof(Post.Sender)).FirstOrDefaultAsync(x => x.Id == id);
            return post;
        }

        public async Task UpdateAsync(Post post)
        {
            _context.Posts.Update(post);
            await _context.SaveChangesAsync();
        }
    }
}
