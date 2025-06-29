using System;
using System.Collections.Generic;
using System.Threading;

namespace ThreadingConsoleDemo
{
    /// <summary>
    /// Utility class for console UI operations
    /// </summary>
    public static class ConsoleHelper
    {
        /// <summary>
        /// Writes a section header with both English and Arabic titles
        /// </summary>
        public static void WriteHeader(string englishTitle, string arabicTitle)
        {
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("\n" + new string('=', 80));
            Console.WriteLine($"{englishTitle} | {arabicTitle}");
            Console.WriteLine(new string('=', 80));
            Console.ResetColor();
        }

        /// <summary>
        /// Writes a subheader for demo sections
        /// </summary>
        public static void WriteSubheader(string title)
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("\n" + new string('-', 50));
            Console.WriteLine(title);
            Console.WriteLine(new string('-', 50));
            Console.ResetColor();
        }

        /// <summary>
        /// Writes an info message in blue
        /// </summary>
        public static void WriteInfo(string message)
        {
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.WriteLine(message);
            Console.ResetColor();
        }

        /// <summary>
        /// Writes a success message in green
        /// </summary>
        public static void 
            
            
            
            
            WriteSuccess(string message)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine(message);
            Console.ResetColor();
        }

        /// <summary>
        /// Writes an error message in red
        /// </summary>
        public static void WriteError(string message)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(message);
            Console.ResetColor();
        }

        /// <summary>
        /// Writes a warning message in yellow
        /// </summary>
        public static void WriteWarning(string message)
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine(message);
            Console.ResetColor();
        }

        /// <summary>
        /// Writes a thread status message with thread ID
        /// </summary>
        public static void WriteThreadStatus(string message, int threadId)
        {
            Console.ForegroundColor = ConsoleColor.Magenta;
            Console.WriteLine($"[Thread {threadId}] {message}");
            Console.ResetColor();
        }

        /// <summary>
        /// Displays a menu and gets user selection
        /// </summary>
        public static int DisplayMenu(string title, List<string> options)
        {
            while (true)
            {
                Console.Clear();
                WriteHeader(title, "القائمة");
                
                for (int i = 0; i < options.Count; i++)
                {
                    Console.WriteLine($"{i + 1}. {options[i]}");
                }
                
                Console.WriteLine("\n0. Return to Main Menu");
                Console.Write("\nEnter your choice: ");
                
                if (int.TryParse(Console.ReadLine(), out int choice) && choice >= 0 && choice <= options.Count)
                {
                    return choice;
                }
                
                WriteError("Invalid choice. Please try again.");
                Thread.Sleep(1500);
            }
        }

        /// <summary>
        /// Waits for user to press a key to continue
        /// </summary>
        public static void WaitForKey()
        {
            Console.WriteLine("\nPress any key to continue...");
            Console.ReadKey(true);
        }

        /// <summary>
        /// Gets an integer input from the user with validation
        /// </summary>
        public static int GetIntInput(string prompt, int min, int max)
        {
            while (true)
            {
                Console.Write($"{prompt} ({min}-{max}): ");
                if (int.TryParse(Console.ReadLine(), out int value) && value >= min && value <= max)
                {
                    return value;
                }
                WriteError($"Please enter a valid number between {min} and {max}.");
            }
        }

        /// <summary>
        /// Gets a yes/no response from the user
        /// </summary>
        public static bool GetYesNoInput(string prompt)
        {
            while (true)
            {
                Console.Write($"{prompt} (y/n): ");
                string input = Console.ReadLine()?.ToLower();
                if (input == "y" || input == "yes")
                {
                    return true;
                }
                if (input == "n" || input == "no")
                {
                    return false;
                }
                WriteError("Please enter 'y' or 'n'.");
            }
        }

        /// <summary>
        /// Displays a progress bar in the console
        /// </summary>
        public static void DisplayProgressBar(int progress, int total)
        {
            int progressBarWidth = 50;
            int progressChars = progress * progressBarWidth / total;
            
            Console.Write("[");
            Console.ForegroundColor = ConsoleColor.Green;
            Console.Write(new string('#', progressChars));
            Console.ResetColor();
            Console.Write(new string('-', progressBarWidth - progressChars));
            Console.Write($"] {progress}/{total} ({progress * 100 / total}%)");
            Console.Write("\r");
        }
    }
}
