
using Game.Piece.PieceLogic;
using Game.Managers;
using Game.Action;
using Game.Effects.Debuffs;
using Game.Action.Internal;

namespace Game.Tile{
    public class AnchorIce : Formation{
        private int stack = 0;
        public AnchorIce(bool color) : base(color){
            MatchManager.Ins.GameState.OnIncreaseTurn += OnIncreaseTurn;
        }
        public override FormationType GetFormationType()
        {
            return FormationType.AnchorIce;
        }
        public override void OnPieceExit(PieceLogic piece)
        {
            base.OnPieceExit(piece);
            stack = 0;
        }
        void OnIncreaseTurn(int currentTurn){
            if (pieceOnFormation == null){
                return; 
            }
            if (stack == 0){
                ActionManager.ExecuteImmediately(new ApplyEffect(new Slow(3, 1, pieceOnFormation)));
            }
            else if (stack == 2){
                ActionManager.ExecuteImmediately(new ApplyEffect(new Stunned(1, pieceOnFormation)));
            }
            stack++;
        }
    }
}