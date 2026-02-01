using Game.Action;
using Game.Action.Internal;
using Game.Piece.PieceLogic.Commons;
using Game.Effects.Debuffs;

namespace Game.Tile
{
    
    [Il2CppSetOption(Option.NullChecks, false), Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public class CarpetAnemone : Formation
    {
        public CarpetAnemone(bool haveDuration, bool color) : base(color)
        {
            HaveDuration = haveDuration;
        }

        public override FormationType GetFormationType()
        {
            return FormationType.CarpetAnemone;
        }

        public override void OnPieceEnter(PieceLogic piece)
        {
            base.OnPieceEnter(piece);
            if (piece != null && piece.Color != Color)
            {
                ActionManager.EnqueueAction(new ApplyEffect(new Leashed(piece, piece.Pos, 3), FormationType.CarpetAnemone));
                ActionManager.EnqueueAction(new ApplyEffect(new Poison(1, piece), FormationType.CarpetAnemone));
            }
        }

        public override void OnPieceExit(PieceLogic piece)
        {
            base.OnPieceExit(piece);
        }

        public override int GetValueForAI()
        {
            return -40;
        }
    }
}

