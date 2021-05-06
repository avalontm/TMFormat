using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats;
using SixLabors.ImageSharp.Formats.Png;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace TMFormat.Helpers
{
    public static class TMImageHelper
    {
        public static byte[] ToBytes(Stream stream)
        {
            Stream outputStream = new MemoryStream();

            try
            {
                using (Image image = Image.Load(stream))
                {
                    image.Save(outputStream, new PngEncoder());

                    using (MemoryStream ms = new MemoryStream())
                    {
                        outputStream.CopyTo(ms);
                        return ms.ToArray();
                    }
                }
            }
            catch
            {
                return null;
            }
        }
    }
}
