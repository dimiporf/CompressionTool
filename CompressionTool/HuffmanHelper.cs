using System;
using System.Collections.Generic;
using System.Linq;

namespace CompressionTool
{
    public static class HuffmanHelper
    {
        public class HuffmanNode
        {
            public char? Character { get; set; }
            public int Frequency { get; set; }
            public HuffmanNode Left { get; set; }
            public HuffmanNode Right { get; set; }
        }

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
    }
}
