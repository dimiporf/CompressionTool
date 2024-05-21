using System;
using System.Collections.Generic;
using System.IO;

namespace CompressionTool
{
    class Program
    {
        static void Main(string[] args)
        {
            // Check if the user provided a filename as an argument
            if (args.Length == 0)
            {
                Console.WriteLine("Please provide a filename as a command-line argument.");
                return;
            }

            string filePath = args[0]; // Get the file path from the command-line arguments

            // Check if the file exists
            if (!File.Exists(filePath))
            {
                Console.WriteLine("The specified file does not exist.");
                return;
            }

            try
            {
                // Read the entire content of the text file
                string text = File.ReadAllText(filePath);

                // Calculate the frequency of each character in the text
                var frequencies = FrequencyCalculator.CalculateFrequencies(text);

                // Print frequencies to the console (for testing purposes)
                foreach (var kvp in frequencies)
                {
                    Console.WriteLine($"{kvp.Key}: {kvp.Value}");
                }

                // Optional: Validate test values (debugging aid)
                if (frequencies.ContainsKey('X') && frequencies.ContainsKey('t'))
                {
                    Console.WriteLine($"'X': {frequencies['X']}, 't': {frequencies['t']}");
                }
            }
            catch (Exception ex)
            {
                // Handle any errors that may occur during file reading
                Console.WriteLine($"An error occurred while reading the file: {ex.Message}");
            }
        }
    }
}
