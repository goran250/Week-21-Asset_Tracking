using Microsoft.EntityFrameworkCore;
using System;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Diagnostics.Metrics;
using System.Net.Sockets;
using System.Reflection;
using System.Text.Json;
using System.Xml.Linq;

namespace Asset_Tracking
{
	public class AssetsHandler
	{
        public List<Asset> AssetsList { get; set; }
        public List<ComputerAsset> ComputerAssetsList { get; set; }
        public List<MobileAsset> MobileAssetsList { get; set; }
        public List<OfficeAsset> OfficeAssetsList { get; set; }

        public List<Office> OfficesList { get; set; }

        private DBService dbService { get; set; } 

        public AssetsHandler()
		{
            dbService = new DBService();
            OfficesList = dbService.Offices.ToList();
            AssetsList = dbService.GetAllAssetsFromDB(true);
            ComputerAssetsList= dbService.GetComputerAssetsFromDB(true);
            MobileAssetsList = dbService.GetMobileAssetsFromDB(true);
            OfficeAssetsList = dbService.GetOfficeAssetsFromDB(true);
           

            // Anropa denna metod för fylla en tom databas med 5 offices och 8 assets.
            // AddOfficesAndAssets();
        }


        public void ShowAssets(string mainAssetType, List<Asset> assetsList, bool showLineNumbers, bool showExtraInformtion)
        {
            int assetTypeLength = GetLongestTextLengthForAssetType(assetsList) + 3;
            int brandLength = GetLongestTextLengthForBrand(assetsList) + 3;
            int modelNameLength = GetLongestTextLengthForModelName(assetsList) + 3;
            int priceLength = 12 + 3;
            int purchaseDateLength = 12 + 3;
            int employeeLength = GetLongestTextLengthForEmployee(assetsList) + 3;
            int locationLength = 9 + 3;

            string firstString = " ";
            string lineNumber = " "; // The lineNumber is empty in the header row of the assets list.
            
            if (showLineNumbers)
                firstString = lineNumber.PadRight(5);

            Console.WriteLine("\n----------------------------------------------------------- " + mainAssetType + " ------------------------------------------------------------\n");
            ColoredText.Write(firstString + "Asset type".PadRight(assetTypeLength) + "Brand".PadRight(brandLength) + "Model".PadRight(modelNameLength), ConsoleColor.Green);
            ColoredText.Write("Local price".PadRight(priceLength) + "Price in USD".PadRight(priceLength) + "Purchase Date".PadRight(purchaseDateLength), ConsoleColor.Green);
            ColoredText.WriteLine("Employee".PadRight(employeeLength) + "Location".PadRight(locationLength) + "\n", ConsoleColor.Green);

            firstString = " ";
           
            // Loop for showing the assetslist.
            // We start the loop from 1 because we want to show line numbers starting from 1.
            for (int i = 1; i <= assetsList.Count; i++)
            {
                Asset asset = assetsList[i - 1];
                if (showLineNumbers)
                {
                    lineNumber = " " + i + ". ";
                    firstString = lineNumber.PadRight(5);
                }

                Console.Write(firstString + asset.assetType.PadRight(assetTypeLength) + asset.brand.PadRight(brandLength));
                Console.Write(asset.modelName.PadRight(modelNameLength));
                Console.Write(asset.localPrice.ToString("F0").PadRight(priceLength));
                Console.Write(asset.purchasePriceUSD.ToString("F0").PadRight(priceLength));
                
                if (showExtraInformtion) 
                    ColoredText.Write(asset.purchaseDate.ToShortDateString().PadRight(purchaseDateLength), GetConsoleColor(asset.purchaseDate));
                else 
                    Console.Write(asset.purchaseDate.ToShortDateString().PadRight(purchaseDateLength));

                if (asset.employee != null)                
                    Console.Write(asset.employee.PadRight(employeeLength));
                else
                    Console.Write(" ".PadRight(employeeLength));
                
                Console.WriteLine(OfficesList.First(o => o.id == asset.officeID).city.PadRight(locationLength));                                     
            }

            if (showExtraInformtion)
            {
                decimal sumPurchasePriceUSD = CalculateSums(assetsList, "purchasePriceUSD");
                ColoredText.WriteLine("\n Total asset value in USD: " + sumPurchasePriceUSD.ToString("F0"), ConsoleColor.Green);

                ColoredText.WriteLine(" Total nbr of assets: " + assetsList.Count, ConsoleColor.Green);

                ColoredText.WriteLine(" Most expensive assets: " + GetMostExpensiveAsset(assetsList), ConsoleColor.Green);

            }

            Console.WriteLine("\n----------------------------------------------------------------------------------------------------------------------------------------");
        }

