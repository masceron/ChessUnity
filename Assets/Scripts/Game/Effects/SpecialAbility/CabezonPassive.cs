using Game.Action;
using Game.Action.Captures;
using Game.Action.Internal;
using Game.Effects.Debuffs;
using Game.Piece.PieceLogic.Commons;
using Game.Triggers;
using static Game.Common.BoardUtils;

namespace Game.Effects.SpecialAbility
{
    public class CabezonPassive : Effect, IAfterPieceActionTrigger
    {
        public CabezonPassive(PieceLogic piece) : base(-1, 1, piece, "effect_cabezon_passive")
        {
        }

        public AfterActionPriority Priority => AfterActionPriority.Debuff;

        public void OnCallAfterPieceAction(Action.Action action)
        {
            if (action is ICaptures && action.GetMaker() as PieceLogic == Piece &&
                action.Result is ResultFlag.Blocked or ResultFlag.Miss)
                ActionManager.EnqueueAction(new ApplyEffect(new Poison(1, action.GetTarget()), Piece));
        }
    }
}