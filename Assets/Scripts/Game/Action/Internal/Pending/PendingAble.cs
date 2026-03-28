using Cysharp.Threading.Tasks;
using UnityEngine.InputSystem;

namespace Game.Action.Internal.Pending
{
    /* Dành cho những thao tác chọn mục tiêu phức tạp, ví dụ chọn 2 mục tiêu,
        cần logic riêng không ExecuteAction ngay sau khi chọn Target */
    public abstract class PendingAction : Action, IInternal
    {
        private UniTaskCompletionSource<Action> _task;

        protected PendingAction(int maker) : base(maker)
        {
        }

        protected PendingAction(int maker, int target) : base(maker, target)
        {
        }

        protected PendingAction(int maker, int target, TargetingType targetingType) : base(maker, target, targetingType)
        {
        }

        protected sealed override void ModifyGameState()
        {
        }

        public UniTask<Action> WaitForCompletion()
        {
            _task = new UniTaskCompletionSource<Action>();
            CompleteAction();

            return _task.Task;
        }

        public void CommitResult(Action action)
        {
            _task.TrySetResult(action);
        }

        public void CancelResult()
        {
            if (_task == null) return;
            _task.TrySetCanceled();
        }

        protected abstract void CompleteAction();
    }
}