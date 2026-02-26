using Game.Managers;
using Game.Piece.PieceLogic.Commons;
using Game.Triggers;

namespace Game.Effects.Others
{
    public class NocturnalRangeBuff : Effect, IMoveRangeModifierTrigger, IAttackRangeModifier
    {
        public NocturnalRangeBuff(PieceLogic piece) : base(-1, 1, piece, "effect_nocturnal_range_buff")
        {
        }

        public int ModifyAttackRange(int baseRange)
        {
            if (!MatchManager.Ins.GameState.IsDay) baseRange += Strength;
            return baseRange;
        }

        public int ModifyMoveRange(int baseRange)
        {
            if (!MatchManager.Ins.GameState.IsDay) baseRange += Strength;
            return baseRange;
        }

        public override int GetValueForAI()
        {
            return base.GetValueForAI() - 20;
        }
    }
}