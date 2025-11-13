using System;
using MemoryPack;

namespace Game.Save.Army
{
    [MemoryPackable]
    public readonly partial struct Troop: IComparable<Troop>
    {
        public readonly string Type;
        public readonly ushort Rank;
        public readonly ushort File;

        public Troop(string type, int rank, int file)
        {
            Type = type;
            Rank = (ushort) rank;
            File = (ushort) file;
        }

        public int CompareTo(Troop other)
        {
            if (Rank < other.Rank) return -1;
            if (Rank > other.Rank) return 1;
            if (File < other.File) return -1;
            return File > other.File ? 1 : 0;
        }
    }
}