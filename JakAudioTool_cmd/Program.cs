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

                            Console.WriteLine($"EntryCount: {v.EntryCount}");
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

                            Console.Write("Path to output file (+.txt): ");
                            outpath = Console.ReadLine();

                            VagDir v = new VagDir(path);

                            try
                            {
                                using (StreamWriter sw = new StreamWriter(outpath + ".txt"))
                                {
                                    sw.WriteLine($"EntryCount: {v.EntryCount}");
                                    foreach (var item in v.Entries)
                                    {
                                        sw.WriteLine(item.ToString());
                                    }
                                }
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

                            VagDir v = new VagDir(outerfill: true);

                            try
                            {
                                using (StreamReader sr = new StreamReader(path))
                                {
                                    while (!sr.EndOfStream)
                                    {
                                        string[] line = sr.ReadLine().Split(';');
                                        if (line[0].Length != 8)
                                        {
                                            Console.WriteLine("String length must always be 8. Aborting process...");
                                            goto case "";
                                        }
                                        v.Entries.Add(new VagDirEntrySimple(line[0], Convert.ToUInt32(line[1])));
                                    }
                                }
                            }
                            catch
                            {
                                Console.WriteLine("There was an error during reading the input file. There may be problems with the output file after this.");
                            }
                            v.EntryCount = v.Entries.Count;

                            try
                            {
                                using (BinaryWriter bw = new BinaryWriter(File.OpenWrite(outpath)))
                                {
                                    bw.Write(v.EntryCount);
                                    foreach (var item in v.Entries)
                                    {
                                        bw.Write(item.Name);
                                        bw.Write(item.Location);
                                    }
                                }
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
