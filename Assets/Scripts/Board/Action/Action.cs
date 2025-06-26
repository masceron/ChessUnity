using Core;

namespace Board.Action
{
    public interface IAction
    {
        bool IsLegal();
        void ApplyAction();
        Move MakeEncodedMove();
    }
}