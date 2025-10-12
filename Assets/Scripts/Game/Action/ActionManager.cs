using System.Collections.Generic;
using System.Linq;
using Game.Action.Internal;
using Game.Action.Relics;
using Game.Action.Skills;
using Game.Common;
using Game.Effects.Traits;
using Game.Managers;
using UnityEngine;

namespace Game.Action
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

        private static void ProcessActionWithTriggers()
        {
            while (_actionQueue.TryDequeue(out var action))
            {
                if (action is not IInternal and not IRelicAction)
                {
                    BoardUtils.SetMainAction(action);
                    BoardUtils.NotifyMainAction();
                }
                else
                {
                    BoardUtils.NotifyInternalAction(action);
                }
                action.Execute();
            }

            while (_actionQueue.TryDequeue(out var action))
            {
                action.Execute();
            }
        }

        private static void EndTurnProcess()
        {
            //End the current turn.
            new EndTurn().Execute();
            CurrentPhase = Phase.AfterEndTurn;
                
            //Process durations.
            _state.EffectCountdown();
            while (_actionQueue.TryDequeue(out var action))
            {
                BoardUtils.NotifyInternalAction(action);
                action.Execute();
            }
            
            //Call triggers when ending turn.
            BoardUtils.NotifyEnd();

            //Execute actions caused by end turn triggers.
            while (_actionQueue.TryDequeue(out var action))
            {
                BoardUtils.NotifyInternalAction(action);
                action.Execute();
            }

            CurrentPhase = Phase.BeforeEndTurn;
        }
        
        public static void EnqueueAction(Action queueAction)
        {
            _actionQueue.Enqueue(queueAction);
            if (queueAction is IInternal) return;
            
            var maker = BoardUtils.PieceOn(queueAction.Maker);
            ProcessActionWithTriggers();
            if (queueAction is not IRelicAction && !(maker.Effects.OfType<QuickReflex>().Any() && queueAction is ISkills))
            {
                EndTurnProcess();
            }
        }

        public static void ExecuteImmediately(Action action)
        {
            action.Execute();
        }
    }
}