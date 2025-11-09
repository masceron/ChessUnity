using System;
using System.Collections.Generic;

namespace Game.Action
{
    public enum ActionResult
    {
        Succeed, Failed, Unblockable
    }
    
    [Il2CppSetOption(Option.NullChecks, false), Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public abstract class Action
    {
        public int Target = -1;
        public int Maker;
        public ActionResult Result = ActionResult.Succeed;
        public readonly bool DoesMoveChangePos;

        protected Action(int maker, bool pos = false)
        {
            Maker = maker;
            DoesMoveChangePos = pos;
        }

        public void Execute()
        {
            Animate();
            if (Result != ActionResult.Failed)
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