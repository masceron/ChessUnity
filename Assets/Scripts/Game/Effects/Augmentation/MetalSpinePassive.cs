using Game.Action.Internal;
using Game.Piece.PieceLogic.Commons;

namespace Game.Effects.Augmentation
{
    public class MetalSpinePassive : Effect, IApplyEffect
    {
        public MetalSpinePassive(sbyte duration, sbyte strength, PieceLogic piece) : base(duration, strength, piece,
            "effect_metal_spine_passive")
        { }
        public void OnCallApplyEffect(ApplyEffect applyEffect)
        {
            if (applyEffect.Effect.EffectName == "effect_broken")
            {
                applyEffect.Result = Action.ResultFlag.Blocked;
            }
        }
    }
}