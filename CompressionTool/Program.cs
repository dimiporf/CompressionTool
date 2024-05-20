using System;
using System.Collections.Generic;
using System.IO;

namespace CompressionTool

{
    class Program
    {
        static void Main(string[] args)
        {
            // Path to the downloaded text file
            string filePath = "/135-0.txt"; // Make sure this path is correct relative to your project directory
            string text = File.ReadAllText(filePath);

            // Calculate frequencies
            var frequencies = CalculateFrequencies(text);

            // Print frequencies (for testing)
            foreach (var kvp in frequencies)
            {
                Console.WriteLine($"{kvp.Key}: {kvp.Value}");
            }
        }

        static Dictionary<char, int> CalculateFrequencies(string text)
        {
            var frequencyDict = new Dictionary<char, int>();
            foreach (char c in text)
            {
                if (frequencyDict.ContainsKey(c))
                {
                    frequencyDict[c]++;
                }
                else
                {
                    frequencyDict[c] = 1;
                }
            }
            return frequencyDict;
        }
    }
}
