using Game.Action;
using Game.Action.Captures;
using Game.Action.Internal;
using Game.Common;
using Game.Effects.Debuffs;
using Game.Effects.Triggers;
using Game.Piece.PieceLogic.Commons;

namespace Game.Effects.Traits
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public class DwarfLionfishPassive : Effect, IAfterPieceActionTrigger
    {
        public DwarfLionfishPassive(PieceLogic piece) : base(-1, 1, piece, "effect_dwarf_lionfish_passive")
        {
        }

        public AfterActionPriority Priority => AfterActionPriority.Debuff;

        public void OnCallAfterPieceAction(Action.Action action)
        {
            if (action.Target == Piece.Pos && action is ICaptures && action.Result == ResultFlag.Success)
                ActionManager.EnqueueAction(new ApplyEffect(new Bleeding(2, BoardUtils.PieceOn(action.Maker))));
        }
    }
}