using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nop.Services.ExportImport
{
    public class ImportCustomerModel
    {
        public string Username { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public int VendorId { get; set; }
        public string Gender { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Company { get; set; }
        public string StreetAddress { get; set; }
        public string StreetAddress2 { get; set; }
        public string ZipPostalCode { get; set; }
        public string City { get; set; }
        public string County { get; set; }
        public int CountryId { get; set; }
        public int StateProvinceId { get; set; }
        public string Phone { get; set; }
        public string Fax { get; set; }
        public bool IsRegistered { get; set; }
        public bool IsTaxExempt { get; set; }
        public bool Active { get; set; }
        public int AffiliateId { get; set; }
        public string TimeZoneId { get; set; }
        public string VatNumber { get; set; }
        public string VatNumberStatusId { get; set; }
        public bool IsGuest { get; set; }
        public bool IsAdministrator { get; set; }
        public bool IsForumModerator { get; set; }
        public bool NewsletterInStore { get; set; }


    }
}
