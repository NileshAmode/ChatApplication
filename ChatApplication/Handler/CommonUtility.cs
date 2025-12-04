using Newtonsoft.Json;
using System.Data;
using System.Globalization;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;

namespace ChatApplication.Handler
{
    public class CommonUtility
    {
        public static string _defaultDateFormat = "dd-MMM-yyyy";


        private readonly IHttpContextAccessor _httpContextAccessor;

        public CommonUtility(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }


        public string GetSessionValue(string key)
        {
            var context = _httpContextAccessor.HttpContext;
            if (context is not null)
            {
                if (context.Session.GetString(key) == null)
                {
                    return "";
                }

                return context.Session.GetString(key);
            }

            return "";
        }

        public int GetSessionValueInt(string key)
        {
            var context = _httpContextAccessor.HttpContext;
            if (context is not null)
            {
                if (context.Session.GetString(key) == null)
                {
                    return 0;
                }

                return Convert.ToInt32(context.Session.GetString(key));
            }
            return 0;
        }
        public static bool IsValidAccess(string PageName, string strDesignationID, string strPageRoles)
        {

            if (strDesignationID == "superadmin")
            {
                return true;
            }


            if (strPageRoles == "")
            {
                return false;
            }

            PageName = CommonUtility.RemoveFirstChar(PageName);

            string[] ArrTemp = strPageRoles.Split("#");

            for (int i = 0; i < ArrTemp.Length; i++)
            {
                if (ArrTemp[i].ToString().ToLower().Trim() == PageName.ToLower().Trim())
                {
                    return true;
                }
            }

            return false;


        }




        public static bool IsNumber(String strNumber)
        {
            Regex objNotNumberPattern = new Regex("[^0-9.-]");
            Regex objTwoDotPattern = new Regex("[0-9]*[.][0-9]*[.][0-9]*");
            Regex objTwoMinusPattern = new Regex("[0-9]*[-][0-9]*[-][0-9]*");
            String strValidRealPattern = "^([-]|[.]|[-.]|[0-9])[0-9]*[.]*[0-9]+$";
            String strValidIntegerPattern = "^([-]|[0-9])[0-9]*$";
            Regex objNumberPattern = new Regex("(" + strValidRealPattern + ")|(" + strValidIntegerPattern + ")");
            return !objNotNumberPattern.IsMatch(strNumber) &&
                !objTwoDotPattern.IsMatch(strNumber) &&
                !objTwoMinusPattern.IsMatch(strNumber) &&
                objNumberPattern.IsMatch(strNumber);
        }

        public static string GetBooleanIntValue(string strValue)
        {

            return strValue.ToLower().Replace("true", "1").Replace("false", "0");
        }

        public static bool IsDataTableEmpty(DataTable dt)
        {
            if (dt == null)
            {
                return true;
            }

            if (dt.Rows.Count == 0)
            {
                return true;
            }

            return false;
        }

        public static bool IsDataSetEmpty(DataSet ds)
        {
            if (ds == null)
            {
                return true;
            }

            if (ds.Tables.Count == 0)
            {
                return true;
            }

            if (ds.Tables[0].Rows.Count == 0)
            {
                return true;
            }

            return false;
        }


        public static Int32 GetInt32(string val)
        {
            try
            {
                if (string.IsNullOrEmpty(val))
                {
                    return Convert.ToInt32("0");
                }
                val = val.Trim();
                if (IsNumber(val) == false)
                {
                    return Convert.ToInt32("0");
                }

                return Convert.ToInt32(val);
            }
            catch (Exception ex)
            {
                return 0;
            }

            return Convert.ToInt32(val);

        }



        public static Decimal GetDecimal(string val)
        {
            try
            {
                if (string.IsNullOrEmpty(val))
                {

                    return Math.Round(Convert.ToDecimal("0"), 2);
                }
                val = val.Trim();
                if (IsNumber(val) == false)
                {
                    return Math.Round(Convert.ToDecimal("0"), 2);
                }


                return Math.Round(Convert.ToDecimal(val), 2);
            }
            catch (Exception ex)
            {
                return 0;
            }

            return Math.Round(Convert.ToDecimal(val), 2);

        }
        // function to pass null value to proc
        public static string GetString(string val)
        {
            if (string.IsNullOrEmpty(val))
            {
                return "";
            }
            else if (string.IsNullOrWhiteSpace(val))
            {
                return "";
            }
            else
            {
                return val.Trim();
            }


        }


        public static DateTime GetTodaysDate()
        {
            return Convert.ToDateTime(DateTime.Today.Date.ToString(_defaultDateFormat).ToUpper());
        }

        public static string GetTodaysDateString()
        {
            return DateTime.Today.Date.ToString(_defaultDateFormat).ToUpper();
        }

