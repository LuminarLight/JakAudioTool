using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VagDirLib
{
    public class VagDir
    {

    }

    /*        class VAGDIR_EntryAdvanced // Not sure how it knows whether it is in a language VAGWAD or the INT VAGWAD, but I believe it won't be hard to figure out once we know the rest.
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
    /*            public VAGDIR_EntrySimple(int unknown, string name, UInt32 location)
            {
                Unknown = unknown;
                Name = name;
                Location = location;
            }

            public int Unknown { get; } // 4 bytes - strange value for the first entry, 0 or 1 for the rest
            public string Name { get; } // always 8 bytes, if less then add space to the end of the name to reach 8 characters
            public UInt32 Location { get; } // multiply this by 0x800 to get the audio's position in the VAGWAD files
        }*/
}
