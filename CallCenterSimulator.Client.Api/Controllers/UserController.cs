using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CallCenterSimulator.Domain.GrpcServer.Core;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using NotificationGrpcService;

namespace CallCenterSimulator.Client.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserController : ControllerBase
    {
        private readonly ServerGrpcSubscribers _grpcSubscribers;

        private readonly ILogger<UserController> _logger;

        public UserController(ILogger<UserController> logger,ServerGrpcSubscribers grpcSubscribers)
        {
            _logger = logger;
            this._grpcSubscribers = grpcSubscribers;
        }

        [HttpGet]
        [Route("GetUserList")]
        public JsonResult Index()
        {
            var subscriberList = this._grpcSubscribers.GetSubscriberList();
            return new JsonResult(subscriberList);
        }
        
        [HttpPost]
        [Route("BrodcastMessage")]
        public async Task<JsonResult> BrodcastMessage(string message)
        {
            await _grpcSubscribers.BroadcastMessageAsync(new RegisterResponse()
            {
                Payload = message,
                Status = MessageStatus.Created,
                MessageId = Guid.NewGuid().ToString(),
                Time = DateTime.Now.Ticks
            });
            
            return new JsonResult(Ok());
        }
    }
}