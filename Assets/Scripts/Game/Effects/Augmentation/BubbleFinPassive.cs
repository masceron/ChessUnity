using Game.Piece.PieceLogic.Commons;
using Game.Triggers;

namespace Game.Effects.Augmentation
{
    public class BubbleFinPassive : Effect, IAttackRangeModifier
    {
        public BubbleFinPassive(int duration, int strength, PieceLogic piece) : base(duration, strength, piece,
            "effect_bubble_fin_passive")
        {
        }

        public int ModifyAttackRange(int baseRange)
        {
            return baseRange + 1;
        }
    }
}