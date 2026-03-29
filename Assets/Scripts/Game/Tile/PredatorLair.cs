using Game.Action;
using Game.Action.Internal;
using Game.Effects.Traits;
using Game.Piece.PieceLogic.Commons;

namespace Game.Tile
{
    public class PredatorLair : Formation
    {
        private LongReach appliedEffect;

        public PredatorLair(bool color) : base(color)
        {
        }

        public override FormationType GetFormationType()
        {
            return FormationType.PredatorLair;
        }

        protected override void OnPieceEnter(PieceLogic piece)
        {
            base.OnPieceEnter(piece);
            appliedEffect = new LongReach(PieceOnFormation, -1, 2);
            ActionManager.EnqueueAction(new ApplyEffect(appliedEffect));
        }

        protected override void OnPieceExit(PieceLogic piece)
        {
            base.OnPieceExit(piece);
            ActionManager.EnqueueAction(new RemoveEffect(appliedEffect));
        }

        public override int GetValueForAI()
        {
            return 40;
        }
    }
}