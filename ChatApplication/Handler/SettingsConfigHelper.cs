namespace ChatApplication.Handler
{
    public class SettingsConfigHelper
    {
        private static IConfiguration _configuration;

        public static void Appsetingconfig(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public static string settingvalue()
        {
            string con = _configuration.GetConnectionString("SqlConnection");
            return con;
        }
        public static string LocationPath()
        {
            //string con = _configuration.GetSection("RequestLoanDocument").GetSection("SharePath").Value;
            string con = Path.Combine("wwwroot", "Document");
            return con;
        }

        public static string PDFPath()
        {
            string con = _configuration.GetSection("PDFPath").GetSection("PathAddress").Value;
            return con;
        }

        public static string IsSiteIsDown()
        {
            string con = _configuration.GetSection("IsSiteIsDown").GetSection("IsSiteIsDownValue").Value;
            return con;
        }

        public static string ServiceIconPath()
        {
            //string con = _configuration.GetSection("RequestLoanDocument").GetSection("SharePath").Value;
            string con = Path.Combine("wwwroot", "ServiceIcon");
            return con;
        }
        public static string LocationFilesPath()
        {
            //string con = _configuration.GetSection("RequestLoanDocument").GetSection("SharePath").Value;
            string con = Path.Combine("wwwroot", "uploads");
            return con;
        }

        #region category single file Upload
        public static string GetImagesLocation(string flag)
        {
            //string con = _configuration.GetSection("RequestLoanDocument").GetSection("SharePath").Value;
            string con = Path.Combine("wwwroot", "uploads", flag);
            return con;
        }
        #endregion

        public static string GetCustomerDocumentLocation()
        {
            //string con = _configuration.GetSection("RequestLoanDocument").GetSection("SharePath").Value;
            string con = Path.Combine("wwwroot", "uploads", "Customer");
            return con;
        }

        public static string CustomerDocumentLocation(string ReportName)
        {

            //string con = _configuration.GetSection("RequestLoanDocument").GetSection("SharePath").Value;
            string con = Path.Combine("wwwroot", "uploads", "Customer", ReportName);
            return con;
        }


        public static string ErroPagekey()
        {
            string con = _configuration.GetSection("ProductionErrorPage")["ErrorKey"];
            return con;
        }


        public static string NumberYears()
        {
            string con = _configuration.GetSection("NumberofYears")["Years"];
            return con;
        }

        public static string MerchantId()
        {
            string con = _configuration.GetSection("Eazypay")["MerchantId"];
            return con;
        }
        public static string AccessCode()
        {
            string con = _configuration.GetSection("Eazypay")["AccessCode"];
            return con;
        }
        public static string RedirectUrl()
        {
            string con = _configuration.GetSection("Eazypay")["RedirectUrl"];
            return con;
        }

        public static string URL()
        {
            string con = _configuration.GetSection("Eazypay")["Url"];
            return con;
        }

        public static long MaxFileSize()
        {
            long con = Convert.ToInt64(_configuration.GetSection("FileSizeDocument")["MaxFileSize"]);
            return con;
        }
        public static string BaseURL()
        {
            string con = _configuration.GetSection("AppDefaultSetting")["BaseUrl"];
            return con;
        }
    }
}
