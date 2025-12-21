using Game.Managers;
using Game.Piece.PieceLogic.Commons;
using Game.Action;
using Game.Action.Internal;

namespace Game.Tile
{
    public class HydroidThicket : Formation, ISubscriber
    {
        bool canPurifyEndTurn = true;
        public HydroidThicket(bool color) : base(color)
        {
            MatchManager.Ins.GameState.Subscribers.Add(this);
        }
        public override void OnPieceEnter(PieceLogic piece)
        {
            canPurifyEndTurn = true;
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
        public void OnCall(Action.Action action) { }
        public void OnCallEnd(bool endOfSide)
        {
            if (PieceOnFormation == null){ return; }
            if (endOfSide == PieceOnFormation.Color && canPurifyEndTurn)
            {
                ActionManager.EnqueueAction(new Purify(Pos, PieceOnFormation.Pos));
                canPurifyEndTurn = false;
            }
        }
    }
}