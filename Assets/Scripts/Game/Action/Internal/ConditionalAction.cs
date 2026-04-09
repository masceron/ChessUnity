namespace Game.Action.Internal
{
    public class ConditionalAction: Action, IInternal
    {
        private readonly Action _prerequisite;
        private readonly Action _onSuccess;
        
        public ConditionalAction(Action prerequisite, Action onSuccess) : base(null)
        {
            _prerequisite = prerequisite;
            _onSuccess = onSuccess;
        }

        protected override void ModifyGameState()
        {
            if (_prerequisite.Result == ResultFlag.Success)
            {
                ActionManager.EnqueueAction(_onSuccess);
            }
        }
    }
}