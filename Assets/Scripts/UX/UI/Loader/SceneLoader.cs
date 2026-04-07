using System.Collections;
using System.Collections.Generic;
using Game.Managers;
using Game.Piece;
using UnityEngine;
using UnityEngine.SceneManagement;
using static UnityEngine.SceneManagement.SceneManager;
using GameConfig = Game.Save.Stage.GameConfig;

namespace UX.UI.Loader
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public static class SceneLoader
    {
        private static bool _isRegistered;
        private static GameMode _chosenGameMode = GameMode.PlayerVsAI;
        private static List<PieceConfig> _statuePieceConfigs;

        public static void Start()
        {
            if (_isRegistered) return;

            sceneLoaded += (scene, _) =>
            {
                _isRegistered = true;
                switch (scene.buildIndex)
                {
                    case 0:
                        UIManager.Ins.Load(CanvasID.MainMenu);
                        break;
                    case 1:
                        if (_statuePieceConfigs != null)
                        {
                            Config.SetBlackPieceConfig(_statuePieceConfigs);
                            _statuePieceConfigs = null;
                        }

                        MatchManager.Ins.Init(
                            new GameConfig(false, false, new Vector2Int(Config.BoardSize, Config.BoardSize)),
                            _chosenGameMode);
                        break;
                    case 2:
                        UIManager.Ins.Load(CanvasID.FreePlayMenu);
                        break;
                    case 3:
                        UIManager.Ins.Load(CanvasID.StartGame);
                        break;
                    case 4:
                        break;
                }
            };
        }

        public static void LoadSceneWithLoadingScreen(int id)
        {
            UIManager.Ins.Load(CanvasID.Loading);
            UIManager.Ins.StartCoroutine(Load(id));
        }

        public static void LoadFreePlay(GameMode gameMode)
        {
            LoadSceneWithLoadingScreen(1);
            _chosenGameMode = gameMode;
        }

        public static void LoadStatueBattle(List<PieceConfig> blackPieceConfigs)
        {
            _statuePieceConfigs = blackPieceConfigs;
            _chosenGameMode = GameMode.PlayerVsPlayer;
            LoadSceneWithLoadingScreen(1);
        }

        private static IEnumerator Load(int id)
        {
            yield return new WaitForSeconds(0.2f);
            var op = LoadSceneAsync(id, LoadSceneMode.Single);
            while (!op.isDone)
            {
                if (op.progress >= 0.9f) op.allowSceneActivation = true;
                yield return null;
            }
        }
    }
}