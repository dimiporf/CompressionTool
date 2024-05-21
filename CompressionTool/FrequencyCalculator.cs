using System.Collections.Generic;

namespace CompressionTool
{
    public static class FrequencyCalculator
    {
        /// <summary>
        /// Calculates the frequency of each character in the given text.
        /// </summary>
        /// <param name="text">The input text for which to calculate character frequencies.</param>
        /// <returns>A dictionary where keys are characters and values are their respective frequencies.</returns>
        public static Dictionary<char, int> CalculateFrequencies(string text)
        {
            // Initialize an empty dictionary to store character frequencies
            var frequencyDict = new Dictionary<char, int>();

            // Iterate through each character in the text
            foreach (char c in text)
            {
                // If the character is already in the dictionary, increment its count
                if (frequencyDict.ContainsKey(c))
                {
                    frequencyDict[c]++;
                }
                // Otherwise, add the character to the dictionary with a count of 1
                else
                {
                    frequencyDict[c] = 1;
                }
            }

            // Return the dictionary containing character frequencies
            return frequencyDict;
        }
    }
}
