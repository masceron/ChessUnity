using Game.Action;
using Game.Action.Internal;
using Game.Piece.PieceLogic.Commons;
using Game.Triggers;

namespace Game.Tile
{
    public class HydroidThicket : Formation, IEndTurnTrigger
    {
        private bool _canPurifyEndTurn = true;

        public HydroidThicket(bool color) : base(color)
        {
            EndTurnEffectType = EndTurnEffectType.EndOfAllyTurn;
        }

        public new EndTurnTriggerPriority Priority => EndTurnTriggerPriority.FormationBuff;

        public EndTurnEffectType EndTurnEffectType { get; }

        public void OnCallEnd(Action.Action lastMainAction)
        {
            if (PieceOnFormation == null) return;
            if (_canPurifyEndTurn)
            {
                ActionManager.EnqueueAction(new Purify(Pos, PieceOnFormation.Pos));
                _canPurifyEndTurn = false;
            }
        }

        protected override void OnPieceEnter(PieceLogic piece)
        {
            _canPurifyEndTurn = true;
            PieceOnFormation = piece;
            ActionManager.EnqueueAction(new Purify(Pos, piece.Pos));
        }

        public override FormationType GetFormationType()
        {
            return FormationType.HydroidThicket;
        }

        public override int GetValueForAI()
        {
            return 50;
        }

        public void OnCall(Action.Action action)
        {
        }
    }
}