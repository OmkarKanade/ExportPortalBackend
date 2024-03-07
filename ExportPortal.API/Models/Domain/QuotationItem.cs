using System.Text.Json.Serialization;

namespace ExportPortal.API.Models.Domain
{
    public class QuotationItem
    {
        public Guid Id { get; set; }
        public Guid QuatationId { get; set; }
        [JsonIgnore]
        public Quotation Quotation { get; set; }
        public Guid ProductId { get; set; }
        public Product Product { get; set; }
        public int Quantity { get; set; }

    }
}
