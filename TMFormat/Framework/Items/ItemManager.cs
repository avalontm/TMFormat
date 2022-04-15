using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TMFormat.Formats;
using TMFormat.Helpers;

namespace TMFormat.Framework.Items
{
    public static class ItemManager
    {
        public static List<TMSprite> Items = new List<TMSprite>();

        public static bool Init(string _file)
        {
            Items = TMItem.Load(_file).ToSprites();

            if (Items != null)
            {
                if (Items.Count > 0)
                {
                    return true;
                }
            }

            return false;
        }

        public static TMSprite GetItem(int item_id)
        {
            return Items.Where(x => x.Id == item_id).FirstOrDefault();
        }

        public static TMSprite GetItem(string item_name)
        {
            return Items.Where(x => x.Name.ToUpper() == item_name.ToUpper()).FirstOrDefault();
        }
    }
}
