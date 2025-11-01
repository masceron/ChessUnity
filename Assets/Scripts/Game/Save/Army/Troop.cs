using System;
using Game.Piece;
using MemoryPack;
using Game.ScriptableObjects;
using Game.Managers;

namespace Game.Save.Army
{
    [MemoryPackable]
    public readonly partial struct Troop: IComparable<Troop>
    {
        public readonly PieceType Type;
        public readonly ushort Rank;
        public readonly ushort File;
        public readonly bool Side;
        public Troop(PieceType pieceType, int rank, int file, bool side = false)
        {
            Type = pieceType;
            Rank = (ushort) rank;
            File = (ushort)file;
            Side = side;
        }

        public int CompareTo(Troop other)
        {
            if (Rank < other.Rank) return -1;
            if (Rank > other.Rank) return 1;
            if (File < other.File) return -1;
            return File > other.File ? 1 : 0;
        }
        public PieceInfo GetPieceInfo()
        {
            return AssetManager.Ins.PieceData[Type];
        }
    }
}