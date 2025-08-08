using Game.Action;
using Game.Action.Internal;
using Game.Common;
using Game.Piece.PieceLogic;

namespace Game.Effects.Others
{
    public class RemoraMarked: Effect, IEndTurnEffect
    {
        private readonly PieceLogic caster;
        public RemoraMarked(PieceLogic caster, PieceLogic piece) : base(-1, 1, piece, EffectName.RemoraMarked)
        {
            this.caster = caster;
            EndTurnEffectType = EndTurnEffectType.EndOfAllyTurn;
        }

        public EndTurnEffectType EndTurnEffectType { get; }
        public void OnCallEnd(Action.Action lastMainAction)
        {
            if (caster.IsDead() || BoardUtils.Distance(caster.Pos, Piece.Pos) > 1)
            {
                ActionManager.EnqueueAction(new RemoveEffect(this));
            }
            else
            {
                ActionManager.EnqueueAction(new DestroyPiece(Piece.Pos));
            }
        }
    }
}