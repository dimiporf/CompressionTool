using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace CompressionTool
{
    public static class HuffmanHelper
    {
        // Definition of HuffmanNode class
        public class HuffmanNode
        {
            public char? Character { get; set; }
            public int Frequency { get; set; }
            public HuffmanNode Left { get; set; }
            public HuffmanNode Right { get; set; }
        }

        // Method to build the Huffman tree
        public static HuffmanNode BuildHuffmanTree(Dictionary<char, int> frequencies)
        {
            var nodes = frequencies.Select(kv => new HuffmanNode { Character = kv.Key, Frequency = kv.Value }).ToList();
            var queue = new PriorityQueue<HuffmanNode>(nodes, (x, y) => x.Frequency.CompareTo(y.Frequency));

            while (queue.Count > 1)
            {
                var left = queue.Dequeue();
                var right = queue.Dequeue();
                var newNode = new HuffmanNode
                {
                    Frequency = left.Frequency + right.Frequency,
                    Left = left,
                    Right = right
                };
                queue.Enqueue(newNode);
            }

            return queue.Dequeue();
        }

        // Method to generate prefix-code table from the Huffman tree
        public static Dictionary<char, string> GenerateCodes(HuffmanNode root)
        {
            var codes = new Dictionary<char, string>();
            GenerateCodesRecursive(root, "", codes);
            return codes;
        }

        // Helper function for generating Huffman codes recursively
        private static void GenerateCodesRecursive(HuffmanNode node, string code, Dictionary<char, string> codes)
        {
            if (node == null)
            {
                return;
            }

            if (node.Character.HasValue)
            {
                codes[node.Character.Value] = code;
            }

            GenerateCodesRecursive(node.Left, code + "0", codes);
            GenerateCodesRecursive(node.Right, code + "1", codes);
        }

        // Method to write encoded text to the output file
        public static void WriteEncodedText(BinaryWriter writer, string encodedText)
        {
            encodedText = encodedText.PadRight((encodedText.Length + 7) / 8 * 8, '0');

            for (int i = 0; i < encodedText.Length; i += 8)
            {
                string byteString = encodedText.Substring(i, 8);
                writer.Write(Convert.ToByte(byteString, 2));
            }
        }

        // Method to read encoded text from the input file
        public static string ReadEncodedText(BinaryReader reader, int encodedLength)
        {
            var encodedText = new StringBuilder();

            for (int i = 0; i < encodedLength; i++)
            {
                byte b = reader.ReadByte();
                encodedText.Append(Convert.ToString(b, 2).PadLeft(8, '0'));
            }

            return encodedText.ToString().Substring(0, encodedLength);
        }

        // Method to decode text using the Huffman tree
        public static string DecodeText(string encodedText, HuffmanNode root)
        {
            var decodedText = new StringBuilder();
            var currentNode = root;

            foreach (char bit in encodedText)
            {
                if (bit == '0')
                {
                    currentNode = currentNode.Left;
                }
                else if (bit == '1')
                {
                    currentNode = currentNode.Right;
                }

                if (currentNode.Character != null)
                {
                    decodedText.Append(currentNode.Character);
                    currentNode = root;
                }
            }

            return decodedText.ToString();
        }
    }
   
}
