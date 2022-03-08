using System.Threading.Tasks;
using Elect.Mapper.AutoMapper.ObjUtils;
using TIGE.Contract.Service;
using Microsoft.AspNetCore.Mvc;
using TIGE.Core.Constants;
using TIGE.Core.Share.Attributes.Auth;
using TIGE.Core.Share.Constants;
using TIGE.Core.Share.Models.User;
using TIGE.Core.Share.Utils;
using TIGE.Core.Utils;
using TIGE.Utils;
using TIGE.Utils.Notification;
using TIGE.Utils.Notification.Models.Constants;

namespace TIGE.Areas.Portal.Controllers
{
    [Auth(Permission.Admin, Permission.Manager)]
    public class UserController : BaseController
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet]
        [Route(PortalEndpoint.User.GetPagedEndpoint)]
        public IActionResult Index()
        {
            return View();
        }

        #region Add

        [HttpGet]
        [Route(PortalEndpoint.User.AddEndpoint)]
        public IActionResult Add()
        {
            return View(new CreateUserModel());
        }

        [HttpPost]
        [Route(PortalEndpoint.User.AddEndpoint)]
        public async Task<IActionResult> SubmitAdd([FromForm] CreateUserModel model)
        {
            if (!ModelState.IsValid)
            {
                this.SetNotification( Messages.Common.InValidFormDataMessage, NotificationStatus.Error);

                return View("Add", model);
            }

            await _userService.CreateAsync(model, this.GetRequestCancellationToken()).ConfigureAwait(true);

            this.SetNotification(
                string.Format(Messages.User.AddedFormat, model.Email),
                NotificationStatus.Success);

            return RedirectToAction("Add");
        }

        #endregion

        #region Edit

        [HttpGet]
        [Route(PortalEndpoint.User.EditEndpoint)]
        public async Task<IActionResult> Edit(string id)
        {
            var user = await _userService.GetByIdAsync(id, this.GetRequestCancellationToken());

            if (user == null)
            {
                this.SetNotification(
                    string.Format(Messages.User.DoesNotExist),
                    NotificationStatus.Error);

                return View("Index");
            }

            return View(user);
        }

        [HttpPost]
        [Route(PortalEndpoint.User.EditEndpoint)]
        public async Task<IActionResult> SubmitEdit([FromForm] UserModel model)
        {
            if (!ModelState.IsValid)
            {
                this.SetNotification( Messages.Common.InValidFormDataMessage, NotificationStatus.Error);

                return View("Edit", model);
            }

            await _userService.UpdateAsync(model, this.GetRequestCancellationToken()).ConfigureAwait(true);

            this.SetNotification(
                string.Format(Messages.User.UpdatedFormat, model.Email),
                NotificationStatus.Success);

            return RedirectToAction("Edit");
        }

        #endregion

        #region Profile

        [HttpGet]
        [Route(PortalEndpoint.User.EditProfileEndpoint)]
        public IActionResult EditProfile(string id)
        {
            var profileModel = LoggedInUser.Current.MapTo<ProfileModel>();

            return View(profileModel);
        }


        [HttpGet]
        [Route(PortalEndpoint.User.ResendEndpoint)]
        public async Task<IActionResult> Resend([FromRoute] string id)
        {
            await _userService.ResendConfirmRegisterAsync(id);
            this.SetNotification("Resend register confirmation mail successfully.", NotificationStatus.Success);
            return RedirectToAction("Edit", new {id = id});
        }

        [HttpPost]
        [Route(PortalEndpoint.User.EditProfileEndpoint)]
        public async Task<IActionResult> SubmitEditProfile([FromForm] ProfileModel model)
        {
            if (!ModelState.IsValid)
            {
                this.SetNotification( Messages.Common.InValidFormDataMessage, NotificationStatus.Error);

                return View("EditProfile", model);
            }

            await _userService.UpdateProfileAsync(model, this.GetRequestCancellationToken()).ConfigureAwait(true);

            this.SetNotification( string.Format(Messages.User.UpdatedProfile), NotificationStatus.Success);

            return RedirectToAction("EditProfile");
        }

        #endregion
    }
}