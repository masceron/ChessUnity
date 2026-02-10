using Game.Action;
using Game.Action.Internal;
using Game.Effects.Traits;
using Game.Piece.PieceLogic.Commons;

namespace Game.Effects.Augmentation
{
    public class CursedFinPassive : Effect, IOnApply, IAttackRangeModifier, IMoveRangeModifier
    {
        private const int attackRangeModifer = 4;
        private const int moveRangeModifier = 4;
        private const int cooldownModifier = 10;
        
        public CursedFinPassive(PieceLogic piece) : base(-1, 1, piece, "effect_cursed_fin_passive")
        {
        }

        public void OnApply()
        {
            if (Piece is IPieceWithSkill skillPiece)
            {
                skillPiece.TimeToCooldown += cooldownModifier;
            }
            ActionManager.EnqueueAction(new ApplyEffect(new Sanity(-1, Piece)));
        }

        public int ModifyAttackRange(int baseRange)
        {
            return baseRange + attackRangeModifer;
        }

        public int ModifyMoveRange(int baseRange)
        {
            return baseRange + moveRangeModifier;
        }
    }
}