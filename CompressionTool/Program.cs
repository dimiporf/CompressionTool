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

                // Build the Huffman tree based on character frequencies
                var huffmanTreeRoot = HuffmanHelper.BuildHuffmanTree(frequencies);

                // Generate the prefix-code table from the Huffman tree
                var huffmanCodes = HuffmanHelper.GenerateCodes(huffmanTreeRoot);

                // Print the Huffman codes to the console
                foreach (var kvp in huffmanCodes)
                {
                    Console.WriteLine($"{kvp.Key}: {kvp.Value}");
                }
            }
            catch (Exception ex)
            {
                // Handle any errors that occur during file reading or processing
                Console.WriteLine($"An error occurred: {ex.Message}");
            }
        }
    }
}
