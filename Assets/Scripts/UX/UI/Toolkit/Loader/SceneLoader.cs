using System;
using System.Collections;
using Game.Common;
using Game.Managers;
using Game.Save.Stage;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;
using static UnityEngine.SceneManagement.SceneManager;

namespace UX.UI.Toolkit.Loader
{
    public class SceneLoader : Singleton<SceneLoader>
    {
        [NonSerialized] private UIDocument _loadingDoc;
        private VisualElement _loadingRoot;

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
        }

        public void ChangeScene(string sceneName)
        {
            StartCoroutine(Load(sceneName));
        }

        private IEnumerator Load(string sceneName)
        {
            _loadingRoot.style.display = DisplayStyle.Flex;

            yield return new WaitForSeconds(0.2f);
            var op = LoadSceneAsync(sceneName, LoadSceneMode.Single);
            op.allowSceneActivation = false;
            while (!op.isDone)
            {
                if (op.progress >= 0.9f) op.allowSceneActivation = true;
                yield return null;
            }

            _loadingRoot.style.display = DisplayStyle.None;
        }
    }
}