using Game.Action;
using Game.Action.Internal;
using Game.Piece.PieceLogic.Commons;
using Game.Effects.Buffs;
namespace Game.Effects.Augmentation
{
    public class LeviathanScalePassive : Effect, IApplyEffect
    {
        public LeviathanScalePassive(PieceLogic piece) : base(-1, 1, piece, "effect_leviathan_scale_passive")
        {
        }

        public void OnCallApplyEffect(ApplyEffect applyEffect)
        {
            if (applyEffect.Effect.Piece != Piece) return;
            var effect = applyEffect.Effect;
            if (effect.EffectName == "effect_shield")
            {
                applyEffect.Result = ResultFlag.CantApplyEffect;
                
                ActionManager.EnqueueAction(new ApplyEffect(new Carapace(effect.Strength, Piece)));
            }
            
        }
    }
}