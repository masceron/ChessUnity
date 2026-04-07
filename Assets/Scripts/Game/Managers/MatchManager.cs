using System;
using System.Collections.Generic;
using Game.Action;
using Game.Action.Internal;
using Game.Common;
using Game.Piece;
using UnityEngine;
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
        PlayerVsPlayer
    }

    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public class MatchManager : Singleton<MatchManager>
    {
        [NonSerialized] public GameState GameState;

        [NonSerialized] private static uint _seed;
        [NonSerialized] private static Unity.Mathematics.Random _randomizer;

        [NonSerialized] public BoardViewer InputProcessor;

        public Vector2Int StartingSize { get; private set; }

        private void Start()
        {
            Init(new GameConfig(false, false, new Vector2Int(Config.BoardSize, Config.BoardSize)), new LineupConfig(
                Config.PieceConfigWhite.ToArray(),
                Config.PieceConfigBlack.ToArray(),
                Config.relicWhiteConfig,
                Config.relicBlackConfig,
                Config.FieldEffectType
            ));
        }

        private static void MakeBoard()
        {
            TileManager.Ins.Spawn();
            FormationManager.Ins.Initialize();
        }

        private void MakePieces(LineupConfig lineup)
        {
            var config = new List<PieceConfig>(lineup.WhiteConfig);
            config.AddRange(lineup.BlackConfig);

            foreach (var pieceConfig in config.Select(cfg =>
                         new PieceConfig(cfg.Type, cfg.Color, PosMap(cfg.Index, StartingSize), cfg.AugmentationNames)))
                ActionManager.ExecuteImmediately(new SpawnPiece(pieceConfig));
        }

        private void MakeGame(GameConfig cfg, LineupConfig lineupConfig)
        {
            GameState = new GameState(MaxLength, cfg.StartingSize, cfg.FirstSideToMove, cfg.OurSide,
                (lineupConfig.WhiteRelic, lineupConfig.BlackRelic), lineupConfig.FieldEffect);
            ActionManager.Init(GameState);
        }

        public void Init(GameConfig cfg, LineupConfig lineupConfig, GameMode gameMode = GameMode.PlayerVsPlayer)
        {
            _seed = (uint)Random.Range(int.MinValue, int.MaxValue);
            _randomizer = new Unity.Mathematics.Random(_seed);
            StartingSize = cfg.StartingSize;
            MakeGame(cfg, lineupConfig);
            MakeBoard();

            StartGame(
                lineupConfig
            );
            if (gameMode == GameMode.AIvsAI) gameObject.AddComponent<AIvsAIController>();
        }

        private void StartGame(LineupConfig cfg)
        {
            MakePieces(cfg);
            ActionManager.ExecuteWhenStart();
        }

        public static bool Roll(int chance)
        {
            var a = _randomizer.NextUInt(100);
            return a <= chance;
        }
    }
}