using Game.Action;
using Game.Action.Captures;
using Game.Action.Internal;
using Game.Common;
using Game.Effects.Condition;
using Game.Effects.Debuffs;
using Game.Piece.PieceLogic.Commons;
using Game.Triggers;

namespace Game.Effects.Others
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public class SlimeheadPassive : Vengeful
    {
        public SlimeheadPassive(PieceLogic piece) : base(piece,VengefulType.OnCapture, "effect_slimehead_passive")
        {
        }

        /// <summary>
        /// Nếu quân ăn có >= 2 buff trên người, gây Infected và Slow 1 3 turn lên người nó
        /// </summary>
        protected override void OnVengefulTrigger()
        {
            if (Killer == null) return;
            var buffEffect = Killer.Effects.Count(t => t.Category == EffectCategory.Buff);

            
            if (buffEffect < 2) return;
            ActionManager.EnqueueAction(new ApplyEffect(new Infected(Killer), Piece));
            ActionManager.EnqueueAction(new ApplyEffect(new Slow(3, 1, Killer), Piece));
        }

        public override int GetValueForAI()
        {
            return base.GetValueForAI() + 50;
        }
    }
}