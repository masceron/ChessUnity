using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Game.Action.Internal;
using Game.Action.Relics;
using Game.Effects;
using Game.Piece.PieceLogic.Commons;
using ZLinq;

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
        
        private class HookBinding
        {
            public IList List;
            public Action<Effect> Add;
            public Action<Effect> Remove;
        }
        
        private readonly Dictionary<Type, HookBinding> bindings = new();

        public EffectHooks()
        {
            Register(onApplies);
            Register(onRemoves);
            Register(onMoveGens);
            Register(afterPieceActions);
            Register(beforePieceActions);
            Register(beforeRelicActions);
            Register(afterRelicActions);
            Register(onDies);
            Register(onStartTurns);
            Register(onEndTurns);
            Register(onEffectApplies);
            Register(onMoveRanges);
        }
        
        private void Register<T>(List<T> list)
        {
            bindings[typeof(T)] = new HookBinding
            {
                List = list,
                Add = e => AddToList(list, (T)(object)e),
                Remove = e => RemoveFromList(list, (T)(object)e)
            };
        }
        

        public void AddObserver(Effect effect)
        {
            foreach (var kvp in bindings.Where(kvp => kvp.Key.IsInstanceOfType(effect)))
            {
                kvp.Value.Add(effect);
            }
        }

        public void RemoveObserver(Effect effect)
        {
            foreach (var kvp in bindings.Where(kvp => kvp.Key.IsInstanceOfType(effect)))
            {
                kvp.Value.Remove(effect);
            }
        }

        public List<T> GetList<T>()
        {
            if (bindings.TryGetValue(typeof(T), out var binding))
            {
                return (List<T>)binding.List;
            }
            return null;
        }

        private static void AddToList<T>(List<T> list, T effect)
        {
            var pos = list.BinarySearch(effect, Effect.GetComparer<T>());
            list.Insert(pos >= 0 ? pos : ~pos, effect);
        }

        private static void RemoveFromList<T>(List<T> list, T effect)
        {
            list.Remove(effect);
        }
        
        public void NotifyDead(PieceLogic pieceToDie)
        {
            onDies.ForEach (effect =>
            {
                if (((Effect)effect).disabled) return;
                effect.OnCallDead(pieceToDie);
            });
        }
        
        public void NotifyOnMoveGen(PieceLogic caller, List<Action.Action> actions)
        {
            onMoveGens.ForEach(e =>
            {
                if (((Effect)e).disabled) return;
                e.OnCallMoveGen(caller, actions);
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

        public void NotifyBeforeRelicAction(IRelicAction relicAction)
        {
            beforeRelicActions.ForEach(e =>
            {
                if (((Effect)e).disabled) return;
                e.OnCallBeforeRelicAction(relicAction);
            });
        }
    }
}