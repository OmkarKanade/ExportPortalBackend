namespace ExportPortal.API.Models.Domain
{
    public class Quotation
    {
        public Guid Id { get; set; }
        public string CustomerId { get; set; }
        public UserProfile Customer { get; set; }
        public bool Status { get; set; }
        //public DateTime SendDate { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime LastModifiedOn { get; set; }
        public ICollection <QuotationItem> Items { get; set; }
    }
}
