using Game.Piece.PieceLogic.Commons;
using Game.Tile;

namespace Game.Triggers
{
    public interface IFormationTrigger
    {
        void OnEnter(PieceLogic piece, Formation formation);
        void OnExit(PieceLogic piece, Formation formation);
    }
}