namespace Game.Action.Internal
{
    public class Block : Action, IInternal
    {
        public readonly Action failedAction;
        public Block(int defender, Action failedAction) : base(defender)
        {
            this.failedAction = failedAction;
            Target = failedAction.Maker;
            failedAction.Succeed = false;
        }
        protected override void ModifyGameState()
        {
            // Do nothing
        }
    }
}