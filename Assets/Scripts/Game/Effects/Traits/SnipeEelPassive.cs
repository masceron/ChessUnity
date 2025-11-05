using System.Linq;
using Game.Action;
using Game.Action.Internal;
using Game.Common;
using Game.Piece.PieceLogic;

namespace Game.Effects.Traits
{
    [Il2CppSetOption(Option.NullChecks, false), Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public class SnipeEelPassive : Effect, IApplyEffect
    {
        public SnipeEelPassive(PieceLogic piece) : base(-1, -1, piece, EffectName.SnipeEelPassive)
        {
            
        }

        public void OnCallApplyEffect(ApplyEffect applyEffect)
        {
            var target = BoardUtils.PieceOn(applyEffect.Target);
            if (target.Effects.Any(e => e.EffectName == EffectName.Bound))
            {
                applyEffect.Result = ActionResult.SkipEvasion;
            }
        }
    }
}