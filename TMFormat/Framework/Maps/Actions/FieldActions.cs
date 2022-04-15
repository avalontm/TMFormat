using System;
using System.Collections.Generic;
using System.Text;
using TMFormat.Enums;
using TMFormat.Formats;
using TMFormat.Framework.Creatures;

namespace TMFormat.Framework.Maps.Actions
{
    public static class FieldActions
    {
        public static void onTileField(this ICreature player, int x, int y, int z, TMItem item)
        {
            switch ((TypeField)item.Field)
            {
                case TypeField.Stay:
                    player.onFieldStay(x, y, z, item);
                    break;
            }
        }


        public static bool onFieldStay(this ICreature player, int x, int y, int z, TMItem item)
        {
            if ((TypeField)item.Field == TypeField.Stay)
            {
                if (player.pos_x == x && player.pos_y == y && player.pos_z == z)
                {
                    item.IndexAnimation = 1;
                    return true;
                }

                for (int i = 0; i < TMInstance.Map.players.Count; i++)
                {
                    if (TMInstance.Map.players[i].pos_x == x && TMInstance.Map.players[i].pos_y == y && TMInstance.Map.players[i].pos_z == z)
                    {
                        item.IndexAnimation = 1;
                        return true;
                    }
                }

                item.IndexAnimation = 0;
            }
            return false;
        }
    }
}
