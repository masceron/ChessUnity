using Core;

namespace Board.Action
{
    public interface IAction
    {
        void ApplyAction();
        Move MakeEncodedMove();
    }
}