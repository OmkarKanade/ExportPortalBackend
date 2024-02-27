
namespace ExportPortal.API.Models.DTO
{
    public class UserUpdateDTO
    {
        public string Name { get; set; }
        public string OrganizationName { get; set; }
        public string PhoneNumber { get; set; }
        public string State { get; set; }
        public string City { get; set; }
        public string Address { get; set; }
        public int Zipcode { get; set; }
        public string NewPassword { get; set; } = string.Empty;
        public string CurrentPassword { get; set; } = string.Empty;
    }
}
