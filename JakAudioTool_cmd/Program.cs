using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using VagDirLib;

namespace JakAudioTool_cmd
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("///////////////////////////////////////////////////////////");
            Console.WriteLine("//////////////// Welcome to the Jak Audio Tool! ///////////");
            Console.WriteLine("///////////////////////////////////////////////////////////");
            Console.WriteLine();

            while (true)
            {
                Console.WriteLine("Please choose what you want to do:");
                Console.WriteLine("0: Exit application.");
                Console.WriteLine("1: Print VAGDIR data to console.");
                Console.WriteLine("2: Print VAGDIR data to text file.");
                Console.WriteLine("3: Build VAGDIR based on text file.");
                Console.WriteLine(": Unpack VAGWAD file. (todo)");
                Console.WriteLine(": Repack VAGWAD file. (todo)");
                Console.WriteLine(": Rebuild VAG directory based on folder. (todo)");
                Console.Write("Your choice: ");
                string choice = Console.ReadLine();

                switch (choice)
                {
                    case "": // Use this to return to menu nicely.
                        {
                            Console.Clear();
                            break;
                        }
                    case "0":
                        {
                            Environment.Exit(0);
                            break;
                        }
                    case "1":
                        {
                            string path;
                            bool good = false;
                            do
                            {
                                Console.Write("Path to VAGDIR file: ");
                                path = Console.ReadLine();
                                if (File.Exists(path)) good = true;
                                else
                                {
                                    Console.Write("File doesn't exist. Write 'q' if you want to turn back, write anything else if you want to continue: ");
                                    string reply = Console.ReadLine();
                                    if (reply == "q") goto case "";
                                }
                            } while (!good);

                            Console.Clear();

                            VagDir v = new VagDir(path);

                            Console.WriteLine($"EntryCount: {v.Entries.Count}");
                            foreach (var item in v.Entries)
                            {
                                Console.WriteLine(item.ToString());
                            }

                            Console.ReadLine();
                            Console.Clear();
                            break;
                        }
                    case "2":
                        {
                            string path;
                            string outpath;
                            bool good = false;
                            do
                            {
                                Console.Write("Path to VAGDIR file: ");
                                path = Console.ReadLine();
                                if (File.Exists(path)) good = true;
                                else
                                {
                                    Console.Write("File doesn't exist. Write 'q' if you want to turn back, write anything else if you want to continue: ");
                                    string reply = Console.ReadLine();
                                    if (reply == "q") goto case "";
                                }
                            } while (!good);

                            Console.Write("Path to output file (.txt is appended): ");
                            outpath = Console.ReadLine();

                            bool simple = false;
                            Console.Write("Do you want simple output? Write 'y' if yes, anything else if no: ");
                            string simplereply = Console.ReadLine();
                            if (simplereply == "y") simple = true; ;

                            VagDir v = new VagDir(path);

                            try
                            {
                                v.GenerateTextFile(outpath + ".txt", simple);
                            }
                            catch
                            {
                                Console.WriteLine("There was an error during writing to output file. The file may not be complete.");
                            }
                            finally
                            {
                                Console.WriteLine("Finished writing to file.");
                            }

                            Console.ReadLine();
                            Console.Clear();
                            break;
                        }
                    case "3":
                        {
                            string path;
                            string outpath;
                            bool good = false;
                            do
                            {
                                Console.Write("Path to input file: ");
                                path = Console.ReadLine();
                                if (File.Exists(path)) good = true;
                                else
                                {
                                    Console.Write("File doesn't exist. Write 'q' if you want to turn back, write anything else if you want to continue: ");
                                    string reply = Console.ReadLine();
                                    if (reply == "q") goto case "";
                                }
                            } while (!good);

                            Console.Write("Path to output file: ");
                            outpath = Console.ReadLine();

                            VagDir v = new VagDir(path, textsource: true);

                            try
                            {
                                v.GenerateVagDirFile(outpath);
                            }
                            catch
                            {
                                Console.WriteLine("There was an error during writing to output file. The file may not be complete.");
                            }
                            finally
                            {
                                Console.WriteLine("Finished writing to file.");
                            }

                            Console.ReadLine();
                            Console.Clear();
                            break;
                        }
                    default:
                        {
                            Console.WriteLine("The selected option does not exist.");
                            break;
                        }
                }

                Console.WriteLine();
            }
        }
    }
}