        public void ShowDataForASpecificOffice()
        {
            ColoredText.WriteLine("\n\n\n Enter the row number for the city you want to show data for.\n", ConsoleColor.Yellow);

            Console.WriteLine(" 1. " + OfficesList[0].city);
            Console.WriteLine(" 2. " + OfficesList[1].city);
            Console.WriteLine(" 3. " + OfficesList[2].city);
            Console.WriteLine(" 4. " + OfficesList[3].city);
            Console.WriteLine(" 5. " + OfficesList[4].city);

            int rowNumberForLocation = Validation.GetValidatedIntFromConsole("location", 1, 5, false);
            Office office = OfficesList[rowNumberForLocation - 1];
            List<Asset> assetsListForOffice = AssetsList.Where(a => a.officeID == office.id).ToList();

            int assetTypeLength = GetLongestTextLengthForAssetType(assetsListForOffice) + 3;
            int brandLength = GetLongestTextLengthForBrand(assetsListForOffice) + 3;
            int modelNameLength = GetLongestTextLengthForModelName(assetsListForOffice) + 3;
            int priceLength = 12 + 3;
            int purchaseDateLength = 12 + 3;

            Console.WriteLine("\n ------------------------------------------------- Data for the office in " + office.city + " -----------------------------------------------\n");
            ColoredText.Write(" Asset type".PadRight(assetTypeLength) + " Brand".PadRight(brandLength) + " Model".PadRight(modelNameLength), ConsoleColor.Green);
            ColoredText.WriteLine(" Price in USD".PadRight(priceLength) + " Local price".PadRight(priceLength) + " Purchase Date".PadRight(purchaseDateLength) + "\n", ConsoleColor.Green);


            // Loop for showing the assetsListForOffice list.
            foreach (Asset asset in assetsListForOffice)
            {
                Console.Write(" " + asset.assetType.PadRight(assetTypeLength) + asset.brand.PadRight(brandLength));
                Console.Write(asset.modelName.PadRight(modelNameLength));
                Console.Write(asset.purchasePriceUSD.ToString("F0", System.Globalization.CultureInfo.InvariantCulture).PadRight(priceLength));
                Console.Write(asset.localPrice.ToString("F0", System.Globalization.CultureInfo.InvariantCulture).PadRight(priceLength));
                ColoredText.WriteLine(asset.purchaseDate.ToShortDateString().PadRight(purchaseDateLength), GetConsoleColor(asset.purchaseDate));
            }

            decimal totalAssetValueUSD = CalculateSums(assetsListForOffice, "purchasePriceUSD");
            decimal totalAssetValueLocal = CalculateSums(assetsListForOffice, "localPrice");
            int nbrOfAssets = assetsListForOffice.Count;

            ColoredText.WriteLine("\n Total asset value in USD: " + totalAssetValueUSD.ToString("F0", System.Globalization.CultureInfo.InvariantCulture) + " USD", 
                                  ConsoleColor.Green);
            ColoredText.WriteLine(" Total asset value in local currency: " + totalAssetValueLocal.ToString("F0", 
                                  System.Globalization.CultureInfo.InvariantCulture) + " " + office.currencyCode, ConsoleColor.Green);
            ColoredText.WriteLine(" Most expensive assets: " + GetMostExpensiveAsset(assetsListForOffice), ConsoleColor.Green);

            Console.WriteLine("\n--------------------------------------------------------------------------------------------------------------------------------------\n\n");
        }

