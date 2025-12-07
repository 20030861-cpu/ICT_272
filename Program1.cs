using System;
using System.Collections.Generic;
using System.Linq;

namespace TigerSoccerClub
{
    // Change 1: Proper Object-Oriented Design - Created dedicated Player class
    public class Player
    {
        public string Name { get; set; }
        public string RegistrationType { get; set; }
        public bool WantsJersey { get; set; }
        public double TotalCost { get; set; }

        public Player(string name, string registrationType, bool wantsJersey)
        {
            Name = name;
            RegistrationType = registrationType;
            WantsJersey = wantsJersey;
            TotalCost = 0; // Will be calculated separately to handle group discounts
        }
    }

    // Change 2: Eliminate Code Duplication - Created RegistrationManager for business logic
    public class RegistrationManager
    {
        private const int KIDS_BASE_COST = 150;
        private const int ADULT_BASE_COST = 230;
        private const int JERSEY_COST = 100;
        private const double GROUP_DISCOUNT_RATE = 0.05;

        public static double CalculatePlayerCost(string registrationType, bool wantsJersey, bool hasGroupDiscount)
        {
            double baseCost = registrationType.ToLower() == "kids" ? KIDS_BASE_COST : ADULT_BASE_COST;

            if (wantsJersey)
                baseCost += JERSEY_COST;

            if (hasGroupDiscount)
                baseCost *= (1 - GROUP_DISCOUNT_RATE);

            return baseCost;
        }

        public static void ApplyGroupDiscount(List<Player> players)
        {
            bool hasGroupDiscount = players.Count > 1;

            foreach (var player in players)
            {
                player.TotalCost = CalculatePlayerCost(player.RegistrationType, player.WantsJersey, hasGroupDiscount);
            }
        }
    }

    // Change 4: Robust Input Validation - Created InputValidator class
    public class InputValidator
    {
        public static int GetValidPlayerCount()
        {
            while (true)
            {
                Console.Write("Enter the number of players (1-4): ");
                string input = Console.ReadLine();

                if (int.TryParse(input, out int count) && count >= 1 && count <= 4)
                    return count;

                Console.WriteLine("Invalid input. Please enter a number between 1 and 4.");
            }
        }

        public static string GetValidName()
        {
            while (true)
            {
                Console.Write("Enter player name: ");
                string name = Console.ReadLine()?.Trim();

                if (!string.IsNullOrWhiteSpace(name) && name.Length >= 2)
                    return name;

                Console.WriteLine("Please enter a valid name (at least 2 characters).");
            }
        }

        public static string GetValidRegistrationType()
        {
            while (true)
            {
                Console.Write("Registration type (Kids/Adult): ");
                string input = Console.ReadLine()?.Trim();

                if (!string.IsNullOrEmpty(input))
                {
                    string lowerInput = input.ToLower();
                    if (lowerInput == "kids" || lowerInput == "adult")
                        return char.ToUpper(lowerInput[0]) + lowerInput.Substring(1);
                }

                Console.WriteLine("Please enter 'Kids' or 'Adult'.");
            }
        }

        public static bool GetJerseyChoice()
        {
            while (true)
            {
                Console.Write("Do you want a jersey? (yes/no): ");
                string input = Console.ReadLine()?.Trim().ToLower();

                if (input == "yes" || input == "y")
                    return true;
                if (input == "no" || input == "n")
                    return false;

                Console.WriteLine("Please enter 'yes' or 'no'.");
            }
        }
    }

    // Change 3: Fix Summary Display Logic - Created proper display methods
    public class DisplayManager
    {
        public static void DisplayWelcomeHeader()
        {
            string welcome = "*****Welcome to TigerSoccerClub*****";
            Console.SetCursorPosition((Console.WindowWidth - welcome.Length) / 2, Console.CursorTop);
            Console.WriteLine(welcome);
            Console.WriteLine();
        }

        public static void DisplayPlayerCost(Player player)
        {
            Console.WriteLine($"Total cost for {player.Name}: ${player.TotalCost:F2}");
            Console.WriteLine();
        }

        public static void DisplaySummary(List<Player> players)
        {
            Console.WriteLine("\n" + new string('*', 80));

            string header = "Summary of Registrations";
            int headerPosition = Math.Max(0, (80 - header.Length) / 2);
            Console.WriteLine(new string(' ', headerPosition) + header);

            Console.WriteLine(new string('*', 80));
            Console.WriteLine($"{"Name",-20} {"Type",-10} {"Jersey",-10} {"Total",-15}");
            Console.WriteLine(new string('-', 80));

            double grandTotal = 0;
            foreach (var player in players)
            {
                string jerseyStatus = player.WantsJersey ? "Yes" : "No";
                Console.WriteLine($"{player.Name,-20} {player.RegistrationType,-10} {jerseyStatus,-10} ${player.TotalCost:F2}");
                grandTotal += player.TotalCost;
            }

            Console.WriteLine(new string('-', 80));
            Console.WriteLine($"{"Grand Total:",-45} ${grandTotal:F2}");

            if (players.Count > 1)
            {
                Console.WriteLine($"{"Group Discount Applied:",-45} 5%");
            }
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                // Display welcome message
                DisplayManager.DisplayWelcomeHeader();

                // Get number of players with validation
                int playerCount = InputValidator.GetValidPlayerCount();
                List<Player> players = new List<Player>();

                // Collect player information
                for (int i = 0; i < playerCount; i++)
                {
                    Console.WriteLine($"\n--- Player {i + 1} Information ---");

                    string name = InputValidator.GetValidName();
                    string registrationType = InputValidator.GetValidRegistrationType();
                    bool wantsJersey = InputValidator.GetJerseyChoice();

                    Player player = new Player(name, registrationType, wantsJersey);
                    players.Add(player);
                }

                // Calculate costs (applies group discount if applicable)
                RegistrationManager.ApplyGroupDiscount(players);

                // Display individual costs
                Console.WriteLine("\n--- Individual Costs ---");
                foreach (var player in players)
                {
                    DisplayManager.DisplayPlayerCost(player);
                }

                // Display final summary
                DisplayManager.DisplaySummary(players);

                Console.WriteLine("\nThank you for registering with Tiger Soccer Club!");
                Console.WriteLine("Press any key to exit...");
                Console.ReadKey();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
                Console.WriteLine("Press any key to exit...");
                Console.ReadKey();
            }
        }
    }
}