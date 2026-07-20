using BusinessLogicLayer;
using CommonDataLayer.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ApartmentsManagement.Ntier.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NotificationsController : BasesController<Notification>
    {
        private readonly IBaseBL<Notification> _notificationBL;
        public NotificationsController(IBaseBL<Notification> notificationBL) : base(notificationBL)
        {
            _notificationBL = notificationBL;
        }
    }
}
