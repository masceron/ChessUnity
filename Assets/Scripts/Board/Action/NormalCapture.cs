using Core;

namespace Board.Action
{
    public class NormalCapture: IAction
    {
        private readonly int _from;
        private readonly int _to;

        public NormalCapture(int f, int t)
        {
            _from = f;
            _to = t;
        }
        public void ApplyAction()
        {
            throw new System.NotImplementedException();
        }

        public Move MakeEncodedMove()
        {
            return new Move()
            { 
                from = (byte)_from,
                to = (byte)_to,
                flag = MoveFlag.NormalCapture
            };
        }
    }
}