using SocialNetwork.Business.Services.Abstracts;
using SocialNetwork.DataAccess.Repositories.Abstracts;
using SocialNetwork.DataAccess.Repositories.Concretes;
using SocialNetwork.Entities.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialNetwork.Business.Services.Concretes
{
    public class LikedPostService : ILikedPostService
    {
        private readonly ILikedPostDAL _likedPostDAL;

        public LikedPostService(ILikedPostDAL likedPostDAL)
        {
            _likedPostDAL = likedPostDAL;
        }

        public async Task AddAsync(LikedPost value)
        {
            await _likedPostDAL.AddAsync(value);
        }

        public async Task DeleteAsync(LikedPost value)
        {
            await _likedPostDAL.DeleteAsync(value);
        }

        public async Task<List<LikedPost>> GetAllAsync()
        {
            return await _likedPostDAL.GetAllAsync();
        }

        public async Task<LikedPost> GetByIdAsync(int id)
        {
            return await _likedPostDAL.GetByIdAsync(id);
        }

        public async Task UpdateAsync(LikedPost value)
        {
            await _likedPostDAL.UpdateAsync(value);
        }
    }
}
