using Core.General;

namespace Board.Action
{
    public abstract class Action
    {
        public ushort From;
        public ushort To;
        public int Caller;
        public bool Success;
        public readonly bool DoesMoveChangePos;
        public readonly bool DoesMoveCapture;

        protected Action(int caller, bool pos, bool cap)
        {
            Caller = caller;
            DoesMoveCapture = cap;
            DoesMoveChangePos = pos;
            Success = true;
        }
        
        public abstract void ApplyAction(GameState state);
        public abstract void ModifyGameState(GameState state);
    }
}