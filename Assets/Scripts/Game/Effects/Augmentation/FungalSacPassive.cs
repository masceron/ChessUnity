    using Game.Action;
using Game.Action.Internal;
using Game.Piece.PieceLogic.Commons;
using Game.Effects.Buffs;
using Game.Effects.Debuffs;
namespace Game.Effects.Augmentation
{
    public class FungalSacPassive : Effect, IApplyEffect
    {
        private const int turnCounter = 4;
        private const int radius = 3;
        public FungalSacPassive(PieceLogic piece) : base(-1, 1, piece, "effect_fungal_sac_passive")
        {
        }

        public void OnCallApplyEffect(ApplyEffect applyEffect)
        {
            var infected = new Infected(Piece);
            infected.SetTurnCounter(turnCounter);
            infected.SetRadius(radius);
            ActionManager.EnqueueAction(new ApplyEffect(infected));
        }
    }
}