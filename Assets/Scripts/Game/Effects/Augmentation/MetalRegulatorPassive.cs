using Game.Action.Internal;
using Game.Piece.PieceLogic.Commons;

namespace Game.Effects.Augmentation
{
    public class MetalRegulatorPassive : Effect, IApplyEffect
    {
        public MetalRegulatorPassive(sbyte duration, sbyte strength, PieceLogic piece) : base(duration, strength, piece,
            "effect_metal_regulator_passive")
        { }
        public void OnCallApplyEffect(ApplyEffect applyEffect)
        {
            if (applyEffect.Effect.EffectName == "effect_bleed")
            {
                applyEffect.Result = Action.ResultFlag.Blocked;
            }
        }
    }
}