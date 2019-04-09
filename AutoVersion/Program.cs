using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoVersion
{
    static class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("-= Hona's Source Map Auto Versioning Tool =-");
            Console.WriteLine();
            if (args.Length == 0)
            {
                Console.WriteLine("Missing parameters, make sure you drag a map onto a .bat file");
                Console.ReadLine();
                return;
            }
            if (args[0] == null)
            {
                Console.WriteLine("Missing map path, have you clicked and dragged the map onto the script?");
                Console.ReadLine();
                return;
            }
            if (args[1] == null)
            {
                Console.WriteLine("Missing map versioning value, are you using included scripts?");
                Console.ReadLine();
                return;
            }

            var oldMapName = args[0].Split('\\').Last();
            Console.WriteLine($" - Automatically versioning {oldMapName}");
            var mapName = oldMapName.Remove(oldMapName.Length-4, 4);

            var folder = string.Join("\\", args[0].Split('\\').Reverse().Skip(1).Reverse())+"\\";
            var version = args[1];
            Console.WriteLine($" - Using '{version}' suffix");

            var partialMapName = mapName + "_" + version;

            var files = Directory.GetFiles(folder);
            var versions = new List<int>();
            foreach (var file in files)
            {
                var trimmedFile = string.Join("", file.Skip(folder.Length));
                if (trimmedFile.StartsWith(partialMapName) && trimmedFile.EndsWith(".bsp"))
                {
                    trimmedFile = trimmedFile.Remove(0, partialMapName.Length);
                    trimmedFile = trimmedFile.Remove(trimmedFile.Length - 4, 4);
                    if(int.TryParse(trimmedFile, out int result))
                    {
                        versions.Add(result);
                    }
                    
                }
            }

            var newVersion = 1;
            if (versions.Count != 0)
            {
                newVersion = versions.OrderBy(x => x).Last() + 1;
            }

            Console.WriteLine($" - Using _{version}{newVersion}");
            File.Move(args[0], folder + "\\" + partialMapName + newVersion + ".bsp");
            Console.WriteLine($" - Moved {oldMapName} to {partialMapName + newVersion + ".bsp"}");

        }
    }
}
