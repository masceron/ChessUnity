namespace Game.Action.Internal.Pending
{
    /* Dành cho những thao tác chọn mục tiêu phức tạp, ví dụ chọn 2 mục tiêu, 
        cần logic riêng không ExecuteAction ngay sau khi chọn Target */
    public abstract class PendingAction : Action
    {
        protected PendingAction(int maker) : base(maker)
        {}

        protected sealed override void ModifyGameState()
        {}

        public abstract void CompleteAction();
    }
}