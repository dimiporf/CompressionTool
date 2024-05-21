using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace CompressionTool
{
    public static class HuffmanEncoder
    {
        public static void Encode(string text, string outputFilePath)
        {
            var frequencies = FrequencyCalculator.CalculateFrequencies(text);
            var huffmanTreeRoot = HuffmanHelper.BuildHuffmanTree(frequencies);
            var huffmanCodes = HuffmanHelper.GenerateCodes(huffmanTreeRoot);

            var encodedText = EncodeText(text, huffmanCodes);
            WriteEncodedData(outputFilePath, frequencies, encodedText);
        }

        public static string EncodeText(string text, Dictionary<char, string> huffmanCodes)
        {
            var encodedText = new StringBuilder();

            foreach (char c in text)
            {
                encodedText.Append(huffmanCodes[c]);
            }

            return encodedText.ToString();
        }

        public static void WriteEncodedData(string outputFilePath, Dictionary<char, int> frequencies, string encodedText)
        {
            using (var writer = new BinaryWriter(File.Open(outputFilePath, FileMode.Create)))
            {
                writer.Write(frequencies.Count);

                foreach (var kvp in frequencies)
                {
                    writer.Write(kvp.Key);
                    writer.Write(kvp.Value);
                }

                writer.Write("HEADER_END");

                byte[] encodedBytes = GetBytesFromBinaryString(encodedText, out int paddingBits);
                writer.Write(encodedBytes.Length);
                writer.Write(paddingBits);
                writer.Write(encodedBytes);
            }
        }

        public static byte[] GetBytesFromBinaryString(string binaryString, out int paddingBits)
        {
            int numOfBytes = (binaryString.Length + 7) / 8;
            byte[] bytes = new byte[numOfBytes];
            paddingBits = 8 - (binaryString.Length % 8);
            paddingBits = paddingBits == 8 ? 0 : paddingBits;

            for (int i = 0; i < binaryString.Length; i++)
            {
                if (binaryString[i] == '1')
                {
                    bytes[i / 8] |= (byte)(1 << (7 - (i % 8)));
                }
            }

            return bytes;
        }
    }
}
