using Board.Piece;
using Board.Tile;
using UnityEngine;

namespace Core.General
{
    public class MatchManager
    {
        public readonly GameState GameState;
        public readonly PieceManager PieceManager;
        public readonly TileManager TileManager;

        public MatchManager(TileManager tile, PieceManager piece, Config cfg)
        {
            PieceManager = piece;
            TileManager = tile;
            GameState = new GameState(cfg.MaxRank, cfg.MaxFile, cfg.PieceConfig, cfg.BoardActive, cfg.SideToMove, cfg.OurSide);
        }

        public bool Roll(int chance)
        {
            return Random.Range(1, 100) <= chance;
        } 
    }
}