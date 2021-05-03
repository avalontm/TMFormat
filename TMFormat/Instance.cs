using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace TMFormat
{
    public static class Instance
    {
        public static ContentManager Content { private set; get; }
        public static GraphicsDevice Graphics { private set; get; }

        public static void Init(GraphicsDevice graphics, ContentManager content)
        {
            Graphics = graphics;
            Content = content;
        }
    }
}
