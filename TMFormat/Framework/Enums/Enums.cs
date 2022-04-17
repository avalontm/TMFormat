using System;
using System.Collections.Generic;
using System.Text;

namespace TMFormat.Framework.Enums
{
    public enum PlayerSex
    {
        Female,
        Male
    }

    public enum PlayerDir
    {
        North,
        East,
        South,
        West,
        Center,
    }
    public enum CreatureType
    {
        CREATURE = 0,
        NPC,
        PLAYER,
        GAMEMASTER,
    }

    public enum PlayerStatus
    {
        None = 0,
        Dead = 4,
    }

    public enum OutfitStatus
    {
        Head = 0,
        Body,
        Legs,
        Feets,
    }

    public enum ItemType
    {
        Tile = 0,
        Border,
        Field,
        Item,
        Tree,
        Wall,
        Stair,
        Door,
    }

    public enum ItemField
    {
        None,
        Stay,
        Teleport,
        Fire,
        Poison,
        Electricity,
    }
}
