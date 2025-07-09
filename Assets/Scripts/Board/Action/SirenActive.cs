using Board.Piece;
using Core;

namespace Board.Action
{
    public class SirenActive: Action
    {
        private readonly PieceManager pieceManager;
        public SirenActive(int caller, int f, int t, PieceManager p) : base(caller)
        {
            From = (ushort)f;
            To = (ushort)t;
            pieceManager = p;
        }
        public override void ApplyAction(GameState state)
        {
            pieceManager.Move(From, To);
            ModifyGameState(state);
        }

        public override void ModifyGameState(GameState state)
        {
            state.LastMovedPiece = state.MainBoard[From];
            state.MainBoard[To] = state.MainBoard[From];
            state.MainBoard[To].Pos = To;
            state.MainBoard[From] = null;
            state.MainBoard[To].Color = state.MainBoard[To].Color == Color.White ? Color.Black : Color.White;
            state.MainBoard[Caller].SkillCooldown = 12;
            state.LastMove = this;
        }

        public override bool DoesMoveChangePos()
        {
            return true;
        }
    }
}