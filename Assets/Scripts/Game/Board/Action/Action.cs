using UnityEngine;

namespace Game.Board.Action
{
    public enum ActionResult
    {
        Succeed, Failed, Unblockable
    }
    
    [Il2CppSetOption(Option.NullChecks, false), Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public abstract class Action
    {
        public ushort From;
        public ushort To;
        public int Caller;
        public ActionResult Result = ActionResult.Succeed;
        public readonly bool DoesMoveChangePos;

        protected Action(int caller, bool pos)
        {
            Caller = caller;
            DoesMoveChangePos = pos;
        }

        public void Execute()
        {
            Debug.Log(GetType());
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
}