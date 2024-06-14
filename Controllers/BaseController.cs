using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace dotnet_CRUD.Controllers
{
    public class BaseController : Controller
    {
        protected string FetchValidationErrorMessage(DbEntityValidationException dbEntityValidationException)
        {
            var entityError = dbEntityValidationException.EntityValidationErrors.SelectMany(x => x.ValidationErrors).Select(x => x.ErrorMessage);

            var getFullMessage = string.Join("\n", entityError);
            var exceptionMessage = string.Concat("errors are: \n", getFullMessage);
            return exceptionMessage;
        }
    }
}