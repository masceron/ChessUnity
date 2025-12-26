using Game.Managers;
using Game.Piece.PieceLogic.Commons;

namespace Game.Effects.Others
{
    public class NocturnalRangeBuff : Effect, IMoveRangeModifier, IAttackRangeModifier
    {
        public NocturnalRangeBuff(PieceLogic piece) : base(-1, 1, piece, "effect_nocturnal_range_buff")
        {
            EndTurnEffectType = EndTurnEffectType.EndOfAnyTurn;
        }

        public EndTurnEffectType EndTurnEffectType { get; }

        public int ModifyMoveRange(int baseRange)
        {
            if (!MatchManager.Ins.GameState.IsDay)
            {
                baseRange += Strength;
            }
            return baseRange;
        }

        public override int GetValueForAI()
        {
            return base.GetValueForAI() - 20;
        }

        public int ModifyAttackRange(int baseRange)
        {
            if (!MatchManager.Ins.GameState.IsDay)
            {
                baseRange += Strength;
            }
            return baseRange;
        }
    }
}