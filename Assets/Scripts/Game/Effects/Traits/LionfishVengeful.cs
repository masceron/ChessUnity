using Game.Action;
using Game.Action.Captures;
using Game.Action.Internal;
using Game.Common;
using Game.Effects.Condition;
using Game.Effects.Debuffs;
using Game.Piece.PieceLogic.Commons;
using Game.Triggers;

namespace Game.Effects.Traits
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public class LionfishVengeful : Vengeful
    {
        public LionfishVengeful(PieceLogic piece) : base(piece, VengefulType.OnCapture, "effect_lionfish_vengeful")
        {
        }

        /// <summary>
        /// Gây 3 stack poison cho quân ăn mình.
        /// </summary>
        protected override void OnVengefulTrigger()
        {
            if (Killer == null) return;
            ActionManager.EnqueueAction(new ApplyEffect(new Poison(3, Killer), Piece));
        }

        public override int GetValueForAI()
        {
            return base.GetValueForAI() + 50;
        }
    }
}