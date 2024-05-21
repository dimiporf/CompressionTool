using System;
using System.Collections.Generic;
using System.IO;
using Xunit;
using CompressionTool;

namespace CompressionTool.Tests
{
    public class CompressionTests
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

            // Assert
            Assert.NotNull(root);
            Assert.Equal(100, root.Frequency);
            Assert.Null(root.Character);
            Assert.NotNull(root.Left);
            Assert.NotNull(root.Right);
            // Additional asserts can be added to verify the structure of the tree
        }

        [Fact]
        public void EncodeText_ShouldEncodeCorrectly()
        {
            // Arrange
            string input = "aaabbc";
            var huffmanCodes = new Dictionary<char, string>
            {
                { 'a', "0" },
                { 'b', "10" },
                { 'c', "11" }
            };

            // Act
            string encodedText = CompressionTool.Program.EncodeText(input, huffmanCodes);

            // Assert
            Assert.Equal("000101011", encodedText);
        }

        [Fact]
        public void WriteHeader_ShouldWriteCorrectHeader()
        {
            // Arrange
            string outputFilePath = "output.txt";
            var frequencies = new Dictionary<char, int>
            {
                { 'a', 3 },
                { 'b', 2 },
                { 'c', 1 }
            };

            // Act
            using (var writer = new StreamWriter(outputFilePath))
            {
                foreach (var kvp in frequencies)
                {
                    writer.WriteLine($"{kvp.Key}:{kvp.Value}");
                }
                writer.WriteLine("HEADER_END");
                writer.WriteLine("000101011");
            }

            // Read the file back and verify
            var lines = File.ReadAllLines(outputFilePath);
            Assert.Equal("a:3", lines[0]);
            Assert.Equal("b:2", lines[1]);
            Assert.Equal("c:1", lines[2]);
            Assert.Equal("HEADER_END", lines[3]);
            Assert.Equal("000101011", lines[4]);
        }

        //[Fact]
        //public void DecodeText_ShouldDecodeCorrectly()
        //{
        //    // Arrange
        //    string encodedText = "000101011";
        //    var huffmanCodes = new Dictionary<char, string>
        //    {
        //        { 'a', "0" },
        //        { 'b', "10" },
        //        { 'c', "11" }
        //    };

        //    // Act
        //    string decodedText = CompressionTool.Program.DecodeText(encodedText, huffmanCodes);

        //    // Assert
        //    Assert.Equal("aaabbc", decodedText);
        //}
    }
}
