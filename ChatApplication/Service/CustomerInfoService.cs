using ChatApplication.Entity;
using ChatApplication.Handler;
using ChatApplication.Interface;
using Microsoft.Data.SqlClient;
using System.Data;

namespace ChatApplication.Service
{
    public class CustomerInfoService : ICustomerInfo
    {
        private readonly CommonUtility _commonUtility;
        private readonly ILogger<CustomerInfoService> _logger;


        public CustomerInfoService(CommonUtility commonUtility, ILogger<CustomerInfoService> logger)
        {
            _commonUtility = commonUtility;
            _logger = logger;

        }
        public async Task<int> InsertCustomerInfo(CustomerInfo obj)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.Parameters.Add("@CustomerID", SqlDbType.Int).Value = CommonUtility.GetInt32(obj.CustomerID.ToString());
            cmd.Parameters.Add("@FirstName", SqlDbType.NVarChar, 250).Value = CommonUtility.GetString(obj.FirstName);
            cmd.Parameters.Add("@MiddleName", SqlDbType.NVarChar, 250).Value = CommonUtility.GetString(obj.MiddleName);
            cmd.Parameters.Add("@LastName", SqlDbType.NVarChar, 250).Value = CommonUtility.GetString(obj.LastName);
            cmd.Parameters.Add("@Email", SqlDbType.NVarChar, 250).Value = CommonUtility.GetString(obj.Email);
            cmd.Parameters.Add("@PasswordHash", SqlDbType.NVarChar, 250).Value = CommonUtility.GetString(obj.Password);
            cmd.Parameters.Add("@MobileNo", SqlDbType.VarChar, 100).Value = CommonUtility.GetString(obj.MobileNo);
            cmd.Parameters.Add("@Gender", SqlDbType.NVarChar, 250).Value = CommonUtility.GetString(obj.Gender);
            cmd.Parameters.Add("@Address", SqlDbType.NVarChar, 250).Value = CommonUtility.GetString(obj.Address);
            cmd.Parameters.Add("@City", SqlDbType.NVarChar, 250).Value = CommonUtility.GetString(obj.City);
            cmd.Parameters.Add("@StateTxt", SqlDbType.NVarChar, 250).Value = CommonUtility.GetString(obj.State);
            cmd.Parameters.Add("@Country", SqlDbType.NVarChar, 250).Value = CommonUtility.GetString(obj.Country);
            cmd.Parameters.Add("@Pincode", SqlDbType.NVarChar, 50).Value = CommonUtility.GetString(obj.Pincode);
            cmd.Parameters.Add("@CustomerID_OUT", SqlDbType.Int).Direction = ParameterDirection.Output;
            cmd.Parameters.Add("@Error_Code", SqlDbType.Int).Direction = ParameterDirection.Output;

            int result  = DBUtility.ExecuteSP("InsertCustomerInfo", cmd);

            obj.Error_Code = CommonUtility.GetString(cmd.Parameters["@Error_Code"].Value.ToString());
            obj.CustomerID = CommonUtility.GetInt32(cmd.Parameters["@CustomerID_OUT"].Value.ToString());
            if (obj.CustomerID > 0)
            {
                result = obj.CustomerID;
            }
            else
            {
                result = Convert.ToInt32(obj.Error_Code);
            }
                return result;
        }
    }
}
