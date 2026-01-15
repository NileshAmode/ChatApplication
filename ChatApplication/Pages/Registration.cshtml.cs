using ChatApplication.Entity;
using ChatApplication.Interface;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ChatApplication.Pages
{
    public class RegistrationModel : PageModel
    {
        private readonly ICustomerInfo Objservice;

        public RegistrationModel(ICustomerInfo objservice)
        {
            Objservice = objservice;
            
        }

        [BindProperty]
        public CustomerInfo customerEntity { get; set; }
        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPost()
        {
            var  obj = await Objservice.InsertCustomerInfo(customerEntity);
            return Page();
        }
    }
}
