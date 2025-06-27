using Core;

namespace Board.Action
{
    public class SwitchSide: IAction
    {
        public void ApplyAction()
        {
            
        }

        public Move MakeEncodedMove()
        {
            return new Move
            {
                from = 0,
                to = 0,
                flag = MoveFlag.SwitchSide
            };
        }
    }
}