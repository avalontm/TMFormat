using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats;
using SixLabors.ImageSharp.Formats.Png;
using SixLabors.ImageSharp.Processing;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;

namespace TMFormat.Helpers
{
    public static class TMImageHelper
    {
        static int TileSize = 32;

        public static byte[] FromFile(string file)
        {
            Stream outputStream = new MemoryStream();

            try
            {
                using (Image image = Image.Load(file))
                {
                    if (image.Width != TileSize || image.Height != TileSize)
                    {
                        image.Mutate(i => i.Resize(TileSize, TileSize));
                    }

                    image.Save(outputStream, new PngEncoder());

                    using (MemoryStream ms = new MemoryStream())
                    {
                        outputStream.CopyTo(ms);
                        return ms.ToArray();
                    }
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
            Stream outputStream = new MemoryStream();

            try
            {
                using (Image image = Image.Load(stream))
                {
                    if (image.Width != TileSize || image.Height != TileSize)
                    {
                        image.Mutate(i => i.Resize(TileSize, TileSize));
                    }
                  
                    image.SaveAsPng(outputStream, new PngEncoder());

                    using (MemoryStream ms = new MemoryStream())
                    {
                        outputStream.CopyTo(ms);
                        return ms.ToArray();
                    }
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
                using (Image image = Image.Load(bytes))
                {
                    image.Save($"{file}.png");
                    return true;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[SaveToImage] {ex}");
                return false;
            }
        }

    }
}
