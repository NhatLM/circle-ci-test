using System;
using System.Collections.Generic;

namespace IdentityService.API.Model
{
    public partial class CustUser
    {
        public int Id { get; set; }
        public int? CustId { get; set; }
        public string UserType { get; set; }
        public int Active { get; set; }
        public string Name { get; set; }
        public string Title { get; set; }
        public string Email { get; set; }
        public int? PhoneCountryCode { get; set; }
        public string Phone { get; set; }
        public int? DirectPhoneCountryCode { get; set; }
        public string DirectPhone { get; set; }
        public string Locale { get; set; }
        public string AdminLocale { get; set; }
        public string Hash { get; set; }
        public string Password { get; set; }
        public int? UserLevel { get; set; }
        public int? AllowCreateUsers { get; set; }
        public int? AllowBidding { get; set; }
        public int? AllowSelling { get; set; }
        public int? AllowGuarenteedBid { get; set; }
        public int? AllowBuyerBlock { get; set; }
        public int? AssessmentUser { get; set; }
        public string StayOnHash { get; set; }
        public string ListViewMode { get; set; }
        public DateTime? LastDailyAuction { get; set; }
        public DateTime? InvoiceFree { get; set; }
        public DateTime? TermsApproval { get; set; }
        public string CurrencyCode { get; set; }
        public DateTime? NewsRead { get; set; }
        public int ConfirmFavoriteRemoval { get; set; }
        public int? BlockUsage { get; set; }
        public string RememberToken { get; set; }
        public string InvoiceContactName { get; set; }
        public string ErpIdNumber { get; set; }
        public DateTimeOffset? CreatedAt { get; set; }
        public string GroupEmail { get; set; }
        public DateTimeOffset? UpdatedAt { get; set; }
        public int? CustTaskSubstitute { get; set; }
        public DateTimeOffset? EmailVerifiedAt { get; set; }
        public long? EmailplatformId { get; set; }
    }
}
