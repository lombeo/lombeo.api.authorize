using Lombeo.Api.Authorize.DTO.MessageDTO;
using Lombeo.Api.Authorize.Hubs;
using Lombeo.Api.Authorize.Infra.Constants;
using Lombeo.Api.Authorize.Services.MessageService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using System.Security.Claims;

namespace Lombeo.Api.Authorize.Controllers
{
    [ApiController]
    [Route(RouteApiConstant.BASE_PATH + "/messenger")]
    public class MessagesController : BaseAPIController
    {
        private readonly IMessageService _messageService;
        private readonly IHubContext<ChatHub> _chatHubContext;

        public MessagesController(IMessageService messageService, IHubContext<ChatHub> chatHubContext)
        {
            _messageService = messageService;
            _chatHubContext = chatHubContext;
        }

        [HttpPost("Send-message")]
        [Authorize]
        public async Task<IActionResult> SendMessage([FromBody] SendMessageDTO sendMessageDTO)
        {
            var senderId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if(senderId == null)
            {
                throw new ApplicationException(Message.CommonMessage.NOT_AUTHEN);
            }
            var message = await _messageService.SendMessage(int.Parse(senderId), sendMessageDTO.ReceiverId, sendMessageDTO.Content);

            // Gửi tin nhắn qua SignalR
            await _chatHubContext.Clients.User(sendMessageDTO.ReceiverId.ToString()).SendAsync("ReceiveMessage", senderId, sendMessageDTO.Content);

            return Ok(message);
        }

        [HttpGet("Get-Messages")]
        [Authorize]
        public async Task<IActionResult> GetMessages(int contactId)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if(userId == null)
            {
                throw new ApplicationException(Message.CommonMessage.NOT_AUTHEN);
            }
            var messages = await _messageService.GetMessages(int.Parse(userId), contactId);
            return Ok(messages);
        }
    }
}
