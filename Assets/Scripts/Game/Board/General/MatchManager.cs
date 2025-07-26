using System.Collections;
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

        private static void MakeBoard(TileManager _tileManager, byte[] active)
        {
            tileManager = _tileManager;
            var activeArray = new BitArray(active.Length);
            for (var i = 0; i < active.Length; i++)
            {
                activeArray[i] = active[i] != 0;
            }
            BoardInteractionUtils.Init(GameObject.Find("BoardViewer").GetComponent<BoardViewer>());
            
            tileManager.Spawn(activeArray);
        }

        private static void MakePieces(PieceManager _pieceManager, List<PieceConfig> config)
        {
            pieceManager = _pieceManager;
            pieceManager.Init(assetManager.PieceData);

            foreach (var cfg in config)
            {
                ActionManager.ExecuteImmediately(new SpawnPiece(cfg));
            }
        }

        private static void MakeGame(Config cfg)
        {
            gameState = new GameState(MaxLength, cfg.BoardActive, cfg.SideToMove, cfg.OurSide);
            ActionManager.Init(gameState);
        }

        private static void LoadAssets(AssetManager asset)
        {
            assetManager = asset;
            assetManager.Init();
        }

        public static void Init(TileManager tile, PieceManager piece, AssetManager asset, Config cfg)
        {
            LoadAssets(asset);
            
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