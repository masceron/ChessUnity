using Game.Action;
using Game.Action.Captures;
using Game.Action.Internal;
using Game.Action.Skills;
using Game.Effects.Buffs;
using Game.Piece.PieceLogic.Commons;
using Game.Triggers;
using static Game.Common.BoardUtils;

namespace Game.Effects.SpecialAbility
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public class MaximaClamPassive : Effect, IAfterPieceActionTrigger
    {
        public MaximaClamPassive(int radius, PieceLogic piece)
            : base(-1, 1, piece, "effect_maxima_clam_passive")
        {
            SetStat(EffectStat.Radius, radius);
        }

        public AfterActionPriority Priority => AfterActionPriority.Buff;

        public void OnCallAfterPieceAction(Action.Action action)
        {
            if (action.GetMakerAsPiece() != Piece) return;

            var triggeredByCapture = action is ICaptures && action.Result == ResultFlag.Success;
            var triggeredByActive = action is MaximaClamActive && action.Result == ResultFlag.Success;
            if (!triggeredByCapture && !triggeredByActive) return;

            var radius = GetStat(EffectStat.Radius);
            for (var i = 0; i < BoardSize; i++)
            {
                var ally = PieceOn(i);
                if (ally == null || ally.Color != Piece.Color) continue;
                if (Distance(ally.Pos, Piece.Pos) > radius) continue;

                ActionManager.EnqueueAction(new ApplyEffect(new Shield(ally), Piece));
            }
        }
    }
}
