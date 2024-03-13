using ExportPortal.API.Models.Domain;

namespace ExportPortal.API.Models.DTO
{
    public class QIAssignmentResponseDTO
    {
        public Guid Id { get; set; }
        public string CustomerId { get; set; }
        public string CustomerName { get; set; }
        public string VendorId { get; set; }
        public string VendorName { get; set; }
        public QuotationItemDTO Item { get; set; }

    }
}
