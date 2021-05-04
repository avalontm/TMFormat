using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using System;
using System.Collections.Generic;
using System.Text;

namespace TMFormat
{
    public static class Instance
    {
        public static ContentManager Content { private set; get; }
        public static GraphicsDeviceManager Graphics { private set; get; }

        public static void Init(GraphicsDeviceManager graphics, ContentManager content)
        {
            Graphics = graphics;
            Content = content;
        }
    }
}
