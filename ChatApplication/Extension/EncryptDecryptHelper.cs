using Microsoft.AspNetCore.DataProtection;

namespace ChatApplication.Extension
{
    public class EncryptDecryptHelper
    {
        IDataProtector _protector { get; set; }
        public EncryptDecryptHelper()
        {
            var dataProtectionProvider = DataProtectionProvider.Create("WebQuery");
            _protector = dataProtectionProvider.CreateProtector("WebQuery.QueryStrings");
        }

        public string Encrypt(string input)
        {
            return _protector.Protect(input);
        }

        public string Decrypt(string encryptedInput)
        {
            return _protector.Unprotect(encryptedInput);
        }
    }
}