        public void AddNewAsset()
        {
            Asset newAsset;

            ColoredText.WriteLine("\n\n\n Enter a new asset. Follow the steps.", ConsoleColor.Yellow);

            string assetType = GetAssetType();
            if (assetType == "Desktop computer" || assetType == "Laptop")
            {
                newAsset = new ComputerAsset();
            }
            else if (assetType == "Mobile phone" || assetType == "Tablet")
            {
                newAsset = new MobileAsset();
            }
            else
            {
                newAsset = new OfficeAsset();
            }

            newAsset.assetType = assetType;

            newAsset.brand = Validation.GetValidatedStringFromConsole("Brand name");
            newAsset.modelName = Validation.GetValidatedStringFromConsole("Model name");
            newAsset.purchaseDate = (DateTime)Validation.GetValidatedDateFromConsole("Purchase date", false);
            ColoredText.WriteLine("\n Enter purchase price in USD: ", ConsoleColor.Yellow);
            newAsset.purchasePriceUSD = decimal.Parse(Validation.GetValidatedIntFromConsole("Purchase price", 1, 10000000, false).ToString());                       
            newAsset.officeID = GetOfficeID(false);
            newAsset.warrantyExpirationDate = (DateTime)Validation.GetValidatedDateFromConsole("Warranty expiration date", false);
            newAsset.serialNumber = Validation.GetValidatedStringFromConsole("Serial number");
            ColoredText.Write("\n Enter the employees name (optional): ", ConsoleColor.Yellow);
            newAsset.employee = Console.ReadLine();
            newAsset.localPrice = ConvertToLocalCurrency(newAsset.purchasePriceUSD, OfficesList.First(o => o.id == newAsset.officeID).currencyCode);

            if (newAsset.assetType == "Desktop computer" || newAsset.assetType == "Laptop") {
                newAsset = dbService.InsertNewComputerAsset((ComputerAsset)newAsset);
                ComputerAssetsList = dbService.GetComputerAssetsFromDB(true);
            } 
            else if (newAsset.assetType == "Mobile phone" || newAsset.assetType == "Tablet") {;
                newAsset = dbService.InsertNewMobileAsset((MobileAsset)newAsset);
                MobileAssetsList = dbService.GetMobileAssetsFromDB(true);
            }
            else if (newAsset.assetType == "Office equipment")
            {
                newAsset = dbService.InsertNewOfficeAsset((OfficeAsset)newAsset);
                OfficeAssetsList = dbService.GetOfficeAssetsFromDB(true);
            }

            ColoredText.WriteLine("\n A new asset has been added.\n\n", ConsoleColor.Green);

            AssetsList = dbService.GetAllAssetsFromDB(true);
        }

