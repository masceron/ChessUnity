using Core;

namespace Board.Action
{
    public abstract class Action
    {
        public Move Move;
        public int From;
        public int To;
        public abstract void ApplyAction();
    }
}