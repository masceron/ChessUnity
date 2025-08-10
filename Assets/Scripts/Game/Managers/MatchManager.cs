using System;
using System.Collections.Generic;
using System.Linq;
using Game.Action;
using Game.Action.Internal;
using Game.Common;
using Game.Data.Pieces;
using Game.Save;
using UnityEngine;
using UX.UI;
using UX.UI.Ingame;
using static Game.Common.BoardUtils;
using Random = UnityEngine.Random;

namespace Game.Managers
{
    [Il2CppSetOption(Option.NullChecks, false), Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public class MatchManager: Singleton<MatchManager>
    {
        [NonSerialized]
        public GameState GameState;
        
        [NonSerialized]
        public BoardViewer InputProcessor;

        private Vector2Int startingSize;

        private static void MakeBoard()
        {
            TileManager.Ins.Spawn();
        }

        private void MakePieces(LineupConfig lineup)
        {
            var config = new List<PieceConfig>(lineup.WhiteConfig);
            config.AddRange(lineup.BlackConfig);
            
            foreach (var pieceConfig in config.Select(cfg => new PieceConfig(cfg.Type, cfg.Color, (ushort) PosMap(cfg.Index, startingSize))))
            {
                ActionManager.ExecuteImmediately(new SpawnPiece(pieceConfig));
            }
        }

        private void MakeGame(GameConfig cfg)
        {
            GameState = new GameState(MaxLength, cfg.StartingSize, cfg.FirstSideToMove, cfg.OurSide);
            ActionManager.Init(GameState);
        }

        public void Init(GameConfig cfg)
        {
            startingSize = cfg.StartingSize;
            AssetManager.Ins.Load();
            MakeGame(cfg);
            MakeBoard();
            
            StartGame(new LineupConfig(new List<PieceConfig>(Config.PieceConfig), new List<PieceConfig>()));
        }

        public void StartGame(LineupConfig cfg)
        {
            MakePieces(cfg);
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