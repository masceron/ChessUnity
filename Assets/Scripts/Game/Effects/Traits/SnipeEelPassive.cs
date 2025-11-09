using System.Linq;
using Game.Action;
using Game.Action.Internal;
using Game.Common;
using Game.Piece.PieceLogic;

namespace Game.Effects.Traits
{
    [Il2CppSetOption(Option.NullChecks, false), Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public class SnipeEelPassive : Effect
    {
        private Effect evasion;
        private sbyte lastDuration;
        public SnipeEelPassive(PieceLogic piece) : base(-1, -1, piece, EffectName.SnipeEelPassive)
        {
            
        }

        public override void OnCall(Action.Action action)
        {
            if (action == null) return;

            var tar = BoardUtils.PieceOn(action.Target);
            if (tar.Effects.Any(e => e.EffectName == EffectName.Bound))
            {
                
                evasion = tar.Effects.First(e => e.EffectName == EffectName.Evasion);
                
                if (evasion == null) return;
                lastDuration = evasion.Duration;
                evasion.Duration = -1;
                
            }
        }

        public override void OnRemove()
        {
            if (evasion == null) return;

            evasion.Duration = lastDuration;
        }
    }
}