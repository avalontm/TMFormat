using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TMFormat.Enums;
using TMFormat.Formats;
using TMFormat.Framework.Creatures;
using TMFormat.Framework.Enums;

namespace TMFormat.Framework.Maps.Actions
{
    public static class FieldActions
    {
        public static void onStair(this ICreature player)
        {
            if (player.type != CreatureType.PLAYER && player.type != CreatureType.GAMEMASTER)
            {
                return;
            }

            var item = TMInstance.Map.MapBase.Floors[player.pos_z][player.pos_x, player.pos_y].items?.FirstOrDefault();

            if (item == null)
            {
                return;
            }

            if (item.Type == (int)ItemType.Stair)
            {
                if (item.Use) //Si el item debe usar una accion.
                {
                    return;
                }

                player.direction = item.Dir;
                //player.paths.Clear();

                if (item.isDown) //Hacia Abajo.
                {
                    switch ((PlayerDir)item.Dir)
                    {
                        case PlayerDir.North:
                            player.ToPosition(new VectorInt3((player.pos_x), (player.pos_y - 1), (player.pos_z + 1)));
                            break;
                        case PlayerDir.East:
                            player.ToPosition(new VectorInt3((player.pos_x + 1), (player.pos_y), (player.pos_z + 1)));
                            break;
                        case PlayerDir.South:
                            player.ToPosition(new VectorInt3((player.pos_x), (player.pos_y + 1), (player.pos_z + 1)));
                            break;
                        case PlayerDir.West:
                            player.ToPosition(new VectorInt3((player.pos_x - 1), (player.pos_y), (player.pos_z + 1)));
                            break;
                        case PlayerDir.Center:
                            player.ToPosition(new VectorInt3((player.pos_x), (player.pos_y), (player.pos_z + 1)));
                            break;
                    }
                }
                else //Hacia Arriba.
                {
                    switch ((PlayerDir)item.Dir)
                    {
                        case PlayerDir.North:
                            player.ToPosition(new VectorInt3((player.pos_x), (player.pos_y - 1), (player.pos_z - 1)));
                            break;
                        case PlayerDir.East:
                            player.ToPosition(new VectorInt3((player.pos_x + 1), (player.pos_y), (player.pos_z - 1)));
                            break;
                        case PlayerDir.South:
                            player.ToPosition(new VectorInt3((player.pos_x), (player.pos_y + 1), (player.pos_z + 1)));
                            break;
                        case PlayerDir.West:
                            player.ToPosition(new VectorInt3((player.pos_x - 1), (player.pos_y), (player.pos_z - 1)));
                            break;
                        case PlayerDir.Center:
                            player.ToPosition(new VectorInt3((player.pos_x), (player.pos_y), (player.pos_z - 1)));
                            break;
                    }
                }
            }
        }

        public static void UseStair(this ICreature player, int _id, VectorInt3 position)
        {
            if (player.type != CreatureType.PLAYER && player.type != CreatureType.GAMEMASTER)
            {
                return;
            }

            var item = TMInstance.Map.MapBase.Floors[position.Z][position.X, position.Y].items?.FirstOrDefault();

            if (item == null)
            {
                return;
            }

            if (item.Id != _id)
            {
                return;
            }

            if (item.Type == (int)ItemType.Stair)
            {
                if (!item.Use) //Si el item debe usar una accion.
                {
                    return;
                }

                player.direction = item.Dir;

                if (item.isDown) //Hacia Abajo.
                {
                    switch ((PlayerDir)item.Dir)
                    {
                        case PlayerDir.North:
                            player.ToPosition(new VectorInt3((position.X), (position.Y - 1), (position.Z + 1)));
                            break;
                        case PlayerDir.East:
                            player.ToPosition(new VectorInt3((position.X + 1), (position.Y), (position.Z + 1)));
                            break;
                        case PlayerDir.South:
                            player.ToPosition(new VectorInt3((position.X), (position.Y + 1), (position.Z + 1)));
                            break;
                        case PlayerDir.West:
                            player.ToPosition(new VectorInt3((position.X - 1), (position.Y), (position.Z + 1)));
                            break;
                        case PlayerDir.Center:
                            player.ToPosition(new VectorInt3((position.X), (position.Y), (position.Z + 1)));
                            break;
                    }
                }
                else //Hacia Arriba.
                {
                    switch ((PlayerDir)item.Dir)
                    {
                        case PlayerDir.North:
                            player.ToPosition(new VectorInt3((position.X), (position.Y - 1), (position.Z - 1)));
                            break;
                        case PlayerDir.East:
                            player.ToPosition(new VectorInt3((position.X + 1), (position.Y), (position.Z - 1)));
                            break;
                        case PlayerDir.South:
                            player.ToPosition(new VectorInt3((position.X), (position.Y + 1), (position.Z - 1)));
                            break;
                        case PlayerDir.West:
                            player.ToPosition(new VectorInt3((position.X - 1), (position.Y), (position.Z - 1)));
                            break;
                        case PlayerDir.Center:
                            player.ToPosition(new VectorInt3((position.X), (position.Y), (position.Z - 1)));
                            break;
                    }
                }
            }
        }

        public static void onTeleport(this ICreature player, TMItem item)
        {
            if (player.type != CreatureType.PLAYER && player.type != CreatureType.GAMEMASTER)
            {
                return;
            }

            if (item.Type != (int)ItemType.Field)
            {
                return;
            }

            if (item.Field == (int)ItemField.Teleport)
            {
                player.ToPosition(new VectorInt3((int)item.Destine.X, (int)item.Destine.Y, (int)item.Destine.Z), true);
            }

        }
        public static void onTileField(this ICreature player, int x, int y, int z, TMItem item)
        {
            switch ((TypeField)item.Field)
            {
                case TypeField.Stay:
                    player.onFieldStay(x, y, z, item);
                    break;
                case TypeField.Teleport:
                    player.onTeleport(item);
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
