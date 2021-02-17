using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VagDirLib;

namespace JakAudioTool_cmd
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Welcome to the Jak VAGDIR Tool!");

            while (true)
            {
                Console.WriteLine("Please choose what you want to do:");
                Console.WriteLine("0: Exit application.");
                Console.WriteLine("1: Dump VAGDIR data to text file. (todo)");
                Console.WriteLine("2: Build VAGDIR based on text file. (todo)");
                Console.WriteLine("3: Unpack VAGWAD file. (todo)");
                Console.WriteLine("4: Repack VAGWAD file. (todo)");
                Console.WriteLine("5: Rebuild VAG directory based on folder. (todo)");
                Console.Write("Your choice: ");
                string choice = Console.ReadLine();

                switch (choice)
                {
                    case "0":
                        {
                            Environment.Exit(0);
                            break;
                        }
                    case "1":
                        {
                            Console.WriteLine("Not implemented.");
                            break;
                        }
                    case "2":
                        {
                            Console.WriteLine("Not implemented.");
                            break;
                        }
                    case "3":
                        {
                            Console.WriteLine("Not implemented.");
                            break;
                        }
                    case "4":
                        {
                            Console.WriteLine("Not implemented.");
                            break;
                        }
                    case "5":
                        {
                            Console.WriteLine("Not implemented.");
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
