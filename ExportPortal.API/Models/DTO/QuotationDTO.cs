namespace ExportPortal.API.Models.DTO
{
    public class QuotationDTO
    {
        public string CustomerId { get; set; }

        public Guid ProductId { get; set; }

        public int Quantity { get; set; }

    }
}
