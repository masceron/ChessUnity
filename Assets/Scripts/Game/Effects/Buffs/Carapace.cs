using Game.Action;
using Game.Action.Captures;
using Game.Action.Internal;
using Game.Managers;
using Game.Piece.PieceLogic.Commons;

namespace Game.Effects.Buffs
{
    [Il2CppSetOption(Option.NullChecks, false), Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public class Carapace: Effect, IBeforePieceActionEffect, IAfterPieceActionEffect
    {
        public Carapace(sbyte duration, PieceLogic piece) : base(duration, 1, piece, "effect_carapace")
        {}

        public void OnCallBeforePieceAction(Action.Action action)
        {
            if (action is not ICaptures || action.Target != Piece.Pos || action.Result != ResultFlag.Success
                || (action.Flag & ActionFlag.Unblockable) != 0) return;
            
            action.Result = ResultFlag.Parry;
        }

        public void OnCallAfterPieceAction(Action.Action action)
        {
            if (action is not ICaptures || action.Target != Piece.Pos || action.Result != ResultFlag.Blocked) return;

            ActionManager.EnqueueAction(new RemoveEffect(this));
            if (MatchManager.Roll(25))
            {
                ActionManager.EnqueueAction(new CarapaceKill(Piece.Pos, action.Maker));
            }
        }

        public override int GetValueForAI()
        {
            return base.GetValueForAI() - 80;
        }
    }
}