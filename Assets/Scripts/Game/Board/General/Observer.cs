using System.Collections.Generic;
using Game.Board.Effects;

namespace Game.Board.General
{
    public enum ObserverPriority: byte
    {
        Other, Buff, Debuff, Kill        
    }
    
    [Il2CppSetOption(Option.NullChecks, false), Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public abstract class Observer: Comparer<Observer>
    {
        public readonly ObserverType Type;
        public readonly ObserverPriority Priority;

        protected Observer(EffectType type)
        {
            var info = MatchManager.AssetManager.EffectData[type];
            Type = info.activeWhen;
            Priority = info.effectCategory;
        }

        public virtual void OnCall(Action.Action action)
        {
            
        }

        public override int Compare(Observer x, Observer y)
        {
            return -x!.Priority.CompareTo(y!.Priority);
        }
    }
}