using System.IO.Compression;
using System.Buffers.Binary;
using System.IO;
using System;

namespace Xui.Runtime.Software
{
    public class PngDecoder
    {
        static readonly byte[] PngSignature = { 137, 80, 78, 71, 13, 10, 26, 10 };

        public static byte[] LoadRGBA(string path, out int width, out int height)
        {
            using var file = File.OpenRead(path);
            return LoadRGBA(file, out width, out height);
        }

        public static byte[] LoadRGBA(Stream stream, out int width, out int height)
        {
            using var reader = new BinaryReader(stream);

            // Verify PNG signature
            var sig = reader.ReadBytes(8);
            for (int i = 0; i < 8; i++)
            {
                if (sig[i] != PngSignature[i])
                    throw new InvalidDataException("Not a valid PNG file.");
            }

            width = 0;
            height = 0;
            int bitDepth = 0;
            int colorType = 0;
            using var idatStream = new MemoryStream();

            // Read chunks
            while (stream.Position < stream.Length)
            {
                var chunk = ReadChunk(reader);

                switch (chunk.Type)
                {
                    case "IHDR":
                        width = BinaryPrimitives.ReadInt32BigEndian(chunk.Data.AsSpan(0, 4));
                        height = BinaryPrimitives.ReadInt32BigEndian(chunk.Data.AsSpan(4, 4));
                        bitDepth = chunk.Data[8];
                        colorType = chunk.Data[9];
                        if (bitDepth != 8 || colorType != 6)
                            throw new NotSupportedException(
                                $"Only 8-bit RGBA PNGs are supported (got bitDepth={bitDepth}, colorType={colorType}).");
                        break;

                    case "IDAT":
                        idatStream.Write(chunk.Data, 0, chunk.Data.Length);
                        break;

                    case "IEND":
                        goto done;
                }
            }

            done:

            if (width == 0 || height == 0)
                throw new InvalidDataException("PNG missing IHDR chunk.");

            // Decompress IDAT data (zlib: 2-byte header + deflate + 4-byte adler32)
            idatStream.Position = 2; // Skip zlib header (CMF + FLG)
            using var deflate = new DeflateStream(idatStream, CompressionMode.Decompress);

            int stride = width * 4;
            var rawData = new byte[(stride + 1) * height]; // +1 for filter byte per row
            int totalRead = 0;
            while (totalRead < rawData.Length)
            {
                int read = deflate.Read(rawData, totalRead, rawData.Length - totalRead);
                if (read == 0) break;
                totalRead += read;
            }

            // Reconstruct pixels by reversing PNG row filters
            var pixels = new byte[width * height * 4];

            for (int y = 0; y < height; y++)
            {
                int rawRowOffset = y * (stride + 1);
                byte filterType = rawData[rawRowOffset];
                int srcOffset = rawRowOffset + 1;
                int dstOffset = y * stride;

                switch (filterType)
                {
                    case 0: // None
                        Array.Copy(rawData, srcOffset, pixels, dstOffset, stride);
                        break;

                    case 1: // Sub
                        for (int x = 0; x < stride; x++)
                        {
                            byte left = x >= 4 ? pixels[dstOffset + x - 4] : (byte)0;
                            pixels[dstOffset + x] = (byte)(rawData[srcOffset + x] + left);
                        }
                        break;

                    case 2: // Up
                        for (int x = 0; x < stride; x++)
                        {
                            byte above = y > 0 ? pixels[dstOffset - stride + x] : (byte)0;
                            pixels[dstOffset + x] = (byte)(rawData[srcOffset + x] + above);
                        }
                        break;

                    case 3: // Average
                        for (int x = 0; x < stride; x++)
                        {
                            byte left = x >= 4 ? pixels[dstOffset + x - 4] : (byte)0;
                            byte above = y > 0 ? pixels[dstOffset - stride + x] : (byte)0;
                            pixels[dstOffset + x] = (byte)(rawData[srcOffset + x] + (left + above) / 2);
                        }
                        break;

                    case 4: // Paeth
                        for (int x = 0; x < stride; x++)
                        {
                            byte left = x >= 4 ? pixels[dstOffset + x - 4] : (byte)0;
                            byte above = y > 0 ? pixels[dstOffset - stride + x] : (byte)0;
                            byte upperLeft = (x >= 4 && y > 0) ? pixels[dstOffset - stride + x - 4] : (byte)0;
                            pixels[dstOffset + x] = (byte)(rawData[srcOffset + x] + PaethPredictor(left, above, upperLeft));
                        }
                        break;

                    default:
                        throw new InvalidDataException($"Unknown PNG filter type: {filterType}");
                }
            }

            return pixels;
        }

        static byte PaethPredictor(byte a, byte b, byte c)
        {
            int p = a + b - c;
            int pa = Math.Abs(p - a);
            int pb = Math.Abs(p - b);
            int pc = Math.Abs(p - c);
            if (pa <= pb && pa <= pc) return a;
            if (pb <= pc) return b;
            return c;
        }

        static (string Type, byte[] Data) ReadChunk(BinaryReader reader)
        {
            int length = BinaryPrimitives.ReverseEndianness(reader.ReadInt32());
            var typeBytes = reader.ReadBytes(4);
            var type = System.Text.Encoding.ASCII.GetString(typeBytes);
            var data = reader.ReadBytes(length);
            reader.ReadInt32(); // skip CRC
            return (type, data);
        }
    }
}
