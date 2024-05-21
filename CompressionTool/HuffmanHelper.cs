using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace CompressionTool
{
    public static class HuffmanHelper
    {
        // Definition of HuffmanNode class
        // This class represents a node in the Huffman Tree
        public class HuffmanNode
        {
            // Character stored in the node (null for non-leaf nodes)
            public char? Character { get; set; }
            // Frequency of the character (sum of frequencies for non-leaf nodes)
            public int Frequency { get; set; }
            // Left child in the Huffman Tree
            public HuffmanNode Left { get; set; }
            // Right child in the Huffman Tree
            public HuffmanNode Right { get; set; }
        }

        // Method to build the Huffman tree from a dictionary of character frequencies
        public static HuffmanNode BuildHuffmanTree(Dictionary<char, int> frequencies)
        {
            // Convert the frequency dictionary to a list of HuffmanNodes
            var nodes = frequencies.Select(kv => new HuffmanNode { Character = kv.Key, Frequency = kv.Value }).ToList();
            // Create a priority queue to hold the HuffmanNodes, sorted by frequency
            var queue = new PriorityQueue<HuffmanNode>(nodes, (x, y) => x.Frequency.CompareTo(y.Frequency));

            // Build the tree by combining the two nodes with the lowest frequencies until only one node is left
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

            // Return the root of the Huffman Tree
            return queue.Dequeue();
        }

        // Method to generate the prefix-code table from the Huffman tree
        public static Dictionary<char, string> GenerateCodes(HuffmanNode root)
        {
            var codes = new Dictionary<char, string>();
            // Recursive helper function to generate codes
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

            // If the node is a leaf, add its code to the dictionary
            if (node.Character.HasValue)
            {
                codes[node.Character.Value] = code;
            }

            // Traverse the left and right subtrees, appending '0' and '1' to the code, respectively
            GenerateCodesRecursive(node.Left, code + "0", codes);
            GenerateCodesRecursive(node.Right, code + "1", codes);
        }

        // Method to write encoded text to the output file
        public static void WriteEncodedText(BinaryWriter writer, string encodedText)
        {
            // Pad the encoded text with '0's to make its length a multiple of 8
            encodedText = encodedText.PadRight((encodedText.Length + 7) / 8 * 8, '0');

            // Write each 8-bit segment of the encoded text as a byte to the output file
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

            // Read each byte from the input file and convert it to an 8-bit binary string
            for (int i = 0; i < encodedLength; i++)
            {
                byte b = reader.ReadByte();
                encodedText.Append(Convert.ToString(b, 2).PadLeft(8, '0'));
            }

            // Return the binary string, truncated to the specified length
            return encodedText.ToString().Substring(0, encodedLength);
        }

        // Method to decode text using the Huffman tree
        public static string DecodeText(string encodedText, HuffmanNode root)
        {
            var decodedText = new StringBuilder();
            var currentNode = root;

            // Traverse the binary string, following the Huffman tree
            foreach (char bit in encodedText)
            {
                // Move left for '0', right for '1'
                if (bit == '0')
                {
                    currentNode = currentNode.Left;
                }
                else if (bit == '1')
                {
                    currentNode = currentNode.Right;
                }

                // If a leaf node is reached, append the character to the decoded text and restart from the root
                if (currentNode.Character != null)
                {
                    decodedText.Append(currentNode.Character);
                    currentNode = root;
                }
            }

            // Return the decoded text
            return decodedText.ToString();
        }
    }
}
