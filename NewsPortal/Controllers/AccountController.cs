using System.Web;
using System.Web.Mvc;
using NewsPortal.Models.DataBaseModels;
using NewsPortal.Models.ViewModels;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using NewsPortal.Account;
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
        public ActionResult Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                var result = SignInManager.PasswordSignIn(model.Login, model.Password, false, false);
                if (result == SignInStatus.Success)
                {
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    return RedirectToAction("Login", "Account");
                }

            }
            return View(model);
        }
        public ActionResult LogOff()
        {
            SignInManager.SignOut();
            return RedirectToAction("Index", "Home");
        }
        public SignInManager SignInManager
        {
            get { return HttpContext.GetOwinContext().Get<SignInManager>(); }
        }

        public UserManager UserManager
        {
            get { return HttpContext.GetOwinContext().GetUserManager<UserManager>(); }
        }

        [HttpGet]
        public ActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> Register(RegisterViewModel registerModel)
        {
            if (!ModelState.IsValid)
            {
                return View(registerModel);
            }
            using (var session = NHibernateHelper.GetCurrentSession())
            using (var transaction = session.BeginTransaction())
            {
                var user = session.QueryOver<User>().Where(u => u.Email == registerModel.Email).SingleOrDefault();
                if (user != null)
                {
                    ModelState.AddModelError("Email", "This E-mail address is not available.");
                    return View(registerModel);
                }
                var newUser = new User()
                {
                    Email = registerModel.Email,
                    Login = registerModel.Login,
                    Password = registerModel.Password,
                    UserName = registerModel.UserName,
                    //PasswordHash = registerModel.Password,    
                    EmailConfirmed = false
                };
                var creationResult = UserManager.Create(newUser, registerModel.Password);
                if (creationResult.Succeeded)
                {
                    session.Save(newUser);
                    transaction.Commit();

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
                }
            }
            return RedirectToAction("Login", "Account");
        }

        [AllowAnonymous]
        public async Task<ActionResult> ConfirmEmail(int userId, string code)
        {
            if (code == null)
            {
                return RedirectToAction("Index", "Home");
            }
            var result = await UserManager.ConfirmEmailAsync(userId, code);
            if (result.Succeeded)
            {
                using (var session = NHibernateHelper.GetCurrentSession())
                using (var transaction = session.BeginTransaction())
                {
                    var user = session.Get<User>(userId);
                    user.EmailConfirmed = true;
                    session.Update(user);
                    transaction.Commit();
                }
                return View("Login");
            }
            else
            {
                return View("Register");
            }
        }
    }
}
