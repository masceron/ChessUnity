using Game.Action;
using Game.Action.Internal;
using Game.Effects.Debuffs;
using Game.Piece.PieceLogic.Commons;
using ZLinq;

namespace Game.Tile
{
    /// <summary>
    /// Urchin Field Tile
    /// </summary>
    ///       
    public class Saprolegnia : Formation
    {
        public Saprolegnia(bool haveDuration, bool color) : base(color)
        {
            HaveDuration = haveDuration;
        }

        public override FormationType GetFormationType()
        {
            return FormationType.Saprolegnia;
        }

        public override void OnFirstTurn(PieceLogic piece)
        {
            base.OnFirstTurn(piece);
        }

        public override void OnPieceEnter(PieceLogic piece)
        {
            base.OnPieceEnter(piece);

            if (piece.Effects.OfType<Infected>().ToList().Count != 0) return;
            ActionManager.EnqueueAction(new ApplyEffect(new Infected(piece), FormationType.Saprolegnia));
        }

        public override void OnPieceExit(PieceLogic piece)
        {
            base.OnPieceExit(piece);
        }

        public override int GetValueForAI()
        {
            return -100;
        }
    }

}
