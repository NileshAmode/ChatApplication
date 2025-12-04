using System.Net;

namespace ChatApplication.Extension
{
    public class BaseCustomException : Exception
    {
        private int _code;
        private string _errorcode;

        public int code
        {
            get => _code;

        }

        public string ErrorCode
        {
            get => _errorcode;
        }

        public BaseCustomException(string message, string description, int Code) : base(description)
        {
            _code = code;
            _errorcode = ErrorCode;
        }



    }
    public class CustomErrorResponse
    {
        public string ErroCode { get; set; }
        public string Description { get; set; }

    }

    public class NotFoundCustomException : BaseCustomException
    {
        public NotFoundCustomException(string message, string description) : base(message, description, (int)HttpStatusCode.NotFound)
        {

        }
    }
}