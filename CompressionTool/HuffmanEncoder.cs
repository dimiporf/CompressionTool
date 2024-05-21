using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace CompressionTool
{
    public static class HuffmanEncoder
    {
        // Main method to perform Huffman encoding on the input text and write the encoded data to a file
        public static void Encode(string text, string outputFilePath)
        {
            // Step 1: Calculate the frequencies of each character in the input text
            var frequencies = FrequencyCalculator.CalculateFrequencies(text);

            // Step 2: Build the Huffman tree from the character frequencies
            var huffmanTreeRoot = HuffmanHelper.BuildHuffmanTree(frequencies);

            // Step 3: Generate the Huffman codes (prefix codes) from the Huffman tree
            var huffmanCodes = HuffmanHelper.GenerateCodes(huffmanTreeRoot);

            // Step 4: Encode the input text using the generated Huffman codes
            var encodedText = EncodeText(text, huffmanCodes);

            // Step 5: Write the encoded data to the specified output file
            WriteEncodedData(outputFilePath, frequencies, encodedText);
        }

        // Method to encode the input text using the given Huffman codes
        public static string EncodeText(string text, Dictionary<char, string> huffmanCodes)
        {
            var encodedText = new StringBuilder();

            // Replace each character in the text with its corresponding Huffman code
            foreach (char c in text)
            {
                encodedText.Append(huffmanCodes[c]);
            }

            return encodedText.ToString();
        }

        // Method to write the encoded data, along with the frequency table, to the output file
        public static void WriteEncodedData(string outputFilePath, Dictionary<char, int> frequencies, string encodedText)
        {
            using (var writer = new BinaryWriter(File.Open(outputFilePath, FileMode.Create)))
            {
                // Write the number of unique characters (frequency table size)
                writer.Write(frequencies.Count);

                // Write each character and its frequency to the file
                foreach (var kvp in frequencies)
                {
                    writer.Write(kvp.Key);
                    writer.Write(kvp.Value);
                }

                // Write a special string to indicate the end of the header section
                writer.Write("HEADER_END");

                // Convert the encoded binary string to a byte array and calculate the number of padding bits
                byte[] encodedBytes = GetBytesFromBinaryString(encodedText, out int paddingBits);

                // Write the length of the byte array and the number of padding bits to the file
                writer.Write(encodedBytes.Length);
                writer.Write(paddingBits);

                // Write the encoded byte array to the file
                writer.Write(encodedBytes);
            }
        }

        // Method to convert a binary string to a byte array and calculate the number of padding bits
        public static byte[] GetBytesFromBinaryString(string binaryString, out int paddingBits)
        {
            // Calculate the number of bytes needed to store the binary string
            int numOfBytes = (binaryString.Length + 7) / 8;
            byte[] bytes = new byte[numOfBytes];

            // Calculate the number of padding bits required to make the binary string length a multiple of 8
            paddingBits = 8 - (binaryString.Length % 8);
            paddingBits = paddingBits == 8 ? 0 : paddingBits;

            // Convert the binary string to a byte array
            for (int i = 0; i < binaryString.Length; i++)
            {
                if (binaryString[i] == '1')
                {
                    // Set the corresponding bit in the byte array
                    bytes[i / 8] |= (byte)(1 << (7 - (i % 8)));
                }
            }

            return bytes;
        }
    }
}
