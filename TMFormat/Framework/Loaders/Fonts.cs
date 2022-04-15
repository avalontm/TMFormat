using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace TMFormat.Framework.Loaders
{
    public static class Fonts
    {
        public static SpriteFont FontDefault { set; get; }

        public static void Init()
        {
            FontDefault = TMInstance.Content.Load<SpriteFont>("defaultFont");
        }

    }
}
