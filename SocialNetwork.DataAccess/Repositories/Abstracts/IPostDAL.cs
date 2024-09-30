using SocialNetwork.Entities.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialNetwork.DataAccess.Repositories.Abstracts
{
    public interface IPostDAL
    {
        Task<List<Post>> GetAllAsync();
        Task<Post> GetByIdAsync(int id);
        Task AddAsync(Post post);
        Task UpdateAsync(Post post);
        Task DeleteAsync(Post post);
    }
}
