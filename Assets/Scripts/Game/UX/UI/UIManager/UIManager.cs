using System;
using Game.Board.General;
using Game.Common;
using UnityEngine;

namespace Game.UX.UI.UIManager
{
    public enum CanvasID
    {
        Ingame
    }
    public class UIManager : Singleton<UIManager>
    {
        private RectTransform currentCanvas;
        
        [Serializable]
        public class CanvasDict : UDictionary<CanvasID, GameObject> {}

        [SerializeField] private CanvasDict canvasDict;
        
        public void Load(CanvasID id)
        {
            if (currentCanvas)
            {
                Destroy(currentCanvas);
                currentCanvas = null;
            }

            currentCanvas = Instantiate(canvasDict[id], transform).GetComponent<RectTransform>();
            currentCanvas.name = canvasDict[id].name;

            switch (id)
            {
                case CanvasID.Ingame:
                    MatchManager.Ins.InputProcessor = currentCanvas.gameObject.GetComponent<BoardViewer>();
                    break;
                default:
                    break;
            }
        }
        
    }
}
