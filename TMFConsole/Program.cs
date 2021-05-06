using System;
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

            TMCreature creature = TMCreature.Load($"{Path.Combine(root, "chr_test.tmc")}");

            if (creature == null)
            {
                Console.WriteLine($"[creature] can't load.");
                return;
            }

            Console.WriteLine($"[creature] {creature.name}");
  
            for (int d = 0; d < creature.dirs.Count; d++)
            {
                for (int a = 0; a < creature.dirs[d].textures.Count; a++)
                {
                    TMCreature.SaveToImage(creature.dirs[d].textures[a], Path.Combine(root, "textures", $"text_dir_{d}_{a}"));
                }

                for (int a = 0; a < creature.dirs[d].masks.Count; a++)
                {
                    TMCreature.SaveToImage(creature.dirs[d].masks[a], Path.Combine(root, "textures", $"text_dir_{d}_{a}"));
                }
            }

            if (string.IsNullOrEmpty(creature.name))
            {
                creature.name = "creature test";
            }
            Console.WriteLine($"[Save] {creature.name}");

            creature.SaveToFile(Path.Combine(root, "chr_test1.tmc"));
        }
    }
}
