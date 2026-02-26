using Game.Action;
using Game.Action.Internal;
using Game.Effects.Debuffs;
using Game.Managers;
using Game.Piece.PieceLogic.Commons;

namespace Game.Tile
{
    public class AnchorIce : Formation
    {
        private int _stack;

        public AnchorIce(bool color) : base(color)
        {
            MatchManager.Ins.GameState.OnIncreaseTurn += OnIncreaseTurn;
            _stack = 0;
        }

        public override FormationType GetFormationType()
        {
            return FormationType.AnchorIce;
        }

        protected override void OnPieceExit(PieceLogic piece)
        {
            base.OnPieceExit(piece);
            _stack = 0;
        }

        private void OnIncreaseTurn(int currentTurn)
        {
            if (PieceOnFormation == null) return;
            if (_stack == 0)
                ActionManager.EnqueueAction(new ApplyEffect(new Slow(3, 1, PieceOnFormation), FormationType.AnchorIce));
            else if (_stack == 2)
                ActionManager.EnqueueAction(new ApplyEffect(new Stunned(1, PieceOnFormation), FormationType.AnchorIce));
            _stack++;
        }

        public override int GetValueForAI()
        {
            return -50;
        }
    }
}