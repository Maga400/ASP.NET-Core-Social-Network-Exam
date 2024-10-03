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
    public class LikedCommentService : ILikedCommentService
    {
        private readonly ILikedCommentDAL _likedCommentDAL;

        public LikedCommentService(ILikedCommentDAL likedCommentDAL)
        {
            _likedCommentDAL = likedCommentDAL;
        }

        public async Task AddAsync(LikedComment vakue)
        {
            await _likedCommentDAL.AddAsync(vakue);
        }

        public async Task DeleteAsync(LikedComment value)
        {
            await _likedCommentDAL.DeleteAsync(value);
        }

        public async Task<List<LikedComment>> GetAllAsync()
        {
            return await _likedCommentDAL.GetAllAsync();
        }

        public async Task<LikedComment> GetByIdAsync(int id)
        {
            return await _likedCommentDAL.GetByIdAsync(id);
        }
        public async Task UpdateAsync(LikedComment value)
        {
            await _likedCommentDAL.UpdateAsync(value);
        }
    }
}
