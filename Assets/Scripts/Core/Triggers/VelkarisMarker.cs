using System;
using System.Linq;
using Board.Action;

namespace Core.Triggers
{
    public class VelkarisMarker: Trigger
    {
        private readonly int[] rows;

        private void TriggerRows(int pos, Color side)
        {
            var row = pos / GameState.MaxFile;
            switch (side)
            {
                case Color.White:
                    rows[0] = row - 1;
                    rows[1] = row - 2;
                    break;
                case Color.Black:
                    rows[0] = row + 1;
                    rows[1] = row + 2;
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(side), side, null);
            }
        }

        public VelkarisMarker(GameState state, PieceData p) : base(state, p)
        {
            rows = new int[2];
            TriggerRows(p.Pos, p.Color);
        }
        
        public override bool CallTrigger()
        {
            if (GameState.LastMovedPiece == null || !GameState.LastMove.DoesMoveChangePos()) return false;
            
            if (GameState.LastMovedPiece == Piece)
            {
                TriggerRows(GameState.LastMove.To, Piece.Color);
                return false;
            }
            
            if (GameState.SideToMove == Piece.Color) return false;

            var rowMovedTo = GameState.LastMove.To / GameState.MaxFile;
            
            if (!rows.Contains(rowMovedTo))
            {
                return false;
            }
            ActionManager.Execute(GameState, new VelkarisMark(Piece.Pos, Piece.Pos, GameState.LastMove.To));

            return true;
        }
    }
}