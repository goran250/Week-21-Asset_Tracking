using Microsoft.EntityFrameworkCore.Metadata.Conventions;
using Microsoft.Extensions.Options;
using System;
using System.Diagnostics.Contracts;

namespace Asset_Tracking
{
    public class ComputerAsset : Asset
    {
        public ComputerAsset(): base(){            
        }

        public ComputerAsset(string assetType, string brand, string modelName, DateTime purchaseDate, Decimal purchasePriceUSD, Decimal localPrice, 
                             int officeID, string serialNumber, string? employee, DateTime warrantyExpirationDate) : 
                            base(assetType, brand, modelName, purchaseDate, purchasePriceUSD, localPrice, officeID, serialNumber, employee, 
                                  warrantyExpirationDate) {
        }
    }
}