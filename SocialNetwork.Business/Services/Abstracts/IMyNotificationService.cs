using SocialNetwork.Entities.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialNetwork.Business.Services.Abstracts
{
    public interface IMyNotificationService
    {
        Task<List<MyNotification>> GetAllAsync();
        Task<MyNotification> GetByIdAsync(int id);
        Task AddAsync(MyNotification notification);
        Task UpdateAsync(MyNotification notification);
        Task DeleteAsync(MyNotification notification);
    }
}
