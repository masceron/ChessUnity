using Game.Piece.PieceLogic;
using Game.Effects.Debuffs;

namespace Game.Tile
{
    /// <summary>
    /// Dazzling Light Tile
    /// Gây hiệu ứng Blinded lên quân cờ trong 1 turn
    /// </summary>

    [Il2CppSetOption(Option.NullChecks, false), Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public class DazzlingLight : Formation
    {
        public DazzlingLight(bool haveDuration, bool color) : base(color)
        {
            HaveDuration = haveDuration;
        }

        public override FormationType GetFormationType()
        {
            return FormationType.DazzlingLight;
        }

        public override void OnPieceEnter(PieceLogic piece)
        {
            base.OnPieceEnter(piece);
            ApplyEffect(piece,new Blinded(1, 100, piece));
        }

        public override void OnPieceExit(PieceLogic piece)
        {
            base.OnPieceExit(piece);
        }

        public override void OnFirstTurn(PieceLogic piece)
        {
            base.OnFirstTurn(piece);
        }
    }
}

