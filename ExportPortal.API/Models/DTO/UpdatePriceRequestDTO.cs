namespace ExportPortal.API.Models.DTO
{
    public class UpdatePriceRequest
    {
        public string VendorId { get; set; }
        public decimal Price { get; set; }
    }
}
