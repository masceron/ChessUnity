using Game.Action;
using Game.Action.Captures;
using Game.Action.Internal;
using Game.Effects.Debuffs;
using Game.Piece.PieceLogic.Commons;
using Game.Triggers;

namespace Game.Effects.Traits
{
    /// <summary>
    ///     Khi tấn công trúng đối phương, đối phương không chết mà bị Bleeding 4. Chết sau khi thực hiện ăn quân địch.
    /// </summary>
    public class LeechPassive : Effect, IAfterPieceActionTrigger
    {
        public LeechPassive(PieceLogic piece) : base(-1, 1, piece, "effect_horse_leech_passive")
        {
        }

        public AfterActionPriority Priority => AfterActionPriority.Kill;

        public void OnCallAfterPieceAction(Action.Action action)
        {
            if (action is not ICaptures || action.Result != ResultFlag.Success || action.GetMaker() != Piece) return;

            ActionManager.EnqueueAction(new ApplyEffect(new Bleeding(4, action.GetTarget()), Piece));
            ActionManager.EnqueueAction(new KillPiece(Piece.Pos));
            action.Result = ResultFlag.SelfDestroy;
        }


        public override int GetValueForAI()
        {
            return base.GetValueForAI() + 0;
        }
    }
}