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
    public class NotificationDAL : INotificationDAL
    {
        private readonly SocialNetworkDbContext _context;
        public NotificationDAL(SocialNetworkDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(Notification notification)
        {
            await _context.Notifications.AddAsync(notification);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Notification notification)
        {
            _context.Notifications.Remove(notification);
            await _context.SaveChangesAsync();
        }

        public async Task<List<Notification>> GetAllAsync()
        {
            var chats = await _context.Notifications.ToListAsync();
            return chats;
        }

        public async Task<Notification> GetByIdAsync(int id)
        {
            var chat = await _context.Notifications.FirstOrDefaultAsync(x => x.Id == id);
            return chat;
        }

        public async Task UpdateAsync(Notification notification)
        {
            _context.Notifications.Update(notification);
            await _context.SaveChangesAsync();
        }
    }
}
