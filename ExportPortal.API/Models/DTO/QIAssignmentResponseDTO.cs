namespace ExportPortal.API.Models.DTO
{
    public class QIAssignmentResponseDTO
    {
        public Guid QuotationId { get; set; }
        public List<QuotationItemVendorDTO> Items { get; set; }

    }
}
