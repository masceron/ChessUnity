using System.Collections.Generic;
using Game.Action.Internal;
using Game.Action.Relics;
using Game.Effects;
using Game.Piece.PieceLogic.Commons;
using UnityEngine;

namespace Game.Managers
{
    public class EffectHooks
    {
        private readonly List<IOnApply> onApplies = new();
        private readonly List<IOnRemove> onRemoves = new();
        private readonly List<IOnMoveGenEffect> onMoveGens = new();
        private readonly List<IAfterPieceActionEffect> afterPieceActions = new();
        private readonly List<IBeforePieceActionEffect> beforePieceActions = new();
        private readonly List<IBeforeRelicActionEffect> beforeRelicActions = new();
        private readonly List<IAfterRelicActionEffect> afterRelicActions = new();
        private readonly List<IDeadEffect> onDies = new();
        private readonly List<IStartTurnEffect> onStartTurns = new();
        private readonly List<IEndTurnEffect> onEndTurns = new();
        private readonly List<IApplyEffect> onEffectApplies = new();
        private readonly List<IMoveRangeModifier> onMoveRanges = new();

        private static void AddToList<T>(List<T> list, T effect)
        {
            var pos = list.BinarySearch(effect, Effect.GetComparer<T>());
            list.Insert(pos >= 0 ? pos : ~pos, effect);
        }

        private static void RemoveFromList<T>(List<T> list, T effect)
        {
            list.Remove(effect);
        }

        public void AddObserver(Effect effect)
        {
            if (effect is IOnApply onApply)
                AddToList(onApplies, onApply);
            if (effect is IOnRemove onRemove)
                AddToList(onRemoves, onRemove);
            if (effect is IOnMoveGenEffect onMoveGenEffect)
                AddToList(onMoveGens, onMoveGenEffect);
            if (effect is IBeforePieceActionEffect beforePieceAction)
                AddToList(beforePieceActions, beforePieceAction);
            if (effect is IAfterPieceActionEffect afterPieceAction)
                AddToList(afterPieceActions, afterPieceAction);
            if (effect is IBeforeRelicActionEffect beforeRelicActionEffect)
                AddToList(beforeRelicActions, beforeRelicActionEffect);
            if (effect is IAfterRelicActionEffect afterRelicActionEffect)
                AddToList(afterRelicActions, afterRelicActionEffect);
            if (effect is IDeadEffect deadEffect)
                AddToList(onDies, deadEffect);
            if (effect is IStartTurnEffect startTurnEffect)
                AddToList(onStartTurns, startTurnEffect);
            if (effect is IEndTurnEffect endTurnEffect)
                AddToList(onEndTurns, endTurnEffect);
            if (effect is IApplyEffect applyEffect)
                AddToList(onEffectApplies, applyEffect);
            if (effect is IMoveRangeModifier moveRangeModifier)
                AddToList(onMoveRanges, moveRangeModifier);
        }

        public void RemoveObserver(Effect effect)
        {
            if (effect is IOnApply onApply)
                RemoveFromList(onApplies, onApply);
            if (effect is IOnRemove onRemove)
                RemoveFromList(onRemoves, onRemove);
            if (effect is IOnMoveGenEffect onMoveGenEffect)
                RemoveFromList(onMoveGens, onMoveGenEffect);
            if (effect is IBeforePieceActionEffect beforePieceAction)
                RemoveFromList(beforePieceActions, beforePieceAction);
            if (effect is IAfterPieceActionEffect afterPieceAction)
                RemoveFromList(afterPieceActions, afterPieceAction);
            if (effect is IBeforeRelicActionEffect beforeRelicActionEffect)
                RemoveFromList(beforeRelicActions, beforeRelicActionEffect);
            if (effect is IAfterRelicActionEffect afterRelicActionEffect)
                RemoveFromList(afterRelicActions, afterRelicActionEffect);
            if (effect is IDeadEffect deadEffect)
                RemoveFromList(onDies, deadEffect);
            if (effect is IStartTurnEffect startTurnEffect)
                RemoveFromList(onStartTurns, startTurnEffect);
            if (effect is IEndTurnEffect endTurnEffect)
                RemoveFromList(onEndTurns, endTurnEffect);
            if (effect is IApplyEffect applyEffect)
                RemoveFromList(onEffectApplies, applyEffect);
            if (effect is IMoveRangeModifier moveRangeModifier)
                RemoveFromList(onMoveRanges, moveRangeModifier);
        }

