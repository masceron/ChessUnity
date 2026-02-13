using System;
using System.Collections.Generic;
using Game.Common;
using UnityEngine;

namespace UX.UI
{
    public enum CanvasID
    {
        MainMenu, PlayMenu, Settings, Ingame, Loading, Followers, CreateArmy,
        DesignArmy, QuitToMainMenu, LineupEdit,
        FreePlayPreset, FreePlayDesignArmy, RegionalEffect, Augmentation,
        EndGameMessage,
        FreePlayMenu,
        StartGame, Assignment, MurkyTower, OutworldInvader, TrainingGround, Trader, Vault,
        StatueBattlePopup,
        None,
    }
    [Il2CppSetOption(Option.NullChecks, false), Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public class UIManager : Singleton<UIManager>
    {
        private RectTransform currentCanvas;
        private CanvasID currentCanvasID;
        private CanvasID previousCanvasID;
        
        [Serializable]
        public class CanvasDict : UDictionary<CanvasID, Canvas> {}

        [SerializeField] private CanvasDict canvasDict;
        private readonly Dictionary<CanvasID, GameObject> loadedCanvases = new();
        protected override void Awake()
        {
            base.Awake();
        }
        public void Load(CanvasID id)
        {
            previousCanvasID = currentCanvasID;
            currentCanvasID = id;
            if (currentCanvas)
            {
                currentCanvas.gameObject.SetActive(false);
                currentCanvas = null;
            }

            if (!loadedCanvases.TryGetValue(id, out var canvasToLoad))
            {
                canvasToLoad = Instantiate(canvasDict[id].gameObject, transform);
                canvasToLoad.gameObject.SetActive(true);
                canvasToLoad.name = canvasDict[id].name;
                loadedCanvases.Add(id, canvasToLoad);
            }
            else
            {
                canvasToLoad.gameObject.SetActive(true);
            }

            currentCanvas = canvasToLoad.GetComponent<RectTransform>();
            currentCanvas.name = canvasToLoad.name;
        }

        public void LoadPreviousCanvas()
        {
            Load(previousCanvasID);
        }
        public CanvasID GetCanvasID()
        {
            return currentCanvasID;
        }
        
        
    }
}
