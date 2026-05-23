using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Migrations.Operations;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Model;

namespace Asset_Tracking
{
    internal class AssetTracking
    {
        private static AssetsHandler assetsHandler { get; set; }

        static void Main(string[] args)
        {
            if (OperatingSystem.IsWindows())
            {
                Console.SetWindowSize(150, 50);
            }

            assetsHandler = new AssetsHandler();

            ShowMenu();
        }


        private static void ShowMenu()
        {
            ColoredText.WriteLine("\n Pick an option:", ConsoleColor.Yellow);

            Console.Write("\n (");
            ColoredText.Write("1", ConsoleColor.Yellow);
            Console.Write(") Show all assets");

            Console.Write("\n (");
            ColoredText.Write("2", ConsoleColor.Yellow);
            Console.Write(") Show Computer assets");

            Console.Write("\n (");
            ColoredText.Write("3", ConsoleColor.Yellow);
            Console.Write(") Show Mobile assets");

            Console.Write("\n (");
            ColoredText.Write("4", ConsoleColor.Yellow);
            Console.Write(") Show Office equipment assets");

            Console.Write("\n (");
            ColoredText.Write("5", ConsoleColor.Yellow);
            Console.Write(") Show data for a specific office!");

            Console.Write("\n (");
            ColoredText.Write("6", ConsoleColor.Yellow);
            Console.Write(") Add a new asset.");

            Console.Write("\n (");
            ColoredText.Write("7", ConsoleColor.Yellow);
            Console.Write(") Edit an asset.");

            Console.Write("\n (");
            ColoredText.Write("8", ConsoleColor.Yellow);
            Console.Write(") Remove an asset");

            Console.Write("\n (");
            ColoredText.Write("9", ConsoleColor.Yellow);
            Console.Write(") Save assets to a JSON-file.");

            Console.Write("\n (");
            ColoredText.Write("10", ConsoleColor.Yellow);
            Console.Write(") Quit.");

            Navigate();
        }

        private static void Navigate()
        {
            Console.Write("\n ");
            int min = 1;
            int max = 9;
            int answer = Validation.GetValidatedIntFromConsole("Row number", min, max, false);

            switch (answer)
            {
                case 1:
                    assetsHandler.ShowAssets("   All Assets  ", assetsHandler.AssetsList, false, true);
                    break;
                case 2:
                    assetsHandler.ShowAssets("Computer Assets", assetsHandler.ComputerAssetsList.Cast<Asset>().ToList(), false, true);
                    break;
                case 3:
                    assetsHandler.ShowAssets(" Mobile Assets ", assetsHandler.MobileAssetsList.Cast<Asset>().ToList(), false, true);
                    break;
                case 4:
                    assetsHandler.ShowAssets(" Office Assets ", assetsHandler.OfficeAssetsList.Cast<Asset>().ToList(), false, true);
                    break;
                case 5:
                    assetsHandler.ShowDataForASpecificOffice();
                    break;
                case 6:
                    assetsHandler.AddNewAsset();
                    break;
                case 7:
                    assetsHandler.EditAsset();
                    break;
                case 8:
                    assetsHandler.RemoveAsset();
                    break;

                case 9:
                    assetsHandler.SaveAssetsToJsonFile();
                    break;
                case 10:
                    System.Environment.Exit(0);
                    break;
            }

            ShowMenu();
        }

    }
}
