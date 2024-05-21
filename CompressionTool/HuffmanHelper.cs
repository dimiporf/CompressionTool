using System;
using System.Collections.Generic;
using System.Linq;

namespace CompressionTool
{
    public static class HuffmanHelper
    {
        /// <summary>
        /// Represents a node in the Huffman binary tree.
        /// </summary>
        public class HuffmanNode
        {
            public char? Character { get; set; } // Nullable char to differentiate internal and leaf nodes
            public int Frequency { get; set; }
            public HuffmanNode Left { get; set; }
            public HuffmanNode Right { get; set; }
        }

        /// <summary>
        /// Constructs the Huffman binary tree from the given character frequencies.
        /// </summary>
        /// <param name="frequencies">The dictionary containing character frequencies.</param>
        /// <returns>The root node of the Huffman binary tree.</returns>
        public static HuffmanNode BuildHuffmanTree(Dictionary<char, int> frequencies)
        {
            // Create leaf nodes for each character frequency
            var nodes = frequencies.Select(kv => new HuffmanNode { Character = kv.Key, Frequency = kv.Value }).ToList();

            // Create a priority queue (min-heap) based on frequencies
            var queue = new PriorityQueue<HuffmanNode>(nodes, (x, y) => x.Frequency.CompareTo(y.Frequency));

            // Build the Huffman tree by combining the two nodes with the lowest frequencies until one node remains
            while (queue.Count > 1)
            {
                // Remove the two nodes with the lowest frequency
                var left = queue.Dequeue();
                var right = queue.Dequeue();

                // Create a new internal node with the sum of their frequencies
                var newNode = new HuffmanNode
                {
                    Frequency = left.Frequency + right.Frequency,
                    Left = left,
                    Right = right
                };

                // Add the new internal node back into the queue
                queue.Enqueue(newNode);
            }

            // The remaining node in the queue is the root of the Huffman tree
            return queue.Dequeue();
        }
    }
}
