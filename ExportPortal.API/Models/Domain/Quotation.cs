namespace ExportPortal.API.Models.Domain
{
    public class Quotation
    {
        public Guid Id { get; set; }

        public string CustomerId { get; set; }

        public UserProfile Customer { get; set; }

        public bool Status { get; set; }



        public ICollection <QuotationItem> Items { get; set; }
    }
}
