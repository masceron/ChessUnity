using System.Collections;
using Game.Configs;
using Game.Managers;
using UnityEngine;
using UnityEngine.SceneManagement;
using static UnityEngine.SceneManagement.SceneManager;

namespace UX.UI.Loader
{
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