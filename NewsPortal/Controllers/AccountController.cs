using System.Web.Mvc;
using System.Web;
using NewsPortal.Models.DataBaseModels;
using NewsPortal.Models.ViewModels;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using NewsPortal.Managers.Identity;
using NewsPortal.Managers.NHibernate;

namespace NewsPortal.Controllers
{
    public class AccountController : Controller
    {
        [HttpGet]
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
                    //---------------------------------------------
                    ViewBag.Message = "Incorrect login or password";
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

        public ActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Register(RegisterViewModel registerModel)
        {
            if (ModelState.IsValid)
            {
                using (var session = NHibernateManager.GetCurrentSession())
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
                        EmailConfirmed = false
                    };

                    var creationResult = UserManager.Create(newUser, registerModel.Password);
                    if (creationResult.Succeeded)
                    {
                        session.Save(newUser);

                        string code = UserManager.GenerateEmailConfirmationToken(newUser.Id);
                        var callbackUrl = Url.Action("ConfirmEmail", "Account", new { userId = newUser.Id, code = code },
                            protocol: Request.Url.Scheme);
                        var message = new IdentityMessage()
                        {
                            Body = "Confirm your account by clicking <a href=\"" + callbackUrl + "\">here</a>",
                            Subject = "Account confirmation",
                            Destination = newUser.Email
                        };
                        UserManager.EmailService.Send(message);

                        return View("SuccesfulRegistration");
                    }
                    ModelState.AddModelError("", "Unsuccessful registration. Please try again");
                }
            }

            return View(registerModel);
        }

        public ActionResult ConfirmEmail(int? userId, string code)
        {
            if (userId != null && code != null)
            {
                var result = UserManager.ConfirmEmail(userId.Value, code);

                if (result.Succeeded)
                {
                    using (var session = NHibernateManager.GetCurrentSession())
                    {
                        var user = session.Get<User>(userId);
                        user.EmailConfirmed = true;
                        session.Update(user);
                    }
                    return View("SuccesfulConfirmation");
                }
            }
            return View("UnsuccesfulConfirmation");
        }
    }
}