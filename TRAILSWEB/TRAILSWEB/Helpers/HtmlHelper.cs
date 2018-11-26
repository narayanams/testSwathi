using System;
using System.Linq.Expressions;
using System.Web.Mvc;

namespace TRAILSWEB.Helpers
{
    public static class HtmlHelper
    {

        /// <summary>
        /// Creates an encrypted version of the field
        /// </summary>
        //[System.Runtime.CompilerServices.Extension()]
        public static MvcHtmlString EncryptedFor<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression, bool doEncrypt)
        {
            string name = null;
            if (expression.Body is MemberExpression)
            {
                name = ((MemberExpression)expression.Body).Member.Name;
            }
            else
            {
                dynamic op = (((UnaryExpression)expression.Body).Operand);
                name = ((MemberExpression)op).Member.Name;
            }

            //Get the value, and then encrypt it
            dynamic value = ModelMetadata.FromLambdaExpression(expression, htmlHelper.ViewData);
            if (doEncrypt)
            {
                dynamic encvalue = AESEncrytDecry.EncryptStringAES((value.Model ?? "").ToString());//RijndaelSimple.Encrypt(value.Model, HttpContext.Current.User.Identity.Name);
                return new MvcHtmlString("<input type=\"hidden\" name=\"" + name + "-encrypted\" value=\"" + encvalue + "\">");
            }
            else
            {
                return new MvcHtmlString("<input type=\"hidden\" name=\"" + name + "\" value=\"" + value.Model + "\">");
            }
        }



    }
}