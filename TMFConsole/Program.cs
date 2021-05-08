﻿using System;
using System.IO;
using TMFormat;
using TMFormat.Formats;
using TMFormat.Models;

namespace TMFConsole
{
    public class Program
    {
        static string root = System.IO.Directory.GetCurrentDirectory();
        static void Main(string[] args)
        {
            Console.WriteLine("Iniciando...");
            TMInstance.Init(true);

            TMCreature creature = TMCreature.Load($"{Path.Combine(root, "chr_128.abochar")}");

            if (creature == null)
            {
                Console.WriteLine($"[creature] can't load.");
                return;
            }

            Console.WriteLine($"[creature] {creature.Name}");
            creature.SaveToImage(creature.Dirs[0].Animations[0].Texture1, Path.Combine(root, "texture_1.png"));
        }
    }
}