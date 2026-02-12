using Game.Action;
using Game.Action.Captures;
using Game.Action.Internal;
using Game.Piece.PieceLogic.Commons;

namespace Game.Effects.Others
{
    public class MarineIguanaKillEffect : Effect, IDeadEffect, IOnApply
    {
        private readonly int maker;
        private readonly int target;
        public MarineIguanaKillEffect(int duration, PieceLogic piece, int maker, int target) : base(duration, 1, piece, "effect_marine_iguana_kill_effect")
        {
            this.target = target;
            this.maker = maker;
        }

        public void OnApply()
        {
            ActionManager.EnqueueAction(new NormalCapture(maker, Piece.Pos));

        }

        public void OnCallDead(PieceLogic pieceToDie)
        {
            if (pieceToDie != Piece) 
            {
                return;
            }
            ActionManager.EnqueueAction(new DestroyPiece(target));
        }
    }
}
