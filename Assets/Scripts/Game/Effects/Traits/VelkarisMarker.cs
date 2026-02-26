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
        private readonly int[] rows;

        public VelkarisMarker(PieceLogic p) : base(-1, 1, p, "effect_velkaris_marker")
        {
            rows = new int[2];
            TriggerRows(p.Pos, p.Color);
            EndTurnEffectType = EndTurnEffectType.EndOfAllyTurn;
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

            if (!rows.Contains(rowMovedTo) || ColorOfPiece(lastMainAction.Target) == Piece.Color) return;

            ActionManager.EnqueueAction(new VelkarisMark(Piece.Pos, Piece.Pos, lastMainAction.Target));
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
                    rows[0] = row - 1;
                    rows[1] = row - 2;
                    break;
                case true:
                    rows[0] = row + 1;
                    rows[1] = row + 2;
                    break;
            }
        }

        public override int GetValueForAI()
        {
            return base.GetValueForAI() + 80;
        }
    }
}