using Game.Action;
using Game.Action.Internal;
using Game.Piece.PieceLogic.Commons;
using Game.Triggers;

namespace Game.Effects.Others
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public class KillPieceAfterSwitchTurn : Effect, IEndTurnTrigger
    {
        public KillPieceAfterSwitchTurn(PieceLogic piece) : base(-1, 1, piece, "effect_kill_piece_after_switch_turn")
        {
            EndTurnEffectType = EndTurnEffectType.EndOfAnyTurn;
        }

        public EndTurnTriggerPriority Priority => EndTurnTriggerPriority.Kill;

        public EndTurnEffectType EndTurnEffectType { get; }

        public void OnCallEnd(Action.Action lastMainAction)
        {
            ActionManager.EnqueueAction(new KillPiece(null, Piece));
        }
    }
}