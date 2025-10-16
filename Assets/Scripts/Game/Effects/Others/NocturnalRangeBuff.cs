using Game.Action;
using Game.Action.Internal;
using Game.Effects.Buffs;
using Game.Managers;
using Game.Piece.PieceLogic;

namespace Game.Effects.Others
{
    public class NocturnalRangeBuff : Effect, IEndTurnEffect
    {
        private bool isBuff = false;
        private byte initialAttackRange;
        public NocturnalRangeBuff(PieceLogic piece) : base(-1, 1, piece, EffectName.NocturnalRangeBuff)
        {
            EndTurnEffectType = EndTurnEffectType.EndOfAnyTurn;
            initialAttackRange = Piece.AttackRange;
        }

        public EndTurnEffectType EndTurnEffectType { get; }
        public void OnCallEnd(Action.Action lastMainAction)
        {
            //TODO: Remove Haste effect and moveRange = initialMoveRange.
            if (MatchManager.Ins.GameState.IsDay && !isBuff)
            {
                isBuff = true;
                Piece.AttackRange++;
                ActionManager.ExecuteImmediately(new ApplyEffect(new Haste(10, 1, Piece)));
            }
            else if (!MatchManager.Ins.GameState.IsDay)
            {
                isBuff = false;
                Piece.AttackRange = initialAttackRange;
            }
        }
    }
}