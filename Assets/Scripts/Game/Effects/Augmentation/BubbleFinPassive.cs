using Game.Piece.PieceLogic.Commons;

namespace Game.Effects.Augmentation
{
    public class BubbleFinPassive : Effect, IAttackRangeModifier
    {
        public BubbleFinPassive(sbyte duration, sbyte strength, PieceLogic piece) : base(duration, strength, piece,
            "effect_bubble_fin_passive")
        { }

        public int ModifyAttackRange(int baseRange)
        {
            return baseRange + 1;
        }
    }
}