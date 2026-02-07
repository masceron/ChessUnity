using Game.Piece.PieceLogic.Commons;

namespace Game.Effects
{
    public interface IOnPieceSpawned
    {
        public void OnPieceSpawn(PieceLogic piece);
    }
}