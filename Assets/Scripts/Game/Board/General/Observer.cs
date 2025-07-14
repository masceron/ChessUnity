using System;
using System.Collections.Generic;
using System.Diagnostics;

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

        protected Observer(ObserverType type, ObserverPriority priority)
        {
            Type = type;
            Priority = priority;
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