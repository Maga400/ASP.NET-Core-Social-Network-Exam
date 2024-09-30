using SocialNetwork.Entities.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialNetwork.DataAccess.Repositories.Abstracts
{
    public interface ICommentDAL
    {
        Task<List<Comment>> GetAllAsync();
        Task<Comment> GetByIdAsync(int id);
        Task AddAsync(Comment comment);
        Task UpdateAsync(Comment comment);
        Task DeleteAsync(Comment comment);
    }
}
