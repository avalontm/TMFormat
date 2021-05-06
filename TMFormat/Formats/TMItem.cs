using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;
using TMFormat.Attributes;
using TMFormat.Models;

namespace TMFormat.Formats
{
    public class TMItemTexture
    {
        public byte[] Texture1 { set; get; }
        public byte[] Texture2 { set; get; }
        public byte[] Texture3 { set; get; }
        public byte[] Texture4 { set; get; }
    }

    public class TMItem
    {
        public List<TMItemTexture> Textures { set; get; }
        public int Id { set; get; }
        public string Name { set; get; }
        public int Type { set; get; }
        public bool Moveable { set; get; }
        public bool Block { set; get; }
        public bool Block2 { set; get; }
        public bool Block3 { set; get; }
        public bool Block4 { set; get; }

        public bool isTop1 { set; get; }
        public bool isTop2 { set; get; }
        public bool isTop3 { set; get; }
        public bool isTop4 { set; get; }

        public bool Use { set; get; }
        public int Field { set; get; }
        public int EffectID { set; get; }
        public double Speed { set; get; }
        public int Container { set; get; }
        public double AniSpeed { set; get; }
        public int Dir { set; get; }
        public bool isDown { set; get; }
        public bool isWindow { set; get; }
        public bool isAnimation { set; get; }
        public bool isLight { set; get; }
        public bool isOffset { set; get; }
        public bool isReader { set; get; }
        public bool isEquip { set; get; }
        public int EquipSlot { set; get; }
        public ItemColor LightColor { set; get; }
        public int WeaponDistance { set; get; }
        public int WeaponValue { set; get; }
        public Vector3 Destine { set; get; }

        [NotReader]
        public string Reader { get; set; }
        [NotReader]
        public int IndexAnimation { set; get; }
        [NotReader]
        public float TimeAnimation { set; get; }

        public TMItem()
        {
            Textures = new List<TMItemTexture>();
        }
    }
}
