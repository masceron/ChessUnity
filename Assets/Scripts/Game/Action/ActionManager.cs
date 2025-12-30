using System.Collections.Generic;
using System.Linq;
using Game.Action.Internal;
using Game.Action.Relics;
using Game.Action.Skills;
using Game.Common;
using Game.Effects.Traits;
using Game.Managers;


namespace Game.Action
{
    public enum Phase
    {
        BeforeEndTurn,
        AfterEndTurn
    }

    public struct StackAction
    {
        public readonly Action Action;
        public bool TriggerCalled;

        public StackAction(Action action)
        {
            Action = action;
            TriggerCalled = false;
        }
        
        public static bool operator !=(StackAction s1, StackAction s2) 
        {
            return !ReferenceEquals(s1.Action, s2.Action);
        }

        public static bool operator ==(StackAction s1, StackAction s2)
        {
            return ReferenceEquals(s1.Action, s2.Action);
        }

        private bool Equals(StackAction other)
        {
            return ReferenceEquals(Action, other.Action);
        }

        public override bool Equals(object obj)
        {
            return obj is StackAction other && Equals(other);
        }

        public override int GetHashCode()
        {
            return Action.GetHashCode();
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
                var currentAction = _actionStack.Peek();
                
                if (!currentAction.TriggerCalled)
                {
                    currentAction.TriggerCalled = true;

                    if (currentAction.Action is IInternal)
                    {
                        BoardUtils.NotifyInternalAction(currentAction.Action);
                    }
                    
                    if (_actionStack.Peek() != currentAction)
                    {
                        continue; 
                    }
                }
                
                if (!currentAction.Action.IsValid())
                {
                    _actionStack.Pop();
                    continue;
                }
                
                _actionStack.Pop(); 
                currentAction.Action.Execute();
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
            
            BoardUtils.NotifyEnd(mainAction);
            ProcessStack();
            
            _state.EffectCountdown();
            ProcessStack();
            
            CurrentPhase = Phase.BeforeEndTurn;
            BoardUtils.NotifyStart(mainAction);
            ProcessStack();
        }
        
        private static void ProcessMainActionSequence()
        {
            if (_actionStack.Count == 0) return;

            var mainAction = _actionStack.Peek().Action;
            
            if (mainAction is not IRelicAction)
            {
                BoardUtils.NotifyMainAction(mainAction);
            }
            
            ProcessStack();
            
            if (mainAction is ISkills and not IRelicAction && mainAction.Result == ResultFlag.Success)
            {
                BoardUtils.IncrementSkillUses(mainAction);
            }
        }

        public static bool DoManualAction(Action action)
        {
            _actionStack.Push(new StackAction(action));
            ProcessMainActionSequence();

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