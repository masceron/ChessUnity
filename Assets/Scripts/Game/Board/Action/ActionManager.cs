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

        public static void EnqueueAction(Action queueAction)
        {
            if (queueAction.GetType() != typeof(EndTurn))
            {
                _actionQueue.Enqueue(queueAction);
            }
            else
            {
                var actionTaken = new Queue<Action>(_actionQueue);
                _actionQueue.Clear();
                /*
                
                The process of executing actions made on a board is as follows:
                Every action user take in a turn will be queued and execute sequentially when an EndTurn action is registered.
                Action that is the result of another action will also be queued.
                For example: A piece tries to capture another piece with Carapace buff, and is killed in the process.
                The queue will be: Capture -- CarapaceKill -- EndTurn.
                When an EndTurn action is registered, the queue will start execute all action taken before, enqueue the actions taken by observers, manage cooldowns of effect,
                and finally execute the EndTurn itself.
                
                */
                
                while (actionTaken.TryDequeue(out var action))
                {
                    if (action is not IInternal)
                    {
                        EventObserver.Notify(action);
                    }
                    action.ApplyAction(_state);
                }

                _state.EffectCountdown();
                EventObserver.Notify(queueAction);
                _actionQueue.Enqueue(queueAction);

                while (_actionQueue.TryDequeue(out var action))
                {
                    action.ApplyAction(_state);
                }
            }
            
        }

        public static void ExecuteImmediately(Action action)
        {
            action.ApplyAction(_state);
        }

        public static void Init()
        {
            _state = MatchManager.GameState;
            _actionQueue = new Queue<Action>();
        }
    }
}