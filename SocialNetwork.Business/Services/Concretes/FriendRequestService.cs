using SocialNetwork.Business.Services.Abstracts;
using SocialNetwork.DataAccess.Repositories.Abstracts;
using SocialNetwork.Entities.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialNetwork.Business.Services.Concretes
{
    public class FriendRequestService : IFriendRequestService
    {
        private readonly IFriendRequestDAL _friendRequestDAL;
        public FriendRequestService(IFriendRequestDAL friendRequestDAL)
        {
            _friendRequestDAL = friendRequestDAL;
        }

        public async Task AddAsync(FriendRequest friendRequest)
        {
            await _friendRequestDAL.AddAsync(friendRequest);
        }

        public async Task DeleteAsync(FriendRequest friendRequest)
        {
            await _friendRequestDAL.DeleteAsync(friendRequest);
        }

        public async Task<List<FriendRequest>> GetAllAsync()
        {
            return await _friendRequestDAL.GetAllAsync();
        }

        public async Task<FriendRequest> GetByIdAsync(int id)
        {
            return await _friendRequestDAL.GetByIdAsync(id);
        }

        public async Task UpdateAsync(FriendRequest friendRequest)
        {
            await _friendRequestDAL.UpdateAsync(friendRequest);
        }
    }
}
