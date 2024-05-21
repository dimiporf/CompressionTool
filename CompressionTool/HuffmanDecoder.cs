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
            Dictionary<char, string> huffmanCodes;
            byte[] encodedData;
            int paddingBits;
            ReadEncodedData(encodedFilePath, out huffmanCodes, out encodedData, out paddingBits);

            string decodedText = DecodeData(encodedData, huffmanCodes, paddingBits);

            File.WriteAllText(outputFilePath, decodedText);
        }

        private static void ReadEncodedData(string encodedFilePath, out Dictionary<char, string> huffmanCodes, out byte[] encodedData, out int paddingBits)
        {
            using (var reader = new BinaryReader(File.Open(encodedFilePath, FileMode.Open)))
            {
                int count = reader.ReadInt32();

                var frequencies = new Dictionary<char, int>();
                for (int i = 0; i < count; i++)
                {
                    char character = reader.ReadChar();
                    int frequency = reader.ReadInt32();
                    frequencies[character] = frequency;
                }

                string headerEnd = reader.ReadString();
                if (headerEnd != "HEADER_END")
                {
                    throw new Exception("Invalid header format.");
                }

                int encodedDataLength = reader.ReadInt32();
                paddingBits = reader.ReadInt32();
                encodedData = reader.ReadBytes(encodedDataLength);

                var huffmanTreeRoot = HuffmanHelper.BuildHuffmanTree(frequencies);
                huffmanCodes = HuffmanHelper.GenerateCodes(huffmanTreeRoot);
            }
        }

        private static string DecodeData(byte[] encodedData, Dictionary<char, string> huffmanCodes, int paddingBits)
        {
            StringBuilder decodedText = new StringBuilder();
            StringBuilder currentCode = new StringBuilder();

            string binaryString = GetBinaryStringFromBytes(encodedData, paddingBits);

            foreach (char bit in binaryString)
            {
                currentCode.Append(bit);

                foreach (var kvp in huffmanCodes)
                {
                    if (kvp.Value == currentCode.ToString())
                    {
                        decodedText.Append(kvp.Key);
                        currentCode.Clear();
                        break;
                    }
                }
            }

            return decodedText.ToString();
        }

        private static string GetBinaryStringFromBytes(byte[] bytes, int paddingBits)
        {
            StringBuilder binaryString = new StringBuilder();

            foreach (byte b in bytes)
            {
                binaryString.Append(Convert.ToString(b, 2).PadLeft(8, '0'));
            }

            if (paddingBits > 0)
            {
                binaryString.Length -= paddingBits;
            }

            return binaryString.ToString();
        }
    }
}
