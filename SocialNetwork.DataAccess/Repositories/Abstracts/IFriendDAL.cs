using SocialNetwork.Entities.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialNetwork.DataAccess.Repositories.Abstracts
{
    public interface IFriendDAL
    {
        Task<List<Friend>> GetAllAsync();
        Task<Friend> GetByIdAsync(int id);
        Task AddAsync(Friend friend);
        Task UpdateAsync(Friend friend);
        Task DeleteAsync(Friend friend);
    }
}
