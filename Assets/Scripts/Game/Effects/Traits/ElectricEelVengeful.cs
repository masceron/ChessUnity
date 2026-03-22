using Game.Action;
using Game.Action.Internal;
using Game.Effects.Condition;
using Game.Effects.Debuffs;
using Game.Piece.PieceLogic.Commons;

namespace Game.Effects.Traits
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public class ElectricEelVengeful : Vengeful
    {
        public ElectricEelVengeful(PieceLogic piece) : base (piece, VengefulType.OnCapture, "effect_electric_eel_vengeful")
        {
        }

        /// <summary>
        /// Khi bị ăn, Stun quân vừa ăn mình trong 2 turn
        /// </summary>
        protected override void OnVengefulTrigger()
        {
            if (Killer == null) return;
            ActionManager.EnqueueAction(new ApplyEffect(new Stunned(2, Killer), Piece));
        }

        public override int GetValueForAI()
        {
            return base.GetValueForAI() + 70;
        }
    }
}