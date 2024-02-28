﻿using ExportPortal.API.Models.Domain;

namespace ExportPortal.API.Models.DTO
{
    public class ProductResponseDTO
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string ImgPath { get; set; }
        public string ScientificName { get; set; }

        public VendorCategory VendorCategory { get; set; }

        public string? VendorId1 { get; set; }
        public string? VendorName1 { get; set; }
        public string? VendorId2 { get; set; }
        public string? VendorName2 { get; set; }
        public string? VendorId3 { get; set; }
        public string? VendorName3 { get; set; }

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
        public Certification Certification { get; set; }

    }
}
