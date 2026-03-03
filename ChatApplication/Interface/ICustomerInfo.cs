using ChatApplication.Entity;

namespace ChatApplication.Interface
{
    public interface ICustomerInfo
    {
     Task<int> InsertCustomerInfo(CustomerInfo obj);


    }
}
