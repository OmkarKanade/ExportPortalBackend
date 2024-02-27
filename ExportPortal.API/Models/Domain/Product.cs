
namespace ExportPortal.API.Models.Domain
{
    public class Product
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public string ScientificName { get; set; }

        public Guid VendorCategoryId { get; set; }

        public string? VendorId1 { get; set; }
        public string? VendorId2 { get; set; }
        public string? VendorId3 { get; set; }

        public string HSNCode { get; set; }

        public decimal ToPuneFreight { get; set; }
        public decimal InnerPackageMaterial { get; set; }
        public decimal OuterPackageMaterial { get; set; }
        public decimal ManualPackage { get; set; }
        public decimal MachinePackage { get; set; }
        public decimal LocalTransport { get; set; }
        public decimal Fumigation { get; set; }
        public decimal TotalRate { get; set; }
        public int GrossWeight { get; set; }
        public string PouchType { get; set; }
        public int BumperisPouches { get; set; }
        public string BagOrBox { get; set; }
        public int BagOrBoxBumpers { get; set; }
        public string Ingredients { get; set; }
        public string ManufacturingProcess { get; set; }
        public bool DairyDeclarationRequired { get; set; }
        public bool IsForHumanConsumption { get; set; }
        public Guid? CertificationId { get; set; }


        // Navigation properties

        public VendorCategory VendorCategory { get; set; }

        public Certification Certification { get; set; }

        public  UserProfile UserProfile1 { get; set; }
        public UserProfile UserProfile2 { get; set; }
        public UserProfile UserProfile3 { get; set; }
    }
}
