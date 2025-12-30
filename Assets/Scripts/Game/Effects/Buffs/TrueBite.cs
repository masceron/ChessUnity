using Game.Action;
using Game.Action.Internal;
using Game.Piece.PieceLogic.Commons;

namespace Game.Effects.Buffs
{
    [Il2CppSetOption(Option.NullChecks, false), Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public class TrueBite: Effect, IApplyEffect
    {
        public TrueBite(PieceLogic piece) : base(-1, -1, piece, "effect_true_bite")
        {}

        public override void OnCallPieceAction(Action.Action action)
        {
            
        }
        
        public void OnCallApplyEffect(ApplyEffect applyEffect)
        {
            if (applyEffect.Effect.Piece != Piece) return;

            var effect = applyEffect.Effect;

            if (effect.EffectName is "effect_evasion")
            {
                applyEffect.Result = ResultFlag.Dodged;
            }
        }

        public override int GetValueForAI()
        {
            return base.GetValueForAI() + 30;
        }
    }
}