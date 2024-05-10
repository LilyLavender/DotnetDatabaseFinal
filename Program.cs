using NLog;
using System.Globalization;
using System.Linq;

namespace BlogsAndPosts
{
    class Program 
    {

        static void Main(string[] args) 
        {
            string path = Directory.GetCurrentDirectory() + "\\nlog.config";

            // Logger
            var logger = LogManager.LoadConfiguration(path).GetCurrentClassLogger();
            logger.Info("Program started");

            // Main functionality
            try {
                while (true) {
                    // Create instance of dbcontext
                    var db = new TradersContext();

                    // Print options
                    Console.WriteLine("\nSelect Option:");
                    Console.WriteLine("  1. Add product");
                    Console.WriteLine("  2. Edit product");
                    Console.WriteLine("  3. Display all products");
                    Console.WriteLine("  4. Display specific product");
                    Console.WriteLine("  5. Delete product");
                    Console.WriteLine();
                    Console.WriteLine("  6. Add category");
                    Console.WriteLine("  7. Edit category");
                    Console.WriteLine("  8. Display all categories");
                    Console.WriteLine("  9. Display all categories & products");
                    Console.WriteLine("  A. Display all products in category");
                    Console.WriteLine("  B. Delete a category");
                    Console.WriteLine();
                    Console.WriteLine("  0. Exit");

                    // Get option from user
                    char[] validOptions = new char[] { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9', 'a', 'b', 'A', 'B' };
                    char option = GetChar(true, validOptions, "", "Invalid option");
                    // Run code based on option acquired from user
                    switch(option) {
                        case '1':
                            Case1(db, logger);
                            break;
                        case '2':
                            Case2(db, logger);
                            break;
                        case '3':
                            while (true) {
                                // Print options
                                Console.WriteLine("\nSelect Option:");
                                Console.WriteLine("  1. View all products");
                                Console.WriteLine("  2. View active products");
                                Console.WriteLine("  3. View discontinued products");
                                Console.WriteLine("  0. Exit");

                                // Get option from user
                                int option2 = GetInt(true, 0, 3, "", "Invalid Option");
                                // Run code based on option acquired from user
                                switch(option2) {
                                    case 1:
                                        Case3(db, logger, true, true);
                                        break;
                                    case 2:
                                        Case3(db, logger, true, false);
                                        break;
                                    case 3:
                                        Case3(db, logger, false, true);
                                        break;
                                    default:
                                        break;
                                }
                                break;
                            }
                            break;
                        case '4':
                            Case4(db, logger);
                            break;
                        case '5':
                            Case5(db, logger);
                            break;
                        case '6':
                            Case6(db, logger);
                            break;
                        case '7':
                            Case7(db, logger);
                            break;
                        case '8':
                            Case8(db, logger);
                            break;
                        case '9':
                            Case9(db, logger);
                            break;
                        case 'A':
                        case 'a':
                            CaseA(db, logger);
                            break;
                        case 'B':
                        case 'b':
                            CaseB(db, logger);
                            break;
                        default:
                            goto End;
                    }
                }
            }
            catch (Exception ex)
            {
                logger.Error(ex.Message);
            }

            End: // ⎛⎝(•ⱅ•)⎠⎞ 
            logger.Info("Program ended");
        }

        public static void Case1(TradersContext db, Logger logger) {
            // Add new product
            var productName = GetString("\nEnter a name for a new Product: ", "Product name cannot be blank.");
            var quantityPerUnit = GetString("Enter the quantity per unit\n(eg. \"24 - 12 oz bottles\"): ", "Quantity per unit cannot be blank.");
            var unitPrice = GetDouble(true, 0.0, Double.MaxValue, "Enter the unit price: ", "Unit price invalid.");
            var reorderLevel = GetInt(false, 0, 0, "Enter reorder level: ", "Invalid reorder level.");
            var discontinued = GetBool("Is the product discontinued?");
            int categoryID = GetCategory(db, "Select the product\'s category");
            var product = new Product { 
                ProductName = productName,
                SupplierID = 1,
                CategoryID = categoryID,
                QuantityPerUnit = quantityPerUnit,
                UnitPrice = (decimal)unitPrice,
                UnitsInStock = 0,
                UnitsOnOrder = 0,
                ReorderLevel = (short)reorderLevel,
                Discontinued = discontinued
            };

            db.AddProduct(product);
            logger.Info("Product added - {name}", productName);
        }

        public static void Case2(TradersContext db, Logger logger) {
            Console.WriteLine("Edit records from Products table");
        }

        public static void Case3(TradersContext db, Logger logger, bool showActiveProducts, bool showDiscontinuedProducts) {
            // Display all products
            if (showActiveProducts) {
                var activeProducts = db.Products.Where(p => p.Discontinued == false).ToList();
                Console.WriteLine("\nActive Products:");
                foreach (var ap in activeProducts) {
                    Console.WriteLine($"{ap.ProductID}. {ap.ProductName}");
                }
            }
            if (showDiscontinuedProducts) {
                var discontinuedProducts = db.Products.Where(p => p.Discontinued == true).ToList();
                Console.WriteLine("\nDiscontinued Products:");
                foreach (var dp in discontinuedProducts) {
                    Console.WriteLine($"{dp.ProductID}. {dp.ProductName}");
                }
            }
        }

        public static void Case4(TradersContext db, Logger logger) {
            // Display a specific product
            int userSelection = GetProduct(db, "Select the product to view");

            var product = db.Products.Where(p => p.ProductID == userSelection).ToList()[0];
            var catName = db.Categories.Where(c => c.CategoryID == product.CategoryID).ToList()[0].CategoryName;
            Console.WriteLine($"ProductID: {product.ProductID}");
            Console.WriteLine($"ProductName: {product.ProductName}");
            Console.WriteLine($"CategoryID: {product.CategoryID} ({catName})");
            Console.WriteLine($"QuantityPerUnit: {product.QuantityPerUnit}");
            Console.WriteLine($"UnitPrice: {product.UnitPrice.ToString("C", CultureInfo.CreateSpecificCulture("en-US"))}");
            Console.WriteLine($"UnitsInStock: {product.UnitsInStock}");
            Console.WriteLine($"UnitsOnOrder: {product.UnitsOnOrder}");
            Console.WriteLine($"ReorderLevel: {product.ReorderLevel}");
            Console.WriteLine($"Discontinued: {product.Discontinued}");
        }

        public static void Case5(TradersContext db, Logger logger) {
            // Delete product
            Product product = db.Products.Where(p => p.ProductID == GetProduct(db, "Select the product to delete")).ToList()[0];
            db.DeleteProduct(product);
            logger.Info("Product deleted - {name}", product.ProductName);
        }

        public static void Case6(TradersContext db, Logger logger) {
            // Add new category
            var categoryName = GetString("\nEnter a name for a new Category: ", "Category name cannot be blank.");
            var description = GetString("Enter the category description: ", "Category description cannot be blank.");
            var category = new Category { CategoryName = categoryName, Description = description };

            db.AddCategory(category);
            logger.Info("Category added - {name}", categoryName);
        }

        public static void Case7(TradersContext db, Logger logger) {
            Console.WriteLine("Edit a specified record from the Categories table");
        }

        public static void Case8(TradersContext db, Logger logger) {
            // Display all categories
            DisplayAllCategories(db);
        }

        public static void Case9(TradersContext db, Logger logger) {
            // Display all categories & related products
            var categories = db.Categories.ToList();
            foreach (var cat in categories) {
                Console.WriteLine($"{cat.CategoryID}. {cat.CategoryName}");
                var products = db.Products.Where(p => p.CategoryID == cat.CategoryID && p.Discontinued == false);
                if (products.Count() == 0) {
                    Console.WriteLine("       No active products in category");
                } else {
                    foreach (var pro in products) {
                        Console.WriteLine($"       {pro.ProductName}");
                    }
                }
            }
        }

        public static void CaseA(TradersContext db, Logger logger) {
            // Display specific category & active products
            int userSelection = GetCategory(db, "Select the category to view");

            var category = db.Categories.Where(c => c.CategoryID == userSelection).ToList()[0];
            Console.WriteLine($"{category.CategoryName}");
            var products = db.Products.Where(p => p.CategoryID == category.CategoryID && p.Discontinued == false);
            if (products.Count() == 0) {
                Console.WriteLine("    No active products in category");
            } else {
                foreach (var pro in products) {
                    Console.WriteLine($"    {pro.ProductName}");
                }
            }
        }

        public static void CaseB(TradersContext db, Logger logger) {
            // Delete category
            Category category = db.Categories.Where(c => c.CategoryID == GetCategory(db, "Select the category to delete")).ToList()[0];
            List<Product> products = db.Products.Where(p => p.CategoryID == category.CategoryID).ToList();
            foreach (var product in products) {
                db.DeleteProduct(product);
            }
            db.DeleteCategory(category);

            logger.Info("Category and its products deleted - {name}", category.CategoryName);
        }

        public static void DisplayAllCategories(TradersContext db) {
            var categories = db.Categories.ToList();
            foreach (var cat in categories) {
                Console.WriteLine($"{cat.CategoryID}. {cat.CategoryName} - {cat.Description}");
            }
        }

        public static int GetCategory(TradersContext db, string prompt) {
            Console.WriteLine(prompt);
            DisplayAllCategories(db);
            int minCatId = db.Categories.Min(b => b.CategoryID);
            int maxCatId = db.Categories.Max(b => b.CategoryID);
            int userInt;
            do {
                userInt = GetInt(true, minCatId, maxCatId, "", "Invalid category ID");
            } while (!db.Categories.Any(c => c.CategoryID == userInt));
            return userInt;
        }

        public static void DisplayAllProducts(TradersContext db) {
            var products = db.Products.ToList();
            foreach (var p in products) {
                Console.WriteLine($"{p.ProductID}. {p.ProductName}");
            }
        }

        public static int GetProduct(TradersContext db, string prompt) {
            Console.WriteLine(prompt);
            DisplayAllProducts(db);
            int minProductId = db.Products.Min(b => b.ProductID);
            int maxProductId = db.Products.Max(b => b.ProductID);
            int userInt;
            do {
                userInt = GetInt(true, minProductId, maxProductId, "", "Invalid product ID");
            } while (!db.Products.Any(p => p.ProductID == userInt));
            return userInt;
        }

        /// <summary>
        /// Gets an integer from the user
        /// </summary>
        /// <param name="restrictValues">Whether or not to restrict the value between intMin and intMax</param>
        /// <param name="intMin">The minimum value to accept</param>
        /// <param name="intMax">The maximum value to accept</param>
        /// <param name="prompt">Message that asks the user for an int</param>
        /// <param name="errorMsg">What to output in the case of an invalid int</param>
        /// <returns>int</returns>
        public static int GetInt(bool restrictValues, int intMin, int intMax, string prompt, string errorMsg) {

            string userString = "";
            int userInt = 0;
            bool repSuccess = false;
            do {
                Console.Write(prompt);
                userString = Console.ReadLine();

                if (Int32.TryParse(userString, out userInt)) {
                    if (restrictValues)
                    {
                        if (userInt >= intMin && userInt <= intMax) {
                            repSuccess = true;
                        }
                    }
                    else
                    {
                        repSuccess = true;
                    }
                }

                // Output error
                if (!repSuccess) {
                    Console.WriteLine(errorMsg);
                }
            } while(!repSuccess);

            return userInt;

        }

        /// <summary>
        /// Gets a string from the user
        /// </summary>
        /// <param name="prompt">Message that asks the user for a string</param>
        /// <param name="errorMsg">What to output in the case of an invalid string</param>
        /// <returns>string</returns>
        public static string GetString(string prompt, string errorMsg) {
            string userString = "";
            bool repSuccess = false;
            do
            {
                Console.Write(prompt);
                userString = Console.ReadLine();
                if (!String.IsNullOrEmpty(userString))
                {
                    repSuccess = true;
                }
                // Output error
                if (!repSuccess)
                {
                    Console.WriteLine(errorMsg);
                }
            } while (!repSuccess);

#pragma warning disable CS8603 // Possible null reference return.
            return userString;
#pragma warning restore CS8603 // Possible null reference return.
        }
        
        /// <summary>
        /// Gets a char from the user
        /// </summary>
        /// <param name="restrictValues">Whether or not to restrict the value to one in possibleAnswers[]</param>
        /// <param name="possibleAnswers">Array of chars to accept</param>
        /// <param name="prompt">Message that asks the user for a char</param>
        /// <param name="errorMsg">What to output in the case of an invalid char</param>
        /// <returns></returns>
        public static char GetChar(bool restrictValues, char[] possibleAnswers, string prompt, string errorMsg)
        {

            string userString = "";
            char userChar = '0';
            bool repSuccess = false;
            do
            {
                Console.Write(prompt);
                userString = Console.ReadLine();

                if (Char.TryParse(userString, out userChar))
                {
                    if (restrictValues)
                    {
                        foreach (char i in possibleAnswers)
                        if (userChar == i)
                        {
                            repSuccess = true;
                                break;
                        }
                    }
                    else
                    {
                        repSuccess = true;
                    }
                }

                // Output error
                if (!repSuccess)
                {
                    Console.WriteLine(errorMsg);
                }
            } while (!repSuccess);

            return userChar;

        }

        /// <summary>
        /// Gets a double from the user
        /// </summary>
        /// <param name="restrictValues">Whether or not to restrict the value between doubleMin and doubleMax</param>
        /// <param name="doubleMin">The minimum value to accept</param>
        /// <param name="doubleMax">The maximum value to accept</param>
        /// <param name="prompt">Message that asks the user for a double</param>
        /// <param name="errorMsg">What to output in the case of an invalid double</param>
        /// <returns></returns>
        public static double GetDouble(bool restrictValues, double doubleMin, double doubleMax, string prompt, string errorMsg) 
        {

            string userString = "";
            double userDouble = 0;
            bool repSuccess = false;
            do
            {
                Console.Write(prompt);
                userString = Console.ReadLine();

                if (Double.TryParse(userString, out userDouble))
                {
                    if (restrictValues) {
                        if (userDouble >= doubleMin && userDouble <= doubleMax)
                        {
                            repSuccess = true;
                        }
                    } else {
                        repSuccess = true;
                    }
                    
                }

                // Output error
                if (!repSuccess)
                {
                    Console.WriteLine(errorMsg);
                }
            } while (!repSuccess);

            return userDouble;

        }

        /// <summary>
        /// Gets a bool from the user
        /// </summary>
        /// <param name="prompt">Message that asks the user for a y/n value</param>
        /// <param name="errorMsg">What to output in the case of an invalid answer</param>
        /// <returns></returns>
        public static bool GetBool(string prompt) {
            char userChar = GetChar(true, new char[] { 'y', 'Y', 'n', 'N' }, prompt + " (Y/N): ", "Invalid input.");
            if (userChar == 'y' || userChar == 'Y') {
                return true;
            } else {
                return false;
            }
        }
    }

}