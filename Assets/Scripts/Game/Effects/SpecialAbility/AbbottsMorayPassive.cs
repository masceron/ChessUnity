using Game.Action;
using Game.Action.Captures;
using Game.Action.Internal;
using Game.Action.Quiets;
using Game.Effects.Buffs;
using Game.Effects.Traits;
using Game.Piece.PieceLogic.Commons;
using Game.Triggers;
using static Game.Common.BoardUtils;

namespace Game.Effects.SpecialAbility
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public class AbbottsMorayPassive : Effect, IAfterPieceActionTrigger
    {
        public AbbottsMorayPassive(PieceLogic piece) : base(-1, 1, piece, "effect_abbotts_moray_passive")
        {
            SetStat(EffectStat.Radius, 3);
            SetStat(EffectStat.Duration, 2);
        }

        public AfterActionPriority Priority => AfterActionPriority.Buff;

        private static bool HasAmbushTrait(PieceLogic target)
        {
            if (target == null) return false;
            foreach (var e in target.Effects)
                if (e is Ambush) return true;
            return false;
        }

        public void OnCallAfterPieceAction(global::Game.Action.Action action)
        {
            //Làm lại
        }
    }
}


