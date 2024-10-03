using SocialNetwork.Entities.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialNetwork.DataAccess.Repositories.Abstracts
{
    public interface ILikedCommentDAL
    {
        Task<List<LikedComment>> GetAllAsync();
        Task<LikedComment> GetByIdAsync(int id);
        Task AddAsync(LikedComment vakue);
        Task UpdateAsync(LikedComment value);
        Task DeleteAsync(LikedComment value);
    }
}
