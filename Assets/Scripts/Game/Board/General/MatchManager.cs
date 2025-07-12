using Game.Board.Piece;
using Game.Board.Tile;
using UnityEngine;

namespace Game.Board.General
{
    [Il2CppSetOption(Option.NullChecks, false), Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public static class MatchManager
    {
        public static GameState GameState;
        public static PieceManager PieceManager;
        public static TileManager TileManager;

        public static void Init(TileManager tile, PieceManager piece, Config cfg)
        {
            EventObserver.Init();
            PieceManager = piece;
            TileManager = tile;
            GameState = new GameState(cfg.MaxRank, cfg.MaxFile, cfg.PieceConfig, cfg.BoardActive, cfg.SideToMove, cfg.OurSide);
        }

        public static bool Roll(int chance)
        {
            var a = Random.Range(1, 101);
            Debug.Log(a);
            return a <= chance;
        } 
    }
}