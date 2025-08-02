using System;
using System.Collections.Generic;
using System.Linq;
using Game.Board.Action;
using Game.Board.Action.Internal;
using Game.Board.Piece;
using Game.Board.Tile;
using Game.Common;
using Game.UX.UI;
using Game.UX.UI.UIManager;
using UnityEngine;
using static Game.Common.BoardUtils;
using Random = UnityEngine.Random;

namespace Game.Board.General
{
    [Il2CppSetOption(Option.NullChecks, false), Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public class MatchManager: Singleton<MatchManager>
    {
        [NonSerialized]
        public GameState GameState;
        
        [NonSerialized]
        public BoardViewer InputProcessor;

        private void MakeBoard()
        {
            TileManager.Ins.Spawn();
        }

        private void MakePieces(List<PieceConfig> config, Vector2Int startingSize)
        {
            foreach (var pieceConfig in config.Select(cfg => new PieceConfig(cfg.Type, cfg.Color, (ushort) PosMap(cfg.Index, startingSize))))
            {
                ActionManager.ExecuteImmediately(new SpawnPiece(pieceConfig));
            }
        }

        private void MakeGame(Config cfg)
        {
            GameState = new GameState(MaxLength, cfg.StartingSize, cfg.SideToMove, cfg.OurSide);
            ActionManager.Init(GameState);
        }

        public void Init(Config cfg)
        {
            MakeGame(cfg);
            MakeBoard();
            MakePieces(cfg.PieceConfig, cfg.StartingSize);
            
            UIManager.Ins.Load(CanvasID.Ingame);
            
            ActionManager.ExecuteWhenStart();
        }

        public static bool Roll(int chance)
        {
            var a = Random.Range(1, 101);
            return a <= chance;
        } 
    }
}