using System;
using System.Collections.Generic;
using System.Linq;

namespace CompressionTool
{
    public static class HuffmanHelper
    {
        // HuffmanNode class definition remains the same
        public class HuffmanNode
        {
            public char? Character { get; set; } // Nullable char to differentiate internal and leaf nodes
            public int Frequency { get; set; }
            public HuffmanNode Left { get; set; }
            public HuffmanNode Right { get; set; }
        }

        // Method to build the Huffman tree remains the same
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

        /// <summary>
        /// Generates the prefix-code table from the Huffman tree.
        /// </summary>
        /// <param name="root">The root node of the Huffman tree.</param>
        /// <returns>A dictionary where keys are characters and values are their respective Huffman codes.</returns>
        public static Dictionary<char, string> GenerateCodes(HuffmanNode root)
        {
            var codes = new Dictionary<char, string>();
            GenerateCodesRecursive(root, "", codes);
            return codes;
        }

        /// <summary>
        /// A recursive helper function to generate Huffman codes.
        /// </summary>
        /// <param name="node">The current node in the Huffman tree.</param>
        /// <param name="code">The current Huffman code being generated.</param>
        /// <param name="codes">The dictionary to store the generated codes.</param>
        private static void GenerateCodesRecursive(HuffmanNode node, string code, Dictionary<char, string> codes)
        {
            if (node == null)
            {
                return;
            }

            // If this is a leaf node, add the character and its code to the dictionary
            if (node.Character.HasValue)
            {
                codes[node.Character.Value] = code;
            }

            // Traverse the left subtree with '0' added to the code
            GenerateCodesRecursive(node.Left, code + "0", codes);

            // Traverse the right subtree with '1' added to the code
            GenerateCodesRecursive(node.Right, code + "1", codes);
        }
    }
}
