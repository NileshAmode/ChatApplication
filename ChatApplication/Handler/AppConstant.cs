namespace ChatApplication.Handler
{
    public class AppConstant
    {
        public static string GetErrorCodeMessage(int Error_Code)
        {
            string strErrorMessage = string.Empty;
            switch (Error_Code)
            {
                case 0:
                    strErrorMessage = "Record Saved Successfully.";
                    break;
                case -1:
                    strErrorMessage = "Record Deleted Successfully.";
                    break;
                case -101:
                    strErrorMessage = " OTP Verified Successfully.(ERR -101)";
                    break;
                case -102:
                    strErrorMessage = "  OTP Verified Failed .(ERR -102)";
                    break;
                case -103:
                    strErrorMessage = "  OTP Verified Successfully";
                    break;
                case -104:
                    strErrorMessage = "Email Id Already Exists";
                    break;
                case -105:
                    strErrorMessage = "Mobile Id Already Exists";
                    break;
                case -106:
                    strErrorMessage = "Company Id Already Exists";
                    break;
                case -107:
                    strErrorMessage = "Lookup type and Lookup Name is Already Exists.";
                    break;
                case -201:
                    strErrorMessage = "Please fill the mandatory fields.";
                    break;

                case -999:
                    strErrorMessage = "Password Updated Successfully.";
                    break;
                case -901:
                    strErrorMessage = "Invalid details.";
                    break;
                case 001:
                    strErrorMessage = "Your profile has been submitted successfully and is now pending admin approval.";
                    break;
                default:
                    strErrorMessage = "Some Error Occurred.";
                    break;
            }
            return strErrorMessage;
        }
    }
}
