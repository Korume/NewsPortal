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

            ValueProviderResult providerId = valueProvider.GetValue("Id");

            int id = 0;
            string email = (string)valueProvider.GetValue("Email").ConvertTo(typeof(string));
            string userName = (string)valueProvider.GetValue("UserName").ConvertTo(typeof(string));
            string password = (string)valueProvider.GetValue("Password").ConvertTo(typeof(string));
            string confirmPassword = (string)valueProvider.GetValue("ConfirmPassword").ConvertTo(typeof(string));
            var user = new RegisterViewModel()
            {
                Id = id,
                Email = email,
                UserName = userName,
                Password = password,
                ConfirmPassword = confirmPassword
            };

            return user;
        }
    }
}