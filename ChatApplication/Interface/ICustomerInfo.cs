using ChatApplication.Entity;

namespace ChatApplication.Interface
{
    public interface ICustomerInfo
    {
     Task<CustomerInfo> InsertCustomerInfo(CustomerInfo obj);


    }
}
