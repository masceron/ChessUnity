using Game.Action;
using Game.Action.Internal;
using Game.Effects.Debuffs;
using Game.Piece.PieceLogic.Commons;
using ZLinq;

namespace Game.Tile
{
    /// <summary>
    ///     Gây hiệu ứng Infected lên quân cờ
    /// </summary>
    public class Saprolegnia : Formation
    {
        public Saprolegnia(bool color) : base(color)
        {

        }

        public override FormationType GetFormationType()
        {
            return FormationType.Saprolegnia;
        }

        protected override void OnPieceEnter(PieceLogic piece)
        {
            base.OnPieceEnter(piece);

            if (piece.Effects.OfType<Infected>().ToList().Count != 0) return;
            ActionManager.EnqueueAction(new ApplyEffect(new Infected(piece)));
        }

        protected override void OnPieceExit(PieceLogic piece)
        {
            base.OnPieceExit(piece);
        }

        public override int GetValueForAI()
        {
            return -100;
        }
    }
}