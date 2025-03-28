using System.Globalization;

class Program
{
    static void Main(string[] args)
    {
        AssetList assetList = new AssetList();
        assetList.AddAsset();
        assetList.DisplayAssets();
    }

    // The design of the Asset Tracking List
    public static void DisplayHeader()
    {
        Console.BackgroundColor = ConsoleColor.White;
        Console.ForegroundColor = ConsoleColor.Blue;
        Console.WriteLine("{0,-20} {1,-20} {2,-20} {3,-20} {4,-20} {5,-20} {6,-20}", "Name", "Brand", "Model", "Office", "Price (USD)", "Price (Local)", "Purchase Date");
        Console.ResetColor();
    }
}

// Parent class for assets - Base Class
class Asset
{
    // Properties
    public string? Name { get; set; }
    public string? Brand { get; set; }
    public string? Model { get; set; }
    public string? Office { get; set; }
    public string? PriceUSD { get; set; }
    public string? PriceLocal { get; set; }
    public string? Purchasedate { get; set; }
}

// Derived class for Computer
class Computer : Asset
{
    public Computer(string nameType, string? brand, string? model, string? office, string? priceUSD, string? priceLocal, string? purchaseDate)
    {
        Name = nameType;
        Brand = brand;
        Model = model;
        Office = office;
        PriceUSD = priceUSD;
        PriceLocal = priceLocal;
        Purchasedate = purchaseDate;
    }
}

// Derived class for Smartphone
class Smartphone : Asset
{
    public Smartphone(string nameType, string? brand, string? model, string? office, string? priceUSD, string? priceLocal, DateTime purchaseDate)
    {
        Name = nameType;
        Brand = brand;
        Model = model;
        Office = office;
        PriceUSD = priceUSD;
        PriceLocal = priceLocal;
        Purchasedate = purchaseDate.ToString("yyyy-MM-dd");
    }
}

// List of the assets
class AssetList
{
    List<Asset> assets = new List<Asset>();

    // Dictionary to store currency conversion rates
    Dictionary<string, double> currencyRates = new Dictionary<string, double>
    {
        { "USA", 1.0 }, // USD
        { "Sweden", 10.0 }, // SEK
        { "Japan", 110.0 }, // JPY
        { "Germany", 0.85 }, // EURO
        { "France", 0.85 }, // EURO
        { "UK", 0.75 }, // GBP
        { "Italy", 0.85 }, // Euro
        { "Spain", 0.85 } // EURO
    };

    // Method to convert USD to local currency
    public string ConvertCurrency(string office, string priceUSD)
    {
        if (currencyRates.ContainsKey(office))
        {
            double rate = currencyRates[office];
            double priceInUSD = double.Parse(priceUSD);
            double priceInLocal = priceInUSD * rate;
            return priceInLocal.ToString("F2");
        }
        return priceUSD;
    }

    // Adds the product to the asset list
    public void AddAsset()
    {
        while (true)
        {
            Console.WriteLine("Enter name type (Computer or Smartphone) or type 'exit' to quit:");
            string nameType = Console.ReadLine();

            if (nameType.Contains("Computer", StringComparison.OrdinalIgnoreCase))
            {
                Console.WriteLine("Enter computer brand:");
                string brand = Console.ReadLine();

                Console.WriteLine("Enter computer model:");
                string model = Console.ReadLine();

                Console.WriteLine("Enter which country office is in:");
                string office = Console.ReadLine();

                Console.WriteLine("Enter computer cost in USD:");
                string priceUSD = Console.ReadLine();

                string priceLocal = ConvertCurrency(office, priceUSD);

                Console.WriteLine("Enter purchase date:");
                string purchaseDate = Console.ReadLine();

                Computer computer = new Computer(nameType, brand, model, office, priceUSD, priceLocal, purchaseDate);
                assets.Add(computer);
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("Computer added successfully!");
                Console.ResetColor();
            }
            else if (nameType.Contains("Smartphone", StringComparison.OrdinalIgnoreCase))
            {
                Console.WriteLine("Enter Smartphone brand:");
                string brand = Console.ReadLine();

                Console.WriteLine("Enter Smartphone model:");
                string model = Console.ReadLine();

                Console.WriteLine("Enter which country office is in:");
                string office = Console.ReadLine();

                Console.WriteLine("Enter Smartphone cost in USD:");
                string priceUSD = Console.ReadLine();

                string priceLocal = ConvertCurrency(office, priceUSD);

                Console.WriteLine("Enter purchase date (yyyy-MM-dd or dd-MM-yyyy):");
                string purchaseDateString = Console.ReadLine();
                DateTime purchaseDate;
                if (!DateTime.TryParseExact(purchaseDateString, new[] { "yyyy-MM-dd", "dd-MM-yyyy" }, CultureInfo.InvariantCulture, DateTimeStyles.None, out purchaseDate))
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Invalid date format");
                    Console.ResetColor();
                    continue;
                }

                Smartphone smartphone = new Smartphone(nameType, brand, model, office, priceUSD, priceLocal, purchaseDate);
                assets.Add(smartphone);
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("Smartphone added successfully!");
                Console.ResetColor();
            }
            else if (nameType.Equals("exit", StringComparison.OrdinalIgnoreCase))
            {
                break;
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Invalid name type");
                Console.ResetColor();
            }
        }
    }

    // Displays the list of assets
    public void DisplayAssets()
    {
        // Sort assets by class and purchase date
        assets = assets.OrderBy(a => a.GetType().Name).ThenBy(a => DateTime.ParseExact(a.Purchasedate, new[] { "yyyy-MM-dd", "dd-MM-yyyy" }, CultureInfo.InvariantCulture, DateTimeStyles.None)).ToList();

        Program.DisplayHeader();
        foreach (var asset in assets)
        {
            DateTime purchaseDate;
            if (!DateTime.TryParseExact(asset.Purchasedate, new[] { "yyyy-MM-dd", "dd-MM-yyyy" }, CultureInfo.InvariantCulture, DateTimeStyles.None, out purchaseDate))
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Invalid date format in asset list");
                Console.ResetColor();
                continue;
            }

            TimeSpan timeToEndOfLife = purchaseDate.AddYears(3) - DateTime.Now;
            if (timeToEndOfLife.TotalDays <= 90)
            {
                Console.ForegroundColor = ConsoleColor.Red;
            }
            else if (timeToEndOfLife.TotalDays <= 180)
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
            }
            else
            {
                Console.ResetColor();
            }

            Console.WriteLine("{0,-20} {1,-20} {2,-20} {3,-20} {4,-20} {5,-20} {6,-20}", asset.Name, asset.Brand, asset.Model, asset.Office, asset.PriceUSD, asset.PriceLocal, asset.Purchasedate);
        }
        Console.ResetColor();
    }
}
