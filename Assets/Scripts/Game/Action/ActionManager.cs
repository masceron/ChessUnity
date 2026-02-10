using System.Collections.Generic;
using Game.Action.Internal;
using Game.Action.Relics;
using Game.Action.Skills;
using Game.Common;
using Game.Effects;
using Game.Effects.Traits;
using Game.Managers;
using ZLinq;

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
            StartTurnProcess(new SkipTurn());
        }

        private static void AfterActionResolve(Action mainAction)
        {
            var afterPieceActionListeners = BoardUtils.GetEffectHookList<IAfterPieceActionEffect>();
            var afterRelicActionListeners = BoardUtils.GetEffectHookList<IAfterRelicActionEffect>();

            if (mainAction is IRelicAction iRelicAction)
            {
                foreach (var listener in afterRelicActionListeners)
                {
                    // if (!BoardUtils.IsAlive(((Effect)listener).Piece)) continue;
                    listener.OnCallAfterRelicAction(iRelicAction);
                    ProcessStack();
                }
            }
            else if (mainAction is not IInternal)
            {
                foreach (var listener in afterPieceActionListeners)
                {
                    // if (!BoardUtils.IsAlive(((Effect)listener).Piece)) continue;
                    listener.OnCallAfterPieceAction(mainAction);
                    ProcessStack();
                }
            }
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
                    ProcessStack();
                }

                AfterActionResolve(currentAction);
            }
        }

        private static void StartTurnProcess(Action mainAction)
        {
            CurrentPhase = Phase.BeforeEndTurn;

            var startTurnListeners = BoardUtils.GetEffectHookList<IStartTurnEffect>();
            
            startTurnListeners.ForEach(effect =>
            {
                // if (!BoardUtils.IsAlive(((Effect)effect).Piece) || ((Effect)effect).disabled) return;
                if (effect.StartTurnEffectType == StartTurnEffectType.StartOfAnyTurn)
                {
                    effect.OnCallStart(mainAction);
                    ProcessStack();
                }
                //The next turn is ours.
                else if (BoardUtils.SideToMove() == ((Observer)effect).Color)
                {
                    if (effect.StartTurnEffectType != StartTurnEffectType.StartOfAllyTurn) return;
                    
                    effect.OnCallStart(mainAction);
                    ProcessStack();
                }
                //The next turn is of the opponent.
                else
                {
                    if (effect.StartTurnEffectType != StartTurnEffectType.StartOfEnemyTurn) return;
                    
                    effect.OnCallStart(mainAction);
                    ProcessStack();
                }
            });
        }

        private static void EndTurnProcess(Action mainAction)
        {
            new EndTurn().Execute();
            CurrentPhase = Phase.AfterEndTurn;

            var endTurnListeners = BoardUtils.GetEffectHookList<IEndTurnEffect>();
            endTurnListeners.ForEach(effect =>
            {
                if (!BoardUtils.IsAlive(((Effect)effect).Piece) || ((Effect)effect).disabled) return;
                if (effect.EndTurnEffectType == EndTurnEffectType.EndOfAnyTurn)
                {
                    effect.OnCallEnd(mainAction);
                    ProcessStack();
                }
                //The next turn is ours.
                else if (BoardUtils.SideToMove() == ((Effect)effect).Piece.Color)
                {
                    if (effect.EndTurnEffectType == EndTurnEffectType.EndOfEnemyTurn)
                    {
                        effect.OnCallEnd(mainAction);
                        ProcessStack();
                    }
                }
                //The next turn is of the opponent.
                else
                {
                    if (effect.EndTurnEffectType == EndTurnEffectType.EndOfAllyTurn)
                    {
                        effect.OnCallEnd(mainAction);
                        ProcessStack();
                    }
                }
            });
            
            _state.EffectCountdown();
            ProcessStack();
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

        public static bool DoManualAction(Action action)
        {
            _actionStack.Push(new StackAction(action));
            ProcessStack();

            if (!ShouldEndTurn(action)) return false;
            
            EndTurnProcess(action);
            StartTurnProcess(action);
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