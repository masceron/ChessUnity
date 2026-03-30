using Game.Action;
using Game.Action.Internal;
using Game.Piece.PieceLogic.Commons;
using Game.Triggers;

namespace Game.Tile
{
    /// <summary>
    /// "Purify khi quân đứng trên ô này 2 turn.
    /// (turn nó đi vào thì purify 1 lần, xong turn sau nó purify thêm lần nữa, nhưng đến turn thứ 3 trở đi thì nó ko purify nữa)
    /// </summary>
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
                ActionManager.EnqueueAction(new Purify(this, PieceOnFormation));
                _canPurifyEndTurn = false;
            }
        }

        protected override void OnPieceEnter(PieceLogic piece)
        {
            _canPurifyEndTurn = true;
            PieceOnFormation = piece;
            ActionManager.EnqueueAction(new Purify(this, piece));
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