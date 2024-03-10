
using ExportPortal.API.Models.Domain;

namespace ExportPortal.API.Models.DTO
{
    public class VendorResponseDTO
    {
        public String Id { get; set; }
        public string Name { get; set; }
        public string OrganizationName { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public string State { get; set; }
        public string City { get; set; }
        public string Address { get; set; }
        public int Zipcode { get; set; }
        public bool IsVerified { get; set; }

        // Navigation properties
        public VendorCategory VendorCategory { get; set; }
    }
}
