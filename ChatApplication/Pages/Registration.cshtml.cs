using AspNetCoreHero.ToastNotification.Abstractions;
using ChatApplication.Entity;
using ChatApplication.Handler;
using ChatApplication.Interface;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ChatApplication.Pages
{
    public class RegistrationModel : PageModel
    {
        private readonly ICustomerInfo Objservice;
        private readonly INotyfService _toastNotification;

        public RegistrationModel(ICustomerInfo objservice, INotyfService toastNotification)
        {
            Objservice = objservice;
            _toastNotification = toastNotification;
        }

        [BindProperty]
        public CustomerInfo __customerEntity { get; set; }
        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPost()
        {
            if (__customerEntity != null)
            {
                var obj = await Objservice.InsertCustomerInfo(__customerEntity);
                if(obj > 0)
                {
                    _toastNotification.Success(AppConstant.GetErrorCodeMessage(0));
                }
                else
                {
                    _toastNotification.Success(AppConstant.GetErrorCodeMessage(0));
                }
            }
            return RedirectToPage("/Login");
        }
    }
}
