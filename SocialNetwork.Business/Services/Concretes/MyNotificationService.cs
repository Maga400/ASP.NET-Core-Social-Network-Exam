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
    public class MyNotificationService : IMyNotificationService
    {
        private readonly IMyNotificationDAL _myNotificationDAL;

        public MyNotificationService(IMyNotificationDAL myNotificationDAL)
        {
            _myNotificationDAL = myNotificationDAL;
        }

        public async Task AddAsync(MyNotification notification)
        {
            await _myNotificationDAL.AddAsync(notification);
        }

        public async Task DeleteAsync(MyNotification notification)
        {
            await _myNotificationDAL.DeleteAsync(notification);
        }

        public async Task<List<MyNotification>> GetAllAsync()
        {
            return await _myNotificationDAL.GetAllAsync();
        }

        public async Task<MyNotification> GetByIdAsync(int id)
        {
            return await _myNotificationDAL.GetByIdAsync(id);
        }

        public async Task UpdateAsync(MyNotification notification)
        {
            await _myNotificationDAL.UpdateAsync(notification);
        }
    }
}
