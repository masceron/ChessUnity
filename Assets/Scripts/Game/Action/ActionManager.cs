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
    
/*
 * TODO: Issue: If a trigger causes a secondary effect that makes the trigger behind it in the queue invalid,
 * the latter should not be active.
 * Right now only a workaround for the case in which the piece is killed is implemented through isDead property and die() method.
 * A more general solution needed for every situations, which now includes:
 * - When the piece is killed.
 * - When the piece moved away.
 * ... And many other cases, when the piece is no longer eligible for trigger activation.
*/

    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
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
            while (_actionQueue.TryDequeue(out var action)) action.Execute();
        }

        private static void ProcessActionWithTriggers()
        {
            var mainAction = _actionQueue.Dequeue();
            if (mainAction is not IRelicAction)
            {
                BoardUtils.NotifyMainAction(mainAction);
            }

            while (_actionQueue.TryDequeue(out var action))
            {
                if (action is IInternal)
                    BoardUtils.NotifyInternalAction(mainAction);
                action.Execute();
            }

            if (mainAction is ISkills and not IRelicAction && mainAction.Result == ActionResult.Succeed)
            {
                BoardUtils.IncrementSkillUses(mainAction);
            }
            mainAction.Execute();
        }

        private static void EndTurnProcess(Action mainAction)
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
            BoardUtils.NotifyEnd(mainAction);

            //Execute actions caused by end turn triggers.
            while (_actionQueue.TryDequeue(out var action))
            {
                BoardUtils.NotifyInternalAction(action);
                action.Execute();
            }

            CurrentPhase = Phase.BeforeEndTurn;
        }

        public static bool DoManualAction(Action action)
        {
            _actionQueue.Enqueue(action);
            ProcessActionWithTriggers();

            switch (action)
            {
                case IRelicAction:
                    return false;
                case ISkills:
                {
                    var maker = BoardUtils.PieceOn(action.Maker);
                    var hasQuickReflex = maker?.Effects.OfType<QuickReflex>().Any() == true;

                    if (hasQuickReflex)
                        return false;
                    break;
                }
            }

            EndTurnProcess(action);
            return true;
        }


        public static void EnqueueAction(Action queueAction)
        {
            _actionQueue.Enqueue(queueAction);
        }

        public static void ExecuteImmediately(Action action)
        {
            action.Execute();
        }
    }
}