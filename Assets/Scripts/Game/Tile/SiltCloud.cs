using Game.Action;
using Game.Action.Internal;
using Game.Effects.Debuffs;
using Game.Piece.PieceLogic.Commons;

namespace Game.Tile
{
    /// <summary>
    /// Gây hiệu ứng Broken lên quân đứng trên nó
    /// </summary>
    public class SiltCloud : Formation
    {
        public SiltCloud(bool color) : base(color)
        {

        }

        public override FormationType GetFormationType()
        {
            return FormationType.SiltCloud;
        }

        protected override void OnPieceEnter(PieceLogic piece)
        {
            base.OnPieceEnter(piece);

            ActionManager.EnqueueAction(new ApplyEffect(new Broken(-1, piece)));
        }

        protected override void OnPieceExit(PieceLogic piece)
        {
            base.OnPieceExit(piece);
        }

        public override int GetValueForAI()
        {
            return -20;
        }
    }
}