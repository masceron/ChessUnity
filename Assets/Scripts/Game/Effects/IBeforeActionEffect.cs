namespace Game.Effects
{
    public interface IBeforePieceActionEffect
    {
        public void OnCallBeforePieceAction(Action.Action action);
    }
}