using System;
using System.Linq;
using Game.Board.Action;
using Game.Board.Action.Internal;
using Game.Board.General;

namespace Game.Board.Triggers
{
    [Il2CppSetOption(Option.NullChecks, false), Il2CppSetOption(Option.ArrayBoundsChecks, false)]
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

        public VelkarisMarker(GameState state, PieceLogic.PieceLogic p) : base(state, p, ObserverType.Moves, 1)
        {
            rows = new int[2];
            TriggerRows(p.Pos, p.Color);
        }
        
        public override void OnCall(Action.Action action)
        {
            if (action.Caller == Piece.Pos)
            {
                TriggerRows(action.To, Piece.Color);
                return;
            }
            
            if (GameState.SideToMove == Piece.Color) return;

            var rowMovedTo = action.To / GameState.MaxFile;
            
            if (!rows.Contains(rowMovedTo))
            {
                return;
            }
            ActionManager.Execute(new VelkarisMark(Piece.Pos, Piece.Pos, action.From));

            GameState.QueueTriggerDeleter(this);
        }
    }
}