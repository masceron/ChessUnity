using Game.Piece.PieceLogic.Commons;
using Game.Action;
using Game.Action.Internal;
using Game.Effects;

namespace Game.Tile
{
    public class HydroidThicket : Formation, IEndTurnEffect
    {
        bool canPurifyEndTurn = true;
        public EndTurnEffectType EndTurnEffectType { get; }
        public HydroidThicket(bool color) : base(color)
        {
            EndTurnEffectType = EndTurnEffectType.EndOfAllyTurn;
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
        public void OnCallEnd(Action.Action lastMainAction)
        {
            if (PieceOnFormation == null){ return; }
            if (canPurifyEndTurn)
            {
                ActionManager.EnqueueAction(new Purify(Pos, PieceOnFormation.Pos));
                canPurifyEndTurn = false;
            }
        }
    }
}