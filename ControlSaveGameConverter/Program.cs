using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace ControlSaveGameConverter
{
    class Program
    {
        static void Main(string[] args)
        {

            string user = Environment.UserName;
            string steam_path = "";
            string epic_path = String.Format("C:\\Users\\{0}\\AppData\\Local\\Remedy\\Control\\Default-Epic-User", user);
            string gog_path = String.Format("C:\\Users\\{0}\\Documents\\My Games\\Control\\Saves", user);
            string source_path = "";
            string target_path = "";
            string[] savefiles = { "global", "hub", "meta", "persistent" };
            string[] dirs;
            string overwrite = "leer";
            int i = 0;
            IDictionary<int, string> slots = new Dictionary<int, string>();

            Console.WriteLine("1: Convert from Epic to GOG\n\n2: Convert from GOG to Epic");
            string pick = Console.ReadLine();
            if (pick == "1")
            {
                Console.WriteLine("Wich saveslot do you want to transfer?\n");
                dirs = Directory.GetDirectories(epic_path, "savegame-slot-*");
                foreach (var dir in dirs)
                {
                    int idx = dir.LastIndexOf("\\");
                    string slot = dir.Substring(idx + 1);
                    i++;
                    Console.WriteLine(i + ": " + slot);
                    slots[i] = slot;
                }
                int num = Convert.ToInt32(Console.ReadLine());

                source_path = epic_path + "\\" + slots[num];
                target_path = gog_path;

                try
                {
                    File.Copy(source_path + "\\" + "global.chunk", target_path + "\\" + slots[num] + "_global");
                }
                catch (System.IO.IOException)
                {
                    Console.WriteLine("There are already control savefiles in your GOG directory!\nDo you want to overwrite them? y/n");


                    do
                    {
                        overwrite = Console.ReadLine();
                        overwrite = overwrite.ToString();
                        if (overwrite == "y" || overwrite == "n")
                        {
                            break;
                        }
                        else
                        {
                            Console.WriteLine("\nPlease type either y or n");

                        }
                    } while (overwrite != "y" || overwrite != "n");


                    if (overwrite == "y")
                    {
                        foreach (var file in savefiles)
                        {
                            File.Copy(source_path + "\\" + file + ".chunk", target_path + "\\" + slots[num] + "_" + file, true);
                        }
                    }
                    else
                    {
                        Console.WriteLine("\nFiles can't be transfered...\nThe Setup ends now");
                        Console.ReadKey();
                        Environment.Exit(0);
                    }


                    Console.WriteLine("\nConvertions and file transfer complete");

                }

                Console.ReadKey();
            }
        }
    }
}
