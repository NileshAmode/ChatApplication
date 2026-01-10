using ChatApplication.Entity;
using ChatApplication.Handler;
using ChatApplication.Interface;
namespace ChatApplication.Service
{
    public class CustomerInfoService : ICustomerInfo
    {
        private readonly CommonUtility _commonUtility;
        //private readonly ILogger<CustomerInfoService> _logger;


        public CustomerInfoService(CommonUtility commonUtility/*, ILogger<CustomerInfoService> logger*/)
        {
            _commonUtility = commonUtility;
           // _logger = logger;
           
        }
        public async Task<CustomerInfo> InsertCustomerInfo(CustomerInfo obj)
        {

            return null;
        }
    }
}
