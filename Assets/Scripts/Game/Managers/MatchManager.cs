using System;
using System.Collections.Generic;
using System.Linq;
using Game.Action;
using Game.Action.Internal;
using Game.Common;
using Game.Piece;
using Game.Relics;
using UnityEngine;
using UX.UI;
using UX.UI.Ingame;
using static Game.Common.BoardUtils;
using GameConfig = Game.Save.Stage.GameConfig;
using LineupConfig = Game.Save.Stage.LineupConfig;
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
            FormationManager.Ins.Intialize();
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
            
            StartGame(new LineupConfig(Config.PieceConfigWhite.ToArray(), Config.PieceConfigBlack.ToArray()), 
                Config.relicWhiteConfig, 
                Config.relicBlackConfig
                );
            
            //UIManager.Ins.Load(CanvasID.LineupEdit);
            //FindAnyObjectByType<LineupEditor>().Load(startingSize.x);
        }

        public void StartGame(LineupConfig cfg, RelicConfig whiteRelic, RelicConfig blackRelic)
        {
            MakePieces(cfg);
            UIManager.Ins.Load(CanvasID.Ingame);
            ActionManager.ExecuteWhenStart();
            InputProcessor.LoadRelic(whiteRelic, blackRelic);
        }

        public static bool Roll(int chance)
        {
            var a = Random.Range(1, 101);
            return a <= chance;
        } 
    }
}