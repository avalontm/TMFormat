using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace TMFormat.Framework.Loaders
{
    public class Shaders
    {
        public static Effect lightingEffect;
        public static Effect ambientegEffect;
        public static Effect bluerEffect;

        public static void Init()
        {
            try
            {
                lightingEffect = TMInstance.Content.Load<Effect>("shaders/pixelshader");
                ambientegEffect = TMInstance.Content.Load<Effect>("shaders/ambiente");
                bluerEffect = TMInstance.Content.Load<Effect>("shaders/blur");
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }
        }
    }
}
