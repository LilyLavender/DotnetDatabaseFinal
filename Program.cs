﻿using NLog;
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
                            Case3(db, logger);
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
                            Environment.Exit(1);
                            break;
                    }
                }
            }
            catch (Exception ex)
            {
                logger.Error(ex.Message);
            }

            logger.Info("Program ended");
        }

        public static void Case1(TradersContext db, Logger logger) {
            Console.WriteLine("Add records to Products table");
        }

        public static void Case2(TradersContext db, Logger logger) {
            Console.WriteLine("Edit records from Products table");
        }

        public static void Case3(TradersContext db, Logger logger) {
            Console.WriteLine("Display all records in the Products table (ProductName only)");
            Console.WriteLine("User decides if they want to see all products, discontinued products, or active products");
            Console.WriteLine("Discontinued products should be distinguished from active products");
        }

        public static void Case4(TradersContext db, Logger logger) {
            Console.WriteLine("Display a specific Product (all product fields should be displayed)");
        }

        public static void Case5(TradersContext db, Logger logger) {
            Console.WriteLine("Delete a specified existing record from the Products table (account for Orphans in related tables)");
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
            Console.WriteLine("Display all Categories in the Categories table (CategoryName and Description)");
        }

        public static void Case9(TradersContext db, Logger logger) {
            Console.WriteLine("Display all Categories and their related active (not discontinued) product data (CategoryName, ProductName)");
        }

        public static void CaseA(TradersContext db, Logger logger) {
            Console.WriteLine("Display a specific Category and its related active product data (CategoryName, ProductName)");
        }

        public static void CaseB(TradersContext db, Logger logger) {
            Console.WriteLine("Delete a specified existing record from the Categories table (account for Orphans in related tables)");
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
    }

}