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

    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public static class ActionManager
    {
        private static GameState _state;
        private static Queue<Action> _actionQueue;
        public static Phase CurrentPhase;

        public static int WhiteSkillUses;
        public static int BlackSkillUses;

        public static void Init(GameState state)
        {
            WhiteSkillUses = 0;
            BlackSkillUses = 0;
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
            while (_actionQueue.TryDequeue(out var action))
            {
                if (action is IInternal)
                    BoardUtils.NotifyInternalAction(action);
                else if (action is not IRelicAction)
                    BoardUtils.NotifyMainAction(action);

                IncrementSkillUses(action);
                action.Execute();
            }

            while (_actionQueue.TryDequeue(out var action)) {
                IncrementSkillUses(action);
                action.Execute();
            }
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

        public static bool EnqueueAction(Action queueAction)
        {
            _actionQueue.Enqueue(queueAction);
            if (queueAction is IInternal) return false;

            ProcessActionWithTriggers();

            //End the turn if:
            //The action is a SkipTurn, or
            //The action is not from a relic, and if it's a skill, then the piece making it cannot have Quick Reflex.
            if (queueAction is not SkipTurn &&
                (queueAction is IRelicAction
                 || queueAction is ISkills &&
                 BoardUtils.PieceOn(queueAction.Maker).Effects.OfType<QuickReflex>().Any())) return false;
            
            EndTurnProcess(queueAction);
            return true;

        }
        public static void IncrementSkillUses(Action action)
        {

            if (action is ISkills && action is not IRelicAction)
            {
                var makerPiece = _state.PieceBoard[action.Maker];
                if (makerPiece != null)
                {
                    if (makerPiece.Color) BlackSkillUses++;
                    else WhiteSkillUses++;
                }
            }
        }

        public static void ExecuteImmediately(Action action)
        {
            IncrementSkillUses(action);
            action.Execute();
        }
    }
}