namespace ExportPortal.API.Models.Domain
{
    public class QuotationItemAssignment
    {
        public Guid Id { get; set; }
        public Guid QuotationId { get; set; }
        public Quotation Quotation { get; set; }
        public Guid ItemId { get; set; }
        public QuotationItem Item { get; set; }
        public string VendorId { get; set; }
        public UserProfile Vendor { get; set; }
    }
}
