using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace TMFormat.Framework.Loaders
{
    public static class Textures
    {
        public static Texture2D Mark;
        public static Texture2D Square;
        public static Texture2D LightMask;
        public static Texture2D Login_BG;
        public static Texture2D ic_money;
        public static Texture2D ic_beta;
        public static Texture2D ic_position;
        public static Texture2D ic_phone_land;

        public static Texture2D setHead;
        public static Texture2D setBody;
        public static Texture2D setLegs;
        public static Texture2D setFeets;

        public static Texture2D setNeacle;
        public static Texture2D setHandLeft;
        public static Texture2D setRing;

        public static Texture2D setBackpack;
        public static Texture2D setHandRight;
        public static Texture2D setAmmount;

        public static void Init()
        {
            Mark = TMInstance.Content.Load<Texture2D>("ui/mark");
            Square = TMInstance.Content.Load<Texture2D>("ui/square");
            LightMask = TMInstance.Content.Load<Texture2D>("ui/lightmask");
            Login_BG = TMInstance.Content.Load<Texture2D>("ui/screen_bg");
            ic_money = TMInstance.Content.Load<Texture2D>("ui/ic_money");
            ic_beta = TMInstance.Content.Load<Texture2D>("ui/iu_bt");
            ic_position = TMInstance.Content.Load<Texture2D>("ui/ic_pos");
            ic_phone_land = TMInstance.Content.Load<Texture2D>("ui/ic_phone_land");

            /* NO SET */
            setNeacle = TMInstance.Content.Load<Texture2D>("ui/NoAmulet");
            setHandLeft = TMInstance.Content.Load<Texture2D>("ui/NoWeapon");
            setRing = TMInstance.Content.Load<Texture2D>("ui/NoRing");

            setHead = TMInstance.Content.Load<Texture2D>("ui/NoHelmet");
            setBody = TMInstance.Content.Load<Texture2D>("ui/NoArmor");
            setLegs = TMInstance.Content.Load<Texture2D>("ui/NoLegs");
            setFeets = TMInstance.Content.Load<Texture2D>("ui/NoBoots");

            setBackpack = TMInstance.Content.Load<Texture2D>("ui/NoAmulet");
            setHandRight = TMInstance.Content.Load<Texture2D>("ui/NoShield");
            setAmmount = TMInstance.Content.Load<Texture2D>("ui/NoBelt");
        }
    }
}
