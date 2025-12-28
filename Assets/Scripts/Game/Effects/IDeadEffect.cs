using Game.Piece.PieceLogic.Commons;

namespace Game.Effects
{
    public interface IDeadEffect
    {
        public void OnCallDead(PieceLogic pieceToDie);
    }
}