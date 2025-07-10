using Board.Piece;
using Core;
using Core.General;
using Core.Piece;

namespace Board.Action
{
    public class SirenActive: Action
    {
        private readonly PieceManager pieceManager;
        public SirenActive(ushort caller, int f, int t, PieceManager p) : base(caller, true, false)
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
            state.Move(From, To);
            state.MainBoard[To].Color = state.MainBoard[To].Color == Color.White ? Color.Black : Color.White;
            ((GuidingSiren) state.MainBoard[Caller]).SkillCooldown = 12;
            state.LastMove = this;
        }
    }
}