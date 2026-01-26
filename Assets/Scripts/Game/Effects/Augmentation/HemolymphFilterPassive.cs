using Game.Action.Internal;
using Game.Piece.PieceLogic.Commons;

namespace Game.Effects.Augmentation
{
    public class HemolymphFilterPassive : Effect, IApplyEffect
    {
        public HemolymphFilterPassive(PieceLogic piece) : base(-1, 1, piece, "effect_hemolymph_filter_passive")
        {
        }

        /// <summary>
        /// Kháng hiệu ứng Infected
        /// </summary>
        /// <param name="applyEffect">Effect is applied</param>
        public void OnCallApplyEffect(ApplyEffect applyEffect)
        {
            if (applyEffect.Effect.Piece != Piece || applyEffect.Effect.EffectName != "effect_infected") return;
            applyEffect.Result = Action.ResultFlag.EffectResistance;
        }
    }
}