        public static string FormatDate(string val)
        {
            if (string.IsNullOrEmpty(val))
            {
                val = "1-JAN-1900";
            }

            return GetDate(val).ToString(_defaultDateFormat).ToUpper();


        }


        public static string FormatDate(DateTime val)
        {
            return val.ToString(_defaultDateFormat).ToUpper();

        }
        public static DateTime GetDate(string val)
        {
            try
            {
                if (string.IsNullOrEmpty(val))
                {
                    val = "1-JAN-1900";
                }

                if (validateDateFormat(val) == false)// incorrect date format
                {
                    Convert.ToDateTime("1-JAN-1900");
                }

                return Convert.ToDateTime(val.ToString());
            }
            catch (Exception ex)
            {
                return Convert.ToDateTime("1-JAN-1900");
            }
            return Convert.ToDateTime(val.ToString());
        }
        public static TimeSpan GetTime(string val)
        {
            // Default time if input is null or empty
            if (string.IsNullOrEmpty(val))
            {
                val = "00:00:00"; // Default to midnight
            }

            // Validate time format
            if (!TimeSpan.TryParse(val, out TimeSpan parsedTime))
            {
                return TimeSpan.Zero; // Default time on error (00:00:00)
            }

            return parsedTime;
        }

        public static bool validateDateFormat(string val)
        {
            DateTime dd;
            return System.DateTime.TryParse(val, out dd);

        }
        public static string GetDateString(string val)
        {

            if (string.IsNullOrEmpty(val))
            {
                return "";
            }


            if (validateDateFormat(val) == false)// incorrect date format
            {
                return "";
            }

            DateTime temp = Convert.ToDateTime(val);

            if (temp.Year <= 1910)
            {
                return "";
            }

            return Convert.ToDateTime(val.ToString()).ToString(_defaultDateFormat);

        }


        public static string FormatDateMM(string val)
        {
            if (string.IsNullOrEmpty(val))
            {
                return "";
            }


            if (validateDateFormat(val) == false)// incorrect date format
            {
                return "";
            }

            DateTime temp = Convert.ToDateTime(val);

            if (temp.Year <= 1910)
            {
                return "";
            }

            return Convert.ToDateTime(val.ToString()).ToString(_defaultDateFormat);

        }




        public static string RemoveFirstChar(string strIds)
        {
            if (strIds.Trim() != "")
            {
                strIds = strIds.Substring(1, strIds.Length - 1);
            }

            return strIds;
        }

        public static string RemoveFirstChar(string strIds, string separator)
        {
            if (strIds.Trim() != "")
            {

                if (strIds.Substring(0, 1) == separator)
                {
                    strIds = strIds.Substring(1, strIds.Length - 1);
                }
            }

            return strIds;
        }




        public static string CallingAPI(string Url, object obj)
        {
            string strResult = "";

            string JSONString1 = string.Empty;
            JSONString1 = JsonConvert.SerializeObject(obj);

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(Url);
            request.Method = "POST";
            request.ContentType = "application/json";
            //request.ContentLength = DATA.Length;
            request.ContentLength = JSONString1.Length;
            string OwnerId = "1";
            string ManagerId = "1";
            request.Headers["Authorization"] = "Basic " + Convert.ToBase64String(Encoding.Default.GetBytes("sa|sa1|sa2|" + OwnerId + "|" + ManagerId));
            StreamWriter requestWriter = new StreamWriter(request.GetRequestStream(), System.Text.Encoding.ASCII);
            // requestWriter.Write(DATA);
            requestWriter.Write(JSONString1);
            requestWriter.Close();

            try
            {
                WebResponse webResponse = request.GetResponse();

                Stream webStream = webResponse.GetResponseStream();
                StreamReader responseReader = new StreamReader(webStream);
                string response = responseReader.ReadToEnd();
                // response = response.Replace("'\'", "");
                // object dssss = JsonConvert.DeserializeObject(response);

                // DataSet myDataSet = JsonConvert.DeserializeObject<DataSet>(response);
                Console.Out.WriteLine(response);
                responseReader.Close();
                strResult = response;

            }
            catch (Exception e)
            {
                Console.Out.WriteLine("-----------------");
                Console.Out.WriteLine(e.Message);
            }
            return strResult;
        }

        public static string GetStringNew(string val)
        {
            if (string.IsNullOrEmpty(val))
            {
                return "0";
            }
            else if (string.IsNullOrWhiteSpace(val))
            {
                return "";
            }
            else
            {
                return val;
            }


        }

        #region Month, Day AND Year

        public static string _dateFormat = "dd-MMM-yyyy";
        public static string _dateFormatAjax = "dd-MMM-yyyy";


