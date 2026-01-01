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
        BeforeEndTurn,
        AfterEndTurn
    }

    public class StackAction
    {
        public readonly Action Action;
        public bool TriggerCalled;

        public StackAction(Action action)
        {
            Action = action;
        }
    }

    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public static class ActionManager
    {
        private static GameState _state;
        private static Stack<StackAction> _actionStack;
        public static Phase CurrentPhase;

        public static void Init(GameState state)
        {
            _state = state;
            _actionStack = new Stack<StackAction>();
            CurrentPhase = Phase.BeforeEndTurn;
        }

        public static void ExecuteWhenStart()
        {
            while (_actionStack.TryPop(out var stackAction)) stackAction.Action.Execute();
        }

        private static void ProcessStack()
        {
            while (_actionStack.Count > 0)
            {
                var currentActionStack = _actionStack.Peek();
                var currentAction = currentActionStack.Action;

                if (!currentActionStack.TriggerCalled)
                {
                    currentActionStack.TriggerCalled = true;

                    switch (currentAction)
                    {
                        case IInternal action:
                            BoardUtils.NotifyInternalAction(action);
                            break;
                        case IRelicAction relicAction:
                            BoardUtils.NotifyBeforeRelicAction(relicAction);
                            break;
                        default:
                            BoardUtils.NotifyBeforePieceAction(currentAction);
                            break;
                    }

                    if (_actionStack.Peek() != currentActionStack) continue;
                }
                
                _actionStack.Pop();

                if (currentAction.IsValid())
                {
                    currentAction.Execute();
                }
                
                if (currentAction is IRelicAction iRelicAction)
                {
                    BoardUtils.NotifyAfterRelicAction(iRelicAction);
                }
                else if (currentAction is not IInternal)
                {
                    BoardUtils.NotifyAfterPieceAction(currentAction);
                }
            }
        }

        private static bool ShouldEndTurn(Action action)
        {
            switch (action)
            {
                case IRelicAction:
                    return false;
                case ISkills:
                {
                    var maker = BoardUtils.PieceOn(action.Maker);
                    var hasQuickReflex = maker?.Effects.OfType<QuickReflex>().Any() == true;

                    if (hasQuickReflex || action is IDontEndTurn)
                        return false;
                    break;
                }
            }

            return true;
        }

        private static void EndTurnProcess(Action mainAction)
        {
            new EndTurn().Execute();
            CurrentPhase = Phase.AfterEndTurn;
            
            BoardUtils.NotifyOnEndTurn(mainAction);
            ProcessStack();
            
            _state.EffectCountdown();
            ProcessStack();
            
            CurrentPhase = Phase.BeforeEndTurn;
            BoardUtils.NotifyOnStartTurn(mainAction);
            ProcessStack();
        }

        public static bool DoManualAction(Action action)
        {
            _actionStack.Push(new StackAction(action));
            ProcessStack();

            if (!ShouldEndTurn(action)) return false;
            
            EndTurnProcess(action);
            return true;
        }

        public static void EnqueueAction(Action queueAction)
        {
            _actionStack.Push(new StackAction(queueAction));
        }

        public static void ExecuteImmediately(Action action)
        {
            action.Execute();
        }
    }
}