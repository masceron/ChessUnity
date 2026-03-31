using Game.Action;
using Game.Action.Internal;
using Game.Effects.Condition;
using Game.Effects.Debuffs;
using Game.Piece.PieceLogic.Commons;

namespace Game.Effects.Traits
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public class PenpointGunnelPassive : Vengeful
    {
        public PenpointGunnelPassive(PieceLogic piece) : base(piece, VengefulType.OnCapture,"effect_penpoint_gunnel_passive")
        {
        }

        /// <summary>
        /// Gây hiệu ứng Leash lên quân ăn mình.
        /// </summary>
        protected override void OnVengefulTrigger()
        {
            if (Killer == null) return;
            ActionManager.EnqueueAction(new ApplyEffect(new Leashed(Killer, -1), Piece));
        }
    
    }
}