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
                /*
                
                The process of executing actions made on a board is as follows:
                Every action user take in a turn will be queued and execute sequentially when an EndTurn action is registered.
                Action that is the result of another action will also be queued.
                For example: A piece tries to capture another piece with Carapace buff, and is killed in the process.
                The queue will be: Capture -- CarapaceKill -- EndTurn.
                When an EndTurn action is registered, the queue will start execute all action taken before, enqueue the actions taken by observers, manage cooldowns of effect,
                and finally execute the EndTurn itself.
                
                */
                
                while (_actionQueue.TryDequeue(out var action))
                {
                    if (action is not IInternal)
                    {
                        _lastMainAction = action;
                        EventObserver.Notify(_lastMainAction);
                    }
                    action.Execute();
                }

                _state.EffectCountdown();
                
                _actionQueue.Enqueue(queueAction);
                EventObserver.Notify(queueAction);
                
                _lastMainAction = null;

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