namespace XeMart.Web.Areas.Administration.Controllers
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Mvc;

    using XeMart.Services.Data;
    using XeMart.Services.Mapping;
    using XeMart.Web.ViewModels.Administration.UserMessages;

    public class UserMessagesController : AdministrationController
    {
        private readonly IUserMessagesService userMessagesService;

        public UserMessagesController(IUserMessagesService userMessagesService)
        {
            this.userMessagesService = userMessagesService;
        }

        public IActionResult Index(string id)
        {
            var userMessages = this.userMessagesService.All().OrderByDescending(x => x.CreatedOn).ToList();
            if (userMessages.Count == 0)
            {
                return this.View(new IndexUserMessageViewModel());
            }

            var currentUserMessage = this.userMessagesService.GetById(id);
            if (currentUserMessage == null)
            {
                currentUserMessage = userMessages.FirstOrDefault();
            }

            this.userMessagesService.SetIsRead(id, true);

            var userMessageViewModelCollection = AutoMapperConfig.MapperInstance.Map<IEnumerable<UserMessageViewModel>>(userMessages);
            var currentUserMessageViewModel = AutoMapperConfig.MapperInstance.Map<UserMessageViewModel>(currentUserMessage);

            var viewModel = new IndexUserMessageViewModel
            {
                UserMessageViewModelCollection = userMessageViewModelCollection,
                UserMessageViewModel = currentUserMessageViewModel,
            };

            return this.View(viewModel);
        }

        public async Task<IActionResult> Delete(string id)
        {
            await this.userMessagesService.Delete(id);

            return this.RedirectToAction(nameof(this.Index));
        }

        public async Task<IActionResult> Unread(string id)
        {
            await this.userMessagesService.SetIsRead(id, false);

            return this.RedirectToAction(nameof(this.Index));
        }
    }
}
