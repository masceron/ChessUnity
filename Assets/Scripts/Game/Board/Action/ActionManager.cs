using System.Collections.Generic;
using Game.Board.Action.Internal;
using Game.Board.General;

namespace Game.Board.Action
{
    [Il2CppSetOption(Option.NullChecks, false), Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public static class ActionManager
    {
        private static GameState _state;
        private static Queue<Action> _actionQueue;

        private static void EnqueueAction(Action _action)
        {
            _actionQueue.Enqueue(_action);

            if (_action.GetType() != typeof(EndTurn)) return;
            while (_actionQueue.TryDequeue(out var action))
            {
                Execute(action);
            }
        }

        private static void ExecuteImmediately(Action action)
        {
            action.ApplyAction(_state);
        }

        public static void TakeAction(Action action)
        {
            if (action is IInternal)
            {
                ExecuteImmediately(action);
            }
            else
            {
                EnqueueAction(action);
                EnqueueAction(new EndTurn());
            }
        }

        public static void Init()
        {
            _state = MatchManager.GameState;
            _actionQueue = new Queue<Action>();
        }
        private static void Execute(Action action)
        {
            if (action.GetType() != typeof(EndTurn))
            {
                if (action is not IInternal)
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