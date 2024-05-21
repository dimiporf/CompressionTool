using System;
using System.IO;

namespace CompressionTool
{
    public class Program
    {
        public static void Main(string[] args)
        {
            // Check if the correct number of arguments were provided
            if (args.Length < 1)
            {
                Console.WriteLine("Usage: CompressionTool <inputfile>");
                return;
            }

            // Get the input file path from the command-line arguments
            string inputFilePath = args[0];

            // Check if the input file exists
            if (!File.Exists(inputFilePath))
            {
                Console.WriteLine("The specified input file does not exist.");
                return;
            }

            try
            {
                // Read the entire content of the input file
                string text = File.ReadAllText(inputFilePath);

                // Calculate the frequency of each character in the text
                var frequencies = FrequencyCalculator.CalculateFrequencies(text);

                // Build the Huffman tree based on character frequencies
                var huffmanTreeRoot = HuffmanHelper.BuildHuffmanTree(frequencies);

                // Generate the prefix-code table from the Huffman tree
                var huffmanCodes = HuffmanHelper.GenerateCodes(huffmanTreeRoot);

                // Encode the text using the prefix-code table
                string encodedText = EncodeText(text, huffmanCodes);

                // Get the output file path by manipulating the input file path
                string outputFilePath = GetOutputFilePath(inputFilePath);

                // Write the header (frequency table) and encoded data to the output file
                using (var writer = new StreamWriter(outputFilePath))
                {
                    // Write the frequency table
                    foreach (var kvp in frequencies)
                    {
                        writer.WriteLine($"{kvp.Key}:{kvp.Value}");
                    }

                    // Write a marker to indicate the end of the header section
                    writer.WriteLine("HEADER_END");

                    // Write the encoded text
                    writer.WriteLine(encodedText);
                }

                Console.WriteLine($"File compressed successfully and saved to {outputFilePath}");
            }
            catch (Exception ex)
            {
                // Handle any errors that occur during file reading or processing
                Console.WriteLine($"An error occurred: {ex.Message}");
            }
        }

        // Method to encode the text remains the same
        public static string EncodeText(string text, Dictionary<char, string> huffmanCodes)
        {
            var encodedText = new System.Text.StringBuilder();

            // Replace each character in the input text with its corresponding Huffman code
            foreach (char c in text)
            {
                encodedText.Append(huffmanCodes[c]);
            }

            return encodedText.ToString();
        }

        // Method to get the output file path based on the input file path
        private static string GetOutputFilePath(string inputFilePath)
        {
            // Get the directory of the input file
            string directory = Path.GetDirectoryName(inputFilePath);

            // Get the filename without extension
            string fileName = Path.GetFileNameWithoutExtension(inputFilePath);

            // Append "_compressed.txt" to the filename
            string outputFileName = $"{fileName}_compressed.txt";

            // Combine directory and output file name to get the output file path
            return Path.Combine(directory, outputFileName);
        }
    }
}
