using System;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Action
{

    [Flags]
    public enum ActionFlag : byte
    {
        None = 0,
        Unblockable = 1,
        Undodgeable = 1 << 1,

    }
    public enum ResultFlag
    {
        Success,
        Blocked,
        Dodged,
        Missed
    }
    
    [Il2CppSetOption(Option.NullChecks, false), Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public abstract class Action
    {
        public int Target = -1;
        public int Maker;
        public ResultFlag Result = ResultFlag.Success;
        public ActionFlag Flag = ActionFlag.None;

        protected Action(int maker)
        {
            Maker = maker;
        }
        
        public void Execute()
        {
            Debug.Log(this.GetType());
            Animate();
            ModifyGameState();
        }

        protected virtual void Animate()
        {
            
        }

        public bool IsValid()
        {
            return Result == ResultFlag.Success;
        }
        protected abstract void ModifyGameState();
    }

    [Il2CppSetOption(Option.NullChecks, false), Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public class ActionComparer: IEqualityComparer<Action>
    {
        public bool Equals(Action x, Action y)
        {
            if (x!.GetType() != y!.GetType()) return false;
            return x.Target == y.Target && x.Maker == y.Maker;
        }

        public int GetHashCode(Action obj)
        {
            return HashCode.Combine(obj.Target, obj.Maker);
        }
    }
}