using System.Linq;
using Game.Board.Action;
using Game.Board.Action.Internal;
using Game.Board.General;
using Game.Board.Piece.PieceLogic;
using static Game.Common.BoardUtils;

namespace Game.Board.Effects.Traits
{
    [Il2CppSetOption(Option.NullChecks, false), Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public class VelkarisMarker: Effect, IEndTurnEffect
    {
        private readonly int[] rows;
        
        public VelkarisMarker(PieceLogic p) : base(-1, 1, p, EffectName.VelkarisMarker)
        {
            rows = new int[2];
            TriggerRows(p.Pos, p.Color);
            EndTurnEffectType = EndTurnEffectType.EndOfAllyTurn;
        }

        private void TriggerRows(int pos, bool side)
        {
            var row = RankOf(pos);
            switch (side)
            {
                case false:
                    rows[0] = row - 1;
                    rows[1] = row - 2;
                    break;
                case true:
                    rows[0] = row + 1;
                    rows[1] = row + 2;
                    break;
            }
        }
        
        public void OnCallEnd(Action.Action action)
        {
            if (action == null) return;
            
            if (action.Caller == Piece.Pos)
            {
                TriggerRows(action.To, Piece.Color);
                return;
            }
            
            if (MatchManager.Ins.GameState.SideToMove == Piece.Color) return;

            var rowMovedTo = RankOf(action.To);
            
            if (!rows.Contains(rowMovedTo) || ColorOfPiece(action.To) == Piece.Color)
            {
                return;
            }
            
            ActionManager.EnqueueAction(new VelkarisMark(Piece.Pos, Piece.Pos, action.To));
            ActionManager.EnqueueAction(new RemoveEffect(this));
        }

        public EndTurnEffectType EndTurnEffectType { get; set; }
    }
}