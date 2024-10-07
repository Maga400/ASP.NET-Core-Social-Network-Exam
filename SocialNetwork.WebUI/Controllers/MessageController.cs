using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SocialNetwork.Business.Services.Abstracts;
using SocialNetwork.DataAccess.Data;
using SocialNetwork.Entities.Entities;
using SocialNetwork.WebUI.Models;
using System;
using System.Net.Security;

namespace SocialNetwork.WebUI.Controllers
{
    [Authorize]
    public class MessageController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly UserManager<CustomIdentityUser> _userManager;
        private readonly ICustomIdentityUserService _customIdentityUserService;
        private readonly IFriendService _friendService;
        private readonly IFriendRequestService _friendRequestService;
        private readonly IChatService _chatService;
        private readonly IMessageService _messageService;
        private readonly SocialNetworkDbContext _context;

        public MessageController(ILogger<HomeController> logger, UserManager<CustomIdentityUser> userManager, ICustomIdentityUserService customIdentityUserService, IFriendService friendService, IFriendRequestService friendRequestService, IChatService chatService, IMessageService messageService, SocialNetworkDbContext context)
        {
            _logger = logger;
            _userManager = userManager;
            _customIdentityUserService = customIdentityUserService;
            _friendService = friendService;
            _friendRequestService = friendRequestService;
            _chatService = chatService;
            _messageService = messageService;
            _context = context;
        }
        // GET: MessageController
        public ActionResult Index()
        {
            return View();
        }

        //public async Task<ActionResult> Hello()
        //{
        //var user = await _userManager.GetUserAsync(HttpContext.User);
        //var chat = await _context.Chats.Include(nameof(Chat.Messages)).FirstOrDefaultAsync(c => c.SenderId == user.Id && c.ReceiverId == id || c.ReceiverId == user.Id && c.SenderId == id);
        //if (chat == null)
        //{
        //    chat = new Chat
        //    {
        //        ReceiverId = id,
        //        SenderId = user.Id,
        //        Messages = new List<Message>()
        //    };

        //    await _context.Chats.AddAsync(chat);
        //    await _context.SaveChangesAsync();
        //}

        //var chats = _context.Chats.Include(nameof(Chat.Receiver)).Where(c => c.SenderId == user.Id || c.ReceiverId == user.Id);


        //var chatBlocks = from c in chats
        //                 let receiver = (user.Id != c.ReceiverId) ? c.Receiver : _context.Users.FirstOrDefault(u => u.Id == c.SenderId)
        //                 select new Chat
        //                 {
        //                     Messages = c.Messages,
        //                     Id = c.Id,
        //                     SenderId = c.SenderId,
        //                     Receiver = receiver,
        //                     ReceiverId = receiver.Id,
        //                 };

        //var result = chatBlocks.ToList().Where(c => c.ReceiverId != user.Id);
        //var model = new ChatViewModel
        //{
        //    CurrentUserId = user.Id,
        //    CurrentReceiver = id,
        //    CurrentChat = chat,
        //    Chats = result.Count() == 0 ? chatBlocks : result,
        //};

        //return View(model);
        //}

