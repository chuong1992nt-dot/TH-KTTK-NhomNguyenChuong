using ASC.Web.Models;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.Collections.Generic;

namespace ASC.Web.Areas.Accounts.Models
{
    public class CustomerViewModel
    {
        [ValidateNever] 
        public List<ApplicationUser> Customers { get; set; }

        public CustomerRegistrationViewModel Registration { get; set; }
    }
}