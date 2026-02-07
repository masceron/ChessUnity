using System;
using System.Collections.Generic;

namespace Game.Common
{
    public enum ObserverPriority: byte
    {
        //Effect does not have a trigger
        None,
        
        // Priorities of effect trigger when an action is taken.
        AfterAction, DefenderAction, AttackerAction, Kill,
        
        //Priorities of effect trigger when ending a ply.
        //Effects on the start of turn have to run after effects at the end of turn.
        
        StartTurnBuff, StartTurnDebuff, StartTurnKill, StartTurnMove,
        
        RegionalEffect, RealmInfluence,
        
        EndturnBuff, EndturnDebuff, EndturnKill, EndturnMove,
    }
    public abstract class Observer
    {
        public bool disabled = false;
        protected ObserverPriority priority;
        public static IComparer<TItem> GetComparer<TItem>()
        {
            return Comparer<TItem>.Create((x, y) =>
            {
                if (x is not Observer effectX || y is not Observer effectY) return 0;

                return effectX.priority.CompareTo(effectY.priority);
            });
        }
    }
}