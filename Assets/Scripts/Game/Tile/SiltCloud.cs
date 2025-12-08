using Game.Action;
using Game.Action.Internal;
using Game.Effects.Debuffs;
using Game.Piece.PieceLogic.Commons;

namespace Game.Tile
{
    public class SiltCloud : Formation
    {
        
        public SiltCloud(bool haveDuration, bool color) : base(color)
        {
            HaveDuration = haveDuration;
        }
        public override FormationType GetFormationType() => FormationType.SiltCloud;

        public override void OnFirstTurn(PieceLogic piece)
        {
            base.OnFirstTurn(piece);
        }

        public override void OnPieceEnter(PieceLogic piece)
        {
            base.OnPieceEnter(piece);
            
            ActionManager.EnqueueAction(new ApplyEffect(new Broken(-1, piece)));
        }

        public override void OnPieceExit(PieceLogic piece)
        {
            base.OnPieceExit(piece);
        }

        public override int GetValueForAI()
        {
            return -20;
        }
    }
    
}
