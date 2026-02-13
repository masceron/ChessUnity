using Game.Action;
using Game.Action.Internal;
using Game.Common;
using Game.Effects.Triggers;
using Game.Piece.PieceLogic.Commons;

namespace Game.Effects.Others
{
    [Il2CppSetOption(Option.NullChecks, false), Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public class RemoraMarked: Effect, IEndTurnTrigger
    {
        private readonly PieceLogic _caster;
        public RemoraMarked(PieceLogic caster, PieceLogic piece) : base(-1, 1, piece, "effect_remora_marked")
        {
            this._caster = caster;
            EndTurnEffectType = EndTurnEffectType.EndOfAllyTurn;
        }

        public EndTurnTriggerPriority Priority => EndTurnTriggerPriority.Kill;

        public EndTurnEffectType EndTurnEffectType { get; }
        public void OnCallEnd(Action.Action lastMainAction)
        {
            if (!BoardUtils.IsAlive(_caster) || BoardUtils.Distance(_caster.Pos, Piece.Pos) > 1)
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