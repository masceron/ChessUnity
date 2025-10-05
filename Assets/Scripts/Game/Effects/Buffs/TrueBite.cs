using Game.Action;
using Game.Action.Internal;
using Game.Piece.PieceLogic;

namespace Game.Effects.Buffs
{
    [Il2CppSetOption(Option.NullChecks, false), Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public class TrueBite: Effect, IApplyEffect
    {
        public TrueBite(PieceLogic piece) : base(-1, -1, piece, EffectName.TrueBite)
        {}

        public override void OnCall(Action.Action action)
        {
            
        }
        
        public void OnCallApplyEffect(ApplyEffect applyEffect)
        {
            if (applyEffect.Effect.Piece != Piece) return;

            var effect = applyEffect.Effect;

            if (effect.EffectName is EffectName.Evasion)
            {
                applyEffect.Result = ActionResult.Failed;
            }
        }
    }
}