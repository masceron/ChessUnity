using Game.Action.Internal;
using Game.Piece.PieceLogic.Commons;

namespace Game.Effects.Augmentation
{
    public class MetalMindPassive : Effect, IApplyEffect
    {
        public MetalMindPassive(sbyte duration, sbyte strength, PieceLogic piece) : base(duration, strength, piece,
            "effect_metal_mind_passive")
        { }
        public void OnCallApplyEffect(ApplyEffect applyEffect)
        {
            if (applyEffect.Effect.EffectName == "effect_frenzied")
            {
                applyEffect.Result = Action.ResultFlag.Incorruptible;
            }
        }
    }
}