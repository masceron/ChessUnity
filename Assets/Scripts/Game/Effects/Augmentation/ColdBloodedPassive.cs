using Game.Action.Internal;
using Game.Piece.PieceLogic.Commons;

namespace Game.Effects.Augmentation
{
    public class ColdBloodedPassive : Effect, IApplyEffect
    {
        public ColdBloodedPassive(sbyte duration, sbyte strength, PieceLogic piece) : base(duration, strength, piece,
            "effect_cold_blooded_passive")
        { }
        public void OnCallApplyEffect(ApplyEffect applyEffect)
        {
            if (applyEffect.Effect.EffectName == "effect_shortreach")
            {
                applyEffect.Result = Action.ResultFlag.Blocked;
            }
        }
    }
}