using System.Web.Mvc;
using System.Web;
using NewsPortal.Models.DataBaseModels;
using NewsPortal.Models.ViewModels;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using NewsPortal.Managers.Identity;
using NewsPortal.Managers.NHibernate;
using System.Threading.Tasks;
using NewsPortal.Domain;

namespace NewsPortal.Controllers
{
    public class AccountController : Controller
    {
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                var result = await SignInManager.PasswordSignInAsync(model.UserName, model.Password, false, false);
                if (result == SignInStatus.Success)
                {
                    return RedirectToAction("Index", "News");
                }
                else
                {
                    ModelState.AddModelError("", "Wrong login or password!");
                }
            }
            return View(model);
        }

        [Authorize]
        public ActionResult LogOff()
        {
            SignInManager.SignOut();
            return RedirectToAction("Index", "News");
        }

        public ActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Register(RegisterViewModel registerModel)
        {
            if (ModelState.IsValid)
            {
                var userRepository = UnityConfig.Resolve<IUserRepository>();

                var userByEmail = await userRepository.GetByEmail(registerModel.Email);
                if (userByEmail != null)
                {
                    ModelState.AddModelError("Email", $"This E-mail address is not available.");
                    return View(registerModel);
                }

                var userByUserName = await userRepository.GetByName(registerModel.UserName);
                if (userByUserName != null)
                {
                    ModelState.AddModelError("UserName", "This UserName is not available.");
                    return View(registerModel);
                }

                var newUser = new User()
                {
                    Email = registerModel.Email,
                    UserName = registerModel.UserName,
                    Password = registerModel.Password,
                    EmailConfirmed = false
                };

                var creationResult = await UserManager.CreateAsync(newUser, newUser.Password);
                if (creationResult.Succeeded)
                {
                    var userId = newUser.Id;

                    await SendEmailConfirmation(userId, newUser.Email);

                    return View("SuccesfulRegistration");
                }
                ModelState.AddModelError("", "Unsuccessful registration. Please try again");
            }

            return View(registerModel);
        }

        private async Task SendEmailConfirmation(int? userId, string email)
        {
            if (userId != null && email != null)
            {
                string code = await UserManager.GenerateEmailConfirmationTokenAsync(userId.Value);
                var callbackUrl = Url.Action("ConfirmEmail", "Account",
                    new { userId = userId.Value, code = code },
                    protocol: Request.Url.Scheme);
                var message = new IdentityMessage()
                {
                    Body = "Confirm your account by clicking <a href=\"" + callbackUrl + "\">here</a>",
                    Subject = "Account confirmation",
                    Destination = email
                };
                await UserManager.EmailService.SendAsync(message);
            }
        }

        public async Task<ActionResult> ConfirmEmail(int? userId, string code)
        {
            if (userId != null && code != null)
            {
                var result = await UserManager.ConfirmEmailAsync(userId.Value, code);

                if (result.Succeeded)
                {
                    var userRepository = UnityConfig.Resolve<IUserRepository>();

                    var user = await userRepository.GetById(userId.Value);
                    user.EmailConfirmed = true;
                    await userRepository.Update(user);

                    return View("SuccesfulConfirmation");
                }
            }
            return View("UnsuccesfulConfirmation");
        }

        private SignInManager SignInManager
        {
            get { return HttpContext.GetOwinContext().Get<SignInManager>(); }
        }

        private UserManager UserManager
        {
            get { return HttpContext.GetOwinContext().GetUserManager<UserManager>(); }
        }
    }
}