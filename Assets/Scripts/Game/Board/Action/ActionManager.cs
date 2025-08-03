using System.Collections.Generic;
using Game.Board.Action.Internal;
using Game.Board.General;
using Game.Common;

namespace Game.Board.Action
{
    public enum Phase
    {
        BeforeEndTurn, AfterEndTurn
    }
    [Il2CppSetOption(Option.NullChecks, false), Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public static class ActionManager
    {
        private static GameState _state;
        private static Queue<Action> _actionQueue;
        public static Phase CurrentPhase;

        public static void Init(GameState state)
        {
            _state = state;
            _actionQueue = new Queue<Action>();
            CurrentPhase = Phase.BeforeEndTurn;
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
                        BoardUtils.SetMainAction(action);
                        BoardUtils.Notify();
                    }
                    action.Execute();
                }
                
                //End the current turn.
                queueAction.Execute();
                CurrentPhase = Phase.AfterEndTurn;
                
                //Process durations.
                _state.EffectCountdown();
                while (_actionQueue.TryDequeue(out var action))
                {
                    action.Execute();
                }
                
                //Call triggers when ending turn.
                BoardUtils.NotifyEnd();

                //Execute actions caused by end turn triggers.
                while (_actionQueue.TryDequeue(out var action))
                {
                    action.Execute();
                }

                CurrentPhase = Phase.BeforeEndTurn;
            }
            
        }

        public static void ExecuteImmediately(Action action)
        {
            action.Execute();
        }
    }
}