using System;
using System.Collections.Generic;
using Game.Augmentation;
using MemoryPack;
using Game.ScriptableObjects;
using Game.Managers;

namespace Game.Save.Army
{
    [MemoryPackable]
    public partial struct Troop: IComparable<Troop>
    {
        public readonly string PieceType;
        public readonly ushort Rank;
        public readonly ushort File;
        // public readonly bool Side;
        public Dictionary<AugmentationSlot, AugmentationName> equippedAugmentation;
        public Troop(string pieceType, int rank, int file)
        {
            PieceType = pieceType;
            Rank = (ushort) rank;
            File = (ushort)file;
            // Side = side;
            equippedAugmentation = new();
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
            return AssetManager.Ins.PieceData[PieceType];
        }
        public void EquipAugmentation(AugmentationName aug)
        {
            AugmentationSlot slot = AssetManager.Ins.AugmentationData[aug].Slot;
            equippedAugmentation[slot] = aug;
        }
    }
}