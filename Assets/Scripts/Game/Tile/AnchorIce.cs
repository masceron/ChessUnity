using Game.Action;
using Game.Action.Internal;
using Game.Effects.Debuffs;
using Game.Managers;
using Game.Piece.PieceLogic.Commons;

namespace Game.Tile
{
    /// <summary>
    /// Gây hiệu ứng Slow 1 3 turn lên quân địch dẫm phải, 
    /// nếu quân này tiếp tục đứng trên Anchor Ice trong 2 turn tiếp theo, nó sẽ bị Stun 1 turn.
    /// </summary>
    public class AnchorIce : Formation
    {
        //private int _stack;

        public AnchorIce(bool color) : base(color)
        {
           // _stack = 0;
        }

        public override FormationType GetFormationType()
        {
            return FormationType.AnchorIce;
        }

        //Làm lại
        // private void OnIncreaseTurn(int currentTurn)
        // {
        //     if (PieceOnFormation == null) return;
        //     if (PieceOnFormation.Color == Color) return;
        //     if (_stack == 0)
        //         ActionManager.EnqueueAction(new ApplyEffect(new Slow(3, 1, PieceOnFormation)));
        //     else if (_stack == 2)
        //         ActionManager.EnqueueAction(new ApplyEffect(new Stunned(1, PieceOnFormation)));
        //     _stack++;
        // }

        public override int GetValueForAI()
        {
            return -50;
        }
    }
}