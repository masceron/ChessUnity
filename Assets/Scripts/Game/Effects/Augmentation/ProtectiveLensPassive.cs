using Game.Action;
using Game.Action.Internal;
using Game.Piece.PieceLogic.Commons;

namespace Game.Effects.Augmentation
{
    public class ProtectiveLensPassive : Effect, IApplyEffect
    {
        public ProtectiveLensPassive(sbyte duration, sbyte strength, PieceLogic piece) : base(duration, strength, piece, "effect_protected_lens_passive")
        { }

        public void OnCallApplyEffect(ApplyEffect applyEffect)
        {
            if (applyEffect.Effect.Piece != Piece) return;

            if (applyEffect.Effect.EffectName == "effect_blinded")
            {
                applyEffect.Result = ResultFlag.EffectResistance;
            }
        }
    }
}