using Game.Action.Internal;
using Game.Action;
using Game.Effects.Triggers;
using Game.Piece.PieceLogic.Commons;

namespace Game.Effects.Others
{
    [Il2CppSetOption(Option.NullChecks, false), Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public class KillPieceAfterSwitchTurn : Effect, IEndTurnTrigger
    {
        public EndTurnTriggerPriority Priority => EndTurnTriggerPriority.Kill;

        public EndTurnEffectType EndTurnEffectType { get; }
        public KillPieceAfterSwitchTurn(PieceLogic piece) : base(-1, 1, piece, "effect_kill_after_switch_turn")
        {
            EndTurnEffectType = EndTurnEffectType.EndOfAnyTurn;
        }

        public void OnCallEnd(Action.Action lastMainAction){
            ActionManager.EnqueueAction(new KillPiece(Piece.Pos));
        }
    }
}