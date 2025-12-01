using Game.Action;
using Game.Action.Internal;
using Game.Action.Quiets;
using Game.Piece.PieceLogic.Commons;
using static Game.Common.BoardUtils;

namespace Game.Effects.Debuffs
{
    [Il2CppSetOption(Option.NullChecks, false), Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public class Taunted: Effect, IEndTurnEffect
    {
        public Taunted(sbyte duration, PieceLogic piece) : base(duration, 1, piece, "effect_taunted")
        {
            EndTurnEffectType = EndTurnEffectType.EndOfEnemyTurn;
        }
        public EndTurnEffectType EndTurnEffectType { get; }
        public void OnCallEnd(Action.Action lastMainAction)
        {
            var push = !Piece.Color ? PushWhite(Piece.Pos) : PushBlack(Piece.Pos);
            if (!VerifyIndex(push) || !IsActive(push) || PieceOn(push) != null) return;

            ActionManager.ExecuteImmediately(new NormalMove(Piece.Pos, push));
            ActionManager.EnqueueAction(new ApplyEffect(new Stunned(1, Piece)));
        }

        public override int GetValueForAI()
        {
            return base.GetValueForAI() - 30;
        }
    }
}