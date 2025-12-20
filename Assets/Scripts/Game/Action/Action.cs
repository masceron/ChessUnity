using System;
using System.Collections.Generic;

namespace Game.Action
{
    
    [Flags]
    public enum ActionFlag: byte
    {
        None = 0,
        Unblockable = 1,
        Undodgeable = 1 << 1
    }
    
    [Il2CppSetOption(Option.NullChecks, false), Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public abstract class Action
    {
        public int Target = -1;
        public int Maker;
        public bool Succeed = true;
        public ActionFlag Flag = ActionFlag.None;
        public readonly bool DoesMoveChangePos;

        protected Action(int maker, bool pos = false)
        {
            Maker = maker;
            DoesMoveChangePos = pos;
        }
        
        public void Execute()
        {
            Animate();
            if (Succeed)
            {
                ModifyGameState();
            }
        }

        protected virtual void Animate()
        {
            
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