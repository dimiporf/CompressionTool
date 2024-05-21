using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace CompressionTool
{
    public static class HuffmanDecoder
    {
        public static void Decode(string encodedFilePath, string outputFilePath)
        {
            // Read the encoded data from the file
            Dictionary<char, string> huffmanCodes;
            string encodedText;
            ReadEncodedData(encodedFilePath, out huffmanCodes, out encodedText);

            // Decode the encoded data using the prefix table
            string decodedText = DecodeText(encodedText, huffmanCodes);

            // Write the decoded text to the output file
            File.WriteAllText(outputFilePath, decodedText);
        }

        private static void ReadEncodedData(string encodedFilePath, out Dictionary<char, string> huffmanCodes, out string encodedText)
        {
            using (var reader = new BinaryReader(File.Open(encodedFilePath, FileMode.Open)))
            {
                // Read the number of characters in the huffmanCodes dictionary
                int count = reader.ReadInt32();

                // Read each character and its corresponding Huffman code
                huffmanCodes = new Dictionary<char, string>();
                for (int i = 0; i < count; i++)
                {
                    char character = reader.ReadChar();
                    string code = reader.ReadString();
                    huffmanCodes[character] = code;
                }

                // Read the encoded text
                encodedText = reader.ReadString();
            }
        }

        private static string DecodeText(string encodedText, Dictionary<char, string> huffmanCodes)
        {
            StringBuilder decodedText = new StringBuilder();
            StringBuilder currentCode = new StringBuilder();

            foreach (char bit in encodedText)
            {
                currentCode.Append(bit);

                // Check if the current code matches any prefix in the prefix table
                foreach (var kvp in huffmanCodes)
                {
                    if (kvp.Value == currentCode.ToString())
                    {
                        // If a match is found, append the corresponding character to the decoded text
                        decodedText.Append(kvp.Key);
                        currentCode.Clear();
                        break;
                    }
                }
            }

            return decodedText.ToString();
        }
    }
}
