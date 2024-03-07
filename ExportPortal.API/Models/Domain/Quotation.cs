namespace ExportPortal.API.Models.Domain
{
    public class Quotation
    {
        public Guid Id { get; set; }

        public Guid CustomerId { get; set; }

        public UserProfile Customer { get; set; }

        public ICollection <QuotationItem> Items { get; set; }
    }
}
