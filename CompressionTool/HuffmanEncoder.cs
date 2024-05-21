using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace CompressionTool
{
    public static class HuffmanEncoder
    {
        public static void Encode(string text, Dictionary<char, string> huffmanCodes, string outputFilePath)
        {
            // Encode the text using the prefix-code table
            string encodedText = EncodeText(text, huffmanCodes);

            // Write the encoded text to the output file
            WriteEncodedData(outputFilePath, huffmanCodes, encodedText);
        }

        public static string EncodeText(string text, Dictionary<char, string> huffmanCodes)
        {
            StringBuilder encodedText = new StringBuilder();

            foreach (char c in text)
            {
                encodedText.Append(huffmanCodes[c]);
            }

            return encodedText.ToString();
        }

        public static void WriteEncodedData(string outputFilePath, Dictionary<char, string> huffmanCodes, string encodedText)
        {
            using (var writer = new BinaryWriter(File.Open(outputFilePath, FileMode.Create)))
            {
                // Write the number of characters in the huffmanCodes dictionary
                writer.Write(huffmanCodes.Count);

                // Write each character and its corresponding Huffman code
                foreach (var kvp in huffmanCodes)
                {
                    writer.Write(kvp.Key);
                    writer.Write(kvp.Value);
                }

                // Write the encoded text
                writer.Write(encodedText);
            }
        }
    }
}
