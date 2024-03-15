namespace ExportPortal.API.Models.DTO
{
    public class QuotationItemVendorDTO
    {
        public Guid Id { get; set; }

        public Guid ProductId { get; set; }

        public String ProductName { get; set; }

        public int Quantity { get; set; }

        public decimal? Price { get; set; }

    }
}
