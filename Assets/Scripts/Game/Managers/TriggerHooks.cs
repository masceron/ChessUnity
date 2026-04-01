using System;
using System.Collections;
using System.Collections.Generic;
using Game.Action.Internal;
using Game.Action.Relics;
using Game.Piece.PieceLogic.Commons;
using Game.Triggers;
using ZLinq;

namespace Game.Managers
{
    public class TriggerHooks
    {
        private readonly List<IAfterPieceActionTrigger> _afterPieceActions = new();
        private readonly List<IAfterRelicActionTrigger> _afterRelicActions = new();
        private readonly List<IBeforePieceActionTrigger> _beforePieceActions = new();
        private readonly List<IBeforeRelicActionTrigger> _beforeRelicActions = new();

        private readonly Dictionary<Type, HookBinding> _bindings = new();
        private readonly List<IOnApplyTrigger> _onApplies = new();
        private readonly List<IDeadTrigger> _onDies = new();
        private readonly List<IBeforeApplyEffectTrigger> _onEffectApplies = new();
        private readonly List<IEndTurnTrigger> _onEndTurns = new();
        private readonly List<IOnMoveGenTrigger> _onMoveGens = new();
        private readonly List<IMoveRangeModifierTrigger> _onMoveRanges = new();
        private readonly List<IOnRemoveTrigger> _onRemoves = new();
        private readonly List<IOnPieceSpawnedTrigger> _onSpawns = new();
        private readonly List<IStartTurnTrigger> _onStartTurns = new();
        private readonly List<IBeforeDestroyOrKill> _onBeforeDestroyOrKill = new();

        public TriggerHooks()
        {
            Register(_onApplies);
            Register(_onRemoves);
            Register(_onMoveGens);
            Register(_afterPieceActions);
            Register(_beforePieceActions);
            Register(_beforeRelicActions);
            Register(_afterRelicActions);
            Register(_onDies);
            Register(_onStartTurns);
            Register(_onEndTurns);
            Register(_onEffectApplies);
            Register(_onMoveRanges);
            Register(_onSpawns);
            Register(_onBeforeDestroyOrKill);
        }

        private void Register<T>(List<T> list)
        {
            _bindings[typeof(T)] = new HookBinding
            {
                List = list,
                Add = e => AddToList(list, (T)(object)e),
                Remove = e => RemoveFromList(list, (T)(object)e)
            };
        }


        public void AddObserver(Observer effect)
        {
            foreach (var kvp in _bindings.Where(kvp => kvp.Key.IsInstanceOfType(effect))) kvp.Value.Add(effect);
        }

        public void RemoveObserver(Observer effect)
        {
            foreach (var kvp in _bindings.Where(kvp => kvp.Key.IsInstanceOfType(effect))) kvp.Value.Remove(effect);
        }

        public List<T> GetList<T>()
        {
            if (_bindings.TryGetValue(typeof(T), out var binding)) return (List<T>)binding.List;
            return null;
        }

        private static void AddToList<T>(List<T> list, T effect)
        {
            var comparer = Comparer<T>.Default;
            var pos = list.BinarySearch(effect, comparer);

            if (pos >= 0)
                //Making sure the last added item will be inserted at the lowest index possible.
                while (pos > 0 && comparer.Compare(list[pos - 1], effect) == 0)
                    pos--;
            else
                pos = ~pos;

            list.Insert(pos, effect);
        }

        private static void RemoveFromList<T>(List<T> list, T effect)
        {
            list.Remove(effect);
        }

        public void NotifyDead(PieceLogic pieceToDie)
        {
            var snapshot = _onDies.ToArray();
            foreach (var effect in snapshot)
            {
                if (((Observer)effect).Disabled) continue;
                effect.OnCallDead(pieceToDie);
            }
        }

        public void NotifyBeforeDestroyOrKill(IInternal action)
        {
            _onBeforeDestroyOrKill.ForEach(effect =>
            {
                if (((Observer)effect).Disabled) return;
                effect.OnCallBeforeDestroyOrKill(action);
            });
        }

        public void NotifyOnMoveGen(PieceLogic caller, List<Action.Action> actions)
        {
            _onMoveGens.ForEach(e =>
            {
                if (((Observer)e).Disabled) return;
                e.OnCallMoveGen(caller, actions);
            });
        }

        public void NotifyWhenApplyEffect(ApplyEffect action)
        {
            _onEffectApplies.ForEach(e =>
            {
                if (((Observer)e).Disabled) return;
                e.OnCallApplyEffect(action);
            });
        }

        public void NotifyBeforePieceAction(Action.Action action)
        {
            _beforePieceActions.ForEach(e =>
            {
                if (((Observer)e).Disabled) return;
                e.OnCallBeforePieceAction(action);
            });
        }

        public void NotifyBeforeRelicAction(IRelicAction relicAction)
        {
            _beforeRelicActions.ForEach(e =>
            {
                if (((Observer)e).Disabled) return;
                e.OnCallBeforeRelicAction(relicAction);
            });
        }

        public void NotifySpawnPiece(PieceLogic piece)
        {
            _onSpawns.ForEach(e =>
            {
                if (((Observer)e).Disabled) return;

                e.OnPieceSpawn(piece);
            });
        }
        private class HookBinding
        {
            public Action<Observer> Add;
            public IList List;
            public Action<Observer> Remove;
        }
    }
}