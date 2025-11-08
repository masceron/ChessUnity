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
        None,
    }
    [Il2CppSetOption(Option.NullChecks, false), Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public class UIManager : Singleton<UIManager>
    {
        private RectTransform currentCanvas;
        public CanvasID initialCanvas = CanvasID.None;
        public CanvasID currentCanvasID;
        
        [Serializable]
        public class CanvasDict : UDictionary<CanvasID, Canvas> {}

        [SerializeField] private CanvasDict canvasDict;
        private readonly Dictionary<CanvasID, GameObject> loadedCanvases = new();
        protected override void Awake()
        {
            base.Awake();
            if (initialCanvas != CanvasID.None)
            {
                Load(initialCanvas);
            }
        }
        public void Load(CanvasID id)
        {
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
        public CanvasID GetCanvasID()
        {
            return currentCanvasID;
        }
    }
}
