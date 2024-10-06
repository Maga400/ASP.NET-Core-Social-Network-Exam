

using Microsoft.AspNetCore.Identity;

namespace SocialNetwork.Entities.Entities
{
    public class CustomIdentityUser : IdentityUser
    {
        public string? Image { get; set; }
        public bool IsOnline { get; set; }
        public bool IsFriend { get; set; }
        public bool HasRequestPending { get; set; }
        public DateTime DisConnectTime { get; set; } = DateTime.Now;
        public string? ConnectTime { get; set; } = "";
        public virtual ICollection<Friend>? Friends { get; set; }
        public virtual ICollection<FriendRequest>? FriendRequests { get; set; }
        public virtual ICollection<Notification>? Notifications { get; set; }
        public virtual ICollection<Chat>? Chats { get; set; }
        public virtual ICollection<Post>? Posts { get; set; }
        public virtual ICollection<MyNotification>? MyNotifications { get; set; }
        public CustomIdentityUser()
        {
            Friends = new List<Friend>();
            FriendRequests = new List<FriendRequest>();
            Chats = new List<Chat>();
            Notifications = new List<Notification>();
            Posts = new List<Post>();
            MyNotifications = new List<MyNotification>();
        }

    }
}
