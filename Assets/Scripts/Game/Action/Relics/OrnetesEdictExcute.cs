using System.Linq;
using Game.Action.Internal;
using Game.Common;
using Game.Effects;
using Game.Managers;

namespace Game.Action.Relics
{
    [Il2CppSetOption(Option.NullChecks, false), Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public class OrnetesEdictExcute : Action, IRelicAction
    {
        private int Target;
        
        public OrnetesEdictExcute(int target) : base(target)
        {
            Target = target;
        }

        protected override void ModifyGameState()
        {
            var piece = BoardUtils.PieceOn(Target);
            if (piece == null) return;
            
            int numberOfDebuffedPieces = piece.Effects.Count(e => e.Category == EffectCategory.Debuff);
            int rate = 7 * numberOfDebuffedPieces;
            
            if (MatchManager.Roll(rate)) 
            {
                ActionManager.EnqueueAction(new KillPiece(Target));
            }
        }
    }
}
