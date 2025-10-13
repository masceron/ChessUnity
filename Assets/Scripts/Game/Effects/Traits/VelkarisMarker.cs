using System.Linq;
using Game.Action;
using Game.Action.Internal;
using Game.Managers;
using Game.Piece.PieceLogic;
using static Game.Common.BoardUtils;

namespace Game.Effects.Traits
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
        
        public void OnCallEnd(Action.Action lastMainAction)
        {
            if (lastMainAction.Maker == Piece.Pos)
            {
                TriggerRows(lastMainAction.Target, Piece.Color);
                return;
            }
            
            if (MatchManager.Ins.GameState.SideToMove == Piece.Color) return;

            var rowMovedTo = RankOf(lastMainAction.Target);
            
            if (!rows.Contains(rowMovedTo) || ColorOfPiece(lastMainAction.Target) == Piece.Color)
            {
                return;
            }
            
            ActionManager.EnqueueAction(new VelkarisMark(Piece.Pos, Piece.Pos, (ushort)lastMainAction.Target));
            ActionManager.EnqueueAction(new RemoveEffect(this));
        }

        public EndTurnEffectType EndTurnEffectType { get; set; }
    }
}