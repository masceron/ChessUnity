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
            var row = pos / MatchManager.MaxFile;
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

        public VelkarisMarker(PieceLogic.PieceLogic p) : base(p, ObserverType.Moves, ObserverPriority.Debuff)
        {
            rows = new int[2];
            TriggerRows(p.pos, p.color);
        }
        
        public override void OnCall(Action.Action action)
        {
            if (action.Caller == Piece.pos)
            {
                TriggerRows(action.To, Piece.color);
                return;
            }
            
            if (MatchManager.GameState.SideToMove == Piece.color) return;

            var rowMovedTo = action.To / MatchManager.MaxFile;
            
            if (!rows.Contains(rowMovedTo))
            {
                return;
            }
            ActionManager.TakeAction(new VelkarisMark(Piece.pos, Piece.pos, action.From));

            MatchManager.GameState.QueueTriggerDeleter(this);
        }
    }
}