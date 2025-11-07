using Game.Piece.PieceLogic;
using Game.Tile;
using Game.Effects;

namespace Game.Effects
{
    public interface IImmunity
    {
        bool CheckImmunity(FormationType formationType, Effect effect);
    }
}