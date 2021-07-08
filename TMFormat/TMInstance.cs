using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace TMFormat
{
    public static class TMInstance
    {
        public static bool UseMonoGame { private set; get; }
        public static ContentManager Content { private set; get; }
        public static bool UseTextures { get; private set; }
        public static GraphicsDevice GraphicsDevice { get; set; }

        public static void Init(bool useMonoGame = false, bool useTextures = false)
        {
            UseMonoGame = useMonoGame;
            UseTextures = useTextures;
        }

        public static void Init(ContentManager content, bool useMonoGame = true, bool useTextures = true)
        {
            Content = content;
            UseMonoGame = useMonoGame;
            UseTextures = useTextures;
        }
    }
}
