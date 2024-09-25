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
    public class FriendRequestDAL : IFriendRequestDAL
    {
        private readonly SocialNetworkDbContext _context;
        public FriendRequestDAL(SocialNetworkDbContext context)
        {
            _context = context;
        }
        public async Task AddAsync(FriendRequest friendRequest)
        {
            await _context.FriendRequests.AddAsync(friendRequest);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(FriendRequest friendRequest)
        {
            _context.FriendRequests.Remove(friendRequest);
            await _context.SaveChangesAsync();
        }

        public async Task<List<FriendRequest>> GetAllAsync()
        {
            var friendRequests = await _context.FriendRequests.ToListAsync();
            return friendRequests;
        }

        public async Task<FriendRequest> GetByIdAsync(int id)
        {
            var friendRequest = await _context.FriendRequests.FirstOrDefaultAsync(x => x.Id == id);
            return friendRequest;
        }

        public async Task UpdateAsync(FriendRequest friendRequest)
        {
            _context.FriendRequests.Update(friendRequest);
            await _context.SaveChangesAsync();
        }
    }
}
