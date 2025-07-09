using Core;
using UnityEditor.Rendering;

namespace Board.Action
{
    public abstract class Action
    {
        public ushort From;
        public ushort To;
        public int Caller;

        protected Action(int caller)
        {
            Caller = caller;
        }
        
        public abstract void ApplyAction(GameState state);
        public abstract void ModifyGameState(GameState state);

        public abstract bool DoesMoveChangePos();
    }
}