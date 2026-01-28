using System;
using System.Collections.Generic;

namespace Game.Piece
{
    [Il2CppSetOption(Option.NullChecks, false), Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public readonly struct PieceConfig : IEquatable<PieceConfig>
    {
        public readonly string Type;
        public readonly bool Color;
        public readonly ushort Index;
        public readonly List<Augmentation.Augmentation> Augmentations;

        public PieceConfig(string type, bool color, ushort index, List<Augmentation.Augmentation> augmentations = null)
        {
            Type = type;
            Color = color;
            Index = index;
            Augmentations = augmentations ?? new List<Augmentation.Augmentation>();
        }

        public bool Equals(PieceConfig other)
        {
            return Type == other.Type && Color == other.Color && Index == other.Index;
        }

        public override bool Equals(object obj)
        {
            return obj is PieceConfig other && Equals(other);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = Type.GetHashCode();
                hashCode = (hashCode * 397) ^ Color.GetHashCode();
                hashCode = (hashCode * 397) ^ Index.GetHashCode();
                return hashCode;
            }
        }

        public static bool operator ==(PieceConfig left, PieceConfig right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(PieceConfig left, PieceConfig right)
        {
            return !left.Equals(right);
        }
    }
}