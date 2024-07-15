using Lombeo.Api.Authorize.Infra;
using Lombeo.Api.Authorize.Infra.Entities;
using Microsoft.EntityFrameworkCore;
using System;

namespace Lombeo.Api.Authorize.Services.MessageService
{
    public interface IMessageService
    {
        Task<Messenger> SendMessage(int senderId, int receiverId, string content);
        Task<IEnumerable<Messenger>> GetMessages(int userId, int contactId);
    }

    public class MessageService : IMessageService
    {
        private readonly LombeoAuthorizeContext _context;

        public MessageService(LombeoAuthorizeContext context)
        {
            _context = context;
        }

        public async Task<Messenger> SendMessage(int senderId, int receiverId, string content)
        {
            var message = new Messenger
            {
                SenderId = senderId,
                ReceiverId = receiverId,
                Content = content,
                Timestamp = DateTime.UtcNow
            };

            _context.Messages.Add(message);
            await _context.SaveChangesAsync();

            return message;
        }

        public async Task<IEnumerable<Messenger>> GetMessages(int userId, int contactId)
        {
            return await _context.Messages
                .Where(m => (m.SenderId == userId && m.ReceiverId == contactId) || (m.SenderId == contactId && m.ReceiverId == userId))
                .OrderBy(m => m.Timestamp)
                .ToListAsync();
        }
    }

}
