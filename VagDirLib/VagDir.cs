using System;
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

        public VagDir(string path, int version = 1, bool textsource = false)
        {
            Path = path;
            Version = version;

            if (!textsource) ReadVagDirFile();
            else ReadTextFile();
        }

        private void ReadVagDirFile() // All the file-related operations will return an "error code" integer, in the future.
        {
            using (BinaryReader br = new BinaryReader(File.Open(Path, FileMode.Open)))
            {
                int count = br.ReadInt32();

                for (int i = 0; i < count; i++)
                {
                    byte[] namebytes = br.ReadBytes(8);
                    Entries.Add(new VagDirEntrySimple(Encoding.UTF8.GetString(namebytes), br.ReadUInt32()));
                }
            }
        }

        private void ReadTextFile() // name (8 chars);length (base 10 integer number)
        {
            using (StreamReader sr = new StreamReader(Path))
            {
                while (!sr.EndOfStream)
                {
                    string[] line = sr.ReadLine().Split(';');
                    Entries.Add(new VagDirEntrySimple(line[0], Convert.ToUInt32(line[1])));
                }
            }
        }

        public void GenerateVagDirFile(string outpath)
        {
            using (BinaryWriter bw = new BinaryWriter(File.OpenWrite(outpath)))
            {
                bw.Write(Entries.Count);
                foreach (var item in Entries)
                {
                    bw.Write(Encoding.UTF8.GetBytes(item.Name));
                    bw.Write(item.Location);
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

    public abstract class VagDirEntry
    {
        public string Name { get; set; }
        public UInt32 Location { get; set; }

        public override string ToString()
        {
            return ToString(false);
        }

        public abstract string ToString(bool simple);
    }

    public class VagDirEntrySimple : VagDirEntry // version 1
    {
        public VagDirEntrySimple(string name, UInt32 location)
        {
            Name = name.Trim();
            Location = location;
        }

        public override string ToString(bool simple)
        {
            if (simple) return $"{Name};{Location}";
            else return $"{Name} @ 0x{Location:X} (0x{Location * 0x800:X} in file)";
        }
    }
}
