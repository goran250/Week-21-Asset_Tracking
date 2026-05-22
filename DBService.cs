using Microsoft.EntityFrameworkCore;
using System;
using System.Net.Sockets;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace Asset_Tracking
{
	public class DBService : DbContext
    {
		private string connectionString = "Server = (localdb)\\mssqllocaldb;Database=Asset_Tracking;Trusted_Connection=True;";

        public DbSet<ComputerAsset> ComputerAssets { get; set; }
        public DbSet<MobileAsset> MobileAssets { get; set; }
        public DbSet<OfficeAsset> OfficeAssets { get; set; }
        public DbSet<Office> Offices { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            // We tell the app to use the connectionstring.
            optionsBuilder.UseSqlServer(connectionString);
        }


        protected override void OnModelCreating(ModelBuilder ModelBuilder)
        {

        }


        public List<Asset> GetAllAssetsFromDB(bool sorted)
        {
            /** Fick inte denna koden att funka. Istället kombinerar jag Offices och assetslist i AssetHandler.cs.
            List<ComputerAsset> computerAssetsList = ComputerAssets.Join(Offices, c => c.officeID, o => o.id, (c, o) => new { ComputerAssets = c, Offices = o }).Select(x => x.ComputerAssets).ToList();
            List<MobileAsset> mobileAssetsList = MobileAssets.Join(Offices, c => c.officeID, o => o.id, (c, o) => new { MobileAssets = c, Offices = o }).Select(x => x.MobileAssets).ToList();
            List<OfficeAsset> officeAssetsList = OfficeAssets.Join(Offices, c => c.officeID, o => o.id, (c, o) => new { OfficeAssets = c, Offices = o }).Select(x => x.OfficeAssets).ToList();            
            */

            List<ComputerAsset> computerAssetsList = ComputerAssets.ToList();
            List<MobileAsset> mobileAssetsList = MobileAssets.ToList();
            List<OfficeAsset> officeAssetsList = OfficeAssets.ToList();
            
            SaveChanges();

            List<Asset> assetsList = new List<Asset>();
            assetsList.AddRange(computerAssetsList);
            assetsList.AddRange(mobileAssetsList);
            assetsList.AddRange(officeAssetsList);

            List<Asset> newAssetsList = assetsList;
            
            if (sorted)
            {
                newAssetsList = assetsList.OrderBy(a => a.assetType).ThenBy(a => a.purchaseDate).ToList();
            }

            return newAssetsList;
        }

        public List<ComputerAsset> GetComputerAssetsFromDB(bool sorted)
        {
            List<ComputerAsset> computerAssetsList = ComputerAssets.ToList();

            SaveChanges();

            List<ComputerAsset> newComputerAssetsList = computerAssetsList;

            if (sorted)
            {
                newComputerAssetsList = computerAssetsList.OrderBy(a => a.assetType).ThenBy(a => a.purchaseDate).ToList();
            }

            return newComputerAssetsList;
        }

        public List<MobileAsset> GetMobileAssetsFromDB(bool sorted)
        {
            List<MobileAsset> mobileAssetsList = MobileAssets.ToList();

            SaveChanges();

            List<MobileAsset> newMobileAssetsList = mobileAssetsList;

            if (sorted)
            {
                newMobileAssetsList = mobileAssetsList.OrderBy(a => a.assetType).ThenBy(a => a.purchaseDate).ToList();
            }

            return newMobileAssetsList;
        }


        public List<OfficeAsset> GetOfficeAssetsFromDB(bool sorted)
        {
            List<OfficeAsset> officeAssetsList = OfficeAssets.ToList();

            SaveChanges();

            List<OfficeAsset> newOfficeAssetsList = officeAssetsList;

            if (sorted)
            {
                newOfficeAssetsList = officeAssetsList.OrderBy(a => a.assetType).ThenBy(a => a.purchaseDate).ToList();
            }

            return officeAssetsList;
        }

        public List<Office> GetOfficesFromDB()
        {
            List<Office> officesList = Offices.ToList();

            SaveChanges();

            return officesList;
        }

        public void InsertNewOffices(List<Office> offices)
        {
            Offices.AddRange(offices);

            SaveChanges();
        }

        public void InsertNewComputerAssetsList(List<ComputerAsset> assets)
        {
            ComputerAssets.AddRange(assets);

            SaveChanges();
        }

        public void InsertNewMobileAssetsList(List<MobileAsset> assets)
        {
            MobileAssets.AddRange(assets);

            SaveChanges();
        }

        public void InsertNewOfficeAssetsList(List<OfficeAsset> assets)
        {
            OfficeAssets.AddRange(assets);

            SaveChanges();
        }
        public ComputerAsset InsertNewComputerAsset(ComputerAsset asset)
        {
            ComputerAssets.Add(asset);

            SaveChanges();

            return ComputerAssets.OrderByDescending(c => c.id).FirstOrDefault();
        }

        public MobileAsset InsertNewMobileAsset(MobileAsset asset)
        {
            MobileAssets.Add(asset);

            SaveChanges();

            return MobileAssets.OrderByDescending(m => m.id).FirstOrDefault();
        }

        public OfficeAsset InsertNewOfficeAsset(OfficeAsset asset)
        {
            OfficeAssets.Add(asset);

            SaveChanges();

            return OfficeAssets.OrderByDescending(o => o.id).FirstOrDefault();
        }

        public void UpdateAsset(Asset asset)
        {
            if (asset.assetType == "Desktop computer" || asset.assetType == "Laptop")
            {
                ComputerAssets.Update((ComputerAsset)asset);
            }
            else if (asset.assetType == "Mobile phone" || asset.assetType == "Tablet")
            {
                MobileAssets.Update((MobileAsset)asset);
            }
            else if (asset.assetType == "Office equipment")
            {
                OfficeAssets.Update((OfficeAsset)asset);
            }

            SaveChanges();
        }

       

        public void RemoveAssetFromDB(Asset asset)
        {
            if (asset.assetType == "Desktop computer" || asset.assetType == "Laptop")
            {
                ComputerAssets.Remove((ComputerAsset)asset);
            }
            else if (asset.assetType == "Mobile phone" || asset.assetType == "Tablet")
            {
                MobileAssets.Remove((MobileAsset)asset);
            }
            else if (asset.assetType == "Office equipment")
            {
                OfficeAssets.Remove((OfficeAsset)asset);
            }

            SaveChanges();
        }

    }
}