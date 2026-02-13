using Game.Action;
using Game.Action.Captures;
using Game.Action.Internal;
using Game.Common;
using Game.Effects.Triggers;
using Game.Managers;
using Game.Piece.PieceLogic.Commons;

namespace Game.Effects.Traits
{
    [Il2CppSetOption(Option.NullChecks, false), Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public class CoffinFishVengeful : Effect, IAfterPieceActionTrigger
    {
        public CoffinFishVengeful(PieceLogic piece, int probability) : base(-1, probability, piece, "effect_coffin_fish_vengeful")
        {}

        public AfterActionPriority Priority => AfterActionPriority.Buff;

        public void OnCallAfterPieceAction(Action.Action action)
        {
            if (action is not ICaptures || action.Result != ResultFlag.Success) return;

            if (!MatchManager.Roll(Strength)) return;

            if (action.Target == Piece.Pos)
            {
                ActionManager.EnqueueAction(new ApplyEffect(new Relentless(BoardUtils.PieceOn(action.Maker), 1)));
            }
        }
        public override int GetValueForAI()
        {
            return base.GetValueForAI() + 50;
        }
    }
}