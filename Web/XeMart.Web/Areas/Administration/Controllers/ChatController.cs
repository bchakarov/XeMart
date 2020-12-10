namespace XeMart.Web.Areas.Administration.Controllers
{
    using Microsoft.AspNetCore.Mvc;

    public class ChatController : AdministrationController
    {
        public IActionResult Index()
        {
            return this.View();
        }
    }
}
