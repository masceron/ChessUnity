using Game.Action;
using Game.Action.Internal;
using Game.Effects.Traits;
using Game.Piece.PieceLogic.Commons;

namespace Game.Tile
{
    public class PredatorLair : Formation{
        private LongReach appliedEffect;
        public PredatorLair(bool color) : base(color){
            
        }
        public override FormationType GetFormationType()
        {
            return FormationType.PredatorLair;
        }

        public override void OnPieceEnter(PieceLogic piece){
            base.OnPieceEnter(piece);
            appliedEffect = new LongReach(PieceOnFormation);
            ActionManager.ExecuteImmediately(new ApplyEffect(appliedEffect));
        }

        public override void OnPieceExit(PieceLogic piece){
            base.OnPieceExit(piece);
            ActionManager.ExecuteImmediately(new RemoveEffect(appliedEffect));
        }

        public override int GetValueForAI()
        {
            return 40;
        }
    }
}