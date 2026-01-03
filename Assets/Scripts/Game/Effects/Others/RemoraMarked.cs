using Game.Action;
using Game.Action.Internal;
using Game.Common;
using Game.Piece.PieceLogic.Commons;

namespace Game.Effects.Others
{
    [Il2CppSetOption(Option.NullChecks, false), Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public class RemoraMarked: Effect, IEndTurnEffect
    {
        private readonly PieceLogic caster;
        public RemoraMarked(PieceLogic caster, PieceLogic piece) : base(-1, 1, piece, "effect_remora_marked")
        {
            this.caster = caster;
            EndTurnEffectType = EndTurnEffectType.EndOfAllyTurn;
        }

        public EndTurnEffectType EndTurnEffectType { get; }
        public void OnCallEnd(Action.Action lastMainAction)
        {
            if (!BoardUtils.IsAlive(caster) || BoardUtils.Distance(caster.Pos, Piece.Pos) > 1)
            {
                ActionManager.EnqueueAction(new RemoveEffect(this));
            }
            else
            {
                ActionManager.EnqueueAction(new KillPiece(Piece.Pos));
            }
        }
    }
}