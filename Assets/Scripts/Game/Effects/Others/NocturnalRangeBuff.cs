using Game.Managers;
using Game.Piece.PieceLogic.Commons;

namespace Game.Effects.Others
{
    public class NocturnalRangeBuff : Effect, IEndTurnEffect, IMoveRangeModifier
    {
        private bool isBuff;
        private readonly byte initialAttackRange;
        public NocturnalRangeBuff(PieceLogic piece) : base(-1, 1, piece, "effect_nocturnal_range_buff")
        {
            EndTurnEffectType = EndTurnEffectType.EndOfAnyTurn;
            initialAttackRange = Piece.AttackRange;
        }

        public EndTurnEffectType EndTurnEffectType { get; }
        public void OnCallEnd(Action.Action lastMainAction)
        {
            if (MatchManager.Ins.GameState.IsDay && !isBuff)
            {
                isBuff = true;
                Piece.AttackRange++;
            }
            else if (!MatchManager.Ins.GameState.IsDay)
            {
                isBuff = false;
                Piece.AttackRange = initialAttackRange;
            }
        }

        public int ModifyMoveRange(int baseRange)
        {
            if (MatchManager.Ins.GameState.IsDay)
            {
                baseRange += Strength;
            }
            return baseRange;
        }

        public override int GetValueForAI()
        {
            return base.GetValueForAI() + 40;
        }
    }
}