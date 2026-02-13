using System.Collections.Generic;
using Game.Action.Internal;
using Game.Action.Relics;
using Game.Action.Skills;
using Game.Common;
using Game.Effects;
using Game.Effects.Traits;
using Game.Effects.Triggers;
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
        private static List<StackAction> _buffer;
        private static bool _buffering;
        public static Phase CurrentPhase;

        public static void Init(GameState state)
        {
            _state = state;
            _actionStack = new Stack<StackAction>();
            _buffer = new List<StackAction>();
            _buffering = false;
            CurrentPhase = Phase.BeforeEndTurn;
        }

        public static void ExecuteWhenStart()
        {
            StartTurnProcess(new SkipTurn());
        }

        private static void AfterActionResolve(Action mainAction)
        {
            var afterPieceActionListeners = BoardUtils.GetEffectHookList<IAfterPieceActionTrigger>();
            var afterRelicActionListeners = BoardUtils.GetEffectHookList<IAfterRelicActionTrigger>();

            if (mainAction is IRelicAction iRelicAction)
                foreach (var listener in afterRelicActionListeners)
                {
                    _buffering = true;
                    listener.OnCallAfterRelicAction(iRelicAction);
                    _buffering = false;
                    FlushBuffer();

                    ProcessStack();
                }
            else if (mainAction is not IInternal)
                foreach (var listener in afterPieceActionListeners.Where(listener =>
                             BoardUtils.IsAlive(((Effect)listener).Piece)))
                {
                    _buffering = true;
                    listener.OnCallAfterPieceAction(mainAction);
                    _buffering = false;
                    FlushBuffer();

                    ProcessStack();
                }
        }

        private static void ProcessStack(int targetDepth = 0)
        {
            while (_actionStack.Count > targetDepth)
            {
                var currentActionStack = _actionStack.Peek();
                var currentAction = currentActionStack.Action;

                if (!currentActionStack.TriggerCalled)
                {
                    currentActionStack.TriggerCalled = true;

                    _buffering = true;
                    switch (currentAction)
                    {
                        case IInternal action: BoardUtils.NotifyInternalAction(action); break;
                        case IRelicAction relicAction: BoardUtils.NotifyBeforeRelicAction(relicAction); break;
                        default: BoardUtils.NotifyBeforePieceAction(currentAction); break;
                    }

                    _buffering = false;
                    FlushBuffer();

                    if (_actionStack.Peek() != currentActionStack) continue;
                }

                _actionStack.Pop();

                if (currentAction.IsValid())
                {
                    _buffering = true;
                    currentAction.Execute();
                    _buffering = false;

                    FlushBuffer();
                }

                AfterActionResolve(currentAction);
            }
        }

        private static void StartTurnProcess(Action mainAction)
        {
            CurrentPhase = Phase.BeforeEndTurn;
            var startTurnListeners = BoardUtils.GetEffectHookList<IStartTurnTrigger>();

            foreach (var effect in startTurnListeners)
            {
                if (!BoardUtils.IsAlive(((Effect)effect).Piece) || ((Effect)effect).disabled) continue;

                var shouldTrigger = false;

                if (effect.StartTurnEffectType == StartTurnEffectType.StartOfAnyTurn)
                {
                    shouldTrigger = true;
                }
                else if (BoardUtils.SideToMove() == ((Observer)effect).Color)
                {
                    if (effect.StartTurnEffectType == StartTurnEffectType.StartOfAllyTurn) shouldTrigger = true;
                }
                else
                {
                    if (effect.StartTurnEffectType == StartTurnEffectType.StartOfEnemyTurn) shouldTrigger = true;
                }

                if (!shouldTrigger) continue;

                _buffering = true;
                effect.OnCallStart(mainAction);
                _buffering = false;
                FlushBuffer();

                ProcessStack();
            }
        }

        private static void EndTurnProcess(Action mainAction)
        {
            new EndTurn().Execute();
            CurrentPhase = Phase.AfterEndTurn;

            var endTurnListeners = BoardUtils.GetEffectHookList<IEndTurnTrigger>();

            endTurnListeners.ForEach(effect =>
            {
                if (!BoardUtils.IsAlive(((Effect)effect).Piece) || ((Effect)effect).disabled) return;

                var shouldTrigger = false;
                if (effect.EndTurnEffectType == EndTurnEffectType.EndOfAnyTurn)
                {
                    shouldTrigger = true;
                }
                else if (BoardUtils.SideToMove() == ((Effect)effect).Piece.Color)
                {
                    if (effect.EndTurnEffectType == EndTurnEffectType.EndOfEnemyTurn) shouldTrigger = true;
                }
                else
                {
                    if (effect.EndTurnEffectType == EndTurnEffectType.EndOfAllyTurn) shouldTrigger = true;
                }

                if (!shouldTrigger) return;
                var depthBefore = _actionStack.Count;

                _buffering = true;
                effect.OnCallEnd(mainAction);
                _buffering = false;

                FlushBuffer();

                ProcessStack(depthBefore);
            });

            var countdownDepth = _actionStack.Count;
            _buffering = true;
            _state.EffectCountdown();
            _buffering = false;
            FlushBuffer();
            ProcessStack(countdownDepth);
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
            if (_buffering)
                _buffer.Add(new StackAction(queueAction));
            else
                _actionStack.Push(new StackAction(queueAction));
        }

        private static void FlushBuffer()
        {
            if (_buffer.Count == 0) return;

            for (var i = _buffer.Count - 1; i >= 0; i--) _actionStack.Push(_buffer[i]);

            _buffer.Clear();
        }

        public static void ExecuteImmediately(Action action)
        {
            action.Execute();
        }
    }
}