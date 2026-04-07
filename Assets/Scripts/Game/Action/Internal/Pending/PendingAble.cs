using System;
using Cysharp.Threading.Tasks;
using Game.Common;

namespace Game.Action.Internal.Pending
{
    /* Dành cho những thao tác chọn mục tiêu phức tạp, ví dụ chọn 2 mục tiêu,
        cần logic riêng không ExecuteAction ngay sau khi chọn Target */
    public abstract class PendingAction : Action, IInternal
    {
        private UniTaskCompletionSource<Action> _task;

        protected PendingAction(Entity maker) : base(maker)
        {
        }

        protected PendingAction(Entity maker, int target) : base(maker, target)
        {
        }

        protected PendingAction(Entity maker, Entity target) : base(maker, target)
        {
            
        }

        protected sealed override void ModifyGameState()
        {
        }

        public async UniTask<Action> WaitForCompletion(ITargetingContext context)
        {
            try
            {
                return await BuildAction(context);
            }
            catch (OperationCanceledException)
            {
                return null; 
            }
        }
        
        protected abstract UniTask<Action> BuildAction(ITargetingContext context);
    }
}