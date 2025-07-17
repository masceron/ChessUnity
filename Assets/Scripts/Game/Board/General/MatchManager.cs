using System.Collections;
using System.Collections.Generic;
using Game.Board.Assets;
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
        public static AssetManager AssetManager;
        public static int MaxLength;

        private static void MakeBoard(TileManager tileManager, byte[] active)
        {
            TileManager = tileManager;
            var activeArray = new BitArray(active.Length);
            for (var i = 0; i < active.Length; i++)
            {
                activeArray[i] = active[i] != 0;
            }
            
            tileManager.Spawn(GameState.MaxLength, GameState.MaxLength, activeArray);
        }

        private static void MakePieces(PieceManager pieceManager, List<PieceConfig> config)
        {
            PieceManager = pieceManager;
            PieceManager.Init(MaxLength, config, AssetManager.PieceData);
        }

        private static void MakeGame(Config cfg)
        {
            GameState = new GameState(cfg.MaxLength, cfg.BoardActive, cfg.SideToMove, cfg.OurSide);
        }

        private static void LoadAssets(AssetManager asset)
        {
            AssetManager = asset;
            AssetManager.Init();
        }

        public static void Init(TileManager tile, PieceManager piece, AssetManager asset, Config cfg)
        {
            LoadAssets(asset);
            
            MaxLength = cfg.MaxLength;
            
            EventObserver.Init();
            
            MakeGame(cfg);
            MakeBoard(tile, cfg.BoardActive);
            MakePieces(piece, cfg.PieceConfig);
        }

        public static bool Roll(int chance)
        {
            var a = Random.Range(1, 101);
            Debug.Log(a);
            return a <= chance;
        } 
    }
}