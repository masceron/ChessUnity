using Game.Action;
using Game.Action.Captures;
using Game.Action.Internal;
using Game.Piece.PieceLogic.Commons;
using Game.Triggers;

namespace Game.Effects.Others
{
    public class MarineIguanaKillEffect : Effect, IDeadTrigger, IOnApplyTrigger
    {
        private readonly PieceLogic maker;
        private readonly PieceLogic target;

        public MarineIguanaKillEffect(int duration, PieceLogic maker, PieceLogic first, PieceLogic second) : base(duration, 1, first,
            "effect_marine_iguana_kill_effect")
        {
            this.maker = maker;
            target = second;
        }

        public void OnCallDead(PieceLogic pieceToDie)
        {
            if (pieceToDie != Piece) return;
            ActionManager.EnqueueAction(new DestroyPiece(target));
        }

        public void OnApply()
        {
            ActionManager.EnqueueAction(new NormalCapture(maker, Piece));
        }
    }
}