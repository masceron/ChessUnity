using System;
using System.Collections.Generic;
using Game.Augmentation;
using Game.Managers;
using Game.ScriptableObjects;
using MemoryPack;

namespace Game.Save.Army
{
    [MemoryPackable]
    public partial struct Troop : IComparable<Troop>
    {
        public readonly string PieceType;
        public readonly int Rank;

        public readonly int File;

        // public readonly bool Side;
        public Dictionary<AugmentationSlot, AugmentationName> equippedAugmentation;

        public Troop(string pieceType, int rank, int file)
        {
            PieceType = pieceType;
            Rank = rank;
            File = file;
            // Side = side;
            equippedAugmentation = new Dictionary<AugmentationSlot, AugmentationName>();
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
            var slot = AssetManager.Ins.AugmentationData[aug].Slot;
            equippedAugmentation[slot] = aug;
        }
    }
}