        public void EditAsset()
        {
            ColoredText.WriteLine("\n Enter the row number for the asset you want to edit", ConsoleColor.Yellow);
            ShowAssets("   All Assets   ", AssetsList, true, false);
            int min = 1;
            int max = AssetsList.Count;
            int index = Validation.GetValidatedIntFromConsole("Row number", min, max, false);

            Asset assetToEdit = AssetsList[index - 1];
            
            ColoredText.WriteLine("\n Enter a new values. Just press enter if you do not want to change the value.", ConsoleColor.Yellow);

            string oldAssetType = assetToEdit.assetType;
            string assetType = GetAssetTypeUpdate(oldAssetType, true);
            if (assetType != null) { 
                assetToEdit.assetType = assetType;
            }

            ColoredText.Write("\n Enter a new brand: ", ConsoleColor.Yellow);
            string brand = Console.ReadLine();
            if (String.IsNullOrEmpty(brand) == false)
                assetToEdit.brand = brand;

            ColoredText.Write("\n Enter a new model name: ", ConsoleColor.Yellow);
            string modelName = Console.ReadLine();
            if (String.IsNullOrEmpty(modelName) == false)
                assetToEdit.modelName = modelName;

            // GetValidatedDateFromConsole() returns null if the user just pressed enter.
            DateTime? purchaseDate = Validation.GetValidatedDateFromConsole("Purchase date", true); 
            if (purchaseDate != null)
                assetToEdit.purchaseDate = (DateTime)purchaseDate;

            ColoredText.WriteLine("\n Enter purchase price in USD: ", ConsoleColor.Yellow);
            decimal purchasePriceUSD = decimal.Parse(Validation.GetValidatedIntFromConsole("Purchase price", 1, 10000000, true).ToString());
            if (purchasePriceUSD > -1)
                assetToEdit.purchasePriceUSD = purchasePriceUSD;

            int officeID = GetOfficeID(true);

            if (officeID > -1) {
                assetToEdit.officeID = officeID;
            }

          
            // GetValidatedDateFromConsole() returns null if the user just pressed enter.
            DateTime? warrantyExpirationDate = Validation.GetValidatedDateFromConsole("Warranty expiration date", true);
            if (warrantyExpirationDate != null)
                assetToEdit.warrantyExpirationDate = (DateTime)warrantyExpirationDate;

            ColoredText.Write("\n Enter a new serial number: ", ConsoleColor.Yellow);
            string serialNumber = Console.ReadLine();
            if (String.IsNullOrEmpty(serialNumber) == false)
                assetToEdit.serialNumber = serialNumber;

            ColoredText.Write("\n Enter a new employee name: ", ConsoleColor.Yellow);
            string employee = Console.ReadLine();
            if (String.IsNullOrEmpty(employee) == false) { 
                assetToEdit.employee = employee;
            }

            assetToEdit.localPrice = ConvertToLocalCurrency(assetToEdit.purchasePriceUSD, OfficesList.First(o => o.id == assetToEdit.officeID).currencyCode);

            dbService.UpdateAsset(assetToEdit);          

            ColoredText.WriteLine("\n The asset has been edited.\n\n", ConsoleColor.Green);

            AssetsList = dbService.GetAllAssetsFromDB(true);
            ComputerAssetsList = dbService.GetComputerAssetsFromDB(true);
            MobileAssetsList = dbService.GetMobileAssetsFromDB(true);
            OfficeAssetsList = dbService.GetOfficeAssetsFromDB(true);
        }

        
            
        public void RemoveAsset()
        {
            ColoredText.WriteLine("\n\n\n Enter the row number for the asset you want to remove", ConsoleColor.Yellow);
            ShowAssets("   All Assets   ", AssetsList, true, false);
            
            int min = 1;
            int max = AssetsList.Count;
            int rowNumber = Validation.GetValidatedIntFromConsole("Row number", min, max, false);

            Asset assetToRemove = AssetsList[rowNumber - 1];
            string modelName = assetToRemove.modelName;
            string brand = assetToRemove.brand;
                       
            dbService.RemoveAssetFromDB(assetToRemove);
            
            ComputerAssetsList = dbService.GetComputerAssetsFromDB(true);
            MobileAssetsList = dbService.GetMobileAssetsFromDB(true);
            OfficeAssetsList = dbService.GetOfficeAssetsFromDB(true);

            AssetsList.RemoveAt(rowNumber - 1);

            ColoredText.WriteLine("\n The asset with rownumber " + rowNumber + " and brand and model name " + brand + " " + modelName + " has been removed.\n\n", ConsoleColor.Green);
        }

        public void SaveAssetsToJsonFile()
        {
            ColoredText.WriteLine("\n\n\n Select what you want to save.\n", ConsoleColor.Yellow);

            Console.WriteLine(" 1. All assets");
            Console.WriteLine(" 2. Computer assets");
            Console.WriteLine(" 3. Mobile assets");
            Console.WriteLine(" 4. Office equipment assets");
            
            int rowNumber = Validation.GetValidatedIntFromConsole("Row number", 1, 5, false);

            List<Asset> assetsList = new List<Asset>();
            string filename = "";
            string assetsType = "";

            if (rowNumber == 1 )
            {
                assetsList = AssetsList;
                filename = "assets.json";
            }
            else if (rowNumber == 2)
            {
                assetsList = ComputerAssetsList.Cast<Asset>().ToList();
                filename = "computerAssets.json";
                assetsType = "Computer";
            }
            else if (rowNumber == 3)
            {
                assetsList = MobileAssetsList.Cast<Asset>().ToList();
                filename = "mobileAssets.json";
                assetsType = "Mobile";
            }
            else if (rowNumber == 4)
            {
                assetsList = OfficeAssetsList.Cast<Asset>().ToList();
                filename = "officeAssets.json";
            }

            // Filen sparas i hämtade filer mappen. Det ska fungera på både windows och mac.
            string filePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), "Downloads");

