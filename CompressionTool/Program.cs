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
                Console.WriteLine("Input file found. Starting compression process...");

                // Create the output folder if it doesn't exist
                if (!Directory.Exists(outputFolderPath))
                {
                    Console.WriteLine("Output folder doesn't exist. Creating...");
                    Directory.CreateDirectory(outputFolderPath);
                    Console.WriteLine("Output folder created.");
                }
                else
                {
                    Console.WriteLine("Output folder already exists.");
                }

                // Create the output file path within the output folder
                string outputFilePath = Path.Combine(outputFolderPath, "compressed.bin");

                // Read the entire content of the input file
                string text = File.ReadAllText(inputFilePath);

                Console.WriteLine("Encoding text...");

                // Encode the text using Huffman encoding and write to the output file
                HuffmanEncoder.Encode(text, outputFilePath);

                Console.WriteLine("Text encoded successfully.");

                // Decompress the output file
                string decompressedFilePath = Path.Combine(outputFolderPath, "decompressed.txt");

                Console.WriteLine("Validating file, please wait...");

                // Decode the compressed file and write the decompressed data to the output file
                HuffmanDecoder.Decode(outputFilePath, decompressedFilePath);

                Console.WriteLine("File decompressed, checking...");

                // Read the original file content
                string originalText = File.ReadAllText(inputFilePath);

                // Read the decompressed file content
                string decompressedText = File.ReadAllText(decompressedFilePath);

                // Compare the original and decompressed content
                if (originalText == decompressedText)
                {
                    Console.WriteLine("Validation successful. Output matches original.");
                }
                else
                {
                    Console.WriteLine("Validation failed. Output does not match original.");
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
