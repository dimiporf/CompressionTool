using System;
using System.IO;
using System.Collections.Generic;
using System.Text;

namespace CompressionTool
{
    public class Program
    {
        public static void Main(string[] args)
        {
            // Check if the correct number of arguments were provided
            if (args.Length < 2)
            {
                Console.WriteLine("Usage: CompressionTool <inputfile> <outputfolder>");
                return;
            }

            // Get the input file path and output folder path from the command-line arguments
            string inputFilePath = args[0];
            string outputFolderPath = args[1];

            // Check if the input file exists
            if (!File.Exists(inputFilePath))
            {
                Console.WriteLine("The specified input file does not exist.");
                return;
            }

            try
            {
                // Create the output folder if it doesn't exist
                if (!Directory.Exists(outputFolderPath))
                {
                    Directory.CreateDirectory(outputFolderPath);
                }

                // Create the output file path within the output folder
                string outputFilePath = Path.Combine(outputFolderPath, "compressed.bin");

                // Read the entire content of the input file
                string text = File.ReadAllText(inputFilePath);

                // Encode the text using Huffman encoding and write to the output file
                HuffmanEncoder.Encode(text, outputFilePath);

                // Decompress the output file
                string decompressedFilePath = Path.Combine(outputFolderPath, "decompressed.txt");
                HuffmanDecoder.Decode(outputFilePath, decompressedFilePath);

                // Read the original file content
                string originalText = File.ReadAllText(inputFilePath);

                // Read the decompressed file content
                string decompressedText = File.ReadAllText(decompressedFilePath);

                // Compare the original and decompressed content
                if (originalText == decompressedText)
                {
                    Console.WriteLine("Decompression successful. Output matches original.");
                }
                else
                {
                    Console.WriteLine("Decompression failed. Output does not match original.");
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
