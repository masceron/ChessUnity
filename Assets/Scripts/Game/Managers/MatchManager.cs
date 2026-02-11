using System;
using System.Collections.Generic;
using Game.Action;
using Game.Action.Internal;
using Game.Common;
using Game.Effects.RegionalEffect;
using Game.Piece;
using Game.Relics.Commons;
using UnityEngine;
using UX.UI;
using UX.UI.Ingame;
using ZLinq;
using static Game.Common.BoardUtils;
using GameConfig = Game.Save.Stage.GameConfig;
using LineupConfig = Game.Save.Stage.LineupConfig;
using Random = UnityEngine.Random;

namespace Game.Managers
{
    public enum GameMode
    {
        AIvsAI,
        PlayerVsAI,
        PlayerVsPlayer,
    }
    [Il2CppSetOption(Option.NullChecks, false), Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public class MatchManager: Singleton<MatchManager>
    {
        [NonSerialized]
        public GameState GameState;
        
        [NonSerialized]
        public BoardViewer InputProcessor;

        public Vector2Int StartingSize {get; private set;}

        private static void MakeBoard()
        {
            TileManager.Ins.Spawn();
            FormationManager.Ins.Initialize();
        }

        private void MakePieces(LineupConfig lineup)
        {
            var config = new List<PieceConfig>(lineup.WhiteConfig);
            config.AddRange(lineup.BlackConfig);
            
            foreach (var pieceConfig in config.Select(cfg => new PieceConfig(cfg.Type, cfg.Color, (ushort) PosMap(cfg.Index, StartingSize), cfg.AugmentationNames)))
            {
                ActionManager.ExecuteImmediately(new SpawnPiece(pieceConfig));
            }
        }

        private void MakeGame(GameConfig cfg)
        {
            GameState = new GameState(MaxLength, cfg.StartingSize, cfg.FirstSideToMove, cfg.OurSide);
            GameState.OnIncreaseTurn += ct => { Debug.Log("current turn : " + ct); };
            ActionManager.Init(GameState);
        }

        public void Init(GameConfig cfg, GameMode gameMode = GameMode.PlayerVsPlayer)
        {
            StartingSize = cfg.StartingSize;
            MakeGame(cfg);
            MakeBoard();

            StartGame(new LineupConfig(Config.PieceConfigWhite.ToArray(), Config.PieceConfigBlack.ToArray()),
                Config.relicWhiteConfig,
                Config.relicBlackConfig,
                Config.regionalEffectType
            );
            if (gameMode == GameMode.AIvsAI)
            {
                gameObject.AddComponent<AIvsAIController>();
            }
            //UIManager.Ins.Load(CanvasID.LineupEdit);
            //FindAnyObjectByType<LineupEditor>().Load(startingSize.x);
        }
        // public void InitForFreePlay(GameConfig cfg, GameMode gameMode)
        // {
        //     Init(cfg);
        //     if (gameMode == GameMode.AIvsAI)
        //     {
        //         this.gameObject.AddComponent<AIvsAIController>();
        //     }
        // }
        private void MakeRelics(RelicConfig white, RelicConfig black)
        {
            GameState.WhiteRelic = RelicMaker.Get(white);
            GameState.BlackRelic = RelicMaker.Get(black);
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
            GameState.OnStart();
            ActionManager.ExecuteWhenStart();
            // InputProcessor.LoadRelic(whiteRelic, blackRelic);
        }

        public static bool Roll(int chance)
        {
            var a = Random.Range(1, 101);
            return a <= chance;
        } 
        public void CallDraw(bool side)
        {
            
        }
    }
}