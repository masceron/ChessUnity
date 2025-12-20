using Game.Piece.PieceLogic.Commons;

namespace Game.Effects.Augmentation
{
    public class ColdBloodedPassive : Effect
    {
        public ColdBloodedPassive(sbyte duration, sbyte strength, PieceLogic piece) : base(duration, strength, piece,
            "effect_cold_blooded_passive")
        { }
    }
}