using System;
using System.Collections.Generic;
using Xunit;
using CompressionTool;

namespace CompressionTool.Tests
{
    public class FrequencyTests
    {
        [Fact]
        public void CalculateFrequencies_ShouldReturnCorrectFrequencies()
        {
            // Arrange
            string testData = "aaabbc";
            var expectedFrequencies = new Dictionary<char, int>
            {
                {'a', 3},
                {'b', 2},
                {'c', 1}
            };

            // Act
            var actualFrequencies = FrequencyCalculator.CalculateFrequencies(testData);

            // Assert
            Assert.Equal(expectedFrequencies, actualFrequencies);
        }

        [Fact]
        public void CalculateFrequencies_ShouldReturnZeroForEmptyString()
        {
            // Arrange
            string testData = "";
            var expectedFrequencies = new Dictionary<char, int>();

            // Act
            var actualFrequencies = FrequencyCalculator.CalculateFrequencies(testData);

            // Assert
            Assert.Equal(expectedFrequencies, actualFrequencies);
        }

        [Fact]
        public void CalculateFrequencies_ShouldHandleSpecialCharacters()
        {
            // Arrange
            string testData = "a!@#a";
            var expectedFrequencies = new Dictionary<char, int>
            {
                {'a', 2},
                {'!', 1},
                {'@', 1},
                {'#', 1}
            };

            // Act
            var actualFrequencies = FrequencyCalculator.CalculateFrequencies(testData);

            // Assert
            Assert.Equal(expectedFrequencies, actualFrequencies);
        }

        [Fact]
        public void BuildHuffmanTree_ShouldBuildCorrectTree()
        {
            // Arrange
            var frequencies = new Dictionary<char, int>
            {
                { 'a', 5 },
                { 'b', 9 },
                { 'c', 12 },
                { 'd', 13 },
                { 'e', 16 },
                { 'f', 45 }
            };

            // Act
            var root = HuffmanHelper.BuildHuffmanTree(frequencies);

            // Check that the root is not null and has the correct frequency
            // Assert
            
            Assert.NotNull(root);
            Assert.Equal(100, root.Frequency);            
            Assert.Null(root.Character);
            Assert.NotNull(root.Left);
            Assert.NotNull(root.Right);

            // Additional asserts can be added to verify the structure of the tree
        }
    }
}
