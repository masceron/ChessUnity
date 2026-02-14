using Game.Action;
using Game.Action.Internal;
using Game.Effects.Debuffs;
using Game.Piece.PieceLogic.Commons;
using Game.Triggers;

namespace Game.Effects.Augmentation
{
    public class FungalSacPassive : Effect, IOnApplyTrigger
    {
        private const int TurnCounter = 4;
        private const int Radius = 3;

        public FungalSacPassive(PieceLogic piece) : base(-1, 1, piece, "effect_fungal_sac_passive")
        {
        }

        public void OnApply()
        {
            var infected = new Infected(Piece);
            infected.SetTurnCounter(TurnCounter);
            infected.SetRadius(Radius);
            ActionManager.EnqueueAction(new ApplyEffect(infected));
        }
    }
}