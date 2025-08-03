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
        public ushort To;
        public ushort From;
        public ActionResult Result = ActionResult.Succeed;
        public readonly bool DoesMoveChangePos;

        protected Action(int from, bool pos)
        {
            From = (ushort)from;
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