using System;
using System.Linq;
using Board.Action;
using UnityEngine;

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

        public VelkarisMarker(PieceData p, GameState state) : base(state, p)
        {
            rows = new int[2];
            TriggerRows(p.Pos, p.Color);
        }
        
        public override bool CallTrigger(PieceData movedPiece, Move lastMove)
        {
            if (movedPiece == null) return false;
            
            if (movedPiece == Piece)
            {
                TriggerRows(lastMove.to, Piece.Color);
                return false;
            }
            
            if (!lastMove.DoesMoveChangePos() || GameState.Position.side_to_move == Piece.Color) return false;

            var rowMovedTo = lastMove.to / GameState.MaxFile;
            
            if (!rows.Contains(rowMovedTo))
            {
                return false;
            }
            ActionManager.Execute(GameState, new VelkarisMark(Piece.Pos, lastMove.to));

            return true;
        }
    }
}