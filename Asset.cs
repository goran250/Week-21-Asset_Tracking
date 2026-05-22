using Microsoft.EntityFrameworkCore.Metadata.Conventions;
using Microsoft.Extensions.Options;
using System;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.Contracts;


namespace Asset_Tracking
{
    public class Asset
	{
        public int id { get; set; }
		[MaxLength(50)]
		public string assetType { get; set; } // Fyra typer: Desktop computer, Laptop, Mobile phone, Tablet och Office equipment.
        [MaxLength(50)]
        public string brand { get; set; }
        [MaxLength(50)]
        public string modelName { get; set; }
        public DateTime purchaseDate { get; set; }
		public decimal purchasePriceUSD { get; set; }
		public decimal localPrice { get; set; }        
        public int officeID { get; set; }  // Fem kontor: Los Angeles, New York, Stockholm, Hamburg, Paris   (huvudkontoret i Los Angeles)
        [MaxLength(50)]
        public string serialNumber { get; set; }
        [MaxLength(50)]
        public string? employee {  get; set; }
		public DateTime warrantyExpirationDate { get; set; }

        public Asset()
        {
        }

        public Asset(string assetType, string brand, string modelName, DateTime purchaseDate, Decimal purchasePriceUSD, Decimal localPrice, 
			         int officeID, string serialNumber, string employee, DateTime warrantyExpirationDate)
		{
			this.assetType = assetType;
			this.brand = brand;
			this.modelName = modelName;
			this.purchaseDate = purchaseDate;
			this.purchasePriceUSD = purchasePriceUSD;
			this.localPrice = localPrice;
			this.officeID = officeID;
			this.serialNumber = serialNumber;
			this.employee = employee;
			this.warrantyExpirationDate = warrantyExpirationDate;
        }
	}
}