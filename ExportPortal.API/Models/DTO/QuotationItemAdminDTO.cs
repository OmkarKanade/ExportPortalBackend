namespace ExportPortal.API.Models.DTO
{
    public class QuotationItemAdminDTO
    {
        public Guid Id { get; set; }
        public Guid ProductId { get; set; }
        public String ProductName { get; set; }
        public String? Vendor1 { get; set; }
        public String? Vendor2 { get; set; }
        public String? Vendor3 { get; set; }
        public decimal? Vendor1Price { get; set; }
        public decimal? Vendor2Price { get; set; }
        public decimal? Vendor3Price { get; set; }
        public int Quantity { get; set; }

    }
}
