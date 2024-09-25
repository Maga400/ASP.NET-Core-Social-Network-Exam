using SocialNetwork.Entities.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialNetwork.DataAccess.Repositories.Abstracts
{
    public interface IFriendRequestDAL
    {
        Task<List<FriendRequest>> GetAllAsync();
        Task<FriendRequest> GetByIdAsync(int id);
        Task AddAsync(FriendRequest friendRequest);
        Task UpdateAsync(FriendRequest friendRequest);
        Task DeleteAsync(FriendRequest friendRequest);
    }
}
