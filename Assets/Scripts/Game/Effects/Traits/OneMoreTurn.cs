using Game.Action;
using Game.Action.Internal;
using Game.Piece.PieceLogic.Commons;
using Game.Triggers;

namespace Game.Effects.Traits
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public class OneMoreTurn : Effect, IEndTurnTrigger
    {
        public OneMoreTurn(PieceLogic piece) : base(-1, 1, piece, "effect_one_more_turn")
        {
            EndTurnEffectType = EndTurnEffectType.EndOfAllyTurn;
        }

        // private bool willDie = false;
        public EndTurnTriggerPriority Priority => EndTurnTriggerPriority.Kill;

        public EndTurnEffectType EndTurnEffectType { get; }

        public void OnCallEnd(Action.Action lastMainAction)
        {
            ActionManager.EnqueueAction(new KillPiece(Piece.Pos));
        }
    }
}