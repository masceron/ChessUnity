using Game.Action;
using Game.Action.Internal;
using Game.Effects.Condition;
using Game.Effects.Debuffs;
using Game.Piece.PieceLogic.Commons;

namespace Game.Effects.Traits
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public class DwarfLionfishPassive : Vengeful
    {
        private const int Stack = 2;
        public DwarfLionfishPassive(PieceLogic piece) : base(piece, VengefulType.OnCapture, "effect_dwarf_lionfish_passive")
        {
        }

        /// <summary>
        /// Khi bị ăn: gây Bleeding 2 lên quân ăn mình.
        /// </summary>
        protected override void OnVengefulTrigger()
        {
            if (Killer == null) return;
            ActionManager.EnqueueAction(new ApplyEffect(new Bleeding(Stack, Killer)));
        }
    }
}