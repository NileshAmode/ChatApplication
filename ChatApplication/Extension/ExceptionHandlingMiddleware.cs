using ChatApplication.Controller;
using ChatApplication.Handler;
using Microsoft.Data.SqlClient;
using Newtonsoft.Json;
using System.Data;
using System.Net;

namespace ChatApplication.Extension
{
    public class ExceptionHandlingMiddleware
    {
        public RequestDelegate _next;
        private readonly ILogger<BaseController> _logger;
        private readonly CommonUtility _commonUtility;
        public ExceptionHandlingMiddleware(RequestDelegate next, ILogger<BaseController> logger, CommonUtility commonUtility)
        {
            _next = next;
            logger = _logger;
            _commonUtility = commonUtility;
        }


        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next.Invoke(context);
            }
            catch (Exception ex)
            {
                //_logger.LogError(ex, "Path :" + context.Request.Path + "-Error Message :" + ex.Message);
                //await HandleException(context, ex);

                string key = SettingsConfigHelper.ErroPagekey();

                if (key == "true")
                {
                    context.Response.Redirect("/Error");
                }
                else
                {

                    _logger.LogError(ex, "Path :" + context.Request.Path + "-Error Message :" + ex.Message);
                    await HandleException(context, ex);
                }

                string strPageName = CommonUtility.RemoveFirstChar(context.Request.Path);


                InsertErrorInfo(ex, strPageName);
                // developer mode --- show exception on browser page
                // production mode --- redirect to other page i.e error
            }
        }

        public void InsertErrorInfo(Exception Ex, string strPageName)
        {
            SqlCommand cmd = new SqlCommand();
            int i;

            string ErrorMessage = Ex.Message;
            string StackTrace = Ex.StackTrace.ToString();

            cmd.Parameters.Add("@PageName", SqlDbType.VarChar, 2000).Value = CommonUtility.GetString(strPageName);
            cmd.Parameters.Add("@ErrorMessage", SqlDbType.VarChar, 8000).Value = CommonUtility.GetString(ErrorMessage);
            cmd.Parameters.Add("@StackTrace", SqlDbType.VarChar, 8000).Value = CommonUtility.GetString(StackTrace);

            i = DBUtility.ExecuteSP("InsertErrorInfo", cmd);

        }

        private async Task HandleException(HttpContext context, Exception ex)
        {
            var response = context.Response;
            var CustomException = ex as BaseCustomException;
            var statusCode = (int)HttpStatusCode.InternalServerError;
            var ErrorCode = "Ranka Construction";
            var description = $"Response Code :{statusCode}-{ex.Message}";

            if (null != CustomException)
            {
                ErrorCode = CustomException.ErrorCode;
                description = CustomException.Message;
                statusCode = CustomException.code;
            }

            response.ContentType = "application/json";
            response.StatusCode = statusCode;
            await response.WriteAsync(JsonConvert.SerializeObject(new CustomErrorResponse
            {
                ErroCode = ErrorCode,
                Description = description,
            }));
        }
    }
}
