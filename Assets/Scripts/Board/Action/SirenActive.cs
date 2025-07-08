using Board.Piece;
using Core;

namespace Board.Action
{
    public class SirenActive: Action
    {
        private readonly PieceManager pieceManager;
        public SirenActive(int c, int f, int t, PieceManager p)
        {
            Caller = (ushort)c;
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
            state.MainBoard[Caller].SkillCooldown = 12;
            state.LastMove = this;
        }

        public override bool DoesMoveChangePos()
        {
            return true;
        }
    }
}