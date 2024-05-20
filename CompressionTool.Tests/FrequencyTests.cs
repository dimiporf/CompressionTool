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
            var actualFrequencies = HuffmanHelper.CalculateFrequencies(testData);

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
            var actualFrequencies = HuffmanHelper.CalculateFrequencies(testData);

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
            var actualFrequencies = HuffmanHelper.CalculateFrequencies(testData);

            // Assert
            Assert.Equal(expectedFrequencies, actualFrequencies);
        }
    }
}
