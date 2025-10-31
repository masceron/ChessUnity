using System;
using System.Collections.Generic;
using System.Linq;
using Game.Action;
using Game.Action.Internal;
using Game.Common;
using Game.Effects.RegionalEffect;
using Game.Piece;
using Game.Relics;
using Game.Tile;
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

        public Vector2Int startingSize{get; private set;}

        private void MakeBoard(GameConfig cfg)
        {
            TileManager.Ins.Spawn();
            FormationManager.Ins.Intialize();
            // For testing purpose
            FormationManager.Ins.SetFormation(PosMap(15, startingSize), new PredatorLair(cfg.FirstSideToMove));
            GameState.regionalEffect = new Whirlpool();
        }

        private void MakePieces(LineupConfig lineup)
        {
            var config = new List<PieceConfig>(lineup.WhiteConfig);
            config.AddRange(lineup.BlackConfig);
            
            foreach (var pieceConfig in config.Select(cfg => new PieceConfig(cfg.Type, cfg.Color, (ushort) PosMap(cfg.Index, startingSize), cfg.Augmentations)))
            {
                ActionManager.ExecuteImmediately(new SpawnPiece(pieceConfig));
            }
        }

        private void MakeGame(GameConfig cfg)
        {
            GameState = new GameState(MaxLength, cfg.StartingSize, cfg.FirstSideToMove, cfg.OurSide);
            GameState.OnIncreaseTurn += (ct) => { Debug.Log("current turn : " + ct); };
            ActionManager.Init(GameState);
        }

        public void Init(GameConfig cfg)
        {
            startingSize = cfg.StartingSize;
            AssetManager.Ins.Load();
            MakeGame(cfg);
            MakeBoard(cfg);
            
            StartGame(new LineupConfig(Config.PieceConfigWhite.ToArray(), Config.PieceConfigBlack.ToArray()), 
                Config.relicWhiteConfig, 
                Config.relicBlackConfig,
                Config.regionalEffectType
                );
            
            //UIManager.Ins.Load(CanvasID.LineupEdit);
            //FindAnyObjectByType<LineupEditor>().Load(startingSize.x);
        }

        private void MakeRelics(RelicConfig white, RelicConfig black)
        {
            GameState.WhiteRelic = GameState.GetRelicLogicByConfig(white);
            GameState.BlackRelic = GameState.GetRelicLogicByConfig(black);
        }

        private void MakeRegionalEffect(RegionalEffectType ret)
        {
            GameState.MakeRegionalEffect(ret);
        }

        private void StartGame(LineupConfig cfg, RelicConfig whiteRelic, RelicConfig blackRelic, RegionalEffectType ret)
        {
            MakeRegionalEffect(ret);
            MakePieces(cfg);
            MakeRelics(whiteRelic, blackRelic);
            UIManager.Ins.Load(CanvasID.Ingame);
            ActionManager.ExecuteWhenStart();
            // InputProcessor.LoadRelic(whiteRelic, blackRelic);
        }

        public static bool Roll(int chance)
        {
            var a = Random.Range(1, 101);
            return a <= chance;
        } 
    }
}