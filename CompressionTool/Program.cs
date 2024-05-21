using System;
using System.Collections.Generic;
using System.IO;

namespace CompressionTool
{
    class Program
    {
        static void Main(string[] args)
        {
            // Check if a filename was provided as a command-line argument
            if (args.Length == 0)
            {
                Console.WriteLine("Please provide a filename as a command-line argument.");
                return;
            }

            // Get the file path from the command-line arguments
            string filePath = args[0];

            // Check if the specified file exists
            if (!File.Exists(filePath))
            {
                Console.WriteLine("The specified file does not exist.");
                return;
            }

            try
            {
                // Read the entire content of the file
                string text = File.ReadAllText(filePath);

                // Calculate the frequency of each character in the text
                var frequencies = FrequencyCalculator.CalculateFrequencies(text);

                // Print the character frequencies to the console
                foreach (var kvp in frequencies)
                {
                    Console.WriteLine($"{kvp.Key}: {kvp.Value}");
                }
            }
            catch (Exception ex)
            {
                // Handle any errors that occur during file reading
                Console.WriteLine($"An error occurred while reading the file: {ex.Message}");
            }
        }
    }
}
