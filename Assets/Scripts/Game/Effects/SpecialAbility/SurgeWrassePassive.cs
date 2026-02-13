using Game.Action;
using Game.Action.Internal;
using Game.Action.Quiets;
using Game.Common;
using Game.Effects.Buffs;
using Game.Effects.Triggers;
using Game.Managers;
using Game.Piece.PieceLogic.Commons;
using Game.Tile;

namespace Game.Effects.SpecialAbility
{
    public class SurgeWrassePassive : Effect, IAfterPieceActionTrigger
    {
        public SurgeWrassePassive(PieceLogic piece) : base(-1, 1, piece, "effect_surge_wrasse_passive")
        {
        }

        public AfterActionPriority Priority => AfterActionPriority.Buff;

        public void OnCallAfterPieceAction(Action.Action action)
        {
            if (action is not IQuiets) return;
            var fmt = BoardUtils.GetFormation(Piece.Pos);
            if (fmt != null && AssetManager.Ins.FormationData[fmt.GetFormationType()].formationCategory ==
                FormationCategory.Negative) ActionManager.EnqueueAction(new ApplyEffect(new Haste(2, 4, Piece)));
        }
    }
}