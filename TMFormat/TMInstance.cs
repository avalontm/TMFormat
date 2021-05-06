using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using System;
using System.Collections.Generic;
using System.Text;

namespace TMFormat
{
    public static class TMInstance
    {
        public static bool ShowGrapics { private set; get; }
        public static ContentManager Content { private set; get; }
        public static GraphicsDeviceManager Graphics { private set; get; }

        public static void Init(bool useGrapics = false)
        {
            ShowGrapics = useGrapics;
            Graphics = null;
            Content = null;
        }

        public static void Init(GraphicsDeviceManager graphics, ContentManager content, bool useGrapics = true)
        {
            ShowGrapics = useGrapics;
            Graphics = graphics;
            Content = content;
        }
    }
}
