using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats;
using SixLabors.ImageSharp.Formats.Png;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;

namespace TMFormat.Helpers
{
    public static class TMImageHelper
    {
        static int TileSize = 32;

        public static byte[] FromFile(string file, bool transparent)
        {
            MemoryStream outputStream = new MemoryStream();

            try
            {
                using (Image image = Image.Load(file))
                {
            
                    if (image.Width != TileSize || image.Height != TileSize)
                    {
                        image.Mutate(i => i.Resize(TileSize, TileSize));
                    }

                    image.Save(outputStream, new PngEncoder());

                    if (transparent)
                    {
                       return ToReplacePixels(outputStream.ToArray());
                    }

                    return outputStream.ToArray();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[FromFile] {ex}");
                return null;
            }
        }

        public static byte[] ToBytes(Stream stream)
        {
            MemoryStream outputStream = new MemoryStream();

            try
            {
                using (Image image = Image.Load(stream))
                {
                    if (image.Width != TileSize || image.Height != TileSize)
                    {
                        image.Mutate(i => i.Resize(TileSize, TileSize));
                    }
                  
                    image.SaveAsPng(outputStream, new PngEncoder());

                    return outputStream.ToArray();

                }
            }
            catch(Exception ex)
            {
                Console.WriteLine($"[ToBytes] {ex}");
                return null;
            }
        }

        public static bool SaveToImage(byte[] bytes, string file)
        {
            try
            {
                if (string.IsNullOrEmpty(file))
                {
                    Console.WriteLine($"[SaveToImage] File rute is empty.");
                    return false;
                }
                if (bytes == null || bytes.Length == 0)
                {
                    return false;
                }
                using (Image image = Image.Load(bytes))
                {
                    image.Save($"{file}.png", new PngEncoder());
                    return true;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[SaveToImage] {ex}");
                return false;
            }
        }


        public static byte[] ToReplacePixels(byte[] bytes)
        {
            Rgba32 color = new Rgba32(255, 0, 255);
            Rgba32 replace = new Rgba32(0, 0, 0, 0);
            MemoryStream outputStream = new MemoryStream();

            using (Image<Rgba32> image = Image.Load(bytes))
            {
                for (int y = 0; y < image.Height; y++)
                {
                    for (int x = 0; x < image.Width; x++)
                    {
                        var pixel = image[x, y];
                        if (pixel == color)
                        {
                            image[x, y] = replace;
                        }
                    }
                }

                image.SaveAsPng(outputStream, new PngEncoder());

                return outputStream.ToArray();
            }
 
        }
    }
}
