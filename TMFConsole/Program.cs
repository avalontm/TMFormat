using System;
using System.IO;
using TMFormat;
using TMFormat.Formats;
using TMFormat.Helpers;
using TMFormat.Models;

namespace TMFConsole
{
    public class Program
    {
        static string root = System.IO.Directory.GetCurrentDirectory();
        static void Main(string[] args)
        {
            Console.WriteLine("Iniciando...");
            TMInstance.Init();

            TMCreature creature = TMCreature.Load($"{Path.Combine(root, "orc.tmc")}");

            if (creature == null)
            {
                Console.WriteLine($"[creature] can't load.");
                return;
            }

            Console.WriteLine($"[creature] {creature.name}");

            for (int d = 0; d < creature.dirs.Count; d++)
            {
                for (int s = 0; s < creature.dirs[d].sprites.Count; s++)
                {
                    for (int i = 0; i < 4; i++)
                    {
                        TMImageHelper.SaveToImage(creature.dirs[d].sprites[s].textures[i], Path.Combine(root, "textures", $"text_{d}_{s}_{i}"));
                    }

                    for (int a = 0; a < 4; a++)
                    {
                        TMImageHelper.SaveToImage(creature.dirs[d].sprites[s].masks[a], Path.Combine(root, "textures", $"text_{d}_{a}"));
                    }
                }
            }

            if (string.IsNullOrEmpty(creature.name))
            {
                creature.name = "creature test";
            }
            Console.WriteLine($"[Save] {creature.name}");

            creature.SaveToFile(Path.Combine(root, "chr_orc.tmc"));
        }
    }
}
