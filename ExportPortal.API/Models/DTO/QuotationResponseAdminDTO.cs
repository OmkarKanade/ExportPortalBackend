namespace ExportPortal.API.Models.DTO
{
    public class QuotationResponseAdminDTO
    {
        public Guid Id { get; set; }
        public string CustomerId { get; set; }
        public string CustomerName { get; set; }
        public bool Status { get; set; }
        public ICollection<QuotationItemAdminDTO> Items { get; set; }

    }
}
