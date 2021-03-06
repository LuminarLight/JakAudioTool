﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace VagDirLib
{
    public class VagDir
    {
        public string Path { get; set; }
        public int Version { get; set; }

        //public int EntryCount { get; set; } // The List below has its own count property, we are going to use that.
        public List<VagDirEntry> Entries { get; set; } = new List<VagDirEntry>();

        public VagDir(string path, int version, bool textsource = false)
        {
            Path = path;
            Version = version;

            if (!textsource) ReadVagDirFile();
            else ReadTextFile();
        }

        private void ReadVagDirFile() // All the file-related operations will return an "error code" integer, in the future.
        {
            switch (Version)
            {
                case 1:
                    {
                        using (BinaryReader br = new BinaryReader(File.Open(Path, FileMode.Open)))
                        {
                            int count = br.ReadInt32();

                            for (int i = 0; i < count; i++)
                            {
                                byte[] namebytes = br.ReadBytes(8);
                                Entries.Add(new VagDirEntryV1(Encoding.UTF8.GetString(namebytes), br.ReadUInt32()));
                            }
                        }
                        break;
                    }
                case 2:
                    {
                        using (BinaryReader br = new BinaryReader(File.Open(Path, FileMode.Open)))
                        {
                            int count = br.ReadInt32();

                            br.BaseStream.Position = 0;

                            for (int i = 0; i < count; i++)
                            {
                                int stereo = br.ReadInt32();
                                byte[] namebytes = br.ReadBytes(8);
                                Entries.Add(new VagDirEntryV2(Encoding.UTF8.GetString(namebytes), br.ReadUInt32(), Convert.ToBoolean(stereo)));
                            }
                        }
                        break;
                    }
                case 3:
                    {
                        using (BinaryReader br = new BinaryReader(File.Open(Path, FileMode.Open)))
                        {
                            br.ReadBytes(8); // VGWADDIR
                            br.ReadInt32(); // 2 in Jak 3, idk
                            int count = br.ReadInt32();

                            for (int i = 0; i < count; i++)
                            {
                                byte[] namebytes = br.ReadBytes(4);
                                byte[] idk = br.ReadBytes(2);

                                Entries.Add(new VagDirEntryV3(Encoding.UTF8.GetString(namebytes), br.ReadUInt16(), true, false));
                            }
                        }
                        break;
                    }
                default:
                    {
                        break;
                    }
            }
        }

        private void ReadTextFile() // stereo bool;name (8 chars);length (base 10 integer number)
        {
            switch (Version)
            {
                case 1:
                    {
                        using (StreamReader sr = new StreamReader(Path))
                        {
                            while (!sr.EndOfStream)
                            {
                                string[] line = sr.ReadLine().Split(';');
                                Entries.Add(new VagDirEntryV1(line[1], Convert.ToUInt32(line[2])));
                            }
                        }
                        break;
                    }
                case 2:
                    {
                        using (StreamReader sr = new StreamReader(Path))
                        {
                            while (!sr.EndOfStream)
                            {
                                string[] line = sr.ReadLine().Split(';');
                                Entries.Add(new VagDirEntryV2(line[1], Convert.ToUInt32(line[2]), Convert.ToBoolean(line[0])));
                            }
                        }
                        break;
                    }
                case 3:
                    {
                        break;
                    }
                default:
                    {
                        break;
                    }
            }
        }

        public void GenerateVagDirFile(string outpath)
        {
            switch (Version)
            {
                case 1:
                    {
                        using (BinaryWriter bw = new BinaryWriter(File.OpenWrite(outpath)))
                        {
                            bw.Write(Entries.Count);
                            foreach (var item in Entries)
                            {
                                bw.Write(Encoding.UTF8.GetBytes(item.Name.PadRight(8, ' ')));
                                bw.Write(item.Location);
                            }
                        }
                        break;
                    }
                case 2:
                    {
                        using (BinaryWriter bw = new BinaryWriter(File.OpenWrite(outpath)))
                        {
                            foreach (var item in Entries)
                            {
                                bw.Write(Convert.ToInt32(item.Stereo));
                                bw.Write(Encoding.UTF8.GetBytes(item.Name.PadRight(8, ' ')));
                                bw.Write(item.Location);
                            }
                            bw.Seek(0, SeekOrigin.Begin);
                            bw.Write(Entries.Count);
                        }
                        break;
                    }
                case 3:
                    {
                        break;
                    }
                default:
                    {
                        break;
                    }
            }
        }

        public void GenerateTextFile(string outpath, bool simple)
        {
            using (StreamWriter sw = new StreamWriter(outpath))
            {
                if (!simple) sw.WriteLine($"EntryCount: {Entries.Count}");
                foreach (var item in Entries)
                {
                    sw.WriteLine(item.ToString(simple));
                }
            }
        }

        private string StringToLength(string s, int length)
        {
            if (s.Length > length) return s.Substring(0, length);
            else return s.PadRight(length);
        }
    }

    public abstract class VagDirEntry
    {
        public bool Stereo { get; set; }
        public string Name { get; set; }
        public UInt32 Location { get; set; }
        public bool InInt { get; set; }

        public override string ToString()
        {
            return ToString(false);
        }

        public abstract string ToString(bool simple);
    }

    public class VagDirEntryV1 : VagDirEntry // version 1 (TPL)
    {
        public VagDirEntryV1(string name, UInt32 location)
        {
            Name = name.Trim();
            Location = location;
            Stereo = false;
            InInt = false;
        }

        public override string ToString(bool simple)
        {
            if (simple) return $"{Stereo};{Name};{Location}";
            else return $"{Name} @ 0x{Location:X} (0x{Location * 0x800:X} in file)";
        }
    }

    // First entry is entry count (4 bytes), entries after that are 1 for stereo or 0 for mono;name (8 bytes);location (*0x800)
    // Entry count overlaps with stereoness of first entry. Weird, need to study this.
    public class VagDirEntryV2 : VagDirEntry // version 2 (II)
    {
        public VagDirEntryV2(string name, UInt32 location, bool stereo)
        {
            Name = name.Trim();
            Location = location;
            Stereo = stereo;
            InInt = false;
        }

        public override string ToString(bool simple)
        {
            if (simple) return $"{Stereo};{Name};{Location}";
            else return $"{Name} ({(Stereo ? "Stereo" : "Mono")}) @ 0x{Location:X} (0x{Location * 0x800:X} in file)";
        }
    }

    /*class VAGDIR_EntryAdvanced // Not sure how it knows whether it is in a language VAGWAD or the INT VAGWAD, but I believe it won't be hard to figure out once we know the rest.
{
    public VAGDIR_EntryAdvanced(int unknown, UInt16 unknown2, UInt16 location)
    {
        Unknown = unknown;
        Unknown2 = unknown2;
        Location = location;
    }

    public int Unknown { get; } // This could be the name but I did the math and it seems to be impossible to store a length 8 string in 4 bytes...
    public UInt16 Unknown2 { get; } // these 2 bytes seem to have some kind of pattern...
    public UInt16 Location { get; } // multiply this by 0x8000 to get the audio's position in the VAGWAD files
}*/
    // Placeholder
    public class VagDirEntryV3 : VagDirEntry // version 3 (3, maybe X?)
    {
        public VagDirEntryV3(string name, UInt32 location, bool stereo, bool inint)
        {
            Name = name.Trim();
            Location = location;
            Stereo = stereo;
            InInt = inint;
        }

        public override string ToString(bool simple)
        {
            if (simple) return $"{Stereo};{Name};{Location};{InInt}";
            else return $"{Name} ({(Stereo ? "Stereo" : "Mono")}) @ 0x{Location:X} (0x{Location * 0x8000:X} in file) | {(InInt ? "INT" : "lang")}";
        }
    }
}
