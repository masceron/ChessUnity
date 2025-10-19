using Game.Piece.PieceLogic;
using Game.Action;
using Game.Action.Internal;
using Game.Effects.Debuffs;

namespace Game.Tile
{
    /// <summary>
    ///     Placeholder for BubbleVentEffect implementation.
    /// </summary>
    [Il2CppSetOption(Option.NullChecks, false), Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public class BubbleVent : Formation
    {
        public BubbleVent(int d, bool hd, bool color) : base(color)
        {
            this.duration = d;
            this.haveDuration = hd;
        }

        public override void OnPieceEnter(PieceLogic piece)
        {
            base.OnPieceEnter(piece);
            ActionManager.EnqueueAction(new ApplyEffect(new Bound(1, piece)));
        }

        public override void OnPieceExit(PieceLogic piece)
        {
            base.OnPieceExit(piece);
        }

        override public void OnFirstTurn(PieceLogic piece)
        {
            base.OnFirstTurn(piece);
        }

        public override FormationType GetFormationType()
        {
            return FormationType.BubbleVent;
        }
    }


}

