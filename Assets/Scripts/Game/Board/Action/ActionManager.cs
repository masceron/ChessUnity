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
        private static Action _lastMainAction;

        public static void Init(GameState state)
        {
            _state = state;
            _actionQueue = new Queue<Action>();
            _lastMainAction = null;
        }

        public static void ExecuteWhenStart()
        {
            while (_actionQueue.TryDequeue(out var action))
            {
                action.Execute();
            }
        }

        public static void EnqueueAction(Action queueAction)
        {
            if (queueAction.GetType() != typeof(EndTurn))
            {
                _actionQueue.Enqueue(queueAction);
            }
            else
            {
                //Execute main action and its follow up.
                while (_actionQueue.TryDequeue(out var action))
                {
                    if (action is not IInternal)
                    {
                        _lastMainAction = action;
                        EventObserver.Notify(_lastMainAction);
                    }
                    action.Execute();
                }
                
                //End the current turn.
                queueAction.Execute();
                
                //Process durations.
                _state.EffectCountdown();
                
                //Call triggers when ending turn.
                EventObserver.Notify(queueAction);
                
                _lastMainAction = null;

                //Execute actions caused by end turn triggers.
                while (_actionQueue.TryDequeue(out var action))
                {
                    action.Execute();
                }
            }
            
        }

        public static void ExecuteImmediately(Action action)
        {
            action.Execute();
        }
    }
}