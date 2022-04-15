using System;
using System.Collections.Generic;
using System.Text;
using TMFormat.Framework.Creatures;
using TMFormat.Framework.Enums;

namespace TMFormat.Framework.Extentions
{
    public static class CreaturelExt
    {
        public static int onNextExperience(this ICreature creature)
        {
            int level = (creature.level + 1);
            int experience = (25 * level * level - 25 * level);
            return experience;
        }

        public static bool onTargetRange(this ICreature creature, ICreature target, bool FloorIgnore = false)
        {
            if (target != null)
            {
                if (!target.is_dead)
                {
                    if (FloorIgnore)
                    {
                        if (creature.pos_z != target.pos_z)
                        {
                            var resultX = Math.Abs(creature.pos_x - target.pos_x);
                            var resultY = Math.Abs(creature.pos_y - target.pos_y);

                            if (resultX <= 8 && resultY <= 8)
                            {
                                target.isTarget = false;
                                return true;
                            }
                        }
                    }
                    else
                    {
                        if (creature != null)
                        {
                            if (creature.pos_z == target.pos_z) //Si estan en el mismo piso.
                            {
                                var resultX = Math.Abs(creature.pos_x - target.pos_x);
                                var resultY = Math.Abs(creature.pos_y - target.pos_y);
                                if (resultX <= 8 && resultY <= 8)
                                {
                                    target.isTarget = true;
                                    return true;
                                }
                            }
                        }
                    }
                }
                target.isTarget = false;
            }
            return false;
        }

        public static bool isOnScreen(this ICreature creature, ICreature targe)
        {
            if (creature.pos_z == targe.pos_z)
            {
                var resultX = Math.Abs(creature.pos_x - targe.pos_x);
                var resultY = Math.Abs(creature.pos_y - targe.pos_y);

                if (resultX <= 10 && resultY <= 10) //Accion solo si esta en pantalla.
                {
                    return true;
                }
            }
            return false;
        }

        /*
        public static bool onTargetMeleeAtack(this EntityBase creature, EntityBase target)
        {
            if (target != null)
            {
                if (target.isTarget)
                {
                    if (creature.pos_z == target.pos_z) //Si estan en el mismo piso.
                    {
                        var resultX = Math.Abs(creature.pos_x - target.pos_x);
                        var resultY = Math.Abs(creature.pos_y - target.pos_y);
                        if (resultX <= 1 && resultY <= 1)
                        {
                            if (!creature.is_attack)
                            {
                                creature.is_attack = true;
                                target.onDamage(new Random().Next(0, 3));
                                creature.onTargetDir(target);
                            }
                            return true;
                        }
                    }
                }
            }
            return false;
        }
        */

        public static void onTargetDir(this ICreature creature, ICreature target)
        {
            if (creature.isCreature)
            {
                if (!creature.is_dead)
                {
                    if (!creature.is_walking)
                    {
                        if (creature.pos_y < target.pos_y)
                        {
                            creature.direction = (int)PlayerDir.South;
                        }
                        if (creature.pos_x > target.pos_x)
                        {
                            creature.direction = (int)PlayerDir.West;
                        }
                        if (creature.pos_y > target.pos_y)
                        {
                            creature.direction = (int)PlayerDir.North;
                        }
                        if (creature.pos_x < target.pos_x)
                        {
                            creature.direction = (int)PlayerDir.East;
                        }
                    }
                }
            }
        }

        public static bool onCreatureRange(this ICreature creature)
        {
            var resultX = Math.Abs(creature.pos_x - TMInstance.Map.player.pos_x);
            var resultY = Math.Abs(creature.pos_y - TMInstance.Map.player.pos_y);

            if (resultX <= 10 && resultY <= 10)
            {
                bool isDungeon = (creature.pos_z > TMInstance.Map.FloorDefault);

                if (isDungeon && TMInstance.Map.isDungeon)
                {
                    return true;
                }
                if ((int)creature.pos_z <= TMInstance.Map.FloorDefault)
                {
                    return true;
                }
            }
            return false;
        }

    }
}