        public static string GetDateWithFormat(string val)
        {

            if (string.IsNullOrEmpty(val))
            {
                return "";
            }

            if (validateDateFormat(val) == false)// incorrect date format
            {
                return "";
            }

            DateTime temp = Convert.ToDateTime(val);

            if (temp.Year <= 1910)
            {
                return "";
            }

            return Convert.ToDateTime(val.ToString()).ToString(_dateFormat);
        }

        public static DateTime LastDayOfMonth(int month, int year)
        {
            DateTime dt;
            //Select the first day of the month by using the DateTime class
            dt = new DateTime(year, month, 1);// -------pass  DateTime(year, month, 1);
                                              //Add one month to our adjusted DateTime
            dt = dt.AddMonths(1);
            //Subtract one day from our adjusted DateTime
            dt = dt.AddDays(-1);
            //Return the DateTime, now set to the last day of the month
            return dt;
        }


        public static void GetDateRange(ref DateTime FromDate, ref DateTime ToDate, string ReportType, string month, string year)
        {

            if (ReportType == "day")
            {
                return;
            }

            FromDate = CommonUtility.GetTodaysDate();
            ToDate = CommonUtility.GetTodaysDate();



            if (ReportType == null)
            {
                return;
            }

            if (month == null)
            {
                month = DateTime.Now.Month.ToString();
            }

            if (year == null)
            {
                year = DateTime.Now.Year.ToString();
            }

            if (ReportType == "month")
            {
                FromDate = CommonUtility.FirstDayOfMonth(GetInt32(month), GetInt32(year));
                ToDate = CommonUtility.LastDayOfMonth(GetInt32(month), GetInt32(year));
            }
            else if (ReportType == "year")
            {
                FromDate = CommonUtility.FirstDayOfMonth(1, GetInt32(year));
                ToDate = CommonUtility.LastdayofYear(GetInt32(year));
            }
        }

        public static DateTime LastdayofYear(int year)
        {
            DateTime lastDayOfPreviousYear = new DateTime(year, 12, 31);
            return lastDayOfPreviousYear;

        }


        public static DateTime FirstDayOfMonth(int Month, int Year)
        {
            DateTime firstDayOfCurrentMonth = new DateTime(Year, Month, 1);
            return firstDayOfCurrentMonth;
        }

        public static DateTime FirstDayOfMonth()
        {
            DateTime firstDayOfCurrentMonth = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
            return firstDayOfCurrentMonth;
        }

        public static DateTime FirstDayOfMonth(DateTime dt)
        {
            DateTime firstDayOfCurrentMonth = new DateTime(dt.Year, dt.Month, 1);

            return firstDayOfCurrentMonth;
        }

        public static List<(string MonthName, int MonthNumber)> GetAllMonths()
        {
            List<(string MonthName, int MonthNumber)> result = new List<(string, int)>();

            for (int i = 1; i <= 12; i++)
            {
                string monthName = new DateTime(2022, i, 1).ToString("MMM"); // Get full month name
                result.Add((monthName, i));
            }

            return result;
        }


        public static List<string> GetMonthList() /*Return Only Month*/
        {
            List<string> months = new List<string>();


            // Get the DateTimeFormatInfo for the current culture
            DateTimeFormatInfo dtfi = DateTimeFormatInfo.CurrentInfo;
            //
            // Loop through the month names and add them to the list
            for (int i = 1; i <= 12; i++)
            {
                months.Add(dtfi.GetMonthName(i));
            }
            return months;
        }


        /*Return Year list*/
        public static List<int> GetYearList()
        {


            List<int> yearsList = new List<int>();


            int currentYear = DateTime.Now.Year + 2;


            for (int i = currentYear; i >= 2012; i--)
            {
                yearsList.Add(i);
            }
            return yearsList;
        }


        public static int GetMonthFromDate(DateTime date)
        {
            return date.Month;
        }


        public static int GetDayFromDate(DateTime date)
        {
            return date.Day;
        }


        public static int GetYearFromDate(DateTime date)
        {
            return date.Year;
        }


        /*Return month and year (JAN-2023)*/
        public static List<string> GetFormattedMonthYearList(int startYear, int endYear)
        {
            List<string> formattedMonthYearList = new List<string>();


            // Validate the input range
            if (startYear <= endYear)
            {
                // Loop through the months and years, adding formatted strings to the list
                for (int year = startYear; year <= endYear; year++)
                {
                    for (int month = 1; month <= 12; month++)
                    {
                        string monthName = CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(month).ToUpper();
                        string monthShortForm = CultureInfo.CurrentCulture.DateTimeFormat.GetAbbreviatedMonthName(month);


                        string formattedMonthYear = $"{monthShortForm}-{year}";
                        formattedMonthYearList.Add(formattedMonthYear);
                    }
                }
            }
            return formattedMonthYearList;
        }

        internal static object GetString(Func<string> toString)
        {
            throw new NotImplementedException();
        }


        #endregion

    }
}
