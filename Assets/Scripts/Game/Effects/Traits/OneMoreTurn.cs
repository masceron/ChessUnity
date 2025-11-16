using Game.Action;
using Game.Action.Internal;
using Game.Piece.PieceLogic.Commons;

namespace Game.Effects.Traits
{
    [Il2CppSetOption(Option.NullChecks, false), Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public class OneMoreTurn: Effect, IEndTurnEffect
    {
        // private bool willDie = false;
        public EndTurnEffectType EndTurnEffectType { get; }
        public OneMoreTurn(PieceLogic piece) : base(-1, 1, piece, "effect_one_more_turn")
        {
            EndTurnEffectType = EndTurnEffectType.EndOfAllyTurn;

        }

        public void OnCallEnd(Action.Action lastMainAction){
            ActionManager.EnqueueAction(new KillPiece(Piece.Pos));
        }
    }
}