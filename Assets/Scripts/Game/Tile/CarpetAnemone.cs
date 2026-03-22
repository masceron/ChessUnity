using Game.Action;
using Game.Action.Internal;
using Game.Effects.Debuffs;
using Game.Piece.PieceLogic.Commons;

namespace Game.Tile
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public class CarpetAnemone : Formation
    {

        public override FormationType GetFormationType()
        {
            return FormationType.CarpetAnemone;
        }

        protected override void OnPieceEnter(PieceLogic piece)
        {
            base.OnPieceEnter(piece);
            if (piece != null && piece.Color != Color)
            {
                ActionManager.EnqueueAction(new ApplyEffect(new Leashed(piece, piece.Pos, 3),
                    FormationType.CarpetAnemone));
                ActionManager.EnqueueAction(new ApplyEffect(new Poison(1, piece), FormationType.CarpetAnemone));
            }
        }

        public override int GetValueForAI()
        {
            return -40;
        }
    }
}