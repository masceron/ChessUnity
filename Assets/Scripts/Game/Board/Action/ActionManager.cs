using Game.Board.General;

namespace Game.Board.Action
{
    [Il2CppSetOption(Option.NullChecks, false), Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public static class ActionManager
    {
        private static GameState _state;
        private static Action _lastAction;

        public static void Init()
        {
            _state = MatchManager.GameState;
        }
        public static void Execute(Action action)
        {
            if (action.GetType() != typeof(EndTurn))
            {
                if (!action.IsInternal)
                {
                    EventObserver.Notify(action);
                }
                action.ApplyAction(_state);
            }
            else
            {
                action.ApplyAction(_state);
                EventObserver.Notify(action);
            }
        }
    }
}