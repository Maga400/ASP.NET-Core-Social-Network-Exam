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
    public class FriendService : IFriendService
    {
        private readonly IFriendDAL _friendDAL;

        public FriendService(IFriendDAL friendDAL)
        {
            _friendDAL = friendDAL;
        }

        public async Task AddAsync(Friend friend)
        {
            await _friendDAL.AddAsync(friend);
        }

        public async Task DeleteAsync(Friend friend)
        {
            await _friendDAL.DeleteAsync(friend);

        }

        public async Task<List<Friend>> GetAllAsync()
        {
            return await _friendDAL.GetAllAsync();
        }

        public async Task<Friend> GetByIdAsync(int id)
        {
            return await _friendDAL.GetByIdAsync(id);
        }

        public async Task UpdateAsync(Friend friend)
        {
            await _friendDAL.UpdateAsync(friend);
        }
    }
}
