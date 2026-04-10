using Game.Action;
using Game.Action.Captures;
using Game.Piece.PieceLogic.Commons;
using Game.Triggers;

namespace Game.Effects.Condition
{
    /// <summary>
    /// Base class cho các hiệu ứng Hunger.
    /// Trigger sau khi quân này ăn thành công 1 quân địch, gọi OnHungerTriggered() để subclass xử lý.
    /// </summary>
    public abstract class Hunger : Effect, IAfterPieceActionTrigger
    {
        public AfterActionPriority Priority => AfterActionPriority.Other;

        protected Hunger(PieceLogic piece, string effectName) : base(-1, 1, piece, effectName)
        {
        }

        public void OnCallAfterPieceAction(Action.Action action)
        {
            if (action is ICaptures 
                    && action.GetMakerAsPiece() == Piece 
                    && action.Result == ResultFlag.Success
                )
                OnHungerTriggered();
        }

        /// <summary>
        /// Gọi khi quân này vừa ăn thành công 1 quân địch.
        /// Subclass override để áp dụng hiệu ứng tương ứng.
        /// </summary>
        protected abstract void OnHungerTriggered();
    }
}
