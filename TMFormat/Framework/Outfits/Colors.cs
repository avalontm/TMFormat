using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace TMFormat.Framework.Outfits
{
    public enum ColorOutfit
    {
        Head,
        Body,
        Legs,
        Feet
    }

    public class OutfitColors
    {
        public Color color { set; get; }

    }

    public static class Colors
    {
        public static List<OutfitColors> colors;

        public static Color GetColor(int color_id)
        {
            if (color_id < colors.Count)
            {
                return colors[color_id].color;
            }

            return Color.White;
        }

        public static void Init()
        {
            if (colors == null)
            {
                colors = new List<OutfitColors>();
            }

            if (colors.Count > 0)
            {
                return;
            }

            //Paleta 1
            colors.Add(new OutfitColors() { color = new Color(255, 255, 255) });
            colors.Add(new OutfitColors() { color = new Color(255, 212, 212) });
            colors.Add(new OutfitColors() { color = new Color(255, 233, 191) });
            colors.Add(new OutfitColors() { color = new Color(255, 255, 191) });
            colors.Add(new OutfitColors() { color = new Color(233, 255, 191) });
            colors.Add(new OutfitColors() { color = new Color(212, 255, 191) });
            colors.Add(new OutfitColors() { color = new Color(191, 255, 191) });
            colors.Add(new OutfitColors() { color = new Color(191, 255, 212) });
            colors.Add(new OutfitColors() { color = new Color(191, 255, 233) });
            colors.Add(new OutfitColors() { color = new Color(191, 255, 255) });
            colors.Add(new OutfitColors() { color = new Color(191, 233, 255) });
            colors.Add(new OutfitColors() { color = new Color(191, 212, 255) });
            colors.Add(new OutfitColors() { color = new Color(191, 191, 255) });
            colors.Add(new OutfitColors() { color = new Color(212, 191, 255) });
            colors.Add(new OutfitColors() { color = new Color(233, 191, 255) });
            colors.Add(new OutfitColors() { color = new Color(255, 191, 255) });
            colors.Add(new OutfitColors() { color = new Color(255, 191, 233) });
            colors.Add(new OutfitColors() { color = new Color(255, 191, 212) });
            colors.Add(new OutfitColors() { color = new Color(255, 191, 191) });

            //Paleta 2
            colors.Add(new OutfitColors() { color = new Color(109, 109, 109) });
            colors.Add(new OutfitColors() { color = new Color(255, 85, 0) });
            colors.Add(new OutfitColors() { color = new Color(255, 170, 0) });
            colors.Add(new OutfitColors() { color = new Color(255, 255, 0) });
            colors.Add(new OutfitColors() { color = new Color(169, 255, 0) });
            colors.Add(new OutfitColors() { color = new Color(84, 255, 0) });
            colors.Add(new OutfitColors() { color = new Color(0, 255, 0) });
            colors.Add(new OutfitColors() { color = new Color(0, 255, 85) });
            colors.Add(new OutfitColors() { color = new Color(0, 255, 170) });
            colors.Add(new OutfitColors() { color = new Color(0, 255, 255) });
            colors.Add(new OutfitColors() { color = new Color(0, 169, 255) });
            colors.Add(new OutfitColors() { color = new Color(0, 85, 255) });
            colors.Add(new OutfitColors() { color = new Color(0, 0, 255) });
            colors.Add(new OutfitColors() { color = new Color(84, 0, 255) });
            colors.Add(new OutfitColors() { color = new Color(170, 0, 255) });
            colors.Add(new OutfitColors() { color = new Color(254, 0, 255) });
            colors.Add(new OutfitColors() { color = new Color(255, 0, 169) });
            colors.Add(new OutfitColors() { color = new Color(255, 0, 85) });
            colors.Add(new OutfitColors() { color = new Color(255, 0, 0) });

            //Paleta 3
            colors.Add(new OutfitColors() { color = new Color(72, 72, 72) });
            colors.Add(new OutfitColors() { color = new Color(191, 63, 0) });
            colors.Add(new OutfitColors() { color = new Color(191, 127, 0) });
            colors.Add(new OutfitColors() { color = new Color(191, 191, 0) });
            colors.Add(new OutfitColors() { color = new Color(127, 191, 0) });
            colors.Add(new OutfitColors() { color = new Color(63, 191, 0) });
            colors.Add(new OutfitColors() { color = new Color(0, 191, 0) });
            colors.Add(new OutfitColors() { color = new Color(0, 191, 63) });
            colors.Add(new OutfitColors() { color = new Color(0, 191, 127) });
            colors.Add(new OutfitColors() { color = new Color(0, 191, 191) });
            colors.Add(new OutfitColors() { color = new Color(0, 127, 191) });
            colors.Add(new OutfitColors() { color = new Color(0, 63, 191) });
            colors.Add(new OutfitColors() { color = new Color(0, 0, 191) });
            colors.Add(new OutfitColors() { color = new Color(63, 0, 191) });
            colors.Add(new OutfitColors() { color = new Color(127, 0, 191) });
            colors.Add(new OutfitColors() { color = new Color(191, 0, 191) });
            colors.Add(new OutfitColors() { color = new Color(191, 0, 127) });
            colors.Add(new OutfitColors() { color = new Color(191, 0, 63) });
            colors.Add(new OutfitColors() { color = new Color(191, 0, 0) });

            //Paleta 4
            colors.Add(new OutfitColors() { color = new Color(145, 145, 145) });
            colors.Add(new OutfitColors() { color = new Color(191, 106, 63) });
            colors.Add(new OutfitColors() { color = new Color(191, 148, 63) });
            colors.Add(new OutfitColors() { color = new Color(191, 191, 63) });
            colors.Add(new OutfitColors() { color = new Color(148, 191, 63) });
            colors.Add(new OutfitColors() { color = new Color(106, 191, 63) });
            colors.Add(new OutfitColors() { color = new Color(63, 191, 63) });
            colors.Add(new OutfitColors() { color = new Color(63, 191, 106) });
            colors.Add(new OutfitColors() { color = new Color(63, 191, 148) });
            colors.Add(new OutfitColors() { color = new Color(63, 191, 191) });
            colors.Add(new OutfitColors() { color = new Color(63, 148, 191) });
            colors.Add(new OutfitColors() { color = new Color(63, 106, 191) });
            colors.Add(new OutfitColors() { color = new Color(63, 63, 191) });
            colors.Add(new OutfitColors() { color = new Color(106, 63, 191) });
            colors.Add(new OutfitColors() { color = new Color(148, 63, 191) });
            colors.Add(new OutfitColors() { color = new Color(191, 63, 191) });
            colors.Add(new OutfitColors() { color = new Color(191, 63, 148) });
            colors.Add(new OutfitColors() { color = new Color(191, 63, 106) });
            colors.Add(new OutfitColors() { color = new Color(191, 63, 63) });

            //Paleta 5
            colors.Add(new OutfitColors() { color = new Color(109, 109, 109) });
            colors.Add(new OutfitColors() { color = new Color(255, 85, 0) });
            colors.Add(new OutfitColors() { color = new Color(255, 170, 0) });
            colors.Add(new OutfitColors() { color = new Color(255, 255, 0) });
            colors.Add(new OutfitColors() { color = new Color(169, 255, 0) });
            colors.Add(new OutfitColors() { color = new Color(84, 255, 0) });
            colors.Add(new OutfitColors() { color = new Color(0, 255, 0) });
            colors.Add(new OutfitColors() { color = new Color(0, 255, 85) });
            colors.Add(new OutfitColors() { color = new Color(0, 255, 170) });
            colors.Add(new OutfitColors() { color = new Color(0, 255, 255) });
            colors.Add(new OutfitColors() { color = new Color(0, 169, 255) });
            colors.Add(new OutfitColors() { color = new Color(0, 85, 255) });
            colors.Add(new OutfitColors() { color = new Color(0, 0, 255) });
            colors.Add(new OutfitColors() { color = new Color(84, 0, 255) });
            colors.Add(new OutfitColors() { color = new Color(170, 0, 255) });
            colors.Add(new OutfitColors() { color = new Color(254, 0, 255) });
            colors.Add(new OutfitColors() { color = new Color(255, 0, 169) });
            colors.Add(new OutfitColors() { color = new Color(255, 0, 85) });
            colors.Add(new OutfitColors() { color = new Color(255, 0, 0) });

            //Paleta 6
            colors.Add(new OutfitColors() { color = new Color(72, 72, 72) });
            colors.Add(new OutfitColors() { color = new Color(191, 63, 0) });
            colors.Add(new OutfitColors() { color = new Color(191, 127, 0) });
            colors.Add(new OutfitColors() { color = new Color(191, 191, 0) });
            colors.Add(new OutfitColors() { color = new Color(127, 191, 0) });
            colors.Add(new OutfitColors() { color = new Color(63, 191, 0) });
            colors.Add(new OutfitColors() { color = new Color(0, 191, 0) });
            colors.Add(new OutfitColors() { color = new Color(0, 191, 63) });
            colors.Add(new OutfitColors() { color = new Color(0, 191, 127) });
            colors.Add(new OutfitColors() { color = new Color(0, 191, 191) });
            colors.Add(new OutfitColors() { color = new Color(0, 127, 191) });
            colors.Add(new OutfitColors() { color = new Color(0, 63, 191) });
            colors.Add(new OutfitColors() { color = new Color(0, 0, 191) });
            colors.Add(new OutfitColors() { color = new Color(63, 0, 191) });
            colors.Add(new OutfitColors() { color = new Color(127, 0, 191) });
            colors.Add(new OutfitColors() { color = new Color(191, 0, 191) });
            colors.Add(new OutfitColors() { color = new Color(191, 0, 127) });
            colors.Add(new OutfitColors() { color = new Color(191, 0, 63) });
            colors.Add(new OutfitColors() { color = new Color(191, 0, 0) });

            //Paleta 7
            colors.Add(new OutfitColors() { color = new Color(36, 36, 36) });
            colors.Add(new OutfitColors() { color = new Color(127, 42, 0) });
            colors.Add(new OutfitColors() { color = new Color(127, 85, 0) });
            colors.Add(new OutfitColors() { color = new Color(127, 127, 0) });
            colors.Add(new OutfitColors() { color = new Color(84, 127, 0) });
            colors.Add(new OutfitColors() { color = new Color(42, 127, 0) });
            colors.Add(new OutfitColors() { color = new Color(0, 127, 0) });
            colors.Add(new OutfitColors() { color = new Color(0, 127, 42) });
            colors.Add(new OutfitColors() { color = new Color(0, 127, 85) });
            colors.Add(new OutfitColors() { color = new Color(0, 127, 127) });
            colors.Add(new OutfitColors() { color = new Color(0, 84, 127) });
            colors.Add(new OutfitColors() { color = new Color(0, 42, 127) });
            colors.Add(new OutfitColors() { color = new Color(0, 0, 127) });
            colors.Add(new OutfitColors() { color = new Color(42, 0, 127) });
            colors.Add(new OutfitColors() { color = new Color(85, 0, 127) });
            colors.Add(new OutfitColors() { color = new Color(127, 0, 127) });
            colors.Add(new OutfitColors() { color = new Color(127, 0, 84) });
            colors.Add(new OutfitColors() { color = new Color(127, 0, 42) });
            colors.Add(new OutfitColors() { color = new Color(127, 0, 0) });

            //Paleta 7
            colors.Add(new OutfitColors() { color = new Color(36, 36, 36) });
            colors.Add(new OutfitColors() { color = new Color(127, 42, 0) });
            colors.Add(new OutfitColors() { color = new Color(127, 85, 0) });
            colors.Add(new OutfitColors() { color = new Color(127, 127, 0) });
            colors.Add(new OutfitColors() { color = new Color(84, 127, 0) });
            colors.Add(new OutfitColors() { color = new Color(42, 127, 0) });
            colors.Add(new OutfitColors() { color = new Color(0, 127, 0) });
            colors.Add(new OutfitColors() { color = new Color(0, 127, 42) });
            colors.Add(new OutfitColors() { color = new Color(0, 127, 85) });
            colors.Add(new OutfitColors() { color = new Color(0, 127, 127) });
            colors.Add(new OutfitColors() { color = new Color(0, 84, 127) });
            colors.Add(new OutfitColors() { color = new Color(0, 42, 127) });
            colors.Add(new OutfitColors() { color = new Color(0, 0, 127) });
            colors.Add(new OutfitColors() { color = new Color(42, 0, 127) });
            colors.Add(new OutfitColors() { color = new Color(85, 0, 127) });
            colors.Add(new OutfitColors() { color = new Color(127, 0, 127) });
            colors.Add(new OutfitColors() { color = new Color(127, 0, 84) });
            colors.Add(new OutfitColors() { color = new Color(127, 0, 42) });
            colors.Add(new OutfitColors() { color = new Color(127, 0, 0) });
        }
    }

}
