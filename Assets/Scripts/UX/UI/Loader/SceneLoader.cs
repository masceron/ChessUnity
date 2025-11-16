using System.Collections;
using Game.Managers;
using UnityEngine;
using UnityEngine.SceneManagement;
using static UnityEngine.SceneManagement.SceneManager;
using GameConfig = Game.Save.Stage.GameConfig;

namespace UX.UI.Loader
{

    [Il2CppSetOption(Option.NullChecks, false), Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public static class SceneLoader
    {
        private static bool isRegistered = false;
        public static void Start()
        {
            if (isRegistered) { return; }
            
            sceneLoaded += (scene, _) =>
            {
                isRegistered = true;
                switch (scene.buildIndex)
                {
                    case 0:
                        UIManager.Ins.Load(CanvasID.MainMenu);
                        break;
                    case 1:
                        MatchManager.Ins.Init(new GameConfig(false, false, new Vector2Int(Config.boardSize, Config.boardSize)));
                        break;
                    case 2:
                        UIManager.Ins.Load(CanvasID.FreePlayMenu);
                        break;
                }
            };
        }
        
        public static void LoadSceneWithLoadingScreen(int id)
        {
            UIManager.Ins.Load(CanvasID.Loading);
            UIManager.Ins.StartCoroutine(Load(id));
        }
        
        private static IEnumerator Load(int id)
        {
            var op = LoadSceneAsync(id, LoadSceneMode.Single);
            while (!op.isDone)
            {
                if (op.progress >= 0.9f)
                {
                    op.allowSceneActivation = true;
                }
                yield return null;
            }
        }
    }
}