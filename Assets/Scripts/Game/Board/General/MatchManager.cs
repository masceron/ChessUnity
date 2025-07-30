using System.Collections.Generic;
using Game.Board.Action;
using Game.Board.Action.Internal;
using Game.Board.Piece;
using Game.Board.Tile;
using Game.Interaction;
using Game.UX.UI;
using UnityEngine;
using static Game.Common.BoardUtils;

namespace Game.Board.General
{
    [Il2CppSetOption(Option.NullChecks, false), Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public static class MatchManager
    {
        public static GameState gameState;
        public static PieceManager pieceManager;
        internal static TileManager tileManager;
        public static AssetManager assetManager;

        private static void MakeBoard()
        {
            tileManager = Object.FindAnyObjectByType<TileManager>();
            
            BoardInteractionUtils.Init(Object.FindAnyObjectByType<BoardViewer>());
            
            tileManager.Spawn();
        }

        private static void MakePieces(List<PieceConfig> config, Vector2Int startingSize)
        {
            pieceManager = Object.FindAnyObjectByType<PieceManager>();
            pieceManager.Init(assetManager.PieceData);

            foreach (var cfg in config)
            {
                var pieceConfig = new PieceConfig(cfg.Type, cfg.Color, (ushort) PosMap(cfg.Index, startingSize));
                ActionManager.ExecuteImmediately(new SpawnPiece(pieceConfig));
            }
        }

        private static void MakeGame(Config cfg)
        {
            gameState = new GameState(MaxLength, cfg.StartingSize, cfg.SideToMove, cfg.OurSide);
            ActionManager.Init(gameState);
            Object.FindAnyObjectByType<BoardViewer>().Load();
        }

        private static void LoadAssets()
        {
            assetManager = Object.FindAnyObjectByType<AssetManager>();
            assetManager.Init();
        }

        public static void Init(Config cfg)
        {
            
            LoadAssets();
            EventObserver.Init();
            MakeGame(cfg);
            MakeBoard();
            MakePieces(cfg.PieceConfig, cfg.StartingSize);
            
            ActionManager.ExecuteWhenStart();
        }

        public static bool Roll(int chance)
        {
            var a = Random.Range(1, 101);
            return a <= chance;
        } 
    }
}