using Game.Action;
using Game.Action.Internal;
using Game.Managers;
using Game.Piece.PieceLogic.Commons;
using Game.Triggers;
using static Game.Common.BoardUtils;

namespace Game.Effects.Traits
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public class VelkarisMarker : Effect, IEndTurnTrigger
    {
        private readonly int[] _rows;

        public VelkarisMarker(PieceLogic p) : base(-1, 1, p, "effect_velkaris_marker")
        {
            _rows = new int[2];
            TriggerRows(p.Pos, p.Color);
            EndTurnEffectType = EndTurnEffectType.EndOfAllyTurn;
        }

        public void OnCallEnd(Action.Action lastMainAction)
        {
            if (lastMainAction.GetMakerAsPiece() == Piece)
            {
                TriggerRows(lastMainAction.GetTargetPos(), Piece.Color);
                return;
            }

            if (MatchManager.Ins.GameState.SideToMove == Piece.Color) return;

            var rowMovedTo = RankOf(lastMainAction.GetTargetPos());

            if (!_rows.Contains(rowMovedTo) || lastMainAction.GetMakerAsPiece().Color == Piece.Color) return;

            ActionManager.EnqueueAction(new VelkarisMark(Piece, lastMainAction.GetMakerAsPiece()));
            ActionManager.EnqueueAction(new RemoveEffect(this));
        }

        public EndTurnTriggerPriority Priority => EndTurnTriggerPriority.Debuff;

        public EndTurnEffectType EndTurnEffectType { get; set; }

        private void TriggerRows(int pos, bool side)
        {
            var row = RankOf(pos);
            switch (side)
            {
                case false:
                    _rows[0] = row - 1;
                    _rows[1] = row - 2;
                    break;
                case true:
                    _rows[0] = row + 1;
                    _rows[1] = row + 2;
                    break;
            }
        }

        public override int GetValueForAI()
        {
            return base.GetValueForAI() + 80;
        }
    }
}