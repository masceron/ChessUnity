using Game.Board.General;

namespace Game.Board.Action
{
    public enum ActionResult
    {
        UNKNOWN, SUCCEED, FAILED
    }
    
    [Il2CppSetOption(Option.NullChecks, false), Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public abstract class Action
    {
        public ushort From;
        public ushort To;
        public int Caller;
        public ActionResult Success;
        public readonly bool DoesMoveChangePos;

        protected Action(int caller, bool pos)
        {
            Caller = caller;
            DoesMoveChangePos = pos;
            Success = ActionResult.UNKNOWN;
        }
        
        public abstract void ApplyAction(GameState state);
        public abstract void ModifyGameState(GameState state);
    }
}