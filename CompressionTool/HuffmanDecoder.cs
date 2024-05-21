using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace CompressionTool
{
    public static class HuffmanDecoder
    {
        // Main method to decode the Huffman encoded file and write the decoded data to an output file
        public static void Decode(string encodedFilePath, string outputFilePath)
        {
            // Variables to hold the Huffman codes, encoded data, and padding bits
            Dictionary<char, string> huffmanCodes;
            byte[] encodedData;
            int paddingBits;

            // Step 1: Read the encoded data from the input file and reconstruct the Huffman codes
            ReadEncodedData(encodedFilePath, out huffmanCodes, out encodedData, out paddingBits);

            // Step 2: Decode the encoded data using the Huffman codes
            string decodedText = DecodeData(encodedData, huffmanCodes, paddingBits, outputFilePath);

            // Step 3: Write the decoded text to the specified output file
            File.WriteAllText(outputFilePath, decodedText);
        }

        // Method to read the encoded data from the input file
        private static void ReadEncodedData(string encodedFilePath, out Dictionary<char, string> huffmanCodes, out byte[] encodedData, out int paddingBits)
        {
            using (var reader = new BinaryReader(File.Open(encodedFilePath, FileMode.Open)))
            {
                // Read the number of unique characters (frequency table size)
                int count = reader.ReadInt32();

                // Read each character and its frequency into a dictionary
                var frequencies = new Dictionary<char, int>();
                for (int i = 0; i < count; i++)
                {
                    char character = reader.ReadChar();
                    int frequency = reader.ReadInt32();
                    frequencies[character] = frequency;
                }

                // Read the special string indicating the end of the header section
                string headerEnd = reader.ReadString();
                if (headerEnd != "HEADER_END")
                {
                    throw new Exception("Invalid header format.");
                }

                // Read the length of the encoded data and the number of padding bits
                int encodedDataLength = reader.ReadInt32();
                paddingBits = reader.ReadInt32();
                encodedData = reader.ReadBytes(encodedDataLength);

                // Reconstruct the Huffman tree from the frequencies and generate the Huffman codes
                var huffmanTreeRoot = HuffmanHelper.BuildHuffmanTree(frequencies);
                huffmanCodes = HuffmanHelper.GenerateCodes(huffmanTreeRoot);
            }
        }

        // Method to decode the encoded data using the Huffman codes
        private static string DecodeData(byte[] encodedData, Dictionary<char, string> huffmanCodes, int paddingBits, string outputFilePath)
        {
            StringBuilder decodedText = new StringBuilder(); // To store the decoded text
            StringBuilder currentCode = new StringBuilder(); // To build the current Huffman code from the binary string

            // Convert the byte array to a binary string, considering the padding bits
            string binaryString = GetBinaryStringFromBytes(encodedData, paddingBits);

            int totalBits = binaryString.Length; // Total number of bits in the binary string
            int processedBits = 0; // Number of bits processed so far
            int lastPercentage = 0; // Last reported progress percentage

            // Iterate over each bit in the binary string
            foreach (char bit in binaryString)
            {
                currentCode.Append(bit); // Add the bit to the current Huffman code

                // Check if the current Huffman code matches any code in the Huffman codes dictionary
                foreach (var kvp in huffmanCodes)
                {
                    if (kvp.Value == currentCode.ToString())
                    {
                        decodedText.Append(kvp.Key); // Add the corresponding character to the decoded text
                        currentCode.Clear(); // Clear the current code to start building the next code
                        processedBits += kvp.Value.Length; // Update the number of processed bits

                        // Calculate the decompression progress percentage
                        int currentPercentage = (int)((double)processedBits / totalBits * 100);
                        if (currentPercentage != lastPercentage)
                        {
                            Console.CursorLeft = 0; // Reset the cursor position
                            Console.Write("[");
                            int progressWidth = 50; // Width of the progress bar
                            int progress = (int)Math.Round((double)currentPercentage / 100 * progressWidth);
                            Console.Write(new string('#', progress).PadRight(progressWidth));
                            Console.Write($"] {currentPercentage}%");
                            lastPercentage = currentPercentage; // Update the last reported progress percentage
                        }

                        break; // Exit the loop once a match is found
                    }
                }
            }

            Console.WriteLine(); // Move to the next line after the progress bar
            return decodedText.ToString(); // Return the decoded text
        }

        // Method to convert a byte array to a binary string, considering padding bits
        private static string GetBinaryStringFromBytes(byte[] bytes, int paddingBits)
        {
            StringBuilder binaryString = new StringBuilder();

            // Convert each byte to its binary representation and append it to the binary string
            foreach (byte b in bytes)
            {
                binaryString.Append(Convert.ToString(b, 2).PadLeft(8, '0'));
            }

            // Remove the padding bits from the end of the binary string
            if (paddingBits > 0)
            {
                binaryString.Length -= paddingBits;
            }

            return binaryString.ToString(); // Return the resulting binary string
        }
    }
}
