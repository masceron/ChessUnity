using System;
using System.Collections;
using Game.Common;
using Game.Managers;
using Game.Save.Stage;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;
using UX.UI.Toolkit.Common;
using static UnityEngine.SceneManagement.SceneManager;

namespace UX.UI.Toolkit.Loader
{
    public class SceneLoader : Singleton<SceneLoader>
    {
        [NonSerialized] private UIDocument _loadingDoc;
        private VisualElement _loadingRoot;
        private RadialProgress _loadingProgress;

        protected override void Awake()
        {
            base.Awake();
            DontDestroyOnLoad(gameObject);
            _loadingDoc = GetComponent<UIDocument>();
        }

        private void Start()
        {
            sceneLoaded += (sceneCount, _) =>
            {
                switch (sceneCount.name)
                {
                    case "Game":
                        MatchManager.Ins.Init(
                            new GameConfig(false, false, new Vector2Int(Config.BoardSize, Config.BoardSize)),
                            new LineupConfig(
                                Config.PieceConfigWhite.ToArray(),
                                Config.PieceConfigBlack.ToArray(),
                                Config.relicWhiteConfig,
                                Config.relicBlackConfig,
                                Config.FieldEffectType
                            ));
                        break;
                }
            };
            _loadingRoot = _loadingDoc.rootVisualElement.Q<VisualElement>("Root");
            _loadingProgress = _loadingRoot.Q<RadialProgress>("RadialProgress");
        }

        public void ChangeScene(string sceneName)
        {
            StartCoroutine(Load(sceneName));
        }

        private IEnumerator Load(string sceneName)
        {
            _loadingRoot.style.display = DisplayStyle.Flex;
            _loadingProgress.Progress = 1f;

            yield return new WaitForSeconds(0.2f);
    
            var op = LoadSceneAsync(sceneName, LoadSceneMode.Single);
            op.allowSceneActivation = false;

            var visualProgress = 0f;
            
            while (visualProgress < 0.9f)
            {
                visualProgress = Mathf.MoveTowards(visualProgress, op.progress, Time.deltaTime * 2.5f);
                
                _loadingProgress.Progress = 1f - visualProgress;

                yield return null;
            }
            
            op.allowSceneActivation = true;
            
            while (visualProgress < 1f)
            {
                visualProgress = Mathf.MoveTowards(visualProgress, 1f, Time.deltaTime * 2f);
                _loadingProgress.Progress = 1f - visualProgress;
                yield return null;
            }
            
            _loadingProgress.Progress = 0f; 
            
            yield return new WaitForSeconds(0.1f);

            _loadingRoot.style.display = DisplayStyle.None;
        }
    }
}