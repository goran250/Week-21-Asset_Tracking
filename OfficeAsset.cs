using Microsoft.EntityFrameworkCore.Metadata.Conventions;
using Microsoft.Extensions.Options;
using System;
using System.Diagnostics.Contracts;

namespace Asset_Tracking
{
	public class OfficeAsset: Asset
	{
        public OfficeAsset() : base(){ }


        public OfficeAsset(string assetType, string brand, string modelName, DateTime purchaseDate, Decimal purchasePriceUSD, Decimal localPrice,
                           int officeID, string serialNumber, string? employee, DateTime warrantyExpirationDate) :
                          base(assetType, brand, modelName, purchaseDate, purchasePriceUSD, localPrice, officeID, serialNumber, employee,
                                  warrantyExpirationDate)
        {
            
			
        }
	}
}