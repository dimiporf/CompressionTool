using System;
using System.Collections.Generic;
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
                using (var writer = new BinaryWriter(File.Open(outputFilePath, FileMode.Create)))
                {
                    // Write the number of distinct characters in the frequency table
                    writer.Write(frequencies.Count);

                    // Write the frequency table
                    foreach (var kvp in frequencies)
                    {
                        writer.Write(kvp.Key);
                        writer.Write(kvp.Value);
                    }

                    // Write a marker to indicate the end of the header section
                    writer.Write("HEADER_END");

                    // Write the length of the encoded text
                    writer.Write(encodedText.Length);

                    // Write the encoded text as binary data
                    WriteEncodedText(writer, encodedText);
                }

                Console.WriteLine($"File compressed successfully and saved to {outputFilePath}");
            }
            catch (Exception ex)
            {
                // Handle any errors that occur during file reading or processing
                Console.WriteLine($"An error occurred: {ex.Message}");
            }
        }

        // Method to encode the text using the Huffman codes
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

        // Method to write the encoded text to the output file
        private static void WriteEncodedText(BinaryWriter writer, string encodedText)
        {
            // Pad the encoded text to ensure it's a multiple of 8
            encodedText = encodedText.PadRight((encodedText.Length + 7) / 8 * 8, '0');

            // Convert the bit string to bytes and write to the output file
            for (int i = 0; i < encodedText.Length; i += 8)
            {
                string byteString = encodedText.Substring(i, 8);
                writer.Write(Convert.ToByte(byteString, 2));
            }
        }

        // Method to get the output file path based on the input file path
        private static string GetOutputFilePath(string inputFilePath)
        {
            // Get the directory of the input file
            string directory = Path.GetDirectoryName(inputFilePath);

            // Get the filename without extension
            string fileName = Path.GetFileNameWithoutExtension(inputFilePath);

            // Append "_compressed.bin" to the filename
            string outputFileName = $"{fileName}_compressed.bin";

            // Combine directory and output file name to get the output file path
            return Path.Combine(directory, outputFileName);
        }
    }
}