            filePath = Path.Combine(filePath, filename);

            try
            {
                string json = JsonSerializer.Serialize(assetsList);
                File.WriteAllText(filePath, json);
            }
            catch (Exception)
            {
                ColoredText.WriteLine("Failed to save assets!", ConsoleColor.Red);
            }

            ColoredText.WriteLine("\n All " + assetsType + " assets have been saved to a json-file called " + filename + ".\n The file is in the Downloaded files-map on your computer.\n", ConsoleColor.Green);
        }

        private string GetAssetType()
        {
            ColoredText.WriteLine("\n Enter the row number of the asset type. The type of the asset you want to add.\n", ConsoleColor.Yellow);
            Console.WriteLine(" 1. Desktop computer");
            Console.WriteLine(" 2. Laptop computer");
            Console.WriteLine(" 3. Mobile phone");
            Console.WriteLine(" 4. Tablet");
            Console.WriteLine(" 5. Office equipment");
            int rowNumberForAssetType = Validation.GetValidatedIntFromConsole("Row number", 1, 5, false);
       
            string assetType = "";
            if (rowNumberForAssetType == 1)
                assetType = "Desktop computer";
            else if (rowNumberForAssetType == 2)
                assetType = "Laptop";   
            else if (rowNumberForAssetType == 3)
                assetType = "Mobile phone";
            else if (rowNumberForAssetType == 4)
                assetType = "Tablet";
            else if (rowNumberForAssetType == 5)
                assetType = "Office equipment";

            return assetType;
        }

        private string GetAssetTypeUpdate(string oldAssetType, bool allowNullOrEmpty)
        {
            ColoredText.WriteLine("\n Enter the row number of the asset type.\n", ConsoleColor.Yellow);

            if (oldAssetType == "Desktop computer" || oldAssetType == "Laptop") {
                Console.WriteLine(" 1. Desktop computer");
                Console.WriteLine(" 2. Laptop computer");
            }
            else if (oldAssetType == "Mobile phone" || oldAssetType == "Tablet")
            {
                Console.WriteLine(" 1. Mobile phone");
                Console.WriteLine(" 2. Tablet");
            }
            else if (oldAssetType == "Office equipment")
            {
                Console.WriteLine(" 1. Office equipment");
            }
            
            int rowNumberForAssetType = Validation.GetValidatedIntFromConsole("Row number", 1, 5, allowNullOrEmpty);

            if (allowNullOrEmpty && rowNumberForAssetType == -1) { // GetValidatedIntFromConsole() returns -1 if the user just pressed enter.
                return null;
            }

            string assetType = "";

            if ((oldAssetType == "Desktop computer" || oldAssetType == "Laptop") && rowNumberForAssetType == 1)
                assetType = "Desktop computer";
            else if ((oldAssetType == "Desktop computer" || oldAssetType == "Laptop") && rowNumberForAssetType == 2)
                assetType = "Laptop";
            else if ((oldAssetType == "Mobile phone" || oldAssetType == "Tablet") && rowNumberForAssetType == 1)
                assetType = "Mobile phone";
            else if ((oldAssetType == "Mobile phone" || oldAssetType == "Tablet") && rowNumberForAssetType == 2)
                assetType = "Tablet";
            else if (oldAssetType == "Office equipment" && rowNumberForAssetType == 1)
                assetType = "Office equipment";

            return assetType;
        }

        private int GetOfficeID(bool allowNullOrEmpty)
        {
            ColoredText.WriteLine("\n Enter the row number of your office location. The city there your office is located.\n", ConsoleColor.Yellow);
            Console.WriteLine(" 1. " + OfficesList[0].city);
            Console.WriteLine(" 2. " + OfficesList[1].city);
            Console.WriteLine(" 3. " + OfficesList[2].city);
            Console.WriteLine(" 4. " + OfficesList[3].city);
            Console.WriteLine(" 5. " + OfficesList[4].city);
           
            int rowNumberForLocation = Validation.GetValidatedIntFromConsole("location", 1, 5, allowNullOrEmpty);

            if (allowNullOrEmpty && rowNumberForLocation == -1) {  // GetValidatedIntFromConsole() returns -1 if the user just pressed enter.
                return -1;
            }

            return OfficesList[rowNumberForLocation - 1].id;
        }

        private decimal ConvertToLocalCurrency(decimal priceUSD, string currencyCode)
        {
            decimal localPrice = 0;

            if (currencyCode == "SEK")
            {
                localPrice = priceUSD * (decimal)9.3772;
            }
            else if (currencyCode == "EUR")
            {
                localPrice = priceUSD * (decimal)0.8621;
            }
            else // if currencyCode == USD 
            {
                localPrice = priceUSD ;
            }

            return localPrice;
        }

        public decimal CalculateSums(List<Asset> assetsList, string variableToSum)
        {
            decimal sum = 0;
                                  
            if (variableToSum == "purchasePriceUSD")
            {
                foreach (Asset asset in assetsList)
                {
                    sum = sum + asset.purchasePriceUSD;
                }
            }
            else if (variableToSum == "localPrice")
            {
                foreach (Asset asset in assetsList)
                {
                    sum += asset.localPrice;
                }
            }

            return sum;
        }
        private ConsoleColor GetConsoleColor(DateTime date)
        {
            if (DateTime.Now - date > TimeSpan.FromDays(1005)) // 1005 is approximately 2 years and 9 months)                                                                                     // we show the purchase date in red color, otherwise in white color.
            {
                return ConsoleColor.Red;
            }
            else if (DateTime.Now - date > TimeSpan.FromDays(912)) // 912 is approximately 2 years and 6 months)      
            {
                return ConsoleColor.Yellow;
            }
            else
            {
                return ConsoleColor.Gray;
            }
        }

        private string GetMostExpensiveAsset(List<Asset> assetsList)
        {
            decimal highestPrice = 0;
            string mostExpensiveAsset = "";

            foreach (Asset asset in assetsList)
            {
                if (asset.purchasePriceUSD > highestPrice)
                {
                    highestPrice = asset.purchasePriceUSD;
                    mostExpensiveAsset = asset.brand + " " + asset.modelName;
                }
            }

            return mostExpensiveAsset + " " + highestPrice.ToString("F0") + " USD";
        }


        // GetLongestTextLengthForBrand() finds the longest asset.brand.
        private int GetLongestTextLengthForBrand(List<Asset> assetsList)
        {
            int textLength = "Brand".Length; // 

            foreach (Asset asset in assetsList)  
            {                
                if (asset.brand.Length > textLength)
                {
                    textLength = asset.brand.Length;
                }   
            }

            return textLength;
        }

        // GetLongestTextLengthForModelName() finds the longest asset.modelName.
        private int GetLongestTextLengthForModelName(List<Asset> assetsList)
        {
            int textLength = "Model".Length; // 

            foreach (Asset asset in assetsList)
            {
                if (asset.modelName.Length > textLength)
                {
                    textLength = asset.modelName.Length;
                }
            }

            return textLength;
        }

        // GetLongestTextLengthForEmployee() finds the longest asset.employee.
        private int GetLongestTextLengthForEmployee(List<Asset> assetsList)
        {
            int textLength = "Employee".Length;

            foreach (Asset asset in assetsList)
            {
                if (asset.employee.Length > textLength)
                {
                    textLength = asset.employee.Length;
                }
            }

            return textLength;
        }

        // GetLongestTextLengthForAssetType() finds the longest asset.assetType.
        private int GetLongestTextLengthForAssetType(List<Asset> assetsList)
        {
            int textLength = "Asset Type".Length; // 

            foreach (Asset asset in assetsList)
            {
                if (asset.assetType.Length > textLength)
                {
                    textLength = asset.assetType.Length;
                }
            }

            return textLength;
        }


        
        // Denna metoden används för att lägga till fem offices och sex assets i en tom databas.
        public void AddOfficesAndAssets()
        {
            OfficesList = new List<Office>();

            OfficesList.Add(new Office("USA", "Los Angeles", "USD"));
            OfficesList.Add(new Office("USA", "New York", "USD"));
            OfficesList.Add(new Office("Sweden", "Stockholm", "SEK"));
            OfficesList.Add(new Office("Germany", "Hamburg", "EUR"));
            OfficesList.Add(new Office("France", "Paris", "EUR"));

            dbService.InsertNewOffices(OfficesList);

            // Hämtar listan från db för att få med de id:n som har skapats i databasen.
            OfficesList = dbService.GetOfficesFromDB();

            ComputerAssetsList = new List<ComputerAsset>();
            MobileAssetsList = new List<MobileAsset>();
            OfficeAssetsList = new List<OfficeAsset>();

            ComputerAsset computerAsset = new ComputerAsset();

            computerAsset.assetType = "Laptop";
            computerAsset.brand = "Dell";
            computerAsset.modelName = "XPS 15";
            computerAsset.purchaseDate = new DateTime(2022, 1, 15);
            computerAsset.warrantyExpirationDate = new DateTime(2024, 1, 15);
            computerAsset.purchasePriceUSD = 1120;
            computerAsset.officeID = OfficesList[2].id; // Stockholm
            computerAsset.serialNumber = "SN123456789";
            computerAsset.employee = "Göran Rosenberg";
            computerAsset.localPrice = ConvertToLocalCurrency(computerAsset.purchasePriceUSD, OfficesList[2].currencyCode);

            ComputerAssetsList.Add(computerAsset);
            

            computerAsset = new ComputerAsset();
            computerAsset.assetType = "Laptop";
            computerAsset.brand = "HP";
            computerAsset.modelName = "Pavilion";
            computerAsset.purchaseDate = new DateTime(2022, 1, 15);
            computerAsset.warrantyExpirationDate = new DateTime(2024, 1, 15);
            computerAsset.purchasePriceUSD = 1180;
            computerAsset.officeID = OfficesList[2].id; // Stockholm
            computerAsset.serialNumber = "SNGR12345678";
            computerAsset.employee = "Hans Persson";
            computerAsset.localPrice = ConvertToLocalCurrency(computerAsset.purchasePriceUSD, OfficesList[2].currencyCode);

            ComputerAssetsList.Add(computerAsset);

            computerAsset = new ComputerAsset();
            computerAsset.assetType = "Desktop computer";
            computerAsset.brand = "HP";
            computerAsset.modelName = "XE12";
            computerAsset.purchaseDate = new DateTime(2022, 1, 15);
            computerAsset.warrantyExpirationDate = new DateTime(2024, 1, 15);
            computerAsset.purchasePriceUSD = 1050;
            computerAsset.officeID = OfficesList[3].id; // Hamburg
            computerAsset.serialNumber = "MNSN HGTD 1243 5789";
            computerAsset.employee = "Herman Müller";
            computerAsset.localPrice = ConvertToLocalCurrency(computerAsset.purchasePriceUSD, OfficesList[3].currencyCode);

            ComputerAssetsList.Add(computerAsset);

            MobileAsset mobileAsset = new MobileAsset();
            mobileAsset.assetType = "Mobile phone";
            mobileAsset.brand = "Apple";
            mobileAsset.modelName = "Iphone 16";
            mobileAsset.purchaseDate = new DateTime(2023, 1, 15);
            mobileAsset.warrantyExpirationDate = new DateTime(2024, 1, 15);
            mobileAsset.purchasePriceUSD = 1250;
            mobileAsset.officeID = OfficesList[0].id; // Los angeles
            mobileAsset.serialNumber = "RTSN 1234 5678";
            mobileAsset.employee = "John Smith";
            mobileAsset.localPrice = ConvertToLocalCurrency(mobileAsset.purchasePriceUSD, OfficesList[0].currencyCode);

            MobileAssetsList.Add(mobileAsset);

            mobileAsset = new MobileAsset();
            mobileAsset.assetType = "Mobile phone";
            mobileAsset.brand = "Apple";
            mobileAsset.modelName = "Iphone SE";
            mobileAsset.purchaseDate = new DateTime(2023, 10, 15);
            mobileAsset.warrantyExpirationDate = new DateTime(2024,10, 15);
            mobileAsset.purchasePriceUSD = 580;
            mobileAsset.officeID = OfficesList[4].id; // Paris
            mobileAsset.serialNumber = "SGNM 1234 5678";
            mobileAsset.employee = "Xavier D'Aboville";
            mobileAsset.localPrice = ConvertToLocalCurrency(mobileAsset.purchasePriceUSD, OfficesList[4].currencyCode);

            MobileAssetsList.Add(mobileAsset);


            mobileAsset = new MobileAsset();
            mobileAsset.assetType = "Mobile phone";
            mobileAsset.brand = "Apple";
            mobileAsset.modelName = "Iphone 12";
            mobileAsset.purchaseDate = new DateTime(2023, 10, 15);
            mobileAsset.warrantyExpirationDate = new DateTime(2024, 10, 15);
            mobileAsset.purchasePriceUSD = 900;
            mobileAsset.officeID = OfficesList[3].id; // Hamburg
            mobileAsset.serialNumber = "DRSN 1234 5678";
            mobileAsset.employee = "Dietrich Schmidt";
            mobileAsset.localPrice = ConvertToLocalCurrency(mobileAsset.purchasePriceUSD, OfficesList[3].currencyCode);

            MobileAssetsList.Add(mobileAsset);



            OfficeAsset officeAsset = new OfficeAsset();
            officeAsset.assetType = "Office equipment";
            officeAsset.brand = "HP";
            officeAsset.modelName = "Color LaserJet Pro MFP M177fw";
            officeAsset.purchaseDate = new DateTime(2025, 1, 15);
            officeAsset.warrantyExpirationDate = new DateTime(2026, 1, 15);
            officeAsset.purchasePriceUSD = 319;
            officeAsset.officeID = OfficesList[2].id; // Stockholm
            officeAsset.serialNumber = "SN123456789";
            officeAsset.employee = "Göran Rosenberg";
            officeAsset.localPrice = ConvertToLocalCurrency(officeAsset.purchasePriceUSD, OfficesList[2].currencyCode);

            OfficeAssetsList.Add(officeAsset);


            officeAsset = new OfficeAsset();
            officeAsset.assetType = "Office equipment";
            officeAsset.brand = "HP";
            officeAsset.modelName = "Color LaserJet Pro MFP M177fw";
            officeAsset.purchaseDate = new DateTime(2024, 10, 15);
            officeAsset.warrantyExpirationDate = new DateTime(2026, 1, 15);
            officeAsset.purchasePriceUSD = 350;
            officeAsset.officeID = OfficesList[1].id; // New York
            officeAsset.serialNumber = "DCFF 1234 5678";
            officeAsset.employee = "Adam Hamilton";
            officeAsset.localPrice = ConvertToLocalCurrency(officeAsset.purchasePriceUSD, OfficesList[1].currencyCode);

            OfficeAssetsList.Add(officeAsset);

            dbService.InsertNewComputerAssetsList(ComputerAssetsList);
            dbService.InsertNewMobileAssetsList(MobileAssetsList);
            dbService.InsertNewOfficeAssetsList(OfficeAssetsList);

            AssetsList = dbService.GetAllAssetsFromDB(true);
            
        }
    }
}
