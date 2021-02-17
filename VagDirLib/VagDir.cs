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

        public int EntryCount { get; set; }
        public List<VagDirEntry> Entries { get; set; } = new List<VagDirEntry>();

        public VagDir(string path = @"D:\PS2 ISOs\Jak and Daxter - The Precursor Legacy (En,Fr,De,Es,It) (Alt)\VAG\VAGDIR.AYB", int version = 1)
        {
            Path = path;
            Version = version;

            ReadFile();
        }

        private void ReadFile()
        {
            using (BinaryReader br = new BinaryReader(File.Open(Path, FileMode.Open)))
            {
                EntryCount = br.ReadInt32();

                for (int i = 0; i < EntryCount; i++)
                {
                    byte[] namebytes = br.ReadBytes(8);
                    Entries.Add(new VagDirEntrySimple(Encoding.UTF8.GetString(namebytes), br.ReadUInt32()));
                }
            }
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
    }

    class VagDirEntrySimple : VagDirEntry // version 1
    {
        public VagDirEntrySimple(string name, UInt32 location)
        {
            Name = name;
            Location = location;
        }

        public override string ToString()
        {
            return $"{Name} @ 0x{Location:X} (0x{Location * 0x800:X} in file)";
        }
    }
}