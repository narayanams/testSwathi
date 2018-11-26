using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace TRAILSWEB.Helpers
{
    public class EncryptedModelBinder : DefaultModelBinder
    {
        //protected override void BindProperty(ControllerContext controllerContext, ModelBindingContext bindingContext, System.ComponentModel.PropertyDescriptor propertyDescriptor)
        //{
        //    if (controllerContext.HttpContext.Request.Form[propertyDescriptor.Name + "-encrypted"] != null)
        //    {
        //        dynamic encryptedValue = controllerContext.HttpContext.Request.Form[propertyDescriptor.Name + "-encrypted"];
        //        encryptedValue = AESEncrytDecry.DecryptStringAES(encryptedValue);

        //        dynamic decryptedValue = Convert.ChangeType(encryptedValue, propertyDescriptor.PropertyType);

        //        propertyDescriptor.SetValue(bindingContext.Model, decryptedValue);
        //    }

        //    base.BindProperty(controllerContext, bindingContext, propertyDescriptor);
        //}
    }
}