        public async Task<IActionResult> GoChat(string id = "")
        {
            
            if (id == "")
            {
                var user = await _userManager.GetUserAsync(HttpContext.User);
                //var chat = await _context.Chats.Include(nameof(Chat.Messages)).FirstOrDefaultAsync(c => c.SenderId == user.Id && c.ReceiverId == id || c.ReceiverId == user.Id && c.SenderId == id);
                var chats = _context.Chats.Include(nameof(Chat.Receiver)).Where(c => c.SenderId == user.Id || c.ReceiverId == user.Id);


                var chatBlocks = from c in chats
                                 let receiver = (user.Id != c.ReceiverId) ? c.Receiver : _context.Users.FirstOrDefault(u => u.Id == c.SenderId)
                                 select new Chat
                                 {
                                     Messages = c.Messages,
                                     Id = c.Id,
                                     SenderId = c.SenderId,
                                     Receiver = receiver,
                                     ReceiverId = receiver.Id,

                                 };
                var result = chatBlocks.ToList().Where(c => c.ReceiverId != user.Id);

                var model = new ChatViewModel
                {
                    CurrentUserId = "",
                    CurrentReceiver = "",
                    CurrentReceiverImage = "",
                    CurrentChat = null,
                    Chats = result.Count() == 0 ? chatBlocks : result,
                    CurrentUserName = "",
                };

                return View(model);
            }
            else
            {
                var user = await _userManager.GetUserAsync(HttpContext.User);
                var chat = await _context.Chats.Include(nameof(Chat.Messages)).FirstOrDefaultAsync(c => c.SenderId == user.Id && c.ReceiverId == id || c.ReceiverId == user.Id && c.SenderId == id);
                var chats = _context.Chats.Include(nameof(Chat.Receiver)).Where(c => c.SenderId == user.Id || c.ReceiverId == user.Id);


                var chatBlocks = from c in chats
                                 let receiver = (user.Id != c.ReceiverId) ? c.Receiver : _context.Users.FirstOrDefault(u => u.Id == c.SenderId)
                                 select new Chat
                                 {
                                     Messages = c.Messages,
                                     Id = c.Id,
                                     SenderId = c.SenderId,
                                     Receiver = receiver,
                                     ReceiverId = receiver.Id,

                                 };
                var result = chatBlocks.ToList().Where(c => c.ReceiverId != user.Id);

                if (chat == null)
                {
                    chat = new Chat
                    {
                        ReceiverId = id,
                        SenderId = user.Id,
                        Messages = new List<Message>()
                    };

                    await _context.Chats.AddAsync(chat);
                    await _context.SaveChangesAsync();
                }
                var model = new ChatViewModel
                {
                    CurrentUserId = user.Id,
                    CurrentReceiver = id,
                    CurrentReceiverImage = _context.Users.FirstOrDefault(u => u.Id == id).Image,
                    CurrentChat = chat,
                    Chats = result.Count() == 0 ? chatBlocks : result,
                    CurrentUserName = _context.Users.FirstOrDefault(u => u.Id == id).UserName,
                };

                return View(model);

            }
        }

        [HttpPost(Name = "AddMessage")]
        public async Task<IActionResult> AddMessage(MessageViewModel model)
        {
            try
            {
                var user = await _userManager.GetUserAsync(HttpContext.User);
                var chats = await _chatService.GetAllAsync();
                var chat = chats.FirstOrDefault(c => c.SenderId == model.SenderId && c.ReceiverId == model.ReceiverId || c.SenderId == model.ReceiverId && c.ReceiverId == model.SenderId);
                if (chat != null)
                {
                    var message = new Message
                    {
                        ChatId = chat.Id,
                        Content = model.Content,
                        DateTime = DateTime.Now,
                        IsImage = false,
                        HasSeen = false,
                        SenderId = user.Id,
                        ReceiverId = user.Id != model.ReceiverId?model.ReceiverId : model.SenderId,
                    };
                    await _messageService.AddAsync(message);
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
            return Ok();
        }

        public async Task<IActionResult> GetAllMessages(string receiverId, string senderId)
        {
            var chats = await _chatService.GetAllAsync();
            var chat = chats.FirstOrDefault(c => c.SenderId == senderId && c.ReceiverId == receiverId || c.SenderId == receiverId && c.ReceiverId == senderId);
            if (chat != null)
            {
                var user = await _userManager.GetUserAsync(HttpContext.User);
                return Ok(new { Messages = chat.Messages, CurrentUserId = user.Id });
            }

            return Ok();

        }

        // GET: MessageController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: MessageController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: MessageController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: MessageController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: MessageController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: MessageController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: MessageController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
