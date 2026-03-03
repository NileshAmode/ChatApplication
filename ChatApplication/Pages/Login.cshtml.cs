using ChatApplication.Entity;
using ChatApplication.Handler;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;

namespace ChatApplication.Pages
{
    public class LoginModel : PageModel
    {


        [BindProperty]
        public LoginEntity loginEntity { get; set; }
        [BindProperty]
        public string ErrorMessage { get; set; }

        [BindProperty]
        public string ErrorMessagePassword { get; set; }
        public void OnGet()
        {
            HttpContext.Session.Remove("LoginType");
            HttpContext.Session.Remove("CustomerID");
            HttpContext.Session.Remove("FullName");
            HttpContext.Session.Remove("LocationID");
            HttpContext.Session.Remove("MobileNumber");
            HttpContext.Session.Remove("LoginID");
            HttpContext.Session.Remove("EmailID");
            HttpContext.Session.Remove("UserID");
            HttpContext.Session.Remove("FinalStatus");
            HttpContext.Session.Remove("CustomerCode");
            HttpContext.Session.Remove("GenderText");
            HttpContext.Session.Remove("IsCompleted");
            HttpContext.Session.Remove("MembershipName");
            HttpContext.Session.Remove("IsMembership");
            HttpContext.Session.Remove("CompanyName");
            HttpContext.Session.Remove("PlanType");
            HttpContext.Session.Remove("MembershipExpired");
        }


        public async Task<IActionResult> OnPost()
        {

            if (loginEntity.LoginID == null)
            {

                ErrorMessage = "Please Enter Login ID";
                return Page();
            }

            if (loginEntity.LoginPassword == null )
            {
                ErrorMessagePassword = "Please Enter Password";
                return Page();
            }

            string ipAddress = HttpContext.Request.Headers["X-Forwarded-For"].FirstOrDefault() ?? HttpContext.Connection.RemoteIpAddress?.ToString();

            if (ipAddress == "::1")
            {
                ipAddress = "127.0.0.1";
            }

           

            var result = await _loginService.ValidateLogin(loginModel, ipAddress).ConfigureAwait(false);

            if (result != null)
            {

                if (result.ErrorCode == -150)
                {
                    ErrorMessage = "Invalid Login ID / Username or Password";
                    return Page();
                }
                else if (result.ErrorCode == -201)
                {
                   
                }
               

                string LoginType = CommonUtility.GetString(result.LoginType.ToString());
                string CustomerID = CommonUtility.GetString(result.CustomerID.ToString());
                string FullName = CommonUtility.GetString(result.FullName.ToString());
                string LocationID = CommonUtility.GetString(result.LocationID.ToString());
                string MobileNo = CommonUtility.GetString(result.MobileNo.ToString());
                string EmailID = CommonUtility.GetString(result.EmailID.ToString());
                string LoginID = CommonUtility.GetString(result.LoginID.ToString());
                string UserID = CommonUtility.GetString(result.UserID.ToString());
                string FinalStatus = CommonUtility.GetString(result.FinalStatus.ToString());
                string IsCompleted = CommonUtility.GetString(result.IsCompleted.ToString());
                string CustomerCode = CommonUtility.GetString(result.CustomerCode.ToString());
                string GenderText = CommonUtility.GetString(result.GenderText.ToString());
                string IsApproved = CommonUtility.GetString(result.IsApproved.ToString());
                string IsMembership = CommonUtility.GetString(result.IsMembership.ToString());
                string MembershipName = CommonUtility.GetString(result.MembershipName.ToString());
                string CompanyName = CommonUtility.GetString(result.CompanyName.ToString());
                string PlanType = CommonUtility.GetString(result.PlanType.ToString());

                string MembershipExpired = CommonUtility.GetString(result.MembershipExpired.ToString());


                HttpContext.Session.SetString("LoginType", LoginType);
                HttpContext.Session.SetString("CustomerID", CustomerID);
                HttpContext.Session.SetString("FullName", FullName);
                HttpContext.Session.SetString("LocationID", LocationID);
                HttpContext.Session.SetString("MobileNumber", MobileNo);
                HttpContext.Session.SetString("LoginID", LoginID);
                HttpContext.Session.SetString("EmailID", EmailID);
                HttpContext.Session.SetString("UserID", UserID);
                HttpContext.Session.SetString("FinalStatus", FinalStatus);
                HttpContext.Session.SetString("IsCompleted", IsCompleted);
                HttpContext.Session.SetString("CustomerCode", CustomerCode);
                HttpContext.Session.SetString("GenderText", GenderText);
                HttpContext.Session.SetString("IsApproved", IsApproved);
                HttpContext.Session.SetString("IsMembership", IsMembership);
                HttpContext.Session.SetString("MembershipName", MembershipName);
                HttpContext.Session.SetString("CompanyName", CompanyName);
                HttpContext.Session.SetString("PlanType", PlanType);
                HttpContext.Session.SetString("MembershipExpired", MembershipExpired);
            }
            return Page();


        }
    }
}
