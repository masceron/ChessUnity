using Core;

namespace Board.Action
{
    public abstract class Action
    {
        public ushort From;
        public ushort To;
        public ushort Caller;
        
        public abstract void ApplyAction(GameState state);
        public abstract void ModifyGameState(GameState state);

        public abstract bool DoesMoveChangePos();
    }
}