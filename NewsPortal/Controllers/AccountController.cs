using System.Web.Mvc;
using System.Web;
using NewsPortal.Models.DataBaseModels;
using NewsPortal.Models.ViewModels;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using NewsPortal.Managers.Identity;
using NewsPortal.Managers.NHibernate;
using System.Threading.Tasks;

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
                    return RedirectToAction("Index", "Home");
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
            return RedirectToAction("Index", "Home");
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
                using (var manager = new NHibernateManager())
                {
                    var session = manager.GetSession();

                    var userByEmail = await session.QueryOver<User>().
                        Where(u => u.Email == registerModel.Email).
                        SingleOrDefaultAsync();

                    if (userByEmail != null)
                    {
                        ModelState.AddModelError("Email", $"This E-mail address is not available.");
                        return View(registerModel);
                    }

                    var userByUserName = await session.QueryOver<User>().
                        Where(u => u.UserName == registerModel.UserName).
                        SingleOrDefaultAsync();

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

                    var creationResult = UserManager.Create(newUser, newUser.Password);
                    if (creationResult.Succeeded)
                    {
                        session.Save(newUser);

                        string code = await UserManager.GenerateEmailConfirmationTokenAsync(newUser.Id);
                        var callbackUrl = Url.Action("ConfirmEmail", "Account", new { userId = newUser.Id, code = code },
                            protocol: Request.Url.Scheme);
                        var message = new IdentityMessage()
                        {
                            Body = "Confirm your account by clicking <a href=\"" + callbackUrl + "\">here</a>",
                            Subject = "Account confirmation",
                            Destination = newUser.Email
                        };
                        await UserManager.EmailService.SendAsync(message);

                        return View("SuccesfulRegistration");
                    }
                    ModelState.AddModelError("", "Unsuccessful registration. Please try again");
                }
            }

            return View(registerModel);
        }

        public async Task<ActionResult> ConfirmEmail(int? userId, string code)
        {
            if (userId != null && code != null)
            {
                var result = await UserManager.ConfirmEmailAsync(userId.Value, code);

                if (result.Succeeded)
                {
                    using (var manager = new NHibernateManager())
                    {
                        var session = manager.GetSession();

                        var user = session.Get<User>(userId);
                        user.EmailConfirmed = true;
                        session.Update(user);
                    }
                    return View("SuccesfulConfirmation");
                }
            }
            return View("UnsuccesfulConfirmation");
        }

        #region Вспомогательные приложения
        private SignInManager SignInManager
        {
            get { return HttpContext.GetOwinContext().Get<SignInManager>(); }
        }

        private UserManager UserManager
        {
            get { return HttpContext.GetOwinContext().GetUserManager<UserManager>(); }
        }
        #endregion
    }
}