        public void NotifyEnd(Action.Action mainAction, bool sideToMove)
        {
            onEndTurns.ForEach(effect =>
            {
                if (((Effect)effect).disabled) return;
                if (effect.EndTurnEffectType == EndTurnEffectType.EndOfAnyTurn)
                {
                    effect.OnCallEnd(mainAction);
                }
        
                //The next turn is ours.
                else if (sideToMove == ((Effect)effect).Piece.Color)
                {
                    if (effect.EndTurnEffectType == EndTurnEffectType.EndOfEnemyTurn)
                    {
                        effect.OnCallEnd(mainAction);
                    }
                }
                //The next turn is of the opponent.
                else
                {
                    if (effect.EndTurnEffectType == EndTurnEffectType.EndOfAllyTurn)
                    {
                        effect.OnCallEnd(mainAction);
                    }
                }
            });
        }
        
        public void NotifyStart(Action.Action mainAction, bool sideToMove)
        {
            onStartTurns.ForEach(effect =>
            {
                if (((Effect)effect).disabled) return;
                if (effect.StartTurnEffectType == StartTurnEffectType.StartOfAnyTurn)
                {
                    effect.OnCallStart(mainAction);
                }
        
                //The next turn is ours.
                else if (sideToMove == ((Effect)effect).Piece.Color)
                {
                    if (effect.StartTurnEffectType == StartTurnEffectType.StartOfAllyTurn)
                    {
                        effect.OnCallStart(mainAction);
                    }
                }
                //The next turn is of the opponent.
                else
                {
                    if (effect.StartTurnEffectType == StartTurnEffectType.StartOfEnemyTurn)
                    {
                        effect.OnCallStart(mainAction);
                    }
                }
            });
        }
        
        public void NotifyDead(PieceLogic pieceToDie)
        {
            onDies.ForEach (effect =>
            {
                if (((Effect)effect).disabled) return;
                effect.OnCallDead(pieceToDie);
            });
        }
        
        public void NotifyOnMoveGen(List<Action.Action> actions)
        {
            onMoveGens.ForEach(e =>
            {
                if (((Effect)e).disabled) return;
                e.OnCallMoveGen(actions);
            });
        }
        
        public void NotifyWhenApplyEffect(ApplyEffect action)
        {
            onEffectApplies.ForEach(e =>
            {
                if (((Effect)e).disabled) return;
                e.OnCallApplyEffect(action);
            });
        }

        public void NotifyBeforePieceAction(Action.Action action)
        {
            beforePieceActions.ForEach(e =>
            {
                if (((Effect)e).disabled) return;
                e.OnCallBeforePieceAction(action);
            });
        }
        
        public void NotifyAfterPieceAction(Action.Action action)
        {
            afterPieceActions.ForEach(e =>
            {
                if (((Effect)e).disabled) return;
                e.OnCallAfterPieceAction(action);
            });
        }

        public void NotifyBeforeRelicAction(IRelicAction relicAction)
        {
            beforeRelicActions.ForEach(e =>
            {
                if (((Effect)e).disabled) return;
                e.OnCallBeforeRelicAction(relicAction);
            });
        }
        
        public void NotifyAfterRelicAction(IRelicAction relicAction)
        {
            afterRelicActions.ForEach(e =>
            {
                if (((Effect)e).disabled) return;
                e.OnCallAfterRelicAction(relicAction);
            });
        }
    }
}