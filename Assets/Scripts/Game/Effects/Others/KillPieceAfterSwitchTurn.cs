using Game.Piece.PieceLogic;
using Game.Action.Internal;
using Game.Action;

namespace Game.Effects.Others
{
    [Il2CppSetOption(Option.NullChecks, false), Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public class KillPieceAfterSwitchTurn : Effect, IEndTurnEffect
    {
        public EndTurnEffectType EndTurnEffectType { get; }
        public KillPieceAfterSwitchTurn(PieceLogic piece) : base(-1, 1, piece, EffectName.KillPieceAfterSwitchTurn)
        {
            EndTurnEffectType = EndTurnEffectType.EndOfAnyTurn;
        }

        public void OnCallEnd(Action.Action lastMainAction){
            ActionManager.EnqueueAction(new KillPiece(Piece.Pos));
        }
    }
}