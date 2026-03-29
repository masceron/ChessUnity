using Game.Action;
using Game.Action.Internal;
using Game.Effects.Debuffs;
using Game.Piece.PieceLogic.Commons;

namespace Game.Tile
{
    /// <summary>
    ///     Placeholder for BubbleVentEffect implementation.
    /// </summary>
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public class BubbleVent : Formation
    {
        public BubbleVent(bool color) : base(color)
        {
        }

        protected override void OnPieceEnter(PieceLogic piece)
        {
            base.OnPieceEnter(piece);
            ActionManager.EnqueueAction(new ApplyEffect(new Bound(1, piece)));
        }

        public override FormationType GetFormationType()
        {
            return FormationType.BubbleVent;
        }

        public override int GetValueForAI()
        {
            return -10;
        }
    }
}