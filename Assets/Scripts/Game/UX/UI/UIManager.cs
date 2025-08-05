using System;
using System.Collections.Generic;
using Game.Board.General;
using Game.Common;
using Game.UX.UI.Ingame;
using Game.UX.UI.Menus;
using UnityEngine;

namespace Game.UX.UI
{
    public enum CanvasID
    {
        MainMenu, PlayMenu, Settings, Ingame, Loading
    }
    public class UIManager : Singleton<UIManager>
    {
        private RectTransform currentCanvas;
        
        [Serializable]
        public class CanvasDict : UDictionary<CanvasID, Canvas> {}

        [SerializeField] private CanvasDict canvasDict;
        private readonly Dictionary<CanvasID, GameObject> loadedCanvases = new();

        public void Load(CanvasID id)
        {
            if (currentCanvas)
            {
                currentCanvas.gameObject.SetActive(false);
                currentCanvas = null;
            }

            if (!loadedCanvases.TryGetValue(id, out var canvasToLoad))
            {
                canvasToLoad = Instantiate(canvasDict[id].gameObject, transform);
                loadedCanvases.Add(id, canvasToLoad);
            }
            else canvasToLoad.gameObject.SetActive(true);

            currentCanvas = canvasToLoad.GetComponent<RectTransform>();
            currentCanvas.name = canvasToLoad.name;

            switch (id)
            {
                case CanvasID.Ingame:
                    MatchManager.Ins.InputProcessor = currentCanvas.gameObject.GetComponent<BoardViewer>();
                    break;
                case CanvasID.PlayMenu:
                    FindAnyObjectByType<PlayPanel>().OnOpen();
                    break;
                case CanvasID.MainMenu:
                    break;
                default:
                    break;
            }
        }
    }
}
