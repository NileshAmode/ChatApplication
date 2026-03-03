namespace ChatApplication.Entity
{
    public class LoginEntity
    {
            public string? CustomerID { get; set; }

            public string? UserID { get; set; }
            public string? FullName { get; set; }
            public string? MobileNo { get; set; }
            public string? EmailID { get; set; }
            public string? OTP { get; set; }
            public string? LocationID { get; set; }
            public string? LocationName { get; set; }
            public int? ErrorCode { get; set; }
            public string LoginID { get; set; }
            public string LoginPassword { get; set; }
            public string? LoginName { get; set; }
            public string? IsApproved { get; set; }
            public string? IsMembership { get; set; }
            public string? MembershipName { get; set; }
            public string? CompanyName { get; set; }
            public string? PlanType { get; set; }
            public string? MembershipExpired { get; set; }
            public int ERROR_CODE { get; set; } = 0;
        }
    }
