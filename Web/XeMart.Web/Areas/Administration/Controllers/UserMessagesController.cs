namespace XeMart.Web.Areas.Administration.Controllers
{
    using System.Linq;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Mvc;

    using XeMart.Services.Data;
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
            var userMessages = this.userMessagesService.GetAll<UserMessageViewModel>().ToList();
            if (userMessages.Count == 0)
            {
                return this.View(new IndexUserMessageViewModel<UserMessageViewModel>());
            }

            var currentUserMessage = this.userMessagesService.GetById<UserMessageViewModel>(id);
            if (currentUserMessage == null)
            {
                currentUserMessage = userMessages.FirstOrDefault();
            }

            this.userMessagesService.SetIsReadAsync(id, true);

            var viewModel = new IndexUserMessageViewModel<UserMessageViewModel>
            {
                UserMessageViewModelCollection = userMessages,
                UserMessageViewModel = currentUserMessage,
            };

            return this.View(viewModel);
        }

        public IActionResult Deleted(string id)
        {
            var userMessages = this.userMessagesService.GetAllDeleted<DeletedUserMessagesViewModel>().ToList();
            if (userMessages.Count == 0)
            {
                return this.View(new IndexUserMessageViewModel<DeletedUserMessagesViewModel>());
            }

            var currentUserMessage = this.userMessagesService.GetById<DeletedUserMessagesViewModel>(id);
            if (currentUserMessage == null)
            {
                currentUserMessage = userMessages.FirstOrDefault();
            }

            this.userMessagesService.SetIsReadAsync(id, true);

            var viewModel = new IndexUserMessageViewModel<DeletedUserMessagesViewModel>
            {
                UserMessageViewModelCollection = userMessages,
                UserMessageViewModel = currentUserMessage,
            };

            return this.View(viewModel);
        }

        public async Task<IActionResult> Delete(string id)
        {
            var deleteResult = await this.userMessagesService.DeleteAsync(id);
            if (deleteResult)
            {
                this.TempData["Alert"] = "Successfully deleted message.";
            }
            else
            {
                this.TempData["Error"] = "There was a problem deleting the message.";
            }

            return this.RedirectToAction(nameof(this.Index));
        }

        public async Task<IActionResult> Undelete(string id)
        {
            var undeleteResult = await this.userMessagesService.UndeleteAsync(id);
            if (undeleteResult)
            {
                this.TempData["Alert"] = "Successfully undeleted message.";
            }
            else
            {
                this.TempData["Error"] = "There was a problem undeleting the message.";
            }

            return this.RedirectToAction(nameof(this.Deleted));
        }

        public async Task<IActionResult> Unread(string id)
        {
            var unreadResult = await this.userMessagesService.SetIsReadAsync(id, false);
            if (unreadResult)
            {
                this.TempData["Alert"] = "Successfully marked message as unread";
            }
            else
            {
                this.TempData["Error"] = "There was a problem marking the message as unread.";
            }

            return this.RedirectToAction(nameof(this.Index));
        }
    }
}
