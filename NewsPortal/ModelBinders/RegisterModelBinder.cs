using NewsPortal.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace NewsPortal.ModelBinders
{
    public class RegisterModelBinder : IModelBinder
    {
        public object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
        {
            var valueProvider = bindingContext.ValueProvider;

            var email = (string)valueProvider.GetValue("Email").ConvertTo(typeof(string));
            var userName = (string)valueProvider.GetValue("UserName").ConvertTo(typeof(string));
            var password = (string)valueProvider.GetValue("Password").ConvertTo(typeof(string));
            var confirmPassword = (string)valueProvider.GetValue("ConfirmPassword").ConvertTo(typeof(string));
            var user = new RegisterViewModel()
            {
                Email = email,
                UserName = userName,
                Password = password,
                ConfirmPassword = confirmPassword
            };

            return user;
        }
    }
}