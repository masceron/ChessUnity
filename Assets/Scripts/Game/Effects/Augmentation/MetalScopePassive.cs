using Game.Action.Internal;
using Game.Piece.PieceLogic.Commons;

namespace Game.Effects.Augmentation
{
    public class MetalScopePassive : Effect, IApplyEffect
    {
        public MetalScopePassive(int duration, int strength, PieceLogic piece) : base(duration, strength, piece,
            "effect_metal_scope_passive")
        { }
        public void OnCallApplyEffect(ApplyEffect applyEffect)
        {
            if (applyEffect.Effect.EffectName == "effect_marked")
            {
                applyEffect.Result = Action.ResultFlag.EffectResistance;
            }
        }
    }
}