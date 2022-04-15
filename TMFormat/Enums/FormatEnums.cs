using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;
using TMFormat.Formats;

namespace TMFormat.Enums
{
    public enum EquipSlotType
    {
        NONE,
        HEAD,
        ARMOR,
        LEGS,
        BOOTS,
        WEAPON,
        SHIELD,
        NEACLE,
        RING,
        AMMOUNT,
        BACKPACK,
    }

    public enum TypeItem
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

    public enum TypeField
    {
        None,
        Stay,
        Teleport,
        Fire,
        Poison,
        Electricity,
    }

    public struct TilePlayer
    {
        public bool Send;
    }

    public struct MapInfo
    {
        public string Name;
        public Vector2 Size;
        public string Version;
        public string Autor;
    }

    public struct MapProperties
    {
        public bool isCreature;
        public bool isPZ;
        public bool isTop;
        public TMSprite item;
        public List<TMSprite> items;
    }
}
