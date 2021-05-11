﻿using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;
using TMFormat.Attributes;
using TMFormat.Formats;

namespace TMFormat.Helpers
{
   public static class Extentions
    {
        public static bool isNotReader(this FieldInfo info)
        {
            string[] fields = info.Name.Substring(1).Split('>');
            string name = fields[0];
            bool notReader = false;
            MemberInfo memberInfo = typeof(TMItem).GetMember(name)?[0];

            if (memberInfo != null)
            {
                object[] attributes = Attribute.GetCustomAttributes(memberInfo, true);

                foreach (NotReaderAttribute attr in attributes)
                {
                    if (attr.NotReader)
                    {
                        notReader = true;
                        break;
                    }
                }
            }

            return notReader;
        }

        public static List<TMSprite> ToSprites(this List<TMItem> items)
        {
            List<TMSprite> sprites = new List<TMSprite>();

            foreach (var item in items)
            {
                TMSprite spr = new TMSprite();
                spr.Copy(item);
                sprites.Add(spr);
            }
            return sprites;
        }
    }
}
