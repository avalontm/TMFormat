using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using TMFormat.Models;

namespace TMFormat.Formats
{
    public class TMCreatureProperties
    {
        public byte[] Texture1 { set; get; }
        public byte[] Texture2 { set; get; }
        public byte[] Texture3 { set; get; }
        public byte[] Texture4 { set; get; }

        public byte[] Mask1 { set; get; }
        public byte[] Mask2 { set; get; }
        public byte[] Mask3 { set; get; }
        public byte[] Mask4 { set; get; }
    }

    public class TMCreatureLoot
    {
        public int id { set; get; }
        public int count { set; get; }
        public int probability { set; get; }
    }

    public class TMCreatureAnimations
    {
        public List<TMCreatureProperties> Animations { set; get; }

        public TMCreatureAnimations()
        {
            Animations = new List<TMCreatureProperties>();
            TMCreatureProperties anim = new TMCreatureProperties();
            Animations.Add(anim);

            anim = new TMCreatureProperties();
            Animations.Add(anim);

            anim = new TMCreatureProperties();
            Animations.Add(anim);
        }
    }

    public class TMCreature
    {
        public string Name { set; get; }
        public int Type { set; get; }
        public float Speed { set; get; }
        public bool isAgressive { set; get; }
        public int Experience { set; get; }
        public int Attack { set; get; }
        public int Defence { set; get; }
        public int Level { set; get; }
        public int Heal { set; get; }
        public bool isOffset { set; get; }
        public bool useSpell { set; get; }
        public int TimeSpawn { set; get; }
        public bool UseDistance { set; get; }
        public List<TMCreatureAnimations> Dirs { set; get; }
        public List<TMCreatureLoot> Loots { set; get; }

        public TMCreature()
        {
            Dirs = new List<TMCreatureAnimations>();
            Loots = new List<TMCreatureLoot>();

            TMCreatureAnimations _dir = new TMCreatureAnimations();
            Dirs.Add(_dir);

            _dir = new TMCreatureAnimations();
            Dirs.Add(_dir);

            _dir = new TMCreatureAnimations();
            Dirs.Add(_dir);

            _dir = new TMCreatureAnimations();
            Dirs.Add(_dir);

            _dir = new TMCreatureAnimations();
            Dirs.Add(_dir);
        }

        public static TMCreature Load(string filename)
        {
            byte[] readBytes = null;

            try
            {
                if (Instance.Content == null)
                {
                    if (!File.Exists(filename))
                    {
                        Console.WriteLine($"[CreatureBase] NotFound! => {filename}");
                        return null;
                    }

                    readBytes = File.ReadAllBytes(filename);
                    return CreatureReader.Deserialize(readBytes);
                }

                readBytes = Instance.Content.Load<byte[]>(filename);
                return CreatureReader.Deserialize(readBytes);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[TMCreature] [LOAD] => {ex}");
                return null;
            }
        }
    }
}
