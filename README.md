# CompressionTool

CompressionTool is a command-line tool for text compression and decompression using Huffman coding.

## Table of Contents

- [Introduction](#introduction)
- [Installation](#installation)
- [Usage](#usage)
- [Functionality](#functionality)
- [Contributing](#contributing)
- [License](#license)

## Introduction

**CompressionTool** is a lightweight tool written in C# that utilizes Huffman coding, a popular algorithm for lossless data compression. It compresses text files efficiently and decompresses them back to their original form without any loss of data.

## Installation

To use **CompressionTool**, you need to have the .NET runtime installed on your machine. You can download it from the [official .NET website](https://dotnet.microsoft.com/download).

Once you have .NET installed, you can clone this repository or download the source code as a ZIP file and extract it to your desired location.

## Usage

**CompressionTool** is a command-line tool, and it accepts two arguments: the path to the input file and the path to the output folder. Here's how you can run it:

```bash
dotnet run -- <inputfile> <outputfolder>
```

Replace <inputfile> with the path to the text file you want to compress and <outputfolder> with the path to the folder where you want to save the compressed and decompressed files.


## Functionality

**CompressionTool** provides the following functionality:

* __Compression:__ It compresses the input text file using Huffman coding and saves the compressed data to a binary file.
  
* __Decompression:__ It decompresses the compressed binary file back to its original text form.
  
* __Validation:__ It validates whether the decompressed text matches the original text, ensuring lossless compression and decompression.
  

## Contributing

Contributions are welcome! If you find any bugs, have feature requests, or want to contribute code, please open an issue or submit a pull request on the GitHub repository.
