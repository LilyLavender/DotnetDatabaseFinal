using NLog;

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

            Console.WriteLine("Hello World!");

            logger.Info("Program ended");
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

            string? userString = "";
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
            string? userString = "";
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

    }

}