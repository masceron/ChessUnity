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
        public static void Start()
        {
            sceneLoaded += (scene, _) =>
            {
                switch (scene.buildIndex)
                {
                    case 0: 
                        UIManager.Ins.Load(CanvasID.MainMenu); 
                        break;
                    case 1:
                        MatchManager.Ins.Init(new GameConfig(false, false, new Vector2Int(12, 12)));
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